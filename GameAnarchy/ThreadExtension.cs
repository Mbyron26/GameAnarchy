namespace GameAnarchy;
using GameAnarchy.UI;

public class ThreadExtension : ModThreadExtensionBase {
    private bool addCashFlag;
    private bool toggleControlPanel;
    public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
        base.OnUpdate(realTimeDelta, simulationTimeDelta);

        AddCallOnceInvoke(Config.Instance.AddCash.IsPressed(), ref addCashFlag, SingletonManager<Manager>.Instance.AddMoneyManually);
        AddCallOnceInvoke(Config.Instance.ControlPanelHotkey.IsPressed(), ref toggleControlPanel, ControlPanelManager<Mod, ControlPanel>.CallPanel);
    }

    public override void OnBeforeSimulationFrame() {
        SingletonManager<Manager>.Instance.ChargeInterest();
        base.OnBeforeSimulationFrame();
    }

}
