using System;
using ColossalFramework.Globalization;
using CSLModsCommon.Manager;
using GameAnarchy.ModSettings;
using GameAnarchy.UI;

namespace GameAnarchy.Managers;

internal class ControlPanelManager : ControlPanelManagerBase {
    private SettingManager _settingManager;
    private ModSetting _modSetting;

    protected override void OnCreate() {
        base.OnCreate();
        _settingManager = Domain.GetOrCreateManager<SettingManager>();
        _modSetting = _settingManager.GetSetting<ModSetting>();
    }

    protected override void OnGameInitialized() {
        base.OnGameInitialized();
        LocaleManager.eventLocaleChanged += ReloadPanelIfOpen;
    }

    public override Type ResisterPanelType() => typeof(ModControlPanel);

    protected override void OnBeforePanelDestroyed() {
        base.OnBeforePanelDestroyed();
        _settingManager.Save(_modSetting);
    }
}