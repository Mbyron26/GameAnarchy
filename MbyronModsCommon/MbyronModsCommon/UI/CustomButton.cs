using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomButton {
        public static ToggleButton AddToggleButton(UIComponent parent, bool isChecked, Action<bool> callback) {
            var button = parent.AddUIComponent<ToggleButton>();
            button.autoSize = false;
            button.height = 20;
            button.width = 36;
            button.SetSprite();
            button.IsChecked = isChecked;
            button.EventOnCheckedChanged += callback;
            return button;
        }

        public static UIButton AddClickButton(UIComponent parent, string text, float? width, float height, OnButtonClicked eventCallback, float textScale = 0.9f, bool isBlueStyle = false) {
            var button = parent.AddUIComponent<UIButton>();
            if (!isBlueStyle) {
                button.atlas = CustomAtlas.InGameAtlas;
                button.normalBgSprite = "ButtonWhite";
                button.disabledBgSprite = "ButtonWhite";
                button.hoveredBgSprite = "ButtonWhite";
                button.pressedBgSprite = "ButtonWhite";
                button.color = CustomColor.GreenNormal;
                button.disabledColor = CustomColor.GreenDisabled;
                button.hoveredColor = CustomColor.GreenHovered;
                button.pressedColor = CustomColor.GreenPressed;
                button.focusedColor = CustomColor.Orange;
            } else {
                button.atlas = CustomAtlas.CommonAtlas;
                button.normalBgSprite = CustomAtlas.ButtonNormal;
                button.disabledBgSprite = CustomAtlas.ButtonNormal;
                button.hoveredBgSprite = CustomAtlas.ButtonHovered;
                button.pressedBgSprite = CustomAtlas.ButtonPressed;
            }
            button.disabledTextColor = CustomColor.DisabledTextColor;
            button.autoSize = false;
            button.wordWrap = true;
            button.textScale = textScale;
            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.height = height;
            button.text = text;
            if (width.HasValue) {
                button.width = width.Value;
            } else {
                using UIFontRenderer fontRenderer = button.font.ObtainRenderer();
                button.width = fontRenderer.MeasureString(text).x + 16f;
            }
            button.eventClicked += (c, e) => eventCallback?.Invoke();
            return button;
        }

    }

    public class ToggleButton : MultiStateButtonBase {
        public override void SetSprite() {
            atlas = CustomAtlas.CommonAtlas;
            FgSpriteSet0.normal = CustomAtlas.ToggleButtonFGZeroNormal;
            FgSpriteSet0.focused = CustomAtlas.ToggleButtonFGZeroNormal;
            FgSpriteSet0.hovered = CustomAtlas.ToggleButtonFGZeroNormal;
            FgSpriteSet0.pressed = CustomAtlas.ToggleButtonFGZeroNormal;
            FgSpriteSet0.disabled = CustomAtlas.ToggleButtonFGZeroDisabled;
            BgSpriteSet0.normal = CustomAtlas.ToggleButtonBGZeroNormal;
            BgSpriteSet0.focused = CustomAtlas.ToggleButtonBGZeroNormal;
            BgSpriteSet0.hovered = CustomAtlas.ToggleButtonBGZeroHovered;
            BgSpriteSet0.pressed = CustomAtlas.ToggleButtonBGZeroNormal;
            BgSpriteSet0.disabled = CustomAtlas.ToggleButtonBGZeroDisabled;
            FgSpriteSet1.normal = CustomAtlas.ToggleButtonFGOneNormal;
            FgSpriteSet1.focused = CustomAtlas.ToggleButtonFGOneNormal;
            FgSpriteSet1.hovered = CustomAtlas.ToggleButtonFGOneNormal;
            FgSpriteSet1.pressed = CustomAtlas.ToggleButtonFGOneNormal;
            FgSpriteSet1.disabled = CustomAtlas.ToggleButtonFGOneDisabled;
            BgSpriteSet1.normal = CustomAtlas.ToggleButtonBGOneNormal;
            BgSpriteSet1.focused = CustomAtlas.ToggleButtonBGOneNormal;
            BgSpriteSet1.hovered = CustomAtlas.ToggleButtonBGOneHovered;
            BgSpriteSet1.pressed = CustomAtlas.ToggleButtonBGOneNormal;
            BgSpriteSet1.disabled = CustomAtlas.ToggleButtonBGOneDisabled;
        }
    }

    public abstract class MultiStateButtonBase : UIMultiStateButton {
        public event Action<bool> EventOnCheckedChanged;

        public SpriteSetState FgSpriteSetState => foregroundSprites;
        public SpriteSetState BgSpriteSetState => backgroundSprites;

        public SpriteSet FgSpriteSet0 {
            get {
                if (FgSpriteSetState[0] is null) {
                    FgSpriteSetState.AddState();
                }
                return FgSpriteSetState[0];
            }
        }

        public SpriteSet BgSpriteSet0 {
            get {
                if (BgSpriteSetState[0] is null) {
                    BgSpriteSetState.AddState();
                }
                return BgSpriteSetState[0];
            }
        }

        public SpriteSet FgSpriteSet1 {
            get {
                if (FgSpriteSetState.Count == 1) {
                    FgSpriteSetState.AddState();
                }
                return FgSpriteSetState[1];
            }
        }

        public SpriteSet BgSpriteSet1 {
            get {
                if (BgSpriteSetState.Count == 1) {
                    BgSpriteSetState.AddState();
                }
                return BgSpriteSetState[1];
            }
        }

        public virtual bool IsChecked {
            get => activeStateIndex == 1;
            set {
                if (value) {
                    activeStateIndex = 1;
                } else {
                    activeStateIndex = 0;
                }
            }
        }

        public MultiStateButtonBase() {
            state = ButtonState.Normal;
            foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            spritePadding = new RectOffset(0, 0, 0, 0);
            canFocus = false;
            isInteractive = true;
            eventActiveStateIndexChanged += OnEventActiveStateIndexChanged;
        }

        protected virtual void OnEventActiveStateIndexChanged(UIComponent component, int value) => EventOnCheckedChanged?.Invoke(value != 0);

        public abstract void SetSprite();
    }

}
