using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;
using GameAnarchy.Localization;
using GameAnarchy.UI;

namespace GameAnarchy {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);
            new OptionPanel_General(panel.AddTab(CommonLocalize.OptionPanel_General, CommonLocalize.OptionPanel_General, 0, 30, 1.2f).MainPanel, TypeWidth.ShrinkageWidth);
            new OptionPanel_Hotkey(panel.AddTab(CommonLocalize.OptionPanel_Hotkeys, CommonLocalize.OptionPanel_Hotkeys, 0, 30, 1.2f).MainPanel, TypeWidth.NormalWidth);
            new OptionPanel_Advanced(panel.AddTab(CommonLocalize.OptionPanel_Advanced, CommonLocalize.OptionPanel_Advanced, 0, 30, 1.2f).MainPanel, TypeWidth.NormalWidth);
        }
    }

    public class OptionPanel_Advanced : AdvancedBase<Mod, Config> {
        public OptionPanel_Advanced(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) { }
        protected override void ResetSettings() => ResetSettings<OptionPanel>();
    }

    public class OptionPanel_Hotkey {
        public OptionPanel_Hotkey(UIComponent parent, TypeWidth typeWidth) {
            OptionPanelTool.AddGroup(parent, (float)typeWidth, CommonLocalize.OptionPanel_Hotkeys);
            OptionPanelTool.AddKeymapping(Localize.AddCash, Config.Instance.AddCash, Localize.AddCashTooltip);
            OptionPanelTool.AddKeymapping(CommonLocalize.ShowControlPanel, Config.Instance.ControlPanelHotkey);
            OptionPanelTool.Reset();
        }
    }

    public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
        private ToggleButton VanillaUnlimitedMoney;
        private ToggleButton CashAnarchy;
        private ToggleButton UnlockAll;
        private ToggleButton CustomUnlock;

        private string[] MilestoneLevelNames => new string[] {
            Localize.MilestonelevelName_Vanilla,
            Localize.MilestonelevelName_LittleHamlet,
            Localize.MilestonelevelName_WorthyVillage,
            Localize.MilestonelevelName_TinyTown,
            Localize.MilestonelevelName_BoomTown,
            Localize.MilestonelevelName_BusyTown,
            Localize.MilestonelevelName_BigTown,
            Localize.MilestonelevelName_SmallCity,
            Localize.MilestonelevelName_BigCity,
            Localize.MilestonelevelName_GrandCity,
            Localize.MilestonelevelName_CapitalCity,
            Localize.MilestonelevelName_ColossalCity,
            Localize.MilestonelevelName_Metropolis,
            Localize.MilestonelevelName_Megalopolis
        };

        public OptionPanel_General(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) {
            var defaultWidth = (float)typeWidth;
            AddModInfoProperty(parent, defaultWidth);
            AddOptimizeOptionsProperty(parent, defaultWidth);
            AddUnlockOptionsProperty(parent, defaultWidth);
            AddResourceOptionsProperty(parent, defaultWidth);
        }

        private static void AddModInfoProperty(UIComponent parent, float width) {
            OptionPanelTool.AddGroup(parent, width, CommonLocalize.OptionPanel_ModInfo);
            OptionPanelTool.AddLabel($"{CommonLocalize.OptionPanel_Version}", $"{ModMainInfo<Mod>.ModVersion}({ModMainInfo<Mod>.VersionType})", out UILabel _, out UILabel _);
            OptionPanelTool.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"[{Localize.FastReturn}]", out UILabel _, out UILabel _);
            OptionPanelTool.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"[{Localize.SortSettings}]", out UILabel _, out UILabel _);
            OptionPanelTool.AddDropDown(CommonLocalize.Language, null, GetLanguages().ToArray(), LanguagesIndex, 310, 30, out UILabel _, out UILabel _, out UIDropDown _, (v) => {
                OnLanguageSelectedIndexChanged<OptionPanel>(v);
                ControlPanelManager.OnLocaleChanged();
            });
            OptionPanelTool.Reset();
        }

        private void AddOptimizeOptionsProperty(UIComponent parent, float width) {
            OptionPanelTool.AddGroup(parent, width, Localize.OptimizeOptions);
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledAchievements, Localize.EnableAchievements, Localize.AllowsDynamicToggling, _ => {
                Config.Instance.EnabledAchievements = _;
                AchievementsManager.UpdateAchievements(_);
            }, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledSkipIntro, Localize.EnabledSkipIntro, null, _ => Config.Instance.EnabledSkipIntro = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddToggleButton(Config.Instance.OptionPanelCategoriesUpdated, Localize.OptionPanelCategoriesUpdated, Localize.OptionPanelCategoriesUpdatedMinor, _ => Config.Instance.OptionPanelCategoriesUpdated = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddSliderAlpha(Localize.OptionsPanelHorizontalOffsetTooltip, null, Localize.OptionsPanelHorizontalOffset, 0, 600f, 5f, Config.Instance.OptionPanelCategoriesHorizontalOffset, new SliderAlphaSize(60, 594, 30), (c, _) => Config.Instance.OptionPanelCategoriesHorizontalOffset = (uint)_, out UILabel _, out UILabel _, out SliderAlpha slider0);
            OptionPanelTool.Reset();
        }

        private readonly List<UIPanel> CustomUnlockPanels = new();
        private void AddUnlockOptionsProperty(UIComponent parent, float width) {
            OptionPanelTool.AddGroup(parent, width, Localize.UnlockOptions);
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledUnlockAll, Localize.UnlockAll, Localize.UnlockAllMinor, _ => {
                Config.Instance.EnabledUnlockAll = _;
                if (_) CustomUnlock.IsChecked = false;
            }, out UILabel _, out UILabel _, out UnlockAll);
            OptionPanelTool.AddToggleButton(Config.Instance.CustomUnlock, Localize.CustomUnlock, Localize.CustomUnlockPanelTooltip, _ => {
                Config.Instance.CustomUnlock = _;
                if (_) UnlockAll.IsChecked = false;
                if (CustomUnlockPanels.Count > 0) {
                    foreach (var item in CustomUnlockPanels) {
                        item.isEnabled = _;
                    }
                }
            }, out UILabel _, out UILabel _, out CustomUnlock);
            CustomUnlockPanels.Add(OptionPanelTool.AddDropDown(Localize.MilestonelevelName_MilestoneUnlockLevel, null, MilestoneLevelNames, Config.Instance.MilestoneLevel, 250, 30, out UILabel _, out UILabel _, out UIDropDown dropDown0, majorOffset: new RectOffset(20, 0, 0, 0)));
            dropDown0.eventSelectedIndexChanged += (c, value) => Config.Instance.MilestoneLevel = value;
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localize.EnabledInfoView, null, Config.Instance.EnabledInfoView, _ => Config.Instance.EnabledInfoView = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localize.UnlockAllRoads, null, Config.Instance.UnlockAllRoads, _ => Config.Instance.UnlockAllRoads = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localize.UnlockTransport, null, Config.Instance.UnlockTransport, _ => Config.Instance.UnlockTransport = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localize.UnlockTrainTrack, null, Config.Instance.UnlockTrainTrack, _ => Config.Instance.UnlockTrainTrack = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localize.UnlockMetroTrack, null, Config.Instance.UnlockMetroTrack, _ => Config.Instance.UnlockMetroTrack = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localize.UnlockPolicies, null, Config.Instance.UnlockPolicies, _ => Config.Instance.UnlockPolicies = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            CustomUnlockPanels.Add(OptionPanelTool.AddCheckBox(Localize.UnlockUniqueBuilding, Localize.UnlockUniqueBuildingMinor, Config.Instance.UnlockUniqueBuilding, _ => Config.Instance.UnlockUniqueBuilding = _, out UILabel _, out UILabel _, out CheckBox _, new RectOffset(30, 10, 10, 10)));
            if (CustomUnlockPanels.Count > 0)
                foreach (var item in CustomUnlockPanels) {
                    item.isEnabled = CustomUnlock.IsChecked;
                }
            OptionPanelTool.Reset();
        }

        private readonly List<UIPanel> CashAnarchyPanels = new();
        private UIPanel InitalCashPanel;
        private void AddResourceOptionsProperty(UIComponent parent, float width) {
            OptionPanelTool.AddGroup(parent, width, Localize.UnlockOptions);
            OptionPanelTool.AddToggleButton(Config.Instance.Refund, Localize.Refund, Localize.AllowsDynamicToggling, _ => Config.Instance.Refund = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTool.AddToggleButton(Config.Instance.UnlimitedMoney, Localize.VanillaUnlimitedMoneyMode, Localize.VanillaUnlimitedMoneyModeMinor, _ => {
                Config.Instance.UnlimitedMoney = _;
                if (_) CashAnarchy.IsChecked = false;
            }, out UILabel _, out UILabel _, out VanillaUnlimitedMoney);
            OptionPanelTool.AddToggleButton(Config.Instance.CashAnarchy, Localize.MoneyAnarchyMode, Localize.MoneyAnarchyModeMinor, _ => {
                Config.Instance.CashAnarchy = _;
                if (_) VanillaUnlimitedMoney.IsChecked = false;
                foreach (var item in CashAnarchyPanels) {
                    item.isEnabled = _;
                }
            }, out UILabel _, out UILabel _, out CashAnarchy);
            CashAnarchyPanels.Add(OptionPanelTool.AddField<CustomLongValueField, long>(Localize.AddCashThreshold, null, Config.Instance.DefaultMinAmount, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField0, majorOffset: new(20, 0, 0, 0)));
            valueField0.OnValueChanged += (v) => Config.Instance.DefaultMinAmount = (int)v;
            CashAnarchyPanels.Add(OptionPanelTool.AddField<CustomLongValueField, long>(Localize.AddCashAmount, null, Config.Instance.DefaultGetCash, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField1, majorOffset: new(20, 0, 0, 0)));
            valueField1.OnValueChanged += (v) => Config.Instance.DefaultGetCash = (int)v;
            foreach (var item in CashAnarchyPanels) {
                item.isEnabled = Config.Instance.CashAnarchy;
            }
            OptionPanelTool.AddToggleButton(Config.Instance.EnabledInitialCash, Localize.InitialMoney, Localize.InitialMoneyWarning, _ => {
                Config.Instance.EnabledInitialCash = _;
                InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
            }, out UILabel _, out UILabel _, out ToggleButton _);
            InitalCashPanel = OptionPanelTool.AddField(Localize.Amount, null, Config.Instance.InitialCash, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField2, (v) => Config.Instance.InitialCash = v, majorOffset: new(20, 0, 0, 0));
            InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
            OptionPanelTool.Reset();
        }

    }

}

