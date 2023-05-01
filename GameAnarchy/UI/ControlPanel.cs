using ColossalFramework.UI;
using MbyronModsCommon.UI;
using System.Collections.Generic;
using UnityEngine;
namespace GameAnarchy.UI;

internal class ControlPanel : CustomUIPanel {
    private const string Name = nameof(GameAnarchy) + nameof(ControlPanel);
    private const float PanelWidth = 440;
    private const float PanelHeight = 600;
    private const float ElementOffset = 10;
    private const float CaptionHeight = 40;
    public const float PorpertyPanelWidth = PanelWidth - 2 * 16;
    private readonly Vector2 ContainerSize = new(PorpertyPanelWidth, 514);
    private UIDragHandle DragBar;
    private CustomUILabel title;
    private CustomUITabContainer tabContainer;
    private CustomUIButton closeButton;

    public static Vector2 PanelPosition { get; set; }
    public static Vector2 ButtonSize => new(28, 28);


    private CustomUIScrollablePanel GeneralContainer => tabContainer.Containers[0];
    private CustomUIScrollablePanel ServiceContainer => tabContainer.Containers[1];
    private CustomUIScrollablePanel IncomeContainer => tabContainer.Containers[2];
    public ControlPanel() {
        name = Name;
        atlas = CustomUIAtlas.MbyronModsAtlas;
        bgSprite = CustomUIAtlas.CustomBackground;
        isVisible = true;
        canFocus = true;
        isInteractive = true;
        size = new Vector2(PanelWidth, PanelHeight);

        AddCaption();
        AddTabContainer();
        FillGeneralContainer();
        FillServiceContainer();
        FillIncomeContainer();
        SetPosition();
        eventPositionChanged += (c, v) => PanelPosition = relativePosition;
    }
    private void SetPosition() {
        if (PanelPosition == Vector2.zero) {
            Vector2 vector = GetUIView().GetScreenResolution();
            var x = vector.x - PanelWidth - 360;
            PanelPosition = relativePosition = new Vector3(x, 80);
        } else {
            relativePosition = PanelPosition;
        }
    }

    private void FillIncomeContainer() {
        ControlPanelHelper.AddGroup(IncomeContainer, PorpertyPanelWidth, Localization.Localize.IncomeMultiplier);
        ControlPanelHelper.AddField<UIIntValueField, int>(Localization.Localize.Residential, null, 80, Config.Instance.ResidentialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.ResidentialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(Localization.Localize.Industrial, null, 80, Config.Instance.IndustrialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.IndustrialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(Localization.Localize.Commercial, null, 80, Config.Instance.CommercialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.CommercialMultiplierFactor = _);
        ControlPanelHelper.AddField<UIIntValueField, int>(Localization.Localize.Office, null, 80, Config.Instance.OfficeMultiplierFactor, 10, 1, 100, (_) => Config.Instance.OfficeMultiplierFactor = _);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(IncomeContainer, PorpertyPanelWidth, Localization.Localize.Refund);
        ControlPanelHelper.AddToggle(Config.Instance.BuildingRefund, Localization.Localize.BuildingRefund, null, (_) => {
            Config.Instance.BuildingRefund = _;
            foreach (var item in BuildingRefundOptions)
                item.isEnabled = Config.Instance.BuildingRefund;
        });
        BuildingRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveBuildingRefundTimeLimitation, Localization.Localize.RemoveBuildingRefundTimeLimitation, null, (_) => Config.Instance.RemoveBuildingRefundTimeLimitation = _));
        var slider0 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(Localization.Localize.BuildingRefundMultipleFactor, Config.Instance.BuildingRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.BuildingRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRefundMultipleFactor = _;
            label4.Text = GetRefundMultipleFactor(Localization.Localize.BuildingRefundMultipleFactor, Config.Instance.BuildingRefundMultipleFactor);
        });
        label4 = slider0.MajorLabel;
        BuildingRefundOptions.Add(slider0);
        foreach (var item in BuildingRefundOptions)
            item.isEnabled = Config.Instance.BuildingRefund;

