namespace GameAnarchy.Patches;
using HarmonyLib;
using System.Reflection;

public static class UnlimitedUniqueBuildingsPatch {
    public static MethodInfo GetOriginalPlayerBuildingAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(PlayerBuildingAI), nameof(PlayerBuildingAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetPlayerBuildingAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.PlayerBuildingAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalStockExchangeAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(StockExchangeAI), nameof(StockExchangeAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetStockExchangeAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.StockExchangeAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalUniqueFacultyAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(UniqueFacultyAI), nameof(UniqueFacultyAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetUniqueFacultyAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.UniqueFacultyAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalWeatherRadarAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(WeatherRadarAI), nameof(WeatherRadarAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetWeatherRadarAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.WeatherRadarAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalUniqueFactoryAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(UniqueFactoryAI), nameof(UniqueFactoryAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetUniqueFactoryAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.UniqueFactoryAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalSpaceRadarAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(SpaceRadarAI), nameof(SpaceRadarAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetSpaceRadarAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.SpaceRadarAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalMonumentAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(MonumentAI), nameof(MonumentAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetMonumentAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.MonumentAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalMainCampusBuildingAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(MainCampusBuildingAI), nameof(MainCampusBuildingAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetMainCampusBuildingAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.MainCampusBuildingAICanBeBuiltOnlyOncePostfix));
    public static MethodInfo GetOriginalFestivalAreaAICanBeBuiltOnlyOnce() => AccessTools.Method(typeof(FestivalAreaAI), nameof(FestivalAreaAI.CanBeBuiltOnlyOnce));
    public static MethodInfo GetFestivalAreaAICanBeBuiltOnlyOncePostfix() => AccessTools.Method(typeof(UnlimitedUniqueBuildingsPatch), nameof(UnlimitedUniqueBuildingsPatch.FestivalAreaAICanBeBuiltOnlyOncePostfix));

    public static void FestivalAreaAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedFestivalArea;
    public static void PlayerBuildingAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedPlayerBuilding; 
    public static void StockExchangeAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedStockExchange; 
    public static void UniqueFacultyAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedUniqueFaculty;
    public static void WeatherRadarAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedWeatherRadar;
    public static void UniqueFactoryAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedUniqueFactory;
    public static void SpaceRadarAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedSpaceRadar;
    public static void MonumentAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedMonument;
    public static void MainCampusBuildingAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !Config.Instance.UnlimitedMainCampusBuilding;
}