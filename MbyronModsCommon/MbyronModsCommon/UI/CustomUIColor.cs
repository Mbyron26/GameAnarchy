using UnityEngine;
namespace MbyronModsCommon.UI;

public static class CustomUIColor {
    public static Color32 DisabledTextColor => new(71, 71, 71, 255);
    public static Color32 White => new(255, 255, 255, 255);
    public static Color32 OffWhite => new(200, 200, 200, 255);

    public static Color32 CPPrimaryBg => new(70, 94, 114, 255);
    public static Color32 CPPrimaryFg => new(146, 162, 174, 255);
    public static Color32 CPButtonNormal => new(92, 120, 140, 255);
    public static Color32 CPButtonHovered => new(104, 134, 156, 255);
    public static Color32 CPButtonPressed => new(86, 114, 132, 255);
    public static Color32 CPButtonFocused => BlueNormal;
    public static Color32 CPButtonDisabled => new(52, 70, 86, 255);


    public static Color32 GreenNormal => new(76, 164, 50, 255);
    public static Color32 GreenHovered => new(86, 184, 58, 255);
    public static Color32 GreenPressed => new(72, 154, 46, 255);
    public static Color32 GreenFocused => GreenNormal;
    public static Color32 GreenDisabled => new(44, 86, 44, 255);

    public static Color32 BlueNormal => new(10, 92, 186, 255);
    public static Color32 BlueHovered => new(40, 126, 226, 255);
    public static Color32 BluePressed => new(18, 102, 200, 255);
    public static Color32 BlueFocused => BlueNormal;
    public static Color32 BlueDisabled => new(22, 42, 76, 255);

    public static Color32 ToggleFgNormal => new(220, 220, 220, 255);
    public static Color32 ToggleFgDisabled => new(110, 110, 110, 255);


    public static Color32 OPPrimaryBg => new(36, 48, 62, 255);
    public static Color32 OPPrimaryFg => new(78, 86, 100, 255);
    public static Color32 OPButtonNormal => new(56, 70, 86, 255);
    public static Color32 OPButtonHovered => new(70, 84, 102, 255);
    public static Color32 OPButtonPressed => OPButtonNormal;
    public static Color32 OPButtonFocused => BlueNormal;
    public static Color32 OPButtonDisabled => new(46, 58, 70, 255);

}