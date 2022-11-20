using ColossalFramework.UI;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace MbyronModsCommon {
    public class AdvancedBase<Mod> where Mod : IMod {
        public AdvancedBase(UIComponent parent, TypeWidth typeWidth) {
            var advanced = OptionPanelCard.AddCard(parent, typeWidth, CommonLocale.OptionPanel_Advanced, out _, true);
            CustomButton.AddButton(advanced, 1f, CommonLocale.OptionPanel_ChangeLog, 400f, 34f, ShowLog);
            CustomButton.AddButton(advanced, 1f, CommonLocale.OptionPanel_CompatibilityCheck, 400f, 34f, ShowCompatibility);
        }
        private static void ShowLog() {
            var messageBox = MessageBox.Show<LogMessageBox>();
            messageBox.Initialize<Mod>(false);
        }
        private static void ShowCompatibility() {
            var messageBox = MessageBox.Show<CompatibilityMessageBox>();
            messageBox.Initialize<Mod>();
        }

    }

    public class GeneralOptionsBase<Mod, Config> where Mod : IMod where Config : ModConfigBase<Config> {
        public UIPanel ModInfo { get; }
        public GeneralOptionsBase(UIComponent parent, TypeWidth typeWidth) {
            ModInfo = OptionPanelCard.AddCard(parent, typeWidth, ModMainInfo<Mod>.ModName, out _, true);
            CustomLabel.AddLabel(ModInfo, $"{CommonLocale.OptionPanel_Version}: {ModMainInfo<Mod>.ModVersion}", 690, 1f, UIColor.White);
        }

        public static void AddLocaleDropdown<Panel>(UIComponent parent) where Panel : UIPanel {
            var dropdown = CustomDropdown.AddDropdown(parent, CommonLocale.Language, 1f, GetLanguages().ToArray(), LanguagesIndex, 300, 32, 1f, new RectOffset(10, 10, 8, 0), new RectOffset(6, 6, 4, 0));
            dropdown.eventSelectedIndexChanged += (c, s) => {
                if (s == 0) {
                    SingletonMod<Mod>.Instance.ModCulture = new CultureInfo(ModLocalize.LocalizationExtension());
                    SingletonMod<Config>.Instance.ModLanguage = "GameLanguage";
                } else {
                    SingletonMod<Mod>.Instance.ModCulture = new CultureInfo(ModLocalize.ModLanguageOptionIDList[s]);
                    SingletonMod<Config>.Instance.ModLanguage = ModLocalize.ModLanguageOptionIDList[s];
                }
                OptionPanelManager<Mod, Panel>.LocaleChanged();
            };
        }

        protected static int LanguagesIndex => ModLocalize.ModLanguageOptionIDList.FindIndex(x => x == SingletonMod<Config>.Instance.ModLanguage);
        protected static List<string> GetLanguages() {
            List<string> result = new();
            var IDs = ModLocalize.ModLanguageOptionIDList;
            var currentCulture = SingletonMod<Mod>.Instance.ModCulture;
            result.Add(CommonLocale.ResourceManager.GetString($"Language_GameLanguage", currentCulture));
            for (int i = 1; i < IDs.Count; i++) {
                var prefix = CommonLocale.ResourceManager.GetString($"Language_{IDs[i].Replace('-', '_')}", new CultureInfo(IDs[i]));
                string suffix = "(" + CommonLocale.ResourceManager.GetString($"Language_{IDs[i].Replace('-', '_')}", currentCulture) + ")";
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

}

