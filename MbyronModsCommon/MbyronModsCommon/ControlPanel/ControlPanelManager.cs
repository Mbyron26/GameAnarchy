namespace MbyronModsCommon;
using ColossalFramework.UI;
using System;
using UnityEngine;

public class ControlPanelManager<T> where T : UIComponent {
    private static GameObject panelGameObject;
    private static bool isVisible;
    private static T panel;

    public static event Action<bool> EventOnVisibleChanged;
    public static bool IsVisible {
        get => isVisible;
        private set {
            if (value != isVisible) {
                isVisible = value;
                EventOnVisibleChanged?.Invoke(isVisible);
            }
        }
    }

    public static void CallPanel() {
        if (IsVisible) {
            Close();
        } else {
#if BETA_DEBUG
            DebugUtils.TimeCalculater(Create, tag: "Control Panel Invoke time: ");
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
    public static void Create() {
        if (panelGameObject is not null)
            return;
        panelGameObject = new GameObject(AssemblyUtils.CurrentAssemblyName + "ControlPanel");
        panelGameObject.transform.parent = UIView.GetAView().transform;
        panel = panelGameObject.AddComponent<T>();
        panel.Show();
        IsVisible = true;
    }
    public static void Close() {
        if (panelGameObject is null)
            return;
        UnityEngine.Object.Destroy(panel);
        UnityEngine.Object.Destroy(panelGameObject);
        panel = null;
        panelGameObject = null;
        IsVisible = false;
    }
}

