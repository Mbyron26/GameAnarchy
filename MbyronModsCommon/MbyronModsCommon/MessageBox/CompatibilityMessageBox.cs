namespace MbyronModsCommon;
using ColossalFramework.UI;
using System.Collections.Generic;

public class CompatibilityMessageBox : MessageBoxBase {
    public CompatibilityMessageBox() {
        if (CompatibilityCheck.IncompatibleModsInfo.Count > 0) {
            AddButtons(1, 2, CommonLocalize.MessageBox_OK, Close);
            AddButtons(2, 2, CommonLocalize.CompatibilityMessageBox_Unsubscribe, UnsubscribeMod);
        } else {
            AddButtons(1, 1, CommonLocalize.MessageBox_OK, Close);
        }
    }

    private void UnsubscribeMod() => CompatibilityCheck.RemoveConflictMods(this);
    public void Initialize(string modName) {
        TitleText = $"{modName} {CommonLocalize.OptionPanel_CompatibilityCheck}";
        var info = CompatibilityCheck.IncompatibleModsInfo;
        if (info.Count > 0) {
            AddPrompt(CommonLocalize.MessageBox_WarningPrompt);
            AddList(MainPanel, CompatibilityCheck.IncompatibleModsInfo);
        } else {
            AddPrompt(CommonLocalize.MessageBox_NormalPrompt);
        }
    }

    private UILabel AddPrompt(string text) {
        var label = MainPanel.AddUIComponent<UILabel>();
        label.autoSize = false;
        label.autoHeight = true;
        label.width = MessageBoxParm.ComponentWidth;
        label.wordWrap = true;
        label.textAlignment = UIHorizontalAlignment.Center;
        label.text = text;
        return label;
    }
    private void AddList(UIComponent root, List<string> list) {
        foreach (var item in list) {
            var label = root.AddUIComponent<UILabel>();
            label.textAlignment = UIHorizontalAlignment.Center;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.autoSize = false;
            label.wordWrap = true;
            label.width = MessageBoxParm.ComponentWidth;
            label.autoHeight = true;
            label.text = item;
            label.autoHeight = false;
        }
    }

}