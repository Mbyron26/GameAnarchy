using ColossalFramework.UI;
using CSShared.ToolButton;
using UnityEngine;

namespace GameAnarchy.UI;

internal class ToolButton : ToolButtonBase<Config> {
    public override Vector2 DefaultPosition { get; set; } = GetDefaultPosition();
    public override void Start() {
        base.Start();
        fgAtlas = UIAtlas.GameAnarchyAtlas;
        offFgSprites.SetSprites(UIAtlas.InGameButton);
        onFgSprites.SetSprites(UIAtlas.InGameButton);
        renderFg = true;
    }
    private static Vector2 GetDefaultPosition() {
        Vector2 resolution = UIView.GetAView().GetScreenResolution();
        var pos = new Vector2(resolution.x - 60f, resolution.y * 3f / 4f + 60);
        return pos;
    }
}
