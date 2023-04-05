using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomColor {
        public static Color32 White => new(255, 255, 255, 255);
        public static Color32 OffWhite => new(200, 200, 200, 255);
        public static Color32 DisabledTextColor => new(71, 71, 71, 255);
        public static Color32 GreenNormal => new(126, 179, 69, 255);
        public static Color32 GreenDisabled => new(50, 60, 30, 255);
        public static Color32 GreenHovered => new(158, 217, 94, 255);
        public static Color32 GreenPressed => new(105, 177, 26, 255);
        public static Color32 Orange => new(170, 93, 46, 255);

        public static Color32 BlueNormal => new(20, 112, 220, 255);
        public static Color32 BlueHovered => new(40, 120, 220, 255);
        public static Color32 BluePressed => new(64, 134, 216, 255);
        public static Color32 BlueDisabled => new(30, 54, 90, 255);

        public static Color32 GrayNormal => new(50, 64, 80, 255);
        public static Color32 GrayHovered => new(56, 74, 92, 255);
        public static Color32 GrayPressed => new(78, 100, 120, 255);
        public static Color32 GrayDisabled => new(36, 46, 56, 255);

        public static Color32 Red => new(170, 4, 4, 255);

        public static Color32 PrimaryNormal => new (46, 54, 72, 255);
        //public static Color32 PrimaryHovered => new(88, 106, 124, 255);
        public static Color32 PrimaryHovered => new(64, 80, 100, 255);
        public static Color32 Secondary => new(0, 0, 4, 255);
    }
}
