using ColossalFramework.Plugins;
using ColossalFramework;
using ICities;
using System.Collections.Generic;
using ColossalFramework.Globalization;
using MbyronModsCommon;

namespace GameAnarchy {
    public class CompatibilityExtension {
        public static List<string> GetLocalIncompatibleMods() {
            List<string> modList = new();
            foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfo()) {
                if (info is not null && info.userModInstance is IUserMod) {
                    if (info.isEnabled && info.isBuiltin) {
                        switch (info.name) {
                            case "UnlockAll":
                                modList.Add('[' + Locale.Get("MOD_NAME", "Unlock All") + ']' + "  -  " + SingletonMod<Mod>.Instance.GetLocale("LocalModWarning"));
                                break;
                            case "UnlimitedOilAndOre":
                                modList.Add('[' + Locale.Get("MOD_NAME", "Unlimited Oil And Ore") + ']' + "  -  " + SingletonMod<Mod>.Instance.GetLocale("LocalModWarning"));
                                break;
                            case "UnlimitedMoney":
                                modList.Add('[' + Locale.Get("MOD_NAME", "Unlimited Money") + ']' + "  -  " + SingletonMod<Mod>.Instance.GetLocale("LocalUnlimitedMoneyWarning"));
                                break;
                        }
                    }
                }
            }
            return modList;
        }
    }
}
