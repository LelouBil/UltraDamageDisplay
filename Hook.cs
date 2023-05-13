using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace UltraDamageDisplay;


[HarmonyPatch]
public class Hook
{
    
    static IEnumerable<MethodBase> TargetMethods()
    {
        //todo SpiderBody & Drone différent
        return 
            new [] {typeof(Zombie),typeof(Machine),typeof(Statue)}
                .Select(enemy => AccessTools.Method(enemy, "GetHurt"));
    }

    private struct DamageData
    {
        public DamageData(object entity, float beforeHealth, float multiplier, float critMultiplier, string enemyName)
        {
            BeforeHealth = beforeHealth;
            Multiplier = multiplier;
            CritMultiplier = critMultiplier;
            EnemyName = enemyName;
            Entity = entity;
        }
        public object Entity { get; }
        public float BeforeHealth { get; }
        public float Multiplier { get; }
        public float CritMultiplier { get; }
        public string EnemyName { get; }
    }

    private static readonly Stack<DamageData> HealthStackCount = new();

    // [HarmonyPatch(typeof(Zombie), nameof(Zombie.GetHurt))]
    [HarmonyPrefix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    static void HurtPrefix(object __instance, float multiplier, float critMultiplier)
    {
        float? health = (float?)__instance.GetType()
            .GetField("health", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?
            .GetValue(__instance);
        EnemyIdentifier enemyIdentifier = (EnemyIdentifier)__instance.GetType()
            .GetField("eid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(__instance);
        if(enemyIdentifier == null)
        {
            Plugin.Logger.LogError("Could not get enemy identifier for entity!");
            return;
        }
        EnemyType enemyType = enemyIdentifier.enemyType;
        if (HealthStackCount.Count > 0)
        {
            Plugin.Logger.LogError($"health stack is not empty inside prefix for enemy {enemyType}!");
            return;
        }

        if (health == null)
        {
            Plugin.Logger.LogError($"Could not get health for enemy {enemyType}!");
            return;
        }
        HealthStackCount.Push(new DamageData(__instance,health.Value, multiplier, critMultiplier, enemyType.ToString()));
    }
    
    // [HarmonyPatch(typeof(Zombie), nameof(Zombie.GetHurt))]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    static void HurtPostfix(Zombie __instance)
    {
        if (HealthStackCount.Count == 0)
        {
            Plugin.Logger.LogError("Husk health stack is empty inside postfix!");
            return;
        }
        var dmgData = HealthStackCount.Pop();
        var damage = dmgData.BeforeHealth - __instance.health;
        DamageDisplay.DisplayDamage(dmgData.EnemyName, dmgData.Multiplier, dmgData.CritMultiplier, damage);
    }
}