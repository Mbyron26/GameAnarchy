namespace MbyronModsCommon.UI;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomUILabel : CustomUITextComponent {
    protected UIRenderData bgRenderData;
    protected string bgSprite = string.Empty;
    protected Color32 bgNormalColor = Color.white;
    protected Color32 bgDisabledColor = Color.white;
    protected string prefixText = string.Empty;
    protected string suffixText = string.Empty;
    protected new bool autoSize = true;
    protected bool autoHeight;
    protected RectOffset padding = new();
    protected int tabSize = 48;
    protected List<int> tabStops = new();

    public event PropertyChangedEventHandler<string> EventTextChanged;
    public event Action<Color32> EventColorChanged;

    public CustomUILabel() => textHorizontalAlignment = UIHorizontalAlignment.Left;

    public string BgSprite {
        get => bgSprite;
        set {
            if (value != bgSprite) {
                bgSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 BgNormalColor {
        get => bgNormalColor;
        set {
            if (!bgNormalColor.Equals(value)) {
                bgNormalColor = value;
                Invalidate();
                EventColorChanged?.Invoke(value);
            }
        }
    }
    public Color32 BgDisabledColor {
        get => bgDisabledColor;
        set {
            if (!bgDisabledColor.Equals(value)) {
                bgDisabledColor = value;
                Invalidate();
                EventColorChanged?.Invoke(value);
            }
        }
    }
    public string PrefixText {
        get => prefixText;
        set {
            if (!string.Equals(value, prefixText)) {
                prefixText = value;
                OnTextChanged();
            }
        }
    }
    public string SuffixText {
        get => suffixText;
        set {
            if (!string.Equals(value, suffixText)) {
                suffixText = value;
                OnTextChanged();
            }
        }
    }
    public override string Text {
        get {
            bool flag = !string.IsNullOrEmpty(prefixText);
            bool flag2 = !string.IsNullOrEmpty(suffixText);
            if (flag || flag2) {
                var rawText = text;
                if (flag) {
                    rawText = prefixText + text;
                }
                if (flag2) {
                    rawText += suffixText;
                }
                return rawText;
            }
            return text;
        }
        set {
            if (!string.Equals(value, text)) {
                text = value;
                OnTextChanged();
            }
        }
    }
    public bool AutoSize {
        get => autoSize;
        set {
            if (value != autoSize) {
                if (value) {
                    autoHeight = false;
                }
                autoSize = value;
                Invalidate();
            }
        }
    }
    public bool AutoHeight {
        get => autoHeight && !autoSize;
        set {
            if (value != autoHeight) {
                if (value) {
                    autoSize = false;
                }
                autoHeight = value;
                Invalidate();
            }
        }
    }
    public int TabSize {
        get => tabSize;
        set {
            value = Mathf.Max(0, value);
            if (value != tabSize) {
                tabSize = value;
                Invalidate();
            }
        }
    }
    public List<int> TabStops => tabStops;

    public static CustomUILabel Add(UIComponent parent, string text, float? width, float textScale = 1f, RectOffset rectOffset = null, bool wordWrap = true, bool autoHeight = true) {
        var label = parent.AddUIComponent<CustomUILabel>();
        label.AutoHeight = autoHeight;
        label.TextScale = textScale;
        label.WordWrap = wordWrap;
        if (rectOffset is not null)
            label.TextPadding = rectOffset;
        label.Text = text;
        if (width.HasValue) {
            label.width = width.Value;
        } else {
            using UIFontRenderer fontRenderer = label.ObtainRenderer();
            label.width = fontRenderer.MeasureString(label.text).x;
        }
        return label;
    }

    public void SetStyle(UITextureAtlas atlas, string bgSprite, Color32 bgNormalColor = default, Color32 bgDisabledColor = default) {
        Atlas = atlas;
        BgSprite = bgSprite;
        BgNormalColor = bgNormalColor;
        BgDisabledColor = bgDisabledColor;
    }
    protected internal void OnTextChanged() {
        Invalidate();
        EventTextChanged?.Invoke(this, Text);
        Invoke("OnTextChanged", new object[] { Text });
    }
    public override Vector2 CalculateMinimumSize() {
        if (Font != null) {
            float num = Font.size * textScale * 0.75f;
            return Vector2.Max(base.CalculateMinimumSize(), new Vector2(num, num));
        }
        return base.CalculateMinimumSize();
    }
    public override void Invalidate() {
        base.Invalidate();
        if (Font is null || !Font.isValid || string.IsNullOrEmpty(Text)) {
            return;
        }
        bool flag = size.sqrMagnitude <= float.Epsilon;
        if (!AutoSize && !AutoHeight && !flag) {
            return;
        }
        if (string.IsNullOrEmpty(Text)) {
            if (flag) {
                size = new Vector2(150f, 24f);
            }
            if (AutoSize || AutoHeight) {
                height = Mathf.CeilToInt(Font.lineHeight * textScale);
            }
            return;
        }
        using UIFontRenderer uifontRenderer = ObtainRenderer();
        Vector2 a = uifontRenderer.MeasureString(Text).RoundToInt();
        if (AutoSize || flag) {
            size = a + new Vector2(TextPadding.horizontal, TextPadding.vertical);
        } else if (AutoHeight) {
            size = new Vector2(size.x, a.y + TextPadding.vertical);
        }
        if ((AutoSize || AutoHeight || flag) && anchor.IsAnyFlagSet(UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical)) {
            PerformLayout();
        }
    }
    protected override void OnRebuildRenderData() {
        if (Font is null || !Font.isValid) {
            return;
        }
        RenderBackground();
        if (textRenderData is not null) {
            textRenderData.Clear();
        } else {
            textRenderData = UIRenderData.Obtain();
            m_RenderData.Add(textRenderData);
        }
        textRenderData.material = Atlas.material;
        if (string.IsNullOrEmpty(Text)) {
            return;
        }
        bool flag = size.sqrMagnitude <= float.Epsilon;
        using UIFontRenderer uifontRenderer = ObtainRenderer();
        if (uifontRenderer is UIDynamicFont.DynamicFontRenderer dynamicFontRenderer) {
            dynamicFontRenderer.spriteAtlas = Atlas;
            dynamicFontRenderer.spriteBuffer = bgRenderData;
        }
        uifontRenderer.Render(Text, textRenderData);
        if (AutoSize || flag) {
            size = uifontRenderer.renderedSize.RoundToInt() + new Vector2(TextPadding.horizontal, TextPadding.vertical);
        } else if (AutoHeight) {
            size = new Vector2(size.x, uifontRenderer.renderedSize.y + TextPadding.vertical).RoundToInt();
        }
    }
    public UIFontRenderer ObtainRenderer() {
        Vector2 vector = size - new Vector2(TextPadding.horizontal, TextPadding.vertical);
        Vector2 maxSize = vector;
        if (AutoHeight) {
            maxSize = new Vector2(vector.x, 4096f);
        }
        float num = PixelsToUnits();
        Vector3 vector2 = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset) + new Vector3(TextPadding.left, (float)(-(float)TextPadding.top))) * num;
        float textScale = base.textScale * GetTextScaleMultiplier();
        UIFontRenderer uifontRenderer = Font.ObtainRenderer();
        uifontRenderer.wordWrap = WordWrap;
        uifontRenderer.maxSize = maxSize;
        uifontRenderer.pixelRatio = num;
        uifontRenderer.textScale = textScale;
        uifontRenderer.characterSpacing = characterSpacing;
        uifontRenderer.vectorOffset = vector2.Quantize(num);
        uifontRenderer.multiLine = true;
        uifontRenderer.tabSize = TabSize;
        uifontRenderer.tabStops = TabStops;
        uifontRenderer.textAlign = TextHorizontalAlignment;
        uifontRenderer.processMarkup = processMarkup;
        uifontRenderer.defaultColor = (isEnabled ? TextNormalColor : textDisabledColor);
        uifontRenderer.colorizeSprites = colorizeSprites;
        uifontRenderer.bottomColor = useTextGradient ? new Color32?(GetGradientBottomColorForState()) : default;
        uifontRenderer.overrideMarkupColors = !isEnabled;
        uifontRenderer.opacity = CalculateOpacity();
        uifontRenderer.outline = useOutline;
        uifontRenderer.outlineSize = outlineSize;
        uifontRenderer.outlineColor = outlineColor;
        uifontRenderer.shadow = useDropShadow;
        uifontRenderer.shadowColor = dropShadowColor;
        uifontRenderer.shadowOffset = dropShadowOffset;
        if (TextVerticalAlignment != UIVerticalAlignment.Top) {
            uifontRenderer.vectorOffset = GetVertAlignOffset(uifontRenderer);
        }
        return uifontRenderer;
    }
    protected override float GetTextScaleMultiplier() {
        if (textScaleMode == UITextScaleMode.None || !Application.isPlaying) {
            return 1f;
        }
        if (textScaleMode == UITextScaleMode.ScreenResolution) {
            return Screen.height / (float)GetUIView().fixedHeight;
        }
        if (AutoSize) {
            return 1f;
        }
        return size.y / startSize.y;
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
    protected virtual void RenderBackground() {
        if (Atlas is null) {
            return;
        }
        if (bgRenderData is not null) {
            bgRenderData.Clear();
        } else {
            bgRenderData = UIRenderData.Obtain();
            m_RenderData.Add(bgRenderData);
        }
        bgRenderData.material = Atlas.material;
        UITextureAtlas.SpriteInfo spriteInfo = Atlas[BgSprite];
        if (spriteInfo is null) {
            return;
        }
        Color32 color = ApplyOpacity(isEnabled ? bgNormalColor : bgDisabledColor);
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
            pixelsToUnits = PixelsToUnits(),
            size = size,
            spriteInfo = spriteInfo
        };
        if (spriteInfo.isSliced) {
            UISlicedSpriteRender.RenderSprite(bgRenderData, options);
            return;
        }
        UISpriteRender.RenderSprite(bgRenderData, options);
    }

}

