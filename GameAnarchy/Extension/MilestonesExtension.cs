using CSLModsCommon.Logging;
using CSLModsCommon.Manager;
using GameAnarchy.Data;
using GameAnarchy.Managers;
using GameAnarchy.ModSettings;
using ICities;

namespace GameAnarchy.Extension;

public class MilestonesExtension : MilestonesExtensionBase {
    private ModSetting _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();

    public override void OnCreated(IMilestones milestones) {
        base.OnCreated(milestones);
        LogManager.GetLogger().Info("Call milestones extension OnCreated");
    }

    public override void OnReleased() => LogManager.GetLogger().Info("Call milestones extension OnReleased");

    public override void OnRefreshMilestones() {
        Domain.DefaultDomain.GetOrCreateManager<ModUnlockManager>().UnlockAll(managers, milestonesManager);
        Domain.DefaultDomain.GetOrCreateManager<ModUnlockManager>().CustomUnlock(milestonesManager);
    }

    public override int OnGetPopulationTarget(int originalTarget, int scaledTarget) => _modSetting.CurrentUnlockMode == UnlockMode.UnlockAll ? 0 : base.OnGetPopulationTarget(originalTarget, scaledTarget);
}