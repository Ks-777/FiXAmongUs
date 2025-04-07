using HarmonyLib;
using TMPro;
using UnityEngine;
using FiXAmongUs.Sprites;
using BepInEx.Logging;
using AmongUs.Data;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using FiXAmongUs;

namespace FiXAmongUs
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class StartMenu
    {
        public static void Postfix(VersionShower __instance)
        {
            // HexColorから Color32 に変換
            var customColor = new Color32(
                r: 0x18,  // 24
                g: 0x06,  // 6
                b: 0x14,  // 20
                a: 0xFF   // 255 (完全不透明)
            );

            TextMeshPro AddtionalText = new GameObject("text").AddComponent<TextMeshPro>();
            AddtionalText.text = $"{Plugin.ColorFullName}"; 
            AddtionalText.fontSize = 2;
            AddtionalText.alignment = TextAlignmentOptions.Right;
            AddtionalText.enableWordWrapping = false;
            AddtionalText.transform.SetParent(__instance.transform);
            AddtionalText.transform.localPosition = new Vector3(0.3f, 0, 0);
            AddtionalText.transform.localScale = Vector3.one;
            AddtionalText.color = customColor;
        }
    }
}

namespace NetherTownRoles.Patches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStart
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("FiXAmongUs");

        public static void Postfix(HudManager __instance)
        {
            Logger.LogInfo("HudManager started");

            var customColor = new Color32(0x18, 0x06, 0x14, 0xFF);

            TextMeshPro FXAUText = new GameObject("FXAU").AddComponent<TextMeshPro>();
            FXAUText.text = $"{Plugin.ColorShortName}"; 
            FXAUText.fontSize = 2;
            FXAUText.alignment = TextAlignmentOptions.Midline;
            FXAUText.enableWordWrapping = false;
            FXAUText.transform.SetParent(__instance.transform);
            FXAUText.transform.position = new Vector3(1.6f, 2.6f, 0);
            FXAUText.transform.localScale = Vector3.one;
            FXAUText.gameObject.layer = 5;
            FXAUText.enabled = true;
            FXAUText.gameObject.active = true;
            FXAUText.material = DestroyableSingleton<PingTracker>.Instance.text.material;
            FXAUText.font = DestroyableSingleton<PingTracker>.Instance.text.font;
            FXAUText.color = customColor;
            FXAUText.outlineColor = Color.black;
            FXAUText.outlineWidth = 0.1f;
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class MainMenuStartPatch
    {
        private static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("NetherTownRoles");
        private static GameObject logoObject;

        private static Sprite LoadSprite()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("NetherTownRoles.Resources.NTRLogo.png");
                if (stream == null)
                {
                    Logger.LogError("Resource stream is null");
                    return null;
                }

                var texture = new Texture2D(2, 2);
                var imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);
                
                if (!ImageConversion.LoadImage(texture, imageData))
                {
                    Logger.LogError("Failed to load image data");
                    return null;
                }

                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error loading sprite: {ex.Message}");
                return null;
            }
        }

        public static void Postfix(MainMenuManager __instance)
        {
            try
            {
                if (logoObject != null)
                {
                    UnityEngine.Object.Destroy(logoObject);
                    logoObject = null;
                }

                // メインメニューを検索
                var menuRoot = GameObject.Find("MainUI")?.transform;
                if (menuRoot == null)
                {
                    Logger.LogError("Could not find MainUI");
                    return;
                }

                // ロゴ画像を読み込み
                var sprite = LoadSprite();
                if (sprite == null)
                {
                    Logger.LogError("Failed to load logo sprite");
                    return;
                }

                // ロゴオブジェクトを作成
                logoObject = new GameObject("FXAULogo");
                logoObject.transform.SetParent(menuRoot, false);
                
                var renderer = logoObject.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                
                // 位置とスケールを設定
                logoObject.transform.localPosition = new Vector3(2f, 0f, 0);
                logoObject.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
                renderer.sortingOrder = 11;

                Logger.LogInfo($"Logo created at position: {logoObject.transform.localPosition}");
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error in MainMenuStartPatch: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}