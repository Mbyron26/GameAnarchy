using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnarchy {
    public class OptionPanel : OptionPanelBase<Mod, Config, OptionPanel> {
        private ToggleButton VanillaUnlimitedMoney;
        private ToggleButton CashAnarchy;
        private ToggleButton UnlockAll;
        private ToggleButton CustomUnlock;

        private string[] MilestoneLevelNames => new string[] {
            Localization.Localize.MilestonelevelName_Vanilla,
            Localization.Localize.MilestonelevelName_LittleHamlet,
            Localization.Localize.MilestonelevelName_WorthyVillage,
            Localization.Localize.MilestonelevelName_TinyTown,
            Localization.Localize.MilestonelevelName_BoomTown,
            Localization.Localize.MilestonelevelName_BusyTown,
            Localization.Localize.MilestonelevelName_BigTown,
            Localization.Localize.MilestonelevelName_SmallCity,
            Localization.Localize.MilestonelevelName_BigCity,
            Localization.Localize.MilestonelevelName_GrandCity,
            Localization.Localize.MilestonelevelName_CapitalCity,
            Localization.Localize.MilestonelevelName_ColossalCity,
            Localization.Localize.MilestonelevelName_Metropolis,
            Localization.Localize.MilestonelevelName_Megalopolis
        };


        protected override void AddExtraModInfoProperty() {
            OptionPanelTool.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"[{Localization.Localize.FastReturn}]", out UILabel _, out UILabel _);
            OptionPanelTool.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"[{Localization.Localize.SortSettings}]", out UILabel _, out UILabel _);
        }

        protected override void FillGeneralContainer() {
            AddOptimizeOptionsProperty();
            AddUnlockOptionsProperty();
            AddResourceOptionsProperty();
        }

        protected override void FillHotkeyContainer() {
            base.FillHotkeyContainer();
            OptionPanelTool.AddGroup(HotkeyContainer, PropertyPanelWidth, CommonLocalize.OptionPanel_Hotkeys);
            OptionPanelTool.AddKeymapping(Localization.Localize.AddCash, Config.Instance.AddCash, Localization.Localize.AddCashTooltip);
            OptionPanelTool.AddKeymapping(CommonLocalize.ShowControlPanel, Config.Instance.ControlPanelHotkey);
            OptionPanelTool.Reset();
        }

        private void AddOptimizeOptionsProperty() {
            OptionPanelTool.AddGroup(GeneralContainer, PropertyPanelWidth, Localization.Localize.OptimizeOptions);
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledAchievements, Localization.Localize.EnableAchievements, Localization.Localize.AllowsDynamicToggling, _ => {
                Config.Instance.EnabledAchievements = _;
                AchievementsManager.UpdateAchievements(_);
            }, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledSkipIntro, Localization.Localize.EnabledSkipIntro, null, _ => Config.Instance.EnabledSkipIntro = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddToggleButton(Config.Instance.OptionPanelCategoriesUpdated, Localization.Localize.OptionPanelCategoriesUpdated, Localization.Localize.OptionPanelCategoriesUpdatedMinor, _ => Config.Instance.OptionPanelCategoriesUpdated = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddSliderAlpha(Localization.Localize.OptionsPanelHorizontalOffsetTooltip, null, Localization.Localize.OptionsPanelHorizontalOffset, 0, 600f, 5f, Config.Instance.OptionPanelCategoriesHorizontalOffset, new SliderAlphaSize(60, 580, 30), (c, _) => Config.Instance.OptionPanelCategoriesHorizontalOffset = (uint)_, out UILabel _, out UILabel _, out SliderAlpha slider0);
            OptionPanelTool.Reset();
        }

        private readonly List<UIPanel> CustomUnlockPanels = new();
        private void AddUnlockOptionsProperty() {
            OptionPanelTool.AddGroup(GeneralContainer, PropertyPanelWidth, Localization.Localize.UnlockOptions);
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledUnlockAll, Localization.Localize.UnlockAll, Localization.Localize.UnlockAllMinor, _ => {
                Config.Instance.EnabledUnlockAll = _;
                if (_) CustomUnlock.IsChecked = false;
            }, out UILabel _, out UILabel _, out UnlockAll);
            OptionPanelTool.AddToggleButton(Config.Instance.CustomUnlock, Localization.Localize.CustomUnlock, Localization.Localize.CustomUnlockPanelTooltip, _ => {
                Config.Instance.CustomUnlock = _;
                if (_) UnlockAll.IsChecked = false;
                if (CustomUnlockPanels.Count > 0) {
                    foreach (var item in CustomUnlockPanels) {
                        item.isEnabled = _;
                    }
                }
            }, out UILabel _, out UILabel _, out CustomUnlock);
            CustomUnlockPanels.Add(OptionPanelTool.AddDropDown(Localization.Localize.MilestonelevelName_MilestoneUnlockLevel, null, MilestoneLevelNames, Config.Instance.MilestoneLevel, 250, 30, out UILabel _, out UILabel _, out UIDropDown dropDown0, majorOffset: new RectOffset(20, 0, 0, 0)));
            dropDown0.eventSelectedIndexChanged += (c, value) => Config.Instance.MilestoneLevel = value;
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localization.Localize.EnabledInfoView, null, Config.Instance.EnabledInfoView, _ => Config.Instance.EnabledInfoView = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localization.Localize.UnlockAllRoads, null, Config.Instance.UnlockAllRoads, _ => Config.Instance.UnlockAllRoads = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localization.Localize.UnlockTransport, null, Config.Instance.UnlockTransport, _ => Config.Instance.UnlockTransport = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localization.Localize.UnlockTrainTrack, null, Config.Instance.UnlockTrainTrack, _ => Config.Instance.UnlockTrainTrack = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localization.Localize.UnlockMetroTrack, null, Config.Instance.UnlockMetroTrack, _ => Config.Instance.UnlockMetroTrack = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localization.Localize.UnlockPolicies, null, Config.Instance.UnlockPolicies, _ => Config.Instance.UnlockPolicies = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localization.Localize.UnlockUniqueBuilding, Localization.Localize.UnlockUniqueBuildingMinor, Config.Instance.UnlockUniqueBuilding, _ => Config.Instance.UnlockUniqueBuilding = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            if (CustomUnlockPanels.Count > 0)
                foreach (var item in CustomUnlockPanels) {
                    item.isEnabled = CustomUnlock.IsChecked;
                }
            OptionPanelTool.Reset();
        }

        private readonly List<UIPanel> CashAnarchyPanels = new();
        private UIPanel InitalCashPanel;
        private void AddResourceOptionsProperty() {
            OptionPanelTool.AddGroup(GeneralContainer, PropertyPanelWidth, Localization.Localize.ResourceOptions);
            OptionPanelTool.AddToggleButton(Config.Instance.Refund, Localization.Localize.Refund, Localization.Localize.AllowsDynamicToggling, _ => Config.Instance.Refund = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddToggleButton(Config.Instance.UnlimitedMoney, Localization.Localize.VanillaUnlimitedMoneyMode, Localization.Localize.VanillaUnlimitedMoneyModeMinor, _ => {
                Config.Instance.UnlimitedMoney = _;
                if (_) CashAnarchy.IsChecked = false;
            }, out UILabel _, out UILabel _, out VanillaUnlimitedMoney);
            OptionPanelTool.AddToggleButton(Config.Instance.CashAnarchy, Localization.Localize.MoneyAnarchyMode, Localization.Localize.MoneyAnarchyModeMinor, _ => {
                Config.Instance.CashAnarchy = _;
                if (_) VanillaUnlimitedMoney.IsChecked = false;
                foreach (var item in CashAnarchyPanels) {
                    item.isEnabled = _;
                }
            }, out UILabel _, out UILabel _, out CashAnarchy);
            CashAnarchyPanels.Add(OptionPanelTool.AddField<CustomLongValueField, long>(Localization.Localize.AddCashThreshold, null, Config.Instance.DefaultMinAmount, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField0, majorOffset: new(20, 0, 0, 0)));
            valueField0.OnValueChanged += (v) => Config.Instance.DefaultMinAmount = (int)v;
            CashAnarchyPanels.Add(OptionPanelTool.AddField<CustomLongValueField, long>(Localization.Localize.AddCashAmount, null, Config.Instance.DefaultGetCash, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField1, majorOffset: new(20, 0, 0, 0)));
            valueField1.OnValueChanged += (v) => Config.Instance.DefaultGetCash = (int)v;
            foreach (var item in CashAnarchyPanels) {
                item.isEnabled = Config.Instance.CashAnarchy;
            }
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledInitialCash, Localization.Localize.InitialMoney, Localization.Localize.InitialMoneyWarning, _ => {
                Config.Instance.EnabledInitialCash = _;
                InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
            }, out UILabel _, out UILabel _, out ToggleButton _);
            InitalCashPanel = OptionPanelTool.AddField(Localization.Localize.Amount, null, Config.Instance.InitialCash, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField2, (v) => Config.Instance.InitialCash = v, majorOffset: new(20, 0, 0, 0));
            InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
            OptionPanelTool.Reset();
        }

    }

}

