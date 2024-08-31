using CSShared.Common;
using CSShared.Manager;
using CSShared.UI;
using CSShared.UI.ControlPanel;
using GameAnarchy.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnarchy.UI;

internal class ControlPanel : ControlPanelBase<Mod, ControlPanel> {
    private CustomUITabContainer tabContainer;

    private Vector2 ContainerSize => new(PropertyPanelWidth, 514);
    private CustomUIScrollablePanel GeneralContainer { get; set; }
    private CustomUIScrollablePanel ServiceContainer { get; set; }
    private CustomUIScrollablePanel EconomyContainer { get; set; }
    private static int SelectedIndex { get; set; }

    protected override void InitComponents() {
        base.InitComponents();
        AddTabContainer();
        FillGeneralContainer();
        FillServiceContainer();
        FillEconomyContainer();
        ControlPanelManager<Mod, ControlPanel>.EventPanelClosing += (_) => {
            if (ManagerPool.GetOrCreateManager<ToolButtonManager>().InGameToolButton is not null) {
                ManagerPool.GetOrCreateManager<ToolButtonManager>().InGameToolButton.IsOn = false;
            }
        };
        tabContainer.EventSelectedIndexChanged += (_) => SelectedIndex = _;
        tabContainer.SelectedIndex = SelectedIndex;
    }

