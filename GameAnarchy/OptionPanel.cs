using ColossalFramework.UI;
using MbyronModsCommon;
using UnityEngine;

namespace GameAnarchy {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);

            new OptionPanel_General(panel.AddTabs(CommonLocale.OptionPanel_General, CommonLocale.OptionPanel_General, 0, 30).MainPanel, TypeWidth.ShrinkageWidth);

            new OptionPanel_Hotkey(panel.AddTabs(CommonLocale.OptionPanel_Hotkeys, CommonLocale.OptionPanel_Hotkeys, 0, 30).MainPanel);

            new AdvancedBase<Mod>(panel.AddTabs(CommonLocale.OptionPanel_Advanced, CommonLocale.OptionPanel_Advanced, 0, 30).MainPanel, TypeWidth.ShrinkageWidth);

        }
    }
    public class OptionPanel_Hotkey {
        public OptionPanel_Hotkey(UIComponent component) {
            var optimizeOptions = OptionPanelCard.AddCard(component, TypeWidth.NormalWidth, CommonLocale.OptionPanel_Hotkeys, out _);
            CustomKeymapping.AddKeymapping(optimizeOptions, Localize.AddCash, Config.Instance.AddCash, Localize.AddCashTooltip);

        }
    }
    public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
        public UIPanel InitialCashAmount { get; private set; }
        public UILabel InitialCashWarning { get; private set; }
        public UICheckBox VanillaUnlimitedMoney { get; private set; }
        public UICheckBox CashAnarchy { get; private set; }
        public CustomSliderBase AddCashThreshold { get; private set; }
        public CustomSliderBase AddCashAmount { get; private set; }

        public OptionPanel_General(UIComponent parent, TypeWidth typeWidth) : base(parent, typeWidth) {
            CustomLabel.AddBuiltinFunctionLabel(ModInfo, Localize.FastReturn, true, 1f);
            CustomLabel.AddBuiltinFunctionLabel(ModInfo, Localize.SortSettings, true, 1f);
            AddLocaleDropdown<OptionPanel>(ModInfo);

            var optimizeOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.OptimizeOptions, out _);
            CustomCheckBox.AddCheckBox(optimizeOptions, Localize.EnableAchievements, Config.Instance.EnabledAchievements, 680f, (_) => Config.Instance.EnabledAchievements = _);
            CustomCheckBox.AddCheckBox(optimizeOptions, Localize.EnabledSkipIntro, Config.Instance.EnabledSkipIntro, 680f, (_) => Config.Instance.EnabledSkipIntro = _);
            CustomCheckBox.AddCheckBox(optimizeOptions, Localize.EnabledUnlimitedUniqueBuildings, Config.Instance.EnabledUnlimitedUniqueBuildings, 680f, (_) => Config.Instance.EnabledUnlimitedUniqueBuildings = _);
            CustomSlider.AddCustomSliderStyleA(optimizeOptions, Localize.OptionsPanelHorizontalOffset, 0, 400f, 5f, Config.Instance.OptionPanelCategoriesOffset, (c, _) => Config.Instance.OptionPanelCategoriesOffset = (uint)_);
            CustomLabel.AddLabel(optimizeOptions, Localize.OptionsPanelHorizontalOffsetTooltip, 694f, color: UIColor.Yellow);

            var unlockOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.UnlockOptions, out _);
            var infoViews = CustomCheckBox.AddCheckBox(unlockOptions, Localize.EnabledInfoView, Config.Instance.EnabledInfoView, 680f, (_) => Config.Instance.EnabledInfoView = _, tooltip: Localize.EnabledInfoViewTooltip);
            infoViews.isEnabled = !Config.Instance.EnabledUnlockAll;
            var unlockSetting = CustomCheckBox.AddCheckBox(unlockOptions, Localize.UnlockAll, Config.Instance.EnabledUnlockAll, 680f, (_) => {
                Config.Instance.EnabledUnlockAll = _;
                infoViews.isChecked = Config.Instance.EnabledUnlockAll;
                infoViews.isEnabled = !Config.Instance.EnabledUnlockAll;
            }, tooltip: Localize.EnabledInfoViewTooltip);


            var cashOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.ResourceOptions, out UIPanel cashOptionsTitle);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.UnlimitedOil, Config.Instance.UnlimitedOil, 680f, (_) => Config.Instance.UnlimitedOil = _);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.UnlimitedOre, Config.Instance.UnlimitedOre, 680f, (_) => Config.Instance.UnlimitedOre = _);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.Refund, Config.Instance.Refund, 680f, (_) => Config.Instance.Refund = _);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.InitialCash, Config.Instance.EnabledInitialCash, 680f, (_) => {
                Config.Instance.EnabledInitialCash = _;
                InitialCashWarning.parent.isVisible = InitialCashWarning.isVisible = InitialCashAmount.parent.isVisible = InitialCashAmount.isVisible = Config.Instance.EnabledInitialCash;
            });
            var panel = CustomPanel.AddAutoMatchChildPanel(cashOptions, new RectOffset(30, 0, 0, 0));
            panel.autoLayoutDirection = LayoutDirection.Vertical;
            InitialCashAmount = CustomTextfield.AddLongTypeField(panel, Config.Instance.InitialCash, null, (c, v) => {
                long min = 715000;
                long max = long.MaxValue;
                long defaultValue = Config.Instance.InitialCash;
                if (long.TryParse(v, out var longValue)) {
                    if (longValue < min) longValue = min;
                    if (longValue > max) longValue = max;
                    (c as UITextField).text = longValue.ToString();
                    Config.Instance.InitialCash = longValue;
                } else {
                    (c as UITextField).text = defaultValue.ToString();
                }
            }, null, default, 1f);
            InitialCashAmount.parent.isVisible = InitialCashAmount.isVisible = Config.Instance.EnabledInitialCash;
            InitialCashWarning = CustomLabel.AddLabel(panel, Localize.InitialCashWarning, 600f, 0.8f, color: UIColor.Yellow);
            InitialCashWarning.parent.isVisible = InitialCashWarning.isVisible = Config.Instance.EnabledInitialCash;
            VanillaUnlimitedMoney = CustomCheckBox.AddCheckBox(cashOptions, Localize.VanillaUnlimitedMoney, Config.Instance.UnlimitedMoney, 680f, (_) => {
                Config.Instance.UnlimitedMoney = _;
                if (_) Config.Instance.CashAnarchy = CashAnarchy.isChecked = false;
            });
            CashAnarchy = CustomCheckBox.AddCheckBox(cashOptions, Localize.CashAnarchy, Config.Instance.CashAnarchy, 680f, (_) => {
                Config.Instance.CashAnarchy = _;
                if (_) Config.Instance.UnlimitedMoney = VanillaUnlimitedMoney.isChecked = false;
                AddCashAmount.isVisible = AddCashThreshold.isVisible = Config.Instance.CashAnarchy;
            });
            AddCashThreshold = CustomSlider.AddCustomSliderStyleA(cashOptions, Localize.AddCashThreshold, 10000f, 100000f, 5000f, Config.Instance.DefaultMinAmount, (_, value) => Config.Instance.DefaultMinAmount = (int)value);
            AddCashThreshold.isVisible = Config.Instance.CashAnarchy;
            AddCashAmount = CustomSlider.AddCustomSliderStyleA(cashOptions, Localize.AddCashAmount, 500000f, 8000000f, 100000f, Config.Instance.DefaultGetCash, (_, value) => Config.Instance.DefaultGetCash = (int)value);
            AddCashAmount.isVisible = Config.Instance.CashAnarchy;
            CustomButton.AddGreenButton(cashOptionsTitle, Localize.ResetValue, 130, 28, new UnityEngine.Vector2(598, 6), CashReset);
            void CashReset() {
                AddCashThreshold.SliderValue = 50000;
                AddCashAmount.SliderValue = 5000000;
            }

            var cityServiceOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.CityServiceOptions, out _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.RemoveNoisePollution, Config.Instance.RemoveNoisePollution, 680f, (_) => Config.Instance.RemoveNoisePollution = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.RemoveGroundPollution, Config.Instance.RemoveGroundPollution, 680f, (_) => Config.Instance.RemoveGroundPollution = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.RemoveWaterPollution, Config.Instance.RemoveWaterPollution, 680f, (_) => Config.Instance.RemoveWaterPollution = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.RemoveDeath, Config.Instance.RemoveDeath, 680f, (_) => Config.Instance.RemoveDeath = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.RemoveGarbage, Config.Instance.RemoveGarbage, 680f, (_) => Config.Instance.RemoveGarbage = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.RemoveCrime, Config.Instance.RemoveCrime, 680f, (_) => Config.Instance.RemoveCrime = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.RemoveFire, Config.Instance.RemoveFire, 680f, (_) => Config.Instance.RemoveFire = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.MaximizeAttractiveness, Config.Instance.MaximizeAttractiveness, 680f, (_) => Config.Instance.MaximizeAttractiveness = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.MaximizeEntertainment, Config.Instance.MaximizeEntertainment, 680f, (_) => Config.Instance.MaximizeEntertainment = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.MaximizeLandValue, Config.Instance.MaximizeLandValue, 680f, (_) => Config.Instance.MaximizeLandValue = _);
            CustomCheckBox.AddCheckBox(cityServiceOptions, Localize.MaximizeEducationCoverage, Config.Instance.MaximizeEducationCoverage, 680f, (_) => Config.Instance.MaximizeEducationCoverage = _);

            var incomeOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.IncomeOptions, out _);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.ResidentialMultiplierFactor, 1f, 100f, 1f, Config.Instance.ResidentialMultiplierFactor, (c, value) => Config.Instance.ResidentialMultiplierFactor = (int)value);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.IndustrialMultiplierFactor, 1f, 100f, 1f, Config.Instance.IndustrialMultiplierFactor, (c, value) => Config.Instance.IndustrialMultiplierFactor = (int)value);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.CommercialMultiplierFactor, 1f, 100f, 1f, Config.Instance.CommercialMultiplierFactor, (c, value) => Config.Instance.CommercialMultiplierFactor = (int)value);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.OfficeMultiplierFactor, 1f, 100f, 1f, Config.Instance.OfficeMultiplierFactor, (c, value) => Config.Instance.OfficeMultiplierFactor = (int)value);

        }

    }


    //public sealed class _OptionPanel : ModOptionPanelBase<Mod, Config> {
    //    private UICheckBox InfoViews { get; set; }
    //    private UIPanel InitialCashAmount { get; set; }
    //    private UILabel InitialCashWarning { get; set; }
    //    private UICheckBox VanillaUnlimitedMoney { get; set; }
    //    private UICheckBox CashAnarchy { get; set; }
    //    private CustomSliderBase AddCashThreshold { get; set; }
    //    private CustomSliderBase AddCashAmount { get; set; }


    //    protected override void FillHotkeysContent() {
    //        var hotkeys = HotkeysPanel.AddCard(CommonLocale.OptionPanel_Hotkeys);
    //        hotkeys.AddKeymapping(GameAnarchy.Localize.AddCash, Config.Instance.AddCash, GameAnarchy.Localize.AddCashTooltip);

    //    }
    //    protected override void AddStatusInfo() {
    //        ModInfoCard.AddBuiltinFunctionLabel(GameAnarchy.Localize.FastReturn, true);
    //        ModInfoCard.AddBuiltinFunctionLabel(GameAnarchy.Localize.SortSettings, true);
    //    }
    //    protected override void FillGeneralContent() {
    //        base.FillGeneralContent();
    //        AddOptimizeOptions();
    //        AddUnlockOptions();
    //        AddResourceOptions();
    //        AddCityServiceOptions();
    //        AddIncomeOptions();

    //    }

    //    private void AddIncomeOptions() {
    //        var incomeOptions = GeneralPanel.AddCard(GameAnarchy.Localize.IncomeOptions);
    //        incomeOptions.AddSlider(GameAnarchy.Localize.ResidentialMultiplierFactor, 1f, 100f, 1f, Config.Instance.ResidentialMultiplierFactor, (c, value) => Config.Instance.ResidentialMultiplierFactor = (int)value);
    //        incomeOptions.AddSlider(GameAnarchy.Localize.IndustrialMultiplierFactor, 1f, 100f, 1f, Config.Instance.IndustrialMultiplierFactor, (c, value) => Config.Instance.IndustrialMultiplierFactor = (int)value);
    //        incomeOptions.AddSlider(GameAnarchy.Localize.CommercialMultiplierFactor, 1f, 100f, 1f, Config.Instance.CommercialMultiplierFactor, (c, value) => Config.Instance.CommercialMultiplierFactor = (int)value);
    //        incomeOptions.AddSlider(GameAnarchy.Localize.OfficeMultiplierFactor, 1f, 100f, 1f, Config.Instance.OfficeMultiplierFactor, (c, value) => Config.Instance.OfficeMultiplierFactor = (int)value);
    //    }
    //    private void AddCityServiceOptions() {
    //        var cityServiceOptions = GeneralPanel.AddCard(GameAnarchy.Localize.CityServiceOptions);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.RemoveNoisePollution, Config.Instance.RemoveNoisePollution, (_) => Config.Instance.RemoveNoisePollution = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.RemoveGroundPollution, Config.Instance.RemoveGroundPollution, (_) => Config.Instance.RemoveGroundPollution = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.RemoveWaterPollution, Config.Instance.RemoveWaterPollution, (_) => Config.Instance.RemoveWaterPollution = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.RemoveDeath, Config.Instance.RemoveDeath, (_) => Config.Instance.RemoveDeath = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.RemoveGarbage, Config.Instance.RemoveGarbage, (_) => Config.Instance.RemoveGarbage = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.RemoveCrime, Config.Instance.RemoveCrime, (_) => Config.Instance.RemoveCrime = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.RemoveFire, Config.Instance.RemoveFire, (_) => Config.Instance.RemoveFire = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.MaximizeAttractiveness, Config.Instance.MaximizeAttractiveness, (_) => Config.Instance.MaximizeAttractiveness = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.MaximizeEntertainment, Config.Instance.MaximizeEntertainment, (_) => Config.Instance.MaximizeEntertainment = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.MaximizeLandValue, Config.Instance.MaximizeLandValue, (_) => Config.Instance.MaximizeLandValue = _);
    //        cityServiceOptions.AddCheckBox(GameAnarchy.Localize.MaximizeEducationCoverage, Config.Instance.MaximizeEducationCoverage, (_) => Config.Instance.MaximizeEducationCoverage = _);
    //    }
    //    private void AddResourceOptions() {
    //        var cashOptions = GeneralPanel.AddCard(GameAnarchy.Localize.ResourceOptions);
    //        cashOptions.AddCheckBox(GameAnarchy.Localize.UnlimitedOil, Config.Instance.UnlimitedOil, (_) => Config.Instance.UnlimitedOil = _);
    //        cashOptions.AddCheckBox(GameAnarchy.Localize.UnlimitedOre, Config.Instance.UnlimitedOre, (_) => Config.Instance.UnlimitedOre = _);
    //        cashOptions.AddCheckBox(GameAnarchy.Localize.Refund, Config.Instance.Refund, (_) => Config.Instance.Refund = _);
    //        cashOptions.AddCheckBox(GameAnarchy.Localize.InitialCash, Config.Instance.EnabledInitialCash, (_) => {
    //            Config.Instance.EnabledInitialCash = _;
    //            InitialCashWarning.parent.isVisible = InitialCashWarning.isVisible = InitialCashAmount.parent.isVisible = InitialCashAmount.isVisible = Config.Instance.EnabledInitialCash;
    //        });
    //        var panel = cashOptions.AddSecondLevelContainer();
    //        InitialCashAmount = CustomTextfield.AddLongTypeField(panel, Config.Instance.InitialCash, null, CashAnarchyOnTextSubmitted, null, default, 1f);
    //        InitialCashAmount.isVisible = Config.Instance.EnabledInitialCash;
    //        InitialCashAmount.parent.isVisible = Config.Instance.EnabledInitialCash;
    //        InitialCashWarning = CustomLabel.AddLabel(panel, GameAnarchy.Localize.InitialCashWarning, 0.8f, color: UIColor.Yellow);
    //        InitialCashWarning.isVisible = Config.Instance.EnabledInitialCash;
    //        InitialCashWarning.parent.isVisible = Config.Instance.EnabledInitialCash;
    //        InitialCashWarning.width = 600f;

    //        VanillaUnlimitedMoney = cashOptions.AddCheckBox(GameAnarchy.Localize.VanillaUnlimitedMoney, Config.Instance.UnlimitedMoney, (_) => {
    //            Config.Instance.UnlimitedMoney = _;
    //            if (_) Config.Instance.CashAnarchy = CashAnarchy.isChecked = false;
    //        });

    //        CashAnarchy = cashOptions.AddCheckBox(GameAnarchy.Localize.CashAnarchy, Config.Instance.CashAnarchy, (_) => {
    //            Config.Instance.CashAnarchy = _;
    //            if (_) Config.Instance.UnlimitedMoney = VanillaUnlimitedMoney.isChecked = false;
    //            AddCashAmount.isVisible = AddCashThreshold.isVisible = Config.Instance.CashAnarchy;
    //        });
    //        AddCashThreshold = cashOptions.AddSlider(GameAnarchy.Localize.AddCashThreshold, 10000f, 100000f, 5000f, Config.Instance.DefaultMinAmount, (_, value) => Config.Instance.DefaultMinAmount = (int)value);
    //        AddCashThreshold.isVisible = Config.Instance.CashAnarchy;
    //        AddCashAmount = cashOptions.AddSlider(GameAnarchy.Localize.AddCashAmount, 500000f, 8000000f, 100000f, Config.Instance.DefaultGetCash, (_, value) => Config.Instance.DefaultGetCash = (int)value);
    //        AddCashAmount.isVisible = Config.Instance.CashAnarchy;
    //        cashOptions.AddTitlePanelButton(GameAnarchy.Localize.ResetValue, 130, 28, new UnityEngine.Vector2(598, 6), CashReset);
    //    }
    //    private void CashReset() {
    //        AddCashThreshold.SliderValue = 50000;
    //        AddCashAmount.SliderValue = 5000000;
    //    }

    //    void CashAnarchyOnTextSubmitted(UIComponent component, string value) {
    //        long min = 715000;
    //        long max = long.MaxValue;
    //        long defaultValue = Config.Instance.InitialCash;
    //        if (long.TryParse(value, out var longValue)) {
    //            if (longValue < min) longValue = min;
    //            if (longValue > max) longValue = max;
    //            (component as UITextField).text = longValue.ToString();
    //            Config.Instance.InitialCash = longValue;
    //        } else {
    //            (component as UITextField).text = defaultValue.ToString();
    //        }
    //    }

    //    private void AddUnlockOptions() {
    //        var unlockSetting = GeneralPanel.AddCard(GameAnarchy.Localize.UnlockOptions);
    //        InfoViews = unlockSetting.AddCheckBox(GameAnarchy.Localize.EnabledInfoView, Config.Instance.EnabledInfoView, (_) => Config.Instance.EnabledInfoView = _);
    //        InfoViews.tooltip = GameAnarchy.Localize.EnabledInfoViewTooltip;
    //        InfoViews.isEnabled = !Config.Instance.EnabledUnlockAll;
    //        unlockSetting.AddCheckBox(GameAnarchy.Localize.UnlockAll, Config.Instance.EnabledUnlockAll, (_) => {
    //            Config.Instance.EnabledUnlockAll = _;
    //            InfoViews.isChecked = Config.Instance.EnabledUnlockAll;
    //            InfoViews.isEnabled = !Config.Instance.EnabledUnlockAll;
    //        });
    //    }
    //    private void AddOptimizeOptions() {
    //        var gameExtension = GeneralPanel.AddCard(GameAnarchy.Localize.OptimizeOptions);
    //        gameExtension.AddCheckBox(GameAnarchy.Localize.EnableAchievements, Config.Instance.EnabledAchievements, (_) => Config.Instance.EnabledAchievements = _);
    //        gameExtension.AddCheckBox(GameAnarchy.Localize.EnabledSkipIntro, Config.Instance.EnabledSkipIntro, (_) => Config.Instance.EnabledSkipIntro = _);
    //        gameExtension.AddCheckBox(GameAnarchy.Localize.EnabledUnlimitedUniqueBuildings, Config.Instance.EnabledUnlimitedUniqueBuildings, (_) => Config.Instance.EnabledUnlimitedUniqueBuildings = _);
    //        gameExtension.AddSlider(GameAnarchy.Localize.OptionsPanelHorizontalOffset, 0, 400f, 5f, Config.Instance.OptionPanelCategoriesOffset, (c, _) => Config.Instance.OptionPanelCategoriesOffset = (uint)_);
    //        gameExtension.AddLabel(GameAnarchy.Localize.OptionsPanelHorizontalOffsetTooltip, color: UIColor.Yellow);
    //    }

    //}
}
