using ColossalFramework.UI;
using CSLModsCommon.Collections;
using CSLModsCommon.Localization;
using CSLModsCommon.Manager;
using CSLModsCommon.UI.Buttons;
using CSLModsCommon.UI.Containers;
using CSLModsCommon.UI.ControlPanel;
using CSLModsCommon.UI.Labels;
using CSLModsCommon.UI.SettingsCard;
using GameAnarchy.Data;
using GameAnarchy.Localization;
using GameAnarchy.Managers;
using GameAnarchy.ModSettings;
using UnityEngine;

namespace GameAnarchy.UI;

public class ModControlPanel : ControlPanelBase {
    private const string General = nameof(General);
    private const string Service = nameof(Service);
    private const string Economy = nameof(Economy);

    private static string SelectedTab = General;

    private ModSetting _modSetting;
    private FireControlManager _fireControlManager;
    private InGameToolButtonManager _inGameToolButtonManager;
    private ScrollContainer _generalPage;
    private Label _oilDepletionRateHeaderElement;
    private Label _oreDepletionRateHeaderElement;
    private ScrollContainer _servicePage;
    private Label _buildingSpreadFireProbabilityCardHeaderElement;
    private Label _treeSpreadFireProbabilityCardHeaderElement;
    private ScrollContainer _economyPage;
    private Label _annualInterestRateMinorCardHeaderElement;
    private SliderCard _annualInterestRateMinorCard;
    private ReusableList<ISettingsCard> _buildingRefundOptions;
    private Label _buildingRefundMultipleFactorSliderHeaderElement;
    private ReusableList<ISettingsCard> _segmentRefundOptions;
    private Label _segmentRefundMultipleFactorSliderCardHeaderElement;
    private Label _relocateBuildingSliderCardHeaderElement;
    private ToggleSwitchIndicator _unlimitedMonumentElement;
    private ToggleSwitchIndicator _unlimitedMainCampusBuildingElement;
    private ToggleSwitchIndicator _unlimitedUniqueFactoryElement;
    private ToggleSwitchIndicator _unlimitedStockExchangeElement;
    private ToggleSwitchIndicator _unlimitedUniqueFacultyElement;
    private ToggleSwitchIndicator _unlimitedWeatherRadarElement;
    private ToggleSwitchIndicator _unlimitedSpaceRadarElement;
    private ToggleSwitchIndicator _unlimitedFestivalAreaElement;
    private ToggleSwitchIndicator _unlimitedLibraryAIMinorElement;
    private ToggleSwitchIndicator _unlimitedSpaceElevatorElement;
    private ToggleSwitchIndicator _unlimitedParkAIElement;
    private ToggleSwitchIndicator _removeDeathElement;
    private ToggleSwitchIndicator _removeCrimeElement;
    private ToggleSwitchIndicator _removeGarbageElement;
    private ToggleSwitchIndicator _maximizeAttractivenessElement;
    private ToggleSwitchIndicator _maximizeEntertainmentElement;
    private ToggleSwitchIndicator _maximizeLandValueElement;
    private ToggleSwitchIndicator _maximizeEducationCoverageElement;
    private ToggleSwitchIndicator _maximizeFireCoverageElement;
    private ToggleSwitchIndicator _removePlayerBuildingFireElement;
    private ToggleSwitchIndicator _removeResidentialBuildingFireElement;
    private ToggleSwitchIndicator _removeIndustrialBuildingFireElement;
    private ToggleSwitchIndicator _removeCommercialBuildingFireElement;
    private ToggleSwitchIndicator _removeOfficeBuildingFireElement;
    private ToggleSwitchIndicator _removeParkBuildingFireElement;
    private ToggleSwitchIndicator _removeMuseumFireElement;
    private ToggleSwitchIndicator _removeCampusBuildingFireElement;
    private ToggleSwitchIndicator _removeAirportBuildingFireElement;

    protected override void OnCloseButtonClicked(UIComponent component, UIMouseEventParameter eventParam) => _inGameToolButtonManager.OnPanelClosed();

