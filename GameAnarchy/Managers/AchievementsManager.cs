using System;
using ColossalFramework.UI;
using CSLModsCommon;
using CSLModsCommon.Manager;
using GameAnarchy.ModSettings;
using ICities;

namespace GameAnarchy.Managers;

public class AchievementsManager : ManagerBase {
    private ModSetting _modSetting;
    private UIPanel _unlockingPanel;
    private UIButton _button;

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
    }

    protected override void OnGameLoaded(LoadContext context) {
        base.OnGameLoaded(context);
        if (context.LoadMode is LoadMode.NewGame or LoadMode.LoadGame or LoadMode.NewGameFromScenario or LoadMode.LoadScenario) {
            _unlockingPanel = UIView.Find<UIPanel>("UnlockingPanel");
            if (_unlockingPanel is null) {
                Logger.Info("Init achievements failed, couldn't find UnlockingPanel");
            }
            else {
                Logger.Info($"{context.LoadMode} mode, init UnlockingPanel");
                var tabStrip = _unlockingPanel.Find<UITabstrip>("Tabstrip");
                _button = tabStrip.Find<UIButton>("Achievements");
            }
        }
        else {
            Logger.Info("Not game mode, do not init UnlockingPanel");
        }

        UpdateAchievementSystemStatus();
    }

    protected override void OnGameUnloaded() {
        base.OnGameUnloaded();
        _button = null;
        _unlockingPanel = null;
    }

    public void UpdateAchievementSystemStatus() {
        try {
            if (!SimulationManager.exists || SimulationManager.instance.m_metaData is null) return;

            var isEnabled = _modSetting.AchievementSystemEnabled;
            SimulationManager.instance.m_metaData.m_disableAchievements = isEnabled ? SimulationMetaData.MetaBool.False : SimulationMetaData.MetaBool.True;

            if (_button is not null) _button.isEnabled = isEnabled;

            Logger.Info($"Update achievement system status: {(isEnabled ? "enabled" : "disabled")}");
        }
        catch (Exception e) {
            Logger.Error(e, "Update achievements status failed");
        }
    }
}