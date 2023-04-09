using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomButton {
        public static ToggleButton AddToggleButton(UIComponent parent, bool isChecked, Action<bool> callback) {
            var button = parent.AddUIComponent<ToggleButton>();
            button.autoSize = false;
            button.size = new Vector2(40, 24);
            button.SetStyle();
            button.IsOn = isChecked;
            button.EventCheckChanged += callback;
            return button;
        }

        public static UIButton AddClickButton(UIComponent parent, string text, float? width, float height, OnButtonClicked eventCallback, float textScale = 0.9f) {
            var button = parent.AddUIComponent<UIButton>();
            button.SetDefaultStyle();
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

    public static class UIButtonExtension {
        public static void SetBlueStyle(this UIButton button) {
            button.atlas = CustomAtlas.MbyronModsAtlas;
            button.normalBgSprite = CustomAtlas.RoundedRectangle3;
            button.disabledBgSprite = CustomAtlas.RoundedRectangle3;
            button.hoveredBgSprite = CustomAtlas.RoundedRectangle3;
            button.pressedBgSprite = CustomAtlas.RoundedRectangle3;
            button.color = CustomColor.BlueNormal;
            button.focusedColor = CustomColor.BlueNormal;
            button.hoveredColor = CustomColor.BlueHovered;
            button.pressedColor = CustomColor.BluePressed;
            button.disabledColor = CustomColor.BlueDisabled;
        }

        public static void SetGrayStyle(this UIButton button) {
            button.atlas = CustomAtlas.MbyronModsAtlas;
            button.normalBgSprite = CustomAtlas.RoundedRectangle3;
            button.disabledBgSprite = CustomAtlas.RoundedRectangle3;
            button.hoveredBgSprite = CustomAtlas.RoundedRectangle3;
            button.pressedBgSprite = CustomAtlas.RoundedRectangle3;
            button.color = CustomColor.GrayNormal;
            button.focusedColor = CustomColor.GrayNormal;
            button.hoveredColor = CustomColor.GrayHovered;
            button.pressedColor = CustomColor.GrayPressed;
            button.disabledColor = CustomColor.GrayDisabled;
        }

        public static void SetWarningStyle(this UIButton button) {
            //SetGrayStyle(button);
            SetDefaultStyle(button);
            button.textColor = CustomColor.Red;
            button.focusedTextColor = CustomColor.Red;
            button.hoveredTextColor = CustomColor.Red;
            button.pressedTextColor = CustomColor.Red;
        }

        public static void SetDefaultStyle(this UIButton button) {
            SetDefalutSprite(button);
            button.color = CustomColor.DefaultButtonNormal;
            button.focusedColor = CustomColor.DefaultButtonFocused;
            button.hoveredColor = CustomColor.DefaultButtonHovered;
            button.pressedColor = CustomColor.DefaultButtonPressed;
            button.disabledColor = CustomColor.DefaultButtonDisabled;
        }

        private static void SetDefalutSprite(this UIButton button) {
            button.atlas = CustomAtlas.MbyronModsAtlas;
            button.normalBgSprite = CustomAtlas.RoundedRectangle3;
            button.disabledBgSprite = CustomAtlas.RoundedRectangle3;
            button.hoveredBgSprite = CustomAtlas.RoundedRectangle3;
            button.pressedBgSprite = CustomAtlas.RoundedRectangle3;
            button.focusedBgSprite = CustomAtlas.RoundedRectangle3;
        }

    }

    public class ToggleButton : CustomButtonBase {
        public override void SetStyle() {
            atlas = CustomAtlas.MbyronModsAtlas;
            NormalOffBgSprite = CustomAtlas.ToggleNormalOff;
            HoveredOffBgSprite = CustomAtlas.ToggleHoveredOff;
            PressedOffBgSprite = CustomAtlas.ToggleNormalOff;
            FocusedOffBgSprite = CustomAtlas.ToggleNormalOff;
            DisabledOffBgSprite = CustomAtlas.ToggleDisabledOff;
            NormalOnBgSprite = CustomAtlas.ToggleNormalOn;
            HoveredOnBgSprite = CustomAtlas.ToggleHoveredOn;
            PressedOnBgSprite = CustomAtlas.ToggleNormalOn;
            FocusedOnBgSprite = CustomAtlas.ToggleNormalOn;
            DisabledOnBgSprite = CustomAtlas.ToggleDisabledOn;
        }
    }

    public abstract class CustomButtonBase : UITextComponent {
        protected UITextureAtlas atlas;
        protected bool isOn;
        protected ButtonState state;
        protected SpriteStateE offBgSprites = new();
        protected SpriteStateE onBgSprites = new();
        protected SpriteStateE offFgSprites = new();
        protected SpriteStateE onFgSprites = new();
        protected float scaleFactor = 1f;
        protected RectOffset spritePadding;
        protected UIHorizontalAlignment horizontalAlignment = UIHorizontalAlignment.Center;
        protected UIVerticalAlignment verticalAlignment = UIVerticalAlignment.Middle;
        protected UIMouseButton buttonsMask = UIMouseButton.Left;
        protected Color32 normalOffBgColor = Color.white;
        protected Color32 hoveredOffBgColor = Color.white;
        protected Color32 focusedOffBgColor = Color.white;
        protected Color32 pressedOffBgColor = Color.white;
        protected Color32 disabledOffBgColor = Color.white;
        protected Color32 normalOnBgColor = Color.white;
        protected Color32 hoveredOnBgColor = Color.white;
        protected Color32 pressedOnBgColor = Color.white;
        protected Color32 focusedOnBgColor = Color.white;
        protected Color32 disabledOnBgColor = Color.white;

        protected bool isRenderFg = true;
        protected Color32 normalOffFgColor = Color.white;
        protected Color32 hoveredOffFgColor = Color.white;
        protected Color32 focusedOffFgColor = Color.white;
        protected Color32 pressedOffFgColor = Color.white;
        protected Color32 disabledOffFgColor = Color.white;
        protected Color32 normalOnFgColor = Color.white;
        protected Color32 hoveredOnFgColor = Color.white;
        protected Color32 pressedOnFgColor = Color.white;
        protected Color32 focusedOnFgColor = Color.white;
        protected Color32 disabledOnFgColor = Color.white;

        public event Action<ButtonState> EventStateChanged;
        public event Action<bool> EventCheckChanged;

        public UITextureAtlas Atlas {
            get {
                if (atlas is null) {
                    UIView uiview = GetUIView();
                    if (GetUIView() != null) {
                        atlas = uiview.defaultAtlas;
                    }
                }
                return atlas;
            }
            set {
                if (!Equals(value, atlas)) {
                    atlas = value;
                    Invalidate();
                }
            }
        }

        public bool IsOn {
            get => isOn;
            set {
                if (isOn != value) {
                    OnCheckChanged(value);
                }
            }
        }

        public ButtonState State {
            get => state;
            set {
                if (state != value) {
                    OnStateChanged(value);
                }
            }
        }

        public string NormalOffBgSprite {
            get => offBgSprites.Normal;
            set {
                if (value != offBgSprites.Normal) {
                    offBgSprites.Normal = value;
                    SetDefaultSize(value);
                    Invalidate();
                }
            }
        }

        public string NormalOffFgSprite {
            get => offFgSprites.Normal;
            set {
                if (value != offFgSprites.Normal) {
                    offFgSprites.Normal = value;
                    SetDefaultSize(value);
                    Invalidate();
                }
            }
        }

        public Color32 NormalOffBgColor {
            get => normalOffBgColor;
            set {
                normalOffBgColor = value;
                Invalidate();
            }
        }

        public Color32 NormalOffFgColor {
            get => normalOffFgColor;
            set {
                normalOffFgColor = value;
                Invalidate();
            }
        }

        public string HoveredOffBgSprite {
            get => offBgSprites.Hovered;
            set {
                if (value != offBgSprites.Hovered) {
                    offBgSprites.Hovered = value;
                    Invalidate();
                }
            }
        }

        public string HoveredOffFgSprite {
            get => offFgSprites.Hovered;
            set {
                if (value != offFgSprites.Hovered) {
                    offFgSprites.Hovered = value;
                    Invalidate();
                }
            }
        }

        public Color32 HoveredOffBgColor {
            get => hoveredOffBgColor;
            set {
                hoveredOffBgColor = value;
                Invalidate();
            }
        }

        public Color32 HoveredOffFgColor {
            get => hoveredOffFgColor;
            set {
                hoveredOffFgColor = value;
                Invalidate();
            }
        }

        public string PressedOffBgSprite {
            get => offBgSprites.Pressed;
            set {
                if (value != offBgSprites.Pressed) {
                    offBgSprites.Pressed = value;
                    Invalidate();
                }
            }
        }

        public string PressedOffFgSprite {
            get => offFgSprites.Pressed;
            set {
                if (value != offFgSprites.Pressed) {
                    offFgSprites.Pressed = value;
                    Invalidate();
                }
            }
        }

        public Color32 PressedOffBgColor {
            get => pressedOffBgColor;
            set {
                pressedOffBgColor = value;
                Invalidate();
            }
        }

        public Color32 PressedOffFgColor {
            get => pressedOffFgColor;
            set {
                pressedOffFgColor = value;
                Invalidate();
            }
        }

        public string FocusedOffBgSprite {
            get => offBgSprites.Focused;
            set {
                if (value != offBgSprites.Focused) {
                    offBgSprites.Focused = value;
                    Invalidate();
                }
            }
        }

        public string FocusedOffFgSprite {
            get => offFgSprites.Focused;
            set {
                if (value != offFgSprites.Focused) {
                    offFgSprites.Focused = value;
                    Invalidate();
                }
            }
        }

        public Color32 FocusedOffBgColor {
            get => focusedOffBgColor;
            set {
                focusedOffBgColor = value;
                Invalidate();
            }
        }

        public Color32 FocusedOffFgColor {
            get => focusedOffFgColor;
            set {
                focusedOffFgColor = value;
                Invalidate();
            }
        }

        public string DisabledOffBgSprite {
            get => offBgSprites.Disabled;
            set {
                if (value != offBgSprites.Disabled) {
                    offBgSprites.Disabled = value;
                    Invalidate();
                }
            }
        }

        public string DisabledOffFgSprite {
            get => offFgSprites.Disabled;
            set {
                if (value != offFgSprites.Disabled) {
                    offFgSprites.Disabled = value;
                    Invalidate();
                }
            }
        }

        public Color32 DisabledOffBgColor {
            get => disabledOffBgColor;
            set {
                disabledOffBgColor = value;
                Invalidate();
            }
        }

        public Color32 DisabledOffFgColor {
            get => disabledOffFgColor;
            set {
                disabledOffFgColor = value;
                Invalidate();
            }
        }

        public string NormalOnBgSprite {
            get => onBgSprites.Normal;
            set {
                if (value != onBgSprites.Normal) {
                    onBgSprites.Normal = value;
                    SetDefaultSize(value);
                    Invalidate();
                }
            }
        }

        public string NormalOnFgSprite {
            get => onFgSprites.Normal;
            set {
                if (value != onFgSprites.Normal) {
                    onFgSprites.Normal = value;
                    SetDefaultSize(value);
                    Invalidate();
                }
            }
        }

        public Color32 NormalOnBgColor {
            get => normalOnBgColor;
            set {
                normalOnBgColor = value;
                Invalidate();
            }
        }

        public Color32 NormalOnFgColor {
            get => normalOnFgColor;
            set {
                normalOnFgColor = value;
                Invalidate();
            }
        }

        public string HoveredOnBgSprite {
            get => onBgSprites.Hovered;
            set {
                if (value != onBgSprites.Hovered) {
                    onBgSprites.Hovered = value;
                    Invalidate();
                }
            }
        }

        public string HoveredOnFgSprite {
            get => onFgSprites.Hovered;
            set {
                if (value != onFgSprites.Hovered) {
                    onFgSprites.Hovered = value;
                    Invalidate();
                }
            }
        }

        public Color32 HoveredOnBgColor {
            get => hoveredOnBgColor;
            set {
                hoveredOnBgColor = value;
                Invalidate();
            }
        }

        public Color32 HoveredOnFgColor {
            get => hoveredOnFgColor;
            set {
                hoveredOnFgColor = value;
                Invalidate();
            }
        }

        public string PressedOnBgSprite {
            get => onBgSprites.Pressed;
            set {
                if (value != onBgSprites.Pressed) {
                    onBgSprites.Pressed = value;
                    Invalidate();
                }
            }
        }

        public string PressedOnFgSprite {
            get => onFgSprites.Pressed;
            set {
                if (value != onFgSprites.Pressed) {
                    onFgSprites.Pressed = value;
                    Invalidate();
                }
            }
        }

        public Color32 PressedOnBgColor {
            get => pressedOnBgColor;
            set {
                pressedOnBgColor = value;
                Invalidate();
            }
        }

        public Color32 PressedOnFgColor {
            get => pressedOnFgColor;
            set {
                pressedOnFgColor = value;
                Invalidate();
            }
        }

        public string FocusedOnBgSprite {
            get => onBgSprites.Focused;
            set {
                if (value != onBgSprites.Focused) {
                    onBgSprites.Focused = value;
                    Invalidate();
                }
            }
        }

        public string FocusedOnFgSprite {
            get => onFgSprites.Focused;
            set {
                if (value != onFgSprites.Focused) {
                    onFgSprites.Focused = value;
                    Invalidate();
                }
            }
        }

        public Color32 FocusedOnBgColor {
            get => focusedOnBgColor;
            set {
                focusedOnBgColor = value;
                Invalidate();
            }
        }

        public Color32 FocusedOnFgColor {
            get => focusedOnFgColor;
            set {
                focusedOnFgColor = value;
                Invalidate();
            }
        }

        public string DisabledOnBgSprite {
            get => onBgSprites.Disabled;
            set {
                if (value != onBgSprites.Disabled) {
                    onBgSprites.Disabled = value;
                    Invalidate();
                }
            }
        }

        public string DisabledOnFgSprite {
            get => onFgSprites.Disabled;
            set {
                if (value != onFgSprites.Disabled) {
                    onFgSprites.Disabled = value;
                    Invalidate();
                }
            }
        }

        public Color32 DisabledOnBgColor {
            get => disabledOnBgColor;
            set {
                disabledOnBgColor = value;
                Invalidate();
            }
        }

        public Color32 DisabledOnFgColor {
            get => disabledOnFgColor;
            set {
                disabledOnFgColor = value;
                Invalidate();
            }
        }

        public bool IsRenderFg {
            get => isRenderFg;
            set => isRenderFg = value;
        }

        public float ScaleFactor {
            get => scaleFactor;
            set {
                if (!Mathf.Approximately(value, scaleFactor)) {
                    scaleFactor = value;
                    Invalidate();
                }
            }
        }

        public RectOffset SpritePadding {
            get {
                spritePadding ??= new RectOffset();
                return spritePadding;
            }
            set {
                if (!Equals(value, spritePadding)) {
                    spritePadding = value;
                    Invalidate();
                }
            }
        }

        public UIMouseButton ButtonsMask {
            get => buttonsMask;
            set => buttonsMask = value;
        }

        public virtual UIHorizontalAlignment HorizontalAlignment {
            get => horizontalAlignment;
            set {
                if (value != horizontalAlignment) {
                    horizontalAlignment = value;
                    Invalidate();
                }
            }
        }

        public virtual UIVerticalAlignment VerticalAlignment {
            get => verticalAlignment;
            set {
                if (value != verticalAlignment) {
                    verticalAlignment = value;
                    Invalidate();
                }
            }
        }

        public override bool canFocus => (isEnabled && isVisible) || base.canFocus;

        public abstract void SetStyle();

        protected virtual void OnStateChanged(ButtonState value) {
            if (!isEnabled && value != ButtonState.Disabled) {
                return;
            }
            state = value;
            EventStateChanged?.Invoke(value);
            Invalidate();
        }

        protected virtual void OnCheckChanged(bool value) {
            if (!isEnabled) {
                return;
            }
            isOn = value;
            EventCheckChanged?.Invoke(value);
            Invalidate();
        }

        protected virtual void SetDefaultSize(string spriteName) {
            if (atlas is null) {
                return;
            }
            UITextureAtlas.SpriteInfo spriteInfo = atlas[spriteName];
            if (m_Size == Vector2.zero && spriteInfo != null) {
                size = spriteInfo.pixelSize;
            }
        }

        public override Vector2 CalculateMinimumSize() {
            UITextureAtlas.SpriteInfo backgroundSprite = GetBgSprite();
            if (backgroundSprite == null) {
                return base.CalculateMinimumSize();
            }
            RectOffset border = backgroundSprite.border;
            if (border.horizontal > 0 || border.vertical > 0) {
                return Vector2.Max(base.CalculateMinimumSize(), new Vector2((float)border.horizontal, (float)border.vertical));
            }
            return base.CalculateMinimumSize();
        }

        protected override void OnRebuildRenderData() {
            if (atlas is null) {
                return;
            }
            renderData.material = atlas.material;
            RenderBgSprite();
            RenderFgSprite();
        }

        protected virtual void RenderFgSprite() {
            if (!IsRenderFg) {
                return;
            }
            if (atlas is null) {
                return;
            }
            var fgSprite = GetFgSprite();
            if (fgSprite is null) {
                return;
            }
            Color32 color = ApplyOpacity(GetFgActiveColor());
            RenderOptions options = new() {
                atlas = atlas,
                color = color,
                fillAmount = 1f,
                flip = UISpriteFlip.None,
                offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
                pixelsToUnits = PixelsToUnits(),
                size = size,
                spriteInfo = fgSprite
            };
            if (fgSprite.isSliced) {
                Render.UISlicedSpriteRender.RenderSprite(renderData, options);
                return;
            }
            Render.UISlicedSpriteRender.RenderSprite(renderData, options);
        }

        protected virtual void RenderBgSprite() {
            if (atlas is null) {
                return;
            }
            UITextureAtlas.SpriteInfo sprite = GetBgSprite();
            if (sprite is null) {
                return;
            }
            Color32 color = ApplyOpacity(GetBgActiveColor());
            RenderOptions options = new() {
                atlas = atlas,
                color = color,
                fillAmount = 1f,
                flip = UISpriteFlip.None,
                offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
                pixelsToUnits = PixelsToUnits(),
                size = size,
                spriteInfo = sprite
            };
            if (sprite.isSliced) {
                Render.UISlicedSpriteRender.RenderSprite(renderData, options);
                return;
            }
            Render.UISpriteRender.RenderSprite(renderData, options);
        }

        protected virtual Color32 GetFgActiveColor() {
            if (IsOn) {
                return State switch {
                    ButtonState.Focused => FocusedOnFgColor,
                    ButtonState.Hovered => HoveredOnFgColor,
                    ButtonState.Pressed => PressedOnFgColor,
                    ButtonState.Disabled => DisabledOnFgColor,
                    _ => NormalOnFgColor,
                };
            } else {
                return State switch {
                    ButtonState.Focused => FocusedOffFgColor,
                    ButtonState.Hovered => HoveredOffFgColor,
                    ButtonState.Pressed => PressedOffFgColor,
                    ButtonState.Disabled => DisabledOffFgColor,
                    _ => NormalOffFgColor,
                };
            }
        }

        protected virtual Color32 GetBgActiveColor() {
            if (IsOn) {
                return State switch {
                    ButtonState.Focused => FocusedOnBgColor,
                    ButtonState.Hovered => HoveredOnBgColor,
                    ButtonState.Pressed => PressedOnBgColor,
                    ButtonState.Disabled => DisabledOnBgColor,
                    _ => NormalOnBgColor,
                };
            } else {
                return State switch {
                    ButtonState.Focused => FocusedOffBgColor,
                    ButtonState.Hovered => HoveredOffBgColor,
                    ButtonState.Pressed => PressedOffBgColor,
                    ButtonState.Disabled => DisabledOffBgColor,
                    _ => NormalOffBgColor,
                };
            }
        }

        protected virtual UITextureAtlas.SpriteInfo GetFgSprite() {
            if (atlas is null) {
                return null;
            }
            UITextureAtlas.SpriteInfo spriteInfo = null;
            if (IsOn) {
                switch (State) {
                    case ButtonState.Normal:
                        spriteInfo = atlas[NormalOnFgSprite];
                        break;
                    case ButtonState.Focused:
                        spriteInfo = atlas[FocusedOnFgSprite];
                        break;
                    case ButtonState.Hovered:
                        spriteInfo = atlas[HoveredOnFgSprite];
                        break;
                    case ButtonState.Pressed:
                        spriteInfo = atlas[PressedOnFgSprite];
                        break;
                    case ButtonState.Disabled:
                        spriteInfo = atlas[DisabledOnFgSprite];
                        break;
                }
            } else {
                switch (State) {
                    case ButtonState.Normal:
                        spriteInfo = atlas[NormalOffFgSprite];
                        break;
                    case ButtonState.Focused:
                        spriteInfo = atlas[FocusedOffFgSprite];
                        break;
                    case ButtonState.Hovered:
                        spriteInfo = atlas[HoveredOffFgSprite];
                        break;
                    case ButtonState.Pressed:
                        spriteInfo = atlas[PressedOffFgSprite];
                        break;
                    case ButtonState.Disabled:
                        spriteInfo = atlas[DisabledOffFgSprite];
                        break;
                }
            }
            if (spriteInfo == null) {
                spriteInfo = atlas[NormalOnFgSprite];
            }
            return spriteInfo;
        }

        protected virtual UITextureAtlas.SpriteInfo GetBgSprite() {
            if (atlas is null) {
                return null;
            }
            UITextureAtlas.SpriteInfo spriteInfo = null;
            if (IsOn) {
                switch (State) {
                    case ButtonState.Normal:
                        spriteInfo = atlas[NormalOnBgSprite];
                        break;
                    case ButtonState.Focused:
                        spriteInfo = atlas[FocusedOnBgSprite];
                        break;
                    case ButtonState.Hovered:
                        spriteInfo = atlas[HoveredOnBgSprite];
                        break;
                    case ButtonState.Pressed:
                        spriteInfo = atlas[PressedOnBgSprite];
                        break;
                    case ButtonState.Disabled:
                        spriteInfo = atlas[DisabledOnBgSprite];
                        break;
                }
            } else {
                switch (State) {
                    case ButtonState.Normal:
                        spriteInfo = atlas[NormalOffBgSprite];
                        break;
                    case ButtonState.Focused:
                        spriteInfo = atlas[FocusedOffBgSprite];
                        break;
                    case ButtonState.Hovered:
                        spriteInfo = atlas[HoveredOffBgSprite];
                        break;
                    case ButtonState.Pressed:
                        spriteInfo = atlas[PressedOffBgSprite];
                        break;
                    case ButtonState.Disabled:
                        spriteInfo = atlas[DisabledOffBgSprite];
                        break;
                }
            }
            if (spriteInfo == null) {
                spriteInfo = atlas[NormalOnBgSprite];
            }
            return spriteInfo;
        }

        protected override void OnIsEnabledChanged() {
            if (!isEnabled) {
                State = ButtonState.Disabled;
            } else {
                State = ButtonState.Normal;
            }
            base.OnIsEnabledChanged();
        }

        protected override void OnEnterFocus(UIFocusEventParameter p) {
            if (State != ButtonState.Pressed) {
                State = ButtonState.Focused;
            }
            base.OnEnterFocus(p);
        }

        protected override void OnGotFocus(UIFocusEventParameter p) {
            base.OnGotFocus(p);
            Invalidate();
        }

        protected override void OnLostFocus(UIFocusEventParameter p) {
            base.OnLostFocus(p);
            Invalidate();
        }

        protected override void OnLeaveFocus(UIFocusEventParameter p) {
            State = (containsMouse ? ButtonState.Hovered : ButtonState.Normal);
            base.OnLeaveFocus(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p) {
            if (m_IsMouseHovering) {
                State = ButtonState.Hovered;
            } else {
                State = ButtonState.Normal;
            }
            base.OnMouseUp(p);
        }

        protected override void OnMouseDown(UIMouseEventParameter p) {
            if ((p.buttons & buttonsMask) != UIMouseButton.None) {
                if (isEnabled && State != ButtonState.Focused) {
                    State = ButtonState.Pressed;
                }
                base.OnMouseDown(p);
            }
        }

        protected override void OnMouseEnter(UIMouseEventParameter p) {
            if (isEnabled) {
                State = ButtonState.Hovered;
            }
            Invalidate();
            base.OnMouseEnter(p);
        }

        protected override void OnMouseLeave(UIMouseEventParameter p) {
            if (isEnabled) {
                State = ButtonState.Normal;
            }
            Invalidate();
            base.OnMouseLeave(p);
        }

        protected override void OnClick(UIMouseEventParameter p) {
            if (!p.used) {
                IsOn = !IsOn;

            }
            base.OnClick(p);
        }

    }

    public class SpriteStateE {
        public string Normal = "";
        public string Hovered = "";
        public string Pressed = "";
        public string Focused = "";
        public string Disabled = "";
    }

    public enum ButtonState {
        Normal,
        Hovered,
        Pressed,
        Focused,
        Disabled
    }

}
