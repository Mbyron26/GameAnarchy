using ColossalFramework.UI;
using CSShared.Common;
using CSShared.Manager;
using CSShared.UI;
using CSShared.UI.ControlPanel;
using CSShared.UI.OptionPanel;
using GameAnarchy.Managers;
using GameAnarchy.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnarchy;

public class OptionPanel : OptionPanelBase<Mod, Config, OptionPanel> {
    private CustomUIToggleButton VanillaUnlimitedMoney;
    private CustomUIToggleButton CashAnarchy;
    private CustomUIToggleButton UnlockAll;
    private CustomUIToggleButton CustomUnlock;
#if BETA_DEBUG
    protected CustomUIScrollablePanel DebugContainer { get; private set; }
#endif
    private string[] MilestoneLevelNames => new string[] {
            Localize("MilestonelevelName_Vanilla"),
            Localize("MilestonelevelName_LittleHamlet"),
            Localize("MilestonelevelName_WorthyVillage"),
            Localize("MilestonelevelName_TinyTown"),
            Localize("MilestonelevelName_BoomTown"),
            Localize("MilestonelevelName_BusyTown"),
            Localize("MilestonelevelName_BigTown"),
            Localize("MilestonelevelName_SmallCity"),
            Localize("MilestonelevelName_BigCity"),
            Localize("MilestonelevelName_GrandCity"),
            Localize("MilestonelevelName_CapitalCity"),
            Localize("MilestonelevelName_ColossalCity"),
            Localize("MilestonelevelName_Metropolis"),
            Localize("MilestonelevelName_Megalopolis")
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
        OptionPanelHelper.AddLabel($"{Localize("OptionPanel_BuiltinFunction")}", $"{Localize("FastReturn")}");
        OptionPanelHelper.AddLabel($"{Localize("OptionPanel_BuiltinFunction")}", $"{Localize("SortSettings")}");
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
        ManagerPool.GetOrCreateManager<ToolButtonManager>().Disable();
        ManagerPool.GetOrCreateManager<ToolButtonManager>().Enable();
    }

