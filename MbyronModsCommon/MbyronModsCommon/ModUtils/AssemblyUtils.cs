namespace MbyronModsCommon;
using ColossalFramework.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class AssemblyUtils {
    private static string currentAssemblyName;
    private static Version currentAssemblyVersion;
    private static string currentAssemblyPath;
    private static List<string> folderNamesUnderCurrentAssembly;
    private static List<string> foldersNameUnderLocalePath;

    public static string CurrentAssemblyName => currentAssemblyName ??= Assembly.GetExecutingAssembly().GetName().Name;
    public static Version CurrentAssemblyVersion => currentAssemblyVersion ??= Assembly.GetExecutingAssembly().GetName().Version;
    public static string CurrentAssemblyPath => currentAssemblyPath ??= GetCuttentAssemblyPath();
    public static List<string> FoldersNameUnderLocalePath => foldersNameUnderLocalePath ??= GetFoldersNameByPath(Path.Combine(CurrentAssemblyPath, "Locale")).ToList();
    public static List<string> FoldersNameUnderCurrentAssembly => folderNamesUnderCurrentAssembly ??= GetFoldersNameByPath(CurrentAssemblyPath).ToList();

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