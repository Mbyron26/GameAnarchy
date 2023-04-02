using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class PropertyPanel : UIPanel {
        public IUIStyle UITool { get; set; }
        public UILabel MajorLabel { get; set; }
        public UILabel MinorLabel { get; set; }
        public UIPanel CardGroupPanel { get; set; }
        public List<UIPanel> ChildrenPanels { get; set; } = new();

        public PropertyPanel() {
            name = nameof(PropertyPanel);
            autoLayout = true;
            clipChildren = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoFitChildrenVertically = true;
        }

        public void Init(float width, string caption, RectOffset captionRectOffset, float textScale, Color32 captionColor, RectOffset groupPanelPadding, Action<UIPanel> setGroupPanelStyle = null, UIFont font = null) {
            this.width = width;
            if (groupPanelPadding is not null)
                autoLayoutPadding = groupPanelPadding;
            if (caption is not null) {
                MajorLabel = CustomLabel.AddLabel(this, caption, width, captionRectOffset, textScale, captionColor);
                if (font is not null) {
                    MajorLabel.font = font;
                }
            }
            CardGroupPanel = AddUIComponent<UIPanel>();
            CardGroupPanel.clipChildren = true;
            CardGroupPanel.width = width;
            CardGroupPanel.autoLayout = true;
            CardGroupPanel.autoLayoutDirection = LayoutDirection.Vertical;
            CardGroupPanel.autoFitChildrenVertically = true;
            if (setGroupPanelStyle is not null) {
                setGroupPanelStyle.Invoke(CardGroupPanel);
            } else {
                CardGroupPanel.atlas = CustomAtlas.InGameAtlas;
                CardGroupPanel.backgroundSprite = "ButtonWhite";
                CardGroupPanel.color = new Color32(82, 101, 117, 255);
            }
        }

        public UILabel AddMinorLabel(string text, RectOffset rectOffset, float textScale, Color32 color) => MinorLabel = CustomLabel.AddLabel(this, text, width, rectOffset, textScale, color);

        public UIPanel AddChildPanel(Action<UIPanel> setStyle = null, float height = 32f) {
            var panel = CardGroupPanel.AddUIComponent<UIPanel>();
            panel.name = "PropertyPanel_ChildPanel";
            panel.width = width;
            panel.height = height;
            ChildrenPanels.Add(panel);
            if (ChildrenPanels.Count % 2 == 0) {
                var underline = panel.AddUIComponent<UIPanel>();
                underline.width = width;
                underline.height = height;
                underline.relativePosition = Vector2.zero;
                if (setStyle is not null) {
                    setStyle.Invoke(underline);
                } else {
                    underline.atlas = CustomAtlas.CommonAtlas;
                    underline.backgroundSprite = CustomAtlas.EmptySprite;
                    underline.color = new Color32(0, 0, 0, 50);
                    underline.disabledColor = new Color32(0, 0, 0, 50);
                }
                panel.eventSizeChanged += (c, v) => underline.size = panel.size;
            }
            return panel;
        }
    }

}
