namespace MbyronModsCommon;
using System.IO;
using System.Xml.Serialization;
using System;
using ColossalFramework.IO;

public class ModConfig<T> : SingletonItem<T> where T : ModConfig<T>, new() {
    public string ModVersion { get; set; } = "0.0.0";
    public string ModLanguage { get; set; } = "GameLanguage";
    public bool DebugMode { get; set; }
    [XmlIgnore]
    public static string ConfigFilePath => Path.Combine(DataLocation.localApplicationData, $"{AssemblyUtils.CurrentAssemblyName}Config.xml");

    public static void Save() {
        try {
            using StreamWriter sw = new(ConfigFilePath);
            XmlSerializer xmlSerializer = new(typeof(T));
            XmlSerializerNamespaces xmlSerializerNamespaces = new();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            xmlSerializer.Serialize(sw, Instance, xmlSerializerNamespaces);
        } catch (Exception e) {
            InternalLogger.Exception($"Saving {ConfigFilePath} failed.", e);
        }
    }
    public static void Load() {
        InternalLogger.Log("Start loading mod config data.");
        var path = Path.Combine(DataLocation.localApplicationData, $"{AssemblyUtils.CurrentAssemblyName}Config.xml");
        try {
            if (File.Exists(path)) {
                using StreamReader sr = new(path);
                XmlSerializer xmlSerializer = new(typeof(T));
                var c = xmlSerializer.Deserialize(sr);
                if (c is not T) {
                    InternalLogger.Log($"Couldn't deserialize XML file, the target path: {path}.");
                    InternalLogger.Log("Generate mod default data.");
                    Instance = new();
                } else {
                    InternalLogger.Log("Local config exists, deserialize XML file succeeded.");
                    Instance = c as T;
                }
            } else {
                InternalLogger.Warning($"No local config found, use mod default config.");
                Instance = new();
            }
        } catch (Exception e) {
            InternalLogger.Exception($"Could't load data from XML file.", e);
        }
    }
}