using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class CustomUIToggleButton : CustomUIButtonBase {
    public CustomUIToggleButton() => buttonType = UIButtonType.Toggle;

    public static CustomUIToggleButton Add(UIComponent parent, bool isChecked, Action<bool> callback, bool opStyle = true) {
        var button = parent.AddUIComponent<CustomUIToggleButton>();
        if (opStyle)
            button.SetOPStyle();
        else 
            button.SetCPStyle();
        button.autoSize = false;
        button.size = new Vector2(40, 24);
        button.IsOn = isChecked;
        button.EventToggleChanged += callback;
        return button;
    }
    public void SetOPStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        renderFg = true;
        offBgSprites.SetSprites(CustomUIAtlas.RoundedRectangle5);
        OffBgSprites.SetColors(CustomUIColor.OPButtonNormal, CustomUIColor.OPButtonHovered, CustomUIColor.OPButtonPressed, CustomUIColor.OPButtonFocused, CustomUIColor.OPButtonDisabled);
        offFgSprites.SetSprites(CustomUIAtlas.ToggleOffFg);
        offFgSprites.SetColors(CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgDisabled);
        onBgSprites.SetSprites(CustomUIAtlas.RoundedRectangle5);
        onBgSprites.SetColors(CustomUIColor.GreenNormal, CustomUIColor.GreenHovered, CustomUIColor.GreenPressed, CustomUIColor.GreenFocused, CustomUIColor.GreenDisabled);
        onFgSprites.SetSprites(CustomUIAtlas.ToggleOnFg);
        onFgSprites.SetColors(CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgDisabled);
        base.SetStyle();
    }
    public void SetCPStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        renderFg = true;
        offBgSprites.SetSprites(CustomUIAtlas.RoundedRectangle5);
        OffBgSprites.SetColors(CustomUIColor.CPButtonNormal, CustomUIColor.CPButtonHovered, CustomUIColor.CPButtonPressed, CustomUIColor.CPButtonFocused, CustomUIColor.CPButtonDisabled);
        offFgSprites.SetSprites(CustomUIAtlas.ToggleOffFg);
        offFgSprites.SetColors(CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgDisabled);
        onBgSprites.SetSprites(CustomUIAtlas.RoundedRectangle5);
        onBgSprites.SetColors(CustomUIColor.GreenNormal, CustomUIColor.GreenHovered, CustomUIColor.GreenPressed, CustomUIColor.GreenFocused, CustomUIColor.GreenDisabled);
        onFgSprites.SetSprites(CustomUIAtlas.ToggleOnFg);
        onFgSprites.SetColors(CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgNormal, CustomUIColor.ToggleFgDisabled);
        base.SetStyle();
    }
}

public class CustomUITabButton : CustomUIButtonBase {
    private readonly string sprite = CustomUIAtlas.RoundedRectangle2;

    public CustomUITabButton() => buttonType = UIButtonType.Tab;

    public void SetDefaultControlPanelStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        onBgSprites.SetSprites(sprite, sprite, sprite, sprite, sprite);
        onBgSprites.SetColors(CustomUIColor.CPPrimaryBg, CustomUIColor.CPButtonHovered, CustomUIColor.CPButtonPressed, CustomUIColor.GreenNormal, CustomUIColor.CPButtonDisabled);
        SetStyle();
    }
    public void SetDefaultOptionPanelStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        onBgSprites.SetSprites(sprite, sprite, sprite, sprite, sprite);
        onBgSprites.SetColors(CustomUIColor.OPPrimaryBg, CustomUIColor.OPButtonNormal, CustomUIColor.OPPrimaryBg, CustomUIColor.BlueNormal, CustomUIColor.OPButtonDisabled);
        SetStyle();
    }
}