    protected override void FillHotkeyContainer() {
        base.FillHotkeyContainer();
        OptionPanelHelper.AddGroup(HotkeyContainer, Localize("OptionPanel_Hotkeys"));
        OptionPanelHelper.AddKeymapping(Localize("AddMoney"), Config.Instance.AddCash, Localize("AddMoneyTooltip"));
        OptionPanelHelper.AddKeymapping(Localize("DecreaseMoney"), Config.Instance.DecreaseMoney, Localize("AddMoneyTooltip"));
        OptionPanelHelper.AddKeymapping(Localize("ShowControlPanel"), Config.Instance.ControlPanelHotkey);
        OptionPanelHelper.Reset();
    }
    private void AddOptimizeOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, Localize("OptimizeOptions"));
        OptionPanelHelper.AddToggle(Config.Instance.EnabledAchievements, Localize("EnableAchievements"), Localize("AllowsDynamicToggling"), _ => {
            Config.Instance.EnabledAchievements = _;
            ManagerPool.GetOrCreateManager<Manager>().UpdateAchievements();
        });
        OptionPanelHelper.AddToggle(Config.Instance.EnabledSkipIntro, Localize("EnabledSkipIntro"), null, _ => Config.Instance.EnabledSkipIntro = _);
        OptionPanelHelper.AddToggle(Config.Instance.OptionPanelCategoriesUpdated, Localize("OptionPanelCategoriesUpdated"), Localize("OptionPanelCategoriesUpdatedMinor"), _ => Config.Instance.OptionPanelCategoriesUpdated = _);
        OptionPanelHelper.AddSlider(Config.Instance.OptionPanelCategoriesHorizontalOffset.ToString(), Localize("OptionPanelCategoriesHorizontalOffsetMinor"), 0, 600f, 5f, Config.Instance.OptionPanelCategoriesHorizontalOffset, new Vector2(700, 16), (_) => {
            Config.Instance.OptionPanelCategoriesHorizontalOffset = (uint)_;
            ManagerPool.GetOrCreateManager<Manager>().SetCategoriesOffset();
        }, Localize("OptionsPanelHorizontalOffset") + ": ");
        OptionPanelHelper.Reset();
    }

    private readonly List<UIComponent> CustomUnlockPanels = new();
    private void AddUnlockOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, Localize("UnlockOptions"));
        UnlockAll = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.EnabledUnlockAll, Localize("UnlockAll"), Localize("UnlockAllMinor"), _ => {
            Config.Instance.EnabledUnlockAll = _;
            if (_) CustomUnlock.IsOn = false;
        }).Child;
        CustomUnlock = (CustomUIToggleButton)OptionPanelHelper.AddToggle(Config.Instance.CustomUnlock, Localize("CustomUnlock"), Localize("CustomUnlockMinor"), _ => {
            Config.Instance.CustomUnlock = _;
            if (_) UnlockAll.IsOn = false;
            if (CustomUnlockPanels.Count > 0) {
                foreach (var item in CustomUnlockPanels) {
                    item.isEnabled = _;
                }
            }
        }).Child;

        CustomUnlockPanels.Add(OptionPanelHelper.AddDropDown(Localize("MilestonelevelName_MilestoneUnlockLevel"), null, MilestoneLevelNames, Config.Instance.MilestoneLevel, 250, 30, (_) => Config.Instance.MilestoneLevel = _, new RectOffset(20, 0, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockBasicRoads, Localize("UnlockBasicRoads"), Localize("UnlockBasicRoadsMinor"), _ => Config.Instance.UnlockBasicRoads = _, new RectOffset(20, 10, 0, 0), new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockInfoViews, Localize("UnlockInfoViews"), null, _ => Config.Instance.UnlockInfoViews = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockAllRoads, Localize("UnlockAllRoads"), null, _ => Config.Instance.UnlockAllRoads = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockPublicTransport, Localize("UnlockPublicTransport"), null, _ => Config.Instance.UnlockPublicTransport = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockTrainTrack, Localize("UnlockTrainTrack"), null, _ => Config.Instance.UnlockTrainTrack = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockMetroTrack, Localize("UnlockMetroTrack"), null, _ => Config.Instance.UnlockMetroTrack = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockPolicies, Localize("UnlockPolicies"), null, _ => Config.Instance.UnlockPolicies = _, new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockUniqueBuildings, Localize("UnlockUniqueBuilding"), Localize("UnlockUniqueBuildingMinor"), _ => Config.Instance.UnlockUniqueBuildings = _, new RectOffset(20, 10, 0, 0), new RectOffset(20, 10, 0, 0)));
        CustomUnlockPanels.Add(OptionPanelHelper.AddToggle(Config.Instance.UnlockLandscaping, Localize("UnlockLandscaping"), null, _ => Config.Instance.UnlockLandscaping = _, new RectOffset(20, 10, 0, 0)));
        if (CustomUnlockPanels.Count > 0)
            CustomUnlockPanels.ForEach(_ => _.isEnabled = CustomUnlock.IsOn);
        OptionPanelHelper.Reset();
    }

    private readonly List<UIComponent> CashAnarchyPanels = new();
    private UIComponent InitalCashPanel;
    private void AddResourceOptionsProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, Localize("ResourceOptions"));
        VanillaUnlimitedMoney = OptionPanelHelper.AddToggle(Config.Instance.UnlimitedMoney, Localize("VanillaUnlimitedMoneyMode"), Localize("VanillaUnlimitedMoneyModeMinor"), _ => {
            Config.Instance.UnlimitedMoney = _;
            if (_) CashAnarchy.IsOn = false;
        }).Child as CustomUIToggleButton;
        CashAnarchy = OptionPanelHelper.AddToggle(Config.Instance.CashAnarchy, Localize("MoneyAnarchyMode"), Localize("MoneyAnarchyModeMinor"), _ => {
            Config.Instance.CashAnarchy = _;
            if (_) VanillaUnlimitedMoney.IsOn = false;
            foreach (var item in CashAnarchyPanels) {
                item.isEnabled = _;
            }
        }).Child as CustomUIToggleButton;
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(Localize("AddMoneyThreshold"), null, Config.Instance.DefaultMinAmount, 100, 100000000, (v) => Config.Instance.DefaultMinAmount = (int)v, majorOffset: new(20, 0, 0, 0)));
        CashAnarchyPanels.Add(OptionPanelHelper.AddField<UILongValueField, long>(Localize("AddMoneyAmount"), null, Config.Instance.DefaultGetCash, 100, 100000000, (_) => Config.Instance.DefaultGetCash = (int)_, majorOffset: new(20, 0, 0, 0)));
        foreach (var item in CashAnarchyPanels) {
            item.isEnabled = Config.Instance.CashAnarchy;
        }
        OptionPanelHelper.AddToggle(Config.Instance.EnableStartMoney, Localize("StartMoneyMajor"), Localize("StartMoneyMinor"), _ => {
            Config.Instance.EnableStartMoney = _;
            InitalCashPanel.isEnabled = Config.Instance.EnableStartMoney;
        });
        InitalCashPanel = OptionPanelHelper.AddField<UILongValueField, long>(Localize("Amount"), null, Config.Instance.StartMoneyAmount, 100, 100000000, (v) => Config.Instance.StartMoneyAmount = v, majorOffset: new(20, 0, 0, 0));
        InitalCashPanel.isEnabled = Config.Instance.EnableStartMoney;
        OptionPanelHelper.Reset();
    }
    private void AddOtherFunctionProperty() {
        OptionPanelHelper.AddGroup(GeneralContainer, null);
        OptionPanelHelper.AddButton(Localize("OtherFunctionsMajor"), Localize("OtherFunctionsMinor"), Localize("OpenControlPanel"), null, 30, () => {
            ControlPanelManager<Mod, ControlPanel>.CallPanel();
        });
        OptionPanelHelper.Reset();
    }

    protected override void OnModLocaleChanged() => ControlPanelManager<Mod, ControlPanel>.OnLocaleChanged();

}