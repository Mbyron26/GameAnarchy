using CitiesHarmony.API;
using ICities;
using MbyronModsCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace GameAnarchy {
    public class Mod : ModBase<Mod, OptionPanel, Config> {
        public override string SolidModName => "GameAnarchy";
        public override string ModName => "Game Anarchy";
        public override Version ModVersion => new(0, 9, 6);
        public override ulong ModID => 2781804786;
#if DEBUG
        public override ulong? BetaID => 2917685008;
#endif
        public override string Description => Localize.MOD_Description;
        private GameObject InfoViewsObject { get; set; }
        public override void SetModCulture(CultureInfo cultureInfo) {
            Localize.Culture = cultureInfo;
        }
        public override void IntroActions() {
            base.IntroActions();
            CompatibilityCheck.IncompatibleMods = ConflictMods;
            CompatibilityCheck.GetExtraModsInfo += CompatibilityExtension.GetLocalIncompatibleMods;
            CompatibilityCheck.RemoveConflictModsAction += CompatibilityExtension.RemoveConflictMods;
            CompatibilityCheck.CheckCompatibility();
            ModLogger.OutputPluginsList();
        }

        public bool AchievementFlag { get; set; }
        public override void OnLevelLoaded(LoadMode mode) {
            base.OnLevelLoaded(mode);
            EconomyExtension.UpdateStartCash();
            AchievementsManager.InitializeAchievements(mode);
            InfoViewsObject = new GameObject("InfoViewsExtension");
            InfoViewsObject.AddComponent<InfoViewsExtension>();
            //if (UnlockManager.exists) {
            //    ModLogger.ModLog("exists");
            //    var all = UnlockManager.instance.m_allMilestones;
            //    foreach (var item in all) {
            //        ModLogger.ModLog(item.Key);
            //    }
            //}
        }
        public override void OnLevelUnloading() {
            base.OnLevelUnloading();
            AchievementsManager.Destroy();
            if (InfoViewsObject != null) {
                UnityEngine.Object.Destroy(InfoViewsObject);
            }
            ModLogger.OutputPluginsList();
        }
        public override void OnEnabled() {
            base.OnEnabled();
            HarmonyHelper.DoOnHarmonyReady(Patcher.EnablePatches);

        }
        public override void OnDisabled() {
            base.OnDisabled();
            if (HarmonyHelper.IsHarmonyInstalled) {
                Patcher.DisablePatches();
            }
        }

        public override string GetLocale(string text) => Localize.ResourceManager.GetString(text, ModCulture);

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
        };

        #region ModUpdateLogs
        public override List<ModUpdateInfo> ModUpdateLogs { get; set; } = new List<ModUpdateInfo>() {
            new ModUpdateInfo(new Version(0, 9, 6), @"2023/02/05", new List<string> {
                "UpdateLog_V0_9_6UPT","UpdateLog_V0_9_6FIX","UpdateLog_V0_9_6OPT"
            }),
            new ModUpdateInfo(new Version(0, 9, 5), @"2022/12/21", new List<string> {
                "UpdateLog_V0_9_5UPT1","UpdateLog_V0_9_5UPT2","UpdateLog_V0_9_5ADD"
            }),
            new ModUpdateInfo(new Version(0, 9, 4), @"2022/12/13", new List<string> {
                "UpdateLog_V0_9_4UPT","UpdateLog_V0_9_4ADD"
            }),
            new ModUpdateInfo(new Version(0, 9, 3), @"2022/12/08", new List<string> {
                "UpdateLog_V0_9_3FIX","UpdateLog_V0_9_3ADJ","UpdateLog_V0_9_3OPT","UpdateLog_V0_9_3ADD",
            }),
            new ModUpdateInfo(new Version(0, 9, 2), @"2022/12/04", new List<string> {
                "UpdateLog_V0_9_2OPT1","UpdateLog_V0_9_2OPT2","UpdateLog_V0_9_2ADD","UpdateLog_V0_9_2ADJ",
            }),
            new ModUpdateInfo(new Version(0, 9, 1), @"2022/11/19", new List<string> {
                "UpdateLog_V0_9_1_ADJ1","UpdateLog_V0_9_1_OPT1", "UpdateLog_V0_9_1_ADD1",
            }),
            new ModUpdateInfo(new Version(0, 9, 0), @"2022/11/10", new List<string> {
                "UpdateLog_V0_9_0_ADD1", "UpdateLog_V0_9_0_ADD2",  "UpdateLog_V0_9_0_ADD3", "UpdateLog_V0_9_0_ADD4",
                "UpdateLog_V0_9_0_UPT1", "UpdateLog_V0_9_0_UPT2","UpdateLog_V0_9_0_UPT3",
                "UpdateLog_V0_9_0_FIX1",
            })
        };
        #endregion
    }
}
