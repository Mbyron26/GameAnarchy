using ColossalFramework.UI;
using ColossalFramework;
using MbyronModsCommon;
using System;
using UnityEngine;
using ColossalFramework.Plugins;
using HarmonyLib;
using ColossalFramework.Globalization;

namespace GameAnarchy {
    public class AchievementsManager : MonoBehaviour {
        private UnlockingPanel unlockingPanel;
        private UITabstrip tabstrip;
        private UIButton button;
        private void Start() {
            unlockingPanel = GameObject.Find("UnlockingPanel").GetComponent<UnlockingPanel>();
            tabstrip = unlockingPanel.Find("Tabstrip").GetComponent<UITabstrip>();
            button = tabstrip.Find("Achievements").GetComponent<UIButton>();
        }

        public void Update() => ToggleAchievements();

        private void ToggleAchievements() {
            try {
                if (Config.Instance.EnabledAchievements) Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;
                else Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.True;
                if (button != null) {
                    button.isEnabled = Config.Instance.EnabledAchievements;
                }
            }
            catch (Exception e) {
                ModLogger.ModLog($"Toggle achievements status failure, detail: {e.Message}");
            }
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
