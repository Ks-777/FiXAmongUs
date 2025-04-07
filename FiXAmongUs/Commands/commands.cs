using System;
using HarmonyLib;
using AmongUs.Data;
using BepInEx.Logging;
using UnityEngine;

namespace FiXAmongUs;

[HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
class ChatCommandPatch
{
    public static bool Prefix(ChatController __instance)
    {
        string text = __instance.freeChatField.textArea.text;
        if (!text.StartsWith("/")) return true;

        // チャット送信をキャンセル
        __instance.freeChatField.textArea.Clear();

        try
        {
            // コマンドの処理
            if (text.Equals("/help"))
            {
                string helpText = "=== Commands ===\n" +
                                "/help : これを表示します\n";

                // ホストの場合は追加コマンドを表示
                if (AmongUsClient.Instance.AmHost)
                {
                    helpText += "\n=== ホスト専用コマンド ===\n" +
                              "現在追加のコマンドはありません\n";
                }

                // チャットに表示
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(
                    PlayerControl.LocalPlayer,
                    helpText
                );
                return false;
            }

            // 未知のコマンド
            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(
                PlayerControl.LocalPlayer,
                "未知のコマンドです。/help でコマンド一覧を確認できます。"
            );
        }
        catch (Exception e)
        {
            FiXAmongUs.Plugin.Log.LogError($"コマンド実行中にエラーが発生しました: {e}");
        }

        return false;
    }
}