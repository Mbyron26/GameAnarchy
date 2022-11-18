using ColossalFramework.UI;
using HarmonyLib;
using MbyronModsCommon;
using System;
using UnityEngine;

namespace GameAnarchy {
    [HarmonyPatch(typeof(OptionsMainPanel), "OnVisibilityChanged")]
    public static class OptionsMainPanelPatch {
        public static float MainPanelWidth = 1074f;
        public static float CategoriesDefaultWidth = 268;
        public static float ContainerDefaultWidth = 764;
        public static float ContainerDefaultPosX = 296;
        public static float ContainerDefaultPosY = 52;
        public static uint Offset => Config.Instance.OptionPanelCategoriesOffset;
        static void Postfix(UIComponent comp, bool visible) {
            try {
                var categories = comp.Find<UIListBox>("Categories");
                var optionsContainer = comp.Find<UITabContainer>("OptionsContainer");
                var delta = Offset + CategoriesDefaultWidth;
                var panelTotalWidth = MainPanelWidth + Offset;
                comp.width = panelTotalWidth;
                categories.width = delta;
                optionsContainer.width = ContainerDefaultWidth;
                optionsContainer.relativePosition = new Vector2(ContainerDefaultPosX + Offset, ContainerDefaultPosY);
            }
            catch (Exception e) {
                ModLogger.ModLog($"Options main panel patch failure, detail: {e.Message}");
            }

        }
    }

}
