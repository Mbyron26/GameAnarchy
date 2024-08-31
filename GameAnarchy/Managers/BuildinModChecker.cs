using ColossalFramework.Globalization;
using CSShared.Common;
using CSShared.Debug;
using CSShared.Localization;
using CSShared.Manager;
using CSShared.Tools;
using CSShared.UI;
using CSShared.UI.MessageBoxes;
using ICities;
using System.Collections.Generic;
using System.Linq;

namespace GameAnarchy.Managers;

public class BuiltInModManager : IManager {
    public void OnCreated() {
        var result = HandleBuiltInConflictMods();
        if (result.Any()) {
            MessageBox.Show<BuiltInCompatibilityMessageBox>().Init(result);
        }
    }

    public void OnReleased() { }
    public void Reset() { }
    public void Update() { }

    private List<BuiltinConflictModInfo> HandleBuiltInConflictMods() {
        var list = new List<BuiltinConflictModInfo>();
        foreach (var info in PluginTools.GetPluginsInfoSortByName()) {
            if (info is not null && info.userModInstance is IUserMod && info.isBuiltin && info.isEnabled) {
                switch (info.name) {
                    case "UnlockAll":
                        list.Add(new BuiltinConflictModInfo(info.name, Locale.Get("MOD_NAME", "Unlock All"), ModLocalizationManager.Localize("UnlockAllConflict")));
                        info.isEnabled = false;
                        LogManager.GetLogger().Info("Disabled builtIn mod: UnlockAll");
                        break;
                    case "UnlimitedOilAndOre":
                        list.Add(new BuiltinConflictModInfo(info.name, Locale.Get("MOD_NAME", "Unlimited Oil And Ore"), ModLocalizationManager.Localize("UnlimitedOilAndOreConflict")));
                        info.isEnabled = false;
                        LogManager.GetLogger().Info("Disabled builtIn mod: UnlimitedOilAndOre");
                        break;
                    case "UnlimitedMoney":
                        list.Add(new BuiltinConflictModInfo(info.name, Locale.Get("MOD_NAME", "Unlimited Money"), ModLocalizationManager.Localize("UnlimitedMoneyConflict")));
                        info.isEnabled = false;
                        LogManager.GetLogger().Info("Disabled builtIn mod: UnlimitedMoney");
                        break;
                }
            }
        }
        return list;
    }
}

public class BuiltInCompatibilityMessageBox : MessageBoxBase {
    public void Init(List<BuiltinConflictModInfo> builtInConflictModInfo) {
        TitleText = $"{SingletonMod<Mod>.Instance.ModName} {Localize("BuiltinModCheck")}";
        AddLabelInMainPanel(Localize("BuiltinModWarning"));
        builtInConflictModInfo.ForEach(a => AddItem(a));
        AddButton(Localize("MessageBox_OK"), Close);
    }

    private AlphaSinglePropertyPanel AddItem(BuiltinConflictModInfo mod) {
        var panel = MainPanel.AddUIComponent<AlphaSinglePropertyPanel>();
        panel.Atlas = CustomUIAtlas.CSSharedAtlas;
        panel.BgSprite = CustomUIAtlas.RoundedRectangle3;
        panel.BgNormalColor = CustomUIColor.CPPrimaryBg;
        panel.Padding = new UnityEngine.RectOffset(10, 10, 10, 10);
        panel.width = MessageBoxParm.ComponentWidth;
        panel.MajorLabelText = mod.major;
        panel.MinorLabelText = mod.minor;
        panel.MinorLabelTextScale = 0.9f;
        panel.StartLayout();
        return panel;
    }
}

public record struct BuiltinConflictModInfo {
    public string name;
    public string major;
    public string minor;
    public BuiltinConflictModInfo(string name, string major, string minor) {
        this.name = name;
        this.major = major;
        this.minor = minor;
    }
}