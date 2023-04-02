using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                InternalLogger.Log($"Start resetting mod config.");
                SingletonMod<Config>.Instance = null;
                SingletonMod<Config>.Instance = new();
                OptionPanelManager<Mod, OptionPanel>.LocaleChanged();
                InternalLogger.Log($"Reset mod config succeeded.");
                MessageBox.Show<ResetModMessageBox>().Init<Mod>();
            }
            catch (Exception e) {
                InternalLogger.Exception($"Reset settings failed:", e);
                MessageBox.Show<ResetModMessageBox>().Init<Mod>(false);
            }
        }

        private static void ShowLog() => MessageBox.Show<LogMessageBox>().Initialize<Mod>(false);
        private static void ShowCompatibility() => MessageBox.Show<CompatibilityMessageBox>().Initialize(ModMainInfo<Mod>.ModName);
    }

    public class GeneralOptionsBase<Mod, Config> where Mod : IMod where Config : ModConfigBase<Config> {
        protected UIComponent Parent { get; set; }
        public GeneralOptionsBase(UIComponent parent, TypeWidth typeWidth) {
            Parent = parent;
            //CustomLabel.AddLabel(parent, ModMainInfo<Mod>.ModName, (float)typeWidth, new(), 2f, Color.white).font = CustomFont.SemiBold;
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


    public class OptionPanelBase<TypeMod, TypeConfig, TypeOptionPanel> : UIPanel where TypeMod : IMod where TypeConfig : ModConfigBase<TypeConfig>, new() where TypeOptionPanel : UIPanel {
        protected OptionPanelTabs BasePanel { get; private set; }
        protected AutoLayoutScrollablePanel GeneralContainer { get; private set; }
        protected AutoLayoutScrollablePanel HotkeyContainer { get; private set; }
        protected AutoLayoutScrollablePanel AdvancedContainer { get; private set; }
        protected virtual float GeneralContainerWidth => (float)TypeWidth.NormalWidth;
        protected virtual float HotkeyContainerWidth => (float)TypeWidth.NormalWidth;
        protected virtual float AdvancedContainerWidth => (float)TypeWidth.NormalWidth;


        public OptionPanelBase() {
            BasePanel = CustomTabs.AddCustomTabs(this);
            AddGeneralContainer();
            AddExtraContainer();
            AddAdvancedContainer();
        }


        private void AddGeneralContainer() {
            GeneralContainer = AddTab(nameof(GeneralContainer), CommonLocalize.OptionPanel_General);
            OptionPanelTool.AddGroup(GeneralContainer, GeneralContainerWidth, CommonLocalize.OptionPanel_ModInfo);
            OptionPanelTool.AddLabel($"{ModMainInfo<TypeMod>.ModName}", $"{CommonLocalize.OptionPanel_Version}{ModMainInfo<TypeMod>.ModVersion}({ModMainInfo<TypeMod>.VersionType})", out UILabel _, out UILabel _);
            AddExtraModInfoProperty();
            OptionPanelTool.AddDropDown(CommonLocalize.Language, null, GetLanguages().ToArray(), LanguagesIndex, 310, 30, out UILabel _, out UILabel _, out UIDropDown _, (v) => {
                OnLanguageSelectedIndexChanged<TypeConfig>(v);
                AddLanguageSelectedIndexChanged();
            });
            OptionPanelTool.Reset();
            FillGeneralContainer();
        }

        protected virtual void AddExtraContainer() => FillHotkeyContainer();

        protected virtual void FillGeneralContainer() { }
        protected virtual void FillHotkeyContainer() => HotkeyContainer = AddTab(nameof(HotkeyContainer), CommonLocalize.OptionPanel_Hotkeys);
        protected virtual void FillAdvancedContainer() { }
        protected virtual void AddExtraModInfoProperty() { }
        protected virtual void AddLanguageSelectedIndexChanged() { }
        private void AddAdvancedContainer() {
            AdvancedContainer = AddTab(nameof(AdvancedContainer), CommonLocalize.OptionPanel_Advanced);
            OptionPanelTool.AddGroup(AdvancedContainer, AdvancedContainerWidth, CommonLocalize.OptionPanel_Advanced);
            OptionPanelTool.AddToggleButton(SingletonMod<TypeConfig>.Instance.DebugMode, CommonLocalize.OptionPanel_DebugMode, CommonLocalize.OptionPanel_DebugMinor, _ => SingletonMod<TypeConfig>.Instance.DebugMode = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddButton(CommonLocalize.ChangeLog_Major, null, CommonLocalize.ChangeLog, 250, 30, ShowLog, out UILabel _, out UILabel _, out UIButton _);
            OptionPanelTool.AddButton(CommonLocalize.CompatibilityCheck_Major, CommonLocalize.CompatibilityCheck_Minor, CommonLocalize.Check, 250, 30, ShowCompatibility, out UILabel _, out UILabel _, out UIButton _);
            OptionPanelTool.AddButton(CommonLocalize.ResetModMajor, CommonLocalize.ResetModMinor, CommonLocalize.Reset, 250, 30, ResetSettings, out UILabel _, out UILabel _, out UIButton _);
            OptionPanelTool.Reset();
        }

        protected AutoLayoutScrollablePanel AddTab(string containerName, string text) => BasePanel.AddTab(containerName, text, 0, 30, 1.2f).MainPanel;

        protected static void OnLanguageSelectedIndexChanged<Panel>(int value) {
            if (value == 0) {
                SingletonMod<TypeMod>.Instance.ModCulture = new CultureInfo(ModLocalize.LocalizationExtension());
                SingletonMod<TypeConfig>.Instance.ModLanguage = "GameLanguage";
            } else {
                SingletonMod<TypeMod>.Instance.ModCulture = new CultureInfo(ModLocalize.ModLanguageOptionIDList[value]);
                SingletonMod<TypeConfig>.Instance.ModLanguage = ModLocalize.ModLanguageOptionIDList[value];
            }
            OptionPanelManager<TypeMod, TypeOptionPanel>.LocaleChanged();
        }

        protected static int LanguagesIndex => ModLocalize.ModLanguageOptionIDList.FindIndex(x => x == SingletonMod<TypeConfig>.Instance.ModLanguage);
        protected static List<string> GetLanguages() {
            List<string> result = new();
            var IDs = ModLocalize.ModLanguageOptionIDList;
            var currentCulture = SingletonMod<TypeMod>.Instance.ModCulture;
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

        //protected virtual void ResetSettings() { }

        protected void ResetSettings/*<OptionPanel>*/() /*where OptionPanel : UIPanel*/ {
            try {
                InternalLogger.Log($"Start resetting mod config.");
                SingletonMod<TypeConfig>.Instance = null;
                SingletonMod<TypeConfig>.Instance = new();
                OptionPanelManager<TypeMod, TypeOptionPanel>.LocaleChanged();
                InternalLogger.Log($"Reset mod config succeeded.");
                MessageBox.Show<ResetModMessageBox>().Init<TypeMod>();
            }
            catch (Exception e) {
                InternalLogger.Exception($"Reset settings failed:", e);
                MessageBox.Show<ResetModMessageBox>().Init<TypeMod>(false);
            }
        }

        private static void ShowLog() => MessageBox.Show<LogMessageBox>().Initialize<TypeMod>(false);
        private static void ShowCompatibility() => MessageBox.Show<CompatibilityMessageBox>().Initialize(ModMainInfo<TypeMod>.ModName);

    }




    public enum TypeWidth {
        NormalWidth = 732/*744*/,
        ShrinkageWidth = 734
    }
}
