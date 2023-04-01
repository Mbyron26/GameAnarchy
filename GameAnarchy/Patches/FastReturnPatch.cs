using HarmonyLib;
using MbyronModsCommon;
using System;
using System.Diagnostics;

namespace GameAnarchy {
    [HarmonyPatch(typeof(LoadingManager), "QuitApplication")]
    public class FastReturnPatch {
        static bool Prefix() {
            return FastReturn.Terminate();
        }
    }
    public class FastReturn {
        public static bool Terminate() {
            try {
                LoadingManager.instance.autoSaveTimer.Stop();
                Process.GetCurrentProcess().Kill();
                return false;
            }
            catch (Exception e) {
                InternalLogger.Error($"Fast return patch failure.", e);
            }
            return false;
        }
    }
}
