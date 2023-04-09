using ColossalFramework;
using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class ListBox : UIInteractiveComponent {
        public Color32 SelectionColor { get; set; } = Color.white;
        public Color32 ItemHovereColor { get; set; } = Color.white;

        public event PropertyChangedEventHandler<int> eventSelectedIndexChanged;

        public event PropertyChangedEventHandler<int> eventItemClicked;

        public event PropertyChangedEventHandler<int> eventItemDoubleClicked;

        public event PropertyChangedEventHandler<int> eventItemMouseDown;

        public event PropertyChangedEventHandler<int> eventItemMouseUp;

        public event PropertyChangedEventHandler<int> eventItemMouseHover;

        public float scrollPosition {
            get {
                return this.m_ScrollPosition;
            }
            set {
                if (!Mathf.Approximately(value, this.m_ScrollPosition)) {
                    this.m_ScrollPosition = this.ConstrainScrollPosition(value);
                    this.Invalidate();
                }
            }
        }

        public string itemHighlight {
            get {
                return this.m_ItemHighlight;
            }
            set {
                if (value != this.m_ItemHighlight) {
                    this.m_ItemHighlight = value;
                    this.Invalidate();
                }
            }
        }


        public string itemHover {
            get {
                return this.m_ItemHover;
            }
            set {
                if (value != this.m_ItemHover) {
                    this.m_ItemHover = value;
                    this.Invalidate();
                }
            }
        }

        public int[] filteredItems {
            set {
                this.m_FilteredItems = value;
            }
        }


        public int selectedIndex {
            get {
                return this.m_SelectedIndex;
            }
            set {
                value = Mathf.Max(-1, value);
                value = Mathf.Min(this.m_Items.Length - 1, value);
                if (!this.IsFilteredItem(value) && value != this.m_SelectedIndex) {
                    this.m_SelectedIndex = value;
                    this.EnsureVisible(value);
                    this.OnSelectedIndexChanged();
                    this.Invalidate();
                }
            }
        }

        private bool IsFilteredItem(int idx) {
            return idx != -1 && Array.Exists<int>(this.m_FilteredItems, (int c) => c == idx);
        }


        public RectOffset itemPadding {
            get {
                if (this.m_ItemPadding == null) {
                    this.m_ItemPadding = new RectOffset();
                }
                return this.m_ItemPadding;
            }
            set {
                value = value.ConstrainPadding();
                if (!value.Equals(this.m_ItemPadding)) {
                    this.m_ItemPadding = value;
                    this.Invalidate();
                }
            }
        }


        public Color32 itemTextColor {
            get {
                return this.m_ItemTextColor;
            }
            set {
                if (!value.Equals(this.m_ItemTextColor)) {
                    this.m_ItemTextColor = value;
                    this.Invalidate();
                }
            }
        }

        public int itemHeight {
            get {
                return this.m_ItemHeight;
            }
            set {
                this.m_ScrollPosition = 0f;
                value = Mathf.Max(1, value);
                if (value != this.m_ItemHeight) {
                    this.m_ItemHeight = value;
                    this.Invalidate();
                }
            }
        }

        public string[] items {
            get {
                if (this.m_Items == null) {
                    this.m_Items = new string[0];
                }
                return this.m_Items;
            }
            set {
                if (value != this.m_Items) {
                    this.selectedIndex = -1;
                    this.m_ScrollPosition = 0f;
                    if (value == null) {
                        value = new string[0];
                    }
                    this.m_Items = value;
                    this.Invalidate();
                }
            }
        }

        public UIScrollbar scrollbar {
            get {
                return this.m_Scrollbar;
            }
            set {
                this.scrollPosition = 0f;
                if (value != this.scrollbar) {
                    this.DetachScrollbarEvents();
                    this.m_Scrollbar = value;
                    this.AttachScrollbarEvents();
                    this.Invalidate();
                }
            }
        }


        public RectOffset listPadding {
            get {
                if (this.m_ListPadding == null) {
                    this.m_ListPadding = new RectOffset();
                }
                return this.m_ListPadding;
            }
            set {
                value = value.ConstrainPadding();
                if (!object.Equals(value, this.m_ListPadding)) {
                    this.m_ListPadding = value;
                    this.Invalidate();
                }
            }
        }

        public bool animateHover {
            get {
                return this.m_AnimateHover;
            }
            set {
                this.m_AnimateHover = value;
            }
        }

        public bool multilineItems {
            get {
                return this.m_MultilineItems;
            }
            set {
                if (value != this.m_MultilineItems) {
                    this.m_MultilineItems = value;
                    this.Invalidate();
                }
            }
        }

        protected internal virtual void OnSelectedIndexChanged() {
            if (this.eventSelectedIndexChanged != null) {
                this.eventSelectedIndexChanged(this, this.selectedIndex);
            }
            InvokeUpward("OnSelectedIndexChanged", new object[]
            {
                this.selectedIndex
            });
        }

        // Token: 0x06000C52 RID: 3154 RVA: 0x000328FC File Offset: 0x00030AFC
        protected internal virtual void OnItemMouseDown() {
            if (this.eventItemMouseDown != null) {
                this.eventItemMouseDown(this, this.selectedIndex);
            }
            Invoke("OnItemMouseDown", new object[]
            {
                this.selectedIndex
            });
        }

        protected internal virtual void OnItemMouseUp() {
            if (this.eventItemMouseUp != null) {
                this.eventItemMouseUp(this, this.selectedIndex);
            }
            Invoke("OnItemMouseUp", new object[]
            {
                this.selectedIndex
            });
        }

        protected internal virtual void OnItemMouseHover() {
            if (this.eventItemMouseHover != null) {
                this.eventItemMouseHover(this, this.m_HoverIndex);
            }
            Invoke("OnItemMouseHover", new object[]
            {
                this.m_HoverIndex
            });
        }

        protected internal virtual void OnItemClick() {
            if (this.eventItemClicked != null) {
                this.eventItemClicked(this, this.selectedIndex);
            }
            InvokeUpward("OnItemClick", new object[]
            {
                this.selectedIndex
            });
        }

        protected internal virtual void OnItemDoubleClick() {
            if (this.eventItemDoubleClicked != null) {
                this.eventItemDoubleClicked(this, this.selectedIndex);
            }
            InvokeUpward("OnItemDoubleClick", new object[]
            {
                this.selectedIndex
            });
        }

        protected override void OnMouseHover(UIMouseEventParameter p) {
            base.OnMouseHover(p);
            if (m_HoverIndex < items.Length) {
                OnItemMouseHover();
            }
            Invoke("OnItemMouseHover", new object[]
            {
                selectedIndex
            });
        }

        protected override void OnMouseMove(UIMouseEventParameter p) {
            base.OnMouseMove(p);
            UpdateItemHover(p);
        }

        protected override void OnMouseEnter(UIMouseEventParameter p) {
            base.OnMouseEnter(p);
            m_TouchStartPosition = p.position;
        }

        protected override void OnMouseLeave(UIMouseEventParameter p) {
            base.OnMouseLeave(p);
            m_HoverIndex = -1;
        }

        protected override void OnMouseWheel(UIMouseEventParameter p) {
            base.OnMouseWheel(p);
            scrollPosition = Mathf.Max(0f, scrollPosition - ((int)p.wheelDelta * itemHeight));
            SynchronizeScrollbar();
            UpdateItemHover(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p) {
            m_HoverIndex = -1;
            base.OnMouseUp(p);
            OnItemMouseUp();
        }

        // Token: 0x06000C5D RID: 3165 RVA: 0x00032B4C File Offset: 0x00030D4C
        protected override void OnMouseDown(UIMouseEventParameter p) {
            base.OnMouseDown(p);
            int itemUnderMouse = GetItemUnderMouse(p);
            if (itemUnderMouse > -1 && itemUnderMouse < m_Items.Length) {
                selectedIndex = itemUnderMouse;
                OnItemMouseDown();
            }
        }

        protected override void OnClick(UIMouseEventParameter p) {
            base.OnClick(p);
            int itemUnderMouse = GetItemUnderMouse(p);
            if (itemUnderMouse == selectedIndex) {
                OnItemClick();
            }
        }


        protected override void OnDoubleClick(UIMouseEventParameter p) {
            base.OnDoubleClick(p);
            int itemUnderMouse = GetItemUnderMouse(p);
            if (itemUnderMouse == selectedIndex) {
                OnItemDoubleClick();
            }
        }

        private bool FindFiltered(int desiredIndex) {
            if (Array.Exists(m_FilteredItems, (int c) => c == desiredIndex)) {
                return true;
            }
            selectedIndex = desiredIndex;
            return false;
        }

        private bool FindPrev(int desiredIndex) {
            while (desiredIndex >= 0) {
                if (!Array.Exists(m_FilteredItems, (int c) => c == desiredIndex)) {
                    selectedIndex = desiredIndex;
                    return true;
                }
                desiredIndex--;
            }
            return false;
        }


        private bool FindNext(int desiredIndex) {
            while (desiredIndex < m_Items.Length) {
                if (!Array.Exists(m_FilteredItems, (int c) => c == desiredIndex)) {
                    selectedIndex = desiredIndex;
                    return true;
                }
                desiredIndex++;
            }
            return false;
        }

        protected override void OnKeyDown(UIKeyEventParameter p) {
            if (builtinKeyNavigation) {
                int num = selectedIndex;
                switch (p.keycode) {
                    case KeyCode.UpArrow:
                        num = Mathf.Max(0, selectedIndex - 1);
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
                        num = items.Length;
                        if (FindFiltered(num) && FindPrev(num)) {
                            FindNext(num);
                        }
                        break;
                    case KeyCode.PageUp: {
                            int b = selectedIndex - Mathf.FloorToInt((size.y - listPadding.vertical) / itemHeight);
                            num = Mathf.Max(0, b);
                            if (FindFiltered(num) && FindNext(num)) {
                                FindPrev(num);
                            }
                            break;
                        }
                    case KeyCode.PageDown:
                        num += Mathf.FloorToInt((size.y - listPadding.vertical) / itemHeight);
                        if (FindFiltered(num) && FindPrev(num)) {
                            FindNext(num);
                        }
                        break;
                }
            }
            base.OnKeyDown(p);
        }

        protected  override void OnKeyPress(UIKeyEventParameter p) {
            if (builtinKeyNavigation) {
                if (p.keycode == KeyCode.Space || p.keycode == KeyCode.Return) {
            OnDoubleClick(new UIMouseEventParameter(this, UIMouseButton.Left, 1, default(Ray), Vector2.zero, Vector2.zero, 0f));
                    return;
                }
                if (!char.IsControl(p.character)) {
                    bool flag = false;
                    int selectedIndex = this.selectedIndex;
                    if (this.selectedIndex >= 0 && this.m_Items[this.selectedIndex].StartsWith(p.character.ToString(), (StringComparison)5)) {
                        int num = this.selectedIndex + 1;
                        if (num < this.m_Items.Length) {
                            bool flag2 = false;
                            for (int i = num; i < this.m_Items.Length; i++) {
                                if (this.m_Items[i].StartsWith(p.character.ToString(), (StringComparison)5)) {
                                    this.selectedIndex = i;
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
                        for (int j = 0; j < m_Items.Length; j++) {
                            if (m_Items[j].StartsWith(p.character.ToString(), (StringComparison)5)) {
                                this.selectedIndex = j;
                                break;
                            }
                        }
                    }
                }
            }
            base.OnKeyPress(p);
        }

        private int GetItemUnderMouse(UIMouseEventParameter p) {
            float num = pivot.TransformToUpperLeft(size, arbitraryPivotOffset).y + ((float)(-(float)this.itemHeight) * ((float)this.selectedIndex - this.scrollPosition) - (float)this.listPadding.top);
            float num2 = ((float)this.selectedIndex - this.scrollPosition + 1f) * (float)this.itemHeight + (float)this.listPadding.vertical;
            float num3 = num2 - size.y;
            if (num3 > 0f) {
                num += num3;
            }
            float num4 = GetHitPosition(p).y - (float)this.listPadding.top;
            if (num4 < 0f || num4 > size.y - (float)this.listPadding.bottom) {
                return -1;
            }
            return (int)((this.scrollPosition + num4) / (float)this.itemHeight);
        }

        private void UpdateItemHover(UIMouseEventParameter p) {
            if (!Application.isPlaying) {
                return;
            }
            Ray ray = p.ray;
            if (!Raycast(ray)) {
                this.m_HoverIndex = -1;
                this.m_HoverTweenLocation = 0f;
                return;
            }
            Vector2 vector;
            GetHitPosition(ray, out vector);
            float num = pivot.TransformToUpperLeft(size, arbitraryPivotOffset).y + ((float)(-(float)this.itemHeight) * ((float)this.selectedIndex - this.scrollPosition) - (float)this.listPadding.top);
            float num2 = ((float)this.selectedIndex - this.scrollPosition + 1f) * (float)this.itemHeight + (float)this.listPadding.vertical;
            float num3 = num2 - size.y;
            if (num3 > 0f) {
                num += num3;
            }
            float num4 = vector.y - (float)this.listPadding.top;
            int num5 = (int)(this.scrollPosition + num4) / this.itemHeight;
            if (num5 != this.m_HoverIndex) {
                this.m_HoverIndex = num5;
                this.Invalidate();
            }
        }

        public override void Update() {
            base.Update();
            if (this.m_Size == Vector2.zero) {
                this.m_Size = new Vector2(200f, 150f);
            }
            if (this.animateHover && this.m_HoverIndex != -1) {
                float num = (float)(this.m_HoverIndex * this.itemHeight) * PixelsToUnits();
                if (Mathf.Abs(this.m_HoverTweenLocation - num) < 1f) {
                    this.Invalidate();
                }
            }
            if (this.m_IsComponentInvalidated) {
                this.SynchronizeScrollbar();
            }
        }

        public override void LateUpdate() {
            base.LateUpdate();
            if (!Application.isPlaying) {
                return;
            }
            this.AttachScrollbarEvents();
        }

        public override void OnDisable() {
            base.OnDisable();
            this.DetachScrollbarEvents();
        }

        private void SynchronizeScrollbar() {
            if (this.scrollbar == null) {
                return;
            }
            int num = this.m_Items.Length * this.itemHeight;
            float scrollSize = size.y - (float)this.listPadding.vertical;
            this.m_Scrollbar.incrementAmount = (float)this.itemHeight;
            this.m_Scrollbar.minValue = 0f;
            this.m_Scrollbar.maxValue = (float)num;
            this.m_Scrollbar.scrollSize = scrollSize;
            this.m_Scrollbar.value = this.scrollPosition;
        }

        private void DetachScrollbarEvents() {
            if (this.m_Scrollbar == null || !this.m_EventsAttached) {
                return;
            }
            this.m_EventsAttached = false;
            this.m_Scrollbar.eventValueChanged -= this.ScrollbarValueChanged;
            this.m_Scrollbar.eventGotFocus -= this.ScrollbarGotFocus;
        }

        private void AttachScrollbarEvents() {
            if (this.m_Scrollbar == null || this.m_EventsAttached) {
                return;
            }
            this.m_EventsAttached = true;
            this.m_Scrollbar.eventValueChanged += this.ScrollbarValueChanged;
            this.m_Scrollbar.eventGotFocus += this.ScrollbarGotFocus;
        }

        private void ScrollbarGotFocus(UIComponent component, UIFocusEventParameter p) {
            Focus();
        }

        private void ScrollbarValueChanged(UIComponent component, float value) {
            this.scrollPosition = value;
        }

        public void EnsureVisible(int index) {
            int num = index * this.itemHeight;
            if (this.scrollPosition > (float)num) {
                this.scrollPosition = (float)num;
            }
            float num2 = size.y - (float)this.listPadding.vertical;
            if (this.scrollPosition + num2 < (float)(num + this.itemHeight)) {
                this.scrollPosition = (float)num - num2 + (float)this.itemHeight;
            }
        }

        private float ConstrainScrollPosition(float value) {
            value = Mathf.Max(0f, value);
            int num = this.m_Items.Length * this.itemHeight;
            float num2 = size.y - (float)this.listPadding.vertical;
            if ((float)num < num2) {
                return 0f;
            }
            return Mathf.Min(value, (float)num - num2);
        }

        protected override void OnRebuildRenderData() {
            if (atlas == null || font == null || !font.isValid) {
                return;
            }
            if (textRenderData != null) {
                textRenderData.Clear();
            } else {
                UIRenderData item = UIRenderData.Obtain();
                this.m_RenderData.Add(item);
            }
            renderData.material = atlas.material;
            textRenderData.material = atlas.material;
            this.RenderBackground();
            int count = renderData.vertices.Count;
            this.RenderHover();
            this.RenderSelection();
            this.RenderItems();
            this.ClipQuads(renderData, count);
            this.ClipQuads(textRenderData, 0);
        }

        private void RenderItems() {
            if (font == null || this.items == null || this.items.Length == 0) {
                return;
            }
            float num = PixelsToUnits();
            Vector2 maxSize = new Vector2(size.x - (float)this.itemPadding.horizontal - (float)this.listPadding.horizontal, (float)(this.itemHeight - this.itemPadding.vertical));
            Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            Vector3 vector2 = new Vector3(vector.x + (float)this.itemPadding.left + (float)this.listPadding.left, vector.y - (float)this.itemPadding.top - (float)this.listPadding.top, 0f) * num;
            vector2.y += this.scrollPosition * num;
            Color32 defaultColor = isEnabled ? this.itemTextColor : disabledColor;
            float textScale = base.textScale * this.GetTextScaleMultiplier();
            float num2 = vector.y * size.y * num;
            float num3 = (vector.y * size.y - size.y) * num;
            for (int i = 0; i < this.items.Length; i++) {
                using (UIFontRenderer uifontRenderer = font.ObtainRenderer()) {
                    UIDynamicFont.DynamicFontRenderer dynamicFontRenderer = uifontRenderer as UIDynamicFont.DynamicFontRenderer;
                    if (dynamicFontRenderer != null) {
                        dynamicFontRenderer.spriteAtlas = atlas;
                        dynamicFontRenderer.spriteBuffer = renderData;
                    }
                    uifontRenderer.wordWrap = false;
                    uifontRenderer.maxSize = maxSize;
                    uifontRenderer.pixelRatio = num;
                    uifontRenderer.textScale = textScale;
                    uifontRenderer.characterSpacing = characterSpacing;
                    uifontRenderer.vectorOffset = vector2.Quantize(num);
                    uifontRenderer.multiLine = this.m_MultilineItems;
                    uifontRenderer.textAlign = UIHorizontalAlignment.Left;
                    uifontRenderer.processMarkup = processMarkup;
                    uifontRenderer.colorizeSprites = colorizeSprites;
                    uifontRenderer.defaultColor = defaultColor;
                    uifontRenderer.bottomColor = (useGradient ? new Color32?(bottomColor) : default(Color32?));
                    uifontRenderer.overrideMarkupColors = false;
                    uifontRenderer.opacity = CalculateOpacity();
                    uifontRenderer.outline = useOutline;
                    uifontRenderer.outlineSize = outlineSize;
                    uifontRenderer.outlineColor = outlineColor;
                    uifontRenderer.shadow = useDropShadow;
                    uifontRenderer.shadowColor = dropShadowColor;
                    uifontRenderer.shadowOffset = dropShadowOffset;
                    if (vector2.y - (float)this.itemHeight * num <= num2) {
                        uifontRenderer.Render(this.items[i], textRenderData);
                    }
                    vector2.y -= (float)this.itemHeight * num;
                    uifontRenderer.vectorOffset = vector2;
                    if (vector2.y < num3) {
                        break;
                    }
                }
            }
        }

        private void RenderSelection() {
            if (atlas == null || selectedIndex < 0) {
                return;
            }
            UITextureAtlas.SpriteInfo spriteInfo = atlas[itemHighlight];
            if (spriteInfo == null) {
                return;
            }
            float pixelsToUnits = PixelsToUnits();
            Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            var offset = new Vector3(vector.x + listPadding.left, vector.y - listPadding.top + scrollPosition, 0f);
            offset.y -= (selectedIndex * itemHeight);
            Color32 color = ApplyOpacity(SelectionColor);
            RenderOptions options = new() {
                atlas = atlas,
                color = color,
                fillAmount = 1f,
                flip = UISpriteFlip.None,
                pixelsToUnits = pixelsToUnits,
                size = new Vector3(size.x - listPadding.horizontal, itemHeight),
                spriteInfo = spriteInfo,
                offset = offset
            };
            if (spriteInfo.isSliced) {
                Render.UISlicedSpriteRender.RenderSprite(renderData, options);
                return;
            }
            Render.UISpriteRender.RenderSprite(renderData, options);
        }

        private void RenderHover() {
            if (!Application.isPlaying) {
                return;
            }
            bool flag = atlas == null || !isEnabled || m_HoverIndex < 0 || m_HoverIndex > items.Length - 1 || string.IsNullOrEmpty(this.itemHover) || IsFilteredItem(m_HoverIndex);
            if (flag) {
                return;
            }
            UITextureAtlas.SpriteInfo spriteInfo = atlas[itemHover];
            if (spriteInfo == null) {
                return;
            }
            Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            var offset = new Vector3(vector.x + listPadding.left, vector.y - listPadding.top + scrollPosition, 0f);
            float num = PixelsToUnits();
            int num2 = m_HoverIndex * itemHeight;
            if (animateHover) {
                float num3 = Mathf.Abs(m_HoverTweenLocation - num2);
                float num4 = (size.y - listPadding.vertical) * 0.5f;
                if (num3 > num4) {
                    m_HoverTweenLocation = num2 + Mathf.Sign(m_HoverTweenLocation - num2) * num4;
                }
                float maxDelta = Time.deltaTime / num * 2f;
                m_HoverTweenLocation = Mathf.MoveTowards(m_HoverTweenLocation, num2, maxDelta);
            } else {
                m_HoverTweenLocation = num2;
            }
            offset.y -= m_HoverTweenLocation.Quantize(num);
            Color32 color = ApplyOpacity(ItemHovereColor);
            RenderOptions options = new() {
                atlas = atlas,
                color = color,
                fillAmount = 1f,
                flip = UISpriteFlip.None,
                pixelsToUnits = PixelsToUnits(),
                size = new Vector3(size.x - listPadding.horizontal, itemHeight),
                spriteInfo = spriteInfo,
                offset = offset
            };
            if (spriteInfo.isSliced) {
                Render.UISlicedSpriteRender.RenderSprite(renderData, options);
            } else {
                Render.UISpriteRender.RenderSprite(renderData, options);
            }
            if (num2 != m_HoverTweenLocation) {
                Invalidate();
            }
        }

        private void ClipQuads(UIRenderData data, int startIndex) {
            PoolList<Vector3> vertices = data.vertices;
            PoolList<Vector2> uvs = data.uvs;
            float num = PixelsToUnits();
            float num2 = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset).y - listPadding.top) * num;
            float num3 = num2 - (size.y - listPadding.vertical) * num;
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
            UIDynamicFont uidynamicFont = font as UIDynamicFont;
            if (uidynamicFont == null) {
                return;
            }
            if (!UIFontManager.IsDirty(font)) {
                return;
            }
            if (m_Items == null || m_Items.Length == 0) {
                return;
            }
            float num = textScale * GetTextScaleMultiplier();
            int fontSize = Mathf.CeilToInt(font.size * num);
            for (int i = 0; i < m_Items.Length; i++) {
                uidynamicFont.AddCharacterRequest(m_Items[i], fontSize, FontStyle.Normal);
            }
        }

        // Token: 0x0400053C RID: 1340
        [SerializeField]
        protected RectOffset m_ListPadding;

        // Token: 0x0400053D RID: 1341
        [SerializeField]
        protected int m_SelectedIndex = -1;

        // Token: 0x0400053E RID: 1342
        [SerializeField]
        protected Color32 m_ItemTextColor = Color.white;

        // Token: 0x0400053F RID: 1343
        [SerializeField]
        protected int m_ItemHeight = 25;

        // Token: 0x04000540 RID: 1344
        [SerializeField]
        protected RectOffset m_ItemPadding;

        // Token: 0x04000541 RID: 1345
        [SerializeField]
        protected string[] m_Items = new string[0];

        // Token: 0x04000542 RID: 1346
        [SerializeField]
        protected int[] m_FilteredItems = new int[0];

        // Token: 0x04000543 RID: 1347
        [SerializeField]
        protected string m_ItemHighlight = "";

        // Token: 0x04000544 RID: 1348
        [SerializeField]
        protected string m_ItemHover = "";

        // Token: 0x04000545 RID: 1349
        [SerializeField]
        protected UIScrollbar m_Scrollbar;

        // Token: 0x04000546 RID: 1350
        [SerializeField]
        protected bool m_AnimateHover;

        // Token: 0x04000547 RID: 1351
        [SerializeField]
        protected bool m_MultilineItems;

        // Token: 0x04000548 RID: 1352
        private bool m_EventsAttached;

        // Token: 0x04000549 RID: 1353
        private float m_ScrollPosition;

        // Token: 0x0400054A RID: 1354
        private int m_HoverIndex = -1;

        // Token: 0x0400054B RID: 1355
        private float m_HoverTweenLocation;

        // Token: 0x0400054C RID: 1356
        private Vector2 m_TouchStartPosition = Vector2.zero;

    }
}
