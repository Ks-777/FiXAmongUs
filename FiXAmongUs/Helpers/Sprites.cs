using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.Logging;
using IntPtr = System.IntPtr;
using Object = UnityEngine.Object;

namespace FiXAmongUs.Sprites;

public static class Sprites
{
    public static Dictionary<string, Sprite> CachedSprites = new();

    public static Dictionary<string, Texture2D> CachedTexture = new();

    public static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("FiXAmongUs");

    public static Sprite GetSprite(string path, float pixelsPerUnit = 115f)
    {
        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;

            Texture2D val = LoadTextureFromResources(path);
            if (val == null) return null;
            
            sprite = Sprite.Create(val, new Rect(0f, 0f, val.width, val.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedSprites[path + pixelsPerUnit] = sprite;
        }
        catch (System.Exception ex)
        {
            Logger.LogWarning($"Error can't load sprite path: {path}\nError: {ex}");
        }
        return null;
    }

    public static Sprite GetSpriteFromResources(string path, float pixelsPerUnit = 115f)
    {
        try
        {
            Logger.LogInfo($"Attempting to load sprite: {path}");
            var resourcePath = Path.Combine("Resources", $"FiXAmongUs.png");
            var sprite = GetSprite(resourcePath, pixelsPerUnit);
            if (sprite == null)
                Logger.LogError($"Failed to load sprite: {resourcePath}");
            else
                Logger.LogInfo($"Successfully loaded sprite: {resourcePath}");
            return sprite;
        }
        catch (System.Exception ex)
        {
            Logger.LogError($"Error loading sprite {path}: {ex}");
            return null;
        }
    }

    public static Texture2D LoadTextureFromResources(string path)
    {
        try
        {
            if (CachedTexture.TryGetValue(path, out var texture2D)) return texture2D;
            
            // 埋め込みリソースの代わりにファイルシステムから読み込む
            return CachedTexture[path] = LoadTextureFromDisk(path);
        }
        catch (System.Exception ex)
        {
            Logger.LogWarning($"Error loading texture from resources: {path} - {ex.Message}");
        }
        return null;
    }

    public static Texture2D LoadTextureFromDisk(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path);
                LoadImage(texture, byteTexture, false); 
                return texture;
            }
        }
        catch
        {
            Logger.LogError($"Error loading texture from disk: {path}");
        }
        return null;
    }
    internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    internal static d_LoadImage iCall_LoadImage;

    private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
    {
        iCall_LoadImage ??= IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
        var il2cppArray = (Il2CppStructArray<byte>)data;
        return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
    }

    public static GameObject Render(string noResourcePath, string name, float pixelsPerUnit = 115f, int layer = 5, int sortingLayerID = 0, int sortingOrder = 5, GameObject parent = null, bool active = true)
    {
        GameObject val = new GameObject
        {
            layer = layer,
            active = active,
            name = name
        };
        if (parent != null)
        {
            val.transform.SetParent(parent.transform);
        }
        val.transform.localPosition = new Vector3(0f, 0f, -38f);
        SpriteRenderer val2 = val.AddComponent<SpriteRenderer>();
        val2.sprite = GetSprite("FiXAmongUs.Resources." + noResourcePath, pixelsPerUnit);
        val2.sortingLayerID = sortingLayerID;
        val2.sortingOrder = sortingOrder;
        return val;
    }

    public static GameObject GobjRender(GameObject @object, string noResourcePath, string name, float scale, float size = 1f, int layer = 5, int sortingLayerID = 50, int sortingOrder = 5, GameObject parent = null, bool active = true)
    {
        Sprite sprite = GetSprite("FiXAmongUs.Resources." + noResourcePath, scale);
        Logger.LogInfo($"FiXAmongUs.Resources.{noResourcePath} : path {sprite?.texture?.ToString() ?? "null"}");

        GameObject val = Object.Instantiate<GameObject>(@object);
        (val).name = name;
        if (val == null)
        {
            return null;
        }
        if (parent != null)
        {
            val.transform.SetParent(parent.transform);
        }
        val.transform.position = new Vector3(0f, 0f, 0f);
        SpriteRenderer component = val.GetComponent<SpriteRenderer>();
        component.sprite = sprite;
        component.sortingLayerID = sortingLayerID;
        component.sortingOrder = sortingOrder;
        return val;
    }
}