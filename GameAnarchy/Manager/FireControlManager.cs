namespace GameAnarchy.Manager;
using ColossalFramework;

internal class FireControlManager {
    public static uint buildingFireSpreadCount;
    public static uint buildingFireSpreadAllowed;
    public static uint treeFireSpreadCount;
    public static uint treeFireSpreadAllowed;

    public static bool GetFireProbability(uint probaility, ref uint count, ref uint allowed) {
        if (Singleton<SimulationManager>.exists) {
            var randomValue = Singleton<SimulationManager>.instance.m_randomizer.UInt32(1, 100);
            count++;
            if (randomValue < probaility) {
                allowed++;
            }
            return randomValue <= probaility;
        }
        ExternalLogger.Error("SimulationManager does not exist.");
        return false;
    }
}
