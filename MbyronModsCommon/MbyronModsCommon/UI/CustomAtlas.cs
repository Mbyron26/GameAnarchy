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
        public static string RoundedRectangle4 => nameof(RoundedRectangle4);

        //Circle
        public static string Circle => nameof(Circle);

        //Toggle
        public static string ToggleDisabledOff => nameof(ToggleDisabledOff);
        public static string ToggleDisabledOn => nameof(ToggleDisabledOn);
        public static string ToggleHoveredOff => nameof(ToggleHoveredOff);
        public static string ToggleHoveredOn => nameof(ToggleHoveredOn);
        public static string ToggleNormalOff => nameof(ToggleNormalOff);
        public static string ToggleNormalOn => nameof(ToggleNormalOn);

        // Slider Alpha
        public static string SliderAlphaSprite => nameof(SliderAlphaSprite);
        public static string SliderAlphaMidSprite => nameof(SliderAlphaMidSprite);
        public static string SliderAlphaLeftSprite => nameof(SliderAlphaLeftSprite);
        public static string SliderAlphaRightSprite => nameof(SliderAlphaRightSprite);

        // Slider Gamma
        public static string SliderGamma => nameof(SliderGamma);
        public static string SliderThumb => nameof(SliderThumb);

        //Check Box
        public static string CheckBoxOnBg => nameof(CheckBoxOnBg);
        public static string CheckBoxOnFg => nameof(CheckBoxOnFg);
        public static string CheckBoxOffBg => nameof(CheckBoxOffBg);


        public static string EmptySprite => nameof(EmptySprite);
        public static string TransparencySprite => nameof(TransparencySprite);


        public static string FieldDisabled => nameof(FieldDisabled);
        public static string FieldFocused => nameof(FieldFocused);
        public static string FieldHovered => nameof(FieldHovered);
        public static string FieldNormal => nameof(FieldNormal);

        public static string ArrowDown => nameof(ArrowDown);
        public static string ArrowDown1 => nameof(ArrowDown1);


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
            SpriteParams[RoundedRectangle4] = new RectOffset(12, 12, 12, 12);

            SpriteParams[Circle] = new RectOffset();

            SpriteParams[ToggleDisabledOff] = new RectOffset(12, 12, 12, 12);
            SpriteParams[ToggleDisabledOn] = new RectOffset(12, 12, 12, 12);
            SpriteParams[ToggleHoveredOff] = new RectOffset(12, 12, 12, 12);
            SpriteParams[ToggleHoveredOn] = new RectOffset(12, 12, 12, 12);
            SpriteParams[ToggleNormalOff] = new RectOffset(12, 12, 12, 12);
            SpriteParams[ToggleNormalOn] = new RectOffset(12, 12, 12, 12);

            SpriteParams[CheckBoxOffBg] = new RectOffset(7, 7, 7, 7);
            SpriteParams[CheckBoxOnBg] = new RectOffset(7, 7, 7, 7);
            SpriteParams[CheckBoxOnFg] = new RectOffset();

            SpriteParams[SliderAlphaSprite] = new RectOffset();
            SpriteParams[SliderAlphaMidSprite] = new RectOffset();
            SpriteParams[SliderAlphaLeftSprite] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderAlphaRightSprite] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderGamma] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderThumb] = new RectOffset(4, 4, 4, 4);

            SpriteParams[EmptySprite] = new RectOffset();
            SpriteParams[TransparencySprite] = new RectOffset();

            SpriteParams[FieldDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldFocused] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldNormal] = new RectOffset(4, 4, 4, 4);

            SpriteParams[ArrowDown] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ArrowDown1] = new RectOffset(4, 4, 4, 4);


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
