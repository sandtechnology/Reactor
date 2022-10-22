using HarmonyLib;
using InnerNet;
using UnityEngine;

namespace Reactor.Patches
{
    internal static class BetterGuestPatch
    {
        [HarmonyPatch(typeof(AmongUs.Data.Settings.MultiplayerSettingsData), nameof(AmongUs.Data.Settings.MultiplayerSettingsData.ChatMode), MethodType.Getter)]
        public static class ChatModeTypePatch
        {
            public static bool Prefix(out QuickChatModes __result)
            {
                __result = QuickChatModes.FreeChatOrQuickChat;
                return false;
            }
        }
    }
}
