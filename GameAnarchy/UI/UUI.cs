using ColossalFramework;
using UnifiedUI.Helpers;

namespace GameAnarchy.UI {
    internal class UUI {
        internal static UUICustomButton UUIButton { get; private set; }
        private static string Tooltip => SingletonMod<Mod>.Instance.ModName + $" ({SavedInputKey.ToLocalizedString("KEYNAME", Config.Instance.ControlPanelHotkey.Encode())})";

        public static void Initialize() {
            if (UUIButton is null) {
                ModLogger.ModLog("Register UUI button.");
                UUIButton = UUIHelpers.RegisterCustomButton(nameof(GameAnarchy), null, Tooltip, UIUtils.LoadTextureFromAssembly($"{AssemblyUtils.CurrentAssemblyName}.UI.UUIResource.UUI.png"), OnToggleButton);
                UUIButton.Button.eventTooltipEnter += (c, e) => c.tooltip = Tooltip;
                UUIButton.IsPressed = false;
            }
        }
        public static void Destory() {
            if (UUIButton is not null) {
                ModLogger.ModLog("Reset UUI button.");
            }
        }
        private static void OnToggleButton(bool isToggled) => ControlPanelManager.HotkeyToggle();

    }

}
