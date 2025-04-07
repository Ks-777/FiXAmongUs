using HarmonyLib;

namespace NetherTownRoles;

[HarmonyPatch(typeof(ModManager), "LateUpdate")]
public class ShowModStampPatch
{
    public static void Postfix(ModManager __instance)
    {
        __instance.ShowModStamp();
    }
}