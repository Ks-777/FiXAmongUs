using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Epic.OnlineServices.Mods;
using HarmonyLib;

namespace FiXAmongUs;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    public const string PluginGuid = "si.f5.ksn";
    public const string PluginName = "FiXAmongUs";
    public const string PluginNameShort = "FXAU";
    public const string PluginVersion = $"{MyPluginInfo.PLUGIN_VERSION}";
    public const string PluginAuthor = "KS";
    public const string ColorFullName = $"<color=#04f0f0>{PluginName}</color> <color=#AFDFE4>v{PluginVersion}</color> <color=#949593>By {PluginAuthor}</color>";
    public const string ColorShortName = $"<color=#04f0f0>{PluginNameShort}</color> <color=#AFDFE4>v{PluginVersion}</color>";
    public const string ColorShortNotVersion = $"<color=#04f0f0>{PluginNameShort}</color>";
    //Settings(Options)
    internal static new ManualLogSource Log;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"{MyPluginInfo.PLUGIN_GUID} CONPLETE LOADING!!");

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }

}