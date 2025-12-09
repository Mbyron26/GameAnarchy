using ColossalFramework;
using CSLModsCommon.Manager;
using CSLModsCommon.Patch;
using GameAnarchy.ModSettings;
using UnityEngine;

namespace GameAnarchy.Patches;

public class BuildingAIPatch {
    private static ModSetting _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();

    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.ApplyPrefix<BuildingAI, BuildingAIPatch>(nameof(BuildingAI.GetRefundAmount), nameof(GetRefundAmountPrefix));
        harmonyPatcher.ApplyPrefix<BuildingAI, BuildingAIPatch>(nameof(BuildingAI.GetRelocationCost), nameof(GetRelocationCostPrefix));
    }

    public static bool GetRefundAmountPrefix(BuildingAI __instance, ref int __result) {
        if (!_modSetting.BuildingRefund) return true;
        var constructionCost = __instance.GetConstructionCost();
        var result = (int)(constructionCost * _modSetting.BuildingRefundMultipleFactor);
        __result = result;
        return false;
    }

    public static bool GetRelocationCostPrefix(BuildingAI __instance, ref int __result) {
        if (Mathf.Approximately(_modSetting.BuildingRelocationCostFactor, 0.2f)) {
            var constructionCost = __instance.GetConstructionCost();
            var result = (int)(constructionCost * _modSetting.BuildingRelocationCostFactor);
            __result = result;
            return false;
        }

        return true;
    }
}

public class BulldozeToolPatch {
    private static ModSetting _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();

    public static void Patch(HarmonyPatcher harmonyPatcher) {
        harmonyPatcher.ApplyPrefix<BulldozeTool, BulldozeToolPatch>("GetBuildingRefundAmount", nameof(GetBuildingRefundAmountPrefix));
        harmonyPatcher.ApplyPrefix<BulldozeTool, BulldozeToolPatch>("GetSegmentRefundAmount", nameof(GetSegmentRefundAmountPrefix));
    }

    public static bool GetBuildingRefundAmountPrefix(ushort building, ref int __result) {
        if (_modSetting.BuildingRefund && _modSetting.RemoveBuildingRefundTimeLimitation) {
            var b = Singleton<BuildingManager>.instance.m_buildings.m_buffer[building];
            __result = b.Info.m_buildingAI.GetRefundAmount(building, ref b);
            return false;
        }

        return true;
    }

    public static bool GetSegmentRefundAmountPrefix(ushort segment, ref int __result) {
        if (!_modSetting.SegmentRefund) return true;
        var instance = Singleton<NetManager>.instance;
        var info = instance.m_segments.m_buffer[segment].Info;
        var startNode = instance.m_segments.m_buffer[segment].m_startNode;
        var endNode = instance.m_segments.m_buffer[segment].m_endNode;
        var position = instance.m_nodes.m_buffer[startNode].m_position;
        var position2 = instance.m_nodes.m_buffer[endNode].m_position;
        float num = instance.m_nodes.m_buffer[startNode].m_elevation;
        float num2 = instance.m_nodes.m_buffer[endNode].m_elevation;
        if (info.m_netAI.IsUnderground()) {
            num = -num;
            num2 = -num2;
        }

        var result = (int)(info.m_netAI.GetConstructionCost(position, position2, num, num2) * _modSetting.SegmentRefundMultipleFactor);
        if (_modSetting.RemoveSegmentRefundTimeLimitation)
            __result = result;
        else
            __result = Singleton<SimulationManager>.instance.IsRecentBuildIndex(instance.m_segments.m_buffer[segment].m_buildIndex) ? result : 0;

        return false;
    }
}