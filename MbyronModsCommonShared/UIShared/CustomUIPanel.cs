using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomPanel {
        public static UIPanel AddAutoMatchChildPanel(UIComponent parent, RectOffset autoLayoutPadding = null) {
            var panel = parent.AddUIComponent<AutoMatchChildPanel>();
            if(autoLayoutPadding is not null) {
                panel.autoLayoutPadding = autoLayoutPadding;
            }
            return panel;
        }
    }
    public class MessageBoxScrollablePanel : AdvancedScrollablePanel<ScrollablePanelWithCard> { }

    public class AdvancedScrollablePanel<T> : UIPanel where T : UIScrollablePanel {
        protected int defaultPadding = 10;
        public T MainPanel { get; set; }
        public UIScrollbar Scrollbar { get; private set; }
        public Vector2 CustomMaxSize {
            set {
                width = value.x;
                MainPanel.maximumSize = new Vector2(value.x - defaultPadding, value.y);

            }
        }
        public AdvancedScrollablePanel() {
            clipChildren = true;
            autoLayout = false;
            AddMainPanel();
        }
        private void AddMainPanel() {
            MainPanel = AddUIComponent<T>();
            MainPanel.relativePosition = new Vector2(0, defaultPadding);
            MainPanel.autoLayoutPadding = new RectOffset(defaultPadding, defaultPadding, 0, 0);
            Scrollbar = CustomScrollbar.AddScrollbar(this, MainPanel);
            MainPanel.verticalScrollbar.eventVisibilityChanged += VerticalScrollbarOnVisibilityChanged;
            MainPanel.eventComponentAdded += MainPanelOnComponentAdded;
            MainPanel.eventSizeChanged += MainPanelOnSizeChanged;
        }

        private void MainPanelOnSizeChanged(UIComponent component, Vector2 value) {
            MainPanel.width = MainPanel.verticalScrollbar.isVisible ? width - defaultPadding : width;
            FitChild();
        }
        private void MainPanelOnComponentAdded(UIComponent container, UIComponent child) {
            child.eventVisibilityChanged += MainPanelChildOnVisibilityChanged;
            child.eventSizeChanged += MainPanelChildOnSizeChanged;
            FitChild();
        }
        private void MainPanelChildOnSizeChanged(UIComponent component, Vector2 value) => FitChild();
        private void MainPanelChildOnVisibilityChanged(UIComponent component, bool value) => FitChild();
        private void VerticalScrollbarOnVisibilityChanged(UIComponent component, bool value) {
            Scrollbar.relativePosition = new Vector2(width - defaultPadding, defaultPadding);
        }


        protected void FitChild() {
            foreach (var item in MainPanel.components) {
                item.width = MainPanel.verticalScrollbar.isVisible ? MainPanel.width - MainPanel.autoLayoutPadding.horizontal : MainPanel.width;
            }
            MainPanel.FitChildrenVertically();
            float totalHeight = 0f;
            foreach (var component in components) {
                if (component.isVisibleSelf)
                    totalHeight = Math.Max(component.relativePosition.y + component.size.y, totalHeight);
            }
            //totalHeight += defaultPadding;
            height = totalHeight;
        }

    }

    public class ScrollablePanelWithCard : UIScrollablePanel {
        public ScrollablePanelWithCard() {
            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            clipChildren = true;
            builtinKeyNavigation = true;
            scrollWheelDirection = UIOrientation.Vertical;
        }



        public AdvancedAutoFitChildrenVerticallyPanel AddCard() {
            var card = AddUIComponent<AdvancedAutoFitChildrenVerticallyPanel>();
            card.autoLayoutPadding = new RectOffset(10, 10, 10, 0);
            card.backgroundSprite = "TextFieldPanel";
            card.color = UIColor.CardBackground;
            return card;
        }
    }


    public class AdvancedAutoFitChildrenVerticallyPanel : AutoLayoutPanel {
        public AdvancedAutoFitChildrenVerticallyPanel() {
            autoLayoutDirection = LayoutDirection.Vertical;
        }

        protected override void OnComponentAdded(UIComponent child) {
            base.OnComponentAdded(child);
            FitChild();
            child.eventVisibilityChanged += OnChildVisibilityChanged;
            child.eventSizeChanged += OnChildSizeChanged;
        }

        protected override void OnComponentRemoved(UIComponent child) {
            base.OnComponentAdded(child);
            FitChild();
            child.eventVisibilityChanged -= OnChildVisibilityChanged;
            child.eventSizeChanged -= OnChildSizeChanged;
        }

        private void OnChildVisibilityChanged(UIComponent component, bool value) => FitChild();
        private void OnChildSizeChanged(UIComponent component, Vector2 value) => FitChild();

        protected void FitChild() {
            float totalHeight = 0f;
            foreach (var component in components) {
                if (component.isVisibleSelf)
                    totalHeight = EMath.Max(component.relativePosition.y + component.size.y, totalHeight);
            }
            totalHeight += autoLayoutPadding.vertical;
            height = totalHeight;
        }
    }




    public class AutoMatchChildPanel : AutoLayoutPanel {
        public AutoMatchChildPanel() {
            autoFitChildrenHorizontally = true;
            autoFitChildrenVertically = true;
        }
    }

    public class AutoLayoutPanel : UIPanel {
        public AutoLayoutPanel() {
            autoLayout = true;
            clipChildren = true;
        }
    }
}
