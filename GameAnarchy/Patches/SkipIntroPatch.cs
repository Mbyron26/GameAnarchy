using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using GameAnarchy.ModSettings;
using HarmonyLib;
using UnityEngine;

namespace GameAnarchy.Patches;

public static class SkipIntroPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) {
        if (!Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().SkipIntroEnabled)
            return;
        var original = typeof(LoadingManager).GetNestedTypes(BindingFlags.NonPublic).Single(x => x.FullName == "LoadingManager+<LoadIntroCoroutine>c__Iterator0").GetMethod("MoveNext");
        var patch = AccessTools.Method(typeof(SkipIntroPatch), nameof(LoadIntroCoroutineTranspiler));
        harmonyPatcher.ApplyTranspiler(original, patch);
    }

    public static IEnumerable<CodeInstruction> LoadIntroCoroutineTranspiler(IEnumerable<CodeInstruction> codeInstructions) {
        var instructionsEnumerator = codeInstructions.GetEnumerator();
        while (instructionsEnumerator.MoveNext()) {
            var instruction = instructionsEnumerator.Current;
            if (instruction.opcode == OpCodes.Ldstr && (instruction.operand as string == "IntroScreen" || instruction.operand as string == "IntroScreen2"))
                instruction = new CodeInstruction(OpCodes.Ldstr, string.Empty);
            else if (instruction.opcode == OpCodes.Ldc_R4 && (Mathf.Approximately(instruction.operand as float? ?? 0f, 4f) || Mathf.Approximately(instruction.operand as float? ?? 0f, 1f) || Mathf.Approximately(instruction.operand as float? ?? 0f, 20f))) instruction = new CodeInstruction(OpCodes.Ldc_R4, 0f);
            yield return instruction;
        }
    }
}