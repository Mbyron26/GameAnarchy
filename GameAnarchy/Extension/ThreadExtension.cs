namespace GameAnarchy;
using GameAnarchy.UI;
using ICities;

public class ThreadExtension : ModThreadExtensionBase {
    private bool addCashFlag;
    private bool toggleControlPanel;
    private byte frameIndex;

    public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
        base.OnUpdate(realTimeDelta, simulationTimeDelta);

        AddCallOnceInvoke(Config.Instance.AddCash.IsPressed(), ref addCashFlag, SingletonManager<Manager>.Instance.AddMoneyManually);
        AddCallOnceInvoke(Config.Instance.ControlPanelHotkey.IsPressed(), ref toggleControlPanel, ControlPanelManager<Mod, ControlPanel>.CallPanel);
    }

    public override void OnBeforeSimulationFrame() {
        SingletonManager<Manager>.Instance.ChargeInterest();
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
