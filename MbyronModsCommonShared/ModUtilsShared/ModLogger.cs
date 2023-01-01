using ColossalFramework.Plugins;
using ColossalFramework;
using ICities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;

namespace MbyronModsCommon {
    public class ModLogger {
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

        public static void GameLog(object message) => UnityEngine.Debug.Log("[" + AssemblyUtils.CurrentAssemblyName + "]" + " => " + message);
        public static void GameLog(object message, Exception e) => UnityEngine.Debug.Log($"[{AssemblyUtils.CurrentAssemblyName}] => {message}, detial: {e}");
        public static void CreateDebugFile<Mod>() where Mod : IMod {
            using FileStream debugFile = new(DebugFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using StreamWriter sw = new(debugFile);
            sw.WriteLine(@"--- " + ModMainInfo<Mod>.ModName + ' ' + ModMainInfo<Mod>.ModVersion + @" Debug File ---");
            sw.WriteLine(Environment.OSVersion);
            sw.WriteLine(@"C# CLR Version " + Environment.Version);
            sw.WriteLine(@"Unity Version " + Application.unityVersion);
            sw.WriteLine(@"-------------------------------------");
        }
        public static void OutputPluginsList() {
            Monitor.Enter(fileLock);
            try {
                using FileStream debugFile = new(DebugFilePath, FileMode.Append, FileAccess.Write, FileShare.None);
                using StreamWriter sw = new(debugFile);
                sw.WriteLine(@"Mods Installed are:");
                foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfo()) {
                    if (info is not null && info.userModInstance is IUserMod modInstance)
                        sw.WriteLine(info.name + '-' + modInstance.Name + ' ' + (info.isEnabled ? @"** Enabled **" : @"** Disabled **"));
                }
                sw.WriteLine(@"-------------------------------------");
            }
            finally {
                Monitor.Exit(fileLock);
            }
        }

        public static void ModLog(string msg)  {
            Monitor.Enter(fileLock);
            try {
                using FileStream debugFile = new(DebugFilePath, FileMode.Append);
                using StreamWriter sw = new(debugFile);
                sw.WriteLine($"{new StackFrame(1, true).GetMethod().Name} ==> {msg}");
            }
            finally {
                Monitor.Exit(fileLock);
            }
        }


    }

}
