using ColossalFramework.UI;
using MbyronModsCommon;
using UnityEngine;

namespace GameAnarchy {
    public class OptionPanel : UIPanel {
        public OptionPanel() {
            var panel = CustomTabs.AddCustomTabs(this);
            new OptionPanel_General(panel.AddTabs(CommonLocale.OptionPanel_General, CommonLocale.OptionPanel_General, 0, 30).MainPanel, TypeWidth.ShrinkageWidth);
            new OptionPanel_Hotkey(panel.AddTabs(CommonLocale.OptionPanel_Hotkeys, CommonLocale.OptionPanel_Hotkeys, 0, 30).MainPanel);
            new AdvancedBase<Mod>(panel.AddTabs(CommonLocale.OptionPanel_Advanced, CommonLocale.OptionPanel_Advanced, 0, 30).MainPanel, TypeWidth.NormalWidth);

        }
    }
    public class OptionPanel_Hotkey {
        public OptionPanel_Hotkey(UIComponent component) {
            var optimizeOptions = OptionPanelCard.AddCard(component, TypeWidth.NormalWidth, CommonLocale.OptionPanel_Hotkeys, out _);
            CustomKeymapping.AddKeymapping(optimizeOptions, Localize.AddCash, Config.Instance.AddCash, Localize.AddCashTooltip);
        }
    }
    public class OptionPanel_General : GeneralOptionsBase<Mod, Config> {
        private UIPanel InitialCashAmount { get; set; }
        private UILabel InitialCashWarning { get; set; }
        private UICheckBox VanillaUnlimitedMoney { get; set; }
        private UICheckBox CashAnarchy { get; set; }
        private CustomSliderBase AddCashThreshold { get; set; }
        private CustomSliderBase AddCashAmount { get; set; }
        private UICheckBox UnlockAll { get; set; }
        private UICheckBox CustomUnlock { get; set; }
        private UIPanel CustomUnlockPanel { get; set; }
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

            #region ModInfo
            CustomLabel.AddBuiltinFunctionLabel(ModInfo, Localize.FastReturn, true, 1f);
            CustomLabel.AddBuiltinFunctionLabel(ModInfo, Localize.SortSettings, true, 1f);
            AddLocaleDropdown<OptionPanel>(ModInfo);
            #endregion

            #region OptimizeOptions
            var optimizeOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.OptimizeOptions, out _);
            CustomCheckBox.AddCheckBox(optimizeOptions, Localize.EnableAchievements, Config.Instance.EnabledAchievements, 680f, (_) => Config.Instance.EnabledAchievements = _);
            CustomCheckBox.AddCheckBox(optimizeOptions, Localize.EnabledSkipIntro, Config.Instance.EnabledSkipIntro, 680f, (_) => Config.Instance.EnabledSkipIntro = _);
            CustomCheckBox.AddCheckBox(optimizeOptions, Localize.EnabledUnlimitedUniqueBuildings, Config.Instance.EnabledUnlimitedUniqueBuildings, 680f, (_) => Config.Instance.EnabledUnlimitedUniqueBuildings = _);
            CustomSlider.AddCustomSliderStyleA(optimizeOptions, Localize.OptionsPanelHorizontalOffset, 0, 400f, 5f, Config.Instance.OptionPanelCategoriesOffset, (c, _) => Config.Instance.OptionPanelCategoriesOffset = (uint)_);
            CustomLabel.AddLabel(optimizeOptions, Localize.OptionsPanelHorizontalOffsetTooltip, 694f, color: UIColor.Yellow);
            #endregion

