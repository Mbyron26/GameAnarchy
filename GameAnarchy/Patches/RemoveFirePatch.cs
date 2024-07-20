namespace GameAnarchy.Patches;
using HarmonyLib;

public static class RemoveFirePatch {
    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(CommonBuildingAI), "TrySpreadFire"), AccessTools.Method(typeof(RemoveFirePatch), nameof(CommonBuildingAITrySpreadFirePrefix)));
        harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(TreeManager), "TrySpreadFire"), AccessTools.Method(typeof(RemoveFirePatch), nameof(TreeManagerTrySpreadFirePrefix)));
        harmonyPatcher.PrefixPatching(AccessTools.Method(typeof(TreeManager), "BurnTree"), AccessTools.Method(typeof(RemoveFirePatch), nameof(TreeManagerBurnTreePrefix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(PlayerBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(PlayerBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(ResidentialBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(ResidentialBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(IndustrialBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(IndustrialBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(CommercialBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(CommercialBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(OfficeBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(OfficeBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(IndustrialExtractorAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(IndustrialExtractorAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(ParkBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(ParkBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(MuseumAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(MuseumAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(DummyBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(DummyBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(CampusBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(CampusBuildingAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(AirportGateAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(AirportGateAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(AirportCargoGateAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(AirportCargoGateAIGetFireParametersPostfix)));
        harmonyPatcher.PostfixPatching(AccessTools.Method(typeof(AirportBuildingAI), "GetFireParameters"), AccessTools.Method(typeof(RemoveFirePatch), nameof(AirportBuildingAIGetFireParametersPostfix)));
    }

    public static bool CommonBuildingAITrySpreadFirePrefix() => SingletonManager<Manager>.Instance.GetFireProbability(Config.Instance.BuildingSpreadFireProbability, ref SingletonManager<Manager>.Instance.buildingFireSpreadCount, ref SingletonManager<Manager>.Instance.buildingFireSpreadAllowed);
    public static bool TreeManagerTrySpreadFirePrefix() => SingletonManager<Manager>.Instance.GetFireProbability(Config.Instance.BuildingSpreadFireProbability, ref SingletonManager<Manager>.Instance.buildingFireSpreadCount, ref SingletonManager<Manager>.Instance.buildingFireSpreadAllowed);
    public static bool TreeManagerBurnTreePrefix() => SingletonManager<Manager>.Instance.GetFireProbability(Config.Instance.TreeSpreadFireProbability, ref SingletonManager<Manager>.Instance.treeFireSpreadCount, ref SingletonManager<Manager>.Instance.treeFireSpreadAllowed);

    public static void PlayerBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemovePlayerBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemovePlayerBuildingFire;
    }

    public static void ResidentialBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveResidentialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveResidentialBuildingFire;
    }

    public static void IndustrialBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveIndustrialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveIndustrialBuildingFire;
    }

    public static void CommercialBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveCommercialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveCommercialBuildingFire;
    }

    public static void OfficeBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveOfficeBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveOfficeBuildingFire;
    }

    public static void IndustrialExtractorAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveIndustrialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveIndustrialBuildingFire;
    }

    public static void ParkBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveParkBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveParkBuildingFire;
    }

    public static void MuseumAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveMuseumFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveMuseumFire;
    }

    public static void DummyBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemovePlayerBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemovePlayerBuildingFire;
    }

    public static void CampusBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveCampusBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveCampusBuildingFire;
    }

    public static void AirportGateAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveAirportBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveAirportBuildingFire;
    }

    public static void AirportCargoGateAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveAirportBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveAirportBuildingFire;
    }

    public static void AirportBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (Config.Instance.RemoveAirportBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }
        __result = !Config.Instance.RemoveAirportBuildingFire;
    }

}