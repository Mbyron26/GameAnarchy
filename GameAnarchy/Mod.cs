global using MbyronModsCommon;
namespace GameAnarchy;
using ICities;
using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.Globalization;
using GameAnarchy.UI;
using GameAnarchy.Patches;
using GameAnarchy.Manager;

public class Mod : ModPatcherBase<Mod, Config> {
    public override string ModName { get; } = "Game Anarchy";
    public override ulong StableID => 2781804786;
    public override ulong? BetaID => 2917685008;
    public override string Description => Localize.MOD_Description;

#if BETA_DEBUG
    public override BuildVersion VersionType => BuildVersion.BetaDebug;
#elif BETA_RELEASE
    public override BuildVersion VersionType => BuildVersion.BetaRelease;
#elif STABLE_DEBUG
    public override BuildVersion VersionType => BuildVersion.StableDebug;
#else
    public override BuildVersion VersionType => BuildVersion.StableRelease;
#endif

    public override void SetModCulture(CultureInfo cultureInfo) => Localize.Culture = cultureInfo;
    public override void IntroActions() {
        base.IntroActions();
        CompatibilityCheck.IncompatibleMods = ConflictMods;
        CompatibilityCheck.GetExtraModsInfo += CompatibilityExtension.GetLocalIncompatibleMods;
        CompatibilityCheck.RemoveConflictModsAction += CompatibilityExtension.RemoveConflictMods;
        CompatibilityCheck.CheckCompatibility();
        ExternalLogger.OutputPluginsList();
    }
    public override void OnLevelLoaded(LoadMode mode) {
        base.OnLevelLoaded(mode);
        EconomyExtension.SetStartMoney();
        AchievementsManager.InitializeAchievements(mode);
        InfoViewsManager.Deploy(mode);
        ControlPanelManager<ControlPanel>.EventOnVisibleChanged += (_) => {
            if (UUI.UUIButton is not null) {
                UUI.UUIButton.IsPressed = _;
            }
        };
        UUI.Initialize();
    }
    public override void OnLevelUnloading() {
        base.OnLevelUnloading();
        AchievementsManager.Destroy();
        InfoViewsManager.Destroy();
        UUI.Destory();
    }
    public override void OnReleased() {
        base.OnReleased();
        ExternalLogger.DebugMode($"Building fire spread count: {FireControlManager.buildingFireSpreadCount}, building fire spread allowed: {FireControlManager.buildingFireSpreadAllowed}, tree fire spread count: {FireControlManager.treeFireSpreadCount}, tree fire spread allowed: {FireControlManager.treeFireSpreadAllowed}.", Config.Instance.DebugMode);
    }
    protected override void SettingsUI(UIHelperBase helper) {
        base.SettingsUI(helper);
        OptionPanelManager<Mod, OptionPanel>.SettingsUI(helper);
        LocaleManager.eventLocaleChanged += ControlPanelManager<ControlPanel>.OnLocaleChanged;
    }
    protected override void PatchAction() {
        AddPostfix(OptionsMainPanelPatch.GetOriginalOnVisibilityChanged(), OptionsMainPanelPatch.GetOnVisibilityChangedPostfix());
        AddTranspiler(OptionsMainPanelPatch.GetOriginalAddUserMods(), OptionsMainPanelPatch.GetAddUserModsTranspiler());
        AddPrefix(FastReturnPatch.GetOriginalQuitApplication(), FastReturnPatch.GetQuitApplicationPrefix());
        AddPrefix(UpdateDataStartMoneyPatch.GetOriginalUpdateData(), UpdateDataStartMoneyPatch.GetUpdateDataPrefix());
        AddPostfix(AchievementsPatch.GetOriginalOnListingSelectionChanged(), AchievementsPatch.GetOnListingSelectionChangedPostfix());
        AddPrefix(BulldozeToolPatch.GetOriginalGetBuildingRefundAmount(), BulldozeToolPatch.GetGetBuildingRefundAmountPrefix());
        AddPrefix(BulldozeToolPatch.GetOriginalGetSegmentRefundAmount(), BulldozeToolPatch.GetGetSegmentRefundAmountPrefix());
        AddPrefix(BuildingAIPatch.GetOriginalGetRefundAmount(), BuildingAIPatch.GetGetRefundAmountPrefix());
        AddPrefix(BuildingAIPatch.GetOriginalGetRelocationCost(), BuildingAIPatch.GetGetRelocationCostPrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalPlayerBuildingAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetPlayerBuildingAICanBeBuiltOnlyOncePrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalStockExchangeAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetStockExchangeAICanBeBuiltOnlyOncePrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalUniqueFacultyAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetUniqueFacultyAICanBeBuiltOnlyOncePrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalWeatherRadarAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetWeatherRadarAICanBeBuiltOnlyOncePrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalUniqueFactoryAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetUniqueFactoryAICanBeBuiltOnlyOncePrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalSpaceRadarAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetSpaceRadarAICanBeBuiltOnlyOncePrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalMonumentAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetMonumentAICanBeBuiltOnlyOncePrefix());
        AddPrefix(UnlimitedUniqueBuildingsPatch.GetOriginalMainCampusBuildingAICanBeBuiltOnlyOnce(), UnlimitedUniqueBuildingsPatch.GetMainCampusBuildingAICanBeBuiltOnlyOncePrefix());
        AddPrefix(RemoveFirePatch.GetOriginalCommonBuildingAITrySpreadFire(), RemoveFirePatch.GetCommonBuildingAITrySpreadFirePrefix());
        AddPrefix(RemoveFirePatch.GetOriginalTreeManagerTrySpreadFire(), RemoveFirePatch.GetTreeManagerTrySpreadFirePrefix());
        AddPrefix(RemoveFirePatch.GetOriginalTreeManagerBurnTree(), RemoveFirePatch.GetTreeManagerBurnTreePrefix());
        AddPostfix(RemoveFirePatch.GetOriginalPlayerBuildingAIGetFireParameters(), RemoveFirePatch.GetPlayerBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalResidentialBuildingAIGetFireParameters(), RemoveFirePatch.GetResidentialBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalIndustrialBuildingAIGetFireParameters(), RemoveFirePatch.GetIndustrialBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalCommercialBuildingAIGetFireParameters(), RemoveFirePatch.GetCommercialBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalOfficeBuildingAIGetFireParameters(), RemoveFirePatch.GetOfficeBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalIndustrialExtractorAIGetFireParameters(), RemoveFirePatch.GetIndustrialExtractorAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalParkBuildingAIGetFireParameters(), RemoveFirePatch.GetParkBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalMuseumAIGetFireParameters(), RemoveFirePatch.GetMuseumAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalDummyBuildingAIGetFireParameters(), RemoveFirePatch.GetDummyBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalCampusBuildingAIGetFireParameters(), RemoveFirePatch.GetCampusBuildingAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalAirportGateAIGetFireParameters(), RemoveFirePatch.GetAirportGateAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalAirportCargoGateAIGetFireParameters(), RemoveFirePatch.GetAirportCargoGateAIGetFireParametersPostfix());
        AddPostfix(RemoveFirePatch.GetOriginalAirportBuildingAIGetFireParameters(), RemoveFirePatch.GetAirportBuildingAIGetFireParametersPostfix());
        AddPrefix(RemoveFirePatch.GetOriginalCommonBuildingAIHandleFire(), RemoveFirePatch.GetHandleFirePrefix());
        SkipIntroPatch.Patch(Harmony);
    }

