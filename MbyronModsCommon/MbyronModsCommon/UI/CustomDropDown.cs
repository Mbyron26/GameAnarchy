using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomDropDown {
        public static UIDropDown AddCPDropDown(UIComponent parent, string[] options, int defaultSelection, float width, float height, OnDropdownSelectionChanged eventCallback = null) {
            var dropDown = parent.AddUIComponent<UIDropDown>();
            dropDown.width = width;
            dropDown.height = height;
            dropDown.listWidth = (int)width;
            dropDown.itemHeight = (int)height;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.textFieldPadding = dropDown.itemPadding = new RectOffset(8, 0, 4, 0);
            dropDown.textScale = 0.8f;
            dropDown.atlas = CustomAtlas.MbyronModsAtlas;
            dropDown.normalBgSprite = CustomAtlas.FieldNormal;
            dropDown.disabledBgSprite = CustomAtlas.FieldDisabled;
            dropDown.hoveredBgSprite = CustomAtlas.FieldHovered;
            dropDown.focusedBgSprite = CustomAtlas.FieldNormal;
            dropDown.listBackground = CustomAtlas.FieldHovered;
            dropDown.itemHover = CustomAtlas.FieldNormal;
            dropDown.itemHighlight = CustomAtlas.FieldFocused;
            dropDown.popupColor = CustomColor.White;
            dropDown.popupTextColor = CustomColor.White;
            dropDown.triggerButton = dropDown;
            dropDown.items = options;
            dropDown.selectedIndex = defaultSelection;
            var arrowDown = dropDown.AddUIComponent<UIPanel>();
            arrowDown.atlas = CustomAtlas.MbyronModsAtlas;
            arrowDown.backgroundSprite = CustomAtlas.ArrowDown1;
            arrowDown.autoSize = false;
            arrowDown.size = new Vector2(22, 22);
            arrowDown.disabledColor = new Color32(100, 100, 100, 255);
            arrowDown.isEnabled = dropDown.isEnabled;
            arrowDown.relativePosition = new Vector2(dropDown.width - 3 - 20, -1);
            dropDown.eventSelectedIndexChanged += (c, v) => eventCallback?.Invoke(v);
            return dropDown;
        }

        public static UIDropDown AddOPDropDown(UIComponent parent, string[] options, int defaultSelection, float width, float height, OnDropdownSelectionChanged eventCallback = null) {
            var dropDown = parent.AddUIComponent<UIDropDown>();
            dropDown.atlas = CustomAtlas.MbyronModsAtlas;
            dropDown.normalBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.disabledBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.hoveredBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.focusedBgSprite = CustomAtlas.RoundedRectangle2;
            dropDown.listBackground = CustomAtlas.RoundedRectangle2;
            dropDown.itemHover = CustomAtlas.RoundedRectangle2;
            dropDown.itemHighlight = CustomAtlas.RoundedRectangle2;
            dropDown.color = CustomColor.PrimaryHovered;
            dropDown.items = options;
            dropDown.popupColor = CustomColor.White;
            dropDown.popupTextColor = CustomColor.White;
            dropDown.width = width;
            dropDown.height = height;
            dropDown.textScale = 1f;
            dropDown.useDropShadow = true;
            dropDown.itemPadding = dropDown.textFieldPadding = new(6, 6, 6, 0);
            dropDown.listScrollbar = null;
            dropDown.listHeight = dropDown.itemHeight * options.Length + 8;
            dropDown.selectedIndex = defaultSelection;
            dropDown.disabledColor = CustomColor.DisabledTextColor;
            var arrowDown = dropDown.AddUIComponent<UIPanel>();
            arrowDown.atlas = CustomAtlas.MbyronModsAtlas;
            arrowDown.backgroundSprite = CustomAtlas.ArrowDown;
            arrowDown.size = new Vector2(26, 26);
            arrowDown.disabledColor = new Color32(100, 100, 100, 255);
            arrowDown.isEnabled = dropDown.isEnabled;
            arrowDown.relativePosition = new Vector2(dropDown.width - 26 - 4, 2);
            dropDown.triggerButton = dropDown;
            dropDown.eventSelectedIndexChanged += (c, v) => eventCallback?.Invoke(v);
            return dropDown;
        }

    }
}
