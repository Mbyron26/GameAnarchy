namespace GameAnarchy;
using ICities;

public class MilestonesExtension : MilestonesExtensionBase {
    public override void OnCreated(IMilestones milestones) {
        base.OnCreated(milestones);
        Mod.Log.Info("Call milestones extension OnCreated");
    }

    public override void OnReleased() => Mod.Log.Info("Call milestones extension OnReleased");

    public override void OnRefreshMilestones() {
        SingletonManager<Manager>.Instance.UnlockAll(managers, milestonesManager);
        SingletonManager<Manager>.Instance.CustomUnlock(milestonesManager);
    }

    public override int OnGetPopulationTarget(int originalTarget, int scaledTarget) => Config.Instance.EnabledUnlockAll ? 0 : base.OnGetPopulationTarget(originalTarget, scaledTarget);
}