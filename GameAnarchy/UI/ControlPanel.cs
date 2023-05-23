namespace GameAnarchy.UI;
using MbyronModsCommon.UI;
using System.Collections.Generic;
using UnityEngine;

internal class ControlPanel : ControlPanelBase<Mod, ControlPanel> {
    private CustomUITabContainer tabContainer;

    private Vector2 ContainerSize => new(PorpertyPanelWidth, 514);    
    private CustomUIScrollablePanel GeneralContainer => tabContainer.Containers[0];
    private CustomUIScrollablePanel ServiceContainer => tabContainer.Containers[1];
    private CustomUIScrollablePanel EconomyContainer => tabContainer.Containers[2];

    protected override void InitComponents() {
        base.InitComponents();
        AddTabContainer();
        FillGeneralContainer();
        FillServiceContainer();
        FillEconomyContainer();
    }
    private void FillEconomyContainer() {
        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, GameAnarchy.Localize.Economy);
        var itemPanel0 = ControlPanelHelper.AddToggle(Config.Instance.RemoveNotEnoughMoney || Config.Instance.UnlimitedMoney || Config.Instance.CashAnarchy, GameAnarchy.Localize.RemoveNotEnoughMoney, GameAnarchy.Localize.RemoveNotEnoughMoneyMinor, (_) => Config.Instance.BuildingRefund = _);
        itemPanel0.Child.isEnabled = !Config.Instance.UnlimitedMoney && !Config.Instance.CashAnarchy;
        ControlPanelHelper.AddField<UIIntValueField, int>(GameAnarchy.Localize.CityBankruptcyWarningThreshold, GameAnarchy.Localize.CityBankruptcyWarningThresholdMinor, 80, Config.Instance.CityBankruptcyWarningThreshold, 100, int.MinValue / 100, 0, (_) => Config.Instance.CityBankruptcyWarningThreshold = _);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, GameAnarchy.Localize.IncomeMultiplier);
        ControlPanelHelper.AddField<UIIntValueField, int>(GameAnarchy.Localize.Residential, null, 80, Config.Instance.ResidentialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.ResidentialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(GameAnarchy.Localize.Industrial, null, 80, Config.Instance.IndustrialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.IndustrialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(GameAnarchy.Localize.Commercial, null, 80, Config.Instance.CommercialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.CommercialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(GameAnarchy.Localize.Office, null, 80, Config.Instance.OfficeMultiplierFactor, 10, 1, 100, (_) => Config.Instance.OfficeMultiplierFactor = _);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, GameAnarchy.Localize.Refund);
        ControlPanelHelper.AddToggle(Config.Instance.BuildingRefund, GameAnarchy.Localize.BuildingRefund, null, (_) => {
            Config.Instance.BuildingRefund = _;
            foreach (var item in BuildingRefundOptions)
                item.Child.isEnabled = Config.Instance.BuildingRefund;
        });
        BuildingRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveBuildingRefundTimeLimitation, GameAnarchy.Localize.RemoveBuildingRefundTimeLimitation, null, (_) => Config.Instance.RemoveBuildingRefundTimeLimitation = _));
        var slider0 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(GameAnarchy.Localize.BuildingRefundMultipleFactor, Config.Instance.BuildingRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.BuildingRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRefundMultipleFactor = _;
            label4.Text = GetRefundMultipleFactor(GameAnarchy.Localize.BuildingRefundMultipleFactor, Config.Instance.BuildingRefundMultipleFactor);
        });
        label4 = slider0.MajorLabel;
        BuildingRefundOptions.Add(slider0);
        foreach (var item in BuildingRefundOptions)
            item.Child.isEnabled = Config.Instance.BuildingRefund;

        ControlPanelHelper.AddToggle(Config.Instance.SegmentRefund, GameAnarchy.Localize.SegmentRefund, null, (_) => {
            Config.Instance.SegmentRefund = _;
            foreach (var item in SegmentRefundOptions)
                item.Child.isEnabled = Config.Instance.SegmentRefund;
        });
        SegmentRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveSegmentRefundTimeLimitation, GameAnarchy.Localize.RemoveSegmentRefundTimeLimitation, null, (_) => Config.Instance.RemoveSegmentRefundTimeLimitation = _));
        var slider1 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(GameAnarchy.Localize.SegmentRefundMultipleFactor, Config.Instance.SegmentRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.SegmentRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.SegmentRefundMultipleFactor = _;
            label5.Text = GetRefundMultipleFactor(GameAnarchy.Localize.SegmentRefundMultipleFactor, Config.Instance.SegmentRefundMultipleFactor);
        });
        label5 = slider1.MajorLabel;
        SegmentRefundOptions.Add(slider1);
        foreach (var item in SegmentRefundOptions)
            item.Child.isEnabled = Config.Instance.SegmentRefund;
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(EconomyContainer, PorpertyPanelWidth, GameAnarchy.Localize.RelocateBuilding);
        var slider2 = ControlPanelHelper.AddSlider(GetRelocateCost(), GameAnarchy.Localize.RelocateBuildingMinor, 0, 1, 0.05f, Config.Instance.BuildingRelocationCostFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRelocationCostFactor = _;
            label6.Text = GetRelocateCost();
        });
        label6 = slider2.MajorLabel;
        ControlPanelHelper.Reset();
    }
    string GetRelocateCost() {
        var factor = Config.Instance.BuildingRelocationCostFactor;
        var prefix = GameAnarchy.Localize.Cost + ": ";
        return prefix + factor switch {
            0 => $"{factor} ({GameAnarchy.Localize.Free})",
            0.2f => $"{factor} ({GameAnarchy.Localize.Vanilla})",
            _ => factor.ToString(),
        };
    }
    string GetRefundMultipleFactor(string prefixText, float factor) {
        var prefix = $"{prefixText}: {factor}";
        var suffix = factor switch {
            0.75f => $" ({GameAnarchy.Localize.Vanilla})",
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
        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, GameAnarchy.Localize.RemovePollution);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveNoisePollution, GameAnarchy.Localize.NoisePollution, null, (v) => Config.Instance.RemoveNoisePollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGroundPollution, GameAnarchy.Localize.GroundPollution, null, (v) => Config.Instance.RemoveGroundPollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveWaterPollution, GameAnarchy.Localize.WaterPollution, null, (v) => Config.Instance.RemoveWaterPollution = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, GameAnarchy.Localize.CityServiceOptions);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveDeath, GameAnarchy.Localize.RemoveDeath, null, (v) => Config.Instance.RemoveDeath = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCrime, GameAnarchy.Localize.RemoveCrime, null, (v) => Config.Instance.RemoveCrime = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGarbage, GameAnarchy.Localize.RemoveGarbage, null, (v) => Config.Instance.RemoveGarbage = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeAttractiveness, GameAnarchy.Localize.MaximizeAttractiveness, null, (v) => Config.Instance.MaximizeAttractiveness = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEntertainment, GameAnarchy.Localize.MaximizeEntertainment, null, (v) => Config.Instance.MaximizeEntertainment = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeLandValue, GameAnarchy.Localize.MaximizeLandValue, null, (v) => Config.Instance.MaximizeLandValue = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEducationCoverage, GameAnarchy.Localize.MaximizeEducationCoverage, null, (v) => Config.Instance.MaximizeEducationCoverage = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, GameAnarchy.Localize.FireControl);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeFireCoverage, GameAnarchy.Localize.MaximizeFireCoverage, null, (v) => Config.Instance.MaximizeFireCoverage = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemovePlayerBuildingFire, GameAnarchy.Localize.RemovePlayerBuildingFire, null, (v) => Config.Instance.RemovePlayerBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveResidentialBuildingFire, GameAnarchy.Localize.RemoveResidentialBuildingFire, null, (v) => Config.Instance.RemoveResidentialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveIndustrialBuildingFire, GameAnarchy.Localize.RemoveIndustrialBuildingFire, null, (v) => Config.Instance.RemoveIndustrialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCommercialBuildingFire, GameAnarchy.Localize.RemoveCommercialBuildingFire, null, (v) => Config.Instance.RemoveCommercialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveOfficeBuildingFire, GameAnarchy.Localize.RemoveOfficeBuildingFire, null, (v) => Config.Instance.RemoveOfficeBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveParkBuildingFire, GameAnarchy.Localize.RemoveParkBuildingFire, null, (v) => Config.Instance.RemoveParkBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveMuseumFire, GameAnarchy.Localize.RemoveMuseumFire, null, (v) => Config.Instance.RemoveMuseumFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCampusBuildingFire, GameAnarchy.Localize.RemoveCampusBuildingFire, null, (v) => Config.Instance.RemoveCampusBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveAirportBuildingFire, GameAnarchy.Localize.RemoveAirportBuildingFire, null, (v) => Config.Instance.RemoveAirportBuildingFire = v);

        var panel0 = ControlPanelHelper.AddSlider(GetString(GameAnarchy.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, GameAnarchy.Localize.NoSpreadFire, GameAnarchy.Localize.Vanilla), null, 0, 100, 1, Config.Instance.BuildingSpreadFireProbability, new(388, 16), (value) => callback0(value), gradientStyle: true);
        label0 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.BuildingSpreadFireProbability = (uint)value;
            label0.Text = GetString(GameAnarchy.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, GameAnarchy.Localize.NoSpreadFire, GameAnarchy.Localize.Vanilla);
        }

        var panel1 = ControlPanelHelper.AddSlider(GetString(GameAnarchy.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, GameAnarchy.Localize.NoSpreadFire, GameAnarchy.Localize.Vanilla), null, 0, 100, 1, Config.Instance.TreeSpreadFireProbability, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label1 = panel1.MajorLabel;
        void callback1(float value) {
            Config.Instance.TreeSpreadFireProbability = (uint)value;
            label1.Text = GetString(GameAnarchy.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, GameAnarchy.Localize.NoSpreadFire, GameAnarchy.Localize.Vanilla);
        }
        ControlPanelHelper.Reset();
    }
    private CustomUILabel label2;
    private CustomUILabel label3;
    public void FillGeneralContainer() {
        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, GameAnarchy.Localize.EnabledUnlimitedUniqueBuildings);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedPlayerBuilding, GameAnarchy.Localize.PlayerBuilding, null, (v) => Config.Instance.UnlimitedPlayerBuilding = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMonument, GameAnarchy.Localize.Monument, null, (v) => Config.Instance.UnlimitedMonument = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMainCampusBuilding, GameAnarchy.Localize.MainCampusBuilding, null, (v) => Config.Instance.UnlimitedMainCampusBuilding = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFactory, GameAnarchy.Localize.UniqueFactory, null, (v) => Config.Instance.UnlimitedUniqueFactory = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedStockExchange, GameAnarchy.Localize.StockExchange, null, (v) => Config.Instance.UnlimitedStockExchange = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFaculty, GameAnarchy.Localize.UniqueFaculty, null, (v) => Config.Instance.UnlimitedUniqueFaculty = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedWeatherRadar, GameAnarchy.Localize.WeatherRadar, null, (v) => Config.Instance.UnlimitedWeatherRadar = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedSpaceRadar, GameAnarchy.Localize.SpaceRadar, null, (v) => Config.Instance.UnlimitedSpaceRadar = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedFestivalArea, GameAnarchy.Localize.FestivalArea, null, (v) => Config.Instance.UnlimitedFestivalArea = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, GameAnarchy.Localize.ResourceOptions);
        var panel0 = ControlPanelHelper.AddSlider(GetString(GameAnarchy.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, GameAnarchy.Localize.Unlimited, GameAnarchy.Localize.Vanilla), null, 0, 100, 1, Config.Instance.OilDepletionRate, new(388, 16), (_) => callback0(_), gradientStyle: true);
        label2 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.OilDepletionRate = (int)value;
            label2.Text = GetString(GameAnarchy.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, GameAnarchy.Localize.Unlimited, GameAnarchy.Localize.Vanilla);
        }

        var panel2 = ControlPanelHelper.AddSlider(GetString(GameAnarchy.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, GameAnarchy.Localize.Unlimited, GameAnarchy.Localize.Vanilla), null, 0, 100, 1, Config.Instance.OreDepletionRate, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label3 = panel2.MajorLabel;
        void callback1(float value) {
            Config.Instance.OreDepletionRate = (int)value;
            label3.Text = GetString(GameAnarchy.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, GameAnarchy.Localize.Unlimited, GameAnarchy.Localize.Vanilla);
        }
        ControlPanelHelper.Reset();
    }

    private static string GetString(string localize, uint value, string flag0, string flag1) {
        if (value == 0) {
            return string.Format(localize + ": {0}", flag0);
        } else if (value == 100) {
            return string.Format(localize + ": {0}", flag1);
        } else {
            return string.Format(localize + ": {0}%", value);
        }
    }


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
        tabContainer.AddTab(CommonLocalize.OptionPanel_General, this);
        tabContainer.AddTab(GameAnarchy.Localize.Service, this);
        tabContainer.AddTab(GameAnarchy.Localize.Economy, this);
    }
}

