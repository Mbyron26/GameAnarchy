using ColossalFramework.UI;
using ColossalFramework;
using System;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class CustomUIListBox : CustomUITextComponent {
    protected string bgSprite = string.Empty;
    protected Color32 bgNormalColor = Color.white;
    protected Color32 bgDisabledColor = Color.white;
    protected string itemSelectionSprite = string.Empty;
    protected string itemHoverSprite = string.Empty;
    protected Color32 itemHoverColor = Color.white;
    protected Color32 itemSelectionColor = Color.white;
    protected Color32 itemTextNormalColor = Color.white;
    protected Color32 itemTextDisabledColor = Color.white;
    protected RectOffset listPadding;
    protected RectOffset itemPadding;
    protected int itemHeight = 25;
    protected string[] items = new string[0];
    protected int[] filteredItems = new int[0];
    protected int selectedIndex = -1;
    protected UIScrollbar scrollbar;
    private float scrollPosition;
    protected bool animateHover;
    protected bool multilineItems;
    private bool eventsAttached;
    private int hoverIndex = -1;
    private float hoverTweenLocation;
    //private Vector2 touchStartPosition = Vector2.zero;

    public event PropertyChangedEventHandler<int> EventSelectedIndexChanged;
    public event PropertyChangedEventHandler<int> EventItemClicked;
    public event PropertyChangedEventHandler<int> EventItemDoubleClicked;
    public event PropertyChangedEventHandler<int> EventItemMouseDown;
    public event PropertyChangedEventHandler<int> EventItemMouseUp;
    public event PropertyChangedEventHandler<int> EventItemMouseHover;

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
            }
        }
    }
    public Color32 BgDisabledColor {
        get => bgDisabledColor;
        set {
            if (!bgDisabledColor.Equals(value)) {
                bgDisabledColor = value;
                Invalidate();
            }
        }
    }
    public string ItemSelectionSprite {
        get => itemSelectionSprite;
        set {
            if (value != itemSelectionSprite) {
                itemSelectionSprite = value;
                Invalidate();
            }
        }
    }
    public string ItemHoverSprite {
        get => itemHoverSprite;
        set {
            if (value != itemHoverSprite) {
                itemHoverSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 ItemHoverColor {
        get => itemHoverColor;
        set {
            if (!value.Equals(itemHoverColor)) {
                itemHoverColor = value;
                Invalidate();
            }
        }
    }
    public Color32 ItemSelectionColor {
        get => itemSelectionColor;
        set {
            if (!value.Equals(itemSelectionColor)) {
                itemSelectionColor = value;
                Invalidate();
            }
        }
    }
    public Color32 ItemTextNormalColor {
        get => itemTextNormalColor;
        set {
            if (!value.Equals(itemTextNormalColor)) {
                itemTextNormalColor = value;
                Invalidate();
            }
        }
    }
    public Color32 ItemTextDisabledColor {
        get => itemTextDisabledColor;
        set {
            if (!value.Equals(itemTextDisabledColor)) {
                itemTextDisabledColor = value;
                Invalidate();
            }
        }
    }
    public RectOffset ListPadding {
        get => listPadding ??= new RectOffset();
        set {
            value = value.ConstrainPadding();
            if (!Equals(value, listPadding)) {
                listPadding = value;
                Invalidate();
            }
        }
    }
    public RectOffset ItemPadding {
        get => itemPadding ??= new RectOffset();
        set {
            value = value.ConstrainPadding();
            if (!value.Equals(itemPadding)) {
                itemPadding = value;
                Invalidate();
            }
        }
    }
    public int ItemHeight {
        get => itemHeight;
        set {
            scrollPosition = 0f;
            value = Mathf.Max(1, value);
            if (value != itemHeight) {
                itemHeight = value;
                Invalidate();
            }
        }
    }
    public string[] Items {
        get => items ??= new string[0];
        set {
            if (value != items) {
                SelectedIndex = -1;
                scrollPosition = 0f;
                value ??= new string[0];
                items = value;
                Invalidate();
            }
        }
    }
    public int[] FilteredItems { set => filteredItems = value; }
    public int SelectedIndex {
        get => selectedIndex;
        set {
            value = Mathf.Max(-1, value);
            value = Mathf.Min(items.Length - 1, value);
            if (!IsFilteredItem(value) && value != selectedIndex) {
                selectedIndex = value;
                EnsureVisible(value);
                OnSelectedIndexChanged();
                Invalidate();
            }
        }
    }
    public UIScrollbar Scrollbar {
        get => scrollbar;
        set {
            scrollPosition = 0f;
            if (value != scrollbar) {
                DetachScrollbarEvents();
                scrollbar = value;
                AttachScrollbarEvents();
                Invalidate();
            }
        }
    }
    public float ScrollPosition {
        get => scrollPosition;
        set {
            if (!Mathf.Approximately(value, scrollPosition)) {
                scrollPosition = ConstrainScrollPosition(value);
                Invalidate();
            }
        }
    }
    public bool AnimateHover {
        get => animateHover;
        set {
            animateHover = value;
        }
    }
    public bool MultilineItems {
        get => multilineItems;
        set {
            if (value != multilineItems) {
                multilineItems = value;
                Invalidate();
            }
        }
    }

    private bool IsFilteredItem(int idx) => idx != -1 && Array.Exists(filteredItems, (int c) => c == idx);
    private bool FindFiltered(int desiredIndex) {
        if (Array.Exists(filteredItems, (int c) => c == desiredIndex)) {
            return true;
        }
        SelectedIndex = desiredIndex;
        return false;
    }
    private bool FindPrev(int desiredIndex) {
        while (desiredIndex >= 0) {
            if (!Array.Exists(filteredItems, (int c) => c == desiredIndex)) {
                SelectedIndex = desiredIndex;
                return true;
            }
            desiredIndex--;
        }
        return false;
    }
    private bool FindNext(int desiredIndex) {
        while (desiredIndex < items.Length) {
            if (!Array.Exists(filteredItems, (int c) => c == desiredIndex)) {
                SelectedIndex = desiredIndex;
                return true;
            }
            desiredIndex++;
        }
        return false;
    }

    protected virtual void OnSelectedIndexChanged() {
        EventSelectedIndexChanged?.Invoke(this, SelectedIndex);
        InvokeUpward("OnSelectedIndexChanged", new object[] { SelectedIndex });
    }
    protected virtual void OnItemMouseDown() {
        EventItemMouseDown?.Invoke(this, SelectedIndex);
        Invoke("OnItemMouseDown", new object[] { SelectedIndex });
    }
    protected virtual void OnItemMouseUp() {
        EventItemMouseUp?.Invoke(this, SelectedIndex);
        Invoke("OnItemMouseUp", new object[] { SelectedIndex });
    }
    protected virtual void OnItemMouseHover() {
        EventItemMouseHover?.Invoke(this, hoverIndex);
        Invoke("OnItemMouseHover", new object[] { hoverIndex });
    }
    protected virtual void OnItemClick() {
        EventItemClicked?.Invoke(this, SelectedIndex);
        InvokeUpward("OnItemClick", new object[] { SelectedIndex });
    }
    protected virtual void OnItemDoubleClick() {
        EventItemDoubleClicked?.Invoke(this, SelectedIndex);
        InvokeUpward("OnItemDoubleClick", new object[] { SelectedIndex });
    }

    protected override void OnGotFocus(UIFocusEventParameter p) {
        base.OnGotFocus(p);
        Invalidate();
    }
    protected override void OnLostFocus(UIFocusEventParameter p) {
        base.OnLostFocus(p);
        Invalidate();
    }
    protected override void OnMouseHover(UIMouseEventParameter p) {
        base.OnMouseHover(p);
        if (hoverIndex < Items.Length) {
            OnItemMouseHover();
        }
        Invoke("OnItemMouseHover", new object[] { SelectedIndex });
    }
    protected override void OnMouseMove(UIMouseEventParameter p) {
        base.OnMouseMove(p);
        UpdateItemHover(p);
    }
    protected override void OnMouseEnter(UIMouseEventParameter p) {
        Invalidate();
        base.OnMouseEnter(p);
        //touchStartPosition = p.position;
    }
    protected override void OnMouseLeave(UIMouseEventParameter p) {
        Invalidate();
        base.OnMouseLeave(p);
        hoverIndex = -1;
    }
    protected override void OnMouseWheel(UIMouseEventParameter p) {
        base.OnMouseWheel(p);
        ScrollPosition = Mathf.Max(0f, ScrollPosition - ((int)p.wheelDelta * ItemHeight));
        SynchronizeScrollbar();
        UpdateItemHover(p);
    }
    protected override void OnMouseUp(UIMouseEventParameter p) {
        hoverIndex = -1;
        base.OnMouseUp(p);
        OnItemMouseUp();
    }
    protected override void OnMouseDown(UIMouseEventParameter p) {
        base.OnMouseDown(p);
        int itemUnderMouse = GetItemUnderMouse(p);
        if (itemUnderMouse > -1 && itemUnderMouse < items.Length) {
            SelectedIndex = itemUnderMouse;
            OnItemMouseDown();
        }
    }
    protected override void OnClick(UIMouseEventParameter p) {
        base.OnClick(p);
        int itemUnderMouse = GetItemUnderMouse(p);
        if (itemUnderMouse == SelectedIndex) {
            OnItemClick();
        }
    }
    protected override void OnDoubleClick(UIMouseEventParameter p) {
        base.OnDoubleClick(p);
        int itemUnderMouse = GetItemUnderMouse(p);
        if (itemUnderMouse == SelectedIndex) {
            OnItemDoubleClick();
        }
    }
    protected override void OnKeyDown(UIKeyEventParameter p) {
        if (builtinKeyNavigation) {
            int num = SelectedIndex;
            switch (p.keycode) {
                case KeyCode.UpArrow:
                    num = Mathf.Max(0, SelectedIndex - 1);
                    if (FindFiltered(num) && FindNext(num)) {
                        FindPrev(num);
                    }
                    break;
                case KeyCode.DownArrow:
                    num++;
                    if (FindFiltered(num) && FindPrev(num)) {
                        FindNext(num);
                    }
                    break;
                case KeyCode.Home:
                    num = 0;
                    if (FindFiltered(num) && FindNext(num)) {
                        FindPrev(num);
                    }
                    break;
                case KeyCode.End:
                    num = Items.Length;
                    if (FindFiltered(num) && FindPrev(num)) {
                        FindNext(num);
                    }
                    break;
                case KeyCode.PageUp: {
                        int b = SelectedIndex - Mathf.FloorToInt((size.y - ListPadding.vertical) / ItemHeight);
                        num = Mathf.Max(0, b);
                        if (FindFiltered(num) && FindNext(num)) {
                            FindPrev(num);
                        }
                        break;
                    }
                case KeyCode.PageDown:
                    num += Mathf.FloorToInt((size.y - ListPadding.vertical) / ItemHeight);
                    if (FindFiltered(num) && FindPrev(num)) {
                        FindNext(num);
                    }
                    break;
            }
        }
        base.OnKeyDown(p);
    }
    protected override void OnKeyPress(UIKeyEventParameter p) {
        if (builtinKeyNavigation) {
            if (p.keycode == KeyCode.Space || p.keycode == KeyCode.Return) {
                OnDoubleClick(new UIMouseEventParameter(this, UIMouseButton.Left, 1, default, Vector2.zero, Vector2.zero, 0f));
                return;
            }
            if (!char.IsControl(p.character)) {
                bool flag = false;
                if (SelectedIndex >= 0 && items[SelectedIndex].StartsWith(p.character.ToString(), (StringComparison)5)) {
                    int num = SelectedIndex + 1;
                    if (num < items.Length) {
                        bool flag2 = false;
                        for (int i = num; i < items.Length; i++) {
                            if (items[i].StartsWith(p.character.ToString(), (StringComparison)5)) {
                                SelectedIndex = i;
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2) {
                            flag = true;
                        }
                    } else {
                        flag = true;
                    }
                } else {
                    flag = true;
                }
                if (flag) {
                    for (int j = 0; j < items.Length; j++) {
                        if (items[j].StartsWith(p.character.ToString(), (StringComparison)5)) {
                            SelectedIndex = j;
                            break;
                        }
                    }
                }
            }
        }
        base.OnKeyPress(p);
    }

    private int GetItemUnderMouse(UIMouseEventParameter p) {
        float num4 = GetHitPosition(p).y - ListPadding.top;
        if (num4 < 0f || num4 > size.y - ListPadding.bottom) {
            return -1;
        }
        return (int)((ScrollPosition + num4) / ItemHeight);
    }
    private void UpdateItemHover(UIMouseEventParameter p) {
        if (!Application.isPlaying) {
            return;
        }
        Ray ray = p.ray;
        if (!Raycast(ray)) {
            hoverIndex = -1;
            hoverTweenLocation = 0f;
            return;
        }
        GetHitPosition(ray, out Vector2 vector);
        float num4 = vector.y - ListPadding.top;
        int num5 = (int)(ScrollPosition + num4) / ItemHeight;
        if (num5 != hoverIndex) {
            hoverIndex = num5;
            Invalidate();
        }
    }

    public override void Update() {
        base.Update();
        if (m_Size == Vector2.zero) {
            m_Size = new Vector2(200f, 150f);
        }
        if (AnimateHover && hoverIndex != -1) {
            float num = (hoverIndex * ItemHeight) * PixelsToUnits();
            if (Mathf.Abs(hoverTweenLocation - num) < 1f) {
                Invalidate();
            }
        }
        if (m_IsComponentInvalidated) {
            SynchronizeScrollbar();
        }
    }
    public override void LateUpdate() {
        base.LateUpdate();
        if (!Application.isPlaying) {
            return;
        }
        AttachScrollbarEvents();
    }
    public override void OnDisable() {
        base.OnDisable();
        DetachScrollbarEvents();
    }

    private void SynchronizeScrollbar() {
        if (Scrollbar == null) {
            return;
        }
        int num = items.Length * ItemHeight;
        float scrollSize = size.y - ListPadding.vertical;
        scrollbar.incrementAmount = ItemHeight;
        scrollbar.minValue = 0f;
        scrollbar.maxValue = num;
        scrollbar.scrollSize = scrollSize;
        scrollbar.value = ScrollPosition;
    }
    private void DetachScrollbarEvents() {
        if (scrollbar == null || !eventsAttached) {
            return;
        }
        eventsAttached = false;
        scrollbar.eventValueChanged -= ScrollbarValueChanged;
        scrollbar.eventGotFocus -= ScrollbarGotFocus;
    }
    private void AttachScrollbarEvents() {
        if (scrollbar == null || eventsAttached) {
            return;
        }
        eventsAttached = true;
        scrollbar.eventValueChanged += ScrollbarValueChanged;
        scrollbar.eventGotFocus += ScrollbarGotFocus;
    }

    private void ScrollbarGotFocus(UIComponent component, UIFocusEventParameter p) => Focus();
    private void ScrollbarValueChanged(UIComponent component, float value) => ScrollPosition = value;
    public void EnsureVisible(int index) {
        int num = index * ItemHeight;
        if (ScrollPosition > num) {
            ScrollPosition = num;
        }
        float num2 = size.y - ListPadding.vertical;
        if (ScrollPosition + num2 < (num + ItemHeight)) {
            ScrollPosition = num - num2 + ItemHeight;
        }
    }
    private float ConstrainScrollPosition(float value) {
        value = Mathf.Max(0f, value);
        int num = items.Length * ItemHeight;
        float num2 = size.y - ListPadding.vertical;
        if (num < num2) {
            return 0f;
        }
        return Mathf.Min(value, num - num2);
    }

    protected override void OnRebuildRenderData() {
        if (Atlas is null || Font is null || !Font.isValid) {
            return;
        }
        renderData.material = Atlas.material;
        RenderBackground();
        int count = renderData.vertices.Count;
        RenderHover();
        RenderSelection();
        RenderItems();
        ClipQuads(renderData, count);
        ClipQuads(textRenderData, 0);
    }
    protected virtual void RenderBackground() {
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
            UISlicedSpriteRender.RenderSprite(renderData, options);
            return;
        }
        UISpriteRender.RenderSprite(renderData, options);
    }
    private void RenderItems() {
        if (Items is null || Items.Length == 0) {
            return;
        }
        if (textRenderData is not null) {
            textRenderData.Clear();
        } else {
            textRenderData = UIRenderData.Obtain();
            m_RenderData.Add(textRenderData);
        }
        textRenderData.material = Atlas.material;
        float num = PixelsToUnits();
        Vector2 maxSize = new(size.x - ItemPadding.horizontal - ListPadding.horizontal, (ItemHeight - ItemPadding.vertical));
        Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 vector2 = new Vector3(vector.x + ItemPadding.left + ListPadding.left, vector.y - ItemPadding.top - ListPadding.top, 0f) * num;
        vector2.y += ScrollPosition * num;
        Color32 defaultColor = isEnabled ? ItemTextNormalColor : ItemTextDisabledColor;
        float textScale = base.textScale * GetTextScaleMultiplier();
        float num2 = vector.y * size.y * num;
        float num3 = (vector.y * size.y - size.y) * num;
        for (int i = 0; i < Items.Length; i++) {
            using UIFontRenderer uifontRenderer = Font.ObtainRenderer();
            if (uifontRenderer is UIDynamicFont.DynamicFontRenderer dynamicFontRenderer) {
                dynamicFontRenderer.spriteAtlas = Atlas;
                dynamicFontRenderer.spriteBuffer = renderData;
            }
            uifontRenderer.wordWrap = false;
            uifontRenderer.maxSize = maxSize;
            uifontRenderer.pixelRatio = num;
            uifontRenderer.textScale = textScale;
            uifontRenderer.characterSpacing = CharacterSpacing;
            uifontRenderer.vectorOffset = vector2.Quantize(num);
            uifontRenderer.multiLine = MultilineItems;
            uifontRenderer.textAlign = TextHorizontalAlignment;
            uifontRenderer.processMarkup = ProcessMarkup;
            uifontRenderer.colorizeSprites = ColorizeSprites;
            uifontRenderer.defaultColor = defaultColor;
            uifontRenderer.bottomColor = UseTextGradient ? gradientBottomNormalColor : GradientBottomDisabledColor;
            uifontRenderer.overrideMarkupColors = false;
            uifontRenderer.opacity = CalculateOpacity();
            uifontRenderer.outline = UseOutline;
            uifontRenderer.outlineSize = OutlineSize;
            uifontRenderer.outlineColor = OutlineColor;
            uifontRenderer.shadow = UseDropShadow;
            uifontRenderer.shadowColor = DropShadowColor;
            uifontRenderer.shadowOffset = DropShadowOffset;
            var flag = !autoSize && TextVerticalAlignment != UIVerticalAlignment.Top;
            if (vector2.y - ItemHeight * num <= num2) {
                if (flag) {
                    uifontRenderer.vectorOffset = GetVertAlignOffset(uifontRenderer, Items[i]);
                }
                uifontRenderer.Render(Items[i], textRenderData);
            }
            vector2.y -= ItemHeight * num;
            uifontRenderer.vectorOffset = vector2;
            if (vector2.y < num3) {
                break;
            }
        }
    }
    private Vector3 GetVertAlignOffset(UIFontRenderer fontRenderer, string text) {
        float num = PixelsToUnits();
        Vector2 vector = fontRenderer.MeasureString(text) * num;
        Vector3 vectorOffset = fontRenderer.vectorOffset;
        float num2 = (itemHeight - TextPadding.vertical) * num;
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
    private void RenderSelection() {
        if (SelectedIndex < 0) {
            return;
        }
        UITextureAtlas.SpriteInfo spriteInfo = Atlas[ItemSelectionSprite];
        if (spriteInfo == null) {
            return;
        }
        float pixelsToUnits = PixelsToUnits();
        Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 offset = new(vector.x + ListPadding.left, vector.y - ListPadding.top + ScrollPosition, 0f);
        offset.y -= (SelectedIndex * ItemHeight);
        Color32 color = ApplyOpacity(ItemSelectionColor);
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            pixelsToUnits = pixelsToUnits,
            size = new Vector3(size.x - ListPadding.horizontal, ItemHeight),
            spriteInfo = spriteInfo,
            offset = offset
        };
        if (spriteInfo.isSliced) {
            UISlicedSpriteRender.RenderSprite(renderData, options);
            return;
        }
        UISpriteRender.RenderSprite(renderData, options);
    }
    private void RenderHover() {
        if (!Application.isPlaying) {
            return;
        }
        bool flag = Atlas == null || !isEnabled || hoverIndex < 0 || hoverIndex > Items.Length - 1 || string.IsNullOrEmpty(ItemHoverSprite) || IsFilteredItem(hoverIndex);
        if (flag) {
            return;
        }
        UITextureAtlas.SpriteInfo spriteInfo = Atlas[ItemHoverSprite];
        if (spriteInfo == null) {
            return;
        }
        Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 offset = new(vector.x + ListPadding.left, vector.y - ListPadding.top + ScrollPosition, 0f);
        float num = PixelsToUnits();
        int num2 = hoverIndex * ItemHeight;
        if (AnimateHover) {
            float num3 = Mathf.Abs(hoverTweenLocation - num2);
            float num4 = (size.y - ListPadding.vertical) * 0.5f;
            if (num3 > num4) {
                hoverTweenLocation = num2 + Mathf.Sign(hoverTweenLocation - num2) * num4;
            }
            float maxDelta = Time.deltaTime / num * 2f;
            hoverTweenLocation = Mathf.MoveTowards(hoverTweenLocation, num2, maxDelta);
        } else {
            hoverTweenLocation = num2;
        }
        offset.y -= hoverTweenLocation.Quantize(num);
        Color32 color = ApplyOpacity(ItemHoverColor);
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            pixelsToUnits = PixelsToUnits(),
            size = new Vector3(size.x - ListPadding.horizontal, ItemHeight),
            spriteInfo = spriteInfo,
            offset = offset
        };
        if (spriteInfo.isSliced) {
            UISlicedSpriteRender.RenderSprite(renderData, options);
        } else {
            UISpriteRender.RenderSprite(renderData, options);
        }
        if (num2 != hoverTweenLocation) {
            Invalidate();
        }
    }
    private void ClipQuads(UIRenderData data, int startIndex) {
        PoolList<Vector3> vertices = data.vertices;
        PoolList<Vector2> uvs = data.uvs;
        float num = PixelsToUnits();
        float num2 = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset).y - ListPadding.top) * num;
        float num3 = num2 - (size.y - ListPadding.vertical) * num;
        for (int i = startIndex; i < vertices.Count; i += 4) {
            Vector3 value = vertices[i];
            Vector3 value2 = vertices[i + 1];
            Vector3 value3 = vertices[i + 2];
            Vector3 value4 = vertices[i + 3];
            float num4 = value.y - value4.y;
            if (value4.y < num3) {
                float t = 1f - Mathf.Abs(-num3 + value.y) / num4;
                PoolList<Vector3> poolList = vertices;
                int index = i;
                value = new Vector3(value.x, Mathf.Max(value.y, num3), value2.z);
                value = (poolList[index] = value);
                PoolList<Vector3> poolList2 = vertices;
                int index2 = i + 1;
                value2 = new Vector3(value2.x, Mathf.Max(value2.y, num3), value2.z);
                value2 = (poolList2[index2] = value2);
                PoolList<Vector3> poolList3 = vertices;
                int index3 = i + 2;
                value3 = new Vector3(value3.x, Mathf.Max(value3.y, num3), value3.z);
                value3 = (poolList3[index3] = value3);
                PoolList<Vector3> poolList4 = vertices;
                int index4 = i + 3;
                value4 = new Vector3(value4.x, Mathf.Max(value4.y, num3), value4.z);
                value4 = (poolList4[index4] = value4);
                uvs[i + 3] = Vector2.Lerp(uvs[i + 3], uvs[i], t);
                uvs[i + 2] = Vector2.Lerp(uvs[i + 2], uvs[i + 1], t);
                num4 = Mathf.Abs(value4.y - value.y);
            }
            if (value.y > num2) {
                float t2 = Mathf.Abs(num2 - value.y) / num4;
                vertices[i] = new Vector3(value.x, Mathf.Min(num2, value.y), value.z);
                vertices[i + 1] = new Vector3(value2.x, Mathf.Min(num2, value2.y), value2.z);
                vertices[i + 2] = new Vector3(value3.x, Mathf.Min(num2, value3.y), value3.z);
                vertices[i + 3] = new Vector3(value4.x, Mathf.Min(num2, value4.y), value4.z);
                uvs[i] = Vector2.Lerp(uvs[i], uvs[i + 3], t2);
                uvs[i + 1] = Vector2.Lerp(uvs[i + 1], uvs[i + 2], t2);
            }
        }
    }
    protected override void RequestCharacterInfo() {
        UIDynamicFont uidynamicFont = Font as UIDynamicFont;
        if (uidynamicFont == null) {
            return;
        }
        if (!UIFontManager.IsDirty(Font)) {
            return;
        }
        if (items == null || items.Length == 0) {
            return;
        }
        float num = textScale * GetTextScaleMultiplier();
        int fontSize = Mathf.CeilToInt(Font.size * num);
        for (int i = 0; i < items.Length; i++) {
            uidynamicFont.AddCharacterRequest(items[i], fontSize, FontStyle.Normal);
        }
    }
}

