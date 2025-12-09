using System.Collections.Generic;
using CSLModsCommon.Localization;
using CSLModsCommon.Manager;
using CSLModsCommon.ToolButton;
using CSLModsCommon.UI.Buttons.RadioButtons;
using CSLModsCommon.UI.Containers;
using CSLModsCommon.UI.DropDown;
using CSLModsCommon.UI.Labels;
using CSLModsCommon.UI.OptionsPanel;
using CSLModsCommon.UI.SettingsCard;
using CSLModsCommon.UI.Utilities;
using GameAnarchy.Data;
using GameAnarchy.Localization;
using GameAnarchy.Managers;
using GameAnarchy.ModSettings;

namespace GameAnarchy.UI;

public partial class OptionsPanel : OptionsPanelBase {
    private static DropDownItem<MilestoneLevel>[] MilestoneLevelDropDownItems => DropDownHelper.FromEnumWithDisplay<MilestoneLevel>(MilestoneLevelNames);

    private ModSetting _modSetting;
    private Label _optionsMainPanelOffsetElement;
    private List<ISettingsCard> _customUnlockCards;
    private List<ISettingsCard> _moneyAnarchyCards;
    private LongFieldCard _startMoneyCard;

    private static string[] MilestoneLevelNames => new[] {
        Translations.Vanilla,
        Translations.LittleHamlet,
        Translations.WorthyVillage,
        Translations.TinyTown,
        Translations.BoomTown,
        Translations.BusyTown,
        Translations.BigTown,
        Translations.SmallCity,
        Translations.BigCity,
        Translations.GrandCity,
        Translations.CapitalCity,
        Translations.ColossalCity,
        Translations.Metropolis,
        Translations.Megalopolis
    };

    protected override void FillKeyBindingPage(ScrollContainer page) {
        var section = AddSection(page);
        section.AddKeyBinding(_modSetting.AddMoneyKeyBinding, Translations.AddMoney, Translations.AddMoneyTooltip);
        section.AddKeyBinding(_modSetting.SubstrateMoneyKeyBinding, Translations.DecreaseMoney, Translations.AddMoneyTooltip);
        section.AddKeyBinding(_modSetting.ControlPanelToggleKeyBinding, SharedTranslations.ToggleControlPanel, SharedTranslations.ToggleControlPanelDescription);
    }

