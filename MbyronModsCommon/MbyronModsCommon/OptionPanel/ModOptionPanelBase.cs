using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace MbyronModsCommon {
    public class AdvancedBase<Mod, Config> where Mod : IMod where Config : ModConfigBase<Config>, new() {
        public AdvancedBase(UIComponent parent, TypeWidth typeWidth) {
            OptionPanelTool.AddGroup(parent, (float)typeWidth, CommonLocalize.OptionPanel_Advanced);
            OptionPanelTool.AddToggleButton(SingletonMod<Config>.Instance.DebugMode, CommonLocalize.OptionPanel_DebugMode, CommonLocalize.OptionPanel_DebugMinor, _ => SingletonMod<Config>.Instance.DebugMode = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddButton(CommonLocalize.ChangeLog_Major, null, CommonLocalize.ChangeLog, 250, 30, ShowLog, out UILabel _, out UILabel _, out UIButton _);
            OptionPanelTool.AddButton(CommonLocalize.CompatibilityCheck_Major, CommonLocalize.CompatibilityCheck_Minor, CommonLocalize.Check, 250, 30, ShowCompatibility, out UILabel _, out UILabel _, out UIButton _);
            OptionPanelTool.AddButton(CommonLocalize.ResetModMajor, CommonLocalize.ResetModMinor, CommonLocalize.Reset, 250, 30, ResetSettings, out UILabel _, out UILabel _, out UIButton _);
            OptionPanelTool.Reset();
        }

        protected virtual void ResetSettings() { }

        protected void ResetSettings<OptionPanel>() where OptionPanel : UIPanel {
            try {
                ModLogger.GameLog($"Start resetting mod config.");
                SingletonMod<Config>.Instance = null;
                SingletonMod<Config>.Instance = new();
                OptionPanelManager<Mod, OptionPanel>.LocaleChanged();
                ModLogger.GameLog($"Reset mod config succeeded.");
                MessageBox.Show<ResetModMessageBox>().Init<Mod>();
            }
            catch (Exception e) {
                ModLogger.GameLog($"Reset settings failed:", e);
                MessageBox.Show<ResetModMessageBox>().Init<Mod>(false);
            }
        }

        private static void ShowLog() {
            var messageBox = MessageBox.Show<LogMessageBox>();
            messageBox.Initialize<Mod>(false);
        }
        private static void ShowCompatibility() {
            var messageBox = MessageBox.Show<CompatibilityMessageBox>();
            messageBox.Initialize(ModMainInfo<Mod>.ModName);
        }

    }

    public class GeneralOptionsBase<Mod, Config> where Mod : IMod where Config : ModConfigBase<Config> {
        protected UIComponent Parent { get; set; }
        public GeneralOptionsBase(UIComponent parent, TypeWidth typeWidth) {
            Parent = parent;
            CustomLabel.AddLabel(parent, ModMainInfo<Mod>.ModName, (float)typeWidth, new(), 2f, Color.white).font = CustomFont.SemiBold;
        }

        protected static void OnLanguageSelectedIndexChanged<Panel>(int value) where Panel : UIPanel {
            if (value == 0) {
                SingletonMod<Mod>.Instance.ModCulture = new CultureInfo(ModLocalize.LocalizationExtension());
                SingletonMod<Config>.Instance.ModLanguage = "GameLanguage";
            } else {
                SingletonMod<Mod>.Instance.ModCulture = new CultureInfo(ModLocalize.ModLanguageOptionIDList[value]);
                SingletonMod<Config>.Instance.ModLanguage = ModLocalize.ModLanguageOptionIDList[value];
            }
            OptionPanelManager<Mod, Panel>.LocaleChanged();
        }

        protected static int LanguagesIndex => ModLocalize.ModLanguageOptionIDList.FindIndex(x => x == SingletonMod<Config>.Instance.ModLanguage);
        protected static List<string> GetLanguages() {
            List<string> result = new();
            var IDs = ModLocalize.ModLanguageOptionIDList;
            var currentCulture = SingletonMod<Mod>.Instance.ModCulture;
            result.Add(CommonLocalize.ResourceManager.GetString($"Language_GameLanguage", currentCulture));
            for (int i = 1; i < IDs.Count; i++) {
                var prefix = CommonLocalize.ResourceManager.GetString($"Language_{IDs[i].Replace('-', '_')}", new CultureInfo(IDs[i]));
                string suffix = "(" + CommonLocalize.ResourceManager.GetString($"Language_{IDs[i].Replace('-', '_')}", currentCulture) + ")";
                var total = prefix + suffix;
                if (IDs[i] == currentCulture.Name) {
                    result.Add(prefix);
                } else {
                    result.Add(total);
                }
            }
            return result;
        }

    }

    public enum TypeWidth {
        NormalWidth = 744,
        ShrinkageWidth = 734
    }
}
