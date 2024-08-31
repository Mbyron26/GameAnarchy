using ColossalFramework.UI;
using CSShared.Debug;
using CSShared.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnarchy.UI;

internal static class UIAtlas {
    private static UITextureAtlas gameAnarchyAtlas;

    public static Dictionary<string, RectOffset> SpriteParams { get; private set; } = new();
    public static string InGameButton => nameof(InGameButton);
    public static UITextureAtlas GameAnarchyAtlas {
        get {
            if (gameAnarchyAtlas is null) {
                gameAnarchyAtlas = CSShared.UI.UIUtils.CreateTextureAtlas(nameof(GameAnarchyAtlas), $"{AssemblyTools.CurrentAssemblyName}.UI.Resources.", SpriteParams);
                LogManager.GetLogger().Info("Initialized GameAnarchyAtlas");
            }
            return gameAnarchyAtlas;
        }
    }

    static UIAtlas() {
        SpriteParams[InGameButton] = new RectOffset(1, 1, 1, 1);
    }
}
