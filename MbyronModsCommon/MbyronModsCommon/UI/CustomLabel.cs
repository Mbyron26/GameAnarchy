using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomLabel {
        public static UILabel AddLabel(UIComponent parent, string text, float? width, RectOffset rectOffset = null, float textScale = 1.0f, Color32? textColor = null, bool wordWrap = true) {
            var label = parent.AddUIComponent<UILabel>();
            label.autoSize = false;
            label.autoHeight = true;
            label.textScale = textScale;
            label.wordWrap = wordWrap;
            if (textColor.HasValue)
                label.textColor = textColor.Value;
            label.processMarkup = true;
            if (rectOffset is not null)
                label.padding = rectOffset;
            label.disabledTextColor = CustomColor.DisabledTextColor;
            label.text = text;
            if (width.HasValue) {
                label.width = width.Value;
            } else {
                using UIFontRenderer fontRenderer = label.ObtainRenderer();
                label.width = fontRenderer.MeasureString(label.text).x;
            }
            return label;
        }
    }
}
