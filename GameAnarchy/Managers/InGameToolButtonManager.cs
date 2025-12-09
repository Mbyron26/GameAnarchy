using System;
using CSLModsCommon.KeyBindings;
using CSLModsCommon.Manager;
using CSLModsCommon.ToolButton;
using GameAnarchy.ModSettings;
using GameAnarchy.UI;

namespace GameAnarchy.Managers;

internal class InGameToolButtonManager : InGameToolManagerBase {
    private ModSetting _modSetting;

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
    }

    protected override KeyBinding ToggleKeyBinding => _modSetting.ControlPanelToggleKeyBinding;

    protected override Type GetToolButtonType() => typeof(ToolButton);

    protected override PanelManagerBase CreatePanelManager() => Domain.GetOrCreateManager<ControlPanelManager>();
}