            #region UnlockOptions
            var unlockOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.UnlockOptions, out _);
            UnlockAll = CustomCheckBox.AddCheckBox(unlockOptions, Localize.UnlockAll, Config.Instance.EnabledUnlockAll, 680f, (_) => {
                Config.Instance.EnabledUnlockAll = _;
                if (Config.Instance.EnabledUnlockAll) {
                    CustomUnlock.isChecked = false;
                }
            });
            CustomUnlock = CustomCheckBox.AddCheckBox(unlockOptions, Localize.CustomUnlock, Config.Instance.CustomUnlock, 680f, (_) => {
                CustomUnlockPanel.isEnabled = Config.Instance.CustomUnlock = _;
                if (Config.Instance.CustomUnlock) {
                    UnlockAll.isChecked = false;
                }
            });
            CustomUnlockPanel = CustomPanel.AddAutoMatchChildPanel(unlockOptions, new RectOffset(25, 0, 0, 3));
            CustomUnlockPanel.autoLayoutDirection = LayoutDirection.Vertical;
            CustomUnlockPanel.isEnabled = CustomUnlock.isChecked;
            CustomUnlockPanel.tooltip = Localize.CustomUnlockPanelTooltip;
            var MilestonelevelLevel = CustomDropdown.AddDropdown(CustomUnlockPanel, Localize.MilestonelevelName_MilestoneUnlockLevel, 1f, MilestoneLevelNames, Config.Instance.MilestoneLevel, 170f, 32, 1f, new RectOffset(10, 10, 8, 0), new RectOffset(6, 6, 4, 0));
            MilestonelevelLevel.eventSelectedIndexChanged += (c, value) => {
                Config.Instance.MilestoneLevel = value;
            };
            var infoViews = CustomCheckBox.AddCheckBox(CustomUnlockPanel, Localize.EnabledInfoView, Config.Instance.EnabledInfoView, 670f, (_) => Config.Instance.EnabledInfoView = _);
            var unlockBase = CustomCheckBox.AddCheckBox(CustomUnlockPanel, Localize.UnlockAllRoads, Config.Instance.UnlockAllRoads, 670f, (_) => {
                Config.Instance.UnlockAllRoads = _;
            });
            var unlockTrainTrack = CustomCheckBox.AddCheckBox(CustomUnlockPanel, Localize.UnlockTrainTrack, Config.Instance.UnlockTrainTrack, 670f, (_) => Config.Instance.UnlockTrainTrack = _);
            var unlockMetroTrack = CustomCheckBox.AddCheckBox(CustomUnlockPanel, Localize.UnlockMetroTrack, Config.Instance.UnlockMetroTrack, 670f, (_) => Config.Instance.UnlockMetroTrack = _);
            var unlock = CustomCheckBox.AddCheckBox(CustomUnlockPanel, Localize.UnlockPolicies, Config.Instance.UnlockPolicies, 670f, (_) => Config.Instance.UnlockPolicies = _);
            #endregion

            #region CashOptions
            var cashOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.ResourceOptions, out UIPanel cashOptionsTitle);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.UnlimitedOil, Config.Instance.UnlimitedOil, 680f, (_) => Config.Instance.UnlimitedOil = _);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.UnlimitedOre, Config.Instance.UnlimitedOre, 680f, (_) => Config.Instance.UnlimitedOre = _);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.Refund, Config.Instance.Refund, 680f, (_) => Config.Instance.Refund = _);
            CustomCheckBox.AddCheckBox(cashOptions, Localize.InitialCash, Config.Instance.EnabledInitialCash, 680f, (_) => {
                Config.Instance.EnabledInitialCash = _;
                InitialCashWarning.parent.isVisible = InitialCashWarning.isVisible = InitialCashAmount.parent.isVisible = InitialCashAmount.isVisible = Config.Instance.EnabledInitialCash;
            });
            var panel = CustomPanel.AddAutoMatchChildPanel(cashOptions, new RectOffset(25, 0, 0, 3));
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
            #endregion

            #region CityServiceOptions
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
            #endregion

            #region IncomeOptions
            var incomeOptions = OptionPanelCard.AddCard(parent, typeWidth, Localize.IncomeOptions, out _);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.ResidentialMultiplierFactor, 1f, 100f, 1f, Config.Instance.ResidentialMultiplierFactor, (c, value) => Config.Instance.ResidentialMultiplierFactor = (int)value);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.IndustrialMultiplierFactor, 1f, 100f, 1f, Config.Instance.IndustrialMultiplierFactor, (c, value) => Config.Instance.IndustrialMultiplierFactor = (int)value);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.CommercialMultiplierFactor, 1f, 100f, 1f, Config.Instance.CommercialMultiplierFactor, (c, value) => Config.Instance.CommercialMultiplierFactor = (int)value);
            CustomSlider.AddCustomSliderStyleA(incomeOptions, Localize.OfficeMultiplierFactor, 1f, 100f, 1f, Config.Instance.OfficeMultiplierFactor, (c, value) => Config.Instance.OfficeMultiplierFactor = (int)value);
            #endregion

        }


    }

}
