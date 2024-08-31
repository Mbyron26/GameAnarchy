namespace GameAnarchy;

using CSShared.Debug;
using CSShared.Manager;
using GameAnarchy.Managers;
using ICities;

public class MilestonesExtension : MilestonesExtensionBase {
    public override void OnCreated(IMilestones milestones) {
        base.OnCreated(milestones);
        LogManager.GetLogger().Info("Call milestones extension OnCreated");
    }

    public override void OnReleased() => LogManager.GetLogger().Info("Call milestones extension OnReleased");

    public override void OnRefreshMilestones() {
        ManagerPool.GetOrCreateManager<Manager>().UnlockAll(managers, milestonesManager);
        ManagerPool.GetOrCreateManager<Manager>().CustomUnlock(milestonesManager);
    }

    public override int OnGetPopulationTarget(int originalTarget, int scaledTarget) => Config.Instance.EnabledUnlockAll ? 0 : base.OnGetPopulationTarget(originalTarget, scaledTarget);
}