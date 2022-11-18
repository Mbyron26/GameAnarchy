using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomLabel {
        public static UILabel AddLabel(UIComponent parent, string text, float? width, float textScale = 1.0f, Color32 color = default) {
            UILabel textLabel = parent.AddUIComponent<UILabel>();
            textLabel.autoSize = false;
            textLabel.autoHeight = true;
            if (width.HasValue) {
                textLabel.width = width.Value;
            }
            textLabel.textScale = textScale;
            textLabel.wordWrap = true;
            textLabel.textColor = color;
            textLabel.text = text;
            return textLabel;
        }

        public static void AddBuiltinFunctionLabel(UIComponent parent, string _nameLabel, bool state, float fontScale) {
            AutoMatchChildPanel panel = parent.AddUIComponent<AutoMatchChildPanel>();
            panel.autoLayoutDirection = LayoutDirection.Horizontal;
            UILabel nameLabel = panel.AddUIComponent<UILabel>();
            UILabel stateLabel = panel.AddUIComponent<UILabel>();
            nameLabel.wordWrap = false;
            nameLabel.textScale = fontScale;
            nameLabel.text = $"{CommonLocale.OptionPanel_BuiltinFunction}: [" + _nameLabel + "] ";
            stateLabel.wordWrap = false;
            stateLabel.textScale = fontScale;
            stateLabel.textColor = state ? UIColor.Green : UIColor.Red;
            stateLabel.text = state ? CommonLocale.OptionPanel_IsEnabled : CommonLocale.OptionPanel_IsDisabled;
            stateLabel.relativePosition = new Vector3(nameLabel.relativePosition.x + nameLabel.size.x, nameLabel.relativePosition.y);
        }
    }
}
