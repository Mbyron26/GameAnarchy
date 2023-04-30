using ColossalFramework.UI;
using System.Linq;
using UnityEngine;
namespace MbyronModsCommon.UI;

public static class CustomUIFontHelper {
    private static UIFont regular;
    private static UIFont semiBold;

    public static UIFont Regular {
        get => regular ??= Resources.FindObjectsOfTypeAll<UIFont>().FirstOrDefault((UIFont f) => f.name == "OpenSans-Regular");
    }
    public static UIFont SemiBold {
        get => semiBold ??= Resources.FindObjectsOfTypeAll<UIFont>().FirstOrDefault((UIFont f) => f.name == "OpenSans-Semibold");
    }

}

