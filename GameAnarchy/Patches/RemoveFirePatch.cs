using HarmonyLib;

namespace GameAnarchy {

    [HarmonyPatch(typeof(CommonBuildingAI), "TrySpreadFire")]
    public static class CommonBuildingAITrySpreadFirePatch {
        public static bool Prefix() => FireControlManager.GetFireProbability(Config.Instance.BuildingSpreadFireProbability, ref FireControlManager.buildingFireSpreadCount, ref FireControlManager.buildingFireSpreadAllowed);
    }

    [HarmonyPatch(typeof(TreeManager), "TrySpreadFire")]
    public static class TreeManagerTrySpreadFirePatch {
        public static bool Prefix() => FireControlManager.GetFireProbability(Config.Instance.BuildingSpreadFireProbability, ref FireControlManager.buildingFireSpreadCount, ref FireControlManager.buildingFireSpreadAllowed);
    }

    [HarmonyPatch(typeof(TreeManager), "BurnTree")]
    public static class TreeManagerBurnTreePatch {
        public static bool Prefix() => FireControlManager.GetFireProbability(Config.Instance.TreeSpreadFireProbability, ref FireControlManager.treeFireSpreadCount, ref FireControlManager.treeFireSpreadAllowed);
    }


    [HarmonyPatch(typeof(PlayerBuildingAI), "GetFireParameters")]
    internal static class PlayerBuildingAIFirePatch {
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemovePlayerBuildingFire;
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        if (fireHazard != 0) {
        //            fireHazard = 0;
        //        }
        //        fireTolerance = 1000;
        //    }
        //}
    }

    [HarmonyPatch(typeof(ResidentialBuildingAI), "GetFireParameters")]
    internal static class ResidentialBuildingAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) {
        //        fireHazard = 80;
        //        fireTolerance = 8;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveResidentialBuildingFire;
    }

    [HarmonyPatch(typeof(IndustrialBuildingAI), "GetFireParameters")]
    internal static class IndustrialBuildingAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) {
        //        fireHazard = 200;
        //        fireTolerance = 10;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveIndustrialBuildingFire;
    }

    [HarmonyPatch(typeof(CommercialBuildingAI), "GetFireParameters")]
    internal static class CommercialBuildingAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) {
        //        fireHazard = 150;
        //        fireTolerance = 10;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveCommercialBuildingFire;
    }

    [HarmonyPatch(typeof(OfficeBuildingAI), "GetFireParameters")]
    internal static class OfficeBuildingAIPatch {
        //public static void Postfix(ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) {
        //        fireTolerance = 10;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveOfficeBuildingFire;
    }

    [HarmonyPatch(typeof(IndustrialExtractorAI), "GetFireParameters")]
    internal static class IndustrialExtractorAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveIndustrialBuildingFire;
    }

    [HarmonyPatch(typeof(ParkBuildingAI), "GetFireParameters")]
    internal static class ParkBuildingAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) {
        //        fireTolerance = 20;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveParkBuildingFire;
    }

    [HarmonyPatch(typeof(MuseumAI), "GetFireParameters")]
    internal static class MuseumAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) {
        //        fireTolerance = 20;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveMuseumFire;
    }

    [HarmonyPatch(typeof(DummyBuildingAI), "GetFireParameters")]
    internal static class DummyBuildingAIPatch {
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemovePlayerBuildingFire;
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    }
        //}
    }

    [HarmonyPatch(typeof(CampusBuildingAI), "GetFireParameters")]
    internal static class CampusBuildingAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) {
        //        fireTolerance = 20;
        //    }
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveCampusBuildingFire;
    }

    [HarmonyPatch(typeof(AirportGateAI), "GetFireParameters")]
    internal static class AirportGateAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveAirportBuildingFire;
        
    }

    [HarmonyPatch(typeof(AirportCargoGateAI), "GetFireParameters")]
    internal static class AirportCargoGateAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveAirportBuildingFire;
    }

    [HarmonyPatch(typeof(AirportBuildingAI), "GetFireParameters")]
    internal static class AirportBuildingAIPatch {
        //public static void Postfix(ref int fireHazard, ref int fireTolerance) {
        //    if (Config.Instance.RemoveFire) {
        //        fireHazard = 0;
        //        fireTolerance = 1000;
        //    } else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        //}
        public static void Postfix(ref bool __result) => __result = !Config.Instance.RemoveAirportBuildingFire;
    }

}
