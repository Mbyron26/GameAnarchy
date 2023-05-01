namespace GameAnarchy.Patches;
using ColossalFramework;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(BuildingAI))]
public static class BuildingAIPatch {
    [HarmonyPatch(nameof(BuildingAI.GetRefundAmount))]
    [HarmonyPrefix]
    public static bool Prefix0(BuildingAI __instance, ref int __result) {
        if (Config.Instance.BuildingRefund) {
            var constructionCost = __instance.GetConstructionCost();
            var result = (int)(constructionCost * Config.Instance.BuildingRefundMultipleFactor);
            __result = result;
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(BuildingAI.GetRelocationCost))]
    [HarmonyPrefix]
    public static bool Prefix1(BuildingAI __instance, ref int __result) {
        if (Config.Instance.BuildingRelocationCostFactor != 0.2) {
            var constructionCost = __instance.GetConstructionCost();
            var result = (int)(constructionCost * Config.Instance.BuildingRelocationCostFactor);
            __result = result;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(BulldozeTool))]
public static class BulldozeToolPatch {
    [HarmonyPatch("GetBuildingRefundAmount")]
    [HarmonyPrefix]
    public static bool Prefix0(ushort building, ref int __result) {
        if (Config.Instance.BuildingRefund && Config.Instance.RemoveBuildingRefundTimeLimitation) {
            var b = Singleton<BuildingManager>.instance.m_buildings.m_buffer[building];
            __result = b.Info.m_buildingAI.GetRefundAmount(building, ref b);
            return false;
        }
        return true;
    }

    [HarmonyPatch("GetSegmentRefundAmount")]
    [HarmonyPrefix]
    public static bool Prefix1(ushort segment, ref int __result) {
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




