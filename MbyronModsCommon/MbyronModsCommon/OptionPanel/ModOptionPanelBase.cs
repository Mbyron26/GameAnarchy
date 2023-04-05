using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MbyronModsCommon {
    public class OptionPanelBase<TypeMod, TypeConfig, TypeOptionPanel> : UIPanel where TypeMod : IMod where TypeConfig : ModConfigBase<TypeConfig>, new() where TypeOptionPanel : UIPanel {
        public const float PropertyPanelWidth = 732;
        protected OptionPanelTabs BasePanel { get; private set; }
        protected AutoLayoutScrollablePanel GeneralContainer { get; private set; }
        protected AutoLayoutScrollablePanel HotkeyContainer { get; private set; }
        protected AutoLayoutScrollablePanel AdvancedContainer { get; private set; }

        public OptionPanelBase() {
            BasePanel = CustomTabs.AddCustomTabs(this);
            AddGeneralContainer();
            AddExtraContainer();
            AddAdvancedContainer();
        }

        private void AddGeneralContainer() {
            GeneralContainer = AddTab(nameof(GeneralContainer), CommonLocalize.OptionPanel_General);
            OptionPanelTool.AddGroup(GeneralContainer, PropertyPanelWidth, CommonLocalize.OptionPanel_ModInfo);
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
            OptionPanelTool.AddGroup(AdvancedContainer, PropertyPanelWidth, CommonLocalize.OptionPanel_Advanced);
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

        ResetModWarningMessageBox messageBox;
        ResetModMessageBox messageBox1;

        protected void ResetSettings() {
            try {
                messageBox = MessageBox.Show<ResetModWarningMessageBox>();
                messageBox.Init<TypeMod>(First);  
            }
            catch (Exception e) {
                InternalLogger.Exception($"Reset settings failed:", e);
                MessageBox.Show<ResetModMessageBox>().Init<TypeMod>(false);
            }

            void First() {
                InternalLogger.Log($"Start resetting mod config.");
                SingletonMod<TypeConfig>.Instance = null;
                SingletonMod<TypeConfig>.Instance = new();
                OptionPanelManager<TypeMod, TypeOptionPanel>.LocaleChanged();
                InternalLogger.Log($"Reset mod config succeeded.");
                MessageBox.Hide(messageBox);
                messageBox1 = MessageBox.Show<ResetModMessageBox>();
                messageBox1.Init<TypeMod>(true);
            }
        }

        private static void ShowLog() => MessageBox.Show<LogMessageBox>().Initialize<TypeMod>(false);
        private static void ShowCompatibility() => MessageBox.Show<CompatibilityMessageBox>().Initialize(ModMainInfo<TypeMod>.ModName);

    }

}