    protected override void CacheManagers() {
        base.CacheManagers();
        _modSetting = _domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
        _fireControlManager = _domain.GetOrCreateManager<FireControlManager>();
        _domain.GetOrCreateManager<ControlPanelManager>();
        _inGameToolButtonManager = _domain.GetOrCreateManager<InGameToolButtonManager>();
    }

    protected override void OnAwake() {
        base.OnAwake();
        _buildingRefundOptions = ReusableList<ISettingsCard>.Rent();
        _segmentRefundOptions = ReusableList<ISettingsCard>.Rent();
        _tabBar.TabClicked += OnTabBarClicked;

        FillGeneralPage();
        FillServicePage();
        FillEconomyPage();
    }

    public override void OnDestroy() {
        base.OnDestroy();
        _buildingRefundOptions.Return();
        _segmentRefundOptions.Return();
    }

    protected override void SetSelectedTab() {
        base.SetSelectedTab();
        _tabGroupLogic.SelectPage(SelectedTab);
    }

    private void FillEconomyPage() {
        _economyPage = AddPage(Economy, Translations.Economy);

        #region Economy

        var economySection = AddSection(_economyPage, Translations.Economy);

        var itemPanel0 = economySection.AddToggleSwitch(_modSetting.RemoveNotEnoughMoney || _modSetting.CurrentMoneyMode == MoneyMode.Unlimited || _modSetting.CurrentMoneyMode == MoneyMode.Anarchy, Translations.RemoveNotEnoughMoney, Translations.RemoveNotEnoughMoneyMinor, (_, b) => _modSetting.RemoveNotEnoughMoney = b);
        itemPanel0.Self.isEnabled = _modSetting.CurrentMoneyMode == MoneyMode.Vanilla;

        economySection.AddIntField(Translations.CityBankruptcyWarningThreshold, Translations.CityBankruptcyWarningThresholdMinor, _modSetting.CityBankruptcyWarningThreshold, int.MinValue / 100, 0, 100, i => _modSetting.CityBankruptcyWarningThreshold = i);

        economySection.AddToggleSwitch(_modSetting.ChargeInterest, Translations.ChargeInterest, null, (_, b) => {
            _modSetting.ChargeInterest = b;
            _annualInterestRateMinorCard.isEnabled = _modSetting.ChargeInterest;
        });

        var annualInterestRateMinorCard = economySection.AddSlider(GetAnnualInterestRate(), Translations.AnnualInterestRateMinor, 0, 0.36f, 0.0025f, _modSetting.AnnualInterestRate, new Vector2(388, 16), f => {
            _modSetting.AnnualInterestRate = f;
            _annualInterestRateMinorCardHeaderElement.Text = GetAnnualInterestRate();
        });
        _annualInterestRateMinorCardHeaderElement = annualInterestRateMinorCard.HeaderElement;
        _annualInterestRateMinorCard = annualInterestRateMinorCard;
        _annualInterestRateMinorCard.isEnabled = _modSetting.ChargeInterest;

        economySection.AddToggleSwitch(_modSetting.NoPoliciesCosts, Translations.NoPoliciesCosts, Translations.NoPoliciesCostsMinor, (_, b) => _modSetting.NoPoliciesCosts = b);

        #endregion

        #region IncomeMultiplier

        var incomeMultiplierSection = AddSection(_economyPage, Translations.IncomeMultiplier);
        incomeMultiplierSection.AddIntField(Translations.Residential, null, _modSetting.ResidentialMultiplierFactor, 1, 100, 10, i => _modSetting.ResidentialMultiplierFactor = i);
        incomeMultiplierSection.AddIntField(Translations.Industrial, null, _modSetting.IndustrialMultiplierFactor, 1, 100, 10, i => _modSetting.IndustrialMultiplierFactor = i);
        incomeMultiplierSection.AddIntField(Translations.Commercial, null, _modSetting.CommercialMultiplierFactor, 1, 100, 10, i => _modSetting.CommercialMultiplierFactor = i);
        incomeMultiplierSection.AddIntField(Translations.Office, null, _modSetting.OfficeMultiplierFactor, 1, 100, 10, i => _modSetting.OfficeMultiplierFactor = i);

        #endregion

        #region Refund

        var refundSection = AddSection(_economyPage, Translations.Refund);
        refundSection.AddToggleSwitch(_modSetting.BuildingRefund, Translations.BuildingRefund, null, (_, b) => {
            _modSetting.BuildingRefund = b;
            foreach (var item in _buildingRefundOptions)
                item.Self.isEnabled = _modSetting.BuildingRefund;
        });

        _buildingRefundOptions.Add(refundSection.AddToggleSwitch(_modSetting.RemoveBuildingRefundTimeLimitation, Translations.RemoveBuildingRefundTimeLimitation, null, (_, b) => _modSetting.RemoveBuildingRefundTimeLimitation = b));

        _buildingRefundOptions.Add(refundSection.AddSlider(GetRefundMultipleFactor(Translations.BuildingRefundMultipleFactor, _modSetting.BuildingRefundMultipleFactor), null, 0, 2, 0.25f, _modSetting.BuildingRefundMultipleFactor, new Vector2(388, 16), f => {
            _modSetting.BuildingRefundMultipleFactor = f;
            _buildingRefundMultipleFactorSliderHeaderElement.Text = GetRefundMultipleFactor(Translations.BuildingRefundMultipleFactor, _modSetting.BuildingRefundMultipleFactor);
        }, beforeLayoutAction: s => _buildingRefundMultipleFactorSliderHeaderElement = s.HeaderElement));

        foreach (var item in _buildingRefundOptions)
            item.Self.isEnabled = _modSetting.BuildingRefund;

        refundSection.AddToggleSwitch(_modSetting.SegmentRefund, Translations.SegmentRefund, null, (_, b) => {
            _modSetting.SegmentRefund = b;
            foreach (var item in _segmentRefundOptions)
                item.Self.isEnabled = _modSetting.SegmentRefund;
        });

        _segmentRefundOptions.Add(refundSection.AddToggleSwitch(_modSetting.RemoveSegmentRefundTimeLimitation, Translations.RemoveSegmentRefundTimeLimitation, null, (_, b) => _modSetting.RemoveSegmentRefundTimeLimitation = b));

        _segmentRefundOptions.Add(refundSection.AddSlider(GetRefundMultipleFactor(Translations.SegmentRefundMultipleFactor, _modSetting.SegmentRefundMultipleFactor), null, 0, 2, 0.25f, _modSetting.SegmentRefundMultipleFactor, new Vector2(388, 16), f => {
            _modSetting.SegmentRefundMultipleFactor = f;
            _segmentRefundMultipleFactorSliderCardHeaderElement.Text = GetRefundMultipleFactor(Translations.SegmentRefundMultipleFactor, _modSetting.SegmentRefundMultipleFactor);
        }, beforeLayoutAction: s => _segmentRefundMultipleFactorSliderCardHeaderElement = s.HeaderElement));

        foreach (var item in _segmentRefundOptions)
            item.Self.isEnabled = _modSetting.SegmentRefund;

        #endregion

        #region RelocateBuilding

        var relocateBuildingSection = AddSection(_economyPage, Translations.RelocateBuilding);

        relocateBuildingSection.AddSlider(GetRelocateCost(), Translations.RelocateBuildingMinor, 0, 1, 0.05f, _modSetting.BuildingRelocationCostFactor, new Vector2(388, 16), f => {
            _modSetting.BuildingRelocationCostFactor = f;
            _relocateBuildingSliderCardHeaderElement.Text = GetRelocateCost();
        }, beforeLayoutAction: s => _relocateBuildingSliderCardHeaderElement = s.HeaderElement);

        #endregion
    }

