using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon {
    public class OptionPanelCard {
        private static float NormalWidth => 744;
        private static float ShrinkageWidth => 734;

        public static UIPanel AddCard(UIComponent parent, TypeWidth typeWidth, string titleText, out UIPanel titlePanel, bool isMainPanel = false) {
            var card = parent.AddUIComponent<Card>();
            var _titlePanel = card.AddUIComponent<UIPanel>();
            var contentPanel = card.AddUIComponent<AdvancedAutoFitChildrenVerticallyPanel>();
            _titlePanel.autoLayout = false;
            if (typeWidth == TypeWidth.NormalWidth) {
                contentPanel.width = _titlePanel.width = card.width = NormalWidth;
            } else {
                contentPanel.width = _titlePanel.width = card.width = ShrinkageWidth;
            }
            if (!isMainPanel) {
                _titlePanel.height = 36f;
                AddTitleTextLabel(_titlePanel, titleText, 1.1f, null);
            } else {
                _titlePanel.height = 52f;
                AddTitleTextLabel(_titlePanel, titleText, 1.8f, CustomFont.SemiBold);
            }
            titlePanel = _titlePanel;
            contentPanel.autoLayoutPadding = new RectOffset(30, 10, 6, 0);
            return contentPanel;
        }

        private static UILabel AddTitleTextLabel(UIComponent parent, string titleText, float _textScale, UIFont font) {
            var label = CustomLabel.AddLabel(parent, titleText, 700f, _textScale, UIColor.White);
            if (font is not null) {
                label.font = font;
            }
            label.relativePosition = new Vector2(10f, 10f);
            return label;
        }

    }

    public enum TypeWidth {
        NormalWidth,
        ShrinkageWidth
    }

    public class Card : AdvancedAutoFitChildrenVerticallyPanel {
        public Card() {
            autoSize = false;
            //backgroundSprite = "SubcategoriesPanel";
            backgroundSprite = "TextFieldPanel";
            color = UIColor.CardBackground;
        }
    }
}
