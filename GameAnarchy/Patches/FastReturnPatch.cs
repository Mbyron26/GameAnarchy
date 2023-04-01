using HarmonyLib;
using System;
using System.Diagnostics;

namespace GameAnarchy {
    [HarmonyPatch(typeof(LoadingManager), nameof(LoadingManager.QuitApplication))]
    public class FastReturnPatch {
        public static bool Prefix() {
            try {
                LoadingManager.instance.autoSaveTimer.Stop();
                Process.GetCurrentProcess().Kill();
                return false;
            }
            catch (Exception e) {
                InternalLogger.Error($"Fast return patch failed.", e);
            }
            return false;
        }
    }
}
