using ColossalFramework.Plugins;
using ColossalFramework;
using ICities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

namespace MbyronModsCommon {
    public static class ExternalLogger {
        private static readonly object fileLock = new();
        private static string debugFilePath;
        public static string DebugFilePath {
            get {
                if (debugFilePath is null) {
                    var path = Path.Combine(Application.dataPath, AssemblyUtils.CurrentAssemblyName + "Debug.log");
                    debugFilePath = path;
                    return path;
                }
                return debugFilePath;
            }
        }

        public static void Error(string tag, string message) => LogBase($"[{nameof(Error)}]: {tag}" + message);
        public static void Error(string message) => LogBase($"[{nameof(Error)}]" + message);
        public static void Warning(string tag, string message) => LogBase($"[{nameof(Warning)}]: {tag}" + message);
        public static void Warning(string message) => LogBase($"[{nameof(Warning)}]" + message);
        public static void Log(string tag, string message) => LogBase($"[{nameof(Log)}]: {tag}" + message);
        public static void Log(string message) => LogBase($"[{nameof(Log)}]" + message);
        public static void Exception(string tag, Exception exception) => LogBase($"[{nameof(Exception)}]: {tag}" + exception.Message);
        public static void Exception(Exception exception) => LogBase($"[{nameof(Exception)}]" + exception.Message);
        public static void DebugMode(string msg, bool enableLog) {
            if (enableLog) {
                LogBase(msg, "Debug Mode");
            }
        }

        private static void LogBase(string message, string tag = null) {
            Monitor.Enter(fileLock);
            try {
                using FileStream debugFile = new(DebugFilePath, FileMode.Append);
                using StreamWriter sw = new(debugFile);
                if (tag is null) {
                    sw.WriteLine($"{new StackFrame(2, true).GetMethod().Name} ==> {message}");
                } else {
                    sw.WriteLine($"{tag}: {new StackFrame(2, true).GetMethod().Name} ==> {message}");
                }
            }
            finally {
                Monitor.Exit(fileLock);
            }
        }

        public static void CreateDebugFile<Mod>() where Mod : IMod {
            using FileStream debugFile = new(DebugFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using StreamWriter sw = new(debugFile);
            sw.WriteLine(@"--- " + ModMainInfo<Mod>.ModName + ' ' + ModMainInfo<Mod>.ModVersion + ' ' + ModMainInfo<Mod>.VersionType + @" Debug File ---");
            sw.WriteLine(Environment.OSVersion);
            sw.WriteLine(@"C# CLR Version: " + Environment.Version);
            sw.WriteLine(@"Unity Version: " + Application.unityVersion);
            sw.WriteLine(@"Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sw.WriteLine(@"----------------------------------------------");
        }

        public static void OutputPluginsList() {
            Monitor.Enter(fileLock);
            try {
                using FileStream debugFile = new(DebugFilePath, FileMode.Append, FileAccess.Write, FileShare.None);
                using StreamWriter sw = new(debugFile);
                sw.WriteLine(@"------------------ Plugins ------------------");
                foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfoSortByName()) {
                    if (info is not null && info.userModInstance is IUserMod modInstance)
                        sw.WriteLine($"{info.name} - {modInstance.Name} " + (info.isEnabled ? @"** Enabled **" : @"** Disabled **"));
                }
                sw.WriteLine(@"----------------------------------------------");
            }
            finally {
                Monitor.Exit(fileLock);
            }
        }

    }

    public static class InternalLogger {
        private static string Name => AssemblyUtils.CurrentAssemblyName;
        public static void Error(string tag, object message) => UnityEngine.Debug.logger.LogError($"[{Name}{nameof(LogType.Error)}]", $"{tag} | {message}");
        public static void Error(object message) => UnityEngine.Debug.logger.LogError($"[{Name}{nameof(LogType.Error)}]", $" {message}");
        public static void Warning(object message) => UnityEngine.Debug.logger.LogWarning($"[{Name}{nameof(LogType.Warning)}]", $"{message}");
        public static void Warning(string tag, object message) => UnityEngine.Debug.logger.LogWarning($"[{Name}{nameof(LogType.Warning)}]", $"{tag} | {message}");
        public static void Log(string tag, object message) => UnityEngine.Debug.logger.Log($"[{AssemblyUtils.CurrentAssemblyName}{nameof(LogType.Log)}]", $"{tag} | {message}");
        public static void Log(object message) => UnityEngine.Debug.logger.Log($"[{Name}{nameof(LogType.Log)}]", $"{message}");
        public static void Exception(string tag, Exception exception) => UnityEngine.Debug.logger.Log($"[{Name}{nameof(LogType.Exception)}]", $"{tag} | {exception}");
    }

}