    protected override void FillGeneralPage(ScrollContainer page) {
        base.FillGeneralPage(page);

        #region Built In Section

        var builtInSection = AddSection(page);

        builtInSection.AddLabel(SharedTranslations.BuiltInFunction, Translations.FastReturn);
        builtInSection.AddLabel(SharedTranslations.BuiltInFunction, Translations.SortSettings);

        #endregion

        #region Optimize Section

        var optimizeSection = AddSection(page, Translations.OptimizeOptions);

        optimizeSection.AddToggleSwitch(_modSetting.AchievementSystemEnabled, Translations.EnableAchievements, Translations.AllowsDynamicToggling, (_, b) => {
            _modSetting.AchievementSystemEnabled = b;
            _domain.GetOrCreateManager<AchievementsManager>().UpdateAchievementSystemStatus();
        });
        optimizeSection.AddToggleSwitch(_modSetting.SkipIntroEnabled, Translations.EnabledSkipIntro, null, (_, b) => _modSetting.SkipIntroEnabled = b);
        optimizeSection.AddToggleSwitch(_modSetting.OptionPanelCategoriesUpdated, Translations.OptionPanelCategoriesUpdated, Translations.OptionPanelCategoriesUpdatedMinor, (_, b) => _modSetting.OptionPanelCategoriesUpdated = b);
        _optionsMainPanelOffsetElement = optimizeSection.AddSlider(GetOptionPanelCategoriesHorizontalOffsetLocalized(), Translations.OptionPanelCategoriesHorizontalOffsetMinor, 0, 600f, 5f, _modSetting.OptionPanelCategoriesHorizontalOffset, f => {
                _modSetting.OptionPanelCategoriesHorizontalOffset = (uint)f;
                _domain.GetOrCreateManager<OptionsPanelCategoriesOffsetManager>().SetCategoriesOffset();
                _optionsMainPanelOffsetElement.Text = GetOptionPanelCategoriesHorizontalOffsetLocalized();
            })
            .HeaderElement;

        #endregion

        #region Unlock Section

        var unlockSection = AddSection(page, Translations.UnlockOptions);
        unlockSection.AddRadioGroup(Translations.UnlockMode, Translations.UnlockModeDescription, unlockRadioGroup => {
            RadioGroupLogic<UnlockMode>.Create()
                .AddRange(
                    new RadioButtonItem<UnlockMode>(UnlockMode.Vanilla, unlockRadioGroup.AddOption(Translations.Vanilla)),
                    new RadioButtonItem<UnlockMode>(UnlockMode.UnlockAll, unlockRadioGroup.AddOption(Translations.UnlockAll)),
                    new RadioButtonItem<UnlockMode>(UnlockMode.CustomUnlock, unlockRadioGroup.AddOption(Translations.CustomUnlock))
                )
                .SetDefault(v => v.Value == _modSetting.CurrentUnlockMode)
                .SelectionChanged += OnUnlockModeChanged;
        });

        _customUnlockCards.Add(unlockSection.AddDropDown(Translations.MilestoneUnlockLevel, null, MilestoneLevelDropDownItems, v => v.Value == _modSetting.CurrentMilestoneLevel, t => _modSetting.CurrentMilestoneLevel = t.Value, 250, 30, c => c.HeaderElement.TextPadding.Left = 20));

        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockBasicRoads, Translations.UnlockBasicRoadsMinor, callback: (_, b) => _modSetting.UnlockBasicRoads = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));

        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockInfoViews, Translations.UnlockInfoViews, callback: (_, b) => _modSetting.UnlockInfoViews = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));
        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockAllRoads, Translations.UnlockAllRoads, callback: (_, b) => _modSetting.UnlockAllRoads = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));
        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockPublicTransport, Translations.UnlockPublicTransport, callback: (_, b) => _modSetting.UnlockPublicTransport = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));
        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockTrainTrack, Translations.UnlockTrainTrack, callback: (_, b) => _modSetting.UnlockTrainTrack = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));
        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockMetroTrack, Translations.UnlockMetroTrack, callback: (_, b) => _modSetting.UnlockMetroTrack = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));
        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockPolicies, Translations.UnlockPolicies, callback: (_, b) => _modSetting.UnlockPolicies = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));
        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockUniqueBuildings, Translations.UnlockUniqueBuildingMinor, callback: (_, b) => _modSetting.UnlockUniqueBuildings = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));
        _customUnlockCards.Add(unlockSection.AddCheckBox(_modSetting.UnlockLandscaping, Translations.UnlockLandscaping, callback: (_, b) => _modSetting.UnlockLandscaping = b, beforeLayoutAction: p => p.Control.LayoutPadding.Left = 20));

        foreach (var c in _customUnlockCards) c.Self.isEnabled = _modSetting.CurrentUnlockMode == UnlockMode.CustomUnlock;

        #endregion

        #region Resource Section

        var resourceSection = AddSection(page, Translations.ResourceOptions);

        resourceSection.AddRadioGroup(Translations.MoneyMode, null, unlockRadioGroup => {
            RadioGroupLogic<MoneyMode>.Create()
                .AddRange(
                    new RadioButtonItem<MoneyMode>(MoneyMode.Vanilla, unlockRadioGroup.AddOption(Translations.Vanilla), true),
                    new RadioButtonItem<MoneyMode>(MoneyMode.Unlimited, unlockRadioGroup.AddOption(Translations.Unlimited)),
                    new RadioButtonItem<MoneyMode>(MoneyMode.Anarchy, unlockRadioGroup.AddOption(Translations.Anarchy))
                )
                .SetDefault(v => v.Value == _modSetting.CurrentMoneyMode)
                .SelectionChanged += OnMoneyModeChanged;
        });

        _moneyAnarchyCards.Add(resourceSection.AddLongField(Translations.AddMoneyThreshold, null, _modSetting.DefaultMinAmount, 100, 100000000, (v) => _modSetting.DefaultMinAmount = (int)v, beforeLayoutAction: p => p.HeaderElement.TextPadding.Left = 20));
        _moneyAnarchyCards.Add(resourceSection.AddLongField(Translations.AddMoneyAmount, null, _modSetting.DefaultGetCash, 100, 100000000, (l) => _modSetting.DefaultGetCash = (int)l, beforeLayoutAction: p => p.HeaderElement.TextPadding.Left = 20));

        foreach (var item in _moneyAnarchyCards) item.Self.isEnabled = _modSetting.CurrentMoneyMode == MoneyMode.Anarchy;

        resourceSection.AddToggleSwitch(_modSetting.EnableStartMoney, Translations.StartMoneyMajor, Translations.StartMoneyMinor, (_, b) => {
            _modSetting.EnableStartMoney = b;
            _startMoneyCard.isEnabled = _modSetting.EnableStartMoney;
        });
        _startMoneyCard = resourceSection.AddLongField(Translations.Amount, null, _modSetting.StartMoneyAmount, 100, 100000000, v => _modSetting.StartMoneyAmount = v, beforeLayoutAction: p => p.HeaderElement.TextPadding.Left = 20);
        _startMoneyCard.isEnabled = _modSetting.EnableStartMoney;

        #endregion

        AddInGameToolButtonSection(value => _domain.GetOrCreateManager<InGameToolButtonManager>().OnButtonStatuesChanged(value));
    }

    protected override InGameToolManagerBase GetInGameToolManager() => _domain.GetOrCreateManager<InGameToolButtonManager>();

    protected override void CacheManagers() {
        base.CacheManagers();
        _modSetting = _domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
        _customUnlockCards = new List<ISettingsCard>();
        _moneyAnarchyCards = new List<ISettingsCard>();
    }

    private string GetOptionPanelCategoriesHorizontalOffsetLocalized() => $"{Translations.OptionsPanelHorizontalOffset}: {_modSetting.OptionPanelCategoriesHorizontalOffset}";

    private void OnUnlockModeChanged(RadioButtonItem<UnlockMode> obj) {
        var currentMode = obj.Value;
        _modSetting.CurrentUnlockMode = currentMode;
        if (currentMode == UnlockMode.CustomUnlock)
            foreach (var customUnlockCard in _customUnlockCards)
                customUnlockCard.Self.isEnabled = true;
        else
            foreach (var customUnlockCard in _customUnlockCards)
                customUnlockCard.Self.isEnabled = false;
    }

    private void OnMoneyModeChanged(RadioButtonItem<MoneyMode> obj) {
        var currentMode = obj.Value;
        _modSetting.CurrentMoneyMode = currentMode;
        if (currentMode == MoneyMode.Anarchy)
            foreach (var moneyAnarchyCard in _moneyAnarchyCards)
                moneyAnarchyCard.Self.isEnabled = true;
        else
            foreach (var moneyAnarchyCard in _moneyAnarchyCards)
                moneyAnarchyCard.Self.isEnabled = false;
    }
}