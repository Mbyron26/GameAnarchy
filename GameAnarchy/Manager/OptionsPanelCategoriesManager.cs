namespace GameAnarchy.Manager;
using ColossalFramework.UI;
using UnityEngine;

internal class OptionsPanelCategoriesManager {
    public static float MainPanelWidth = 1074f;
    public static float CategoriesDefaultWidth = 268;
    public static float ContainerDefaultWidth = 764;
    public static float ContainerDefaultPosX = 296;
    public static float ContainerDefaultPosY = 54;
    public static void SetCategoriesOffset() => SetCategoriesOffset(UIView.library.Get<UIPanel>("OptionsPanel"));
    public static void SetCategoriesOffset(UIComponent component) {
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
