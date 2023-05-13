namespace UltraDamageDisplay;

public class DamageDisplay
{
    public static void DisplayDamage(string mobName,float multiplier, float critMultiplier, float finalDamage)
    {
        var calcDmg = multiplier + multiplier * critMultiplier;
        Plugin.Logger.LogInfo($"[Damage] {mobName} took {multiplier} + {multiplier} * {critMultiplier} = {calcDmg}(actual {finalDamage}) damage!");
    }
}