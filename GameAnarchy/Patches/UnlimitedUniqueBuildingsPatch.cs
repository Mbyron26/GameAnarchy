namespace GameAnarchy.Patches;
using HarmonyLib;
using System.Reflection;

public static class UnlimitedUniqueBuildingsPatch {
    public static MethodInfo GetOriginalPlayerBuildingAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(PlayerBuildingAI), nameof(PlayerBuildingAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetPlayerBuildingAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.PlayerBuildingAICanBeBuiltOnlyOncePrefix));
    public static MethodInfo GetOriginalStockExchangeAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(StockExchangeAI), nameof(StockExchangeAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetStockExchangeAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.StockExchangeAICanBeBuiltOnlyOncePrefix));
    public static MethodInfo GetOriginalUniqueFacultyAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(UniqueFacultyAI), nameof(UniqueFacultyAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetUniqueFacultyAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.UniqueFacultyAICanBeBuiltOnlyOncePrefix));
    public static MethodInfo GetOriginalWeatherRadarAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(WeatherRadarAI), nameof(WeatherRadarAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetWeatherRadarAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.WeatherRadarAICanBeBuiltOnlyOncePrefix));
    public static MethodInfo GetOriginalUniqueFactoryAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(UniqueFactoryAI), nameof(UniqueFactoryAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetUniqueFactoryAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.UniqueFactoryAICanBeBuiltOnlyOncePrefix));
    public static MethodInfo GetOriginalSpaceRadarAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(SpaceRadarAI), nameof(SpaceRadarAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetSpaceRadarAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.SpaceRadarAICanBeBuiltOnlyOncePrefix));
    public static MethodInfo GetOriginalMonumentAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(MonumentAI), nameof(MonumentAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetMonumentAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.MonumentAICanBeBuiltOnlyOncePrefix));
    public static MethodInfo GetOriginalMainCampusBuildingAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(MainCampusBuildingAI), nameof(MainCampusBuildingAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetMainCampusBuildingAICanBeBuiltOnlyOncePrefix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.MainCampusBuildingAICanBeBuiltOnlyOncePrefix));

    public static bool PlayerBuildingAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedPlayerBuilding) {
            __result = false;
            return false;
        }
        return true;
    }
    public static bool StockExchangeAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedStockExchange) {
            __result = false;
            return false;
        }
        return true;
    }
    public static bool UniqueFacultyAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedUniqueFaculty) {
            __result = false;
            return false;
        }
        return true;
    }
    public static bool WeatherRadarAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedWeatherRadar) {
            __result = false;
            return false;
        }
        return true;
    }
    public static bool UniqueFactoryAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedUniqueFactory) {
            __result = false;
            return false;
        }
        return true;
    }
    public static bool SpaceRadarAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedSpaceRadar) {
            __result = false;
            return false;
        }
        return true;
    }
    public static bool MonumentAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedMonument) {
            __result = false;
            return false;
        }
        return true;
    }
    public static bool MainCampusBuildingAICanBeBuiltOnlyOncePrefix(ref bool __result) {
        if (Config.Instance.UnlimitedMainCampusBuilding) {
            __result = false;
            return false;
        }
        return true;
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