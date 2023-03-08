using ColossalFramework.UI;
using MbyronModsCommon;
using UnityEngine;

namespace GameAnarchy.UI {
    internal class ControlPanelManager {
        private static GameObject PanelGameObject { get; set; }
        public static ControlPanel Panel { get; private set; }
        public static bool IsVisible { get; private set; }

        public static void HotkeyToggle() {
            if (IsVisible) {
                Close();
            } else {
#if DEBUG
                DebugUtils.TimeCalculater(Create, tag: "Control Panel Invoke: ");
#else
                Create();
#endif
            }
        }

        public static void OnLocaleChanged() {
            if (IsVisible) {
                Close();
                Create();
            }
        }

        public static void RefreshPanel() {

        }

        public static void Create() {
            if (PanelGameObject is null) {
                PanelGameObject = new GameObject("GameAnarchyControlPanel");
                PanelGameObject.transform.parent = UIView.GetAView().transform;
                Panel = PanelGameObject.AddComponent<ControlPanel>();
                Panel.Show();
                IsVisible = true;
            }
        }
        public static void Close() {
            if (PanelGameObject is not null) {
                UnityEngine.Object.Destroy(Panel);
                UnityEngine.Object.Destroy(PanelGameObject);
                Panel = null;
                PanelGameObject = null;
                IsVisible = false;
                SingletonMod<Mod>.Instance.SaveConfig();
            }
        }
    }
}
