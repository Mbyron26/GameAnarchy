namespace MbyronModsCommon.UI;
using ColossalFramework.UI;
using System;
using UnityEngine;

public class CustomUIDropDown : CustomUIDropDownBase<CustomUIListBox> {
    public CustomUIDropDown() => canFocus = true;

    public static CustomUIDropDown AddCPDropDown(UIComponent parent, Vector2 size, string[] items, int selectedIndex, Action<int> callback = null) {
        var dropDown = parent.AddUIComponent<CustomUIDropDown>();
        dropDown.size = size;
        dropDown.TextScale = 0.8f;
        dropDown.TextPadding = new(10, 0, 4, 0);
        dropDown.ListHeight = (int)(size.y * items.Length + 8);
        dropDown.TextHorizontalAlignment = UIHorizontalAlignment.Left;
        dropDown.TriggerButton = dropDown;
        dropDown.CanWheel = true;
        dropDown.EventPopupAdded += (dropDwon, listBox) => {
            listBox.canFocus = true;
            listBox.TextScale = 0.8f;
            listBox.Atlas = CustomUIAtlas.MbyronModsAtlas;
            listBox.BgSprite = CustomUIAtlas.RoundedRectangle2;
            listBox.BgNormalColor = CustomUIColor.CPButtonNormal;
            listBox.ItemHoverSprite = CustomUIAtlas.RoundedRectangle2;
            listBox.ItemHoverColor = CustomUIColor.CPButtonHovered;
            listBox.ItemSelectionSprite = CustomUIAtlas.RoundedRectangle2;
            listBox.ItemSelectionColor = CustomUIColor.GreenNormal;
            listBox.ItemPadding = new RectOffset(6, 0, 2, 0);
            listBox.ListPadding = new RectOffset(4, 4, 4, 0);
            listBox.ItemHeight = (int)dropDwon.height;
            listBox.TextHorizontalAlignment = UIHorizontalAlignment.Left;
        };
        dropDown.atlas = CustomUIAtlas.MbyronModsAtlas;
        dropDown.bgSprites.SetSprites(CustomUIAtlas.RoundedRectangle2);
        dropDown.bgSprites.SetColors(CustomUIColor.CPButtonNormal, CustomUIColor.CPButtonHovered, CustomUIColor.CPButtonPressed, CustomUIColor.CPButtonFocused, CustomUIColor.CPButtonDisabled);
        dropDown.FgSprites.SetSprites(CustomUIAtlas.ArrowDown);
        var fgColor = new Color32(180, 180, 180, 255);
        dropDown.FgSprites.SetColors(fgColor, fgColor, fgColor, fgColor, new(71, 71, 71, 255));
        dropDown.fgSpritePadding = new(0, 6, 0, 0);
        dropDown.fgScaleFactor = 0.8f;
        dropDown.fgSpriteMode = ForegroundSpriteMode.Scale;
        dropDown.horizontalAlignment = UIHorizontalAlignment.Right;
        dropDown.items = items;
        dropDown.SelectedIndex = selectedIndex;
        dropDown.EventSelectedIndexChanged += (c, v) => callback?.Invoke(v);
        dropDown.RenderFg = true;
        return dropDown;
    }
    public static CustomUIDropDown AddOPDropDown(UIComponent parent, Vector2 size, string[] items, int selectedIndex, Action<int> callback = null) {
        var dropDown = parent.AddUIComponent<CustomUIDropDown>();
        dropDown.size = size;
        dropDown.ListHeight = (int)(size.y * items.Length + 8);
        dropDown.TextHorizontalAlignment = UIHorizontalAlignment.Left;
        dropDown.TextPadding = new(12, 6, 0, 0);
        dropDown.TriggerButton = dropDown;
        dropDown.EventPopupAdded += (dropDwon, listBox) => {
            listBox.canFocus = true;
            listBox.Atlas = CustomUIAtlas.MbyronModsAtlas;
            listBox.BgSprite = CustomUIAtlas.RoundedRectangle3;
            listBox.BgNormalColor = CustomUIColor.OPButtonNormal;
            listBox.ItemHoverSprite = CustomUIAtlas.RoundedRectangle3;
            listBox.ItemHoverColor = CustomUIColor.OPButtonHovered;
            listBox.ItemSelectionSprite = CustomUIAtlas.RoundedRectangle3;
            listBox.ItemSelectionColor = CustomUIColor.BlueNormal;
            listBox.ItemPadding = new RectOffset(8, 0, 0, 0);
            listBox.ListPadding = new RectOffset(4, 4, 4, 0);
            listBox.ItemHeight = (int)dropDwon.height;
            listBox.TextHorizontalAlignment = UIHorizontalAlignment.Left;
        };
        dropDown.atlas = CustomUIAtlas.MbyronModsAtlas;
        dropDown.bgSprites.SetSprites(CustomUIAtlas.RoundedRectangle3);
        dropDown.bgSprites.SetColors(CustomUIColor.OPButtonNormal, CustomUIColor.OPButtonHovered, CustomUIColor.OPButtonPressed, CustomUIColor.OPButtonFocused, CustomUIColor.OPButtonDisabled);
        dropDown.FgSprites.SetSprites(CustomUIAtlas.ArrowDown);
        var fgColor = new Color32(180, 180, 180, 255);
        dropDown.FgSprites.SetColors(fgColor, fgColor, fgColor, fgColor, new(71, 71, 71, 255));
        dropDown.fgSpritePadding = new(0, 6, 0, 0);
        dropDown.fgScaleFactor = 0.8f;
        dropDown.fgSpriteMode = ForegroundSpriteMode.Scale;
        dropDown.horizontalAlignment = UIHorizontalAlignment.Right;
        dropDown.items = items;
        dropDown.SelectedIndex = selectedIndex;
        dropDown.EventSelectedIndexChanged += (c, v) => callback?.Invoke(v);
        dropDown.RenderFg = true;
        return dropDown;
    }
}

