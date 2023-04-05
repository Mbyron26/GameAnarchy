using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public static class CustomAtlas {
        private static UITextureAtlas inGameAtlas;
        private static UITextureAtlas mbyronModsAtlas;
        public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();

        //Rounded Rectangle
        public static string RoundedRectangle1 => nameof(RoundedRectangle1);
        public static string RoundedRectangle2 => nameof(RoundedRectangle2);
        public static string RoundedRectangle3 => nameof(RoundedRectangle3);


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


        static CustomAtlas() {
            SpriteParams[RoundedRectangle1] = new RectOffset(4, 4, 4, 4);
            SpriteParams[RoundedRectangle2] = new RectOffset(6, 6, 6, 6);
            SpriteParams[RoundedRectangle3] = new RectOffset(8, 8, 8, 8);

            SpriteParams[SliderAlphaSprite] = new RectOffset();
            SpriteParams[SliderAlphaMidSprite] = new RectOffset();
            SpriteParams[SliderAlphaLeftSprite] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderAlphaRightSprite] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderGamma] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderThumb] = new RectOffset(4, 4, 4, 4);

            SpriteParams[EmptySprite] = new RectOffset();

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


            SpriteParams[CheckBoxFGOneNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxFGOneDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGZeroNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGZeroHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGZeroDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGOneNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGOneHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CheckBoxBGOneDisabled] = new RectOffset(4, 4, 4, 4);

        }

        public static UITextureAtlas MbyronModsAtlas {
            get {
                if (mbyronModsAtlas is null) {
                    var atlas = UIUtils.GetAtlas(nameof(MbyronModsAtlas));
                    if (atlas is not null) {
                        mbyronModsAtlas = atlas;
                    } else {
                        mbyronModsAtlas = UIUtils.CreateTextureAtlas(nameof(MbyronModsAtlas), $"{AssemblyUtils.CurrentAssemblyName}.UI.Resources.", SpriteParams);
                        ExternalLogger.Log("Initialized MbyronModsAtlas.");
                    }
                    return mbyronModsAtlas;
                } else {
                    return mbyronModsAtlas;
                }
            }
        }

        public static UITextureAtlas InGameAtlas {
            get {
                if (inGameAtlas is null) {
                    inGameAtlas = UIUtils.GetAtlas("Ingame");
                    inGameAtlas ??= UIUtils.GetDefaultAtlas();
                }
                return inGameAtlas;
            }
        }

    }
}
