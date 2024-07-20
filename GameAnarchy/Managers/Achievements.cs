namespace GameAnarchy;
using ColossalFramework.UI;
using ColossalFramework;
using ICities;
using System;

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
                Mod.Log.Info("Init achievements failed, couldn't find UnlockingPanel");
            }
            else {
                Mod.Log.Info($"{loadMode} mode, init UnlockingPanel");
                var tabstrip = unlockingPanel.Find<UITabstrip>("Tabstrip");
                button = tabstrip.Find<UIButton>("Achievements");
            }
            UpdateAchievements();
        }
        else {
            Mod.Log.Info("Not Game mode, do not init UnlockingPanel");
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
            Mod.Log.Info($"Update achievements, status: {isEnabled}");
        }
        catch (Exception e) {
            Mod.Log.Error(e, $"Update achievements status failed");
        }
    }

    public void DeInitAchievements() {
        button = null;
        unlockingPanel = null;
    }
}