        ControlPanelHelper.AddToggle(Config.Instance.SegmentRefund, Localization.Localize.SegmentRefund, null, (_) => {
            Config.Instance.SegmentRefund = _;
            foreach (var item in SegmentRefundOptions)
                item.isEnabled = Config.Instance.SegmentRefund;
        });
        SegmentRefundOptions.Add(ControlPanelHelper.AddToggle(Config.Instance.RemoveSegmentRefundTimeLimitation, Localization.Localize.RemoveSegmentRefundTimeLimitation, null, (_) => Config.Instance.RemoveSegmentRefundTimeLimitation = _));
        var slider1 = ControlPanelHelper.AddSlider(GetRefundMultipleFactor(Localization.Localize.SegmentRefundMultipleFactor, Config.Instance.SegmentRefundMultipleFactor), null, 0, 2, 0.25f, Config.Instance.SegmentRefundMultipleFactor, new(388, 16), (_) => {
            Config.Instance.SegmentRefundMultipleFactor = _;
            label5.Text = GetRefundMultipleFactor(Localization.Localize.SegmentRefundMultipleFactor, Config.Instance.SegmentRefundMultipleFactor);
        });
        label5 = slider1.MajorLabel;
        SegmentRefundOptions.Add(slider1);
        foreach (var item in SegmentRefundOptions)
            item.isEnabled = Config.Instance.SegmentRefund;
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(IncomeContainer, PorpertyPanelWidth, "Relocate building");
        var slider2 = ControlPanelHelper.AddSlider(GetRelocateCost(), Localization.Localize.RelocateBuildingMinor, 0, 1, 0.05f, Config.Instance.BuildingRelocationCostFactor, new(388, 16), (_) => {
            Config.Instance.BuildingRelocationCostFactor = _;
            label6.Text = GetRelocateCost();
        });
        label6 = slider2.MajorLabel;
        ControlPanelHelper.Reset();
    }
    string GetRelocateCost() {
        var factor = Config.Instance.BuildingRelocationCostFactor;
        var prefix = Localization.Localize.Cost + ": ";
        return prefix + factor switch {
            0 => $"{factor} ({Localization.Localize.Free})",
            0.2f => $"{factor} ({Localization.Localize.Vanilla})",
            _ => factor.ToString(),
        };
    }
    string GetRefundMultipleFactor(string prefixText, float factor) {
        var prefix = $"{prefixText}: {factor}";
        var suffix = factor switch {
            0.75f => $" ({Localization.Localize.Vanilla})",
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
        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, Localization.Localize.RemovePollution);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveNoisePollution, Localization.Localize.NoisePollution, null, (v) => Config.Instance.RemoveNoisePollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGroundPollution, Localization.Localize.GroundPollution, null, (v) => Config.Instance.RemoveGroundPollution = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveWaterPollution, Localization.Localize.WaterPollution, null, (v) => Config.Instance.RemoveWaterPollution = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, Localization.Localize.CityServiceOptions);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveDeath, Localization.Localize.RemoveDeath, null, (v) => Config.Instance.RemoveDeath = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCrime, Localization.Localize.RemoveCrime, null, (v) => Config.Instance.RemoveCrime = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveGarbage, Localization.Localize.RemoveGarbage, null, (v) => Config.Instance.RemoveGarbage = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeAttractiveness, Localization.Localize.MaximizeAttractiveness, null, (v) => Config.Instance.MaximizeAttractiveness = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEntertainment, Localization.Localize.MaximizeEntertainment, null, (v) => Config.Instance.MaximizeEntertainment = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeLandValue, Localization.Localize.MaximizeLandValue, null, (v) => Config.Instance.MaximizeLandValue = v);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeEducationCoverage, Localization.Localize.MaximizeEducationCoverage, null, (v) => Config.Instance.MaximizeEducationCoverage = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(ServiceContainer, PorpertyPanelWidth, Localization.Localize.FireControl);
        ControlPanelHelper.AddToggle(Config.Instance.MaximizeFireCoverage, Localization.Localize.MaximizeFireCoverage, null, (v) => Config.Instance.MaximizeFireCoverage = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemovePlayerBuildingFire, Localization.Localize.RemovePlayerBuildingFire, null, (v) => Config.Instance.RemovePlayerBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveResidentialBuildingFire, Localization.Localize.RemoveResidentialBuildingFire, null, (v) => Config.Instance.RemoveResidentialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveIndustrialBuildingFire, Localization.Localize.RemoveIndustrialBuildingFire, null, (v) => Config.Instance.RemoveIndustrialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCommercialBuildingFire, Localization.Localize.RemoveCommercialBuildingFire, null, (v) => Config.Instance.RemoveCommercialBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveOfficeBuildingFire, Localization.Localize.RemoveOfficeBuildingFire, null, (v) => Config.Instance.RemoveOfficeBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveParkBuildingFire, Localization.Localize.RemoveParkBuildingFire, null, (v) => Config.Instance.RemoveParkBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveMuseumFire, Localization.Localize.RemoveMuseumFire, null, (v) => Config.Instance.RemoveMuseumFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveCampusBuildingFire, Localization.Localize.RemoveCampusBuildingFire, null, (v) => Config.Instance.RemoveCampusBuildingFire = v);
        ControlPanelHelper.AddToggle(Config.Instance.RemoveAirportBuildingFire, Localization.Localize.RemoveAirportBuildingFire, null, (v) => Config.Instance.RemoveAirportBuildingFire = v);

        var panel0 = ControlPanelHelper.AddSlider(GetString(Localization.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla), null, 0, 100, 1, Config.Instance.BuildingSpreadFireProbability, new(388, 16), (value) => callback0(value), gradientStyle: true);
        label0 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.BuildingSpreadFireProbability = (uint)value;
            label0.Text = GetString(Localization.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla);
        }

        var panel1 = ControlPanelHelper.AddSlider(GetString(Localization.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla), null, 0, 100, 1, Config.Instance.TreeSpreadFireProbability, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label1 = panel1.MajorLabel;
        void callback1(float value) {
            Config.Instance.TreeSpreadFireProbability = (uint)value;
            label1.Text = GetString(Localization.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla);
        }
        ControlPanelHelper.Reset();
    }
    private CustomUILabel label2;
    private CustomUILabel label3;
    public void FillGeneralContainer() {
        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, Localization.Localize.EnabledUnlimitedUniqueBuildings);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedPlayerBuilding, Localization.Localize.PlayerBuilding, null, (v) => Config.Instance.UnlimitedPlayerBuilding = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMonument, Localization.Localize.Monument, null, (v) => Config.Instance.UnlimitedMonument = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedMainCampusBuilding, Localization.Localize.MainCampusBuilding, null, (v) => Config.Instance.UnlimitedMainCampusBuilding = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFactory, Localization.Localize.UniqueFactory, null, (v) => Config.Instance.UnlimitedUniqueFactory = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedStockExchange, Localization.Localize.StockExchange, null, (v) => Config.Instance.UnlimitedStockExchange = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedUniqueFaculty, Localization.Localize.UniqueFaculty, null, (v) => Config.Instance.UnlimitedUniqueFaculty = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedWeatherRadar, Localization.Localize.WeatherRadar, null, (v) => Config.Instance.UnlimitedWeatherRadar = v);
        ControlPanelHelper.AddToggle(Config.Instance.UnlimitedSpaceRadar, Localization.Localize.SpaceRadar, null, (v) => Config.Instance.UnlimitedSpaceRadar = v);
        ControlPanelHelper.Reset();

        ControlPanelHelper.AddGroup(GeneralContainer, PorpertyPanelWidth, Localization.Localize.ResourceOptions);
        var panel0 = ControlPanelHelper.AddSlider(GetString(Localization.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla), null, 0, 100, 1, Config.Instance.OilDepletionRate, new(388, 16), (_) => callback0(_), gradientStyle: true);
        label2 = panel0.MajorLabel;
        void callback0(float value) {
            Config.Instance.OilDepletionRate = (int)value;
            label2.Text = GetString(Localization.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla);
        }

        var panel2 = ControlPanelHelper.AddSlider(GetString(Localization.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla), null, 0, 100, 1, Config.Instance.OreDepletionRate, new(388, 16), (value) => callback1(value), gradientStyle: true);
        label3 = panel2.MajorLabel;
        void callback1(float value) {
            Config.Instance.OreDepletionRate = (int)value;
            label3.Text = GetString(Localization.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla);
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
        tabContainer.AddTab(Localization.Localize.Service, this);
        tabContainer.AddTab(Localization.Localize.Income, this);
    }
    private void AddCaption() {
        closeButton = AddUIComponent<CustomUIButton>();
        closeButton.Atlas = CustomUIAtlas.MbyronModsAtlas;
        closeButton.size = ButtonSize;
        closeButton.OnBgSprites.SetSprites(CustomUIAtlas.CloseButton);
        closeButton.OnBgSprites.SetColors(CustomUIColor.White, CustomUIColor.OffWhite, new Color32(180, 180, 180, 255), CustomUIColor.White, CustomUIColor.White);
        closeButton.relativePosition = new Vector2(width - 6f - 28f, 6f);
        closeButton.eventClicked += (c, p) => ControlPanelManager.Close();

        DragBar = AddUIComponent<UIDragHandle>();
        DragBar.width = closeButton.relativePosition.x;
        DragBar.height = CaptionHeight;
        DragBar.relativePosition = Vector2.zero;

        title = DragBar.AddUIComponent<CustomUILabel>();
        title.Text = ModMainInfo<Mod>.ModName;
        title.CenterToParent();
    }
}

