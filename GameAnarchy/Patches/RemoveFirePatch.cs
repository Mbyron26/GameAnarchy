using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using GameAnarchy.Managers;
using GameAnarchy.ModSettings;

namespace GameAnarchy.Patches;

public class RemoveFirePatch {
    private static ModSetting _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();

    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.ApplyPrefix<CommonBuildingAI, RemoveFirePatch>("TrySpreadFire", nameof(CommonBuildingAITrySpreadFirePrefix));
        harmonyPatcher.ApplyPrefix<TreeManager, RemoveFirePatch>("TrySpreadFire", nameof(TreeManagerTrySpreadFirePrefix));
        harmonyPatcher.ApplyPrefix<TreeManager, RemoveFirePatch>("BurnTree", nameof(TreeManagerBurnTreePrefix));
        harmonyPatcher.ApplyPostfix<PlayerBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(PlayerBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<ResidentialBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(ResidentialBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<IndustrialBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(IndustrialBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<CommercialBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(CommercialBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<OfficeBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(OfficeBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<IndustrialExtractorAI, RemoveFirePatch>("GetFireParameters", nameof(IndustrialExtractorAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<ParkBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(ParkBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<MuseumAI, RemoveFirePatch>("GetFireParameters", nameof(MuseumAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<DummyBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(DummyBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<CampusBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(CampusBuildingAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<AirportGateAI, RemoveFirePatch>("GetFireParameters", nameof(AirportGateAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<AirportCargoGateAI, RemoveFirePatch>("GetFireParameters", nameof(AirportCargoGateAIGetFireParametersPostfix));
        harmonyPatcher.ApplyPostfix<AirportBuildingAI, RemoveFirePatch>("GetFireParameters", nameof(AirportBuildingAIGetFireParametersPostfix));
    }

    public static bool CommonBuildingAITrySpreadFirePrefix() => Domain.DefaultDomain.GetOrCreateManager<FireControlManager>().GetFireProbability(_modSetting.BuildingSpreadFireProbability, ref Domain.DefaultDomain.GetOrCreateManager<FireControlManager>()._buildingFireSpreadCount, ref Domain.DefaultDomain.GetOrCreateManager<FireControlManager>()._buildingFireSpreadAllowed);

    public static bool TreeManagerTrySpreadFirePrefix() => Domain.DefaultDomain.GetOrCreateManager<FireControlManager>().GetFireProbability(_modSetting.BuildingSpreadFireProbability, ref Domain.DefaultDomain.GetOrCreateManager<FireControlManager>()._buildingFireSpreadCount, ref Domain.DefaultDomain.GetOrCreateManager<FireControlManager>()._buildingFireSpreadAllowed);

    public static bool TreeManagerBurnTreePrefix() => Domain.DefaultDomain.GetOrCreateManager<FireControlManager>().GetFireProbability(_modSetting.TreeSpreadFireProbability, ref Domain.DefaultDomain.GetOrCreateManager<FireControlManager>()._treeFireSpreadCount, ref Domain.DefaultDomain.GetOrCreateManager<FireControlManager>()._treeFireSpreadAllowed);

    public static void PlayerBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemovePlayerBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemovePlayerBuildingFire;
    }

    public static void ResidentialBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveResidentialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveResidentialBuildingFire;
    }

    public static void IndustrialBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveIndustrialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveIndustrialBuildingFire;
    }

    public static void CommercialBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveCommercialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveCommercialBuildingFire;
    }

    public static void OfficeBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveOfficeBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveOfficeBuildingFire;
    }

    public static void IndustrialExtractorAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveIndustrialBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveIndustrialBuildingFire;
    }

    public static void ParkBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveParkBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveParkBuildingFire;
    }

    public static void MuseumAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveMuseumFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveMuseumFire;
    }

    public static void DummyBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemovePlayerBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemovePlayerBuildingFire;
    }

    public static void CampusBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveCampusBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveCampusBuildingFire;
    }

    public static void AirportGateAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveAirportBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveAirportBuildingFire;
    }

    public static void AirportCargoGateAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveAirportBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveAirportBuildingFire;
    }

    public static void AirportBuildingAIGetFireParametersPostfix(ref int fireHazard, ref int fireSize, ref int fireTolerance, ref bool __result) {
        if (_modSetting.RemoveAirportBuildingFire) {
            fireHazard = 0;
            fireSize = 0;
            fireTolerance = 0;
        }

        __result = !_modSetting.RemoveAirportBuildingFire;
    }
}