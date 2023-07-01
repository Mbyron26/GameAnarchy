namespace GameAnarchy.UI;
using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

internal static class UIAtlas {
    private static UITextureAtlas gameAnarchyAtlas;

    public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();
    public static string InGameButton => nameof(InGameButton);
    public static UITextureAtlas GameAnarchyAtlas {
        get {
            if (gameAnarchyAtlas is null) {
                gameAnarchyAtlas = MbyronModsCommon.UI.UIUtils.CreateTextureAtlas(nameof(GameAnarchyAtlas), $"{AssemblyUtils.CurrentAssemblyName}.UI.Resources.", SpriteParams);
                InternalLogger.Log("Initialized GameAnarchyAtlas");
            }
            return gameAnarchyAtlas;
        }
    }

    static UIAtlas() {
        SpriteParams[InGameButton] = new RectOffset(1, 1, 1, 1);
    }
}
