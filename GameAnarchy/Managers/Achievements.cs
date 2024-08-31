using ColossalFramework.UI;
using ColossalFramework;
using ICities;
using System;
using CSShared.Debug;
using CSShared.Common;

namespace GameAnarchy.Managers;

public partial class Manager {
    private UIPanel unlockingPanel;
    private UIButton button;

    public void InitAchievements(LoadMode loadMode) {
        if (!SingletonMod<Mod>.Instance.IsLevelLoaded) {
            return;
        }
        if (loadMode == LoadMode.NewGame || loadMode == LoadMode.LoadGame || loadMode == LoadMode.NewGameFromScenario || loadMode == LoadMode.LoadScenario) {
            unlockingPanel = UIView.Find<UIPanel>("UnlockingPanel");
            if (unlockingPanel is null) {
                LogManager.GetLogger().Info("Init achievements failed, couldn't find UnlockingPanel");
            }
            else {
                LogManager.GetLogger().Info($"{loadMode} mode, init UnlockingPanel");
                var tabstrip = unlockingPanel.Find<UITabstrip>("Tabstrip");
                button = tabstrip.Find<UIButton>("Achievements");
            }
            UpdateAchievements();
        }
        else {
            LogManager.GetLogger().Info("Not Game mode, do not init UnlockingPanel");
            UpdateAchievements();
        }
    }

    public void UpdateAchievements() {
        if (!SingletonMod<Mod>.Instance.IsLevelLoaded) {
            return;
        }
        try {
            var isEnabled = Config.Instance.EnabledAchievements;
            if (isEnabled) {
                Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;
            }
            else {
                Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.True;
            }
            if (button is not null) {
                button.isEnabled = isEnabled;
            }
            LogManager.GetLogger().Info($"Update achievements, status: {isEnabled}");
        }
        catch (Exception e) {
            LogManager.GetLogger().Error(e, $"Update achievements status failed");
        }
    }

    public void DeInitAchievements() {
        button = null;
        unlockingPanel = null;
    }
}