public abstract class CustomUIDropDownBase<T> : CustomUITextComponent where T : CustomUIListBox {
    protected UIRenderData bgRenderData;
    protected UIRenderData fgRenderData;
    protected Sprites bgSprites = new();
    protected Sprites fgSprites = new();
    protected bool renderBg = true;
    protected bool renderFg = false;
    protected ForegroundSpriteMode fgSpriteMode;
    protected float fgScaleFactor = 1f;
    protected Vector2 fgCustomSize;
    protected UIHorizontalAlignment horizontalAlignment = UIHorizontalAlignment.Center;
    protected UIVerticalAlignment verticalAlignment = UIVerticalAlignment.Middle;
    protected RectOffset fgSpritePadding;
    protected SpriteState state;
    protected T popup;
    protected Vector2 listOffset = Vector2.zero;
    protected int listWidth;
    protected bool autoListWidth;
    protected int listHeight = 200;
    protected bool clampListToScreen;
    protected string[] items = new string[0];
    protected int selectedIndex = -1;
    protected UIComponent triggerButton;
    private bool eventsAttached;
    protected bool canWheel;
    protected PopupListPosition listPosition;

    public event Action<CustomUIDropDownBase<T>, T> EventPopupAdded;
    public event Action<CustomUIDropDownBase<T>, T> EventDropdownOpened;
    public event Action<CustomUIDropDownBase<T>, T> EventDropdownClosed;
    public event Action<CustomUIDropDownBase<T>, int> EventSelectedIndexChanged;
    public event Action<SpriteState> EventStateChanged;

