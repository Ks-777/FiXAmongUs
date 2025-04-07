using HarmonyLib;

namespace FiXAmongUs;

[HarmonyPatch(typeof(ModManager), "LateUpdate")]
public class ShowModStampPatch
{
    public static void Postfix(ModManager __instance)
    {
        __instance.ShowModStamp();
    }
}