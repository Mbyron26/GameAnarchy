using ColossalFramework;
using CSShared.Common;
using CSShared.ToolButton;
using CSShared.Tools;
using CSShared.UI;
using CSShared.UI.ControlPanel;
using UnityEngine;

namespace GameAnarchy.UI;

internal class ToolButtonManager : UUIToolManagerBase<ToolButton, Mod, Config> {
    protected override Texture2D UUIIcon { get; } = UIUtils.LoadTextureFromAssembly($"{AssemblyTools.CurrentAssemblyName}.UI.Resources.InGameButton.png");
    protected override string Tooltip => SingletonMod<Mod>.Instance.ModName + $" ({SavedInputKey.ToLocalizedString("KEYNAME", Config.Instance.ControlPanelHotkey.Encode())})";

    protected override void InGameToolButtonToggle(bool isOn) => ControlPanelManager<Mod, ControlPanel>.CallPanel();
    protected override void UUIButtonToggle(bool isOn) => ControlPanelManager<Mod, ControlPanel>.CallPanel();
}