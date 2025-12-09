using System;
using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using CSLModsCommon.Logging;
using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using GameAnarchy.Localization;
using GameAnarchy.Managers;
using GameAnarchy.ModSettings;

namespace GameAnarchy.Patches;

public class AchievementsPatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) => harmonyPatcher.ApplyPostfix<LoadPanel, AchievementsPatch>("OnListingSelectionChanged", nameof(OnListingSelectionChangedPostfix));

    public static void OnListingSelectionChangedPostfix(UIComponent comp) {
        try {
            if (!Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>().AchievementSystemEnabled) return;
            var achLabel = comp.parent.Find("AchAvLabel");
            var achSprite = comp.parent.Find<UISprite>("AchNope");
            var spriteTooltip = comp.parent.Find("Ach");
            var tooltip = $"{Domain.DefaultDomain.GetManager<ModManager>().ModName} {Translations.AchievementForciblyEnabled}{Environment.NewLine}";
            tooltip += "<color #50869a>";
            if (Singleton<PluginManager>.instance.enabledModCountNoOverride > 0) {
                tooltip += Environment.NewLine;
                tooltip += Translations.ModsActive(Singleton<PluginManager>.instance.enabledModCount);
            }

            tooltip += "</color>";
            spriteTooltip.tooltip = achLabel.tooltip = tooltip;
            achSprite.isVisible = false;
        }
        catch (Exception e) {
            LogManager.GetLogger().Error(e, "Achievements patched failed");
        }
    }
}