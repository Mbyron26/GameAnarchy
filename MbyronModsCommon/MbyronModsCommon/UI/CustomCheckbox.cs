using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomCheckBox {
        public static CheckBox AddCheckBox(UIComponent parent, bool isOn, Action<bool> callback = null) {
            var checkbox = parent.AddUIComponent<CheckBox>();
            checkbox.autoSize = false;
            checkbox.size = new Vector2(20, 20);
            checkbox.SetStyle();
            checkbox.IsOn = isOn;
            checkbox.EventCheckChanged += (_) => callback?.Invoke(_);
            return checkbox;
        }
    }

    public class CheckBox : CustomButtonBase {
        public override void SetStyle() {
            atlas = CustomAtlas.MbyronModsAtlas;
            NormalOffBgSprite = CustomAtlas.CheckBoxOffBg;
            HoveredOffBgSprite = CustomAtlas.CheckBoxOffBg;
            PressedOffBgSprite = CustomAtlas.CheckBoxOffBg;
            FocusedOffBgSprite = CustomAtlas.CheckBoxOffBg;
            DisabledOffBgSprite = CustomAtlas.CheckBoxOffBg;
            NormalOnBgSprite = CustomAtlas.CheckBoxOnBg;
            HoveredOnBgSprite = CustomAtlas.CheckBoxOnBg;
            PressedOnBgSprite = CustomAtlas.CheckBoxOnBg;
            FocusedOnBgSprite = CustomAtlas.CheckBoxOnBg;
            DisabledOnBgSprite = CustomAtlas.CheckBoxOnBg;

            NormalOffFgSprite = CustomAtlas.TransparencySprite;
            HoveredOffFgSprite = CustomAtlas.TransparencySprite;
            PressedOffFgSprite = CustomAtlas.TransparencySprite;
            FocusedOffFgSprite = CustomAtlas.TransparencySprite;
            DisabledOffFgSprite = CustomAtlas.TransparencySprite;
            NormalOnFgSprite = CustomAtlas.CheckBoxOnFg;
            HoveredOnFgSprite = CustomAtlas.CheckBoxOnFg;
            PressedOnFgSprite = CustomAtlas.CheckBoxOnFg;
            FocusedOnFgSprite = CustomAtlas.CheckBoxOnFg;
            DisabledOnFgSprite = CustomAtlas.CheckBoxOnFg;

            NormalOffBgColor = CustomColor.DefaultButtonNormal;
            HoveredOffBgColor = CustomColor.DefaultButtonHovered;
            PressedOffBgColor = CustomColor.DefaultButtonPressed;
            FocusedOffBgColor = CustomColor.DefaultButtonFocused;
            DisabledOffBgColor = CustomColor.DefaultButtonDisabled;
            NormalOnBgColor = CustomColor.BlueNormal;
            HoveredOnBgColor = CustomColor.BlueHovered;
            PressedOnBgColor = CustomColor.BluePressed;
            FocusedOnBgColor = CustomColor.BlueNormal;
            DisabledOnBgColor = CustomColor.BlueDisabled;
            DisabledOnFgColor = new Color32(60, 60, 60, 255);

        }
    }

}
