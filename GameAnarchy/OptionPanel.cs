namespace GameAnarchy;
using ColossalFramework.UI;
using System.Collections.Generic;
using MbyronModsCommon.UI;
using UnityEngine;
using GameAnarchy.UI;
using ModLocalize = Localize;

public class OptionPanel : OptionPanelBase<Mod, Config, OptionPanel> {
    private CustomUIToggleButton VanillaUnlimitedMoney;
    private CustomUIToggleButton CashAnarchy;
    private CustomUIToggleButton UnlockAll;
    private CustomUIToggleButton CustomUnlock;
#if BETA_DEBUG
    protected CustomUIScrollablePanel DebugContainer { get; private set; }
#endif
    private string[] MilestoneLevelNames => new string[] {
            ModLocalize.MilestonelevelName_Vanilla,
            ModLocalize.MilestonelevelName_LittleHamlet,
            ModLocalize.MilestonelevelName_WorthyVillage,
            ModLocalize.MilestonelevelName_TinyTown,
            ModLocalize.MilestonelevelName_BoomTown,
            ModLocalize.MilestonelevelName_BusyTown,
            ModLocalize.MilestonelevelName_BigTown,
            ModLocalize.MilestonelevelName_SmallCity,
            ModLocalize.MilestonelevelName_BigCity,
            ModLocalize.MilestonelevelName_GrandCity,
            ModLocalize.MilestonelevelName_CapitalCity,
            ModLocalize.MilestonelevelName_ColossalCity,
            ModLocalize.MilestonelevelName_Metropolis,
            ModLocalize.MilestonelevelName_Megalopolis
        };
#if BETA_DEBUG
    protected override void AddExtraContainer() {
        base.AddExtraContainer();
        DebugContainer = AddTab("Debug");
        OptionPanelHelper.AddGroup(DebugContainer, "Debug");
        OptionPanelHelper.AddButton("Control panel", null, "Open", 250, 30, () => ControlPanelManager<Mod, ControlPanel>.CallPanel());
        OptionPanelHelper.AddStringField("File dirc", Application.dataPath, null);
        OptionPanelHelper.Reset();
    }
#endif
    protected override void AddExtraModInfoProperty() {
        OptionPanelHelper.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"{ModLocalize.FastReturn}");
        OptionPanelHelper.AddLabel($"{CommonLocalize.OptionPanel_BuiltinFunction}", $"{ModLocalize.SortSettings}");
    }

    protected override void FillGeneralContainer() {
        AddOptimizeOptionsProperty();
        AddUnlockOptionsProperty();
        AddResourceOptionsProperty();
        AddToolButtonOptions<ToolButtonManager>();
        AddOtherFunctionProperty();
    }

    protected override void ToolButtonDropDownCallBack(int value) {
        base.ToolButtonDropDownCallBack(value);
        if (!SingletonMod<Mod>.Instance.IsLevelLoaded) {
            return;
        }
        SingletonTool<ToolButtonManager>.Instance.Disable();
        SingletonTool<ToolButtonManager>.Instance.Enable();
    }

    protected override void FillHotkeyContainer() {
        base.FillHotkeyContainer();
        OptionPanelHelper.AddGroup(HotkeyContainer, CommonLocalize.OptionPanel_Hotkeys);
        OptionPanelHelper.AddKeymapping(ModLocalize.AddMoney, Config.Instance.AddCash, ModLocalize.AddMoneyTooltip);
        OptionPanelHelper.AddKeymapping(ModLocalize.DecreaseMoney, Config.Instance.DecreaseMoney, ModLocalize.AddMoneyTooltip);
        OptionPanelHelper.AddKeymapping(CommonLocalize.ShowControlPanel, Config.Instance.ControlPanelHotkey);
        OptionPanelHelper.Reset();
    }
    private void AddOptimizeOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, ModLocalize.OptimizeOptions);
        OptionPanelHelper.AddToggle(Config.Instance.EnabledAchievements, ModLocalize.EnableAchievements, ModLocalize.AllowsDynamicToggling, _ => {
            Config.Instance.EnabledAchievements = _;
            SingletonManager<Manager>.Instance.UpdateAchievements();
        });
        OptionPanelHelper.AddToggle(Config.Instance.EnabledSkipIntro, ModLocalize.EnabledSkipIntro, null, _ => Config.Instance.EnabledSkipIntro = _);
        OptionPanelHelper.AddToggle(Config.Instance.OptionPanelCategoriesUpdated, ModLocalize.OptionPanelCategoriesUpdated, ModLocalize.OptionPanelCategoriesUpdatedMinor, _ => Config.Instance.OptionPanelCategoriesUpdated = _);
        OptionPanelHelper.AddSlider(Config.Instance.OptionPanelCategoriesHorizontalOffset.ToString(), ModLocalize.OptionPanelCategoriesHorizontalOffsetMinor, 0, 600f, 5f, Config.Instance.OptionPanelCategoriesHorizontalOffset, new Vector2(700, 16), (_) => {
            Config.Instance.OptionPanelCategoriesHorizontalOffset = (uint)_;
            SingletonManager<Manager>.Instance.SetCategoriesOffset();
        }, ModLocalize.OptionsPanelHorizontalOffset + ": ");
        OptionPanelHelper.Reset();
    }

    private readonly List<UIComponent> CustomUnlockPanels = new();
    private void AddUnlockOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, ModLocalize.UnlockOptions);
        UnlockAll = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.EnabledUnlockAll, ModLocalize.UnlockAll, ModLocalize.UnlockAllMinor, _ => {
            Config.Instance.EnabledUnlockAll = _;
            if (_) CustomUnlock.IsOn = false;
        }).Child;
        CustomUnlock = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.CustomUnlock, ModLocalize.CustomUnlock, ModLocalize.CustomUnlockMinor, _ => {
            Config.Instance.CustomUnlock = _;
            if (_) UnlockAll.IsOn = false;
            if (CustomUnlockPanels.Count > 0) {
                foreach (var item in CustomUnlockPanels) {
                    item.isEnabled = _;
                }
            }
        }).Child;

        CustomUnlockPanels.Add(OptionPanelHelper.AddDropDown(ModLocalize.MilestonelevelName_MilestoneUnlockLevel, null, MilestoneLevelNames, Config.Instance.MilestoneLevel, 250, 30, (_) => Config.Instance.MilestoneLevel = _, new RectOffset(20, 0, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockBasicRoads, ModLocalize.UnlockBasicRoads, ModLocalize.UnlockBasicRoadsMinor, _ => Config.Instance.UnlockBasicRoads = _, new RectOffset(20, 10, 0, 0), new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockInfoViews, ModLocalize.UnlockInfoViews, null, _ => Config.Instance.UnlockInfoViews = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockAllRoads, ModLocalize.UnlockAllRoads, null, _ => Config.Instance.UnlockAllRoads = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockPublicTransport, ModLocalize.UnlockPublicTransport, null, _ => Config.Instance.UnlockPublicTransport = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockTrainTrack, ModLocalize.UnlockTrainTrack, null, _ => Config.Instance.UnlockTrainTrack = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockMetroTrack, ModLocalize.UnlockMetroTrack, null, _ => Config.Instance.UnlockMetroTrack = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockPolicies, ModLocalize.UnlockPolicies, null, _ => Config.Instance.UnlockPolicies = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockUniqueBuildings, ModLocalize.UnlockUniqueBuilding, ModLocalize.UnlockUniqueBuildingMinor, _ => Config.Instance.UnlockUniqueBuildings = _, new RectOffset(20, 10, 0, 0), new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockLandscaping, ModLocalize.UnlockLandscaping, null, _ => Config.Instance.UnlockLandscaping = _, new RectOffset(20, 10, 0, 0)));
        if (CustomUnlockPanels.Count > 0)
            CustomUnlockPanels.ForEach(_ => _.isEnabled = CustomUnlock.IsOn);
        OptionPanelHelper.Reset();
    }

    private readonly List<UIComponent> CashAnarchyPanels = new();
    private UIComponent InitalCashPanel;
    private void AddResourceOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, ModLocalize.ResourceOptions);
        VanillaUnlimitedMoney = OptionPanelHelper.AddToggle(Config.Instance.UnlimitedMoney, ModLocalize.VanillaUnlimitedMoneyMode, ModLocalize.VanillaUnlimitedMoneyModeMinor, _ => {
            Config.Instance.UnlimitedMoney = _;
            if (_) CashAnarchy.IsOn = false;
        }).Child as CustomUIToggleButton;
        CashAnarchy = OptionPanelHelper.AddToggle(Config.Instance.CashAnarchy, ModLocalize.MoneyAnarchyMode, ModLocalize.MoneyAnarchyModeMinor, _ => {
            Config.Instance.CashAnarchy = _;
            if (_) VanillaUnlimitedMoney.IsOn = false;
            foreach (var item in CashAnarchyPanels) {
                item.isEnabled = _;
            }
        }).Child as CustomUIToggleButton;
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(ModLocalize.AddMoneyThreshold, null, Config.Instance.DefaultMinAmount, 100, 100000000, (v) => Config.Instance.DefaultMinAmount = (int)v, majorOffset: new(20, 0, 0, 0)));
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(ModLocalize.AddMoneyAmount, null, Config.Instance.DefaultGetCash, 100, 100000000, (_) => Config.Instance.DefaultGetCash = (int)_, majorOffset: new(20, 0, 0, 0)));
        foreach (var item in CashAnarchyPanels) {
            item.isEnabled = Config.Instance.CashAnarchy;
        }
        OptionPanelHelper.AddToggle(Config.Instance.EnableStartMoney, ModLocalize.StartMoneyMajor, ModLocalize.StartMoneyMinor, _ => {
            Config.Instance.EnableStartMoney = _;
            InitalCashPanel.isEnabled = Config.Instance.EnableStartMoney;
        });
        InitalCashPanel = OptionPanelHelper.AddField<UILongValueField, long>(ModLocalize.Amount, null, Config.Instance.StartMoneyAmount, 100, 100000000, (v) => Config.Instance.StartMoneyAmount = v, majorOffset: new(20, 0, 0, 0));
        InitalCashPanel.isEnabled = Config.Instance.EnableStartMoney;
        OptionPanelHelper.Reset();
    }
    private void AddOtherFunctionProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, null);
        OptionPanelHelper.AddButton(ModLocalize.OtherFunctionsMajor, ModLocalize.OtherFunctionsMinor, ModLocalize.OpenControlPanel, null, 30, () => {
            ControlPanelManager<Mod, ControlPanel>.CallPanel();
        });
        OptionPanelHelper.Reset();
    }

    protected override void OnModLocaleChanged() => ControlPanelManager<Mod, ControlPanel>.OnLocaleChanged();

}