using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomPanel {
        public static UIPanel AddSpace(UIComponent parent, float width, float height) {
            var space = parent.AddUIComponent<UIPanel>();
            space.autoSize = false;
            space.width = width;
            space.height = height;
            return space;
        }
        public static UIPanel AddAutoMatchChildPanel(UIComponent parent, RectOffset autoLayoutPadding = null) {
            var panel = parent.AddUIComponent<AutoMatchChildPanel>();
            if (autoLayoutPadding is not null) {
                panel.autoLayoutPadding = autoLayoutPadding;
            }
            return panel;
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
                    totalHeight = Mathf.Max(component.relativePosition.y + component.size.y, totalHeight);
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
