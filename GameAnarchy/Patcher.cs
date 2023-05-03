using HarmonyLib;

namespace GameAnarchy {
    public static class Patcher {
        private const string HARMONYID = @"com.mbyron26.GameAanrchy";
        public static void EnablePatches() {
            Harmony harmony = new(HARMONYID);
            harmony.PatchAll();
            Patches.SkipIntroPatch.Patch(harmony);
        }
        public static void DisablePatches() {
            Harmony harmony = new(HARMONYID);
            harmony.UnpatchAll(HARMONYID);
        }
       
    }
}