    string GetAnnualInterestRate() => Localize("AnnualInterestRate") + $": {Config.Instance.AnnualInterestRate:P2}";
    CustomUILabel annualInterestRateLabel;
    CustomUISlider annualInterestRateSlider;
    private void FillEconomyContainer() {
        EconomyContainer = AddTab(Localize("Economy"));
        ControlPanelHelper.AddGroup(EconomyContainer, PropertyPanelWidth, Localize("Economy"));
        var itemPanel0 = ControlPanelHelper.AddToggle(Config.Instance.RemoveNotEnoughMoney || Config.Instance.UnlimitedMoney || Config.Instance.CashAnarchy, Localize("RemoveNotEnoughMoney"), Localize("RemoveNotEnoughMoneyMinor"), (_) => Config.Instance.RemoveNotEnoughMoney = _);
        itemPanel0.Child.isEnabled = !Config.Instance.UnlimitedMoney && !Config.Instance.CashAnarchy;
        ControlPanelHelper.AddField<UIIntValueField, int>(Localize("CityBankruptcyWarningThreshold"), Localize("CityBankruptcyWarningThresholdMinor"), 80, Config.Instance.CityBankruptcyWarningThreshold, 100, int.MinValue / 100, 0, (_) => Config.Instance.CityBankruptcyWarningThreshold = _);
        ControlPanelHelper.AddToggle(Config.Instance.ChargeInterest, Localize("ChargeInterest"), null, (_) => {
            Config.Instance.ChargeInterest = _;
            annualInterestRateSlider.isEnabled = Config.Instance.ChargeInterest;
        }, null);
        var itemPanel2 = ControlPanelHelper.AddSlider(GetAnnualInterestRate(), Localize("AnnualInterestRateMinor"), 0, 0.36f, 0.0025f, Config.Instance.AnnualInterestRate, new(388, 16), (_) => {
            Config.Instance.AnnualInterestRate = _;
            annualInterestRateLabel.Text = GetAnnualInterestRate();
        });
        annualInterestRateLabel = itemPanel2.MajorLabel;
        annualInterestRateSlider = itemPanel2.Child as CustomUISlider;
        annualInterestRateSlider.isEnabled = Config.Instance.ChargeInterest;
        ControlPanelHelper.AddToggle(Config.Instance.NoPoliciesCosts, Localize("NoPoliciesCosts"), Localize("NoPoliciesCostsMinor"), (_) => Config.Instance.NoPoliciesCosts = _);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PropertyPanelWidth, Localize("IncomeMultiplier"));
        ControlPanelHelper.AddField<UIIntValueField, int>(Localize("Residential"), null, 80, Config.Instance.ResidentialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.ResidentialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(Localize("Industrial"), null, 80, Config.Instance.IndustrialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.IndustrialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(Localize("Commercial"), null, 80, Config.Instance.CommercialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.CommercialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(Localize("Office"), null, 80, Config.Instance.OfficeMultiplierFactor, 10, 1, 100, (_) => Config.Instance.OfficeMultiplierFactor = _);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PropertyPanelWidth, Localize("Refund"));
        ControlPanelHelper.AddToggle(Config.Instance.BuildingRefund, Localize("BuildingRefund"), null, (_) => {
            Config.Instance.BuildingRefund = _;
            foreach (var item in BuildingRefundOptions)
                item.Child.isEnabled = Config.Instance.BuildingRefund;
        });
        BuildingRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveBuildingRefundTimeLimitation, Localize("RemoveBuildingRefundTimeLimitation"), null, (_) => Config.Instance.RemoveBuildingRefundTimeLimitation = _));
        var slider0 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(Localize("BuildingRefundMultipleFactor"), Config.Instance.BuildingRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.BuildingRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRefundMultipleFactor = _;
            label4.Text = GetRefundMultipleFactor(Localize("BuildingRefundMultipleFactor"), Config.Instance.BuildingRefundMultipleFactor);
        });
        label4 = slider0.MajorLabel;
        BuildingRefundOptions.Add(slider0);
        foreach (var item in BuildingRefundOptions)
            item.Child.isEnabled = Config.Instance.BuildingRefund;

        ControlPanelHelper.AddToggle(Config.Instance.SegmentRefund, Localize("SegmentRefund"), null, (_) => {
            Config.Instance.SegmentRefund = _;
            foreach (var item in SegmentRefundOptions)
                item.Child.isEnabled = Config.Instance.SegmentRefund;
        });
        SegmentRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveSegmentRefundTimeLimitation, Localize("RemoveSegmentRefundTimeLimitation"), null, (_) => Config.Instance.RemoveSegmentRefundTimeLimitation = _));
        var slider1 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(Localize("SegmentRefundMultipleFactor"), Config.Instance.SegmentRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.SegmentRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.SegmentRefundMultipleFactor = _;
            label5.Text = GetRefundMultipleFactor(Localize("SegmentRefundMultipleFactor"), Config.Instance.SegmentRefundMultipleFactor);
        });
        label5 = slider1.MajorLabel;
        SegmentRefundOptions.Add(slider1);
        foreach (var item in SegmentRefundOptions)
            item.Child.isEnabled = Config.Instance.SegmentRefund;
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PropertyPanelWidth, Localize("RelocateBuilding"));
        var slider2 = ControlPanelHelper.AddSlider(GetRelocateCost(), Localize("RelocateBuildingMinor"), 0, 1, 0.05f, Config.Instance.BuildingRelocationCostFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRelocationCostFactor = _;
            label6.Text = GetRelocateCost();
        });
        label6 = slider2.MajorLabel;
        ControlPanelHelper.Reset();
    }
    string GetRelocateCost() {
        var factor = Config.Instance.BuildingRelocationCostFactor;
        var prefix = Localize("Cost") + ": ";
        return prefix + factor switch {
            0 => $"{factor} ({Localize("Free")})",
            0.2f => $"{factor} ({Localize("Vanilla")})",
            _ => factor.ToString(),
        };
    }
    string GetRefundMultipleFactor(string prefixText, float factor) {
        var prefix = $"{prefixText}: {factor}";
        var suffix = factor switch {
            0.75f => $" ({Localize("Vanilla")})",
            _ => string.Empty
        };
        return prefix + suffix;
    }

    readonly List<SinglePropertyPanelBase> BuildingRefundOptions = new();
    readonly List<SinglePropertyPanelBase> SegmentRefundOptions = new();
    CustomUILabel label0;
    CustomUILabel label1;
    CustomUILabel label4;
    CustomUILabel label5;
    CustomUILabel label6;
    public void FillServiceContainer() {
        ServiceContainer = AddTab(Localize("Service"));
        ControlPanelHelper.AddGroup(ServiceContainer, PropertyPanelWidth, Localize("RemovePollution"));
        ControlPanelHelper.AddToggle(Config.Instance.RemoveNoisePollution, Localize("NoisePollution"), null, (v) => Config.Instance.RemoveNoisePollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGroundPollution, Localize("GroundPollution"), null, (v) => Config.Instance.RemoveGroundPollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveWaterPollution, Localize("WaterPollution"), null, (v) => Config.Instance.RemoveWaterPollution = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PropertyPanelWidth, Localize("CityServiceOptions"));
        ControlPanelHelper.AddToggle(Config.Instance.RemoveDeath, Localize("RemoveDeath"), null, (v) => Config.Instance.RemoveDeath = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCrime, Localize("RemoveCrime"), null, (v) => Config.Instance.RemoveCrime = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGarbage, Localize("RemoveGarbage"), null, (v) => Config.Instance.RemoveGarbage = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeAttractiveness, Localize("MaximizeAttractiveness"), null, (v) => Config.Instance.MaximizeAttractiveness = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEntertainment, Localize("MaximizeEntertainment"), null, (v) => Config.Instance.MaximizeEntertainment = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeLandValue, Localize("MaximizeLandValue"), null, (v) => Config.Instance.MaximizeLandValue = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEducationCoverage, Localize("MaximizeEducationCoverage"), null, (v) => Config.Instance.MaximizeEducationCoverage = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PropertyPanelWidth, Localize("FireControl"));
        ControlPanelHelper.AddButton(Localize("PutOutBurningBuildings"), null, Localize("PutOut"), null, 24, () => {
            if (SingletonMod<Mod>.Instance.IsLevelLoaded) {
                ManagerPool.GetOrCreateManager<Manager>().PutOutBuringBuildingsButtonClicked();
            }
        });
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeFireCoverage, Localize("MaximizeFireCoverage"), null, (v) => Config.Instance.MaximizeFireCoverage = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemovePlayerBuildingFire, Localize("RemovePlayerBuildingFire"), null, (v) => Config.Instance.RemovePlayerBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveResidentialBuildingFire, Localize("RemoveResidentialBuildingFire"), null, (v) => Config.Instance.RemoveResidentialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveIndustrialBuildingFire, Localize("RemoveIndustrialBuildingFire"), null, (v) => Config.Instance.RemoveIndustrialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCommercialBuildingFire, Localize("RemoveCommercialBuildingFire"), null, (v) => Config.Instance.RemoveCommercialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveOfficeBuildingFire, Localize("RemoveOfficeBuildingFire"), null, (v) => Config.Instance.RemoveOfficeBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveParkBuildingFire, Localize("RemoveParkBuildingFire"), null, (v) => Config.Instance.RemoveParkBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveMuseumFire, Localize("RemoveMuseumFire"), null, (v) => Config.Instance.RemoveMuseumFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCampusBuildingFire, Localize("RemoveCampusBuildingFire"), null, (v) => Config.Instance.RemoveCampusBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveAirportBuildingFire, Localize("RemoveAirportBuildingFire"), null, (v) => Config.Instance.RemoveAirportBuildingFire = v);

        var panel0 = ControlPanelHelper.AddSlider(GetString(Localize("BuildingSpreadFireProbability"), Config.Instance.BuildingSpreadFireProbability, Localize("NoSpreadFire"), Localize("Vanilla")), null, 0, 100, 1, Config.Instance.BuildingSpreadFireProbability, new(388, 16), (value) => callback0(value), gradientStyle: true);
        label0 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.BuildingSpreadFireProbability = (uint)value;
            label0.Text = GetString(Localize("BuildingSpreadFireProbability"), Config.Instance.BuildingSpreadFireProbability, Localize("NoSpreadFire"), Localize("Vanilla"));
        }

        var panel1 = ControlPanelHelper.AddSlider(GetString(Localize("TreeSpreadFireProbability"), Config.Instance.TreeSpreadFireProbability, Localize("NoSpreadFire"), Localize("Vanilla")), null, 0, 100, 1, Config.Instance.TreeSpreadFireProbability, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label1 = panel1.MajorLabel;
        void callback1(float value) {
            Config.Instance.TreeSpreadFireProbability = (uint)value;
            label1.Text = GetString(Localize("TreeSpreadFireProbability"), Config.Instance.TreeSpreadFireProbability, Localize("NoSpreadFire"), Localize("Vanilla"));
        }
        ControlPanelHelper.Reset();
    }
    private CustomUILabel label2;
    private CustomUILabel label3;
    public void FillGeneralContainer() {
        GeneralContainer = AddTab(Localize("OptionPanel_General"));
        ControlPanelHelper.AddGroup(GeneralContainer, PropertyPanelWidth, Localize("EnabledUnlimitedUniqueBuildings"));
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMonument, Localize("Monument"), null, (v) => Config.Instance.UnlimitedMonument = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMainCampusBuilding, Localize("MainCampusBuilding"), null, (v) => Config.Instance.UnlimitedMainCampusBuilding = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFactory, Localize("UniqueFactory"), null, (v) => Config.Instance.UnlimitedUniqueFactory = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedStockExchange, Localize("StockExchange"), null, (v) => Config.Instance.UnlimitedStockExchange = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFaculty, Localize("UniqueFaculty"), null, (v) => Config.Instance.UnlimitedUniqueFaculty = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedWeatherRadar, Localize("WeatherRadar"), null, (v) => Config.Instance.UnlimitedWeatherRadar = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedSpaceRadar, Localize("SpaceRadar"), null, (v) => Config.Instance.UnlimitedSpaceRadar = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedFestivalArea, Localize("FestivalArea"), null, (v) => Config.Instance.UnlimitedFestivalArea = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedLibraryAI, Localize("UnlimitedLibraryAI"), Localize("UnlimitedLibraryAIMinor"), (v) => Config.Instance.UnlimitedLibraryAI = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedSpaceElevator, Localize("UnlimitedSpaceElevator"), Localize("UnlimitedSpaceElevatorMinor"), (v) => Config.Instance.UnlimitedSpaceElevator = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedParkAI, Localize("UnlimitedParkAI"), Localize("UnlimitedParkAIMinor"), (v) => Config.Instance.UnlimitedParkAI = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(GeneralContainer, PropertyPanelWidth, Localize("ResourceOptions"));
        var panel0 = ControlPanelHelper.AddSlider(GetString(Localize("OilDepletionRate"), (uint)Config.Instance.OilDepletionRate, Localize("Unlimited"), Localize("Vanilla")), null, 0, 100, 1, Config.Instance.OilDepletionRate, new(388, 16), (_) => callback0(_), gradientStyle: true);
        label2 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.OilDepletionRate = (int)value;
            label2.Text = GetString(Localize("OilDepletionRate"), (uint)Config.Instance.OilDepletionRate, Localize("Unlimited"), Localize("Vanilla"));
        }

        var panel2 = ControlPanelHelper.AddSlider(GetString(Localize("OreDepletionRate"), (uint)Config.Instance.OreDepletionRate, Localize("Unlimited"), Localize("Vanilla")), null, 0, 100, 1, Config.Instance.OreDepletionRate, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label3 = panel2.MajorLabel;
        void callback1(float value) {
            Config.Instance.OreDepletionRate = (int)value;
            label3.Text = GetString(Localize("OreDepletionRate"), (uint)Config.Instance.OreDepletionRate, Localize("Unlimited"), Localize("Vanilla"));
        }
        ControlPanelHelper.Reset();
    }

    private static string GetString(string localize, uint value, string flag0, string flag1) {
        if (value == 0) {
            return string.Format(localize + ": {0}", flag0);
        }
        else if (value == 100) {
            return string.Format(localize + ": {0}", flag1);
        }
        else {
            return string.Format(localize + ": {0}%", value);
        }
    }

    private CustomUIScrollablePanel AddTab(string text) => tabContainer.AddContainer(text, this);

    private void AddTabContainer() {
        tabContainer = AddUIComponent<CustomUITabContainer>();
        tabContainer.size = new Vector2(PropertyPanelWidth, 24);
        tabContainer.Gap = 2;
        tabContainer.Atlas = CustomUIAtlas.CSSharedAtlas;
        tabContainer.BgSprite = CustomUIAtlas.RoundedRectangle2;
        tabContainer.BgNormalColor = CustomUIColor.CPPrimaryBg;
        tabContainer.relativePosition = new Vector2(16, CaptionHeight);
        tabContainer.EventTabAdded += (_) => {
            _.TextScale = 0.9f;
            _.SetDefaultControlPanelStyle();
            _.TextPadding = new RectOffset(0, 0, 2, 0);
        };
        tabContainer.EventContainerAdded += (_) => {
            _.size = ContainerSize;
            _.autoLayoutPadding = new RectOffset(0, 0, 5, 10);
            var scrollbar0 = UIScrollbarHelper.AddScrollbar(this, _, new Vector2(8, 514));
            scrollbar0.thumbObject.color = CustomUIColor.CPPrimaryBg;
            scrollbar0.relativePosition = new Vector2(width - 8, CaptionHeight + 30);
            _.relativePosition = new Vector2(16, CaptionHeight + 30);
        };
    }
}

