using GameAnarchy.UI;
using MbyronModsCommon;

namespace GameAnarchy {
    public class ThreadExtension : ModThreadExtensionBase {
        private bool addCashFlag;
        private bool toggleControlPanel;
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
            base.OnUpdate(realTimeDelta, simulationTimeDelta);

            AddCallOnceInvoke(Config.Instance.AddCash.IsPressed(), ref addCashFlag, EconomyExtension.AddMoneyManually);
            AddCallOnceInvoke(Config.Instance.ControlPanelHotkey.IsPressed(), ref toggleControlPanel, ControlPanelManager.HotkeyToggle);
        }

    }
}
