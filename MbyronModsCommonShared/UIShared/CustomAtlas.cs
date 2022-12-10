using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon {
    public static class CustomAtlas {
        // ------Tab Button------
        public static string TabButtonNormal => nameof(TabButtonNormal);
        public static string TabButtonHovered => nameof(TabButtonHovered);
        public static string TabButtonPressed => nameof(TabButtonPressed);

        // ------Custom Slider------
        public static string SliderSprite => nameof(SliderSprite);
        public static string SliderMidSprite => nameof(SliderMidSprite);
        public static string SliderLeftSprite => nameof(SliderLeftSprite);
        public static string SliderRightSprite => nameof(SliderRightSprite);

        public static string GradientSlider => nameof(GradientSlider);
        public static string SliderThumb => nameof(SliderThumb);


        public static string[] Resource = new string[] {
            TabButtonNormal,
            TabButtonHovered,
            TabButtonPressed,
            SliderSprite,
            SliderMidSprite,
            SliderLeftSprite,
            SliderRightSprite,
            GradientSlider,
            SliderThumb,
        };
        public static UITextureAtlas commonAtlas;

        public static UITextureAtlas CommonAtlas {
            get {
                if (commonAtlas is null) {
                    commonAtlas = UIUtils.CreateTextureAtlas(@"CommonAtlas", $"{AssemblyUtils.CurrentAssemblyName}.Resources.", Resource, 1024);
                    return commonAtlas;
                } else {
                    return commonAtlas;
                }
            }
        }
        public static UITextureAtlas InGameAtlas { get; } = GetAtlas("Ingame");

        public static UITextureAtlas GetAtlas(string name) {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++) {
                if (atlases[i].name == name)
                    return atlases[i];
            }
            return UIView.GetAView().defaultAtlas;
        }
    }
}
