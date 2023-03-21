using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public static class CustomAtlas {
        private static UITextureAtlas inGameAtlas;
        private static UITextureAtlas commonAtlas;
        public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();

        //------Base Button------
        public static string ButtonNormal => nameof(ButtonNormal);
        public static string ButtonHovered => nameof(ButtonHovered);
        public static string ButtonPressed => nameof(ButtonPressed);

        // ------Tab------
        public static string TabButtonDisabled => nameof(TabButtonDisabled);
        public static string TabButtonNormal => nameof(TabButtonNormal);
        public static string TabButtonHovered => nameof(TabButtonHovered);


        // Slider Alpha
        public static string SliderAlphaSprite => nameof(SliderAlphaSprite);
        public static string SliderAlphaMidSprite => nameof(SliderAlphaMidSprite);
        public static string SliderAlphaLeftSprite => nameof(SliderAlphaLeftSprite);
        public static string SliderAlphaRightSprite => nameof(SliderAlphaRightSprite);

        // Slider Gamma
        public static string SliderGamma => nameof(SliderGamma);
        public static string SliderThumb => nameof(SliderThumb);

        //Check Box
        public static string CheckBoxFGOneNormal => nameof(CheckBoxFGOneNormal);
        public static string CheckBoxFGOneDisabled => nameof(CheckBoxFGOneDisabled);
        public static string CheckBoxBGZeroNormal => nameof(CheckBoxBGZeroNormal);
        public static string CheckBoxBGZeroHovered => nameof(CheckBoxBGZeroHovered);
        public static string CheckBoxBGZeroDisabled => nameof(CheckBoxBGZeroDisabled);
        public static string CheckBoxBGOneNormal => nameof(CheckBoxBGOneNormal);
        public static string CheckBoxBGOneHovered => nameof(CheckBoxBGOneHovered);
        public static string CheckBoxBGOneDisabled => nameof(CheckBoxBGOneDisabled);


        public static string EmptySprite => nameof(EmptySprite);
        public static string TextFieldNormal => nameof(TextFieldNormal);
        public static string TextFieldHovered => nameof(TextFieldHovered);

        public static string ListBackground => nameof(ListBackground);

        public static string FieldDisabled => nameof(FieldDisabled);
        public static string FieldFocused => nameof(FieldFocused);
        public static string FieldHovered => nameof(FieldHovered);
        public static string FieldNormal => nameof(FieldNormal);

        public static string ArrowDown => nameof(ArrowDown);
        public static string ArrowDown1 => nameof(ArrowDown1);

        public static string ToggleButtonBGOneDisabled => nameof(ToggleButtonBGOneDisabled);
        public static string ToggleButtonBGOneHovered => nameof(ToggleButtonBGOneHovered);
        public static string ToggleButtonBGOneNormal => nameof(ToggleButtonBGOneNormal);
        public static string ToggleButtonBGZeroDisabled => nameof(ToggleButtonBGZeroDisabled);
        public static string ToggleButtonBGZeroHovered => nameof(ToggleButtonBGZeroHovered);
        public static string ToggleButtonBGZeroNormal => nameof(ToggleButtonBGZeroNormal);
        public static string ToggleButtonFGOneDisabled => nameof(ToggleButtonFGOneDisabled);
        public static string ToggleButtonFGOneNormal => nameof(ToggleButtonFGOneNormal);
        public static string ToggleButtonFGZeroDisabled => nameof(ToggleButtonFGZeroDisabled);
        public static string ToggleButtonFGZeroNormal => nameof(ToggleButtonFGZeroNormal);

        public static string CloseButtonNormal => nameof(CloseButtonNormal);
        public static string CloseButtonHovered => nameof(CloseButtonHovered);
        public static string CloseButtonPressed => nameof(CloseButtonPressed);
        public static string ResetButtonHovered => nameof(ResetButtonHovered);
        public static string ResetButtonNormal => nameof(ResetButtonNormal);
        public static string ResetButtonPressed => nameof(ResetButtonPressed);
        public static string FieldDisabledLeft => nameof(FieldDisabledLeft);
        public static string FieldDisabledRight => nameof(FieldDisabledRight);
        public static string FieldFocusedLeft => nameof(FieldFocusedLeft);
        public static string FieldFocusedRight => nameof(FieldFocusedRight);
        public static string FieldHoveredLeft => nameof(FieldHoveredLeft);
        public static string FieldHoveredRight => nameof(FieldHoveredRight);
        public static string FieldNormalLeft => nameof(FieldNormalLeft);
        public static string FieldNormalRight => nameof(FieldNormalRight);

        public static string TabNormal => nameof(TabNormal);
        public static string TabHovered => nameof(TabHovered);
        public static string TabFocused => nameof(TabFocused);
        public static string TabPressed => nameof(TabPressed);
        
        static CustomAtlas() {
            SpriteParams[ButtonNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ButtonHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ButtonPressed] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderAlphaSprite] = new RectOffset();
            SpriteParams[SliderAlphaMidSprite] = new RectOffset();
            SpriteParams[SliderAlphaLeftSprite] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderAlphaRightSprite] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderGamma] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderThumb] = new RectOffset(4, 4, 4, 4);

            SpriteParams[EmptySprite] = new RectOffset();
            SpriteParams[TextFieldNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TextFieldHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ListBackground] = new RectOffset(4, 4, 4, 4);

            SpriteParams[FieldDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldFocused] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldNormal] = new RectOffset(4, 4, 4, 4);

            SpriteParams[ArrowDown] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ArrowDown1] = new RectOffset(4, 4, 4, 4);

            SpriteParams[ToggleButtonBGOneDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonBGOneHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonBGOneNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonBGZeroDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonBGZeroHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonBGZeroNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonFGOneDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonFGOneNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonFGZeroDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ToggleButtonFGZeroNormal] = new RectOffset(4, 4, 4, 4);

            SpriteParams[CloseButtonNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CloseButtonHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CloseButtonPressed] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ResetButtonHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ResetButtonNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ResetButtonPressed] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldDisabledLeft] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldDisabledRight] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldFocusedLeft] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldFocusedRight] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldHoveredLeft] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldHoveredRight] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldNormalLeft] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldNormalRight] = new RectOffset(4, 4, 4, 4);

            SpriteParams[TabNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabFocused] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabPressed] = new RectOffset(4, 4, 4, 4);

            SpriteParams[CheckBoxFGOneNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxFGOneDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGZeroNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGZeroHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGZeroDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGOneNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGOneHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGOneDisabled] = new RectOffset(4, 4, 4, 4);

        }

        public static UITextureAtlas CommonAtlas {
            get {
                if (commonAtlas is null) {
                    commonAtlas = UIUtils.CreateTextureAtlas(nameof(CommonAtlas), $"{AssemblyUtils.CurrentAssemblyName}.UI.Resources.", SpriteParams);
                    ModLogger.ModLog("Initialized CommonAtlas succeed.");
                    return commonAtlas;
                } else {
                    return commonAtlas;
                }
            }
        }

        public static UITextureAtlas InGameAtlas {
            get {
                if (inGameAtlas is null) {
                    inGameAtlas = UIUtils.GetAtlas("Ingame");
                    return inGameAtlas;
                } else {
                    return inGameAtlas;
                }
            }
        }

    }
}