    private List<IncompatibleModInfo> ConflictMods { get; set; } = new()  {
        new IncompatibleModInfo(1567569285, @"Achieve It!", true),
        new IncompatibleModInfo(2037888659, @"Instant Return To Desktop", true),
        new IncompatibleModInfo(466834228, @"Not So Unqiue Buildings", true),
        new IncompatibleModInfo(1263262833, @"Pollution Solution", true),
        new IncompatibleModInfo(973512634, @"Sort Mod Settings", true),
        new IncompatibleModInfo(1665106193, @"Skip Intro", true),
        new IncompatibleModInfo(458519223, @"Unlock All + Wonders & Landmarks", true),
        new IncompatibleModInfo(769744928, @"Pollution, Death, Garbage and Crime Remover Mod", true),
        new IncompatibleModInfo(1237383751, @"Extended Game Options", true),
        new IncompatibleModInfo(1498036881, @"UltimateMod 2.10.2 [STABLE]", true),
        new IncompatibleModInfo(2506369356, @"UltimateMod v2.12.11 [BETA]", true),
        new IncompatibleModInfo(447789816, @"Unlock All Roads", true),
        new IncompatibleModInfo(552324460, @"No Fires", true),
        new IncompatibleModInfo(1718245521, @"No Park Building Fires", true),
    };