    private void FillServicePage() {
        _servicePage = AddPage(Service, Translations.Service);

        #region RemovePollution

        var removePollutionSection = AddSection(_servicePage, Translations.RemovePollution);
        removePollutionSection.AddToggleSwitch(_modSetting.RemoveNoisePollution, Translations.NoisePollution, null, v => _modSetting.RemoveNoisePollution = v);
        removePollutionSection.AddToggleSwitch(_modSetting.RemoveGroundPollution, Translations.GroundPollution, null, v => _modSetting.RemoveGroundPollution = v);
        removePollutionSection.AddToggleSwitch(_modSetting.RemoveWaterPollution, Translations.WaterPollution, null, v => _modSetting.RemoveWaterPollution = v);

        #endregion

        #region CityServiceOptions

        var cityServiceOptionsSection = AddSection(_servicePage, Translations.CityServiceOptions);

        cityServiceOptionsSection.AddButtons(Translations.EnableOrDisableAll, null, v => {
            v.RegisterButton(Translations.Enable, OnCityServiceOptionsEnableAllClicked);
            v.RegisterButton(Translations.Disable, OnCityServiceOptionsDisableAllClicked);
        });
        _removeDeathElement = cityServiceOptionsSection.AddToggleSwitch(_modSetting.RemoveDeath, Translations.RemoveDeath, null, v => _modSetting.RemoveDeath = v).Control;
        _removeCrimeElement = cityServiceOptionsSection.AddToggleSwitch(_modSetting.RemoveCrime, Translations.RemoveCrime, null, v => _modSetting.RemoveCrime = v).Control;
        _removeGarbageElement = cityServiceOptionsSection.AddToggleSwitch(_modSetting.RemoveGarbage, Translations.RemoveGarbage, null, v => _modSetting.RemoveGarbage = v).Control;
        _maximizeAttractivenessElement = cityServiceOptionsSection.AddToggleSwitch(_modSetting.MaximizeAttractiveness, Translations.MaximizeAttractiveness, null, v => _modSetting.MaximizeAttractiveness = v).Control;
        _maximizeEntertainmentElement = cityServiceOptionsSection.AddToggleSwitch(_modSetting.MaximizeEntertainment, Translations.MaximizeEntertainment, null, v => _modSetting.MaximizeEntertainment = v).Control;
        _maximizeLandValueElement = cityServiceOptionsSection.AddToggleSwitch(_modSetting.MaximizeLandValue, Translations.MaximizeLandValue, null, v => _modSetting.MaximizeLandValue = v).Control;
        _maximizeEducationCoverageElement = cityServiceOptionsSection.AddToggleSwitch(_modSetting.MaximizeEducationCoverage, Translations.MaximizeEducationCoverage, null, v => _modSetting.MaximizeEducationCoverage = v).Control;

        #endregion

        #region FireControl

        var fireControlSection = AddSection(_servicePage, Translations.FireControl);
        fireControlSection.AddButton(Translations.PutOutBurningBuildings, null, Translations.PutOut, null, 24, _ => _fireControlManager.PutOutBurningBuildingsButtonClicked());

        fireControlSection.AddButtons(Translations.EnableOrDisableAll, null, v => {
            v.RegisterButton(Translations.Enable, OnFireControlEnableAllClicked);
            v.RegisterButton(Translations.Disable, OnFireControlDisableAllClicked);
        });
        _maximizeFireCoverageElement = fireControlSection.AddToggleSwitch(_modSetting.MaximizeFireCoverage, Translations.MaximizeFireCoverage, null, v => _modSetting.MaximizeFireCoverage = v).Control;
        _removePlayerBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemovePlayerBuildingFire, Translations.RemovePlayerBuildingFire, null, v => _modSetting.RemovePlayerBuildingFire = v).Control;
        _removeResidentialBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveResidentialBuildingFire, Translations.RemoveResidentialBuildingFire, null, v => _modSetting.RemoveResidentialBuildingFire = v).Control;
        _removeIndustrialBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveIndustrialBuildingFire, Translations.RemoveIndustrialBuildingFire, null, v => _modSetting.RemoveIndustrialBuildingFire = v).Control;
        _removeCommercialBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveCommercialBuildingFire, Translations.RemoveCommercialBuildingFire, null, v => _modSetting.RemoveCommercialBuildingFire = v).Control;
        _removeOfficeBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveOfficeBuildingFire, Translations.RemoveOfficeBuildingFire, null, v => _modSetting.RemoveOfficeBuildingFire = v).Control;
        _removeParkBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveParkBuildingFire, Translations.RemoveParkBuildingFire, null, v => _modSetting.RemoveParkBuildingFire = v).Control;
        _removeMuseumFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveMuseumFire, Translations.RemoveMuseumFire, null, v => _modSetting.RemoveMuseumFire = v).Control;
        _removeCampusBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveCampusBuildingFire, Translations.RemoveCampusBuildingFire, null, v => _modSetting.RemoveCampusBuildingFire = v).Control;
        _removeAirportBuildingFireElement = fireControlSection.AddToggleSwitch(_modSetting.RemoveAirportBuildingFire, Translations.RemoveAirportBuildingFire, null, v => _modSetting.RemoveAirportBuildingFire = v).Control;

        var buildingSpreadFireProbabilityCard = fireControlSection.AddSlider(GetString(Translations.BuildingSpreadFireProbability, _modSetting.BuildingSpreadFireProbability, Translations.NoSpreadFire, Translations.Vanilla), null, 0, 100, 1, _modSetting.BuildingSpreadFireProbability, new Vector2(388, 16), v => {
            _modSetting.BuildingSpreadFireProbability = (uint)v;
            _buildingSpreadFireProbabilityCardHeaderElement.Text = GetString(Translations.BuildingSpreadFireProbability, _modSetting.BuildingSpreadFireProbability, Translations.NoSpreadFire, Translations.Vanilla);
        }, true);
        _buildingSpreadFireProbabilityCardHeaderElement = buildingSpreadFireProbabilityCard.HeaderElement;

        var treeSpreadFireProbabilityCard = fireControlSection.AddSlider(GetString(Translations.TreeSpreadFireProbability, _modSetting.TreeSpreadFireProbability, Translations.NoSpreadFire, Translations.Vanilla), null, 0, 100, 1, _modSetting.TreeSpreadFireProbability, new Vector2(388, 16), v => {
            _modSetting.TreeSpreadFireProbability = (uint)v;
            _treeSpreadFireProbabilityCardHeaderElement.Text = GetString(Translations.TreeSpreadFireProbability, _modSetting.TreeSpreadFireProbability, Translations.NoSpreadFire, Translations.Vanilla);
        }, true);
        _treeSpreadFireProbabilityCardHeaderElement = treeSpreadFireProbabilityCard.HeaderElement;

        #endregion
    }

    private void FillGeneralPage() {
        _generalPage = AddPage(General, SharedTranslations.General);

        #region UnlimitedUniqueBuildings

        var unlimitedUniqueBuildingsSection = AddSection(_generalPage, Translations.EnabledUnlimitedUniqueBuildings);

        unlimitedUniqueBuildingsSection.AddButtons(Translations.EnableOrDisableAll, null, v => {
            v.RegisterButton(Translations.Enable, OnUnlimitedUniqueBuildingsEnableAllClicked);
            v.RegisterButton(Translations.Disable, OnUnlimitedUniqueBuildingsDisableAllClicked);
        });

        _unlimitedMonumentElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedMonument, Translations.Monument, null, v => _modSetting.UnlimitedMonument = v).Control;
        _unlimitedMainCampusBuildingElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedMainCampusBuilding, Translations.MainCampusBuilding, null, v => _modSetting.UnlimitedMainCampusBuilding = v).Control;
        _unlimitedUniqueFactoryElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedUniqueFactory, Translations.UniqueFactory, null, v => _modSetting.UnlimitedUniqueFactory = v).Control;
        _unlimitedStockExchangeElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedStockExchange, Translations.StockExchange, null, v => _modSetting.UnlimitedStockExchange = v).Control;
        _unlimitedUniqueFacultyElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedUniqueFaculty, Translations.UniqueFaculty, null, v => _modSetting.UnlimitedUniqueFaculty = v).Control;
        _unlimitedWeatherRadarElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedWeatherRadar, Translations.WeatherRadar, null, v => _modSetting.UnlimitedWeatherRadar = v).Control;
        _unlimitedSpaceRadarElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedSpaceRadar, Translations.SpaceRadar, null, v => _modSetting.UnlimitedSpaceRadar = v).Control;
        _unlimitedFestivalAreaElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedFestivalArea, Translations.FestivalArea, null, v => _modSetting.UnlimitedFestivalArea = v).Control;
        _unlimitedLibraryAIMinorElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedLibraryAI, Translations.UnlimitedLibraryAI, Translations.UnlimitedLibraryAIMinor, v => _modSetting.UnlimitedLibraryAI = v).Control;
        _unlimitedSpaceElevatorElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedSpaceElevator, Translations.UnlimitedSpaceElevator, Translations.UnlimitedSpaceElevatorMinor, v => _modSetting.UnlimitedSpaceElevator = v).Control;
        _unlimitedParkAIElement = unlimitedUniqueBuildingsSection.AddToggleSwitch(_modSetting.UnlimitedParkAI, Translations.UnlimitedParkAI, Translations.UnlimitedParkAIMinor, v => _modSetting.UnlimitedParkAI = v).Control;

        #endregion

        #region ResourceOptions

        var resourceOptionsSection = AddSection(_generalPage, Translations.ResourceOptions);
        var oilDepletionRateCard = resourceOptionsSection.AddSlider(GetString(Translations.OilDepletionRate, (uint)_modSetting.OilDepletionRate, Translations.Unlimited, Translations.Vanilla), null, 0, 100, 1, _modSetting.OilDepletionRate, new Vector2(388, 16), v => {
            _modSetting.OilDepletionRate = (int)v;
            _oilDepletionRateHeaderElement.Text = GetString(Translations.OilDepletionRate, (uint)_modSetting.OilDepletionRate, Translations.Unlimited, Translations.Vanilla);
        }, true);
        _oilDepletionRateHeaderElement = oilDepletionRateCard.HeaderElement;

        var oreDepletionRateCard = resourceOptionsSection.AddSlider(GetString(Translations.OreDepletionRate, (uint)_modSetting.OreDepletionRate, Translations.Unlimited, Translations.Vanilla), null, 0, 100, 1, _modSetting.OreDepletionRate, new Vector2(388, 16), v => {
            _modSetting.OreDepletionRate = (int)v;
            _oreDepletionRateHeaderElement.Text = GetString(Translations.OreDepletionRate, (uint)_modSetting.OreDepletionRate, Translations.Unlimited, Translations.Vanilla);
        }, true);
        _oreDepletionRateHeaderElement = oreDepletionRateCard.HeaderElement;

        #endregion
    }

    private static string GetString(string localize, uint value, string flag0, string flag1) => value switch {
        0 => string.Format(localize + ": {0}", flag0),
        100 => string.Format(localize + ": {0}", flag1),
        _ => string.Format(localize + ": {0}%", value)
    };

    private string GetRelocateCost() {
        var factor = _modSetting.BuildingRelocationCostFactor;
        var prefix = Translations.Cost + ": ";
        return prefix + factor switch {
            0 => $"{factor} ({Translations.Free})",
            0.2f => $"{factor} ({Translations.Vanilla})",
            _ => factor
        };
    }

    private string GetRefundMultipleFactor(string prefixText, float factor) {
        var prefix = $"{prefixText}: {factor}";
        var suffix = factor switch {
            0.75f => $" ({Translations.Vanilla})",
            _ => string.Empty
        };
        return prefix + suffix;
    }

    private string GetAnnualInterestRate() => Translations.AnnualInterestRate + $": {_modSetting.AnnualInterestRate:P2}";

    private void OnTabBarClicked(string arg) => SelectedTab = arg;

    private void OnUnlimitedUniqueBuildingsDisableAllClicked() {
        _unlimitedMonumentElement.IsOn = false;
        _unlimitedMainCampusBuildingElement.IsOn = false;
        _unlimitedUniqueFactoryElement.IsOn = false;
        _unlimitedStockExchangeElement.IsOn = false;
        _unlimitedUniqueFacultyElement.IsOn = false;
        _unlimitedWeatherRadarElement.IsOn = false;
        _unlimitedSpaceRadarElement.IsOn = false;
        _unlimitedFestivalAreaElement.IsOn = false;
        _unlimitedLibraryAIMinorElement.IsOn = false;
        _unlimitedSpaceElevatorElement.IsOn = false;
        _unlimitedParkAIElement.IsOn = false;
    }

    private void OnUnlimitedUniqueBuildingsEnableAllClicked() {
        _unlimitedMonumentElement.IsOn = true;
        _unlimitedMainCampusBuildingElement.IsOn = true;
        _unlimitedUniqueFactoryElement.IsOn = true;
        _unlimitedStockExchangeElement.IsOn = true;
        _unlimitedUniqueFacultyElement.IsOn = true;
        _unlimitedWeatherRadarElement.IsOn = true;
        _unlimitedSpaceRadarElement.IsOn = true;
        _unlimitedFestivalAreaElement.IsOn = true;
        _unlimitedLibraryAIMinorElement.IsOn = true;
        _unlimitedSpaceElevatorElement.IsOn = true;
        _unlimitedParkAIElement.IsOn = true;
    }

    private void OnCityServiceOptionsEnableAllClicked() {
        _removeDeathElement.IsOn = true;
        _removeCrimeElement.IsOn = true;
        _removeGarbageElement.IsOn = true;
        _maximizeAttractivenessElement.IsOn = true;
        _maximizeEntertainmentElement.IsOn = true;
        _maximizeLandValueElement.IsOn = true;
        _maximizeEducationCoverageElement.IsOn = true;
    }

    private void OnCityServiceOptionsDisableAllClicked() {
        _removeDeathElement.IsOn = false;
        _removeCrimeElement.IsOn = false;
        _removeGarbageElement.IsOn = false;
        _maximizeAttractivenessElement.IsOn = false;
        _maximizeEntertainmentElement.IsOn = false;
        _maximizeLandValueElement.IsOn = false;
        _maximizeEducationCoverageElement.IsOn = false;
    }

    private void OnFireControlEnableAllClicked() {
        _maximizeFireCoverageElement.IsOn = true;
        _removePlayerBuildingFireElement.IsOn = true;
        _removeResidentialBuildingFireElement.IsOn = true;
        _removeIndustrialBuildingFireElement.IsOn = true;
        _removeCommercialBuildingFireElement.IsOn = true;
        _removeOfficeBuildingFireElement.IsOn = true;
        _removeParkBuildingFireElement.IsOn = true;
        _removeMuseumFireElement.IsOn = true;
        _removeCampusBuildingFireElement.IsOn = true;
        _removeAirportBuildingFireElement.IsOn = true;
    }
    
    private void OnFireControlDisableAllClicked() {
        _maximizeFireCoverageElement.IsOn = false;
        _removePlayerBuildingFireElement.IsOn = false;
        _removeResidentialBuildingFireElement.IsOn = false;
        _removeIndustrialBuildingFireElement.IsOn = false;
        _removeCommercialBuildingFireElement.IsOn = false;
        _removeOfficeBuildingFireElement.IsOn = false;
        _removeParkBuildingFireElement.IsOn = false;
        _removeMuseumFireElement.IsOn = false;
        _removeCampusBuildingFireElement.IsOn = false;
        _removeAirportBuildingFireElement.IsOn = false;
    }
}