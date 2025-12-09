using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using GameAnarchy.ModSettings;

namespace GameAnarchy.Patches;

public class UnlimitedUniqueBuildingsPatch {
    private static ModSetting _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();

    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.ApplyPostfix<PlayerBuildingAI, UnlimitedUniqueBuildingsPatch>(nameof(PlayerBuildingAI.CanBeBuiltOnlyOnce), nameof(PlayerBuildingAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<StockExchangeAI, UnlimitedUniqueBuildingsPatch>(nameof(StockExchangeAI.CanBeBuiltOnlyOnce), nameof(StockExchangeAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<UniqueFacultyAI, UnlimitedUniqueBuildingsPatch>(nameof(UniqueFacultyAI.CanBeBuiltOnlyOnce), nameof(UniqueFacultyAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<WeatherRadarAI, UnlimitedUniqueBuildingsPatch>(nameof(WeatherRadarAI.CanBeBuiltOnlyOnce), nameof(WeatherRadarAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<UniqueFactoryAI, UnlimitedUniqueBuildingsPatch>(nameof(UniqueFactoryAI.CanBeBuiltOnlyOnce), nameof(UniqueFactoryAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<SpaceRadarAI, UnlimitedUniqueBuildingsPatch>(nameof(SpaceRadarAI.CanBeBuiltOnlyOnce), nameof(SpaceRadarAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<MonumentAI, UnlimitedUniqueBuildingsPatch>(nameof(MonumentAI.CanBeBuiltOnlyOnce), nameof(MonumentAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<MainCampusBuildingAI, UnlimitedUniqueBuildingsPatch>(nameof(MainCampusBuildingAI.CanBeBuiltOnlyOnce), nameof(MainCampusBuildingAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<FestivalAreaAI, UnlimitedUniqueBuildingsPatch>(nameof(FestivalAreaAI.CanBeBuiltOnlyOnce), nameof(FestivalAreaAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<LibraryAI, UnlimitedUniqueBuildingsPatch>(nameof(LibraryAI.CanBeBuiltOnlyOnce), nameof(LibraryAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<SpaceElevatorAI, UnlimitedUniqueBuildingsPatch>(nameof(SpaceElevatorAI.CanBeBuiltOnlyOnce), nameof(SpaceElevatorAICanBeBuiltOnlyOncePostfix));
        harmonyPatcher.ApplyPostfix<ParkAI, UnlimitedUniqueBuildingsPatch>(nameof(ParkAI.CanBeBuiltOnlyOnce), nameof(ParkAICanBeBuiltOnlyOncePostfix));
    }

    public static void ParkAICanBeBuiltOnlyOncePostfix(ref LibraryAI __instance, ref bool __result) {
        if (__instance.m_info.m_isTreasure) __result = !_modSetting.UnlimitedParkAI;
    }

    public static void SpaceElevatorAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedSpaceElevator;

    public static void LibraryAICanBeBuiltOnlyOncePostfix(ref LibraryAI __instance, ref bool __result) {
        if (__instance.m_info.m_isTreasure) __result = !_modSetting.UnlimitedLibraryAI;
    }

    public static void FestivalAreaAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedFestivalArea;

    public static void PlayerBuildingAICanBeBuiltOnlyOncePostfix(ref bool __result) {
        if (_modSetting.UnlimitedMonument) __result = false;
    }

    public static void StockExchangeAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedStockExchange;

    public static void UniqueFacultyAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedUniqueFaculty;

    public static void WeatherRadarAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedWeatherRadar;

    public static void UniqueFactoryAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedUniqueFactory;

    public static void SpaceRadarAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedSpaceRadar;

    public static void MonumentAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedMonument;

    public static void MainCampusBuildingAICanBeBuiltOnlyOncePostfix(ref bool __result) => __result = !_modSetting.UnlimitedMainCampusBuilding;
}