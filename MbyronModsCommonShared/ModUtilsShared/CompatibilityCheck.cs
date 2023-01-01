using ColossalFramework;
using ColossalFramework.PlatformServices;
using ColossalFramework.Plugins;
using ICities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbyronModsCommon {
    public class CompatibilityCheck {
        public static string ModName { get; set; }
        public static List<IncompatibleModInfo> IncompatibleMods { get; set; } = new();
        public static List<IncompatibleModInfo> DetectedIncompatibleMods { get; private set; } = new();
        public static Action RemoveConflictModsAction { get; set; }
        public static Func<List<string>> GetExtraModsInfo { get; set; }
        public static List<string> IncompatibleModsInfo {
            get {
                var list1 = GetIncompatibleModsInfo();
                List<string> list2 = new();
                if (GetExtraModsInfo != null) {
                    list2 = GetExtraModsInfo.Invoke();
                }
                List<string> result = new();
                if (list1.Count > 0) {
                    result = list1;
                }
                if (list2.Count > 0) {
                    foreach (var item in list2) {
                        result.Add(item);
                    }
                }
                return result;
            }
        }

        public static void RemoveConflictMods(MessageBoxBase messageBoxBase) {
            if (IncompatibleModsInfo.Count == 0) {
                return;
            }
            List<bool> flag = new();
            if (DetectedIncompatibleMods.Count > 0) {
                foreach (var item in DetectedIncompatibleMods) {
                    if (PlatformService.workshop.Unsubscribe(new PublishedFileId(item.fileID))) {
                        ModLogger.ModLog($"Unsubscribed Incompatible mod succeed: {item.name}");
                        flag.Add(true);
                    } else {
                        ModLogger.ModLog($"Unsubscribed Incompatible mod failed: {item.name}");
                        flag.Add(false);
                    }
                }
            }
            RemoveConflictModsAction?.Invoke();
            if (flag.TrueForAll(x => x = true)) {
                MessageBox.Hide(messageBoxBase);
                DetectedIncompatibleMods.Clear();
                var messageBox = MessageBox.Show<CompatibilityMessageBox>();
                messageBox.Initialize(ModName);
            } else {
                MessageBox.Hide(messageBoxBase);
            }

        }

        public static void CheckCompatibility() {
            if (IncompatibleModsInfo.Count > 0) {
                var messageBox = MessageBox.Show<CompatibilityMessageBox>();
                messageBox.Initialize(ModName);
            }
        }

        private static List<string> GetIncompatibleModsInfo() {
            CheckIncompatibleMods();
            if (DetectedIncompatibleMods.Count == 0) {
                return new();
            } else {
                List<string> result = new();
                foreach (var item in DetectedIncompatibleMods) {
                    result.Add($"[{item.name}] - " + (item.inclusive ? (ModName + " " + CommonLocale.CompatibilityCheck_SameFunctionality) : CommonLocale.CompatibilityCheck_Incompatible) + (item.useInstead is null ? "" : string.Format(CommonLocale.CompatibilityCheck_UseInstead, item.useInstead)));
                }
                return result;
            }
        }


        private static void CheckIncompatibleMods() {
            DetectedIncompatibleMods.Clear();
            if (IncompatibleMods.Count == 0) return;
            foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfo()) {
                if (info is not null && info.userModInstance is IUserMod) {
                    for (int i = 0; i < IncompatibleMods.Count; i++) {
                        if (info.publishedFileID.AsUInt64 == IncompatibleMods[i].fileID) {
                            DetectedIncompatibleMods.Add(IncompatibleMods[i]);
                        }
                    }
                }
            }
            if (DetectedIncompatibleMods.Count > 0) {
                StringBuilder stringBuilder = new();
                stringBuilder.Append($"[{DateTime.Now}] The following incompatible mods have been detected:\n");
                foreach (var item in DetectedIncompatibleMods) {
                    stringBuilder.Append($"[{item.name}]\n");
                }
                ModLogger.ModLog(stringBuilder.ToString());
            }
        }

    }

    public readonly struct IncompatibleModInfo {
        public readonly ulong fileID;
        public readonly string name;
        public readonly string useInstead;
        public readonly bool inclusive;
        public IncompatibleModInfo(ulong modID, string modName, bool isInclusive) {
            fileID = modID;
            name = modName;
            useInstead = null;
            inclusive = isInclusive;
        }
        public IncompatibleModInfo(ulong modID, string modName, bool isInclusive, string useInstead) {
            fileID = modID;
            name = modName;
            this.useInstead = useInstead;
            inclusive = isInclusive;
        }
    }
}
