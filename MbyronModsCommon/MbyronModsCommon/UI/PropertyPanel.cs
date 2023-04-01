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

        public UIPanel AddChildPanel(bool showLine, float height = 32f, RectOffset lineOffset = null, UIVerticalAlignment linePosition = UIVerticalAlignment.Bottom, bool highlight = true) {
            var panel = CardGroupPanel.AddUIComponent<SinglePropertyPanel>();
            panel.name = "PropertyPanel_ChildPanel";
            panel.width = width;
            panel.height = height;
            panel.ShowLine = showLine;
            if (lineOffset != null) {
                panel.LineOffset = lineOffset;
            }
            panel.Highlight = highlight;
            panel.LinePosition = linePosition;
            ChildrenPanels.Add(panel);
            return panel;
        }

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

    public class SinglePropertyPanel : UIPanel {
        private bool highlight = true;
        private Color32 highlightColor = CustomColor.OffWhite;
        protected UIPanel line;
        private float lineWidth = 10;
        private float lineHeight = 0.6f;
        private RectOffset lineOffset = new(10, 10, 0, 0);
        private Color32 lineColor = new(150, 150, 150, 50);
        private bool showLine = true;
        private UIVerticalAlignment linePosition = UIVerticalAlignment.Bottom;

        public bool Highlight {
            get => highlight;
            set {
                if (highlight != value) {
                    highlight = value;
                }
            }
        }

        public Color32 HeighlightColor {
            get => highlightColor;
            set {
                if (!highlight.Equals(value)) {
                    highlightColor = value;
                }
            }
        }

        public float LineWidth {
            get => lineWidth;
            set {
                if (lineWidth != value) {
                    lineWidth = value;
                    line.width = value;
                    RefreshLine();
                }
            }
        }

        public float LineHeight {
            get => lineHeight;
            set {
                if (lineHeight != value) {
                    lineHeight = value;
                    line.height = value;
                    RefreshLine();
                }
            }
        }

        public RectOffset LineOffset {
            get => lineOffset;
            set {
                if (!Equals(value, lineOffset)) {
                    lineOffset = value;
                    RefreshLine();
                }
            }
        }

        public Color32 LineColor {
            get => lineColor;
            set {
                if (!Equals(value, lineColor)) {
                    lineColor = value;
                    line.color = value;
                    line.disabledColor = value;
                }
            }
        }

        public bool ShowLine {
            get => showLine;
            set {
                if (showLine != value) {
                    showLine = value;
                    line.isVisible = value;
                }
            }
        }

        public UIVerticalAlignment LinePosition {
            get => linePosition;
            set {
                if (linePosition != value) {
                    linePosition = value;
                    RefreshLine();
                }
            }
        }

        public SinglePropertyPanel() {
            clipChildren = true;
            line = AddUIComponent<UIPanel>();
            line.atlas = CustomAtlas.InGameAtlas;
            line.backgroundSprite = "WhiteRect";
            line.disabledColor = line.color = new Color32(85, 100, 115, 255);
            line.width = lineWidth;
            line.height = lineHeight;
        }

        private void RefreshLine() {
            if (width == 0 || height == 0)
                return;
            if (line.isVisible) {
                line.width = width - lineOffset.horizontal;
                if (linePosition == UIVerticalAlignment.Top) {
                    line.relativePosition = new Vector2(lineOffset.left, 0);
                } else if (linePosition == UIVerticalAlignment.Bottom) {
                    line.relativePosition = new Vector2(lineOffset.left, height - line.height);
                } else {
                    line.relativePosition = new Vector2(lineOffset.left, (height - line.height) / 2);
                }
            }
        }

        protected override void OnSizeChanged() {
            RefreshLine();
            base.OnSizeChanged();
        }

        protected override void OnMouseEnter(UIMouseEventParameter p) {
            base.OnMouseEnter(p);
            if (highlight) {
                line.isVisible = false;
                color = highlightColor;
            }
        }



        protected override void OnMouseLeave(UIMouseEventParameter p) {
            if (highlight) {
                line.isVisible = true;
                color = color;
            }
            base.OnMouseLeave(p);
        }
    }

}
