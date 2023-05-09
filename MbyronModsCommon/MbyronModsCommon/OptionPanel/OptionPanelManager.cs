namespace MbyronModsCommon;
using ColossalFramework.UI;
using MbyronModsCommon.UI;
using ICities;
using UnityEngine;

public class OptionPanelManager<Mod, OptionPanel> where OptionPanel : CustomUIPanel where Mod : IMod {
    private static OptionPanel Panel { get; set; }
    private static UIPanel BasePanel { get; set; }
    private static GameObject ContainerGameObject { get; set; }

    public static void LocaleChanged() {
        if (BasePanel is not null && BasePanel.isVisible) {
            Destroy();
#if BETA_DEBUG
            DebugUtils.TimeCalculater(Create, "OptionPanelManager LocaleChanged Create");
#else
                Create();
#endif
        }
    }
    public static void Create() {
        try {
            if (ContainerGameObject is null) {
                ContainerGameObject = new(typeof(OptionPanel).Name);
                ContainerGameObject.transform.parent = BasePanel.transform;
                Panel = ContainerGameObject.AddComponent<OptionPanel>();
                Panel.relativePosition = Vector2.zero;
            }
        }
        catch (UnityException e) {
            InternalLogger.Exception("Create option panel object failed.", e);
        }
    }
    private static void Destroy() {
        if (ContainerGameObject is not null) {
            SingletonMod<Mod>.Instance.SaveConfig();
            UnityEngine.Object.Destroy(Panel.gameObject);
            UnityEngine.Object.Destroy(Panel);
            UnityEngine.Object.Destroy(ContainerGameObject);
            Panel = null;
            ContainerGameObject = null;
        }
    }
    public static void SettingsUI(UIHelperBase helper) {
        var scrollablePanel = ((UIHelper)helper).self as UIScrollablePanel;
        scrollablePanel.autoLayout = false;
        BasePanel = scrollablePanel.parent as UIPanel;
        foreach (var components in BasePanel.components)
            components.isVisible = false;
        BasePanel.autoLayout = false;
        BasePanel.eventVisibilityChanged += (c, v) => {
            if (v) {
#if BETA_DEBUG
                DebugUtils.TimeCalculater(Create, "OptionPanelManager Create");
#else
                Create();
#endif
            } else {
#if BETA_DEBUG
                DebugUtils.TimeCalculater(Destroy, "OptionPanelManager Destroy");
#else
                Destroy();
#endif
            }
        };
    }
}