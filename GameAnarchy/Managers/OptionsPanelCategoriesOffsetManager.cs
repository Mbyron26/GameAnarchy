using ColossalFramework.UI;
using CSLModsCommon.Manager;
using GameAnarchy.ModSettings;
using UnityEngine;

namespace GameAnarchy.Managers;

public class OptionsPanelCategoriesOffsetManager : ManagerBase {
    private ModSetting _modSetting;

    public float MainPanelWidth { get; set; } = 1074f;
    public float CategoriesDefaultWidth { get; set; } = 268;
    public float ContainerDefaultWidth { get; set; } = 764;
    public float ContainerDefaultPosX { get; set; } = 296;
    public float ContainerDefaultPosY { get; set; } = 54;

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
    }

    public void SetCategoriesOffset() => SetCategoriesOffset(UIView.library.Get<UIPanel>("OptionsPanel"));

    public void SetCategoriesOffset(UIComponent component) {
        var categories = component.Find<UIListBox>("Categories");
        var optionsContainer = component.Find<UITabContainer>("OptionsContainer");
        var delta = _modSetting.OptionsPanelCategoriesHorizontalOffset + CategoriesDefaultWidth;
        var panelTotalWidth = MainPanelWidth + _modSetting.OptionsPanelCategoriesHorizontalOffset;
        component.width = panelTotalWidth;
        categories.width = delta;
        optionsContainer.width = ContainerDefaultWidth;
        optionsContainer.relativePosition = new Vector2(ContainerDefaultPosX + _modSetting.OptionsPanelCategoriesHorizontalOffset, ContainerDefaultPosY);
    }
}