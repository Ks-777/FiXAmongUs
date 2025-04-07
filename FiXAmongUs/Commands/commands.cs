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

        // �`���b�g���M���L�����Z��
        __instance.freeChatField.textArea.Clear();

        try
        {
            // �R�}���h�̏���
            if (text.Equals("/help"))
            {
                string helpText = "=== Commands ===\n" +
                                "/help : �����\�����܂�\n";

                // �z�X�g�̏ꍇ�͒ǉ��R�}���h��\��
                if (AmongUsClient.Instance.AmHost)
                {
                    helpText += "\n=== �z�X�g��p�R�}���h ===\n" +
                              "���ݒǉ��̃R�}���h�͂���܂���\n";
                }

                // �`���b�g�ɕ\��
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(
                    PlayerControl.LocalPlayer,
                    helpText
                );
                return false;
            }

            // ���m�̃R�}���h
            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(
                PlayerControl.LocalPlayer,
                "���m�̃R�}���h�ł��B/help �ŃR�}���h�ꗗ���m�F�ł��܂��B"
            );
        }
        catch (Exception e)
        {
            FiXAmongUs.Plugin.Log.LogError($"�R�}���h���s���ɃG���[���������܂���: {e}");
        }

        return false;
    }
}