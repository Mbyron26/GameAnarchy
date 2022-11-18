using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomButton {
        private static Color GreenNormalColor { get; } = new Color32(126, 179, 69, 255);
        private static Color GreenHoveredColor { get; } = new Color32(158, 217, 94, 255);
        public static UIButton AddGreenButton(UIComponent parent, string text, float? width, float? height, Vector2 _relativePosition, OnButtonClicked eventCallback) {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.normalBgSprite = "ButtonWhite";
            button.disabledBgSprite = "ButtonWhite";
            button.hoveredBgSprite = "ButtonWhite";
            button.pressedBgSprite = "ButtonWhite";
            button.textColor = Color.white;
            button.disabledTextColor = Color.black;
            button.hoveredTextColor = Color.white;
            button.pressedTextColor = Color.white;
            button.focusedTextColor = Color.white;
            button.color = GreenNormalColor;
            button.disabledColor = UIColor.ButtonDisabled;
            button.hoveredColor = GreenHoveredColor;
            button.pressedColor = UIColor.ButtonPressed;
            button.focusedColor = GreenNormalColor;
            button.autoSize = false;
            button.textScale = 0.8f;
            button.text = text;
            button.wordWrap = true;
            if (width != null && height != null) {
                button.size = new Vector2((float)width, (float)height);
                button.textHorizontalAlignment = UIHorizontalAlignment.Center;
                button.textVerticalAlignment = UIVerticalAlignment.Middle;
            } else {
                using (UIFontRenderer fontRenderer = button.font.ObtainRenderer()) {
                    Vector2 strSize = fontRenderer.MeasureString(text);
                    button.width = strSize.x + 16f;
                    button.height = 32;
                    button.textHorizontalAlignment = UIHorizontalAlignment.Center;
                    button.textVerticalAlignment = UIVerticalAlignment.Middle;
                }
            }
            button.relativePosition = _relativePosition;
            button.eventClicked += (c, e) => eventCallback();
            return button;
        }
        public static UIButton AddButton(UIComponent parent, float textScale, string text, float? width, float? height, OnButtonClicked eventCallback = null) {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.normalBgSprite = "ButtonWhite";
            button.disabledBgSprite = "ButtonWhite";
            button.hoveredBgSprite = "ButtonWhite";
            button.pressedBgSprite = "ButtonWhite";
            button.textColor = Color.white;
            button.disabledTextColor = Color.black;
            button.hoveredTextColor = Color.white;
            button.pressedTextColor = Color.white;
            button.focusedTextColor = Color.white;
            button.color = UIColor.ButtonNormal;
            button.disabledColor = UIColor.ButtonDisabled;
            button.hoveredColor = UIColor.ButtonHovered;
            button.pressedColor = UIColor.ButtonPressed;
            button.focusedColor = UIColor.ButtonNormal;
            button.autoSize = false;
            button.textScale = textScale;
            button.text = text;
            button.wordWrap = true;
            if (width != null && height != null) {
                button.size = new Vector2((float)width, (float)height);
                button.textHorizontalAlignment = UIHorizontalAlignment.Center;
                button.textVerticalAlignment = UIVerticalAlignment.Middle;
            } else {
                using (UIFontRenderer fontRenderer = button.font.ObtainRenderer()) {
                    Vector2 strSize = fontRenderer.MeasureString(text);
                    button.width = strSize.x + 16f;
                    button.height = 32;
                    button.textHorizontalAlignment = UIHorizontalAlignment.Center;
                    button.textVerticalAlignment = UIVerticalAlignment.Middle;
                }
            }
            if (eventCallback is not null)
                button.eventClicked += (UIComponent c, UIMouseEventParameter sel) => eventCallback();
            return button;
        }
    }
}
