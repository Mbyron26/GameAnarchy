using ColossalFramework.UI;
using ICities;

namespace MbyronModsCommon.UI {
    public class CustomCheckBox {
        public static UICheckBox AddCheckBox(UIComponent parent, string text, bool defaultValue, float? width, OnCheckChanged eventCallback, float textScale = 1f, string tooltip = null) {
            UICheckBox checkBox = (UICheckBox)parent.AttachUIComponent(UITemplateManager.GetAsGameObject(@"OptionsCheckBoxTemplate"));
            checkBox.text = text;
            checkBox.isChecked = defaultValue;
            checkBox.label.autoSize = false;
            if (width != null) {
                checkBox.width = width.Value;
                checkBox.label.width = width.Value - 30;
            }
            checkBox.label.wordWrap = true;
            checkBox.label.autoHeight = true;
            checkBox.eventCheckChanged += (c, isChecked) => eventCallback(isChecked);
            checkBox.label.textScale = textScale;
            if (!string.IsNullOrEmpty(tooltip))
                checkBox.tooltip = tooltip;
            return checkBox;
        }
    }
}
