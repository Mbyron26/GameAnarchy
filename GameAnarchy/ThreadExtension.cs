using GameAnarchy.UI;
using MbyronModsCommon;

namespace GameAnarchy {
    public class ThreadExtension : ModThreadExtensionBase {
        private bool addCashFlag;
        private bool showControlPanel;
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) {
            base.OnUpdate(realTimeDelta, simulationTimeDelta);

            AddCallOnceInvoke(Config.Instance.AddCash.IsPressed(), ref addCashFlag, EconomyExtension.ManuallyAddCash);
            AddCallOnceInvoke(Config.Instance.ShowControlPanelHotkey.IsPressed(), ref showControlPanel, ControlPanelManager.HotkeyToggle);
        }

    }
}
