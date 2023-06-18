namespace GameAnarchy;
using ColossalFramework.UI;
using UnityEngine;

public partial class Manager {
    public float MainPanelWidth { get; set; } = 1074f;
    public float CategoriesDefaultWidth { get; set; } = 268;
    public float ContainerDefaultWidth { get; set; } = 764;
    public float ContainerDefaultPosX { get; set; } = 296;
    public float ContainerDefaultPosY { get; set; } = 54;

    public void SetCategoriesOffset() => SetCategoriesOffset(UIView.library.Get<UIPanel>("OptionsPanel"));

    public void SetCategoriesOffset(UIComponent component) {
        var categories = component.Find<UIListBox>("Categories");
        var optionsContainer = component.Find<UITabContainer>("OptionsContainer");
        var delta = Config.Instance.OptionPanelCategoriesHorizontalOffset + CategoriesDefaultWidth;
        var panelTotalWidth = MainPanelWidth + Config.Instance.OptionPanelCategoriesHorizontalOffset;
        component.width = panelTotalWidth;
        categories.width = delta;
        optionsContainer.width = ContainerDefaultWidth;
        optionsContainer.relativePosition = new Vector2(ContainerDefaultPosX + Config.Instance.OptionPanelCategoriesHorizontalOffset, ContainerDefaultPosY);
    }
}
