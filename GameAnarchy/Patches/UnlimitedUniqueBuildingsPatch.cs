using HarmonyLib;
using System.Runtime.CompilerServices;

namespace GameAnarchy {

    [HarmonyPatch(typeof(MonumentAI), "CanBeBuiltOnlyOnce")]
    internal class MonumentAIPatch {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.EnabledUnlimitedUniqueBuildings) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(PlayerBuildingAI), "CanBeBuiltOnlyOnce")]
    internal class PlayerBuildingAIPatch {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.EnabledUnlimitedUniqueBuildings) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(SpaceRadarAI), "CanBeBuiltOnlyOnce")]
    internal class SpaceRadarAIPatch {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.EnabledUnlimitedUniqueBuildings) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(UniqueFactoryAI), "CanBeBuiltOnlyOnce")]
    internal class UniqueFactoryAIPatch {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.EnabledUnlimitedUniqueBuildings) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(WeatherRadarAI), "CanBeBuiltOnlyOnce")]
    internal class WeatherRadarAIPatch {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.EnabledUnlimitedUniqueBuildings) {
                __result = false;
                return false;
            } else return true;
        }
    }

}
