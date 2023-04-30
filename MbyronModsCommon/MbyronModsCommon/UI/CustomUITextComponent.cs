using System;
using UnityEngine;
using ColossalFramework.UI;
namespace MbyronModsCommon.UI;

public abstract class CustomUITextComponent : UIComponent {
    protected UITextureAtlas atlas;
    protected UIRenderData textRenderData;
    protected UIFont font;
    protected string text = string.Empty;
    protected float textScale = 1f;
    protected RectOffset textPadding = new();
    protected UITextScaleMode textScaleMode;
    protected int characterSpacing;
    protected bool processMarkup;
    protected Color32 textNormalColor = Color.white;
    protected Color32 textHoveredColor = Color.white;
    protected Color32 textPressedColor = Color.white;
    protected Color32 textFocusedColor = Color.white;
    protected Color32 textDisabledColor = CustomUIColor.DisabledTextColor;
    protected bool useOutline;
    protected int outlineSize = 1;
    protected Color32 outlineColor;
    protected bool useTextGradient;
    protected Color32 gradientBottomNormalColor = Color.white;
    protected Color32 gradientBottomDisabledColor = Color.white;
    protected bool useDropShadow;
    protected Color32 dropShadowColor;
    protected Vector2 dropShadowOffset;
    protected Vector2 startSize;
    private bool isFontCallbackAssigned;
    protected bool colorizeSprites;
    protected UIHorizontalAlignment textHorizontalAlignment = UIHorizontalAlignment.Center;
    protected UIVerticalAlignment textVerticalAlignment = UIVerticalAlignment.Middle;
    protected bool wordWrap;

