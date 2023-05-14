namespace GameAnarchy.UI;
using ColossalFramework;
using MbyronModsCommon.UI;
using UnifiedUI.Helpers;

internal class UUI {
    internal static UUICustomButton UUIButton { get; private set; }
    private static string Tooltip => SingletonMod<Mod>.Instance.ModName + $" ({SavedInputKey.ToLocalizedString("KEYNAME", Config.Instance.ControlPanelHotkey.Encode())})";

    public static void Initialize() {
        if (UUIButton is null) {
            InternalLogger.Log("Register UUI button.");
            UUIButton = UUIHelpers.RegisterCustomButton(nameof(GameAnarchy), null, Tooltip, UIUtils.LoadTextureFromAssembly($"{AssemblyUtils.CurrentAssemblyName}.UI.UUIResource.UUI.png"), OnToggleButton);
            UUIButton.Button.eventTooltipEnter += (c, e) => c.tooltip = Tooltip;
            UUIButton.IsPressed = false;
        }
    }
    public static void Destory() {
        if (UUIButton is not null) {
            InternalLogger.Log("Reset UUI button.");
        }
    }
    private static void OnToggleButton(bool isToggled) => ControlPanelManager<ControlPanel>.CallPanel();

}
