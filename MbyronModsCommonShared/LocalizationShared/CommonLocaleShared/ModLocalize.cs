using ColossalFramework;
using ColossalFramework.Globalization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MbyronModsCommon {
    public static class ModLocalize {
        public static List<string> CheckLocalizationResources() {
            string path = Path.Combine(AssemblyUtils.CurrentAssemblyPath, "Locale");
            var IDs = AssemblyUtils.GetFoldersNameByPath(path);
            List<string> supportCulture = new();
            foreach (var item in IDs) {
                var subPath = Path.Combine(path, item);
                if (File.Exists(subPath + @"\" + AssemblyUtils.CurrentAssemblyName + ".resources.dll")) {
                    supportCulture.Add(item);
                } else {
                    ModLogger.GameLog($"Couldn't find locale resource: {item}. Path: {subPath}");
                }
            }
            return supportCulture;
        }
        private static List<string> modSupportLanguageIDs;
        public static List<string> ModSupportLanguageIDs {
            get {
                if (modSupportLanguageIDs is null) {
                    var IDs = CheckLocalizationResources();
                    string supportLocale = string.Empty;
                    for (int i = 0; i < IDs.Count; i++) {
                        if (i == IDs.Count - 1) {
                            supportLocale += IDs[i];
                        } else {
                            supportLocale += IDs[i] + ", ";
                        }
                    }
                    ModLogger.GameLog($"Support localization resources: {supportLocale}.");
                    modSupportLanguageIDs = IDs;
                    return IDs;
                } else return modSupportLanguageIDs;
            }
        }
        private static List<string> modLanguageOptionIDList;
        public static List<string> ModLanguageOptionIDList {
            get {
                if (modLanguageOptionIDList is not null) return modLanguageOptionIDList;
                var IDs = ModSupportLanguageIDs;
                IDs.Insert(0, "GameLanguage");
                modLanguageOptionIDList = IDs;
                return IDs;
            }
        }

        public static string UseGameLanguage() {
            var ID = LocalizationExtension();
            string culture = string.Empty;
            foreach (var item in ModSupportLanguageIDs) {
                if (item == ID) {
                    culture = item;
                }
            }
            if (culture.IsNullOrWhiteSpace()) {
                return "en";
            } else {
                return culture;
            }
        }

        public static string LocalizationExtension() {
            if (!LocaleManager.exists) return "en";
            string locale = LocaleManager.instance.language;
            if (locale == "zh") {
                var culture = CultureInfo.InstalledUICulture.Name;
                if (culture == "zh-TW" || culture == "zh-HK") {
                    locale = "zh-TW";
                } else {
                    locale = "zh-CN";
                }
            }
            return locale;
        }

    }
}