public class CustomUIButton : CustomUIButtonBase {
    public static CustomUIButton Add(UIComponent parent, string text, float? width, float height, OnButtonClicked eventCallback, float textScale = 0.9f, bool defaultStyle = true) {
        var button = parent.AddUIComponent<CustomUIButton>();
        if (defaultStyle) {
            button.SetOptionPanelStyle();
        }
        button.autoSize = false;
        button.textScale = textScale;
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
    public void SetControlPanelStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        onBgSprites.SetSprites(CustomUIAtlas.RoundedRectangle3);
        onBgSprites.SetColors(CustomUIColor.CPButtonNormal, CustomUIColor.CPButtonHovered, CustomUIColor.CPButtonPressed, CustomUIColor.CPButtonFocused, CustomUIColor.CPButtonDisabled);
        SetStyle();
    }
    public void SetOptionPanelStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        onBgSprites.SetSprites(CustomUIAtlas.RoundedRectangle3);
        onBgSprites.SetColors(CustomUIColor.OPButtonNormal, CustomUIColor.OPButtonHovered, CustomUIColor.OPButtonPressed, CustomUIColor.OPButtonFocused, CustomUIColor.OPButtonDisabled);
        SetStyle();
    }
}

public abstract class CustomUIButtonBase : CustomUITextComponent {
    protected UIRenderData bgRenderData;
    protected UIRenderData fgRenderData;
    protected bool renderFg;
    protected Sprites offBgSprites = new();
    protected Sprites onBgSprites = new();
    protected Sprites offFgSprites = new();
    protected Sprites onFgSprites = new();
    protected ForegroundSpriteMode fgSpriteMode;
    protected float fgScaleFactor = 1f;
    protected Vector2 fgCustomSize;
    protected UIButtonType buttonType;
    protected bool isOn;
    protected SpriteState state;
    protected UIMouseButton buttonMask = UIMouseButton.Left;
    protected UIHorizontalAlignment horizontalAlignment = UIHorizontalAlignment.Center;
    protected UIVerticalAlignment verticalAlignment = UIVerticalAlignment.Middle;
    protected RectOffset fgSpritePadding;

    public event Action<bool> EventToggleChanged;
    public event Action<SpriteState> EventStateChanged;

