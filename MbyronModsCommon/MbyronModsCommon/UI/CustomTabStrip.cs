using ColossalFramework.UI;
using System;
using System.Collections.Generic;

namespace MbyronModsCommon.UI {
    public class CustomTabStrip : UIPanel {
        private int tabHeight = 24;
        private int gap = 3;
        private int index = -1;
        public Action<int> SelectedTabButton;
        public List<TabButton> TabButtons { get; set; } = new List<TabButton>();
        public int TabHeight {
            get => tabHeight;
            set {
                if (!value.Equals(tabHeight)) {
                    tabHeight = value;
                    RefreshButtons();
                }
            }
        }
        public int Gap {
            get => gap;
            set {
                if (!value.Equals(gap)) {
                    gap = value;
                    RefreshButtons();
                }
            }
        }
        public CustomTabStrip() {
            atlas = CustomAtlas.CommonAtlas;
            backgroundSprite = CustomAtlas.TabButtonNormal;
        }
        public int Index {
            get => index;
            set {
                if (value != index) {
                    index = value;
                    SelectedTabButton?.Invoke(index);
                }
            }
        }

        public override void Update() {
            base.Update();
            for (int i = 0; i < TabButtons.Count; i++) {
                if (i == Index)
                    TabButtons[i].state = UIButton.ButtonState.Focused;
                else if (!TabButtons[i].IsHovering)
                    TabButtons[i].state = UIButton.ButtonState.Normal;
            }
        }

        public void AddTab(string text, float textScale = 1f, string tooltip = null, Action<TabButton> setSprite = null) {
            var tabButton = AddUIComponent<TabButton>();
            if (setSprite is null) {
                tabButton.atlas = CustomAtlas.CommonAtlas;
                tabButton.normalBgSprite = CustomAtlas.TabButtonNormal;
                tabButton.focusedBgSprite = CustomAtlas.ButtonNormal;
                tabButton.hoveredBgSprite = CustomAtlas.TabButtonHovered;
                tabButton.pressedBgSprite = CustomAtlas.ButtonPressed;
            } else {
                setSprite.Invoke(tabButton);
            }
            tabButton.height = tabHeight;
            tabButton.text = text;
            if (tooltip is not null) {
                tabButton.tooltip = tooltip;
            }
            tabButton.textScale = textScale;
            tabButton.textHorizontalAlignment = UIHorizontalAlignment.Center;
            tabButton.textVerticalAlignment = UIVerticalAlignment.Middle;
            TabButtons.Add(tabButton);
            RefreshButtons();
        }
        private void RefreshButtons() {
            if (TabButtons.Count <= 0) isVisible = false;
            if (TabButtons.Count == 0) return;
            var amount = TabButtons.Count;
            var buttonWidth = (width - (amount + 1) * gap) / amount;
            var buttonHeight = height - 2 * gap;
            foreach (var item in TabButtons) {
                item.width = buttonWidth;
                item.height = buttonHeight;
            }
        }

        protected override void OnVisibilityChanged() {
            base.OnVisibilityChanged();
            RefreshButtons();
        }
        protected override void OnComponentAdded(UIComponent child) {
            base.OnComponentAdded(child);
            if (child is TabButton tabButton) {
                tabButton.eventClick += ButtonClick;
            }
        }

        private void ButtonClick(UIComponent component, UIMouseEventParameter eventParam) {
            if (component is TabButton tabButton)
                Index = TabButtons.IndexOf(tabButton);
        }
    }

    public class TabButton : UIButton {
        public bool IsHovering => m_IsMouseHovering;
    }
}
