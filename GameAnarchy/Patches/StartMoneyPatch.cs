namespace GameAnarchy.Patches;
using ColossalFramework;
using HarmonyLib;
using System.Reflection;

public static class UpdateDataStartMoneyPatch {
    public static MethodInfo GetOriginalUpdateData() => AccessTools.Method(typeof(EconomyManager), nameof(EconomyManager.UpdateData));
    public static MethodInfo GetUpdateDataPrefix() => AccessTools.Method(typeof(UpdateDataStartMoneyPatch), nameof(UpdateDataStartMoneyPatch.UpdateDataPrefix));
    public static void UpdateDataPrefix(EconomyManager __instance) {
        if (!Config.Instance.EnabledInitialCash) return;
        var updateMode = Singleton<SimulationManager>.instance.m_metaData.m_updateMode;
        if (updateMode == SimulationManager.UpdateMode.NewMap || updateMode == SimulationManager.UpdateMode.NewGameFromMap || updateMode == SimulationManager.UpdateMode.NewScenarioFromMap || updateMode == SimulationManager.UpdateMode.UpdateScenarioFromMap || updateMode == SimulationManager.UpdateMode.NewAsset || updateMode == SimulationManager.UpdateMode.NewGameFromScenario) {
            __instance.StartMoney = Config.Instance.InitialCash * 100;
        }
    }
}

#if DEPRECATED
[HarmonyPatch(typeof(EconomyManager.Data), nameof(EconomyManager.Data.Deserialize))]
public static class DeserializeDataStartMoneyPatch {
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        MethodInfo readLong64 = AccessTools.Method(typeof(DataSerializer), nameof(DataSerializer.ReadLong64));
        MethodInfo startMoneyReadLong64 = AccessTools.Method(typeof(DeserializeDataStartMoneyPatch), nameof(DeserializeDataStartMoneyPatch.StartMoneyReadLong64));
        FieldInfo cashAmount = AccessTools.Field(typeof(EconomyManager), "m_cashAmount");
        bool flag = false;
        IEnumerator<CodeInstruction> instructionsEnumerator = instructions.GetEnumerator();
        while (instructionsEnumerator.MoveNext()) {
            CodeInstruction instruction = instructionsEnumerator.Current;
            if (!flag && instruction.Calls(readLong64)) {
                instructionsEnumerator.MoveNext();
                var next0 = instructionsEnumerator.Current;
                if (next0.Is(OpCodes.Stfld, cashAmount)) {
                    yield return instruction;
                    yield return next0;
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, startMoneyReadLong64);
                    yield return new CodeInstruction(OpCodes.Stfld, cashAmount); ;
                    InternalLogger.LogPatch(PatchType.Transpiler, readLong64.Name, startMoneyReadLong64.Name);
                    flag = true;
                } else {
                    yield return instruction;
                    yield return next0;
                }
            } else {
                yield return instruction;
            }
        }
    }
    public static long StartMoneyReadLong64(DataSerializer dataSerializer) => Config.Instance.EnabledInitialCash ? (Config.Instance.InitialCash * 100) : dataSerializer.ReadLong64();
}
#endif




