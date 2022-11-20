using ColossalFramework;
using ColossalFramework.PlatformServices;
using System.Collections.Generic;

namespace MbyronModsCommon {
    public class CompatibilityCheck {
        public static IncompatibleModInfo[] IncompatibleMods { get; set; }
        public static List<string> IncompatibleModsInfo { get; private set; } = new();
        public static bool IsConflict { get; set; }
        public readonly struct IncompatibleModInfo {
            public readonly ulong fileID;
            public readonly string name;
            public readonly string specialMsg;
            public readonly bool inclusive;
            public IncompatibleModInfo(ulong modID, string modName, bool isInclusive) {
                fileID = modID;
                name = modName;
                specialMsg = null;
                inclusive = isInclusive;
            }
            public IncompatibleModInfo(ulong modID, string modName, bool isInclusive, string extraMsg) {
                fileID = modID;
                name = modName;
                specialMsg = extraMsg;
                inclusive = isInclusive;
            }
        }

        public static void CheckCompatibility<Mod>() where Mod : IMod {
            GetIncompatibleMods<Mod>();
            if (IncompatibleModsInfo.Count > 0) {
                IsConflict = true;
                var messageBox = MessageBox.Show<CompatibilityMessageBox>();
                messageBox.Initialize<Mod>();
            }
        }

        private static void GetIncompatibleMods<Mod>() where Mod : IMod {
            string errorMsg = "";
            foreach (var mod in PlatformService.workshop.GetSubscribedItems()) {
                for (int i = 0; i < IncompatibleMods.Length; i++) {
                    if (mod.AsUInt64 == IncompatibleMods[i].fileID) {
                        errorMsg += '[' + IncompatibleMods[i].name + ']' + "  -  " +
                            (IncompatibleMods[i].inclusive ? $"{ModMainInfo<Mod>.ModName} already includes the same functionality.\n" : $"This mod is incompatible with {ModMainInfo<Mod>.ModName}.\n") +
                            (IncompatibleMods[i].specialMsg is null ? "" : IncompatibleMods[i].specialMsg + "");
                        var errorMsgList = '[' + IncompatibleMods[i].name + ']' + @"  -  " +
                             (IncompatibleMods[i].inclusive ? ModMainInfo<Mod>.ModName + " " + CommonLocale.ResourceManager.GetString("CompatibilityCheck_SameFunctionality", SingletonMod<Mod>.Instance.ModCulture) : CommonLocale.ResourceManager.GetString("CompatibilityCheck_Incompatible", SingletonMod<Mod>.Instance.ModCulture) + (IncompatibleMods[i].specialMsg is null ? "" : IncompatibleMods[i].specialMsg + ""));
                        IncompatibleModsInfo.Add(errorMsgList);
                    }
                }
            }
            if (errorMsg.Length > 0) {
                ModLogger.ModLog("The following incompatible mods have been detected:\n" + errorMsg);
            }
        }


    }
}
