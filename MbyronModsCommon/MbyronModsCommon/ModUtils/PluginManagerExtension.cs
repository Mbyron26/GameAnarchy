namespace MbyronModsCommon;
using ColossalFramework.Plugins;
using ColossalFramework;
using System.Collections.Generic;
using System.Linq;
using ICities;

public static class PluginManagerExtension {
    public static IEnumerable<PluginManager.PluginInfo> GetPluginsInfoSortByName(this PluginManager pluginManager) => Singleton<PluginManager>.instance.GetPluginsInfo().Where(p => p?.userModInstance as IUserMod != null).OrderBy(p => ((IUserMod)p.userModInstance).Name);
}