namespace MbyronModsCommon;
using CitiesHarmony.API;
using HarmonyLib;
using System;
using System.Reflection;

public abstract class ModPatcherBase<TypeMod, TypeConfig> : ModBase<TypeMod, TypeConfig> where TypeMod : ModBase<TypeMod, TypeConfig> where TypeConfig : ModConfig<TypeConfig>, new() {
    public virtual string HarmonyID => $"Mbyron26.{RawName}";
    public Harmony Harmony => new(HarmonyID);
    public bool IsPatched { get; private set; }

    public override void OnEnabled() {
        PatchAll();
        base.OnEnabled();
    }
    public override void OnDisabled() {
        UnpatchAll();
        base.OnDisabled();
    }
    protected virtual void PatchAll() {
        if (IsPatched) return;
        if (HarmonyHelper.IsHarmonyInstalled) {
            InternalLogger.Log("Starting Harmony patches.");
            Harmony.PatchAll();
            PatchAction();
            IsPatched = true;
            InternalLogger.Log("Harmony patches completed.");
        } else {
            InternalLogger.Error("Harmony is not installed correctly.");
        }
    }
    protected virtual void UnpatchAll() {
        if (!IsPatched || !HarmonyHelper.IsHarmonyInstalled) return;
        InternalLogger.Log("Reverting Harmony patches.");
        Harmony.UnpatchAll(HarmonyID);
        IsPatched = false;
    }
    protected virtual void PatchAction() { }
    protected void AddPrefix(Type originalType, string originalMethod, Type patchType, string patchMethod, Type[] targetParm = null) => PatchMethod(PatcherType.Prefix, originalType, originalMethod, patchType, patchMethod, targetParm);
    protected void AddPrefix(MethodBase originalMethodInfo, MethodInfo patchMethodInfo) => PatchMethod(PatcherType.Prefix, originalMethodInfo, patchMethodInfo);
    protected void AddPostfix(Type originalType, string originalMethod, Type patchType, string patchMethod, Type[] targetParm = null) => PatchMethod(PatcherType.Postfix, originalType, originalMethod, patchType, patchMethod, targetParm);
    protected void AddPostfix(MethodBase originalMethodInfo, MethodInfo patchMethodInfo) => PatchMethod(PatcherType.Postfix, originalMethodInfo, patchMethodInfo);
    protected void AddTranspiler(Type originalType, string originalMethod, Type patchType, string patchMethod, Type[] targetParm = null) => PatchMethod(PatcherType.Transpiler, originalType, originalMethod, patchType, patchMethod, targetParm);
    protected void AddTranspiler(MethodBase originalMethodInfo, MethodInfo patchMethodInfo) => PatchMethod(PatcherType.Transpiler, originalMethodInfo, patchMethodInfo);
    private void PatchMethod(PatcherType patcherType, MethodBase originalMethodInfo, MethodInfo patchMethodInfo) {
        if (originalMethodInfo is null) {
            InternalLogger.Error($"Original method not found");
            return;
        }            
        if (patchMethodInfo is null) {
            InternalLogger.Error($"Patch method not found");
            return;
        }          
        switch (patcherType) {
            case PatcherType.Prefix: Harmony.Patch(originalMethodInfo, prefix: new HarmonyMethod(patchMethodInfo)); break;
            case PatcherType.Postfix: Harmony.Patch(originalMethodInfo, postfix: new HarmonyMethod(patchMethodInfo)); break;
            case PatcherType.Transpiler: Harmony.Patch(originalMethodInfo, transpiler: new HarmonyMethod(patchMethodInfo)); break;
        };
        InternalLogger.LogPatch(patcherType, originalMethodInfo, originalMethodInfo.Name, patchMethodInfo, patchMethodInfo.Name);
    }
    private void PatchMethod(PatcherType patcherType, Type originalType, string originalMethod, Type patchType, string patchMethod, Type[] targetParm = null) {
        var original = AccessTools.Method(originalType, originalMethod, targetParm);
        var patch = AccessTools.Method(patchType, patchMethod);
        if (original is null) {
            InternalLogger.Error($"Original method [{originalMethod}] not found");
            return;
        }
        if (patch is null) {
            InternalLogger.Error($"Patch method [{patchMethod}] not found");
            return;
        }
        switch (patcherType) {
            case PatcherType.Prefix: Harmony.Patch(original, prefix: new HarmonyMethod(patch)); break;
            case PatcherType.Postfix: Harmony.Patch(original, postfix: new HarmonyMethod(patch)); break;
            case PatcherType.Transpiler: Harmony.Patch(original, transpiler: new HarmonyMethod(patch)); break;
        };
        InternalLogger.LogPatch(patcherType, original, originalMethod,patch, patchMethod);
    }
}

public enum PatcherType {
    Prefix,
    Postfix,
    Transpiler
}