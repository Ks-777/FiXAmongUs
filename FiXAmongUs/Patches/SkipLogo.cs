using HarmonyLib;
using Rewired.Utils.Platforms.Windows;

namespace TownOfPlus
{
    [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
    class SkipLogo
    {
        public static void Postfix(SplashManager __instance)
        {
            __instance.sceneChanger.AllowFinishLoadingScene();
            __instance.startedSceneLoad = true;
        }
    }
}