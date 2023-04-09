using UnityEngine;

namespace MbyronModsCommon.UI {
    public struct Sprites {
        public string NormalSprite = string.Empty;
        public string HoveredSprite = string.Empty;
        public string PressedSprite = string.Empty;
        public string FocusedSprite = string.Empty;
        public string DisabledSprite = string.Empty;
        public Color32 NormalColor = Color.white;
        public Color32 HoveredColor = Color.white;
        public Color32 PressedColor = Color.white;
        public Color32 FocusedColor = Color.white;
        public Color32 DisabledColor = Color.white;

        public Sprites() { }
        public Sprites(string normalSprite, string hoveredSprite, string pressedSprite, string focusedSprite, string disabledSprite) {
            NormalSprite = normalSprite;
            HoveredSprite = hoveredSprite;
            PressedSprite = pressedSprite;
            FocusedSprite = focusedSprite;
            FocusedSprite = disabledSprite;
        }
        public Color32 GetSpriteColor(SpriteState state) => state switch {
            SpriteState.Hovered => HoveredColor,
            SpriteState.Pressed => PressedColor,
            SpriteState.Focused => FocusedColor,
            SpriteState.Disabled => DisabledColor,
            _ => NormalColor,
        };
    }

    public enum SpriteState {
        Normal,
        Hovered,
        Pressed,
        Focused,
        Disabled
    }
}

