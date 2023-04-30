using ColossalFramework.UI;
using GameAnarchy.UI;
using System.Collections.Generic;
using MbyronModsCommon.UI;
using UnityEngine;
namespace GameAnarchy;

public class OptionPanel : OptionPanelBase<Mod, Config, OptionPanel> {
    private CustomUIToggleButton VanillaUnlimitedMoney;
    private CustomUIToggleButton CashAnarchy;
    private CustomUIToggleButton UnlockAll;
    private CustomUIToggleButton CustomUnlock;
#if DEBUG
    protected CustomUIScrollablePanel DebugContainer { get; private set; }
#endif
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
#if DEBUG
    protected override void AddExtraContainer() {
        base.AddExtraContainer();
        DebugContainer = AddTab("Debug");
        OptionPanelHelper.AddGroup(DebugContainer, "Debug");
        OptionPanelHelper.AddButton("Control panel test", null, "Open", 250, 30, () => ControlPanelManager.HotkeyToggle());
        OptionPanelHelper.AddStringField("File dirc", Application.dataPath, null);
        OptionPanelHelper.Reset();
    }
#endif
    protected override void AddExtraModInfoProperty() {
        OptionPanelHelper.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"{Localization.Localize.FastReturn}");
        OptionPanelHelper.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"{Localization.Localize.SortSettings}");
    }

    protected override void FillGeneralContainer() {
        AddOptimizeOptionsProperty();
        AddUnlockOptionsProperty();
        AddResourceOptionsProperty();
    }

    protected override void FillHotkeyContainer() {
        base.FillHotkeyContainer();
        OptionPanelHelper.AddGroup(HotkeyContainer, CommonLocalize.OptionPanel_Hotkeys);
        OptionPanelHelper.AddKeymapping(Localization.Localize.AddCash, Config.Instance.AddCash, Localization.Localize.AddCashTooltip);
        OptionPanelHelper.AddKeymapping(CommonLocalize.ShowControlPanel, Config.Instance.ControlPanelHotkey);
        OptionPanelHelper.Reset();
    }
    private UILabel OptionPanelCategoriesHorizontalOffsetMajor;
    private void AddOptimizeOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, Localization.Localize.OptimizeOptions);
        OptionPanelHelper.AddToggle(Config.Instance.EnabledAchievements, Localization.Localize.EnableAchievements, Localization.Localize.AllowsDynamicToggling, _ => {
            Config.Instance.EnabledAchievements = _;
            AchievementsManager.UpdateAchievements(_);
        });
        OptionPanelHelper.AddToggle(Config.Instance.EnabledSkipIntro, Localization.Localize.EnabledSkipIntro, null, _ => Config.Instance.EnabledSkipIntro = _);
        OptionPanelHelper.AddToggle(Config.Instance.OptionPanelCategoriesUpdated, Localization.Localize.OptionPanelCategoriesUpdated, Localization.Localize.OptionPanelCategoriesUpdatedMinor, _ => Config.Instance.OptionPanelCategoriesUpdated = _);
        var slider = OptionPanelHelper.AddSlider(GetOPHorizontalOffsetMajorText(), Localization.Localize.OptionsPanelHorizontalOffsetTooltip, 0, 600f, 5f, Config.Instance.OptionPanelCategoriesHorizontalOffset, new Vector2(700, 16), (_) => {
            Config.Instance.OptionPanelCategoriesHorizontalOffset = (uint)_;
            OptionPanelCategoriesHorizontalOffsetMajor.text = GetOPHorizontalOffsetMajorText();
        });
        OptionPanelCategoriesHorizontalOffsetMajor = slider.MajorLabel;
        OptionPanelHelper.Reset();
    }

    string GetOPHorizontalOffsetMajorText() => Localization.Localize.OptionsPanelHorizontalOffset + ": " + Config.Instance.OptionPanelCategoriesHorizontalOffset.ToString();

    private readonly List<UIComponent> CustomUnlockPanels = new();
    private void AddUnlockOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, Localization.Localize.UnlockOptions);
        UnlockAll = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.EnabledUnlockAll, Localization.Localize.UnlockAll, Localization.Localize.UnlockAllMinor, _ => {
            Config.Instance.EnabledUnlockAll = _;
            if (_) CustomUnlock.IsOn = false;
        }).Child;
        CustomUnlock = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.CustomUnlock, Localization.Localize.CustomUnlock, Localization.Localize.CustomUnlockPanelTooltip, _ => {
            Config.Instance.CustomUnlock = _;
            if (_) UnlockAll.IsOn = false;
            if (CustomUnlockPanels.Count > 0) {
                foreach (var item in CustomUnlockPanels) {
                    item.isEnabled = _;
                }
            }
        }).Child;
        CustomUnlockPanels.Add(OptionPanelHelper.AddDropDown(Localization.Localize.MilestonelevelName_MilestoneUnlockLevel, null, MilestoneLevelNames, Config.Instance.MilestoneLevel, 250, 30, (_) => Config.Instance.MilestoneLevel = _, new RectOffset(20, 0, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(Localization.Localize.EnabledInfoView, null, Config.Instance.EnabledInfoView, _ => Config.Instance.EnabledInfoView = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(Localization.Localize.UnlockAllRoads, null, Config.Instance.UnlockAllRoads, _ => Config.Instance.UnlockAllRoads = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(Localization.Localize.UnlockTransport, null, Config.Instance.UnlockTransport, _ => Config.Instance.UnlockTransport = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(Localization.Localize.UnlockTrainTrack, null, Config.Instance.UnlockTrainTrack, _ => Config.Instance.UnlockTrainTrack = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(Localization.Localize.UnlockMetroTrack, null, Config.Instance.UnlockMetroTrack, _ => Config.Instance.UnlockMetroTrack = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(Localization.Localize.UnlockPolicies, null, Config.Instance.UnlockPolicies, _ => Config.Instance.UnlockPolicies = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(Localization.Localize.UnlockUniqueBuilding, Localization.Localize.UnlockUniqueBuildingMinor, Config.Instance.UnlockUniqueBuilding, _ => Config.Instance.UnlockUniqueBuilding = _, new RectOffset(30, 10, 10, 10), false));
        if (CustomUnlockPanels.Count > 0)
            foreach (var item in CustomUnlockPanels) {
                item.isEnabled = CustomUnlock.IsOn;
            }
        OptionPanelHelper.Reset();
    }

    private readonly List<UIComponent> CashAnarchyPanels = new();
    private UIComponent InitalCashPanel;
    private void AddResourceOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, Localization.Localize.ResourceOptions);
        OptionPanelHelper.AddToggle(Config.Instance.Refund, Localization.Localize.Refund, Localization.Localize.AllowsDynamicToggling, _ => Config.Instance.Refund = _);
        VanillaUnlimitedMoney = OptionPanelHelper.AddToggle(Config.Instance.UnlimitedMoney, Localization.Localize.VanillaUnlimitedMoneyMode, Localization.Localize.VanillaUnlimitedMoneyModeMinor, _ => {
            Config.Instance.UnlimitedMoney = _;
            if (_) CashAnarchy.IsOn = false;
        }).Child as CustomUIToggleButton;
        CashAnarchy = OptionPanelHelper.AddToggle(Config.Instance.CashAnarchy, Localization.Localize.MoneyAnarchyMode, Localization.Localize.MoneyAnarchyModeMinor, _ => {
            Config.Instance.CashAnarchy = _;
            if (_) VanillaUnlimitedMoney.IsOn = false;
            foreach (var item in CashAnarchyPanels) {
                item.isEnabled = _;
            }
        }).Child as CustomUIToggleButton;
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(Localization.Localize.AddCashThreshold, null, Config.Instance.DefaultMinAmount, 100, 100000000, (v) => Config.Instance.DefaultMinAmount = (int)v, majorOffset: new(20, 0, 0, 0)));
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(Localization.Localize.AddCashAmount, null, Config.Instance.DefaultGetCash, 100, 100000000, (_) => Config.Instance.DefaultGetCash = (int)_, majorOffset: new(20, 0, 0, 0)));
        foreach (var item in CashAnarchyPanels) {
            item.isEnabled = Config.Instance.CashAnarchy;
        }
        OptionPanelHelper.AddToggle(Config.Instance.EnabledInitialCash, Localization.Localize.InitialMoney, Localization.Localize.InitialMoneyWarning, _ => {
            Config.Instance.EnabledInitialCash = _;
            InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
        });
        InitalCashPanel = OptionPanelHelper.AddField<UILongValueField, long>(Localization.Localize.Amount, null, Config.Instance.InitialCash, 100, 100000000, (v) => Config.Instance.InitialCash = v, majorOffset: new(20, 0, 0, 0));
        InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
        OptionPanelHelper.Reset();
    }

}



