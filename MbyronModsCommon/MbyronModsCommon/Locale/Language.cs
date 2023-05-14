namespace MbyronModsCommon;
using ColossalFramework.Globalization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class Language {
    public static List<string> SupportedLocaleIDs { get; } = GetSupportLocales();
    public static List<string> LanguagesList { get; } = GetLanguagesList();

    public static string GetGameLanguage() {
        foreach (var item in SupportedLocaleIDs) {
            if (item == LocaleExtension()) {
                return item;
            }
        }
        return "en";
    }
    public static List<string> GetLanguagesList() {
        var raw = GetSupportLocales();
        raw.Insert(0, "GameLanguage");
        return raw;
    }

    public static string LocaleExtension() => LocaleExtension(LocaleManager.instance.language);
    public static string LocaleExtension(string locale) {
        //if (!LocaleManager.exists)
        //    return "en";
        //string locale = LocaleManager.instance.language;
        if (locale == "en") {
            locale = "en-US";
        } else if (locale == "zh") {
            var culture = CultureInfo.InstalledUICulture.Name;
            if (culture == "zh-TW" || culture == "zh-HK") {
                locale = "zh-TW";
            } else {
                locale = "zh-CN";
            }
        } else if (locale == "es") {
            locale = "es-ES";
        } else if (locale == "pt") {
            locale = "pt-BR";
        }
        return locale;
    }

    private static List<string> GetSupportLocales() {
        var locales = new List<string> { "en" };
        if (!string.IsNullOrEmpty(AssemblyUtils.CurrentAssemblyPath)) {
            var localeFolder = Path.Combine(AssemblyUtils.CurrentAssemblyPath, "Locale");
            if (Directory.Exists(localeFolder)) {
                foreach (var file in Directory.GetFiles(localeFolder)) {
                    var id = Path.GetFileNameWithoutExtension(file).Split('.').Last();
                    if (id != "Localize")
                        locales.Add(id);
                }
            }
        }
        locales.Sort();
        return locales;
    }
}

public enum LanguageType {
    Default,
    Custom
}