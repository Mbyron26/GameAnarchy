using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon {
    public static class CustomAtlas {
        private static UITextureAtlas inGameAtlas;
        private static UITextureAtlas commonAtlas;
        public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();
        public static string Path => $"{AssemblyUtils.CurrentAssemblyName}.Resources.";

        //------Base Button------
        public static string ButtonNormal => nameof(ButtonNormal);
        public static string ButtonHovered => nameof(ButtonHovered);
        public static string ButtonPressed => nameof(ButtonPressed);

        // ------Tab------
        public static string TabButtonDisabled => nameof(TabButtonDisabled);
        public static string TabButtonNormal => nameof(TabButtonNormal);
        public static string TabButtonHovered => nameof(TabButtonHovered);
        public static string TabButtonPressed => nameof(TabButtonPressed);
        public static string TabButtonFocused => nameof(TabButtonFocused);

        // ------Custom Slider------
        public static string SliderSprite => nameof(SliderSprite);
        public static string SliderMidSprite => nameof(SliderMidSprite);
        public static string SliderLeftSprite => nameof(SliderLeftSprite);
        public static string SliderRightSprite => nameof(SliderRightSprite);

        public static string GradientSlider => nameof(GradientSlider);
        public static string SliderThumb => nameof(SliderThumb);

        public static string EmptySprite => nameof(EmptySprite);
        public static string TextFieldNormal => nameof(TextFieldNormal);
        public static string TextFieldHovered => nameof(TextFieldHovered);
        public static string CornerMark => nameof(CornerMark);
        public static string ListBackground => nameof(ListBackground);

        public static string FieldDisabled => nameof(FieldDisabled);
        public static string FieldFocused => nameof(FieldFocused);
        public static string FieldHovered => nameof(FieldHovered);
        public static string FieldNormal => nameof(FieldNormal);

        public static string ArrowDown => nameof(ArrowDown);
        static CustomAtlas() {
            SpriteParams[ButtonNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ButtonHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[ButtonPressed] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonPressed] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TabButtonFocused] = new RectOffset(4, 4, 4, 4);
            SpriteParams[SliderSprite] = new RectOffset();
            SpriteParams[SliderMidSprite] = new RectOffset();
            SpriteParams[SliderLeftSprite] = new RectOffset();
            SpriteParams[SliderRightSprite] = new RectOffset();
            SpriteParams[GradientSlider] = new RectOffset();
            SpriteParams[SliderThumb] = new RectOffset();
            SpriteParams[EmptySprite] = new RectOffset();
            SpriteParams[TextFieldNormal] = new RectOffset(4, 4, 4, 4);
            SpriteParams[TextFieldHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[CornerMark] = new RectOffset();
            SpriteParams[ListBackground] = new RectOffset(4, 4, 4, 4);

            SpriteParams[FieldDisabled] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldFocused] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldHovered] = new RectOffset(4, 4, 4, 4);
            SpriteParams[FieldNormal] = new RectOffset(4, 4, 4, 4);

            SpriteParams[ArrowDown] = new RectOffset(4, 4, 4, 4);
        }

        public static UITextureAtlas CommonAtlas {
            get {
                if (commonAtlas is null) {
                    commonAtlas = UIUtils.CreateTextureAtlas(nameof(CommonAtlas), Path, SpriteParams);
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
