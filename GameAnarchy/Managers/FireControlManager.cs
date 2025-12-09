using ColossalFramework;
using CSLModsCommon.Manager;
using CSLModsCommon.UI.Dialogs;
using GameAnarchy.Localization;

namespace GameAnarchy.Managers;

public class FireControlManager : ManagerBase {
    public uint _buildingFireSpreadCount;
    public uint _buildingFireSpreadAllowed;
    public uint _treeFireSpreadCount;
    public uint _treeFireSpreadAllowed;
    private DialogManager _dialogManager;

    protected override void OnCreate() {
        base.OnCreate();
        _dialogManager = Domain.GetOrCreateManager<DialogManager>();
    }

    protected override void OnGameUnloaded() {
        base.OnGameUnloaded();
        OutputFireSpreadCount();
    }

    public void PutOutBurningBuildingsButtonClicked() => _dialogManager.Show<ConfirmDialog>().AddContent(Translations.PutOutBurningBuildings, Translations.PutOutBurningBuildingsDescription, PutOutBurningBuildings);

    private void PutOutBurningBuildings() {
        if (!LoadingManager.exists || !LoadingManager.instance.m_loadingComplete) return;
        if (!Singleton<BuildingManager>.exists) {
            Logger.Error("BuildingManager does not exist, unable to call PutOutBurningBuildings");
            return;
        }

        var buffer = Singleton<BuildingManager>.instance.m_buildings.m_buffer;
        for (var i = 0; i < buffer.Length; i++) {
            var temp = buffer[i].m_fireIntensity;
            if (buffer[i].m_fireIntensity == 0) continue;

            buffer[i].m_fireIntensity = 0;
            Logger.Debug($"Put out burning buildings, ID: {buffer[i].m_buildIndex}, raw fireIntensity: {temp}");
        }
    }

    public void OutputFireSpreadCount() => Logger.Debug($"Building fire spread count: {_buildingFireSpreadCount}, building fire spread allowed: {_buildingFireSpreadAllowed}, tree fire spread count: {_treeFireSpreadCount}, tree fire spread allowed: {_treeFireSpreadAllowed}");

    public bool GetFireProbability(uint probability, ref uint count, ref uint allowed) {
        if (Singleton<SimulationManager>.exists) {
            var randomValue = Singleton<SimulationManager>.instance.m_randomizer.UInt32(1, 100);
            count++;
            if (randomValue < probability) allowed++;

            return randomValue <= probability;
        }

        Logger.Error("SimulationManager does not exist.");
        return false;
    }
}