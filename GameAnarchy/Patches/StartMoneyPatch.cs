using ColossalFramework;
using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using GameAnarchy.ModSettings;

namespace GameAnarchy.Patches;

public class UpdateDataStartMoneyPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.ApplyPrefix<EconomyManager, UpdateDataStartMoneyPatch>(nameof(EconomyManager.UpdateData), nameof(UpdateDataPrefix));

    public static void UpdateDataPrefix(EconomyManager __instance) {
        if (!Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().EnableStartMoney) return;
        var updateMode = Singleton<SimulationManager>.instance.m_metaData.m_updateMode;
        if (updateMode is SimulationManager.UpdateMode.NewMap or SimulationManager.UpdateMode.NewGameFromMap or SimulationManager.UpdateMode.NewScenarioFromMap or SimulationManager.UpdateMode.UpdateScenarioFromMap or SimulationManager.UpdateMode.NewAsset or SimulationManager.UpdateMode.NewGameFromScenario) __instance.StartMoney = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().StartMoneyAmount * 100;
    }
}