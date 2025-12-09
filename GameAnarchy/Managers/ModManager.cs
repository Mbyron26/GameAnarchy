using System;
using System.Collections.Generic;
using CSLModsCommon;
using CSLModsCommon.Compatibility;
using CSLModsCommon.Localization;
using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using GameAnarchy.Localization;
using GameAnarchy.ModSettings;
using GameAnarchy.Patches;

namespace GameAnarchy.Managers;

public class ModManager : PatchModManagerBase {
    public override string ModName => "Game Anarchy";
    public override string RowDescription => "Extends and optimize game's functions.";
    public override DateTime VersionDate { get; } = new(2025, 12, 9);
    public override string ModTranslationURL => "https://crowdin.com/project/game-anarchy";
    public override string ModSteamURL => "https://steamcommunity.com/sharedfiles/filedetails/?id=2781804786";

    protected override void OnUpdateMangers(UpdateManager updateManager) {
        base.OnUpdateMangers(updateManager);
        updateManager.UpdateAt<ControlPanelManager>(UpdatePhase.Default);
        updateManager.UpdateAt<InGameToolButtonManager>(UpdatePhase.Default);
        updateManager.UpdateAt<AchievementsManager>(UpdatePhase.Default);
        updateManager.UpdateAt<ModEconomyManager>(UpdatePhase.Simulation);
        updateManager.UpdateAt<CityServicesManager>(UpdatePhase.Simulation);
        updateManager.UpdateAt<FireControlManager>(UpdatePhase.Default);
        updateManager.UpdateAt<OptionsPanelCategoriesOffsetManager>(UpdatePhase.Default);
        updateManager.UpdateAt<ModUnlockManager>(UpdatePhase.Default);
    }

    protected override void OnCreateSettings(SettingManager settingManager) => settingManager.Load<ModSetting>();

    protected override void RegisterPatches(HarmonyPatcher harmonyPatcher) {
        AchievementsPatch.Patch(harmonyPatcher);
        FastReturnPatch.Patch(harmonyPatcher);
        OptionsMainPanelPatch.Patch(harmonyPatcher);
        UpdateDataStartMoneyPatch.Patch(harmonyPatcher);
        BuildingAIPatch.Patch(harmonyPatcher);
        BulldozeToolPatch.Patch(harmonyPatcher);
        UnlimitedUniqueBuildingsPatch.Patch(harmonyPatcher);
        RemoveFirePatch.Patch(harmonyPatcher);
        SkipIntroPatch.Patch(harmonyPatcher);
    }

    protected override void AddIncompatibleModRule(IIncompatibleModRule rule) {
        base.AddIncompatibleModRule(rule);
        rule.Add("UnlockAll", IncompatibilityModLevel.EnableNotAllowed, "Unlock All", true, string.Empty, Translations.UnlockAllConflict)
            .Add("UnlimitedOilAndOre", IncompatibilityModLevel.EnableNotAllowed, "Unlimited Oil And Ore", true, string.Empty, Translations.UnlimitedOilAndOreConflict)
            .Add("UnlimitedMoney2", IncompatibilityModLevel.EnableNotAllowed, "Unlimited Money", true, string.Empty, Translations.UnlimitedMoneyConflict)
            .Add("AchieveIt", IncompatibilityModLevel.LoadNotAllowed, "Achieve It!")
            .Add("InstantReturnToDesktop", IncompatibilityModLevel.LoadNotAllowed, "Instant Return To Desktop")
            .Add("NotSoUniqueBuildings", IncompatibilityModLevel.LoadNotAllowed, "Not So Unique Buildings")
            .Add("PollutionSolution", IncompatibilityModLevel.LoadNotAllowed, "Pollution Solution")
            .Add("SortModSettings", IncompatibilityModLevel.LoadNotAllowed, "Sort Mod Settings")
            .Add("SkipIntro", IncompatibilityModLevel.LoadNotAllowed, "Skip Intro")
            .Add("UnlockAllWondersAndLandmarks", IncompatibilityModLevel.LoadNotAllowed, "Unlock All + Wonders & Landmarks")
            .Add("PollutionRemoverMod", IncompatibilityModLevel.LoadNotAllowed, "Pollution, Death, Garbage and Crime Remover Mod")
            .Add("ExtendedGameOptions", IncompatibilityModLevel.LoadNotAllowed, "Extended Game Options")
            .Add("UltimateMod", IncompatibilityModLevel.LoadNotAllowed, "UltimateMod 2.10.4 [STABLE]")
            .Add("UltimateModBeta", IncompatibilityModLevel.LoadNotAllowed, "UltimateMod v2.12.14 [BETA]")
            .Add("Unlock All Roads0", IncompatibilityModLevel.LoadNotAllowed, "Unlock All Roads")
            .Add("NoFires2", IncompatibilityModLevel.LoadNotAllowed, "No Fires")
            .Add("NoParkBuildingFires", IncompatibilityModLevel.LoadNotAllowed, "No Park Building Fires")
            .Add("InfoPanelOnLoad", IncompatibilityModLevel.LoadNotAllowed, "InfoPanelOnLoad")
            .Add("InfoViewButtonsEnabler", IncompatibilityModLevel.LoadNotAllowed, "Info View Button Enabler")
            .Add("UnlockLandScaping", IncompatibilityModLevel.LoadNotAllowed, "Unlock LandScaping")
            .Add("Unlock Public Transport0", IncompatibilityModLevel.LoadNotAllowed, "Unlock Public Transport")
            .Add("YouCanBuildIt", IncompatibilityModLevel.LoadNotAllowed, "You Can Build It")
            .Add("NoPoliciesCosts", IncompatibilityModLevel.LoadNotAllowed, "No Policies Costs");
    }

    protected override void AddVersionModRule(IVersionModRule rule) {
        base.AddVersionModRule(rule);
        rule.Set(1, 20, 1, 1);
    }

    protected override List<ChangelogCollection> GenerateChangelogs() => [
        new ChangelogCollection(new Version(1, 3, 0), new DateTime(2025, 12, 9))
            .AddEntry(ChangelogFlag.Updated, new FormattedString(nameof(SharedTranslations.UpdatedToCSLModsCommon), "1.0"))
            .AddEntry(ChangelogFlag.Updated, new FormattedString(nameof(SharedTranslations.UpdatedToGameVersion), "1.20.1")),
        new(new Version(1, 2, 1), new DateTime(2024, 9, 7)),
        new(new Version(1, 2, 0), new DateTime(2024, 8, 31)),
        new(new Version(1, 1, 4), new DateTime(2024, 7, 20)),
        new(new Version(1, 1, 3), new DateTime(2023, 8, 5)),
        new(new Version(1, 1, 2), new DateTime(2023, 7, 3)),
        new(new Version(1, 1, 1), new DateTime(2023, 7, 1)),
        new(new Version(1, 1, 0), new DateTime(2023, 6, 18)),
        new(new Version(1, 0, 1), new DateTime(2023, 5, 24)),
        new(new Version(1, 0, 0), new DateTime(2023, 5, 23))
    ];
}