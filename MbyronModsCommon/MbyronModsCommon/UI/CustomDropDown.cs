using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomDropDown {
        public static UIDropDown AddCPDropDown(UIComponent parent, string[] options, int defaultSelection, float width, float height, OnDropdownSelectionChanged eventCallback = null) {
            var dropDown = parent.AddUIComponent<UIDropDown>();
            dropDown.width = width;
            dropDown.height = height;
            dropDown.listWidth = (int)width;
            dropDown.itemHeight = (int)height;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.textFieldPadding = dropDown.itemPadding = new RectOffset(8, 0, 4, 0);
            dropDown.textScale = 0.8f;
            dropDown.atlas = CustomAtlas.MbyronModsAtlas;
            dropDown.normalBgSprite = CustomAtlas.FieldNormal;
            dropDown.disabledBgSprite = CustomAtlas.FieldDisabled;
            dropDown.hoveredBgSprite = CustomAtlas.FieldHovered;
            dropDown.focusedBgSprite = CustomAtlas.FieldNormal;
            dropDown.listBackground = CustomAtlas.FieldHovered;
            dropDown.itemHover = CustomAtlas.FieldNormal;
            dropDown.itemHighlight = CustomAtlas.FieldFocused;
            dropDown.popupColor = CustomColor.DefaultButtonNormal;
            dropDown.popupTextColor = CustomColor.White;
            dropDown.triggerButton = dropDown;
            dropDown.items = options;
            dropDown.selectedIndex = defaultSelection;
            var arrowDown = dropDown.AddUIComponent<UIPanel>();
            arrowDown.atlas = CustomAtlas.MbyronModsAtlas;
            arrowDown.backgroundSprite = CustomAtlas.ArrowDown1;
            arrowDown.autoSize = false;
            arrowDown.size = new Vector2(22, 22);
            arrowDown.disabledColor = new Color32(100, 100, 100, 255);
            arrowDown.isEnabled = dropDown.isEnabled;
            arrowDown.relativePosition = new Vector2(dropDown.width - 3 - 20, -1);
            dropDown.eventSelectedIndexChanged += (c, v) => eventCallback?.Invoke(v);
            return dropDown;
        }

        public static DropDown AddOPDropDown(UIComponent parent, string[] options, int defaultSelection, float width, float height, Action<int> callback = null) {
            var dropDown = parent.AddUIComponent<DropDown>();
            dropDown.IsRenderFgSprites = false;
            dropDown.Atlas = CustomAtlas.MbyronModsAtlas;
            dropDown.NormalBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.DisabledBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.HoveredBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.FocusedBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.PressedBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.NormalBgColor = CustomColor.DefaultButtonNormal;
            dropDown.HoveredBgColor = CustomColor.DefaultButtonHovered;
            dropDown.PressedBgColor = CustomColor.DefaultButtonPressed;
            dropDown.FocusedBgColor = CustomColor.DefaultButtonFocused;
            dropDown.DisabledBgColor = CustomColor.DefaultButtonDisabled;

            dropDown.ListBackgroundSprite = CustomAtlas.RoundedRectangle2;
            dropDown.ListBackgroundColor = CustomColor.DefaultButtonNormal;
            dropDown.ItemHighlightSprite = CustomAtlas.RoundedRectangle2;
            dropDown.ItemHighlightColor = CustomColor.BlueNormal;
            dropDown.ItemHoverSprite = CustomAtlas.RoundedRectangle2;
            dropDown.ItemHovereColor = CustomColor.DefaultButtonHovered;
            dropDown.disabledTextColor = CustomColor.DisabledTextColor;
            dropDown.Items = options;
            dropDown.ItemTextColor = CustomColor.White;
            dropDown.width = width;
            dropDown.height = height;
            dropDown.textScale = 1f;
            dropDown.useDropShadow = true;
            dropDown.ItemPadding = dropDown.TextFieldPadding = new(6, 6, 6, 0);
            dropDown.ListScrollbar = null;
            dropDown.ItemHeight = (int)height;
            dropDown.ListHeight = dropDown.ItemHeight * options.Length + 8;
            dropDown.SelectedIndex = defaultSelection;
            dropDown.ListPadding = new(4, 4, 4, 4);
            dropDown.disabledColor = CustomColor.DisabledTextColor;
            var arrowDown = dropDown.AddUIComponent<UIPanel>();
            arrowDown.atlas = CustomAtlas.MbyronModsAtlas;
            arrowDown.backgroundSprite = CustomAtlas.ArrowDown;
            arrowDown.size = new Vector2(26, 26);
            arrowDown.disabledColor = new Color32(100, 100, 100, 255);
            arrowDown.isEnabled = dropDown.isEnabled;
            arrowDown.relativePosition = new Vector2(dropDown.width - 26 - 4, 2);
            dropDown.TriggerButton = dropDown;
            dropDown.EventSelectedIndexChanged += (_) => callback?.Invoke(_);
            dropDown.CanWheel = false;
            return dropDown;
        }

    }

    public class DropDown : InteractiveComponent {
        protected UIComponent triggerButton;
        protected RectOffset textFieldPadding;
        protected int selectedIndex = -1;
        protected int[] filteredItems = new int[0];
        private bool eventsAttached;
        private ListBox m_Popup;
        private UIScrollbar m_ActiveScrollbar;

        protected string itemHoverSprite = string.Empty;
        protected string itemHighlightSprite = string.Empty;
        protected Color32 itemTextColor = Color.white;
        protected RectOffset itemPadding;
        protected int itemHeight = 25;
        protected string[] items = new string[0];

        protected string listBackgroundSprite = string.Empty;
        protected Color32 listBackgroundColor = Color.black;
        protected RectOffset listPadding;
        protected Vector2 listOffset = Vector2.zero;
        protected PopupListPosition listPosition;
        protected int listHeight = 200;
        protected UIScrollbar listScrollbar;

        public event PopupEventHandler EventDropdownOpen;
        public event PopupEventHandler EventDropdownClose;
        public event Action<int> EventSelectedIndexChanged;
        public event MouseEventHandler EventMouseWheelHandler;

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
        public RectOffset TextFieldPadding {
            get {
                textFieldPadding ??= new RectOffset();
                return textFieldPadding;
            }
            set {
                value = value.ConstrainPadding();
                if (!Equals(value, textFieldPadding)) {
                    textFieldPadding = value;
                    Invalidate();
                }
            }
        }
        public int SelectedIndex {
            get => selectedIndex;
            set {
                value = Mathf.Max(-1, value);
                value = Mathf.Min(Items.Length - 1, value);
                if (value != selectedIndex) {
                    if (m_Popup != null) {
                        m_Popup.selectedIndex = value;
                    }
                    selectedIndex = value;
                    OnSelectedIndexChanged();
                    Invalidate();
                }
            }
        }
        public string SelectedValue {
            get {
                if (selectedIndex < 0 || selectedIndex >= items.Length) {
                    return "";
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
        public int[] FilteredItems { set => filteredItems = value; }
        public bool CanWheel { get; set; } = true;

        #region Item
        public string ItemHoverSprite {
            get => itemHoverSprite;
            set {
                if (value != itemHoverSprite) {
                    itemHoverSprite = value;
                    Invalidate();
                }
            }
        }
        public Color32 ItemHovereColor { get; set; } = Color.white;
        public string ItemHighlightSprite {
            get => itemHighlightSprite;
            set {
                if (value != itemHighlightSprite) {
                    ClosePopup(true);
                    itemHighlightSprite = value;
                    Invalidate();
                }
            }
        }
        public Color32 ItemHighlightColor { get; set; } = Color.white;
        public Color32 ItemTextColor {
            get => itemTextColor;
            set {
                ClosePopup(true);
                itemTextColor = value;
                Invalidate();
            }
        }
        public RectOffset ItemPadding {
            get {
                itemPadding ??= new RectOffset();
                return itemPadding;
            }
            set {
                value = value.ConstrainPadding();
                if (!Equals(value, itemPadding)) {
                    itemPadding = value;
                    Invalidate();
                }
            }
        }
        public int ItemHeight {
            get => itemHeight;
            set {
                value = Mathf.Max(1, value);
                if (value != itemHeight) {
                    ClosePopup(true);
                    itemHeight = value;
                    Invalidate();
                }
            }
        }
        public string[] Items {
            get {
                items ??= new string[0];
                return items;
            }
            set {
                ClosePopup(true);
                value ??= new string[0];
                items = value;
                Invalidate();
            }
        }
        #endregion

        #region List
        public bool AutoListWidth { get; set; } = false;
        public int ListWidth { get; set; }
        public string ListBackgroundSprite {
            get => listBackgroundSprite;
            set {
                if (value != listBackgroundSprite) {
                    ClosePopup(true);
                    listBackgroundSprite = value;
                    Invalidate();
                }
            }
        }
        public Color32 ListBackgroundColor {
            get => listBackgroundColor;
            set {
                ClosePopup(true);
                listBackgroundColor = value;
                Invalidate();
            }
        }
        public RectOffset ListPadding {
            get {
                listPadding ??= new RectOffset();
                return listPadding;
            }
            set {
                value = value.ConstrainPadding();
                if (!Equals(value, listPadding)) {
                    listPadding = value;
                    Invalidate();
                }
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
        public bool ClampListToScreen { get; set; } = true;
        public int ListHeight {
            get => listHeight;
            set {
                listHeight = value;
                Invalidate();
            }
        }
        public UIScrollbar ListScrollbar {
            get => listScrollbar;
            set {
                if (value != listScrollbar) {
                    listScrollbar = value;
                    Invalidate();
                }
            }
        }
        #endregion


        protected internal virtual void OnSelectedIndexChanged() {
            EventSelectedIndexChanged?.Invoke(SelectedIndex);
            InvokeUpward("OnSelectedIndexChanged", new object[] { SelectedIndex });
        }

        protected override void OnKeyDown(UIKeyEventParameter p) {
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
            bool flag = font != null && font.isValid;
            if (Application.isPlaying && !flag) {
                font = GetUIView().defaultFont;
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
            if (m_Popup != null && !m_Popup.containsFocus && !m_IsMouseHovering) {
                ClosePopup(true);
            }
        }

        private void CheckForPopupClose() {
            if (m_Popup == null || !Input.GetMouseButtonDown(0)) {
                return;
            }
            Camera camera = GetCamera();
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (m_Popup.Raycast(ray)) {
                return;
            }
            if (m_Popup.scrollbar != null && m_Popup.scrollbar.Raycast(ray)) {
                return;
            }
            if(!m_IsMouseHovering)
            ClosePopup(true);
        }

        private void AttachChildEvents() {
            if (TriggerButton is not null && !eventsAttached) {
                eventsAttached = true;
                TriggerButton.eventClick += OnTriggerClick;
            }
        }

        private void DetachChildEvents() {
            if (TriggerButton is not null && eventsAttached) {
                TriggerButton.eventClicked -= OnTriggerClick;
                eventsAttached = false;
            }
        }

        private void OnTriggerClick(UIComponent child, UIMouseEventParameter mouseEvent) {
            if (m_Popup is null) {
                OpenPopup();
                ExternalLogger.Log("OnTriggerClick_m_Popup is null");
            } else {
                ClosePopup(true);
            }

        }

        protected override void OnRebuildRenderData() {
            if (atlas == null || font == null || !font.isValid) {
                return;
            }
            if (textRenderData != null) {
                textRenderData.Clear();
            } else {
                UIRenderData item = UIRenderData.Obtain();
                m_RenderData.Add(item);
            }
            renderData.material = atlas.material;
            textRenderData.material = atlas.material;
            RenderBackground();
            RenderForeground();
            RenderText();
        }

        private void RenderText() {
            if (SelectedIndex < 0 || SelectedIndex >= Items.Length) {
                return;
            }
            string text = Items[SelectedIndex];
            float num = PixelsToUnits();
            Vector2 maxSize = new(size.x - TextFieldPadding.horizontal, size.y - TextFieldPadding.vertical);
            Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            Vector3 vectorOffset = new Vector3(vector.x + TextFieldPadding.left, vector.y - TextFieldPadding.top, 0f) * num;
            Color32 defaultColor = isEnabled ? textColor : disabledColor;
            using UIFontRenderer uifontRenderer = font.ObtainRenderer();
            uifontRenderer.wordWrap = false;
            uifontRenderer.maxSize = maxSize;
            uifontRenderer.pixelRatio = num;
            uifontRenderer.textScale = textScale;
            uifontRenderer.characterSpacing = characterSpacing;
            uifontRenderer.vectorOffset = vectorOffset;
            uifontRenderer.multiLine = false;
            uifontRenderer.textAlign = UIHorizontalAlignment.Left;
            uifontRenderer.processMarkup = processMarkup;
            uifontRenderer.colorizeSprites = colorizeSprites;
            uifontRenderer.defaultColor = defaultColor;
            uifontRenderer.bottomColor = (useGradient ? new Color32?(bottomColor) : default);
            uifontRenderer.overrideMarkupColors = false;
            uifontRenderer.opacity = CalculateOpacity();
            uifontRenderer.outline = useOutline;
            uifontRenderer.outlineSize = outlineSize;
            uifontRenderer.outlineColor = outlineColor;
            uifontRenderer.shadow = useDropShadow;
            uifontRenderer.shadowColor = dropShadowColor;
            uifontRenderer.shadowOffset = dropShadowOffset;
            if (uifontRenderer is UIDynamicFont.DynamicFontRenderer dynamicFontRenderer) {
                dynamicFontRenderer.spriteAtlas = atlas;
                dynamicFontRenderer.spriteBuffer = renderData;
            }
            uifontRenderer.Render(text, textRenderData);
        }

        public void AddItem(string item) {
            string[] array = new string[Items.Length + 1];
            Array.Copy(Items, array, Items.Length);
            array[Items.Length] = item;
            Items = array;
        }

        private Vector3 CalculatePopupPosition(int height) {
            float d = PixelsToUnits();
            Vector3 a = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            Vector3 a2 = transform.position + a * d;
            Vector3 scaledDirection = GetScaledDirection(Vector3.down);
            Vector3 b = TransformOffset(ListOffset) * d;
            Vector3 vector = a2 + b + scaledDirection * size.y * d;
            Vector3 result = a2 + b - scaledDirection * m_Popup.size.y * d;
            if (ListPosition == PopupListPosition.Above) {
                return result;
            }
            if (ListPosition == PopupListPosition.Below) {
                return vector;
            }
            Vector3 a3 = m_Popup.transform.parent.position / d + m_Popup.parent.pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            Vector3 vector2 = a3 + scaledDirection * parent.size.y;
            Vector3 a4 = vector / d + scaledDirection * m_Popup.size.y;
            if (a4.y < vector2.y) {
                return result;
            }
            if (GetCamera().WorldToScreenPoint(a4 * d).y <= 0f) {
                return result;
            }
            return vector;
        }

        private Vector2 CalculatePopupSize() {
            float num = (ListWidth > 0) ? (ListWidth) : size.x;
            int b = Items.Length * ItemHeight + ListPadding.vertical;
            if (Items.Length == 0) {
                b = ItemHeight / 2 + ListPadding.vertical;
            }
            if (AutoListWidth) {
                num = Mathf.Max(CalculatePopupWidth(Mathf.Min(ListHeight, b)), num);
            }
            return new Vector2(num, Mathf.Min(ListHeight, b));
        }

        public float CalculatePopupWidth(int height) {
            float num = 0f;
            float pixelRatio = PixelsToUnits();
            for (int i = 0; i < Items.Length; i++) {
                using (UIFontRenderer uifontRenderer = font.ObtainRenderer()) {
                    uifontRenderer.wordWrap = false;
                    uifontRenderer.pixelRatio = pixelRatio;
                    uifontRenderer.textScale = textScale;
                    uifontRenderer.characterSpacing = characterSpacing;
                    uifontRenderer.multiLine = false;
                    uifontRenderer.textAlign = UIHorizontalAlignment.Left;
                    uifontRenderer.processMarkup = processMarkup;
                    uifontRenderer.colorizeSprites = colorizeSprites;
                    uifontRenderer.overrideMarkupColors = false;
                    Vector2 vector = uifontRenderer.MeasureString(Items[i]);
                    if (vector.x > num) {
                        num = vector.x;
                    }
                }
            }
            num += ListPadding.horizontal + ItemPadding.horizontal;
            if (ListScrollbar != null && height >= ListHeight) {
                num += ListScrollbar.size.x;
            }
            return num;
        }

        private bool OpenPopup() {
            if (m_Popup != null || Items.Length == 0) {
                return false;
            }
            UIComponent rootContainer = GetRootContainer();
            Vector2 size2 = CalculatePopupSize();
            m_Popup = rootContainer.AddUIComponent<ListBox>();
            m_Popup.ItemHovereColor = ItemHovereColor;
            m_Popup.SelectionColor = ItemHighlightColor;
            m_Popup.builtinKeyNavigation = builtinKeyNavigation;
            m_Popup.name = cachedName + " - Dropdown List";
            m_Popup.gameObject.hideFlags = HideFlags.DontSave;
            m_Popup.atlas = atlas;
            m_Popup.anchor = UIAnchorStyle.None;
            m_Popup.font = font;
            m_Popup.pivot = UIPivotPoint.TopLeft;
            m_Popup.size = size2;
            m_Popup.itemHeight = ItemHeight;
            m_Popup.itemHighlight = ItemHighlightSprite;
            m_Popup.itemHover = ItemHoverSprite;
            m_Popup.itemPadding = ItemPadding;
            m_Popup.color = ListBackgroundColor;
            m_Popup.itemTextColor = ItemTextColor;
            m_Popup.textScale = textScale;
            m_Popup.items = Items;
            m_Popup.filteredItems = filteredItems;
            m_Popup.listPadding = ListPadding;
            m_Popup.normalBgSprite = ListBackgroundSprite;
            m_Popup.useDropShadow = useDropShadow;
            m_Popup.dropShadowColor = dropShadowColor;
            m_Popup.dropShadowOffset = dropShadowOffset;
            m_Popup.useGradient = useGradient;
            m_Popup.bottomColor = bottomColor;
            m_Popup.useOutline = useOutline;
            m_Popup.outlineColor = outlineColor;
            m_Popup.outlineSize = outlineSize;
            m_Popup.zOrder = int.MaxValue;
            if (size2.y >= ListHeight && ListScrollbar != null) {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ListScrollbar.gameObject);
                m_ActiveScrollbar = gameObject.GetComponent<UIScrollbar>();
                float d = PixelsToUnits();
                Vector3 a = m_Popup.transform.TransformDirection(Vector3.right);
                Vector3 position = m_Popup.transform.position + a * (size2.x - m_ActiveScrollbar.width) * d;
                m_ActiveScrollbar.transform.parent = m_Popup.transform;
                m_ActiveScrollbar.transform.position = position;
                m_ActiveScrollbar.anchor = (UIAnchorStyle.Top | UIAnchorStyle.Bottom);
                m_ActiveScrollbar.height = m_Popup.height;
                m_Popup.width -= m_ActiveScrollbar.width;
                m_Popup.scrollbar = m_ActiveScrollbar;
                m_Popup.eventSizeChanged += (component, size) => m_ActiveScrollbar.height = component.height;
            }
            Vector3 vector = CalculatePopupPosition((int)m_Popup.size.y);
            if (ClampListToScreen) {
                vector = ClampToScreen(vector, m_Popup.size + ((ListScrollbar != null) ? new Vector2(ListScrollbar.size.x, 0f) : default));
            }
            m_Popup.transform.position = vector;
            m_Popup.transform.rotation = transform.rotation;
            m_Popup.eventSelectedIndexChanged += PopupSelectedIndexChanged;
            m_Popup.eventLeaveFocus += PopupLostFocus;
            m_Popup.eventItemClicked += PopupItemClicked;
            m_Popup.eventKeyDown += PopupKeyDown;
            m_Popup.selectedIndex = Mathf.Max(0, SelectedIndex);
            m_Popup.EnsureVisible(m_Popup.selectedIndex);
            m_Popup.Focus();
            if (EventDropdownOpen != null) {
                bool flag = false;
                EventDropdownOpen(this, m_Popup, ref flag);
            }
            Invoke("OnDropdownOpen", new object[]
            {
                m_Popup
            });
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
            if (m_Popup == null) {
                return;
            }
            m_Popup.eventLeaveFocus -= PopupLostFocus;
            m_Popup.eventSelectedIndexChanged -= PopupSelectedIndexChanged;
            m_Popup.eventItemClicked -= PopupItemClicked;
            m_Popup.eventKeyDown -= PopupKeyDown;
            if (!allowOverride) {
                UnityEngine.Object.Destroy(m_Popup.gameObject);
                m_Popup = null;
                return;
            }
            bool flag = false;
            EventDropdownClose?.Invoke(this, m_Popup, ref flag);
            if (!flag) {
                flag = Invoke("OnDropdownClose", new object[]
                {
                    m_Popup
                });
            }
            if (!flag) {
                UnityEngine.Object.Destroy(m_Popup.gameObject);
            }
            m_Popup = null;
            m_ActiveScrollbar = null;
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
            if (m_Popup != null && !m_Popup.containsFocus && !m_IsMouseHovering) {
                ClosePopup(true);
            }
        }

        private void PopupSelectedIndexChanged(UIComponent component, int si) {
            SelectedIndex = si;
            Invalidate();
        }

        protected override void RequestCharacterInfo() {
            UIDynamicFont uidynamicFont = font as UIDynamicFont;
            if (uidynamicFont == null) {
                return;
            }
            if (!UIFontManager.IsDirty(font)) {
                return;
            }
            string selectedValue = SelectedValue;
            if (string.IsNullOrEmpty(selectedValue)) {
                return;
            }
            float num = textScale * GetTextScaleMultiplier();
            int fontSize = Mathf.CeilToInt(font.size * num);
            uidynamicFont.AddCharacterRequest(selectedValue, fontSize, FontStyle.Normal);
        }
        protected override void OnMouseWheel(UIMouseEventParameter p) {
            if (CanWheel) {
                SelectedIndex = Mathf.Max(0, SelectedIndex - Mathf.RoundToInt(p.wheelDelta));
                p.Use();
                if (!p.used) {
                    Invoke("OnMouseWheel", new object[] { p });
                    EventMouseWheelHandler?.Invoke(this, p);
                }
            }

        }

        public enum PopupListPosition {
            Below,
            Above,
            Automatic
        }

        public delegate void PopupEventHandler(DropDown dropdown, ListBox popup, ref bool overridden);
    }



}
