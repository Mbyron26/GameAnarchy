using ColossalFramework.UI;
using ICities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomButton {
        public static PairButton AddPairButton(UIComponent parent, string leftText, string rightText, bool defaultState, float width, float height, Action<int> callback) {
            var button = parent.AddUIComponent<PairButton>();
            button.ButtonSize = new Vector2(width, height);
            button.ButtonAtlas = CustomAtlas.CommonAtlas;
            button.ButtonLeft.normalBgSprite = CustomAtlas.FieldNormalLeft;
            button.ButtonLeft.disabledBgSprite = CustomAtlas.FieldDisabledLeft;
            button.ButtonLeft.hoveredBgSprite = CustomAtlas.FieldHoveredLeft;
            button.ButtonLeft.focusedBgSprite = CustomAtlas.FieldFocusedLeft;
            button.ButtonRight.normalBgSprite = CustomAtlas.FieldNormalRight;
            button.ButtonRight.disabledBgSprite = CustomAtlas.FieldDisabledRight;
            button.ButtonRight.hoveredBgSprite = CustomAtlas.FieldHoveredRight;
            button.ButtonRight.focusedBgSprite = CustomAtlas.FieldFocusedRight;
            button.ButtonLeft.textPadding = new RectOffset(0, 0, 4, 0);
            button.ButtonRight.textPadding = new RectOffset(0, 0, 4, 0);
            button.LeftButtonsText = leftText;
            button.RightButtonsText = rightText;
            if (defaultState) {
                button.Index = 0;
            } else {
                button.Index = 1;
            }
            button.OnSelectedButtonChanged += callback;
            return button;
        }
        public static ToggleButton AddToggleButton(UIComponent parent, bool isChecked, Action<bool> callback) {
            var button = parent.AddUIComponent<ToggleButton>();
            button.autoSize = false;
            button.height = 20;
            button.width = 36;
            button.SetStyleAlpha();
            button.IsChecked = isChecked;
            button.EventOnCheckedChanged += callback;
            return button;
        }

        public static UIButton AddGreenButton(UIComponent parent, string text, float height, float? width, OnButtonClicked eventCallback) {
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
            button.color = new Color32(126, 179, 69, 255);
            button.disabledColor = new(52, 60, 92, 255);
            button.hoveredColor = new Color32(158, 217, 94, 255);
            button.pressedColor = new Color32(105, 177, 26, 255);
            button.focusedColor = new Color32(170, 93, 46, 255);
            button.autoSize = false;
            button.textScale = 0.9f;
            button.text = text;
            button.wordWrap = true;
            if (width.HasValue) {
                button.size = new Vector2(width.Value, height);
                button.textHorizontalAlignment = UIHorizontalAlignment.Center;
                button.textVerticalAlignment = UIVerticalAlignment.Middle;
            } else {
                using (UIFontRenderer fontRenderer = button.font.ObtainRenderer()) {
                    Vector2 strSize = fontRenderer.MeasureString(text);
                    button.width = strSize.x + 16f;
                    button.height = height;
                    button.textHorizontalAlignment = UIHorizontalAlignment.Center;
                    button.textVerticalAlignment = UIVerticalAlignment.Middle;
                }
            }
            button.eventClicked += (c, e) => eventCallback?.Invoke();
            return button;
        }
        public static UIButton AddButton(UIComponent parent, float textScale, string text, float? width, float? height, OnButtonClicked eventCallback = null) {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.atlas = CustomAtlas.CommonAtlas;
            button.normalBgSprite = CustomAtlas.ButtonNormal;
            button.disabledBgSprite = CustomAtlas.ButtonNormal;
            button.hoveredBgSprite = CustomAtlas.ButtonHovered;
            button.pressedBgSprite = CustomAtlas.ButtonPressed;
            button.textColor = Color.white;
            button.disabledTextColor = Color.black;
            button.hoveredTextColor = Color.white;
            button.pressedTextColor = Color.white;
            button.focusedTextColor = Color.white;
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


    public class ToggleButton : MultiStateButtonBase {
        public override void SetStyleAlpha() {
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

    public class PairButton : UIPanel {
        private UITextureAtlas buttonAtlas;
        protected Vector2 buttonSize = new(80, 20);
        private string leftButtonsText;
        private string rightButtonsText;
        private float textScale = 0.7f;
        public event Action<int> OnSelectedButtonChanged;

        public TabButton ButtonLeft { get; private set; }
        public TabButton ButtonRight { get; private set; }
        protected List<TabButton> Buttons { get; } = new();


        public string LeftButtonsText {
            get => leftButtonsText;
            set {
                if (leftButtonsText != value) {
                    leftButtonsText = value;
                    ButtonLeft.text = leftButtonsText;
                }
            }
        }
        public string RightButtonsText {
            get => rightButtonsText;
            set {
                if (rightButtonsText != value) {
                    rightButtonsText = value;
                    ButtonRight.text = value;
                }
            }
        }
        public Vector2 ButtonSize {
            get => buttonSize;
            set {
                if (buttonSize != value) {
                    buttonSize = value;
                    RefreshLayout();
                }
            }
        }
        public UITextureAtlas ButtonAtlas {
            get => buttonAtlas ?? ButtonLeft.atlas;
            set {
                if (value != ButtonLeft.atlas) {
                    buttonAtlas = value;
                    ButtonLeft.atlas = value;
                    ButtonRight.atlas = value;
                }
            }
        }

        public float TextScale {
            get => textScale;
            set {
                if (textScale != value) {
                    textScale = value;
                    ButtonLeft.textScale = value;
                    ButtonRight.textScale = value;
                }
            }
        }
        public void RefreshLayout() {
            var size = buttonSize;
            ButtonLeft.width = ButtonRight.width = (size.x) / 2;
            ButtonLeft.height = ButtonRight.height = size.y;
            ButtonLeft.relativePosition = Vector2.zero;
            ButtonRight.relativePosition = new Vector2(ButtonLeft.width, 0);
            this.size = size;
        }

        private void SettingButton(TabButton button) {
            button.textHorizontalAlignment = UIHorizontalAlignment.Center;
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.textScale = textScale;
            RefreshLayout();
        }
        protected int index = -1;
        public int Index {
            get => index;
            set {
                if (value != index) {
                    index = value;
                    OnSelectedButtonChanged?.Invoke(value);
                }
            }
        }

        public PairButton() {
            autoLayout = false;
            size = buttonSize;
            ButtonLeft = AddUIComponent<TabButton>();
            ButtonRight = AddUIComponent<TabButton>();
            SettingButton(ButtonLeft);
            SettingButton(ButtonRight);
            Buttons.Add(ButtonLeft);
            Buttons.Add(ButtonRight);
            ButtonLeft.eventClicked += ButtonOnClickedChanged;
            ButtonRight.eventClicked += ButtonOnClickedChanged;
        }

        private void ButtonOnClickedChanged(UIComponent component, UIMouseEventParameter eventParam) {
            if (component is TabButton tabButton)
                Index = Buttons.IndexOf(tabButton);
        }

        public override void Update() {
            base.Update();
            for (int i = 0; i < 2; i++) {
                if (i == Index)
                    Buttons[i].state = UIButton.ButtonState.Focused;
                else if (!Buttons[i].IsHovering)
                    Buttons[i].state = UIButton.ButtonState.Normal;
            }
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

        public abstract void SetStyleAlpha();
    }

}
