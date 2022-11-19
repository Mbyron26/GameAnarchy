using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomDropdown {
        public static UIDropDown AddDropdown(UIComponent parent, string textLabel, float textLabelScale, string[] options, int defaultSelection,
                float dropDownWidth, float dropDownHeight, float dropDownTextScale, RectOffset textFieldPadding = null, RectOffset itemPadding = null) {
            UIPanel uiPanel = parent.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsDropdownTemplate")) as UIPanel;
            uiPanel.autoFitChildrenHorizontally = true;
            uiPanel.autoFitChildrenVertically = true;
            var label = uiPanel.Find<UILabel>("Label");
            label.textScale = textLabelScale;
            label.text = textLabel;
            label.disabledTextColor = new Color32(71, 71, 71, 255);
            var dropDown = uiPanel.Find<UIDropDown>("Dropdown");
            dropDown.items = options;
            dropDown.width = dropDownWidth;
            dropDown.height = dropDownHeight;
            dropDown.textScale = dropDownTextScale;
            if (textFieldPadding != null) dropDown.textFieldPadding = textFieldPadding;
            if (itemPadding != null) dropDown.itemPadding = itemPadding;
            dropDown.listScrollbar = null;
            dropDown.listHeight = dropDown.itemHeight * options.Length + 8;
            dropDown.selectedIndex = defaultSelection;
            dropDown.disabledColor = new Color32(71,71,71,255);
            return dropDown;
        }
    }
}
