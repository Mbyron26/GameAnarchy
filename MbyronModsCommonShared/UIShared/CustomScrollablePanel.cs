using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon {
    public class OptionPanelScrollablePanel : CustomScrollablePanelBase<AutoLayoutScrollablePanel> {
        public OptionPanelScrollablePanel() {
            clipChildren = true;
            size = new Vector2(764, 743);
            MainPanel.autoLayoutPadding = new RectOffset(10, 10, 0, 10);
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

