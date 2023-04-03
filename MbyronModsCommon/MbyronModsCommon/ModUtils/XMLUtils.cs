using ColossalFramework;
using System;
using System.IO;
using System.Xml.Serialization;

namespace MbyronModsCommon {
    public class XMLUtils {
        public static bool DeserializationState { get; set; } = false;
        public static void LoadData<Class>(string fileNameWithPath) where Class : SingletonMod<Class>, new() {
            if (fileNameWithPath.IsNullOrWhiteSpace()) {
                InternalLogger.Error($"{fileNameWithPath} is null or empty.");
            } else {
                InternalLogger.Log("Start loading mod config data.");
                try {
                    if (File.Exists(fileNameWithPath)) {
                        using (StreamReader sr = new(fileNameWithPath)) {
                            XmlSerializer xmlSerializer = new(typeof(Class));
                            var c = xmlSerializer.Deserialize(sr);
                            if (c is not Class) {
                                InternalLogger.Log($"Couldn't deserialize XML file, the target path: {fileNameWithPath}.");
                                InternalLogger.Log("Try to generate mod default data.");
                                SingletonMod<Class>.Instance = new();
                                InternalLogger.Log("Generate mod default data succeeded.");
                            } else {
                                SingletonMod<Class>.Instance = c as Class;
                                DeserializationState = true;
                                InternalLogger.Log("Local config exists, deserialize XML file succeeded.");
                            }
                        }
                    } else {
                        SingletonMod<Class>.Instance = new();
                        DeserializationState = true;
                        InternalLogger.Warning($"No local config found, use mod default config.");
                    }
                }
                catch (Exception e) {
                    InternalLogger.Exception($"Could't load data from XML file.", e);
                }
            }
        }

        public static void SaveData<Class>(string fileNameWithPath) where Class : SingletonMod<Class> {
            try {
                using StreamWriter sw = new(fileNameWithPath);
                XmlSerializer xmlSerializer = new(typeof(Class));
                xmlSerializer.Serialize(sw, SingletonMod<Class>.Instance);
            }
            catch (Exception e) {
                InternalLogger.Exception($"Could't save data to XML file.", e);
            }
        }

    }
}
