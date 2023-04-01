using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace GameAnarchy {
    public static class Patcher {
        private const string HARMONYID = @"com.mbyron26.GameAanrchy";
        public static void EnablePatches() {
            Harmony harmony = new(HARMONYID);
            harmony.PatchAll();
            SkipIntroPatch(harmony);
        }
        public static void DisablePatches() {
            Harmony harmony = new(HARMONYID);
            harmony.UnpatchAll(HARMONYID);
        }
        public static void SkipIntroPatch(Harmony harmony) {
            if (Config.Instance.EnabledSkipIntro) {
                var loadIntroCoroutineOriginal = typeof(LoadingManager).GetNestedTypes(BindingFlags.NonPublic)
                .Single(x => x.FullName == "LoadingManager+<LoadIntroCoroutine>c__Iterator0").GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance);
                var loadIntroCoroutineTranspiler = typeof(Patcher).GetMethod(nameof(LoadIntroCoroutineTranspiler), BindingFlags.NonPublic | BindingFlags.Static);
                harmony.Patch(loadIntroCoroutineOriginal, null, null, new HarmonyMethod(loadIntroCoroutineTranspiler));
            }
        }
        private static IEnumerable<CodeInstruction> LoadIntroCoroutineTranspiler(IEnumerable<CodeInstruction> codeInstructions) {
            var menuSceneField = typeof(LoadingManager).GetNestedTypes(BindingFlags.NonPublic).Single(x => x.FullName == "LoadingManager+<LoadIntroCoroutine>c__Iterator0").GetField("<menuScene>__0", BindingFlags.NonPublic | BindingFlags.Instance);
            var codes = codeInstructions.ToList();
            for (int i = 0; i < codes.Count; i++) {
                var code = codes[i];
                if (code.opcode == OpCodes.Ldstr && (code.operand as string == "IntroScreen" || code.operand as string == "IntroScreen2")) {
                    code.operand = string.Empty;
                }
                if (code.opcode == OpCodes.Ldc_R4 && (code.operand as float? == 4f //wait time of IntroScreen and FadeImage
                        || code.operand as float? == 1f //wait time of IntroScreen2
                        || code.operand as float? == 20f //wait time for IsDLCStateReady and LegalDocumentsReady
                    )) {
                    code.operand = 0f;
                }
                if (code.opcode == OpCodes.Ldarg_0
                    && codes[i + 1].opcode == OpCodes.Ldarg_0
                    && codes[i + 2].opcode == OpCodes.Ldfld && codes[i + 2].operand == menuSceneField
                    && codes[i + 3].opcode == OpCodes.Ldc_I4_1) {
                    codes[i + 3].opcode = OpCodes.Ldc_I4_0;
                }
            }
            return codes;
        }

    }
}
