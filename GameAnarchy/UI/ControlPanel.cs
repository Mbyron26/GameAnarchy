namespace GameAnarchy.UI;
using MbyronModsCommon.UI;
using System.Collections.Generic;
using UnityEngine;
using ModLocalize = Localize;

internal class ControlPanel : ControlPanelBase<Mod, ControlPanel> {
    private CustomUITabContainer tabContainer;

    private Vector2 ContainerSize => new(PorpertyPanelWidth, 514);
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
            if (SingletonManager<ToolButtonManager>.Instance.InGameToolButton is not null) {
                SingletonManager<ToolButtonManager>.Instance.InGameToolButton.IsOn = false;
            }
        };
        tabContainer.EventSelectedIndexChanged += (_) => SelectedIndex = _;
        tabContainer.SelectedIndex = SelectedIndex;
    }

    string GetAnnualInterestRate() => ModLocalize.AnnualInterestRate + $": {Config.Instance.AnnualInterestRate:P2}";
    CustomUILabel annualInterestRateLable;
    CustomUISlider annualInterestRateSlider;
    private void FillEconomyContainer() {
        EconomyContainer = AddTab(ModLocalize.Economy);
        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, ModLocalize.Economy);
        var itemPanel0 = ControlPanelHelper.AddToggle(Config.Instance.RemoveNotEnoughMoney || Config.Instance.UnlimitedMoney || Config.Instance.CashAnarchy, ModLocalize.RemoveNotEnoughMoney, ModLocalize.RemoveNotEnoughMoneyMinor, (_) => Config.Instance.RemoveNotEnoughMoney = _);
        itemPanel0.Child.isEnabled = !Config.Instance.UnlimitedMoney && !Config.Instance.CashAnarchy;
        ControlPanelHelper.AddField<UIIntValueField, int>(ModLocalize.CityBankruptcyWarningThreshold, ModLocalize.CityBankruptcyWarningThresholdMinor, 80, Config.Instance.CityBankruptcyWarningThreshold, 100, int.MinValue / 100, 0, (_) => Config.Instance.CityBankruptcyWarningThreshold = _);
        ControlPanelHelper.AddToggle(Config.Instance.ChargeInterest, ModLocalize.ChargeInterest, null, (_) => {
            Config.Instance.ChargeInterest = _;
            annualInterestRateSlider.isEnabled = Config.Instance.ChargeInterest;
        }, null);
        var itemPanel2 = ControlPanelHelper.AddSlider(GetAnnualInterestRate(), ModLocalize.AnnualInterestRateMinor, 0, 0.36f, 0.0025f, Config.Instance.AnnualInterestRate, new(388, 16), (_) => {
            Config.Instance.AnnualInterestRate = _;
            annualInterestRateLable.Text = GetAnnualInterestRate();
        });
        annualInterestRateLable = itemPanel2.MajorLabel;
        annualInterestRateSlider = itemPanel2.Child as CustomUISlider;
        annualInterestRateSlider.isEnabled = Config.Instance.ChargeInterest;
        ControlPanelHelper.AddToggle(Config.Instance.NoPoliciesCosts, ModLocalize.NoPoliciesCosts, ModLocalize.NoPoliciesCostsMinor, (_) => Config.Instance.NoPoliciesCosts = _);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, ModLocalize.IncomeMultiplier);
        ControlPanelHelper.AddField<UIIntValueField, int>(ModLocalize.Residential, null, 80, Config.Instance.ResidentialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.ResidentialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(ModLocalize.Industrial, null, 80, Config.Instance.IndustrialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.IndustrialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(ModLocalize.Commercial, null, 80, Config.Instance.CommercialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.CommercialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(ModLocalize.Office, null, 80, Config.Instance.OfficeMultiplierFactor, 10, 1, 100, (_) => Config.Instance.OfficeMultiplierFactor = _);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, ModLocalize.Refund);
        ControlPanelHelper.AddToggle(Config.Instance.BuildingRefund, ModLocalize.BuildingRefund, null, (_) => {
            Config.Instance.BuildingRefund = _;
            foreach (var item in BuildingRefundOptions)
                item.Child.isEnabled = Config.Instance.BuildingRefund;
        });
        BuildingRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveBuildingRefundTimeLimitation, ModLocalize.RemoveBuildingRefundTimeLimitation, null, (_) => Config.Instance.RemoveBuildingRefundTimeLimitation = _));
        var slider0 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(ModLocalize.BuildingRefundMultipleFactor, Config.Instance.BuildingRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.BuildingRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRefundMultipleFactor = _;
            label4.Text = GetRefundMultipleFactor(ModLocalize.BuildingRefundMultipleFactor, Config.Instance.BuildingRefundMultipleFactor);
        });
        label4 = slider0.MajorLabel;
        BuildingRefundOptions.Add(slider0);
        foreach (var item in BuildingRefundOptions)
            item.Child.isEnabled = Config.Instance.BuildingRefund;

        ControlPanelHelper.AddToggle(Config.Instance.SegmentRefund, ModLocalize.SegmentRefund, null, (_) => {
            Config.Instance.SegmentRefund = _;
            foreach (var item in SegmentRefundOptions)
                item.Child.isEnabled = Config.Instance.SegmentRefund;
        });
        SegmentRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveSegmentRefundTimeLimitation, ModLocalize.RemoveSegmentRefundTimeLimitation, null, (_) => Config.Instance.RemoveSegmentRefundTimeLimitation = _));
        var slider1 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(ModLocalize.SegmentRefundMultipleFactor, Config.Instance.SegmentRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.SegmentRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.SegmentRefundMultipleFactor = _;
            label5.Text = GetRefundMultipleFactor(ModLocalize.SegmentRefundMultipleFactor, Config.Instance.SegmentRefundMultipleFactor);
        });
        label5 = slider1.MajorLabel;
        SegmentRefundOptions.Add(slider1);
        foreach (var item in SegmentRefundOptions)
            item.Child.isEnabled = Config.Instance.SegmentRefund;
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, ModLocalize.RelocateBuilding);
        var slider2 = ControlPanelHelper.AddSlider(GetRelocateCost(), ModLocalize.RelocateBuildingMinor, 0, 1, 0.05f, Config.Instance.BuildingRelocationCostFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRelocationCostFactor = _;
            label6.Text = GetRelocateCost();
        });
        label6 = slider2.MajorLabel;
        ControlPanelHelper.Reset();
    }
    string GetRelocateCost() {
        var factor = Config.Instance.BuildingRelocationCostFactor;
        var prefix = ModLocalize.Cost + ": ";
        return prefix + factor switch {
            0 => $"{factor} ({ModLocalize.Free})",
            0.2f => $"{factor} ({ModLocalize.Vanilla})",
            _ => factor.ToString(),
        };
    }
    string GetRefundMultipleFactor(string prefixText, float factor) {
        var prefix = $"{prefixText}: {factor}";
        var suffix = factor switch {
            0.75f => $" ({ModLocalize.Vanilla})",
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
        ServiceContainer = AddTab(ModLocalize.Service);
        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, ModLocalize.RemovePollution);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveNoisePollution, ModLocalize.NoisePollution, null, (v) => Config.Instance.RemoveNoisePollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGroundPollution, ModLocalize.GroundPollution, null, (v) => Config.Instance.RemoveGroundPollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveWaterPollution, ModLocalize.WaterPollution, null, (v) => Config.Instance.RemoveWaterPollution = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, ModLocalize.CityServiceOptions);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveDeath, ModLocalize.RemoveDeath, null, (v) => Config.Instance.RemoveDeath = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCrime, ModLocalize.RemoveCrime, null, (v) => Config.Instance.RemoveCrime = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGarbage, ModLocalize.RemoveGarbage, null, (v) => Config.Instance.RemoveGarbage = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeAttractiveness, ModLocalize.MaximizeAttractiveness, null, (v) => Config.Instance.MaximizeAttractiveness = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEntertainment, ModLocalize.MaximizeEntertainment, null, (v) => Config.Instance.MaximizeEntertainment = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeLandValue, ModLocalize.MaximizeLandValue, null, (v) => Config.Instance.MaximizeLandValue = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEducationCoverage, ModLocalize.MaximizeEducationCoverage, null, (v) => Config.Instance.MaximizeEducationCoverage = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, ModLocalize.FireControl);
        ControlPanelHelper.AddButton(ModLocalize.PutOutBurningBuildings, null, ModLocalize.PutOut, null, 24, () => {
            if (SingletonMod<Mod>.Instance.IsLevelLoaded) {
                SingletonManager<Manager>.Instance.PutOutBuringBuildingsButtonClicked();
            }
        });
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeFireCoverage, ModLocalize.MaximizeFireCoverage, null, (v) => Config.Instance.MaximizeFireCoverage = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemovePlayerBuildingFire, ModLocalize.RemovePlayerBuildingFire, null, (v) => Config.Instance.RemovePlayerBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveResidentialBuildingFire, ModLocalize.RemoveResidentialBuildingFire, null, (v) => Config.Instance.RemoveResidentialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveIndustrialBuildingFire, ModLocalize.RemoveIndustrialBuildingFire, null, (v) => Config.Instance.RemoveIndustrialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCommercialBuildingFire, ModLocalize.RemoveCommercialBuildingFire, null, (v) => Config.Instance.RemoveCommercialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveOfficeBuildingFire, ModLocalize.RemoveOfficeBuildingFire, null, (v) => Config.Instance.RemoveOfficeBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveParkBuildingFire, ModLocalize.RemoveParkBuildingFire, null, (v) => Config.Instance.RemoveParkBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveMuseumFire, ModLocalize.RemoveMuseumFire, null, (v) => Config.Instance.RemoveMuseumFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCampusBuildingFire, ModLocalize.RemoveCampusBuildingFire, null, (v) => Config.Instance.RemoveCampusBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveAirportBuildingFire, ModLocalize.RemoveAirportBuildingFire, null, (v) => Config.Instance.RemoveAirportBuildingFire = v);

        var panel0 = ControlPanelHelper.AddSlider(GetString(ModLocalize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, ModLocalize.NoSpreadFire, ModLocalize.Vanilla), null, 0, 100, 1, Config.Instance.BuildingSpreadFireProbability, new(388, 16), (value) => callback0(value), gradientStyle: true);
        label0 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.BuildingSpreadFireProbability = (uint)value;
            label0.Text = GetString(ModLocalize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, ModLocalize.NoSpreadFire, ModLocalize.Vanilla);
        }

        var panel1 = ControlPanelHelper.AddSlider(GetString(ModLocalize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, ModLocalize.NoSpreadFire, ModLocalize.Vanilla), null, 0, 100, 1, Config.Instance.TreeSpreadFireProbability, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label1 = panel1.MajorLabel;
        void callback1(float value) {
            Config.Instance.TreeSpreadFireProbability = (uint)value;
            label1.Text = GetString(ModLocalize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, ModLocalize.NoSpreadFire, ModLocalize.Vanilla);
        }
        ControlPanelHelper.Reset();
    }
    private CustomUILabel label2;
    private CustomUILabel label3;
    public void FillGeneralContainer() {
        GeneralContainer = AddTab(CommonLocalize.OptionPanel_General);
        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, ModLocalize.EnabledUnlimitedUniqueBuildings);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMonument, ModLocalize.Monument, null, (v) => Config.Instance.UnlimitedMonument = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMainCampusBuilding, ModLocalize.MainCampusBuilding, null, (v) => Config.Instance.UnlimitedMainCampusBuilding = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFactory, ModLocalize.UniqueFactory, null, (v) => Config.Instance.UnlimitedUniqueFactory = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedStockExchange, ModLocalize.StockExchange, null, (v) => Config.Instance.UnlimitedStockExchange = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFaculty, ModLocalize.UniqueFaculty, null, (v) => Config.Instance.UnlimitedUniqueFaculty = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedWeatherRadar, ModLocalize.WeatherRadar, null, (v) => Config.Instance.UnlimitedWeatherRadar = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedSpaceRadar, ModLocalize.SpaceRadar, null, (v) => Config.Instance.UnlimitedSpaceRadar = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedFestivalArea, ModLocalize.FestivalArea, null, (v) => Config.Instance.UnlimitedFestivalArea = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedLibraryAI, ModLocalize.UnlimitedLibraryAI, ModLocalize.UnlimitedLibraryAIMinor, (v) => Config.Instance.UnlimitedLibraryAI = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedSpaceElevator, ModLocalize.UnlimitedSpaceElevator, ModLocalize.UnlimitedSpaceElevatorMinor, (v) => Config.Instance.UnlimitedSpaceElevator = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedParkAI, ModLocalize.UnlimitedParkAI, ModLocalize.UnlimitedParkAIMinor, (v) => Config.Instance.UnlimitedParkAI = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, ModLocalize.ResourceOptions);
        var panel0 = ControlPanelHelper.AddSlider(GetString(ModLocalize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, ModLocalize.Unlimited, ModLocalize.Vanilla), null, 0, 100, 1, Config.Instance.OilDepletionRate, new(388, 16), (_) => callback0(_), gradientStyle: true);
        label2 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.OilDepletionRate = (int)value;
            label2.Text = GetString(ModLocalize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, ModLocalize.Unlimited, ModLocalize.Vanilla);
        }

        var panel2 = ControlPanelHelper.AddSlider(GetString(ModLocalize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, ModLocalize.Unlimited, ModLocalize.Vanilla), null, 0, 100, 1, Config.Instance.OreDepletionRate, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label3 = panel2.MajorLabel;
        void callback1(float value) {
            Config.Instance.OreDepletionRate = (int)value;
            label3.Text = GetString(ModLocalize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, ModLocalize.Unlimited, ModLocalize.Vanilla);
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
        tabContainer.size = new Vector2(PorpertyPanelWidth, 24);
        tabContainer.Gap = 2;
        tabContainer.Atlas = CustomUIAtlas.MbyronModsAtlas;
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

