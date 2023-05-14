namespace GameAnarchy.Manager;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

internal class InfoViewsManager {
    private static GameObject infoViewsObject;

    public static void Deploy(LoadMode mode) {
        if (!(mode == LoadMode.NewGame || mode == LoadMode.LoadGame))
            return;
        infoViewsObject = new GameObject("InfoViewsExtension");
        infoViewsObject.AddComponent<InfoViewsExtension>();
    }
    public static void Destroy() {
        if (infoViewsObject is not null) {
            Object.Destroy(infoViewsObject);
        }
    }
}

public class InfoViewsExtension : MonoBehaviour {
    private InfoViewsPanel infoViewsPanel;
    private UIButton[] uIButtons;
    public void Start() {
        infoViewsPanel = GameObject.Find("InfoViewsPanel").GetComponent<InfoViewsPanel>();
        uIButtons = infoViewsPanel.GetComponentsInChildren<UIButton>();
    }
    public void Update() {
        if (Config.Instance.EnabledUnlockAll)
            return;
        if (Config.Instance.EnabledInfoView) {
            if (uIButtons is null) {
                ExternalLogger.DebugMode("Couldn't access InfoViewsPanel buttons", Config.Instance.DebugMode);
                return;
            }
            foreach (var button in uIButtons) {
                button.isEnabled = true;
            }
        }
    }
}
