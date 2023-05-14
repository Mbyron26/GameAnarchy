﻿namespace GameAnarchy.Manager;
using ColossalFramework.UI;
using ColossalFramework;
using ICities;
using System;

internal class AchievementsManager {
    private static UIPanel unlockingPanel;
    private static UIButton button;
    private static bool isInGame;

    public static void InitializeAchievements(LoadMode loadMode) {
        isInGame = true;
        if (loadMode == LoadMode.NewGame || loadMode == LoadMode.LoadGame || loadMode == LoadMode.NewGameFromScenario || loadMode == LoadMode.LoadScenario) {

            unlockingPanel = UIView.Find<UIPanel>("UnlockingPanel");
            if (unlockingPanel is null) {
                ExternalLogger.Log("Initialize achievements failed, couldn't find UnlockingPanel.");
            } else {
                ExternalLogger.Log($"{loadMode} mode, start initalize UnlockingPanel.");
                var tabstrip = unlockingPanel.Find<UITabstrip>("Tabstrip");
                button = tabstrip.Find<UIButton>("Achievements");
            }
            UpdateAchievements(Config.Instance.EnabledAchievements);
        } else {
            ExternalLogger.Log("Not Game mode, do not initalize UnlockingPanel.");
            UpdateAchievements(Config.Instance.EnabledAchievements);
        }
    }
    public static void UpdateAchievements(bool isEnable) {
        if (!isInGame) {
            return;
        }
        try {
            if (isEnable) {
                Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;
            } else {
                Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.True;
            }
            if (button is not null) {
                button.isEnabled = isEnable;
            }
            ExternalLogger.Log($"Update achievements succeed, status: {isEnable}");
        } catch (Exception e) {
            ExternalLogger.Log($"Update achievements status failure, detail: {e.Message}");
        }
    }

    public static void Destroy() {
        button = null;
        unlockingPanel = null;
        isInGame = false;
    }
}
