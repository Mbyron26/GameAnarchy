using ColossalFramework.UI;
using System;

namespace MbyronModsCommon {
    public class ResetModWarningMessageBox : MessageBoxBase {
        public void Init<Mod>(Action callback) where Mod : IMod {
            var label1 = CustomLabel.AddLabel(MainPanel, $"{ModMainInfo<Mod>.ModName} {CommonLocalize.Reset}", buttonWidth, textScale: 1.3f);
            label1.font = CustomFont.SemiBold;
            label1.textAlignment = UIHorizontalAlignment.Center;
            label1.textAlignment = UIHorizontalAlignment.Center;
            var label2 = CustomLabel.AddLabel(MainPanel, "Are you sure you want to reset the configuration of the mod?", buttonWidth);
            label2.textAlignment = UIHorizontalAlignment.Center;
            label2.textAlignment = UIHorizontalAlignment.Center;
            var button1 = AddButtons(1, 2, CommonLocalize.MessageBox_OK, () => callback());
            button1.SetWarningStyle();
            var button2 = AddButtons(2, 2, "Cancel", Close);
            button2.SetDefaultStyle();
        }
    }


    public class ResetModMessageBox : SimpleMessageBox {
        public void Init<Mod>(bool isSucceeded = true) where Mod : IMod {
            var label = MainPanel.AddUIComponent<UILabel>();
            label.autoSize = false;
            label.autoHeight = true;
            label.width = buttonWidth;
            label.wordWrap = true;
            label.font = CustomFont.SemiBold;
            label.textScale = 1.3f;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.text = $"{ModMainInfo<Mod>.ModName} {CommonLocalize.Reset}";
            CustomPanel.AddSpace(MainPanel, buttonWidth, 30);
            if (isSucceeded) {
                CustomLabel.AddLabel(MainPanel, CommonLocalize.ResetModSucceeded, buttonWidth, textScale: 1.1f).textAlignment = UIHorizontalAlignment.Center;
            } else {
                CustomLabel.AddLabel(MainPanel, CommonLocalize.ResetModFailed, buttonWidth, textScale: 1.1f).textAlignment = UIHorizontalAlignment.Center;
            }
            CustomPanel.AddSpace(MainPanel, MainPanel.width - 2 * DefaultPadding, 50);
        }
    }
}
