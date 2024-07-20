namespace GameAnarchy.Patches;
using HarmonyLib;
using System;
using System.Diagnostics;
using System.Reflection;

public class FastReturnPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(LoadingManager), nameof(LoadingManager.QuitApplication)), AccessTools.Method(typeof(FastReturnPatch), nameof(QuitApplicationPrefix)));

    public static bool QuitApplicationPrefix() {
        try {
            LoadingManager.instance.autoSaveTimer.Stop();
            Process.GetCurrentProcess().Kill();
            return false;
        }
        catch (Exception e) {
            Mod.Log.Error(e,$"Fast return patch failed");
        }
        return false;
    }
}