    public bool RenderFg {
        get => renderFg;
        set {
            if (!value.Equals(renderFg)) {
                renderFg = value;
                Invalidate();
            }
        }
    }
    public Sprites OffBgSprites => offBgSprites;
    public Sprites OnBgSprites => onBgSprites;
    public Sprites OffFgSprites => offFgSprites;
    public Sprites OnFgSprites => onFgSprites;
    public ForegroundSpriteMode FgSpriteMode {
        get => fgSpriteMode;
        set {
            if (value != fgSpriteMode) {
                fgSpriteMode = value;
                Invalidate();
            }
        }
    }
    public float FgScaleFactor {
        get => fgScaleFactor;
        set {
            if (!Mathf.Approximately(value, fgScaleFactor)) {
                fgScaleFactor = value;
                Invalidate();
            }
        }
    }
    public Vector2 FgCustomSize {
        get => fgCustomSize;
        set {
            if (value != fgCustomSize) {
                fgCustomSize = value;
                Invalidate();
            }
        }
    }
    public string OffBgNormalSprite {
        get => offBgSprites.NormalSprite;
        set {
            if (value != offBgSprites.NormalSprite) {
                offBgSprites.NormalSprite = value;
                SetDefaultSize(value);
                Invalidate();
            }
        }
    }
    public string OffFgNormalSprite {
        get => offFgSprites.NormalSprite;
        set {
            if (value != offFgSprites.NormalSprite) {
                offFgSprites.NormalSprite = value;
                SetDefaultSize(value);
                Invalidate();
            }
        }
    }
    public Color32 OffBgNormalColor {
        get => offBgSprites.NormalColor;
        set {
            offBgSprites.NormalColor = value;
            Invalidate();
        }
    }
    public Color32 OffFgNormalColor {
        get => offFgSprites.NormalColor;
        set {
            offFgSprites.NormalColor = value;
            Invalidate();
        }
    }
    public string OffBgHoveredSprite {
        get => offBgSprites.HoveredSprite;
        set {
            if (value != offBgSprites.HoveredSprite) {
                offBgSprites.HoveredSprite = value;
                Invalidate();
            }
        }
    }
    public string OffFgHoveredSprite {
        get => offFgSprites.HoveredSprite;
        set {
            if (value != offFgSprites.HoveredSprite) {
                offFgSprites.HoveredSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OffBgHoveredColor {
        get => offBgSprites.HoveredColor;
        set {
            offBgSprites.HoveredColor = value;
            Invalidate();
        }
    }
    public Color32 OffFgHoveredColor {
        get => offFgSprites.HoveredColor;
        set {
            offFgSprites.HoveredColor = value;
            Invalidate();
        }
    }
    public string OffBgPressedSprite {
        get => offBgSprites.PressedSprite;
        set {
            if (value != offBgSprites.PressedSprite) {
                offBgSprites.PressedSprite = value;
                Invalidate();
            }
        }
    }
    public string OffFgPressedSprite {
        get => offFgSprites.PressedSprite;
        set {
            if (value != offFgSprites.PressedSprite) {
                offFgSprites.PressedSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OffBgPressedColor {
        get => offBgSprites.PressedColor;
        set {
            offBgSprites.PressedColor = value;
            Invalidate();
        }
    }
    public Color32 OffFgPressedColor {
        get => offFgSprites.PressedColor;
        set {
            offFgSprites.PressedColor = value;
            Invalidate();
        }
    }
    public string OffBgFocusedSprite {
        get => offBgSprites.FocusedSprite;
        set {
            if (value != offBgSprites.FocusedSprite) {
                offBgSprites.FocusedSprite = value;
                Invalidate();
            }
        }
    }
    public string OffFgFocusedSprite {
        get => offFgSprites.FocusedSprite;
        set {
            if (value != offFgSprites.FocusedSprite) {
                offFgSprites.FocusedSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OffBgFocusedColor {
        get => offBgSprites.FocusedColor;
        set {
            offBgSprites.FocusedColor = value;
            Invalidate();
        }
    }
    public Color32 OffFgFocusedColor {
        get => offFgSprites.FocusedColor;
        set {
            offFgSprites.FocusedColor = value;
            Invalidate();
        }
    }
    public string OffBgDisabledSprite {
        get => offBgSprites.DisabledSprite;
        set {
            if (value != offBgSprites.DisabledSprite) {
                offBgSprites.DisabledSprite = value;
                Invalidate();
            }
        }
    }
    public string OffFgDisabledSprite {
        get => offFgSprites.DisabledSprite;
        set {
            if (value != offFgSprites.DisabledSprite) {
                offFgSprites.DisabledSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OffBgDisabledColor {
        get => offBgSprites.DisabledColor;
        set {
            offBgSprites.DisabledColor = value;
            Invalidate();
        }
    }
    public Color32 OffFgDisabledColor {
        get => offFgSprites.DisabledColor;
        set {
            offFgSprites.DisabledColor = value;
            Invalidate();
        }
    }
    public string OnBgNormalSprite {
        get => onBgSprites.NormalSprite;
        set {
            if (value != onBgSprites.NormalSprite) {
                onBgSprites.NormalSprite = value;
                SetDefaultSize(value);
                Invalidate();
            }
        }
    }
    public string OnFgNormalSprite {
        get => onFgSprites.NormalSprite;
        set {
            if (value != onFgSprites.NormalSprite) {
                onFgSprites.NormalSprite = value;
                SetDefaultSize(value);
                Invalidate();
            }
        }
    }
    public Color32 OnBgNormalColor {
        get => onBgSprites.NormalColor;
        set {
            onBgSprites.NormalColor = value;
            Invalidate();
        }
    }
    public Color32 OnFgNormalColor {
        get => onFgSprites.NormalColor;
        set {
            onFgSprites.NormalColor = value;
            Invalidate();
        }
    }
    public string OnBgHoveredSprite {
        get => onBgSprites.HoveredSprite;
        set {
            if (value != onBgSprites.HoveredSprite) {
                onBgSprites.HoveredSprite = value;
                Invalidate();
            }
        }
    }
    public string OnFgHoveredSprite {
        get => onFgSprites.HoveredSprite;
        set {
            if (value != onFgSprites.HoveredSprite) {
                onFgSprites.HoveredSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OnBgHoveredColor {
        get => onBgSprites.HoveredColor;
        set {
            onBgSprites.HoveredColor = value;
            Invalidate();
        }
    }
    public Color32 OnFgHoveredColor {
        get => onFgSprites.HoveredColor;
        set {
            onFgSprites.HoveredColor = value;
            Invalidate();
        }
    }
    public string OnBgPressedSprite {
        get => onBgSprites.PressedSprite;
        set {
            if (value != onBgSprites.PressedSprite) {
                onBgSprites.PressedSprite = value;
                Invalidate();
            }
        }
    }
    public string OnFgPressedSprite {
        get => onFgSprites.PressedSprite;
        set {
            if (value != onFgSprites.PressedSprite) {
                onFgSprites.PressedSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OnBgPressedColor {
        get => onBgSprites.PressedColor;
        set {
            onBgSprites.PressedColor = value;
            Invalidate();
        }
    }
    public Color32 OnFgPressedColor {
        get => onFgSprites.PressedColor;
        set {
            onFgSprites.PressedColor = value;
            Invalidate();
        }
    }
    public string OnBgFocusedSprite {
        get => onBgSprites.FocusedSprite;
        set {
            if (value != onBgSprites.FocusedSprite) {
                onBgSprites.FocusedSprite = value;
                Invalidate();
            }
        }
    }
    public string OnFgFocusedSprite {
        get => onFgSprites.FocusedSprite;
        set {
            if (value != onFgSprites.FocusedSprite) {
                onFgSprites.FocusedSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OnBgFocusedColor {
        get => onBgSprites.FocusedColor;
        set {
            onBgSprites.FocusedColor = value;
            Invalidate();
        }
    }
    public Color32 OnFgFocusedColor {
        get => onFgSprites.FocusedColor;
        set {
            onFgSprites.FocusedColor = value;
            Invalidate();
        }
    }
    public string OnBgDisabledSprite {
        get => onBgSprites.DisabledSprite;
        set {
            if (value != onBgSprites.DisabledSprite) {
                onBgSprites.DisabledSprite = value;
                Invalidate();
            }
        }
    }
    public string OnFgDisabledSprite {
        get => onFgSprites.DisabledSprite;
        set {
            if (value != onFgSprites.DisabledSprite) {
                onFgSprites.DisabledSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 OnBgDisabledColor {
        get => onBgSprites.DisabledColor;
        set {
            onBgSprites.DisabledColor = value;
            Invalidate();
        }
    }
    public Color32 OnFgDisabledColor {
        get => onFgSprites.DisabledColor;
        set {
            onFgSprites.DisabledColor = value;
            Invalidate();
        }
    }
    public UIButtonType ButtonType {
        get => buttonType;
        set {
            if (!Equals(value, buttonType)) {
                buttonType = value;
                Invalidate();
            }
        }
    }
    public bool IsOn {
        get => isOn;
        set {
            if (isOn != value) {
                OnToggleChanged(value);
            }
        }
    }
    public SpriteState State {
        get => state;
        set {
            if (state != value) {
                OnStateChanged(value);
            }
        }
    }
    public UIMouseButton ButtonMask { get => buttonMask; set => buttonMask = value; }
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
    public RectOffset FgSpritePadding {
        get => fgSpritePadding ??= new RectOffset();
        set {
            if (!Equals(value, fgSpritePadding)) {
                fgSpritePadding = value;
                Invalidate();
            }
        }
    }
    public override bool canFocus => (buttonType != UIButtonType.Tab) && base.canFocus;
    public bool IsHovering => m_IsMouseHovering;

    public virtual void SetStyle() {
        Invalidate();
    }
    protected virtual void OnToggleChanged(bool value) {
        if (!isEnabled || buttonType != UIButtonType.Toggle) {
            return;
        }
        isOn = value;
        EventToggleChanged?.Invoke(value);
        Invalidate();
    }
    protected virtual void OnStateChanged(SpriteState value) {
        if (!isEnabled && value != SpriteState.Disabled) {
            return;
        }
        state = value;
        EventStateChanged?.Invoke(value);
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
        if (Atlas is null) {
            return;
        }
        RenderBgSprite();
        RenderFgSprite();
        RenderText();
    }

    private void RenderText() {
        if (Font is null || !Font.isValid) {
            return;
        }
        if (textRenderData != null) {
            textRenderData.Clear();
        } else {
            textRenderData = UIRenderData.Obtain();
            m_RenderData.Add(textRenderData);
        }
        textRenderData.material = Atlas.material;
        using UIFontRenderer uifontRenderer = ObtainTextRenderer();
        if (uifontRenderer is UIDynamicFont.DynamicFontRenderer dynamicFontRenderer) {
            dynamicFontRenderer.spriteAtlas = Atlas;
            dynamicFontRenderer.spriteBuffer = textRenderData;
        }
        uifontRenderer.Render(Text, textRenderData);
    }
    private UIFontRenderer ObtainTextRenderer() {
        Vector2 vector = size - new Vector2(TextPadding.horizontal, TextPadding.vertical);
        Vector2 maxSize = autoSize ? (Vector2.one * 2.1474836E+09f) : vector;
        float num = PixelsToUnits();
        Vector3 vectorOffset = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset) + new Vector3(TextPadding.left, (float)-(float)TextPadding.top)) * num;
        GetTextScaleMultiplier();
        Color32 defaultColor = ApplyOpacity(GetTextColor());
        UIFontRenderer uifontRenderer = Font.ObtainRenderer();
        uifontRenderer.wordWrap = WordWrap;
        uifontRenderer.multiLine = true;
        uifontRenderer.maxSize = maxSize;
        uifontRenderer.pixelRatio = num;
        uifontRenderer.textScale = TextScale;
        uifontRenderer.vectorOffset = vectorOffset;
        uifontRenderer.textAlign = TextHorizontalAlignment;
        uifontRenderer.processMarkup = processMarkup;
        uifontRenderer.defaultColor = defaultColor;
        uifontRenderer.bottomColor = useTextGradient ? new Color32?(GetGradientBottomColorForState()) : default;
        uifontRenderer.overrideMarkupColors = false;
        uifontRenderer.opacity = CalculateOpacity();
        uifontRenderer.shadow = useDropShadow;
        uifontRenderer.shadowColor = dropShadowColor;
        uifontRenderer.shadowOffset = dropShadowOffset;
        uifontRenderer.outline = useOutline;
        uifontRenderer.outlineSize = outlineSize;
        uifontRenderer.outlineColor = outlineColor;
        if (!autoSize && TextVerticalAlignment != UIVerticalAlignment.Top) {
            uifontRenderer.vectorOffset = GetVertAlignOffset(uifontRenderer);
        }
        return uifontRenderer;
    }
    private Vector3 GetVertAlignOffset(UIFontRenderer fontRenderer) {
        float num = PixelsToUnits();
        Vector2 vector = fontRenderer.MeasureString(Text) * num;
        Vector3 vectorOffset = fontRenderer.vectorOffset;
        float num2 = (height - TextPadding.vertical) * num;
        if (vector.y >= num2) {
            return vectorOffset;
        }
        switch (TextVerticalAlignment) {
            case UIVerticalAlignment.Middle:
                vectorOffset.y -= (num2 - vector.y) * 0.5f;
                break;
            case UIVerticalAlignment.Bottom:
                vectorOffset.y -= num2 - vector.y;
                break;
        }
        return vectorOffset;
    }
    protected override Color32 GetTextColor() {
        if (!isEnabled) {
            return TextDisabledColor;
        }
        return State switch {
            SpriteState.Focused => TextFocusedColor,
            SpriteState.Hovered => TextFocusedColor,
            SpriteState.Pressed => TextFocusedColor,
            SpriteState.Disabled => TextFocusedColor,
            _ => TextNormalColor,
        };
    }
    protected override Color32 GetGradientBottomColorForState() => isEnabled ? (State != SpriteState.Disabled ? GradientBottomDisabledColor : GradientBottomNormalColor) : GradientBottomDisabledColor;
    protected virtual void RenderFgSprite() {
        if (!RenderFg) {
            return;
        }
        if (Atlas is null) {
            return;
        }
        var fgSprite = GetFgSprite();
        if (fgSprite is null) {
            return;
        }
        if (fgRenderData is not null) {
            fgRenderData.Clear();
        } else {
            fgRenderData = UIRenderData.Obtain();
            m_RenderData.Add(fgRenderData);
        }
        fgRenderData.material = Atlas.material;
        Vector2 foregroundRenderSize = GetForegroundRenderSize(GetFgSprite());
        Vector2 foregroundRenderOffset = GetForegroundRenderOffset(foregroundRenderSize);
        Color32 color = ApplyOpacity(GetFgActiveColor());
        RenderOptions options = new() {
            atlas = atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = foregroundRenderOffset,
            pixelsToUnits = PixelsToUnits(),
            size = foregroundRenderSize,
            spriteInfo = fgSprite
        };
        if (fgSprite.isSliced) {
            UISlicedSpriteRender.RenderSprite(fgRenderData, options);
            return;
        }
        UISpriteRender.RenderSprite(fgRenderData, options);
    }
    protected virtual void RenderBgSprite() {
        if (Atlas is null) {
            return;
        }
        UITextureAtlas.SpriteInfo sprite = GetBgSprite();
        if (sprite is null) {
            return;
        }
        if (bgRenderData is not null) {
            bgRenderData.Clear();
        } else {
            bgRenderData = UIRenderData.Obtain();
            m_RenderData.Add(bgRenderData);
        }
        bgRenderData.material = Atlas.material;
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
            UISlicedSpriteRender.RenderSprite(bgRenderData, options);
            return;
        }
        UISpriteRender.RenderSprite(bgRenderData, options);
    }
    protected virtual Color32 GetFgActiveColor() => (buttonType == UIButtonType.Toggle) ? (IsOn ? OnFgSprites.GetColor(State) : OffFgSprites.GetColor(State)) : OnFgSprites.GetColor(State);
    protected virtual Color32 GetBgActiveColor() => (buttonType == UIButtonType.Toggle) ? (IsOn ? OnBgSprites.GetColor(State) : OffBgSprites.GetColor(State)) : OnBgSprites.GetColor(State);
    protected virtual UITextureAtlas.SpriteInfo GetFgSprite() {
        if (atlas is null) {
            return null;
        }
        UITextureAtlas.SpriteInfo spriteInfo;
        if (buttonType == UIButtonType.Toggle) {
            spriteInfo = IsOn ? atlas[OnFgSprites.GetSprite(State)] : atlas[OffFgSprites.GetSprite(State)];
        } else {
            spriteInfo = atlas[OnFgSprites.GetSprite(State)];
        }
        if (spriteInfo == null) {
            spriteInfo = atlas[OnFgNormalSprite];
        }
        return spriteInfo;
    }
    protected virtual UITextureAtlas.SpriteInfo GetBgSprite() {
        if (atlas is null) {
            return null;
        }
        UITextureAtlas.SpriteInfo spriteInfo;
        if (buttonType == UIButtonType.Toggle) {
            spriteInfo = IsOn ? atlas[OnBgSprites.GetSprite(State)] : atlas[OffBgSprites.GetSprite(State)];
        } else {
            spriteInfo = atlas[OnBgSprites.GetSprite(State)];
        }
        if (spriteInfo == null) {
            spriteInfo = atlas[OnBgNormalSprite];
        }
        return spriteInfo;
    }
    protected virtual Vector2 GetForegroundRenderOffset(Vector2 renderSize) {
        Vector2 result = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        if (HorizontalAlignment == UIHorizontalAlignment.Left) {
            result.x += FgSpritePadding.left;
        } else if (HorizontalAlignment == UIHorizontalAlignment.Center) {
            result.x += (width - renderSize.x) * 0.5f;
            result.x += (FgSpritePadding.left - FgSpritePadding.right);
        } else if (HorizontalAlignment == UIHorizontalAlignment.Right) {
            result.x += width - renderSize.x;
            result.x -= FgSpritePadding.right;
        }
        if (VerticalAlignment == UIVerticalAlignment.Bottom) {
            result.y -= height - renderSize.y;
            result.y += FgSpritePadding.bottom;
        } else if (VerticalAlignment == UIVerticalAlignment.Middle) {
            result.y -= (height - renderSize.y) * 0.5f;
            result.y -= (FgSpritePadding.top - FgSpritePadding.bottom);
        } else if (VerticalAlignment == UIVerticalAlignment.Top) {
            result.y -= FgSpritePadding.top;
        }
        return result;
    }
    protected virtual Vector2 GetForegroundRenderSize(UITextureAtlas.SpriteInfo spriteInfo) {
        Vector2 vector = Vector2.zero;
        if (spriteInfo == null) {
            return vector;
        }
        if (fgSpriteMode == ForegroundSpriteMode.Custom) {
            vector = fgCustomSize;
        } else if (fgSpriteMode == ForegroundSpriteMode.Fill) {
            vector = spriteInfo.pixelSize;
        } else if (fgSpriteMode == ForegroundSpriteMode.Scale) {
            float num = Mathf.Min(width / spriteInfo.width, height / spriteInfo.height);
            vector = new Vector2(num * spriteInfo.width, num * spriteInfo.height);
            vector *= fgScaleFactor;
        } else {
            vector = size * fgScaleFactor;
        }
        return vector;
    }

    protected override void OnIsEnabledChanged() {
        if (!isEnabled) {
            State = SpriteState.Disabled;
        } else {
            State = SpriteState.Normal;
        }
        base.OnIsEnabledChanged();
    }
    protected override void OnEnterFocus(UIFocusEventParameter p) {
        if (State != SpriteState.Pressed) {
            State = SpriteState.Focused;
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
        State = (containsMouse ? SpriteState.Hovered : SpriteState.Normal);
        base.OnLeaveFocus(p);
    }
    protected override void OnMouseUp(UIMouseEventParameter p) {
        if (m_IsMouseHovering) {
            if (buttonType == UIButtonType.Tab && containsFocus) {
                State = SpriteState.Focused;
            } else {
                State = SpriteState.Hovered;
            }
        } else if (hasFocus) {
            State = SpriteState.Focused;
        } else {
            State = SpriteState.Normal;
        }
        base.OnMouseUp(p);
    }
    protected override void OnMouseDown(UIMouseEventParameter p) {
        if ((p.buttons & buttonMask) != UIMouseButton.None) {
            if (isEnabled && State != SpriteState.Focused && buttonType != UIButtonType.Tab) {
                State = SpriteState.Pressed;
            }
            base.OnMouseDown(p);
        }
    }
    protected override void OnMouseEnter(UIMouseEventParameter p) {
        if (isEnabled) {
            State = SpriteState.Hovered;
        }
        Invalidate();
        base.OnMouseEnter(p);
    }
    protected override void OnMouseLeave(UIMouseEventParameter p) {
        if (isEnabled) {
            if (containsFocus) {
                State = SpriteState.Focused;
            } else {
                State = SpriteState.Normal;
            }
        }
        Invalidate();
        base.OnMouseLeave(p);
    }
    protected override void OnKeyPress(UIKeyEventParameter p) {
        if (builtinKeyNavigation && (p.keycode == KeyCode.Space || p.keycode == KeyCode.Return)) {
            OnClick(new UIMouseEventParameter(this, UIMouseButton.Left, 1, default, Vector2.zero, Vector2.zero, 0f));
            return;
        }
        base.OnKeyPress(p);
    }
    protected override void OnClick(UIMouseEventParameter p) {
        if (!p.used) {
            if (buttonType == UIButtonType.Toggle) {
                IsOn = !IsOn;
            }
        }
        base.OnClick(p);
    }
}

public enum UIButtonType {
    Normal,
    Toggle,
    Tab
}


