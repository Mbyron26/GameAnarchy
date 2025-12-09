using System;
using System.Diagnostics;
using CSLModsCommon.Logging;
using CSLModsCommon.Patch;

namespace GameAnarchy.Patches;

public class FastReturnPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.ApplyPrefix<LoadingManager, FastReturnPatch>(nameof(LoadingManager.QuitApplication), nameof(QuitApplicationPrefix));

    public static bool QuitApplicationPrefix() {
        try {
            LoadingManager.instance.autoSaveTimer.Stop();
            Process.GetCurrentProcess().Kill();
            return false;
        }
        catch (Exception e) {
            LogManager.GetLogger().Error(e, "Fast return patch failed");
        }

        return false;
    }
}