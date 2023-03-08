using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MbyronModsCommon {
    public class AssemblyUtils {
        private static string currentAssemblyName;
        private static Version currentAssemblyVersion;
        private static string currentAssemblyPath;
        private static List<string> folderNamesUnderCurrentAssembly;
        private static List<string> folderNamesUnderLocalePath;
        public static List<string> FoldersNameUnderLocalePath {
            get {
                if (folderNamesUnderLocalePath is null) {
                    var path = Path.Combine(CurrentAssemblyPath, "Locale");
                    var IDs = GetFoldersNameByPath(path).ToList();
                    folderNamesUnderLocalePath = IDs;
                    return IDs;
                } else return folderNamesUnderLocalePath;
            }
        }

        public static string CurrentAssemblyName {
            get {
                if (currentAssemblyName is not null) return currentAssemblyName;
                currentAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
                return currentAssemblyName;
            }
        }
        public static Version CurrentAssemblyVersion {
            get {
                if (currentAssemblyVersion is null) {
                    currentAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    return currentAssemblyVersion;
                }
                return currentAssemblyVersion;
            }
        }


        public static string CurrentAssemblyPath {
            get {
                if (currentAssemblyPath is not null) return currentAssemblyPath;
                currentAssemblyPath = GetCuttentAssemblyPath();
                return currentAssemblyPath;
            }
        }
        public static List<string> FoldersNameUnderCurrentAssembly {
            get {
                if (folderNamesUnderCurrentAssembly is not null) return folderNamesUnderCurrentAssembly;
                folderNamesUnderCurrentAssembly = GetFoldersNameByPath(CurrentAssemblyPath).ToList();
                return folderNamesUnderCurrentAssembly;
            }
        }

        private static string GetCuttentAssemblyPath() {
            foreach (var item in PluginManager.instance.GetPluginsInfo()) {
                var assembliesNames = item.assembliesString;
                if (assembliesNames.Contains(CurrentAssemblyName))
                    return item.modPath;
            }
            return null;
        }
        public static string[] GetFoldersNameByPath(string path) {
            if (Directory.Exists(path)) {
                string[] directory = Directory.GetDirectories(path);
                string[] names = new string[directory.Length];
                for (int i = 0; i < directory.Length; i++) {
                    names[i] = Path.GetFileName(directory[i]);
                }
                return names;
            } else {
                return null;
            }
        }


    }
}
