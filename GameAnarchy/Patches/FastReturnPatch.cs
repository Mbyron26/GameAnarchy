namespace GameAnarchy.Patches;
using HarmonyLib;
using System;
using System.Diagnostics;
using System.Reflection;

public class FastReturnPatch {
    public static MethodInfo GetOriginalQuitApplication() => AccessTools.Method(typeof(LoadingManager), nameof(LoadingManager.QuitApplication));
    public static MethodInfo GetQuitApplicationPrefix() => AccessTools.Method(typeof(FastReturnPatch), nameof(FastReturnPatch.QuitApplicationPrefix));

    public static bool QuitApplicationPrefix() {
        try {
            LoadingManager.instance.autoSaveTimer.Stop();
            Process.GetCurrentProcess().Kill();
            return false;
        } catch (Exception e) {
            InternalLogger.Error($"Fast return patch failed.", e);
        }
        return false;
    }
}