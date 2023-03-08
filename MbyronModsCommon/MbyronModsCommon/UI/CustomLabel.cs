using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomLabel {
        public static RectOffset DefaultOffset { get; } = new(0, 0, 0, 0);
        public static UILabel AddLabel(UIComponent parent, string text, float width, RectOffset rectOffset = null, float textScale = 1.0f, Color32 color = default) {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.autoSize = false;
            label.autoHeight = true;
            label.width = width;
            label.textScale = textScale;
            label.wordWrap = true;
            label.textColor = color;
            label.text = text;
            if (rectOffset is not null)
                label.padding = rectOffset;
            label.disabledTextColor = new Color32(71, 71, 71, 255);
            return label;
        }
        [Obsolete]
        public static AutoMatchChildPanel AddBuiltinFunctionLabel(UIComponent parent, string _nameLabel, bool state, float fontScale) {
            AutoMatchChildPanel panel = parent.AddUIComponent<AutoMatchChildPanel>();
            panel.autoLayoutDirection = LayoutDirection.Horizontal;
            UILabel nameLabel = panel.AddUIComponent<UILabel>();
            UILabel stateLabel = panel.AddUIComponent<UILabel>();
            nameLabel.wordWrap = false;
            nameLabel.textScale = fontScale;
            nameLabel.text = $"{CommonLocalize.OptionPanel_BuiltinFunction}: [" + _nameLabel + "] ";
            stateLabel.wordWrap = false;
            stateLabel.textScale = fontScale;
            stateLabel.textColor = state ? UIColor.Green : UIColor.Red;
            stateLabel.text = state ? CommonLocalize.OptionPanel_IsEnabled : CommonLocalize.OptionPanel_IsDisabled;
            stateLabel.relativePosition = new Vector3(nameLabel.relativePosition.x + nameLabel.size.x, nameLabel.relativePosition.y);
            return panel;
        }

        public static UILabel AddBuiltinFunctionLabel(UIComponent parent, float width, string text, bool state, float fontScale, RectOffset rectOffset) {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.autoSize = false;
            label.width = width;
            label.autoHeight = true;
            label.wordWrap = true;
            label.textScale = fontScale;
            label.padding = rectOffset;
            label.processMarkup = true;
            label.text = state ? $"{CommonLocalize.OptionPanel_BuiltinFunction}: [{text}] <color #6DE007>{CommonLocalize.OptionPanel_IsEnabled}</color>" : $"{CommonLocalize.OptionPanel_BuiltinFunction}: [{text}] <color #C80000>{CommonLocalize.OptionPanel_IsDisabled}</color>";
            return label;
        }
    }
}
