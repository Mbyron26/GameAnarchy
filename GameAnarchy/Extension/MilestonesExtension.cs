using CSLModsCommon.Logging;
using CSLModsCommon.Manager;
using GameAnarchy.Data;
using GameAnarchy.Managers;
using GameAnarchy.ModSettings;
using ICities;

namespace GameAnarchy.Extension;

public class MilestonesExtension : MilestonesExtensionBase {
    private ModSetting _modSetting;
    private ModUnlockManager _modUnlockManager;

    public override void OnCreated(IMilestones milestones) {
        base.OnCreated(milestones);
        var domain = Domain.DefaultDomain;
        _modSetting = domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
        _modUnlockManager = domain.GetOrCreateManager<ModUnlockManager>();
        LogManager.GetLogger().Info("Call milestones extension OnCreated");
    }

    public override void OnReleased() => LogManager.GetLogger().Info("Call milestones extension OnReleased");

    public override void OnRefreshMilestones() {
        _modUnlockManager.UnlockAll(managers, milestonesManager);
        _modUnlockManager.CustomUnlock(milestonesManager);
    }

    public override int OnGetPopulationTarget(int originalTarget, int scaledTarget) => _modSetting.CurrentUnlockMode is UnlockMode.UnlockAll ? 0 : base.OnGetPopulationTarget(originalTarget, scaledTarget);
}