using ColossalFramework.UI;
using ColossalFramework;
using MbyronModsCommon;
using System;
using ColossalFramework.Plugins;
using HarmonyLib;
using ColossalFramework.Globalization;
using ICities;

namespace GameAnarchy {
    public class AchievementsManager {
        private static UIPanel unlockingPanel;
        private static UIButton button;
        private static bool isInGame;
        public static void InitializeAchievements(LoadMode loadMode) {
            isInGame = true;
            if(loadMode == LoadMode.NewGame || loadMode == LoadMode.LoadGame) {
                ModLogger.ModLog("Game mode, start initalize UnlockingPanel.");
                unlockingPanel = UIView.Find<UIPanel>("UnlockingPanel");
                if (unlockingPanel is null) {
                    ModLogger.ModLog("Initialize achievements failed, couldn't find UnlockingPanel.");
                } else {
                    var tabstrip = unlockingPanel.Find<UITabstrip>("Tabstrip");
                    button = tabstrip.Find<UIButton>("Achievements");
                }
                UpdateAchievements(Config.Instance.EnabledAchievements);
            } else {
                ModLogger.ModLog("Not Game mode, do not initalize UnlockingPanel.");
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
                ModLogger.ModLog($"Update achievements succeed, status: {isEnable}");
            }
            catch (Exception e) {
                ModLogger.ModLog($"Update achievements status failure, detail: {e.Message}");
            }
        }

        public static void Destroy() {
            button = null;
            unlockingPanel = null;
            isInGame = false;
        }
    }
    
    [HarmonyPatch(typeof(LoadPanel), "OnListingSelectionChanged")]
    public static class AchievementPatch {
        static void Postfix(UIComponent comp, int sel) {
            try {
                if (Config.Instance.EnabledAchievements) {
                    UISprite m_AchNope = comp.parent.Find<UISprite>("AchNope");
                    UIComponent m_Ach = comp.parent.Find("Ach");
                    UIComponent m_AchAvLabel = comp.parent.Find("AchAvLabel");
                    m_AchNope.isVisible = false;
                    string tooltip = string.Empty;
                    tooltip = Locale.Get("LOADPANEL_ACHSTATUS_ENABLED");
                    tooltip += "<color #50869a>";
                    if (Singleton<PluginManager>.instance.enabledModCountNoOverride > 0) {
                        tooltip += Environment.NewLine;
                        tooltip += LocaleFormatter.FormatGeneric("LOADPANEL_ACHSTATUS_MODSACTIVE", Singleton<PluginManager>.instance.enabledModCount);
                    }
                    tooltip += "</color>";
                    m_Ach.tooltip = tooltip;
                    m_AchAvLabel.tooltip = tooltip;
                }
            }
            catch (Exception e) {
                ModLogger.ModLog($"Achievement patch failure, detail: {e.Message}");
            }
        }
    }

}
