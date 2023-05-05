﻿namespace GameAnarchy.Patches;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

public static class SkipIntroPatch {
    public static void Patch(Harmony harmony) {
        if (!Config.Instance.EnabledSkipIntro) return;
        var original = typeof(LoadingManager).GetNestedTypes(BindingFlags.NonPublic).Single(x => x.FullName == "LoadingManager+<LoadIntroCoroutine>c__Iterator0").GetMethod("MoveNext");
        if (original is null) return;
        var patch = AccessTools.Method(typeof(SkipIntroPatch), nameof(SkipIntroPatch.LoadIntroCoroutineTranspiler));
        if (patch is null) return;
        harmony.Patch(original, null, null, new HarmonyMethod(patch));
        InternalLogger.LogPatch(PatcherType.Transpiler, original, original.Name, patch, patch.Name);
    }
    public static IEnumerable<CodeInstruction> LoadIntroCoroutineTranspiler(IEnumerable<CodeInstruction> codeInstructions) {
        var instructionsEnumerator = codeInstructions.GetEnumerator();
        while (instructionsEnumerator.MoveNext()) {
            var instruction = instructionsEnumerator.Current;
            if (instruction.opcode == OpCodes.Ldstr && (instruction.operand as string == "IntroScreen" || instruction.operand as string == "IntroScreen2")) {
                instruction = new CodeInstruction(OpCodes.Ldstr, string.Empty);
            } else if (instruction.opcode == OpCodes.Ldc_R4 && (instruction.operand as float? == 4f || instruction.operand as float? == 1f || instruction.operand as float? == 20f)) {
                instruction = new CodeInstruction(OpCodes.Ldc_R4, 0f);
            }
            yield return instruction;
        }
    }
}

