namespace GameAnarchy;
using ColossalFramework.Plugins;
using ColossalFramework;
using ICities;
using System.Collections.Generic;
using ColossalFramework.Globalization;
using MbyronModsCommon;
using MbyronModsCommon.UI;
using System.Linq;

public partial class Manager {
    public void InitBuildinModChecker() {
        var result = HandleBuiltinConflictMods();
        if (result.Any()) {
            MessageBox.Show<BuiltinCompatibilityMessageBox>().Init(result);
        }
    }

    private List<BuiltinConflictModInfo> HandleBuiltinConflictMods() {
        var list = new List<BuiltinConflictModInfo>();
        foreach (var info in Singleton<PluginManager>.instance.GetPluginsInfoSortByName()) {
            if (info is not null && info.userModInstance is IUserMod && info.isBuiltin && info.isEnabled) {
                switch (info.name) {
                    case "UnlockAll":
                        list.Add(new BuiltinConflictModInfo(info.name, Locale.Get("MOD_NAME", "Unlock All"), Localize.UnlockAllConflict));
                        info.isEnabled = false;
                        InternalLogger.Log("Disabled builtin mod: UnlockAll");
                        break;
                    case "UnlimitedOilAndOre":
                        list.Add(new BuiltinConflictModInfo(info.name, Locale.Get("MOD_NAME", "Unlimited Oil And Ore"), Localize.UnlimitedOilAndOreConflict));
                        info.isEnabled = false;
                        InternalLogger.Log("Disabled builtin mod: UnlimitedOilAndOre");
                        break;
                    case "UnlimitedMoney":
                        list.Add(new BuiltinConflictModInfo(info.name, Locale.Get("MOD_NAME", "Unlimited Money"), Localize.UnlimitedMoneyConflict));
                        info.isEnabled = false;
                        InternalLogger.Log("Disabled builtin mod: UnlimitedMoney");
                        break;
                }
            }
        }
        return list;
    }
}

public class BuiltinCompatibilityMessageBox : MessageBoxBase {
    public void Init(List<BuiltinConflictModInfo> builtinConflictModInfo) {
        TitleText = $"{ModMainInfo<Mod>.ModName} {GameAnarchy.Localize.BuiltinModCheck}";
        AddLabelInMainPanel(GameAnarchy.Localize.BuiltinModWarning);
        builtinConflictModInfo.ForEach(a => AddItem(a));
        AddButton(CommonLocalize.MessageBox_OK, Close);
    }

    private AlphaSinglePropertyPanel AddItem(BuiltinConflictModInfo mod) {
        var panel = MainPanel.AddUIComponent<AlphaSinglePropertyPanel>();
        panel.Atlas = CustomUIAtlas.MbyronModsAtlas;
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