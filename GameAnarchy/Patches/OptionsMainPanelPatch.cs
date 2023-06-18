namespace GameAnarchy.Patches;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;

public static class OptionsMainPanelPatch {
    public static MethodInfo GetOriginalOnVisibilityChanged() => AccessTools.Method(typeof(OptionsMainPanel), "OnVisibilityChanged");
    public static MethodInfo GetOnVisibilityChangedPostfix() => AccessTools.Method(typeof(OptionsMainPanelPatch), "OnVisibilityChangedPostfix");
    public static MethodInfo GetOriginalAddUserMods() => AccessTools.Method(typeof(OptionsMainPanel), "AddUserMods");
    public static MethodInfo GetAddUserModsTranspiler() => AccessTools.Method(typeof(OptionsMainPanelPatch), "AddUserModsTranspiler");

    public static void OnVisibilityChangedPostfix(UIComponent comp, bool visible) {
        if (visible) {
            try {
                SingletonManager<Manager>.Instance.SetCategoriesOffset(comp);
            } catch (Exception e) {
                ExternalLogger.Exception($"Options main panel OnVisibilityChanged patch failed.", e);
            }
        }
    }
    public static IEnumerable<CodeInstruction> AddUserModsTranspiler(IEnumerable<CodeInstruction> instructions) {
        MethodInfo addCategory = typeof(OptionsMainPanel).GetMethod("AddCategory", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo getPluginsInfo = typeof(PluginManager).GetMethod("GetPluginsInfo", BindingFlags.Public | BindingFlags.Instance);
        MethodInfo addCategoryExtension = AccessTools.Method(typeof(OptionsMainPanelPatch), nameof(AddCategoryExtension));
        MethodInfo getPluginsInfoInOrder = AccessTools.Method(typeof(PluginManagerExtension), nameof(PluginManagerExtension.GetPluginsInfoSortByName));
        FieldInfo categoriesField = AccessTools.Field(typeof(OptionsMainPanel), "m_Categories");
        FieldInfo categoriesContainerField = AccessTools.Field(typeof(OptionsMainPanel), "m_CategoriesContainer");
        FieldInfo dummiesField = AccessTools.Field(typeof(OptionsMainPanel), "m_Dummies");
        IEnumerator<CodeInstruction> instructionsEnumerator = instructions.GetEnumerator();
        while (instructionsEnumerator.MoveNext()) {
            CodeInstruction instruction = instructionsEnumerator.Current;
            if (instruction.Calls(getPluginsInfo)) {
                instruction = new CodeInstruction(OpCodes.Call, getPluginsInfoInOrder);
            }
            if (instruction.Calls(addCategory)) {
                yield return new CodeInstruction(OpCodes.Ldloc_0);
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldflda, categoriesField);
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldflda, categoriesContainerField);
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldflda, dummiesField);
                instruction = new CodeInstruction(OpCodes.Call, addCategoryExtension);
            }
            yield return instruction;
        }

    }

    private static string GetModUpdatedDate(PluginManager.PluginInfo pluginInfo) {
        var updatedTime = pluginInfo.updateTime;
        if (DateTime.Equals(updatedTime, DateTime.MinValue)) {
            updatedTime = GetModUpdatedDate(pluginInfo.modPath);
            if (Config.Instance.DebugMode) {
                if (pluginInfo.publishedFileID.Equals(new ColossalFramework.PlatformServices.PublishedFileId(ulong.MaxValue))) {
                    ExternalLogger.Log($"Plugin [{pluginInfo.name}] is a local mod, get last write time date: {updatedTime}");
                } else {
                    ExternalLogger.Log($"Plugin [{pluginInfo.name}] updated time is not initialized yet, get last write time date: {updatedTime}");
                }
            }
        } else {
            updatedTime = updatedTime.ToLocalTime();
        }
        var span = updatedTime - DateTime.Now;
        var days = Math.Abs(span.Days);
        var months = days / 30;
        var years = months / 12;

        string date;
        if (days == 0) {
            var newDay = new DateTime(updatedTime.Year, updatedTime.Month, updatedTime.Day).AddDays(1);
            var flag = DateTime.Compare(newDay, DateTime.Now);
            if (flag > 0) {
                date = "<color #8BBD3A>" + string.Format(Localize.Updated_Today, updatedTime) + "</color>";
            } else {
                date = "<color #8BBD3A>" + string.Format(Localize.Updated_YesterdayAt, updatedTime) + "</color>";
            }
        } else if (days <= 30) {
            if (days == 1) {
                date = "<color #059641>" + Localize.Updated_Yesterday + "</color>";
            } else {
                date = "<color #059641>" + string.Format(Localize.Updated_DaysAgo, days) + "</color>";
            }
        } else if (days > 30 && days <= 365) {
            if (days <= 60) {
                date = "<color #009ED6>" + string.Format(Localize.Updated_MonthAgo, months) + "</color>";
            } else if (days <= 90) {
                date = "<color #009ED6>" + string.Format(Localize.Updated_MonthsAgo, months) + "</color>";
            } else if (days <= 180) {
                date = "<color #6A2A78>" + string.Format(Localize.Updated_MonthsAgo, months) + "</color>";
            } else {
                date = "<color #F08E2B>" + string.Format(Localize.Updated_MonthsAgo, months) + "</color>";
            }
        } else {
            if (days <= 730) {
                date = "<color #E92E32>" + Localize.Updated_LastYear + "</color>";
            } else {
                date = "<color #E92E32>" + string.Format(Localize.Updated_YearsAgo, years) + "</color>";
            }
        }
        return date;
    }
    private static void AddCategoryExtension(string name, UIComponent container, PluginManager.PluginInfo pluginInfo, ref UIListBox categories, ref UITabContainer categoriesContainer, ref List<UIComponent> dummies) {
        if (Config.Instance.OptionPanelCategoriesUpdated) {
            if (pluginInfo.publishedFileID.Equals(new ColossalFramework.PlatformServices.PublishedFileId(ulong.MaxValue))) {
                name += $" | <color #FEF011>Local</color>";
            }
            name = name + " | " + GetModUpdatedDate(pluginInfo);
        }
        List<string> list;
        if (categories.items != null) {
            list = new List<string>(categories.items);
        } else {
            list = new List<string>();
        }
        list.Add(name);
        if (container == null) {
            container = categoriesContainer.AddUIComponent<UIPanel>();
            dummies.Add(container);
        }
        container.zOrder = list.Count - 1;
        categories.items = list.ToArray();
    }
    public static DateTime GetModUpdatedDate(string path) {
        var dateTime = DateTime.MinValue;
        if (Directory.Exists(path)) {
            foreach (var filePAth in Directory.GetFiles(path, "*", SearchOption.AllDirectories)) {
                if (Path.GetFileName(filePAth) != ".excluded") {
                    var lastWriteTime = File.GetLastWriteTime(filePAth);
                    if (lastWriteTime > dateTime) {
                        dateTime = lastWriteTime;
                    }
                }
            }
        }
        return dateTime;
    }

}
