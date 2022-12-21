using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon {
    public delegate void OnTextSubmitted(UIComponent component, string text);
    public class CustomTextField {
        public static UIPanel AddLongTypeField(UIPanel panel, long defaultValue, float? width, OnTextSubmitted eventSubmittedCallback, string labelText, Color32 labelTextColor, float labelTextScale) {
            UIPanel m_panel = panel.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsTextfieldTemplate")) as UIPanel;
            m_panel.autoFitChildrenVertically = true;
            UILabel label = m_panel.Find<UILabel>("Label");
            if (labelText is null) label.Hide();
            else {
                label.autoSize = false;
                label.width = panel.width - panel.autoLayoutPadding.horizontal;
                label.autoHeight = true;
                label.wordWrap = true;
                label.text = labelText;
                label.textColor = labelTextColor;
                label.textScale = labelTextScale;
            }
            var longTypeTextField = m_panel.Find<UITextField>("Text Field");
            if (width != null) longTypeTextField.width = width.Value;
            longTypeTextField.atlas = CustomAtlas.CommonAtlas;
            longTypeTextField.normalBgSprite = CustomAtlas.TabButtonNormal;
            longTypeTextField.hoveredBgSprite = CustomAtlas.TabButtonNormal;
            longTypeTextField.selectionSprite = CustomAtlas.EmptySprite;
            longTypeTextField.padding = new RectOffset(6, 6, 6, 6);
            longTypeTextField.textScale = 1.0f;
            longTypeTextField.text = defaultValue.ToString();
            longTypeTextField.eventTextSubmitted += (c, e) => eventSubmittedCallback(c, e);
            return m_panel;
        }
    }
}
