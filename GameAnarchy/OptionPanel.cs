using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;
using GameAnarchy.Localization;

namespace GameAnarchy {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);
            new OptionPanel_General(panel.AddTab(CommonLocalize.OptionPanel_General, CommonLocalize.OptionPanel_General, 0, 30, 1.2f).MainPanel, TypeWidth.ShrinkageWidth);
            new OptionPanel_Hotkey(panel.AddTab(CommonLocalize.OptionPanel_Hotkeys, CommonLocalize.OptionPanel_Hotkeys, 0, 30, 1.2f).MainPanel, TypeWidth.NormalWidth);
            new AdvancedBase<Mod, Config>(panel.AddTab(CommonLocalize.OptionPanel_Advanced, CommonLocalize.OptionPanel_Advanced, 0, 30, 1.2f).MainPanel, TypeWidth.NormalWidth);
        }
    }

    public class OptionPanel_Hotkey {
        public OptionPanel_Hotkey(UIComponent parent, TypeWidth typeWidth) {
            OptionPanelTools.Instance.AddGroup(parent, (float)typeWidth, CommonLocalize.OptionPanel_Hotkeys);
            OptionPanelTools.Instance.AddKeymapping(Localize.AddCash, Config.Instance.AddCash, Localize.AddCashTooltip);
            OptionPanelTools.Instance.AddKeymapping(CommonLocalize.ShowControlPanel, Config.Instance.ShowControlPanelHotkey);
        }
    }

    public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
        private ToggleButton VanillaUnlimitedMoney;
        private ToggleButton CashAnarchy;
        private ToggleButton UnlockAll;
        private ToggleButton CustomUnlock;

        private string[] MilestoneLevelNames { get; set; } = new string[] {
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
            #region ModInfo
            OptionPanelTools.Instance.AddGroup(parent, (float)typeWidth, CommonLocalize.OptionPanel_ModInfo);
            OptionPanelTools.Instance.AddLabel($"{CommonLocalize.OptionPanel_Version}: {ModMainInfo<Mod>.ModVersion}", null, out UILabel _, out UILabel _);
            OptionPanelTools.Instance.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}: [{Localize.FastReturn}]", null, out UILabel _, out UILabel _);
            OptionPanelTools.Instance.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}: [{Localize.SortSettings}]", null, out UILabel _, out UILabel _);
            OptionPanelTools.Instance.AddDropDown(CommonLocalize.Language, null, GetLanguages().ToArray(), LanguagesIndex, 310, 30, 1f, out UILabel _, out UILabel _, out UIDropDown dropDown, new RectOffset(6, 10, 6, 0), new RectOffset(6, 6, 4, 0));
            dropDown.eventSelectedIndexChanged += (c, v) => OnLanguageSelectedIndexChanged<OptionPanel>(v);
            OptionPanelTools.Instance.Reset();
            #endregion

            OptionPanelTools.Instance.AddGroup(parent, defaultWidth, Localize.OptimizeOptions);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.EnabledAchievements, Localize.EnableAchievements, null, _ => {
                Config.Instance.EnabledAchievements = _;
                AchievementsManager.UpdateAchievements(_);
            }, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.EnabledSkipIntro, Localize.EnabledSkipIntro, null, _ => Config.Instance.EnabledSkipIntro = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTools.Instance.AddSliderAlpha(null, Localize.OptionsPanelHorizontalOffsetTooltip, Localize.OptionsPanelHorizontalOffset, 0, 400f, 5f, Config.Instance.OptionPanelCategoriesOffset, new CustomSliderBase.Size(60, 594, 30), (c, _) => Config.Instance.OptionPanelCategoriesOffset = (uint)_, out UILabel _, out UILabel _, out CustomSliderBase slider0);
            OptionPanelTools.Instance.Reset();

            AddUnlockOptionsGroup(parent, defaultWidth);
            AddResourceOptionsGroup(parent, defaultWidth);

        }
        private readonly List<UIPanel> CashAnarchyPanels = new();
        private UIPanel InitalCashPanel;
        private void AddResourceOptionsGroup(UIComponent parent, float width) {
            OptionPanelTools.Instance.AddGroup(parent, width, Localize.UnlockOptions);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.Refund, Localize.Refund, null, _ => Config.Instance.Refund = _, out UILabel _, out UILabel _, out ToggleButton _);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.UnlimitedMoney, Localize.VanillaUnlimitedMoney, null, _ => {
                Config.Instance.UnlimitedMoney = _;
                if (_) CashAnarchy.IsChecked = false;
            }, out UILabel _, out UILabel _, out VanillaUnlimitedMoney);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.CashAnarchy, Localize.CashAnarchy, null, _ => {
                Config.Instance.CashAnarchy = _;
                if (_) VanillaUnlimitedMoney.IsChecked = false;
                foreach (var item in CashAnarchyPanels) {
                    item.isEnabled = _;
                }
            }, out UILabel _, out UILabel _, out CashAnarchy);
            CashAnarchyPanels.Add(OptionPanelTools.Instance.AddField<CustomLongValueField, long>(Localize.AddCashThreshold, null, Config.Instance.DefaultMinAmount, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField0, majorOffset: new(20, 0, 0, 0)));
            valueField0.OnValueChanged += (v) => Config.Instance.DefaultMinAmount = (int)v;
            CashAnarchyPanels.Add(OptionPanelTools.Instance.AddField<CustomLongValueField, long>(Localize.AddCashAmount, null, Config.Instance.DefaultGetCash, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField1, majorOffset: new(20, 0, 0, 0)));
            valueField1.OnValueChanged += (v) => Config.Instance.DefaultMinAmount = (int)v;
            foreach (var item in CashAnarchyPanels) {
                item.isEnabled = Config.Instance.CashAnarchy;
            }
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.EnabledInitialCash, Localize.InitialCash, null, _ => {
                Config.Instance.EnabledInitialCash = _;
                InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
            }, out UILabel _, out UILabel _, out ToggleButton _);
            InitalCashPanel = OptionPanelTools.Instance.AddField(Localize.InitialCashWarning, null, Config.Instance.InitialCash, 100, 100000000, out UILabel _, out UILabel _, out CustomLongValueField valueField2, (v) => Config.Instance.InitialCash = v, majorOffset: new(20, 0, 0, 0));
            InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
            OptionPanelTools.Instance.Reset();
        }

        private readonly List<UIPanel> CustomUnlockPanels = new();
        private void AddUnlockOptionsGroup(UIComponent parent, float width) {
            OptionPanelTools.Instance.AddGroup(parent, width, Localize.UnlockOptions);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.EnabledUnlockAll, Localize.UnlockAll, Localize.UnlockAllMinor, _ => {
                Config.Instance.EnabledUnlockAll = _;
                if (_) CustomUnlock.IsChecked = false;
            }, out UILabel _, out UILabel _, out UnlockAll);
            OptionPanelTools.Instance.AddToggleButton(Config.Instance.CustomUnlock, Localize.CustomUnlock, Localize.CustomUnlockPanelTooltip, _ => {
                Config.Instance.CustomUnlock = _;
                if (_) UnlockAll.IsChecked = false;
                if (CustomUnlockPanels.Count > 0) {
                    foreach (var item in CustomUnlockPanels) {
                        item.isEnabled = _;
                    }
                }
            }, out UILabel _, out UILabel _, out CustomUnlock);
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddDropDown(Localize.MilestonelevelName_MilestoneUnlockLevel, null, MilestoneLevelNames, Config.Instance.MilestoneLevel, 250, 30, 1f, out UILabel _, out UILabel _, out UIDropDown dropDown0, new RectOffset(10, 10, 8, 0), new RectOffset(6, 6, 4, 0), new RectOffset(20, 0, 0, 0)));
            dropDown0.eventSelectedIndexChanged += (c, value) => Config.Instance.MilestoneLevel = value;
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddToggleButton(Config.Instance.EnabledInfoView, Localize.EnabledInfoView, null, _ => Config.Instance.EnabledInfoView = _, out UILabel _, out UILabel _, out ToggleButton _, new RectOffset(20, 0, 0, 0)));
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddToggleButton(Config.Instance.UnlockAllRoads, Localize.UnlockAllRoads, null, _ => Config.Instance.UnlockAllRoads = _, out UILabel _, out UILabel _, out ToggleButton _, new RectOffset(20, 0, 0, 0)));
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddToggleButton(Config.Instance.UnlockTransport, Localize.UnlockTransport, null, _ => Config.Instance.UnlockTransport = _, out UILabel _, out UILabel _, out ToggleButton _, new RectOffset(20, 0, 0, 0)));
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddToggleButton(Config.Instance.UnlockTrainTrack, Localize.UnlockTrainTrack, null, _ => Config.Instance.UnlockTrainTrack = _, out UILabel _, out UILabel _, out ToggleButton _, new RectOffset(20, 0, 0, 0)));
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddToggleButton(Config.Instance.UnlockMetroTrack, Localize.UnlockMetroTrack, null, _ => Config.Instance.UnlockMetroTrack = _, out UILabel _, out UILabel _, out ToggleButton _, new RectOffset(20, 0, 0, 0)));
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddToggleButton(Config.Instance.UnlockPolicies, Localize.UnlockPolicies, null, _ => Config.Instance.UnlockPolicies = _, out UILabel _, out UILabel _, out ToggleButton _, new RectOffset(20, 0, 0, 0)));
            CustomUnlockPanels.Add(OptionPanelTools.Instance.AddToggleButton(Config.Instance.UnlockUniqueBuilding, Localize.UnlockUniqueBuilding, Localize.UnlockUniqueBuildingMinor, _ => Config.Instance.UnlockUniqueBuilding = _, out UILabel _, out UILabel _, out ToggleButton _, new RectOffset(20, 0, 0, 0), new RectOffset(20, 0, 0, 0)));
            if (CustomUnlockPanels.Count > 0)
                foreach (var item in CustomUnlockPanels) {
                    item.isEnabled = CustomUnlock.IsChecked;
                }
            OptionPanelTools.Instance.Reset();
        }


    }

}