    public UITextureAtlas Atlas {
        get {
            if (atlas is null) {
                UIView uiview = GetUIView();
                if (uiview is not null) {
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
    public UIFont Font {
        get {
            if (font is null) {
                UIView uiview = GetUIView();
                if (uiview is not null) {
                    font = uiview.defaultFont;
                }
            }
            return font;
        }
        set {
            if (value != font) {
                UnbindTextureRebuildCallback();
                font = value;
                BindTextureRebuildCallback();
                Invalidate();
            }
        }
    }
    public virtual string Text {
        get => text;
        set {
            if (value != text) {
                UIFontManager.Invalidate(Font);
                text = value;
                Invalidate();
            }
        }
    }
    public float TextScale {
        get => textScale;
        set {
            value = Mathf.Max(0.1f, value);
            if (!Mathf.Approximately(textScale, value)) {
                UIFontManager.Invalidate(Font);
                textScale = value;
                Invalidate();
            }
        }
    }
    public RectOffset TextPadding {
        get => textPadding;
        set {
            if (!Equals(value, textPadding)) {
                textPadding = value;
                Invalidate();
            }
        }
    }
    public UITextScaleMode TextScaleMode {
        get => textScaleMode;
        set {
            if (value != textScaleMode) {
                textScaleMode = value;
                Invalidate();
            }
        }
    }
    public int CharacterSpacing {
        get => characterSpacing;
        set {
            value = Mathf.Max(0, value);
            if (value != characterSpacing) {
                characterSpacing = value;
                Invalidate();
            }
        }
    }
    public bool ProcessMarkup {
        get => processMarkup;
        set {
            if (value != processMarkup) {
                processMarkup = value;
                Invalidate();
            }
        }
    }
    public Color32 TextNormalColor {
        get => textNormalColor;
        set {
            textNormalColor = value;
            Invalidate();
        }
    }
    public Color32 TextHoveredColor {
        get => textHoveredColor;
        set {
            textHoveredColor = value;
            Invalidate();
        }
    }
    public Color32 TextPressedColor {
        get => textPressedColor;
        set {
            textPressedColor = value;
            Invalidate();
        }
    }
    public Color32 TextFocusedColor {
        get => textFocusedColor;
        set {
            textFocusedColor = value;
            Invalidate();
        }
    }
    public Color32 TextDisabledColor {
        get => textDisabledColor;
        set {
            textDisabledColor = value;
            Invalidate();
        }
    }
    public bool UseOutline {
        get => useOutline;
        set {
            if (value != useOutline) {
                useOutline = value;
                Invalidate();
            }
        }
    }
    public int OutlineSize {
        get => outlineSize;
        set {
            value = Mathf.Max(0, value);
            if (value != outlineSize) {
                outlineSize = value;
                Invalidate();
            }
        }
    }
    public Color32 OutlineColor {
        get => outlineColor;
        set {
            if (!value.Equals(outlineColor)) {
                outlineColor = value;
                Invalidate();
            }
        }
    }
    public bool UseTextGradient {
        get => useTextGradient;
        set {
            if (value != useTextGradient) {
                useTextGradient = value;
                Invalidate();
            }
        }
    }
    public Color32 GradientBottomNormalColor {
        get => gradientBottomNormalColor;
        set {
            if (!gradientBottomNormalColor.Equals(value)) {
                gradientBottomNormalColor = value;
                OnColorChanged();
            }
        }
    }
    public Color32 GradientBottomDisabledColor {
        get => gradientBottomDisabledColor;
        set {
            if (!gradientBottomDisabledColor.Equals(value)) {
                gradientBottomDisabledColor = value;
                OnColorChanged();
            }
        }
    }
    public bool UseDropShadow {
        get => useDropShadow;
        set {
            if (value != useDropShadow) {
                useDropShadow = value;
                Invalidate();
            }
        }
    }
    public Color32 DropShadowColor {
        get => dropShadowColor;
        set {
            if (!value.Equals(dropShadowColor)) {
                dropShadowColor = value;
                Invalidate();
            }
        }
    }
    public Vector2 DropShadowOffset {
        get => dropShadowOffset;
        set {
            if (value != dropShadowOffset) {
                dropShadowOffset = value;
                Invalidate();
            }
        }
    }
    public bool ColorizeSprites {
        get => colorizeSprites;
        set {
            if (value != colorizeSprites) {
                colorizeSprites = value;
                Invalidate();
            }
        }
    }
    public UIHorizontalAlignment TextHorizontalAlignment {
        get {
            if (autoSize) {
                return UIHorizontalAlignment.Left;
            }
            return textHorizontalAlignment;
        }
        set {
            if (value != textHorizontalAlignment) {
                textHorizontalAlignment = value;
                Invalidate();
            }
        }
    }
    public virtual UIVerticalAlignment TextVerticalAlignment {
        get => textVerticalAlignment;
        set {
            if (value != textVerticalAlignment) {
                textVerticalAlignment = value;
                Invalidate();
            }
        }
    }
    public bool WordWrap {
        get => wordWrap;
        set {
            if (value != wordWrap) {
                wordWrap = value;
                Invalidate();
            }
        }
    }

    public override void Awake() {
        base.Awake();
        startSize = size;
    }
    public override void OnEnable() {
        base.OnEnable();
        BindTextureRebuildCallback();
    }
    public override void OnDisable() {
        base.OnDisable();
        UnbindTextureRebuildCallback();
    }

    protected virtual Color32 GetTextColor() => isEnabled ? TextNormalColor : TextDisabledColor;
    protected virtual Color32 GetGradientBottomColorForState() => isEnabled ? GradientBottomNormalColor : GradientBottomDisabledColor;
    protected virtual float GetTextScaleMultiplier() {
        if (TextScaleMode == UITextScaleMode.None || !Application.isPlaying) {
            return 1f;
        }
        if (TextScaleMode == UITextScaleMode.ScreenResolution) {
            return (float)Screen.height / GetUIView().fixedHeight;
        }
        return size.y / startSize.y;
    }
    private void BindTextureRebuildCallback() {
        if (isFontCallbackAssigned || Font is null) {
            return;
        }
        UIDynamicFont x = Font as UIDynamicFont;
        if (x is not null) {
            UnityEngine.Font.textureRebuilt += new Action<Font>(OnFontTextureRebuilt);
            isFontCallbackAssigned = true;
        }
    }
    private void UnbindTextureRebuildCallback() {
        if (!isFontCallbackAssigned || Font is null) {
            return;
        }
        UIDynamicFont x = Font as UIDynamicFont;
        if (x is not null) {
            UnityEngine.Font.textureRebuilt -= new Action<Font>(OnFontTextureRebuilt);
        }
        isFontCallbackAssigned = false;
    }
    private void OnFontTextureRebuilt(Font font) {
        RequestCharacterInfo();
        Invalidate();
    }
    public virtual void UpdateFontInfo() => RequestCharacterInfo();
    protected virtual void RequestCharacterInfo() {
        if (Font is not UIDynamicFont uidynamicFont) {
            return;
        }
        if (!UIFontManager.IsDirty(Font)) {
            return;
        }
        if (string.IsNullOrEmpty(Text)) {
            return;
        }
        float num = TextScale * GetTextScaleMultiplier();
        int fontSize = Mathf.CeilToInt(Font.size * num);
        uidynamicFont.AddCharacterRequest(Text, fontSize, FontStyle.Normal);
    }

}