    public Sprites BgSprites => bgSprites;
    public Sprites FgSprites => fgSprites;
    public bool RenderBg {
        get => renderBg;
        set {
            if (renderBg != value) {
                renderBg = value;
                Invalidate();
            }
        }
    }
    public bool RenderFg {
        get => renderFg;
        set {
            if (renderFg != value) {
                renderFg = value;
                Invalidate();
            }
        }
    }
    public Color32 BgNormalColor {
        get => bgSprites.NormalColor;
        set {
            if (!bgSprites.NormalColor.Equals(value)) {
                bgSprites.NormalColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgHoveredColor {
        get => bgSprites.HoveredColor;
        set {
            if (!bgSprites.HoveredColor.Equals(value)) {
                bgSprites.HoveredColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgPressedColor {
        get => bgSprites.PressedColor;
        set {
            if (!bgSprites.PressedColor.Equals(value)) {
                bgSprites.PressedColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgFocusedColor {
        get => bgSprites.FocusedColor;
        set {
            if (!bgSprites.FocusedColor.Equals(value)) {
                bgSprites.FocusedColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgDisabledColor {
        get => bgSprites.DisabledColor;
        set {
            if (!bgSprites.DisabledColor.Equals(value)) {
                bgSprites.DisabledColor = value;
                Invalidate();
            }
        }
    }
    public Color32 FgNormalColor {
        get => fgSprites.NormalColor;
        set {
            if (!fgSprites.NormalColor.Equals(value)) {
                fgSprites.NormalColor = value;
                Invalidate();
            }
        }
    }
    public Color32 FgHoveredColor {
        get => fgSprites.HoveredColor;
        set {
            if (!fgSprites.HoveredColor.Equals(value)) {
                fgSprites.HoveredColor = value;
                Invalidate();
            }
        }
    }
    public Color32 FgPressedColor {
        get => fgSprites.PressedColor;
        set {
            if (!fgSprites.PressedColor.Equals(value)) {
                fgSprites.PressedColor = value;
                Invalidate();
            }
        }
    }
    public Color32 FgFocusedColor {
        get => fgSprites.FocusedColor;
        set {
            if (!fgSprites.FocusedColor.Equals(value)) {
                fgSprites.FocusedColor = value;
                Invalidate();
            }
        }
    }
    public Color32 FgDisabledColor {
        get => fgSprites.DisabledColor;
        set {
            if (!fgSprites.DisabledColor.Equals(value)) {
                fgSprites.DisabledColor = value;
                Invalidate();
            }
        }
    }
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
    public SpriteState State {
        get => state;
        set {
            if (state != value) {
                OnStateChanged(value);
            }
        }
    }
    public T Popup => popup;
    public int ListWidth {
        get => listWidth;
        set {
            listWidth = value;
        }
    }
    public bool AutoListWidth {
        get => autoListWidth;
        set {
            autoListWidth = value;
        }
    }
    public int ListHeight {
        get => listHeight;
        set {
            listHeight = value;
            Invalidate();
        }
    }
    public Vector2 ListOffset {
        get => listOffset;
        set {
            if (Vector2.Distance(listOffset, value) > 1f) {
                listOffset = value;
                Invalidate();
            }
        }
    }
    public bool ClampListToScreen {
        get => clampListToScreen;
        set {
            clampListToScreen = value;
        }
    }
    public string[] Items {
        get => items ??= new string[0];
        set {
            ClosePopup(true);
            value ??= new string[0];
            items = value;
            Invalidate();
        }
    }
    public string SelectedValue {
        get {
            if (selectedIndex < 0 || selectedIndex >= items.Length) {
                return string.Empty;
            }
            return items[selectedIndex];
        }
        set {
            SelectedIndex = -1;
            for (int i = 0; i < items.Length; i++) {
                if (items[i] == value) {
                    SelectedIndex = i;
                    return;
                }
            }
        }
    }
    public virtual int SelectedIndex {
        get => selectedIndex;
        set {
            value = Mathf.Max(-1, value);
            value = Mathf.Min(Items.Length - 1, value);
            if (value != selectedIndex) {
                if (popup != null) {
                    popup.SelectedIndex = value;
                }
                selectedIndex = value;
                EventSelectedIndexChanged?.Invoke(this, SelectedIndex);
                Invalidate();
            }
        }
    }
    public UIComponent TriggerButton {
        get => triggerButton;
        set {
            if (value != triggerButton) {
                DetachChildEvents();
                triggerButton = value;
                AttachChildEvents();
                Invalidate();
            }
        }
    }
    public bool CanWheel {
        get => canWheel;
        set {
            if (value != canWheel) {
                canWheel = value;
            }
        }
    }
    public PopupListPosition ListPosition {
        get => listPosition;
        set {
            if (value != listPosition) {
                ClosePopup(true);
                listPosition = value;
                Invalidate();
            }
        }
    }

    protected virtual void OnStateChanged(SpriteState value) {
        if (!isEnabled && value != SpriteState.Disabled) {
            return;
        }
        state = value;
        EventStateChanged?.Invoke(value);
        Invalidate();
    }
    protected override void OnMouseWheel(UIMouseEventParameter p) {
        if (canWheel) {
            SelectedIndex = Mathf.Max(0, SelectedIndex - Mathf.RoundToInt(p.wheelDelta));
            p.Use();
        }
        base.OnMouseWheel(p);
    }
    protected override void OnKeyDown(UIKeyEventParameter p) {
        if(p.keycode == KeyCode.Escape) {
            Unfocus();
        }
        if (builtinKeyNavigation) {
            KeyCode keycode = p.keycode;
            if (keycode != KeyCode.Space) {
                switch (keycode) {
                    case KeyCode.UpArrow:
                        SelectedIndex = Mathf.Max(0, SelectedIndex - 1);
                        p.Use();
                        break;
                    case KeyCode.DownArrow:
                        SelectedIndex = Mathf.Min(Items.Length - 1, SelectedIndex + 1);
                        p.Use();
                        break;
                    case KeyCode.Home:
                        SelectedIndex = 0;
                        p.Use();
                        break;
                    case KeyCode.End:
                        SelectedIndex = Items.Length - 1;
                        p.Use();
                        break;
                }
            } else {
                OpenPopup();
                p.Use();
            }
        }
        base.OnKeyDown(p);
    }
    public override void OnEnable() {
        base.OnEnable();
        bool flag = Font != null && Font.isValid;
        if (Application.isPlaying && !flag) {
            Font = GetUIView().defaultFont;
        }
    }
    public override void OnDisable() {
        base.OnDisable();
        ClosePopup(false);
    }
    public override void OnDestroy() {
        base.OnDestroy();
        ClosePopup(false);
        DetachChildEvents();
    }
    public override void Update() {
        base.Update();
        CheckForPopupClose();
    }
    public override void LateUpdate() {
        base.LateUpdate();
        if (!Application.isPlaying) {
            return;
        }
        if (!eventsAttached) {
            AttachChildEvents();
        }
        if (popup != null && !popup.containsFocus && !m_IsMouseHovering) {
            ClosePopup(true);
        }
    }
    private void CheckForPopupClose() {
        if (popup == null || !Input.GetMouseButtonDown(0)) {
            return;
        }
        Camera camera = GetCamera();
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (popup.Raycast(ray)) {
            return;
        }
        if (popup.Scrollbar != null && popup.Scrollbar.Raycast(ray)) {
            return;
        }
        if (!m_IsMouseHovering) {
            ClosePopup(true);
        }
    }
    private void AttachChildEvents() {
        if (TriggerButton != null && !eventsAttached) {
            eventsAttached = true;
            TriggerButton.eventClick += OnTriggerClick;
        }
    }
    private void DetachChildEvents() {
        if (TriggerButton != null && eventsAttached) {
            TriggerButton.eventClick -= OnTriggerClick;
            eventsAttached = false;
        }
    }
    private void OnTriggerClick(UIComponent child, UIMouseEventParameter mouseEvent) {
        if (popup is null) {
            OpenPopup();
        } else {
            ClosePopup(true);
        }
    }

    protected override void OnRebuildRenderData() {
        if (atlas == null || Font == null || !Font.isValid) {
            return;
        }
        RenderBgSprite();
        RenderFgSprite();
        RenderText();
    }
    protected virtual void RenderFgSprite() {
        if (!RenderFg) {
            return;
        }
        if (Atlas is null) {
            return;
        }
        var spriteInfo = GetFgSprite();
        if (spriteInfo is null) {
            return;
        }
        if (fgRenderData is not null) {
            fgRenderData.Clear();
        } else {
            fgRenderData = UIRenderData.Obtain();
            m_RenderData.Add(fgRenderData);
        }
        fgRenderData.material = Atlas.material;
        Color32 color = ApplyOpacity(GetFgActiveColor());
        Vector2 foregroundRenderSize = GetForegroundRenderSize(GetFgSprite());
        Vector2 foregroundRenderOffset = GetForegroundRenderOffset(foregroundRenderSize);
        if (spriteInfo is null) {
            return;
        }
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = foregroundRenderOffset,
            pixelsToUnits = PixelsToUnits(),
            size = foregroundRenderSize,
            spriteInfo = spriteInfo
        };
        if (spriteInfo.isSliced) {
            UISlicedSpriteRender.RenderSprite(fgRenderData, options);
            return;
        }
        UISpriteRender.RenderSprite(fgRenderData, options);
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
    protected virtual UITextureAtlas.SpriteInfo GetFgSprite() => Atlas?[FgSprites.GetSprite(State)];
    protected virtual Color32 GetFgActiveColor() => FgSprites.GetColor(State);
    protected virtual void RenderBgSprite() {
        if (!RenderBg) {
            return;
        }
        if (Atlas is null) {
            return;
        }
        var spriteInfo = GetBgSprite();
        if (spriteInfo is null) {
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
        if (spriteInfo is null) {
            return;
        }
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
    protected virtual UITextureAtlas.SpriteInfo GetBgSprite() => Atlas?[BgSprites.GetSprite(State)];
    protected virtual Color32 GetBgActiveColor() => BgSprites.GetColor(State);
    private void RenderText() {
        if (Atlas is null || SelectedIndex < 0 || SelectedIndex >= Items.Length) {
            return;
        }
        if (textRenderData is not null) {
            textRenderData.Clear();
        } else {
            textRenderData = UIRenderData.Obtain();
            m_RenderData.Add(textRenderData);
        }
        textRenderData.material = Atlas.material;
        string text = Items[SelectedIndex];
        float num = PixelsToUnits();
        Vector2 maxSize = new(size.x - TextPadding.horizontal, size.y - TextPadding.vertical);
        Vector3 vectorOffset = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset) + new Vector3(TextPadding.left, (float)-(float)TextPadding.top)) * num;
        GetTextScaleMultiplier();
        Color32 defaultColor = isEnabled ? textNormalColor : textDisabledColor;
        using UIFontRenderer uifontRenderer = Font.ObtainRenderer();
        uifontRenderer.wordWrap = WordWrap;
        uifontRenderer.maxSize = maxSize;
        uifontRenderer.pixelRatio = num;
        uifontRenderer.textScale = TextScale;
        uifontRenderer.characterSpacing = CharacterSpacing;
        uifontRenderer.vectorOffset = vectorOffset;
        uifontRenderer.multiLine = false;
        uifontRenderer.textAlign = TextHorizontalAlignment;
        uifontRenderer.processMarkup = ProcessMarkup;
        uifontRenderer.colorizeSprites = ColorizeSprites;
        uifontRenderer.defaultColor = defaultColor;
        uifontRenderer.bottomColor = useTextGradient ? new Color32?(GetGradientBottomColorForState()) : default;
        uifontRenderer.overrideMarkupColors = false;
        uifontRenderer.opacity = CalculateOpacity();
        uifontRenderer.outline = UseOutline;
        uifontRenderer.outlineSize = OutlineSize;
        uifontRenderer.outlineColor = OutlineColor;
        uifontRenderer.shadow = UseDropShadow;
        uifontRenderer.shadowColor = DropShadowColor;
        uifontRenderer.shadowOffset = DropShadowOffset;
        if (!autoSize && TextVerticalAlignment != UIVerticalAlignment.Top) {
            uifontRenderer.vectorOffset = GetVertAlignOffset(uifontRenderer);
        }
        if (uifontRenderer is UIDynamicFont.DynamicFontRenderer dynamicFontRenderer) {
            dynamicFontRenderer.spriteAtlas = Atlas;
            dynamicFontRenderer.spriteBuffer = textRenderData;
        }
        uifontRenderer.Render(text, textRenderData);
    }
    private Vector3 GetVertAlignOffset(UIFontRenderer fontRenderer) {
        float num = PixelsToUnits();
        Vector2 vector = fontRenderer.MeasureString(Items[SelectedIndex]) * num;
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
    public void AddItem(string item) {
        string[] array = new string[Items.Length + 1];
        Array.Copy(Items, array, Items.Length);
        array[Items.Length] = item;
        Items = array;
    }
    private Vector3 CalculatePopupPosition() {
        float d = PixelsToUnits();
        Vector3 a = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 a2 = transform.position + a * d;
        Vector3 scaledDirection = GetScaledDirection(Vector3.down);
        Vector3 b = TransformOffset(ListOffset) * d;
        Vector3 vector = a2 + b + scaledDirection * size.y * d;
        Vector3 result = a2 + b - scaledDirection * popup.size.y * d;
        if (ListPosition == PopupListPosition.Above) {
            return result;
        }
        if (ListPosition == PopupListPosition.Below) {
            return vector;
        }
        Vector3 a3 = popup.transform.parent.position / d + popup.parent.pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 vector2 = a3 + scaledDirection * parent.size.y;
        Vector3 a4 = vector / d + scaledDirection * popup.size.y;
        if (a4.y < vector2.y) {
            return result;
        }
        if (GetCamera().WorldToScreenPoint(a4 * d).y <= 0f) {
            return result;
        }
        return vector;
    }
    private Vector2 CalculatePopupSize() {
        float popupWidth = (ListWidth > 0) ? ListWidth : size.x;
        int popupHeight = Items.Length * (popup is null ? 25 : popup.ItemHeight) + (popup is null ? 0 : popup.ListPadding.vertical);
        if (Items.Length == 0) {
            popupHeight = (popup is null ? 25 : popup.ItemHeight) / 2 + (popup is null ? 0 : popup.ListPadding.vertical);
        }
        if (AutoListWidth) {
            popupWidth = Mathf.Max(CalculatePopupWidth(Mathf.Min(ListHeight, popupHeight)), popupWidth);
        }
        return new Vector2(popupWidth, Mathf.Max(ListHeight, popupHeight));
    }
    public float CalculatePopupWidth(int height) {
        float num = 0f;
        float pixelRatio = PixelsToUnits();
        for (int i = 0; i < Items.Length; i++) {
            using UIFontRenderer uifontRenderer = Font.ObtainRenderer();
            uifontRenderer.wordWrap = false;
            uifontRenderer.pixelRatio = pixelRatio;
            uifontRenderer.textScale = textScale;
            uifontRenderer.characterSpacing = characterSpacing;
            uifontRenderer.multiLine = false;
            uifontRenderer.textAlign = textHorizontalAlignment;
            uifontRenderer.processMarkup = processMarkup;
            uifontRenderer.colorizeSprites = colorizeSprites;
            uifontRenderer.overrideMarkupColors = false;
            Vector2 vector = uifontRenderer.MeasureString(Items[i]);
            if (vector.x > num) {
                num = vector.x;
            }
        }
        num += popup.ListPadding.horizontal + popup.ItemPadding.horizontal;
        if (popup is not null && popup.Scrollbar is not null && height >= ListHeight) {
            num += popup.Scrollbar.size.x;
        }
        return num;
    }

    private bool OpenPopup() {
        if (popup is not null || Items.Length == 0) {
            return false;
        }
        UIComponent rootContainer = GetRootContainer();
        Vector2 size2 = CalculatePopupSize();
        popup = rootContainer.AddUIComponent<T>();
        EventPopupAdded?.Invoke(this, popup);
        popup.gameObject.hideFlags = HideFlags.DontSave;
        popup.anchor = UIAnchorStyle.None;
        popup.pivot = UIPivotPoint.TopLeft;
        popup.size = size2;
        popup.Items = Items;
        popup.zOrder = int.MaxValue;
        if (size2.y >= ListHeight && popup.Scrollbar is not null) {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(popup.Scrollbar.gameObject);
            var activeScrollbar = gameObject.GetComponent<UIScrollbar>();
            float d = PixelsToUnits();
            Vector3 a = popup.transform.TransformDirection(Vector3.right);
            Vector3 position = popup.transform.position + a * (size2.x - activeScrollbar.width) * d;
            activeScrollbar.transform.parent = popup.transform;
            activeScrollbar.transform.position = position;
            activeScrollbar.anchor = (UIAnchorStyle.Top | UIAnchorStyle.Bottom);
            activeScrollbar.height = popup.height;
            popup.width -= activeScrollbar.width;
            popup.eventSizeChanged += (c, v) => popup.Scrollbar.height = c.height; ;
        }
        Vector3 vector = CalculatePopupPosition();
        if (clampListToScreen) {
            vector = ClampToScreen(vector, popup.size + ((popup.Scrollbar != null) ? new Vector2(popup.Scrollbar.size.x, 0f) : default));
        }
        popup.transform.position = vector;
        popup.transform.rotation = transform.rotation;
        popup.EventSelectedIndexChanged += PopupSelectedIndexChanged;
        popup.eventLeaveFocus += PopupLostFocus;
        popup.EventItemClicked += PopupItemClicked;
        popup.eventKeyDown += PopupKeyDown;
        popup.SelectedIndex = Mathf.Max(0, SelectedIndex);
        popup.EnsureVisible(popup.SelectedIndex);
        popup.Focus();
        EventDropdownOpened?.Invoke(this, popup);
        Invoke("OnDropdownOpen", new object[] { popup });
        return true;
    }

    private Vector3 ClampToScreen(Vector3 targetPos, Vector3 targetSize) {
        float num = PixelsToUnits();
        UIView uiview = GetUIView();
        Vector2 vector = uiview.WorldPointToGUI(uiview.uiCamera, targetPos);
        Vector2 vector2 = vector;
        Vector2 screenResolution = uiview.GetScreenResolution();
        if (vector2.x + targetSize.x >= screenResolution.x) {
            vector2.x = screenResolution.x - targetSize.x;
        }
        if (vector2.x < 0f) {
            vector2.x = 0f;
        }
        if (vector2.y + targetSize.y >= screenResolution.y) {
            vector2.y = screenResolution.y - targetSize.y;
        }
        if (vector2.y < 0f) {
            vector2.y = 0f;
        }
        targetPos.x += (vector2.x - vector.x) * num;
        targetPos.y += (vector.y - vector2.y) * num;
        return targetPos;
    }

    public void ClosePopup(bool allowOverride = true) {
        if (popup is null) {
            return;
        }
        popup.eventLeaveFocus -= PopupLostFocus;
        popup.EventSelectedIndexChanged -= PopupSelectedIndexChanged;
        popup.EventItemClicked -= PopupItemClicked;
        popup.eventKeyDown -= PopupKeyDown;
        if (!allowOverride) {
            UnityEngine.Object.Destroy(popup.gameObject);
            popup = null;
            return;
        }
        bool flag = false;
        EventDropdownClosed?.Invoke(this, popup);
        if (!flag) {
            flag = Invoke("OnDropdownClose", new object[] { popup });
        }
        if (!flag) {
            UnityEngine.Object.Destroy(popup.gameObject);
        }
        popup = null;
    }
    private void PopupKeyDown(UIComponent component, UIKeyEventParameter p) {
        if (p.keycode == KeyCode.Escape || p.keycode == KeyCode.Space || p.keycode == KeyCode.Return) {
            ClosePopup(true);
            Focus();
            p.Use();
        }
    }
    private void PopupItemClicked(UIComponent component, int selectedIndex) {
        Focus();
    }
    private void PopupLostFocus(UIComponent component, UIFocusEventParameter p) {
        if (popup != null && !popup.containsFocus && !m_IsMouseHovering) {
            ClosePopup(true);
        }
    }
    private void PopupSelectedIndexChanged(UIComponent component, int si) {
        SelectedIndex = si;
        Invalidate();
    }
    protected override void RequestCharacterInfo() {
        UIDynamicFont uidynamicFont = Font as UIDynamicFont;
        if (uidynamicFont == null) {
            return;
        }
        if (!UIFontManager.IsDirty(Font)) {
            return;
        }
        string selectedValue = SelectedValue;
        if (string.IsNullOrEmpty(selectedValue)) {
            return;
        }
        float num = textScale * GetTextScaleMultiplier();
        int fontSize = Mathf.CeilToInt(Font.size * num);
        uidynamicFont.AddCharacterRequest(selectedValue, fontSize, FontStyle.Normal);
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
    protected override void OnLeaveFocus(UIFocusEventParameter p) {
        State = (containsMouse ? SpriteState.Hovered : SpriteState.Normal);
        base.OnLeaveFocus(p);
    }
    protected override void OnMouseUp(UIMouseEventParameter p) {
        if (m_IsMouseHovering) {
            if (containsFocus) {
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
        if (isEnabled && State != SpriteState.Focused) {
            State = SpriteState.Pressed;
        }
        base.OnMouseDown(p);

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
    public enum PopupListPosition {
        Below,
        Above,
        Automatic
    }
}

