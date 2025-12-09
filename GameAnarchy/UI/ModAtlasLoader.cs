using ColossalFramework.UI;
using CSLModsCommon.UI.Atlas;
using CSLModsCommon.Utilities;
using UnityEngine;

namespace GameAnarchy.UI;

public class ModAtlasLoader : AtlasLoader {
    private static ModAtlasLoader _modAtlas;

    public static UITextureAtlas ModAtlas => (_modAtlas ??= new ModAtlasLoader()).Atlas;
    public static string InGameButton => nameof(InGameButton);

    public override string AtlasName => $"{AssemblyHelper.CurrentAssemblyName}Atlas";
    public override string ResourcePath => $"{AssemblyHelper.CurrentAssemblyName}.UI.Resources";

    protected override void RegisterSprites() {
        base.RegisterSprites();
        SpriteParams[InGameButton] = new RectOffset();
    }
}