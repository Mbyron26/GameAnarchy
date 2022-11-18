using ColossalFramework.UI;
using UnityEngine;

namespace GameAnarchy {
    public class InfoViewsExtension : MonoBehaviour {
        private InfoViewsPanel infoViewsPanel;
        private UIButton[] uIButtons;
        public void Start() {
            infoViewsPanel = GameObject.Find("InfoViewsPanel").GetComponent<InfoViewsPanel>();
            uIButtons = infoViewsPanel.GetComponentsInChildren<UIButton>();
        }

        public void Update() => CheckState();

        private void CheckState() {
            if (Config.Instance.EnabledUnlockAll) return;
            if (Config.Instance.EnabledInfoView) {
                foreach (var button in uIButtons) {
                    button.isEnabled = true;
                }
            }
        }
    }
}
