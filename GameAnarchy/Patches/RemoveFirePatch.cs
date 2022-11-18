using HarmonyLib;

namespace GameAnarchy {

    [HarmonyPatch(typeof(CommercialBuildingAI), "GetFireParameters")]
    internal static class CommercialBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 10;
        }
    }

    [HarmonyPatch(typeof(IndustrialExtractorAI), "GetFireParameters")]
    internal static class IndustrialExtractorAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 10;
        }
    }

    [HarmonyPatch(typeof(OfficeBuildingAI), "GetFireParameters")]
    internal static class OfficeBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 10;
        }
    }

    [HarmonyPatch(typeof(PlayerBuildingAI), "GetFireParameters")]
    internal static class PlayerBuildingAIFirePatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

    [HarmonyPatch(typeof(ResidentialBuildingAI), "GetFireParameters")]
    internal static class ResidentialBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 8;
        }
    }

    [HarmonyPatch(typeof(MuseumAI), "GetFireParameters")]
    internal static class MuseumAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

    [HarmonyPatch(typeof(IndustrialBuildingAI), "GetFireParameters")]
    internal static class IndustrialBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 10;
        }
    }

    [HarmonyPatch(typeof(DummyBuildingAI), "GetFireParameters")]
    internal static class DummyBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }
    [HarmonyPatch(typeof(CampusBuildingAI), "GetFireParameters")]
    internal static class CampusBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

    [HarmonyPatch(typeof(AirportGateAI), "GetFireParameters")]
    internal static class AirportGateAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

    [HarmonyPatch(typeof(AirportCargoGateAI), "GetFireParameters")]
    internal static class AirportCargoGateAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }
    [HarmonyPatch(typeof(AirportBuildingAI), "GetFireParameters")]
    internal static class AirportBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) fireTolerance = 1000;
            else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

}
