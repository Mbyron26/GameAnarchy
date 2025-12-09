using ColossalFramework.UI;
using CSLModsCommon.ToolButton;
using UnityEngine;

namespace GameAnarchy.UI;

internal class ToolButton : ToolButtonBase {
    protected override UITextureAtlas ButtonAtlas { get; } = ModAtlasLoader.ModAtlas;
    protected override string ButtonSpriteName { get; } = ModAtlasLoader.InGameButton;

    protected override Vector2 GetDefaultPosition() => new Vector2(ScreenFixedSize.x - 60f, ScreenFixedSize.y * 3f / 4f + 60);
}