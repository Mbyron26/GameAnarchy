using ColossalFramework.UI;
using System.Linq;
using UnityEngine;

namespace MbyronModsCommon.UI {
    // Base on macsergey's ModsCommon.
    public class AutoSizeScrollablePanel : UIScrollablePanel {
        public AutoSizeScrollablePanel() {
            autoLayout = true;
        }

        protected override void OnComponentAdded(UIComponent child) {
            base.OnComponentAdded(child);
            FitContentChildren();

            child.eventVisibilityChanged += OnChildVisibilityChanged;
            child.eventSizeChanged += OnChildSizeChanged;
        }

        protected override void OnComponentRemoved(UIComponent child) {
            base.OnComponentRemoved(child);
            FitContentChildren();

            child.eventVisibilityChanged -= OnChildVisibilityChanged;
            child.eventSizeChanged -= OnChildSizeChanged;
        }

        private void OnChildVisibilityChanged(UIComponent component, bool value) => FitContentChildren();
        private void OnChildSizeChanged(UIComponent component, Vector2 value) => FitContentChildren();

        private void FitContentChildren() {
            if (autoLayout && parent != null) {
                var height = 0f;
                foreach (var component in components) {
                    if (component.isVisibleSelf)
                        height = Mathf.Max(height, component.relativePosition.y + component.height);
                }
                if (components.Any())
                    height += autoLayoutPadding.bottom;

                if (height < this.height) {
                    this.height = height;
                    parent.height = height;
                } else {
                    parent.height = height;
                    this.height = height;
                }

                verticalScrollbar.isVisible = Mathf.CeilToInt(verticalScrollbar.scrollSize) < Mathf.CeilToInt(verticalScrollbar.maxValue - verticalScrollbar.minValue);
            }
        }

    }

    public class MessageBoxScrollablePanel : UIPanel {
        public AutoSizeScrollablePanel MainPanel { get; private set; }
        public UIScrollbar Scrollbar { get; private set; }
        public float MaxHeight {
            set {
                MainPanel.maximumSize = maximumSize = new Vector2(width, value);
            }
        }

        public MessageBoxScrollablePanel() {
            name = nameof(MessageBoxScrollablePanel);
            clipChildren = true;
            autoLayout = false;
            width = MessageBoxParameters.Width;
            MainPanel = AddUIComponent<AutoSizeScrollablePanel>();
            MainPanel.name = nameof(MainPanel);
            MainPanel.width = width;
            MainPanel.autoLayout = true;
            MainPanel.autoLayoutDirection = LayoutDirection.Vertical;
            MainPanel.autoLayoutPadding = new RectOffset(MessageBoxParameters.Padding, MessageBoxParameters.Padding, MessageBoxParameters.Padding, 0);
            MainPanel.builtinKeyNavigation = true;
            MainPanel.scrollWheelDirection = UIOrientation.Vertical;
            MainPanel.clipChildren = true;
            MainPanel.autoReset = false;
            MainPanel.relativePosition = Vector2.zero;
            Scrollbar = CustomScrollbar.AddScrollbar(this, MainPanel);

            MainPanel.verticalScrollbar.eventVisibilityChanged += VerticalScrollbarOnVisibilityChanged;
            MainPanel.verticalScrollbar.eventSizeChanged += (c, v) => Scrollbar.height = height;
        }

        private void VerticalScrollbarOnVisibilityChanged(UIComponent component, bool value) {
            if (!value)
                return;
            Scrollbar.relativePosition = new Vector2(width - MessageBoxParameters.Padding, 0);
        }

    }

    public class OptionPanelScrollablePanel : CustomScrollablePanelBase<AutoLayoutScrollablePanel> {
        public OptionPanelScrollablePanel() {
            clipChildren = true;
            size = new Vector2(764, 743);
            MainPanel.autoLayoutPadding = new RectOffset(10, 10, 0, 20);
        }
    }

    public abstract class CustomScrollablePanelBase<Panel> : UIPanel where Panel : UIScrollablePanel {
        public Panel MainPanel { get; private set; }
        public CustomScrollablePanelBase() {
            MainPanel = AddUIComponent<Panel>();
            MainPanel.clipChildren = true;
            MainPanel.autoLayoutDirection = LayoutDirection.Vertical;
            MainPanel.scrollWheelDirection = UIOrientation.Vertical;
            MainPanel.builtinKeyNavigation = true;
            CustomScrollbar.AddScrollbar(this, MainPanel);
            MainPanel.verticalScrollbar.eventVisibilityChanged += (c, v) => SetContentSize();
        }

        protected override void OnSizeChanged() {
            base.OnSizeChanged();
            SetContentSize();
        }

        private void SetContentSize() {
            MainPanel.size = size - new Vector2(MainPanel.verticalScrollbar.isVisible ? MainPanel.verticalScrollbar.width : 0, 0);
            MainPanel.relativePosition = new Vector2(0, 10);
            if (MainPanel.verticalScrollbar.isVisibleSelf) {
                MainPanel.verticalScrollbar.relativePosition = new Vector2(size.x - 10, 10);
            }
        }

    }

    public class AutoLayoutScrollablePanel : UIScrollablePanel {
        public AutoLayoutScrollablePanel() {
            autoLayout = true;
            clipChildren = true;
        }
    }
}
