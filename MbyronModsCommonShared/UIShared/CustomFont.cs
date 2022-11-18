using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomFont {
        private static UIFont regular;
        private static UIFont semiBold;

        public static UIFont Regular {
            get => regular ??= Resources.FindObjectsOfTypeAll<UIFont>().FirstOrDefault((UIFont f) => f.name == "OpenSans-Regular");
        }
        public static UIFont SemiBold {
            get => semiBold ??= Resources.FindObjectsOfTypeAll<UIFont>().FirstOrDefault((UIFont f) => f.name == "OpenSans-Semibold");
        }

    }
}
