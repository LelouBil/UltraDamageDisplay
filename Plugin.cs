using BepInEx;
using BepInEx.Logging;

namespace UltraDamageDisplay;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger;
    public void Awake()
    {
        Logger = base.Logger;
        // Plugin startup logic
        Logger.LogInfo($"Patching with Harmony...");
        HarmonyLib.Harmony.CreateAndPatchAll(typeof(Hook));
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}