    public override List<ModChangeLog> ChangeLog => new() {
        new ModChangeLog(new Version(0, 9, 9), new(2023, 5, 14), new List<LogString> {
            new(LogFlag.Added,Localize.UpdateLog_V0_9_9ADD0),
            new(LogFlag.Added,Localize.UpdateLog_V0_9_9ADD1),
            new(LogFlag.Added,Localize.UpdateLog_V0_9_9ADD2),
            new(LogFlag.Added,Localize.UpdateLog_V0_9_9ADD3),
            new(LogFlag.Added,Localize.UpdateLog_V0_9_9ADD4),
            new(LogFlag.Optimized,Localize.UpdateLog_V0_9_9OPT0),
            new(LogFlag.Optimized,Localize.UpdateLog_V0_9_9OPT1),
            new(LogFlag.Updated,Localize.UpdateLog_V0_9_9UPT),
            new(LogFlag.Updated,Localize.UpdateLog_V0_9_9UPT1),
            new(LogFlag.Fixed,Localize.UpdateLog_V0_9_9FIX0),
            new(LogFlag.Translation,Localize.UpdateLog_V0_9_9TRAN),
        }),
        new ModChangeLog(new Version(0, 9, 8), new(2023, 3, 22), new List<LogString> {
            new(LogFlag.Added,"[UPT]Updated to support game version 1.16.1"),
            new(LogFlag.Added, Localize.UpdateLog_V0_9_8UPT),
            new(LogFlag.Updated,Localize.UpdateLog_V0_9_8ADD),
            new(LogFlag.Optimized, Localize.UpdateLog_V0_9_8OPT1),
            new(LogFlag.Optimized, Localize.UpdateLog_V0_9_8OPT2),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_9_8FIX1),
            new(LogFlag.Fixed, Localize.UpdateLog_V0_9_8FIX2),
        }),
        new ModChangeLog(new Version(0, 9, 7), new(2023, 3, 8), new List<LogString> {
            new(LogFlag.Added, Localize.UpdateLog_V0_9_7ADD1),
            new(LogFlag.Added, Localize.UpdateLog_V0_9_7ADD2),
            new(LogFlag.Added, Localize.UpdateLog_V0_9_7ADD3),
            new(LogFlag.Added, Localize.UpdateLog_V0_9_7ADD4),
            new(LogFlag.Added, Localize.UpdateLog_V0_9_7ADD5),
            new(LogFlag.Added, Localize.UpdateLog_V0_9_7ADD2),
            new(LogFlag.Added, Localize.UpdateLog_V0_9_7ADD2),
            new(LogFlag.Updated, Localize.UpdateLog_V0_9_7UPT1),
            new(LogFlag.Updated, Localize.UpdateLog_V0_9_7UPT2),
            new(LogFlag.Fixed,Localize.UpdateLog_V0_9_7FIX),
        }),
    };

}