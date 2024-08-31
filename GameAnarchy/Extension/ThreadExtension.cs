using CSShared.Extension;
using CSShared.Manager;
using CSShared.UI.ControlPanel;
using GameAnarchy.Managers;
using GameAnarchy.UI;
using ICities;

namespace GameAnarchy.Extension;

public class ThreadExtension : ModThreadExtensionBase {
    private bool addCashFlag;
    private bool substrateMoneyFlag;
    private bool toggleControlPanel;
    private byte frameIndex;

    public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
        base.OnUpdate(realTimeDelta, simulationTimeDelta);

        AddCallOnceInvoke(Config.Instance.AddCash.IsPressed(), ref addCashFlag, ManagerPool.GetOrCreateManager<Manager>().AddMoneyManually);
        AddCallOnceInvoke(Config.Instance.DecreaseMoney.IsPressed(), ref substrateMoneyFlag, ManagerPool.GetOrCreateManager<Manager>().SubstrateMoneyManually);
        AddCallOnceInvoke(Config.Instance.ControlPanelHotkey.IsPressed(), ref toggleControlPanel, ControlPanelManager<Mod, ControlPanel>.CallPanel);
    }

    public override void OnBeforeSimulationFrame() {
        ManagerPool.GetOrCreateManager<Manager>().ChargeInterest();
        base.OnBeforeSimulationFrame();
    }

    public override void OnAfterSimulationFrame() {
        if (!(managers.loading is not null && managers.loading.loadingComplete && (managers.loading.currentMode == AppMode.Game)) || SimulationManager.instance.SimulationPaused)
            return;
        frameIndex = (byte)(SimulationManager.instance.m_currentFrameIndex & 15);
        if (Manager.FrameHander.Length <= frameIndex)
            return;
        Manager.FrameHander[frameIndex].Invoke();
    }
}
