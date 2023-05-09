namespace GameAnarchy;
using ColossalFramework.UI;
using System.Collections.Generic;
using MbyronModsCommon.UI;
using UnityEngine;
using GameAnarchy.UI;

public class OptionPanel : OptionPanelBase<Mod, Config, OptionPanel> {
    private CustomUIToggleButton VanillaUnlimitedMoney;
    private CustomUIToggleButton CashAnarchy;
    private CustomUIToggleButton UnlockAll;
    private CustomUIToggleButton CustomUnlock;
#if DEBUG
    protected CustomUIScrollablePanel DebugContainer { get; private set; }
#endif
    private string[] MilestoneLevelNames => new string[] {
            GameAnarchy.Localize.MilestonelevelName_Vanilla,
            GameAnarchy.Localize.MilestonelevelName_LittleHamlet,
            GameAnarchy.Localize.MilestonelevelName_WorthyVillage,
            GameAnarchy.Localize.MilestonelevelName_TinyTown,
            GameAnarchy.Localize.MilestonelevelName_BoomTown,
            GameAnarchy.Localize.MilestonelevelName_BusyTown,
            GameAnarchy.Localize.MilestonelevelName_BigTown,
            GameAnarchy.Localize.MilestonelevelName_SmallCity,
            GameAnarchy.Localize.MilestonelevelName_BigCity,
            GameAnarchy.Localize.MilestonelevelName_GrandCity,
            GameAnarchy.Localize.MilestonelevelName_CapitalCity,
            GameAnarchy.Localize.MilestonelevelName_ColossalCity,
            GameAnarchy.Localize.MilestonelevelName_Metropolis,
            GameAnarchy.Localize.MilestonelevelName_Megalopolis
        };
#if DEBUG
    protected override void AddExtraContainer() {
        base.AddExtraContainer();
        DebugContainer = AddTab("Debug");
        OptionPanelHelper.AddGroup(DebugContainer, "Debug");
        OptionPanelHelper.AddButton("Control panel", null, "Open", 250, 30, () => ControlPanelManager.HotkeyToggle());
        OptionPanelHelper.AddStringField("File dirc", Application.dataPath, null);
        OptionPanelHelper.Reset();
    }
#endif
    protected override void AddExtraModInfoProperty() {
        OptionPanelHelper.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"{GameAnarchy.Localize.FastReturn}");
        OptionPanelHelper.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"{GameAnarchy.Localize.SortSettings}");
    }

    protected override void FillGeneralContainer() {
        AddOptimizeOptionsProperty();
        AddUnlockOptionsProperty();
        AddResourceOptionsProperty();
        AddOtherFunctionProperty();
    }

    protected override void FillHotkeyContainer() {
        base.FillHotkeyContainer();
        OptionPanelHelper.AddGroup(HotkeyContainer, CommonLocalize.OptionPanel_Hotkeys);
        OptionPanelHelper.AddKeymapping(GameAnarchy.Localize.AddMoney, Config.Instance.AddCash, GameAnarchy.Localize.AddMoneyTooltip);
        OptionPanelHelper.AddKeymapping(CommonLocalize.ShowControlPanel, Config.Instance.ControlPanelHotkey);
        OptionPanelHelper.Reset();
    }
    private void AddOptimizeOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, GameAnarchy.Localize.OptimizeOptions);
        OptionPanelHelper.AddToggle(Config.Instance.EnabledAchievements, GameAnarchy.Localize.EnableAchievements, GameAnarchy.Localize.AllowsDynamicToggling, _ => {
            Config.Instance.EnabledAchievements = _;
            AchievementsManager.UpdateAchievements(_);
        });
        OptionPanelHelper.AddToggle(Config.Instance.EnabledSkipIntro, GameAnarchy.Localize.EnabledSkipIntro, null, _ => Config.Instance.EnabledSkipIntro = _);
        OptionPanelHelper.AddToggle(Config.Instance.OptionPanelCategoriesUpdated, GameAnarchy.Localize.OptionPanelCategoriesUpdated, GameAnarchy.Localize.OptionPanelCategoriesUpdatedMinor, _ => Config.Instance.OptionPanelCategoriesUpdated = _);
        OptionPanelHelper.AddSlider(Config.Instance.OptionPanelCategoriesHorizontalOffset.ToString(), GameAnarchy.Localize.OptionPanelCategoriesHorizontalOffsetMinor, 0, 600f, 5f, Config.Instance.OptionPanelCategoriesHorizontalOffset, new Vector2(700, 16), (_) => {
            Config.Instance.OptionPanelCategoriesHorizontalOffset = (uint)_;
            OptionsPanelCategoriesManager.SetCategoriesOffset();
        }, GameAnarchy.Localize.OptionsPanelHorizontalOffset + ": ");
        OptionPanelHelper.Reset();
    }

    private readonly List<UIComponent> CustomUnlockPanels = new();
    private void AddUnlockOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, GameAnarchy.Localize.UnlockOptions);
        UnlockAll = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.EnabledUnlockAll, GameAnarchy.Localize.UnlockAll, GameAnarchy.Localize.UnlockAllMinor, _ => {
            Config.Instance.EnabledUnlockAll = _;
            if (_) CustomUnlock.IsOn = false;
        }).Child;
        CustomUnlock = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.CustomUnlock, GameAnarchy.Localize.CustomUnlock, GameAnarchy.Localize.CustomUnlockPanelTooltip, _ => {
            Config.Instance.CustomUnlock = _;
            if (_) UnlockAll.IsOn = false;
            if (CustomUnlockPanels.Count > 0) {
                foreach (var item in CustomUnlockPanels) {
                    item.isEnabled = _;
                }
            }
        }).Child;
        CustomUnlockPanels.Add(OptionPanelHelper.AddDropDown(GameAnarchy.Localize.MilestonelevelName_MilestoneUnlockLevel, null, MilestoneLevelNames, Config.Instance.MilestoneLevel, 250, 30, (_) => Config.Instance.MilestoneLevel = _, new RectOffset(20, 0, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(GameAnarchy.Localize.EnabledInfoView, null, Config.Instance.EnabledInfoView, _ => Config.Instance.EnabledInfoView = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(GameAnarchy.Localize.UnlockAllRoads, null, Config.Instance.UnlockAllRoads, _ => Config.Instance.UnlockAllRoads = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(GameAnarchy.Localize.UnlockTransport, null, Config.Instance.UnlockTransport, _ => Config.Instance.UnlockTransport = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(GameAnarchy.Localize.UnlockTrainTrack, null, Config.Instance.UnlockTrainTrack, _ => Config.Instance.UnlockTrainTrack = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(GameAnarchy.Localize.UnlockMetroTrack, null, Config.Instance.UnlockMetroTrack, _ => Config.Instance.UnlockMetroTrack = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(GameAnarchy.Localize.UnlockPolicies, null, Config.Instance.UnlockPolicies, _ => Config.Instance.UnlockPolicies = _, new RectOffset(30, 10, 10, 10), false));
        CustomUnlockPanels.Add(OptionPanelHelper.AddCheckBox(GameAnarchy.Localize.UnlockUniqueBuilding, GameAnarchy.Localize.UnlockUniqueBuildingMinor, Config.Instance.UnlockUniqueBuilding, _ => Config.Instance.UnlockUniqueBuilding = _, new RectOffset(30, 10, 10, 10), false));
        if (CustomUnlockPanels.Count > 0)
            foreach (var item in CustomUnlockPanels) {
                item.isEnabled = CustomUnlock.IsOn;
            }
        OptionPanelHelper.Reset();
    }

    private readonly List<UIComponent> CashAnarchyPanels = new();
    private UIComponent InitalCashPanel;
    private void AddResourceOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, GameAnarchy.Localize.ResourceOptions);
        VanillaUnlimitedMoney = OptionPanelHelper.AddToggle(Config.Instance.UnlimitedMoney, GameAnarchy.Localize.VanillaUnlimitedMoneyMode, GameAnarchy.Localize.VanillaUnlimitedMoneyModeMinor, _ => {
            Config.Instance.UnlimitedMoney = _;
            if (_) CashAnarchy.IsOn = false;
        }).Child as CustomUIToggleButton;
        CashAnarchy = OptionPanelHelper.AddToggle(Config.Instance.CashAnarchy, GameAnarchy.Localize.MoneyAnarchyMode, GameAnarchy.Localize.MoneyAnarchyModeMinor, _ => {
            Config.Instance.CashAnarchy = _;
            if (_) VanillaUnlimitedMoney.IsOn = false;
            foreach (var item in CashAnarchyPanels) {
                item.isEnabled = _;
            }
        }).Child as CustomUIToggleButton;
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(GameAnarchy.Localize.AddMoneyThreshold, null, Config.Instance.DefaultMinAmount, 100, 100000000, (v) => Config.Instance.DefaultMinAmount = (int)v, majorOffset: new(20, 0, 0, 0)));
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(GameAnarchy.Localize.AddMoneyAmount, null, Config.Instance.DefaultGetCash, 100, 100000000, (_) => Config.Instance.DefaultGetCash = (int)_, majorOffset: new(20, 0, 0, 0)));
        foreach (var item in CashAnarchyPanels) {
            item.isEnabled = Config.Instance.CashAnarchy;
        }
        OptionPanelHelper.AddToggle(Config.Instance.EnabledInitialCash, GameAnarchy.Localize.StartMoneyMajor, GameAnarchy.Localize.StartMoneyMinor, _ => {
            Config.Instance.EnabledInitialCash = _;
            InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
        });
        InitalCashPanel = OptionPanelHelper.AddField<UILongValueField, long>(GameAnarchy.Localize.Amount, null, Config.Instance.InitialCash, 100, 100000000, (v) => Config.Instance.InitialCash = v, majorOffset: new(20, 0, 0, 0));
        InitalCashPanel.isEnabled = Config.Instance.EnabledInitialCash;
        OptionPanelHelper.Reset();
    }
    private void AddOtherFunctionProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, null);
        OptionPanelHelper.AddButton(GameAnarchy.Localize.OtherFunctionsMajor, GameAnarchy.Localize.OtherFunctionsMinor, GameAnarchy.Localize.OpenControlPanel, null, 30, () => {
            ControlPanelManager.HotkeyToggle();
        });
        OptionPanelHelper.Reset();
    }

}



