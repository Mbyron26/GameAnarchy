using HarmonyLib;

namespace GameAnarchy {
    [HarmonyPatch(typeof(PlayerBuildingAI), nameof(PlayerBuildingAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedPlayerBuildingAIPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedPlayerBuilding) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(StockExchangeAI), nameof(StockExchangeAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedStockExchangeAIPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedStockExchange) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(UniqueFacultyAI), nameof(UniqueFacultyAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedUniqueFacultyAIPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedUniqueFaculty) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(WeatherRadarAI), nameof(WeatherRadarAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedWeatherRadarAIPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedWeatherRadar) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(UniqueFactoryAI), nameof(UniqueFactoryAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedUniqueFactoryAIPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedUniqueFactory) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(SpaceRadarAI), nameof(SpaceRadarAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedSpaceRadarAIPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedSpaceRadar) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(MonumentAI), nameof(MonumentAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedMonumentPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedMonument) {
                __result = false;
                return false;
            } else return true;
        }
    }

    [HarmonyPatch(typeof(MainCampusBuildingAI), nameof(MainCampusBuildingAI.CanBeBuiltOnlyOnce))]
    public class UnlimitedMainCampusBuildingAIPatch {
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.UnlimitedMainCampusBuilding) {
                __result = false;
                return false;
            } else return true;
        }
    }

#if TEST
    [HarmonyPatch]
    public class UnlimitedUniqueBuildingsPatch {
        public static IEnumerable<MethodBase> TargetMethods() {
            yield return AccessTools.Method(typeof(PlayerBuildingAI), "CanBeBuiltOnlyOnce");
            yield return AccessTools.Method(typeof(StockExchangeAI), "CanBeBuiltOnlyOnce");
            yield return AccessTools.Method(typeof(UniqueFacultyAI), "CanBeBuiltOnlyOnce");
            yield return AccessTools.Method(typeof(WeatherRadarAI), "CanBeBuiltOnlyOnce");
            yield return AccessTools.Method(typeof(UniqueFactoryAI), "CanBeBuiltOnlyOnce");
            yield return AccessTools.Method(typeof(SpaceRadarAI), "CanBeBuiltOnlyOnce");
            yield return AccessTools.Method(typeof(MonumentAI), "CanBeBuiltOnlyOnce");
            yield return AccessTools.Method(typeof(MainCampusBuildingAI), "CanBeBuiltOnlyOnce");
        }
        public static bool Prefix(ref bool __result) {
            if (Config.Instance.EnabledUnlimitedUniqueBuildings) {
                __result = false;
                return false;
            } else return true;
        }
    }
#endif

}
