using ColossalFramework.UI;
using System;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class CustomUICheckBox : CustomUIButtonBase {
    public CustomUICheckBox() {
        renderFg = true;
        buttonType = UIButtonType.Toggle;
    }

    public static CustomUICheckBox Add(UIComponent parent, bool isOn, Action<bool> callback = null) {
        var checkbox = parent.AddUIComponent<CustomUICheckBox>();
        checkbox.autoSize = false;
        checkbox.size = new Vector2(20, 20);
        checkbox.SetCheckBoxStyle();
        checkbox.IsOn = isOn;
        checkbox.EventToggleChanged += (_) => callback?.Invoke(_);
        return checkbox;
    }
    public void SetCheckBoxStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        onBgSprites.SetSprites(CustomUIAtlas.Circle);
        onBgSprites.SetColors(CustomUIColor.BlueNormal, CustomUIColor.BlueHovered, CustomUIColor.BluePressed, CustomUIColor.BlueFocused, CustomUIColor.BlueDisabled);

        onFgSprites.SetSprites(CustomUIAtlas.CheckBoxOnFg);
        OnFgDisabledColor = new Color32(60, 60, 60, 255);

        offBgSprites.SetSprites(CustomUIAtlas.CheckBoxOffBg);
        offBgSprites.SetColors(CustomUIColor.OPButtonNormal, CustomUIColor.OPButtonHovered, CustomUIColor.OPButtonPressed, CustomUIColor.OPButtonFocused, CustomUIColor.OPButtonDisabled);

        offFgSprites.SetSprites(CustomUIAtlas.TransparencySprite);
        SetStyle();
    }

}

