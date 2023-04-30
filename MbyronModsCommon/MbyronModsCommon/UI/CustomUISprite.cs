using UnityEngine;
namespace MbyronModsCommon.UI;

public class Sprites {
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
        DisabledSprite = disabledSprite;
    }
    public Color32 GetColor(SpriteState state) => state switch {
        SpriteState.Hovered => HoveredColor,
        SpriteState.Pressed => PressedColor,
        SpriteState.Focused => FocusedColor,
        SpriteState.Disabled => DisabledColor,
        _ => NormalColor,
    };
    public string GetSprite(SpriteState state) => state switch {
        SpriteState.Hovered => HoveredSprite,
        SpriteState.Pressed => PressedSprite,
        SpriteState.Focused => FocusedSprite,
        SpriteState.Disabled => DisabledSprite,
        _ => NormalSprite,
    };
    public void SetSprites(string spriteName) {
        NormalSprite = spriteName;
        HoveredSprite = spriteName;
        PressedSprite = spriteName;
        FocusedSprite = spriteName;
        DisabledSprite = spriteName;
    }
    public void SetSprites(string normalSprite, string hoveredSprite, string pressedSprite, string focusedSprite, string disabledSprite) {
        NormalSprite = normalSprite;
        HoveredSprite = hoveredSprite;
        PressedSprite = pressedSprite;
        FocusedSprite = focusedSprite;
        DisabledSprite = disabledSprite;
    }
    public void SetColors(Color32 color) {
        NormalColor = color;
        HoveredColor = color;
        PressedColor = color;
        FocusedColor = color;
        DisabledColor = color;
    }
    public void SetColors(Color32 normalColor, Color32 hoveredColor, Color32 pressedColor, Color32 focusedColor, Color32 disabledColor) {
        NormalColor = normalColor;
        HoveredColor = hoveredColor;
        PressedColor = pressedColor;
        FocusedColor = focusedColor;
        DisabledColor = disabledColor;
    }
}

public enum SpriteState {
    Normal,
    Hovered,
    Pressed,
    Focused,
    Disabled
}

