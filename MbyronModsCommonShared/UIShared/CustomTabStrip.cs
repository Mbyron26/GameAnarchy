using ColossalFramework.UI;
using System;
using System.Collections.Generic;

namespace MbyronModsCommon {
    public class CustomTabStrip : UIPanel {
        private const int tabHeight = 24;
        private const int gap = 3;
        public Action<int> SelectedTabButton;
        public List<TabButton> TabButtons { get; set; } = new List<TabButton>();
        private int index = -1;
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

        public void AddTab(string name, string _text) {
            var tabButton = AddUIComponent<TabButton>();
            tabButton.name = name;
            tabButton.atlas = CustomAtlas.CommonAtlas;
            tabButton.normalBgSprite = CustomAtlas.TabButtonNormal;
            tabButton.focusedBgSprite = CustomAtlas.ButtonNormal;
            tabButton.hoveredBgSprite = CustomAtlas.TabButtonHovered;
            tabButton.pressedBgSprite = CustomAtlas.ButtonPressed;
            tabButton.height = tabHeight;
            tabButton.text = _text;
            tabButton.tooltip = _text;
            tabButton.textScale = 1.2f;
            tabButton.textHorizontalAlignment = UIHorizontalAlignment.Center;
            tabButton.textVerticalAlignment = UIVerticalAlignment.Middle;
            TabButtons.Add(tabButton);
            AdaptWidth();
        }
        private void AdaptWidth() {
            if (TabButtons.Count <= 0) isVisible = false;
            var amount = TabButtons.Count;
            //var buttonWidth = width / amount;
            var buttonWidth = (width - (amount + 1) * gap) / amount;
            foreach (var item in TabButtons) {
                item.width = buttonWidth;
            }
        }

        protected override void OnVisibilityChanged() {
            base.OnVisibilityChanged();
            AdaptWidth();
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
