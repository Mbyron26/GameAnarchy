namespace GameAnarchy.Patches;

using Epic.OnlineServices.Presence;
using HarmonyLib;
using System.Reflection;

public static class UnlimitedUniqueBuildingsPatch {
    public static MethodInfo GetOriginalPlayerBuildingAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(PlayerBuildingAI), nameof(PlayerBuildingAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetPlayerBuildingAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(PlayerBuildingAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalStockExchangeAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(StockExchangeAI), nameof(StockExchangeAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetStockExchangeAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(StockExchangeAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalUniqueFacultyAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(UniqueFacultyAI), nameof(UniqueFacultyAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetUniqueFacultyAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UniqueFacultyAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalWeatherRadarAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(WeatherRadarAI), nameof(WeatherRadarAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetWeatherRadarAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(WeatherRadarAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalUniqueFactoryAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(UniqueFactoryAI), nameof(UniqueFactoryAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetUniqueFactoryAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UniqueFactoryAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalSpaceRadarAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(SpaceRadarAI), nameof(SpaceRadarAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetSpaceRadarAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(SpaceRadarAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalMonumentAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(MonumentAI), nameof(MonumentAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetMonumentAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(MonumentAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalMainCampusBuildingAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(MainCampusBuildingAI), nameof(MainCampusBuildingAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetMainCampusBuildingAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(MainCampusBuildingAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalFestivalAreaAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(FestivalAreaAI), nameof(FestivalAreaAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetFestivalAreaAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(FestivalAreaAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalLibraryAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(LibraryAI), nameof(LibraryAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetLibraryAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(LibraryAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalSpaceElevatorAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(SpaceElevatorAI), nameof(SpaceElevatorAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetSpaceElevatorAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(SpaceElevatorAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalParkAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(ParkAI), nameof(ParkAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetParkAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(ParkAICanBeBuiltOnlyOncePostfix));


    public static void ParkAICanBeBuiltOnlyOncePostfix(ref LibraryAI __instance, ref bool __result) {
        if (__instance.m_info.m_isTreasure) {
            __result = !Config.Instance.UnlimitedParkAI;
        }
    }
    public static void SpaceElevatorAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedSpaceElevator;
    public static void LibraryAICanBeBuiltOnlyOncePostfix(ref LibraryAI __instance, ref bool __result) {
        if (__instance.m_info.m_isTreasure) {
            __result = !Config.Instance.UnlimitedLibraryAI;
        }
    }
    public static void FestivalAreaAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedFestivalArea;
    public static void PlayerBuildingAICanBeBuiltOnlyOncePostfix(ref bool __result) {
        if (Config.Instance.UnlimitedMonument) {
            __result = false;
        }
    }
    public static void StockExchangeAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedStockExchange;
    public static void UniqueFacultyAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedUniqueFaculty;
    public static void WeatherRadarAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedWeatherRadar;
    public static void UniqueFactoryAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedUniqueFactory;
    public static void SpaceRadarAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedSpaceRadar;
    public static void MonumentAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedMonument;
    public static void MainCampusBuildingAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedMainCampusBuilding;
}