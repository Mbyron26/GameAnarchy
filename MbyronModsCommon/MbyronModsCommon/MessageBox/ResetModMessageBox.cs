using ColossalFramework.UI;

namespace MbyronModsCommon {
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
