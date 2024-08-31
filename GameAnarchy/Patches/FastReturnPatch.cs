using CSShared.Debug;
using CSShared.Patch;
using HarmonyLib;
using System;
using System.Diagnostics;

namespace GameAnarchy.Patches;

public class FastReturnPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(LoadingManager), nameof(LoadingManager.QuitApplication)), AccessTools.Method(typeof(FastReturnPatch), nameof(QuitApplicationPrefix)));

    public static bool QuitApplicationPrefix() {
        try {
            LoadingManager.instance.autoSaveTimer.Stop();
            Process.GetCurrentProcess().Kill();
            return false;
        }
        catch (Exception e) {
            LogManager.GetLogger().Error(e, $"Fast return patch failed");
        }
        return false;
    }
}