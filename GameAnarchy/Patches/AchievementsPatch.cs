using ColossalFramework.UI;
using ColossalFramework;
using System;
using ColossalFramework.Plugins;
using HarmonyLib;
using ColossalFramework.Globalization;
using CSShared.Patch;
using CSShared.Debug;

namespace GameAnarchy.Patches;

public static class AchievementsPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(LoadPanel), "OnListingSelectionChanged"), AccessTools.Method(typeof(AchievementsPatch), nameof(OnListingSelectionChangedPostfix)));
    }

    public static void OnListingSelectionChangedPostfix(UIComponent comp) {
        try {
            if (Config.Instance.EnabledAchievements) {
                var achLabel = comp.parent.Find("AchAvLabel");
                var achSprite = comp.parent.Find<UISprite>("AchNope");
                var spriteTooltip = comp.parent.Find("Ach");
                var tooltip = Locale.Get("LOADPANEL_ACHSTATUS_ENABLED");
                tooltip += "<color #50869a>";
                if (Singleton<PluginManager>.instance.enabledModCountNoOverride > 0) {
                    tooltip += Environment.NewLine;
                    tooltip += LocaleFormatter.FormatGeneric("LOADPANEL_ACHSTATUS_MODSACTIVE", Singleton<PluginManager>.instance.enabledModCount);
                }
                tooltip += "</color>";
                spriteTooltip.tooltip = achLabel.tooltip = tooltip;
                achSprite.isVisible = false;
            }
        }
        catch (Exception e) {
            LogManager.GetLogger().Error(e, $"Achievements patched failed");
        }
    }
}