using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace GameAnarchy {
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

}
