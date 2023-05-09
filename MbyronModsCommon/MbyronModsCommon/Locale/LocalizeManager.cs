//LocalizeManager & LocalizeSet Cited source: https://github.com/MacSergey/ModsCommon Implemented by MacSergey.
namespace MbyronModsCommon;
using ColossalFramework.Plugins;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

public class LocalizeManager {
    private string Name { get; }
    private string AssemblyPatch { get; } = string.Empty;
    private Dictionary<string, LocalizeSet> Languages { get; } = new();

    public LocalizeManager(string name, Assembly assembly) {
        Name = name;
        foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo()) {
            if (plugin != null && plugin.ContainsAssembly(assembly)) {
                AssemblyPatch = plugin.modPath;
                break;
            }
        }
    }

    public string GetString(string key, CultureInfo culture) => TryGetString(key, culture, out var str) ? str : key;
    public bool TryGetString(string key, CultureInfo culture, out string str) {
        if (culture != null) {
            if (!Languages.ContainsKey(culture.Name))
                Load(culture);

            while (culture != null) {
                if (Languages.TryGetValue(culture.Name, out var language) && language.TryGetString(key, out str))
                    return true;
                else if (string.IsNullOrEmpty(culture.Name))
                    break;
                else
                    culture = culture.Parent;
            }
        }

        str = null;
        return false;
    }

    private void Load(CultureInfo culture) {
        if (!string.IsNullOrEmpty(culture.Name) && culture.Parent != null)
            Load(culture.Parent);

        if (!Languages.ContainsKey(culture.Name)) {
            var file = Path.Combine(AssemblyPatch, "Locale");
            if (Directory.GetFiles(file, $"{Name}.*.resx").Length == 0) {
                file = Path.Combine(file, "Common");
            }
            if (string.IsNullOrEmpty(culture.Name))
                file = Path.Combine(file, $"{Name}.resx");
            else
                file = Path.Combine(file, $"{Name}.{culture.Name}.resx");

            var set = new LocalizeSet(file, culture);
            Languages[culture.Name] = set;
        }
    }

    public IEnumerable<string> GetSupportLocales() {
        if (!string.IsNullOrEmpty(AssemblyPatch)) {
            var localeFolder = Path.Combine(AssemblyPatch, "Locale");
            if (Directory.Exists(localeFolder)) {
                foreach (var file in Directory.GetFiles(localeFolder, $"{Name}.*.resx")) {
                    var locale = Path.GetFileNameWithoutExtension(file).Split('.').Last();
                    yield return locale;
                }
            }
        }
    }
}

public class LocalizeSet {
    private Dictionary<string, string> Locales { get; } = new Dictionary<string, string>();

    public bool TryGetString(string key, out string str) => Locales.TryGetValue(key, out str);

    public LocalizeSet(string file, CultureInfo culture) {
        try {
            var reader = new ResxReader(file);
            foreach (var item in reader)
                Locales[item.Name] = item.Value;
        } catch { }
    }
}
