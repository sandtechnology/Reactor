using System.Reflection;
using HarmonyLib;
using static EOSManager;

namespace Reactor.Patches
{
    [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Start))]
    internal static class SplashSkip
    {
        private static void Prefix(SplashManager __instance)
        {
            __instance.minimumSecondsBeforeSceneChange = 0;
        }
    }

    [HarmonyPatch(typeof(SplashManager), nameof(SplashManager.Update))]
    internal static class WaitForEpicAuth
    {
        private static readonly PropertyInfo? _localUserIdProperty = typeof(EpicManager).GetProperty("localUserId", BindingFlags.Static | BindingFlags.Public);
        private static bool _loginFinished;

        private static bool Prefix(SplashManager __instance)
        {
            if (__instance.startedSceneLoad) return true;

            if (_localUserIdProperty != null && !_loginFinished)
            {
                return false;
            }

            return true;
        }

        // EpicManager calls SaveManager.LoadPlayerPrefs(true) both on successful and unsuccessful EOS login
        [HarmonyPatch(typeof(AmongUs.Data.Player.PlayerAccountData), nameof(AmongUs.Data.Player.PlayerAccountData.LoginStatus), MethodType.Setter)]
        private static class LoadPlayerPrefsPatch
        {
            private static void Prefix(AccountLoginStatus value)
            {
                if (value == AccountLoginStatus.LoggedIn) _loginFinished = true;

            }
        }
    }
}
