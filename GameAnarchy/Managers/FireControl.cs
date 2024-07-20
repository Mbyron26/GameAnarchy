namespace GameAnarchy;
using ColossalFramework;

public partial class Manager {
    public uint buildingFireSpreadCount;
    public uint buildingFireSpreadAllowed;
    public uint treeFireSpreadCount;
    public uint treeFireSpreadAllowed;

    public void PutOutBuringBuildingsButtonClicked() => MessageBox.Show<TwoButtonMessageBox>().Init(Localize.PutOutBurningBuildings, Localize.PutOutBurningBuildingsDescription, PutOutBuringBuildings);

    public void PutOutBuringBuildings() {
        if (!Singleton<BuildingManager>.exists) {
            Mod.Log.Error("BuildingManager does not exist, unable to call PutOutBuringBuildings");
            return;
        }
        var buffer = Singleton<BuildingManager>.instance.m_buildings.m_buffer;
        for (int i = 0; i < buffer.Length; i++) {
            var temp = buffer[i].m_fireIntensity;
            if (buffer[i].m_fireIntensity == 0) {
                continue;
            }
            buffer[i].m_fireIntensity = 0;
            Mod.Log.Debug($"Put out buring buildings, ID: {buffer[i].m_buildIndex}, raw fireIntensity: {temp}");
        }
    }

    public void OutputFireSpreadCount() => Mod.Log.Debug($"Building fire spread count: {buildingFireSpreadCount}, building fire spread allowed: {buildingFireSpreadAllowed}, tree fire spread count: {treeFireSpreadCount}, tree fire spread allowed: {treeFireSpreadAllowed}");

    public bool GetFireProbability(uint probaility, ref uint count, ref uint allowed) {
        if (Singleton<SimulationManager>.exists) {
            var randomValue = Singleton<SimulationManager>.instance.m_randomizer.UInt32(1, 100);
            count++;
            if (randomValue < probaility) {
                allowed++;
            }
            return randomValue <= probaility;
        }
        Mod.Log.Error("SimulationManager does not exist.");
        return false;
    }
}
