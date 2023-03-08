global using MbyronModsCommon;
using CitiesHarmony.API;
using GameAnarchy.FireControl;
using ICities;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using GameAnarchy.Localization;

namespace GameAnarchy {
    public class Mod : ModBase<Mod, OptionPanel, Config> {
        public override string SolidModName => "GameAnarchy";
        public override string ModName => "Game Anarchy";
        public override Version ModVersion => new(0, 9, 7);
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

        public override void OnReleased() {
            base.OnReleased();
            ModLogger.ModLog($"Building fire spread count: {FireControlManager.buildingFireSpreadCount}, building fire spread allowed: {FireControlManager.buildingFireSpreadAllowed}, tree fire spread count: {FireControlManager.treeFireSpreadCount}, tree fire spread allowed: {FireControlManager.treeFireSpreadAllowed}.", Config.Instance.DebugMode);
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

        public override List<ModUpdateInfo> ChangeLog => new() {
            new ModUpdateInfo(new Version(0, 9, 7), @"2023/03/08", new List<string> {
                Localize.UpdateLog_V0_9_7ADD1,Localize.UpdateLog_V0_9_7ADD2,Localize.UpdateLog_V0_9_7ADD3,Localize.UpdateLog_V0_9_7ADD4,
                Localize.UpdateLog_V0_9_7FIX,Localize.UpdateLog_V0_9_7UPT1,Localize.UpdateLog_V0_9_7UPT2
            })
        };

    }
}
