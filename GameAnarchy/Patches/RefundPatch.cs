namespace GameAnarchy.Patches;
using ColossalFramework;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

public static class BuildingAIPatch {
    public static MethodInfo GetOriginalGetRefundAmount() => AccessTools.Method(typeof(BuildingAI), nameof(BuildingAI.GetRefundAmount));
    public static MethodInfo GetGetRefundAmountPrefix() => AccessTools.Method(typeof(BuildingAIPatch), nameof(BuildingAIPatch.GetRefundAmountPrefix));
    public static MethodInfo GetOriginalGetRelocationCost() => AccessTools.Method(typeof(BuildingAI), nameof(BuildingAI.GetRelocationCost));
    public static MethodInfo GetGetRelocationCostPrefix() => AccessTools.Method(typeof(BuildingAIPatch), nameof(BuildingAIPatch.GetRelocationCostPrefix));
    public static bool GetRefundAmountPrefix(BuildingAI __instance, ref int __result) {
        if (Config.Instance.BuildingRefund) {
            var constructionCost = __instance.GetConstructionCost();
            var result = (int)(constructionCost * Config.Instance.BuildingRefundMultipleFactor);
            __result = result;
            return false;
        }
        return true;
    }
    public static bool GetRelocationCostPrefix(BuildingAI __instance, ref int __result) {
        if (Config.Instance.BuildingRelocationCostFactor != 0.2) {
            var constructionCost = __instance.GetConstructionCost();
            var result = (int)(constructionCost * Config.Instance.BuildingRelocationCostFactor);
            __result = result;
            return false;
        }
        return true;
    }
}

public static class BulldozeToolPatch {
    public static MethodInfo GetOriginalGetBuildingRefundAmount() => AccessTools.Method(typeof(BulldozeTool), "GetBuildingRefundAmount");
    public static MethodInfo GetGetBuildingRefundAmountPrefix() => AccessTools.Method(typeof(BulldozeToolPatch), nameof(BulldozeToolPatch.GetBuildingRefundAmountPrefix));
    public static MethodInfo GetOriginalGetSegmentRefundAmount() => AccessTools.Method(typeof(BulldozeTool), "GetSegmentRefundAmount");
    public static MethodInfo GetGetSegmentRefundAmountPrefix() => AccessTools.Method(typeof(BulldozeToolPatch), nameof(BulldozeToolPatch.GetSegmentRefundAmountPrefix));
    public static bool GetBuildingRefundAmountPrefix(ushort building, ref int __result) {
        if (Config.Instance.BuildingRefund && Config.Instance.RemoveBuildingRefundTimeLimitation) {
            var b = Singleton<BuildingManager>.instance.m_buildings.m_buffer[building];
            __result = b.Info.m_buildingAI.GetRefundAmount(building, ref b);
            return false;
        }
        return true;
    }
    public static bool GetSegmentRefundAmountPrefix(ushort segment, ref int __result) {
        if (Config.Instance.SegmentRefund) {
            NetManager instance = Singleton<NetManager>.instance;
            NetInfo info = instance.m_segments.m_buffer[segment].Info;
            ushort startNode = instance.m_segments.m_buffer[segment].m_startNode;
            ushort endNode = instance.m_segments.m_buffer[segment].m_endNode;
            Vector3 position = instance.m_nodes.m_buffer[startNode].m_position;
            Vector3 position2 = instance.m_nodes.m_buffer[endNode].m_position;
            float num = instance.m_nodes.m_buffer[startNode].m_elevation;
            float num2 = instance.m_nodes.m_buffer[endNode].m_elevation;
            if (info.m_netAI.IsUnderground()) {
                num = -num;
                num2 = -num2;
            }
            var reslut = (int)(info.m_netAI.GetConstructionCost(position, position2, num, num2) * Config.Instance.SegmentRefundMultipleFactor);
            if (Config.Instance.RemoveSegmentRefundTimeLimitation) {
                __result = reslut;
            } else {
                __result = (Singleton<SimulationManager>.instance.IsRecentBuildIndex(instance.m_segments.m_buffer[segment].m_buildIndex)) ? reslut : 0;
            }
            return false;
        }
        return true;
    }
}