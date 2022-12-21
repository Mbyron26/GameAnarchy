using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomTabs {
        public static OptionPanelTabs AddCustomTabs(UIComponent parent) {
            parent.size = new Vector2(764, 773);
            var customTabs = new OptionPanelTabs(parent);
            customTabs.Initialize(764, 30);
            return customTabs;
        }
    }
    public class OptionPanelTabs : CustomTabs<OptionPanelScrollablePanel> {
        public OptionPanelTabs(UIComponent parent) : base(parent) { }
    }

    public class CustomTabs<TypeContainer> where TypeContainer : UIComponent {
        public UIPanel Panel { get; private set; }
        public CustomTabStrip TabPanel { get; set; }
        public List<TypeContainer> Containers { get; set; } = new();

        public CustomTabs(UIComponent parent) {
            Panel = (UIPanel)parent;
        }
        public void Initialize(float width, float tabHeight) {
            TabPanel = Panel.AddUIComponent<CustomTabStrip>();
            TabPanel.width = width;
            TabPanel.height = tabHeight;
            TabPanel.autoLayout = true;
            TabPanel.autoLayoutPadding = new RectOffset(3, 0, 3, 3);
            TabPanel.autoLayoutDirection = LayoutDirection.Horizontal;
            TabPanel.SelectedTabButton += SelectedTabButtonChanged;
            TabPanel.relativePosition = Vector2.zero;
        }

        public TypeContainer AddTabs(string name, string text, float? posX, float? posY) {
            TabPanel.AddTab(name, text);
            var container = Panel.AddUIComponent<TypeContainer>();
            container.name = name;
            container.isVisible = false;
            if (posX.HasValue && posY.HasValue) {
                container.relativePosition = new Vector2(posX.Value, posY.Value);
            }
            Containers.Add(container);
            TabPanel.Index = 0;
            return container;
        }

        private void SelectedTabButtonChanged(int index) {
            if (index >= 0 && index < Containers.Count) {
                foreach (var item in Containers) {
                    item.isVisible = false;
                }
                Containers[index].isVisible = true;
            }
        }

    }
}
