namespace GameAnarchy.Patches;
using HarmonyLib;
using System.Reflection;

public static class RemoveFirePatch {
    public static MethodInfo GetOriginalCommonBuildingAITrySpreadFire() => AccessTools.Method(typeof(CommonBuildingAI), "TrySpreadFire");
    public static MethodInfo GetCommonBuildingAITrySpreadFirePrefix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.CommonBuildingAITrySpreadFirePrefix));
    public static MethodInfo GetOriginalTreeManagerTrySpreadFire() => AccessTools.Method(typeof(TreeManager), "TrySpreadFire");
    public static MethodInfo GetTreeManagerTrySpreadFirePrefix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.TreeManagerTrySpreadFirePrefix));
    public static MethodInfo GetOriginalTreeManagerBurnTree() => AccessTools.Method(typeof(TreeManager), "BurnTree");
    public static MethodInfo GetTreeManagerBurnTreePrefix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.TreeManagerBurnTreePrefix));
    public static MethodInfo GetOriginalPlayerBuildingAIGetFireParameters() => AccessTools.Method(typeof(PlayerBuildingAI), "GetFireParameters");
    public static MethodInfo GetPlayerBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.PlayerBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalResidentialBuildingAIGetFireParameters() => AccessTools.Method(typeof(ResidentialBuildingAI), "GetFireParameters");
    public static MethodInfo GetResidentialBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.ResidentialBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalIndustrialBuildingAIGetFireParameters() => AccessTools.Method(typeof(IndustrialBuildingAI), "GetFireParameters");
    public static MethodInfo GetIndustrialBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.IndustrialBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalCommercialBuildingAIGetFireParameters() => AccessTools.Method(typeof(CommercialBuildingAI), "GetFireParameters");
    public static MethodInfo GetCommercialBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.CommercialBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalOfficeBuildingAIGetFireParameters() => AccessTools.Method(typeof(OfficeBuildingAI), "GetFireParameters");
    public static MethodInfo GetOfficeBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.OfficeBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalIndustrialExtractorAIGetFireParameters() => AccessTools.Method(typeof(IndustrialExtractorAI), "GetFireParameters");
    public static MethodInfo GetIndustrialExtractorAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.IndustrialExtractorAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalParkBuildingAIGetFireParameters() => AccessTools.Method(typeof(ParkBuildingAI), "GetFireParameters");
    public static MethodInfo GetParkBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.ParkBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalMuseumAIGetFireParameters() => AccessTools.Method(typeof(MuseumAI), "GetFireParameters");
    public static MethodInfo GetMuseumAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.MuseumAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalDummyBuildingAIGetFireParameters() => AccessTools.Method(typeof(DummyBuildingAI), "GetFireParameters");
    public static MethodInfo GetDummyBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.DummyBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalCampusBuildingAIGetFireParameters() => AccessTools.Method(typeof(CampusBuildingAI), "GetFireParameters");
    public static MethodInfo GetCampusBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.CampusBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalAirportGateAIGetFireParameters() => AccessTools.Method(typeof(AirportGateAI), "GetFireParameters");
    public static MethodInfo GetAirportGateAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.AirportGateAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalAirportCargoGateAIGetFireParameters() => AccessTools.Method(typeof(AirportCargoGateAI), "GetFireParameters");
    public static MethodInfo GetAirportCargoGateAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.AirportCargoGateAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalAirportBuildingAIGetFireParameters() => AccessTools.Method(typeof(AirportBuildingAI), "GetFireParameters");
    public static MethodInfo GetAirportBuildingAIGetFireParametersPostfix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.AirportBuildingAIGetFireParametersPostfix));
    public static MethodInfo GetOriginalCommonBuildingAIHandleFire() => AccessTools.Method(typeof(CommonBuildingAI), "HandleFire");
    public static MethodInfo GetHandleFirePrefix() => AccessTools.Method(typeof(RemoveFirePatch), nameof(RemoveFirePatch.HandleFirePrefix));

    public static bool CommonBuildingAITrySpreadFirePrefix() => SingletonManager<Manager>.Instance.GetFireProbability(Config.Instance.BuildingSpreadFireProbability, ref SingletonManager<Manager>.Instance.buildingFireSpreadCount, ref SingletonManager<Manager>.Instance.buildingFireSpreadAllowed);
    public static bool TreeManagerTrySpreadFirePrefix() => SingletonManager<Manager>.Instance.GetFireProbability(Config.Instance.BuildingSpreadFireProbability, ref SingletonManager<Manager>.Instance.buildingFireSpreadCount, ref SingletonManager<Manager>.Instance.buildingFireSpreadAllowed);
    public static bool TreeManagerBurnTreePrefix() => SingletonManager<Manager>.Instance.GetFireProbability(Config.Instance.TreeSpreadFireProbability, ref SingletonManager<Manager>.Instance.treeFireSpreadCount, ref SingletonManager<Manager>.Instance.treeFireSpreadAllowed);
    public static void PlayerBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemovePlayerBuildingFire;
    public static void ResidentialBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveResidentialBuildingFire;
    public static void IndustrialBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveIndustrialBuildingFire;
    public static void CommercialBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveCommercialBuildingFire;
    public static void OfficeBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveOfficeBuildingFire;
    public static void IndustrialExtractorAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveIndustrialBuildingFire;
    public static void ParkBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveParkBuildingFire;
    public static void MuseumAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveMuseumFire;
    public static void DummyBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemovePlayerBuildingFire;
    public static void CampusBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveCampusBuildingFire;
    public static void AirportGateAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveAirportBuildingFire;
    public static void AirportCargoGateAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveAirportBuildingFire;
    public static void AirportBuildingAIGetFireParametersPostfix(ref bool __result) => __result = !Config.Instance.RemoveAirportBuildingFire;
    public static bool HandleFirePrefix() {
        if (Config.Instance.RemovePlayerBuildingFire || Config.Instance.RemoveResidentialBuildingFire || Config.Instance.RemoveIndustrialBuildingFire || Config.Instance.RemoveCommercialBuildingFire || Config.Instance.RemoveOfficeBuildingFire || Config.Instance.RemoveParkBuildingFire || Config.Instance.RemoveMuseumFire || Config.Instance.RemoveCampusBuildingFire || Config.Instance.RemoveAirportBuildingFire) {
            return false;
        }
        return true;
    }
}