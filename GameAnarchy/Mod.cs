using ColossalFramework.Globalization;
using CSShared.Common;
using CSShared.Manager;
using CSShared.Patch;
using CSShared.UI.ControlPanel;
using CSShared.UI.OptionPanel;
using GameAnarchy.Managers;
using GameAnarchy.Patches;
using GameAnarchy.UI;
using ICities;
using System;
using System.Collections.Generic;

namespace GameAnarchy;

public class Mod : ModPatcherBase<Mod, Config> {
    public override string ModName { get; } = "Game Anarchy";
    public override ulong StableID => 2781804786;
    public override ulong? BetaID => 2917685008;
    public override string Description => Localize("MOD_Description");

#if BETA_DEBUG
    public override BuildVersion VersionType => BuildVersion.BetaDebug;
#elif BETA_RELEASE
    public override BuildVersion VersionType => BuildVersion.BetaRelease;
#elif STABLE_DEBUG
    public override BuildVersion VersionType => BuildVersion.StableDebug;
#else
    public override BuildVersion VersionType => BuildVersion.StableRelease;
#endif

    protected override void Enable() {
        base.Enable();
        ManagerPool.GetOrCreateManager<Manager>();
    }

    public override void IntroActions() {
        ManagerPool.GetOrCreateManager<BuiltInModManager>();
    }

    protected override void SettingsUI(UIHelperBase helper) {
        OptionPanelManager<Mod, OptionPanel>.SettingsUI(helper);
        LocaleManager.eventLocaleChanged += ControlPanelManager<Mod, ControlPanel>.OnLocaleChanged;
    }

    protected override void PatchAction(HarmonyPatcher harmonyPatcher) {
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

    public override List<ConflictModInfo> ConflictMods { get; set; } = new()  {
        new ConflictModInfo(1567569285, @"Achieve It!", true),
        new ConflictModInfo(2037888659, @"Instant Return To Desktop", true),
        new ConflictModInfo(466834228, @"Not So Unique Buildings", true),
        new ConflictModInfo(1263262833, @"Pollution Solution", true),
        new ConflictModInfo(973512634, @"Sort Mod Settings", true),
        new ConflictModInfo(1665106193, @"Skip Intro", true),
        new ConflictModInfo(458519223, @"Unlock All + Wonders & Landmarks", true),
        new ConflictModInfo(769744928, @"Pollution, Death, Garbage and Crime Remover Mod", true),
        new ConflictModInfo(1237383751, @"Extended Game Options", true),
        new ConflictModInfo(1498036881, @"UltimateMod 2.10.2 [STABLE]", true),
        new ConflictModInfo(2506369356, @"UltimateMod v2.12.11 [BETA]", true),
        new ConflictModInfo(447789816, @"Unlock All Roads", true),
        new ConflictModInfo(552324460, @"No Fires", true),
        new ConflictModInfo(1718245521, @"No Park Building Fires", true),
        new ConflictModInfo(1595663918, @"InfoPanelOnLoad", true),
        new ConflictModInfo(409809475, @"Info View Button Enabler", true),
        new ConflictModInfo(1614062928, @"Unlock LandScaping", true),
        new ConflictModInfo(635093438, @"Unlock Public Transport", true),
        new ConflictModInfo(635093438, @"Unlock All Roads", true),
        new ConflictModInfo(2962363030, @"You Can Build It", true),
        new ConflictModInfo(413483125, @"No Policies Costs", true),
    };

    public override List<ChangelogInfo> Changelog => new() {
        new ChangelogInfo(new Version(1, 2, 1), new(2024, 9, 7), new List<ChangelogContent> {
            new(ChangelogFlag.Fixed, "Fixed serialization exception issues."),
        }),
        new ChangelogInfo(new Version(1, 2, 0), new(2024, 8, 31), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("Changelog_1_2_0")),
            new(ChangelogFlag.Updated, Localize("Changelog_1_2_1")),
        }),
        new ChangelogInfo(new Version(1, 1, 4), new(2024, 7, 20), new List<ChangelogContent> {
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_1_4ADD0")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_1_4ADD1")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_1_4FIX0")),
        }),
        new ChangelogInfo(new Version(1, 1, 3), new(2023, 8, 5), new List<ChangelogContent> {
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_1_3ADD")),
            new(ChangelogFlag.Updated, Localize("UpdateLog_V1_1_3UPT")),
            new(ChangelogFlag.Optimized, Localize("UpdateLog_V1_1_3OPT")),
        }),
        new ChangelogInfo(new Version(1, 1, 2), new(2023, 7, 3), new List<ChangelogContent> {
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_1_2FIX")),
            new(ChangelogFlag.Updated, Localize("UpdateLog_V1_1_2UPT")),
        }),
        new ChangelogInfo(new Version(1, 1, 1), new(2023, 7, 1), new List<ChangelogContent> {
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_1_1FIX0")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_1_1FIX1")),
        }),
        new ChangelogInfo(new Version(1, 1, 0), new(2023, 6, 18), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("UpdateLog_V1_1_UPT0")),
            new(ChangelogFlag.Updated, Localize("UpdateLog_V1_1_UPT1")),
            new(ChangelogFlag.Updated, Localize("UpdateLog_V1_1UPT2")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_1ADD0")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_1ADD1")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_1ADD2")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_1ADD3")),
            new(ChangelogFlag.Optimized, Localize("UpdateLog_V1_1OPT0")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_1FIX0")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_1FIX1")),
            new(ChangelogFlag.Translation, Localize("UpdateLog_V1_1TRA")),
        }),
        new ChangelogInfo(new Version(1, 0, 1), new(2023, 5, 24), new List<ChangelogContent> {
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_0_1FIX0")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_0_1FIX1")),
        }),
        new ChangelogInfo(new Version(1, 0, 0), new(2023, 5, 23), new List<ChangelogContent> {
            new(ChangelogFlag.Updated, Localize("UpdateLog_V1_0_0UPT")),
            new(ChangelogFlag.Added, Localize("UpdateLog_V1_0_0ADD0")),
            new(ChangelogFlag.Fixed, Localize("UpdateLog_V1_0_0FIX0")),
        }),
    };

}