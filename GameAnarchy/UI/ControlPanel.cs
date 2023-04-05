using ColossalFramework.UI;
using UnityEngine;

namespace GameAnarchy.UI {
    internal class ControlPanel : UIPanel {
        private const string Name = nameof(GameAnarchy) + nameof(ControlPanel);
        private const float PanelWidth = 440;
        private const float PanelHeight = 600;
        private const float ElementOffset = 10;
        private const float CaptionHeight = 40;
        public const float CardWidth = PanelWidth - 2 * ElementOffset;
        public static Vector2 PanelPosition { get; set; }
        public static Vector2 ButtonSize => new(28, 28);

        private UIDragHandle DragBar { get; set; }
        private UILabel Title { get; set; }
        private UIButton CloseButton { get; set; }
        private UIButton ResetButton { get; set; }
        private ControlPanelTabs Tabs { get; set; }
        private AutoLayoutScrollablePanel GeneralContainer => Tabs.Containers[0].MainPanel;
        private AutoLayoutScrollablePanel ServiceContainer => Tabs.Containers[1].MainPanel;
        private AutoLayoutScrollablePanel IncomeContainer => Tabs.Containers[2].MainPanel;
        public ControlPanel() {
            name = Name;
            autoLayout = false;
            atlas = CustomAtlas.InGameAtlas;
            backgroundSprite = "UnlockingItemBackground";
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = PanelWidth;
            height = PanelHeight;

            AddCaption();
            AddTabStrip();
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
            ControlPanelTool.AddGroup(IncomeContainer, CardWidth, Localization.Localize.IncomeMultiplier);
            ControlPanelTool.AddField(Localization.Localize.Residential, null, 80, Config.Instance.ResidentialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.ResidentialMultiplierFactor = _, out CustomIntValueField _);
            ControlPanelTool.AddField(Localization.Localize.Industrial, null, 80, Config.Instance.IndustrialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.IndustrialMultiplierFactor = _, out CustomIntValueField _);
            ControlPanelTool.AddField(Localization.Localize.Commercial, null, 80, Config.Instance.CommercialMultiplierFactor, 10, 1, 100, (_) => Config.Instance.CommercialMultiplierFactor = _, out CustomIntValueField _);
            ControlPanelTool.AddField(Localization.Localize.Office, null, 80, Config.Instance.OfficeMultiplierFactor, 10, 1, 100, (_) => Config.Instance.OfficeMultiplierFactor = _, out CustomIntValueField _);
            ControlPanelTool.Reset();
        }

        public void FillServiceContainer() {
            ServiceContainer.width -= 10;
            ControlPanelTool.AddGroup(ServiceContainer, CardWidth - 10, Localization.Localize.RemovePollution);
            ControlPanelTool.AddToggleButton(Localization.Localize.NoisePollution, null, Config.Instance.RemoveNoisePollution, (v) => Config.Instance.RemoveNoisePollution = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.GroundPollution, null, Config.Instance.RemoveGroundPollution, (v) => Config.Instance.RemoveGroundPollution = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.WaterPollution, null, Config.Instance.RemoveWaterPollution, (v) => Config.Instance.RemoveWaterPollution = v, out ToggleButton _);
            ControlPanelTool.Reset();

            ControlPanelTool.AddGroup(ServiceContainer, CardWidth - 10, Localization.Localize.CityServiceOptions);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveDeath, null, Config.Instance.RemoveDeath, (v) => Config.Instance.RemoveDeath = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveCrime, null, Config.Instance.RemoveCrime, (v) => Config.Instance.RemoveCrime = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveGarbage, null, Config.Instance.RemoveGarbage, (v) => Config.Instance.RemoveGarbage = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.MaximizeAttractiveness, null, Config.Instance.MaximizeAttractiveness, (v) => Config.Instance.MaximizeAttractiveness = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.MaximizeEntertainment, null, Config.Instance.MaximizeEntertainment, (v) => Config.Instance.MaximizeEntertainment = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.MaximizeLandValue, null, Config.Instance.MaximizeLandValue, (v) => Config.Instance.MaximizeLandValue = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.MaximizeEducationCoverage, null, Config.Instance.MaximizeEducationCoverage, (v) => Config.Instance.MaximizeEducationCoverage = v, out ToggleButton _);
            ControlPanelTool.Reset();

            ControlPanelTool.AddGroup(ServiceContainer, CardWidth - 10, Localization.Localize.FireControl);
            ControlPanelTool.AddToggleButton(Localization.Localize.MaximizeFireCoverage, null, Config.Instance.MaximizeFireCoverage, (v) => Config.Instance.MaximizeFireCoverage = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemovePlayerBuildingFire, null, Config.Instance.RemovePlayerBuildingFire, (v) => Config.Instance.RemovePlayerBuildingFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveResidentialBuildingFire, null, Config.Instance.RemoveResidentialBuildingFire, (v) => Config.Instance.RemoveResidentialBuildingFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveIndustrialBuildingFire, null, Config.Instance.RemoveIndustrialBuildingFire, (v) => Config.Instance.RemoveIndustrialBuildingFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveCommercialBuildingFire, null, Config.Instance.RemoveCommercialBuildingFire, (v) => Config.Instance.RemoveCommercialBuildingFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveOfficeBuildingFire, null, Config.Instance.RemoveOfficeBuildingFire, (v) => Config.Instance.RemoveOfficeBuildingFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveParkBuildingFire, null, Config.Instance.RemoveParkBuildingFire, (v) => Config.Instance.RemoveParkBuildingFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveMuseumFire, null, Config.Instance.RemoveMuseumFire, (v) => Config.Instance.RemoveMuseumFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveCampusBuildingFire, null, Config.Instance.RemoveCampusBuildingFire, (v) => Config.Instance.RemoveCampusBuildingFire = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.RemoveAirportBuildingFire, null, Config.Instance.RemoveAirportBuildingFire, (v) => Config.Instance.RemoveAirportBuildingFire = v, out ToggleButton _);

            UILabel label0 = null;
            ControlPanelTool.AddSliderGamma(GetString(Localization.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla), null, new(408 - 10, 20), 0, 100, 1, Config.Instance.BuildingSpreadFireProbability, (_, value) => callback0(value), out label0, out UILabel _, out UISlider _);
            void callback0(float value) {
                Config.Instance.BuildingSpreadFireProbability = (uint)value;
                label0.text = GetString(Localization.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla);
            }
            UILabel label1 = null;
            ControlPanelTool.AddSliderGamma(GetString(Localization.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla), null, new(408 - 10, 20), 0, 100, 1, Config.Instance.TreeSpreadFireProbability, (_, value) => callback1(value), out label1, out UILabel _, out UISlider _);
            void callback1(float value) {
                Config.Instance.TreeSpreadFireProbability = (uint)value;
                label1.text = GetString(Localization.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla);
            }
            ControlPanelTool.Reset();
        }

        public void FillGeneralContainer() {
            ControlPanelTool.AddGroup(GeneralContainer, CardWidth, Localization.Localize.EnabledUnlimitedUniqueBuildings);
            ControlPanelTool.AddToggleButton(Localization.Localize.PlayerBuilding, null, Config.Instance.UnlimitedPlayerBuilding, (v) => Config.Instance.UnlimitedPlayerBuilding = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.Monument, null, Config.Instance.UnlimitedMonument, (v) => Config.Instance.UnlimitedMonument = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.MainCampusBuilding, null, Config.Instance.UnlimitedMainCampusBuilding, (v) => Config.Instance.UnlimitedMainCampusBuilding = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.UniqueFactory, null, Config.Instance.UnlimitedUniqueFactory, (v) => Config.Instance.UnlimitedUniqueFactory = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.StockExchange, null, Config.Instance.UnlimitedStockExchange, (v) => Config.Instance.UnlimitedStockExchange = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.UniqueFaculty, null, Config.Instance.UnlimitedUniqueFaculty, (v) => Config.Instance.UnlimitedUniqueFaculty = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.WeatherRadar, null, Config.Instance.UnlimitedWeatherRadar, (v) => Config.Instance.UnlimitedWeatherRadar = v, out ToggleButton _);
            ControlPanelTool.AddToggleButton(Localization.Localize.SpaceRadar, null, Config.Instance.UnlimitedSpaceRadar, (v) => Config.Instance.UnlimitedSpaceRadar = v, out ToggleButton _);
            ControlPanelTool.Reset();

            ControlPanelTool.AddGroup(GeneralContainer, CardWidth, Localization.Localize.ResourceOptions);
            UILabel label0 = null;
            ControlPanelTool.AddSliderGamma(GetString(Localization.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla), null, new(408, 20), 0, 100, 1, Config.Instance.OilDepletionRate, (_, value) => callback0(value), out label0, out UILabel _, out UISlider _);
            void callback0(float value) {
                Config.Instance.OilDepletionRate = (int)value;
                label0.text = GetString(Localization.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla);
            }
            UILabel label1 = null;
            ControlPanelTool.AddSliderGamma(GetString(Localization.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla), null, new(408, 20), 0, 100, 1, Config.Instance.OreDepletionRate, (_, value) => callback1(value), out label1, out UILabel _, out UISlider _);
            void callback1(float value) {
                Config.Instance.OreDepletionRate = (int)value;
                label1.text = GetString(Localization.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla);
            }
            ControlPanelTool.Reset();
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

        private void AddTabStrip() {
            Tabs = new ControlPanelTabs(this);
            Tabs.Initialize(PanelWidth - 20, 24);
            Tabs.TabPanel.atlas = CustomAtlas.MbyronModsAtlas;
            Tabs.TabPanel.backgroundSprite = CustomAtlas.RoundedRectangle2;
            Tabs.TabPanel.color = CustomColor.PrimaryNormal;
            Tabs.TabPanel.relativePosition = new Vector2(ElementOffset, CaptionHeight);
            Tabs.AddTab("General", CommonLocalize.OptionPanel_General, ElementOffset, CaptionHeight + 24, setSprite: SetSprite);
            Tabs.AddTab("Service", Localization.Localize.Service, ElementOffset, CaptionHeight + 24, setSprite: SetSprite);
            Tabs.AddTab("Income", Localization.Localize.Income, ElementOffset, CaptionHeight + 24, setSprite: SetSprite);
        }

        private static void SetSprite(TabButton tabButton) {
            tabButton.atlas = CustomAtlas.MbyronModsAtlas;
            tabButton.normalBgSprite = CustomAtlas.RoundedRectangle2;
            tabButton.focusedBgSprite = CustomAtlas.RoundedRectangle2;
            tabButton.hoveredBgSprite = CustomAtlas.RoundedRectangle2;
            tabButton.pressedBgSprite = CustomAtlas.RoundedRectangle2;
            tabButton.color = CustomColor.PrimaryNormal;
            tabButton.hoveredColor = CustomColor.PrimaryHovered;
        }

        private void AddCaption() {
            CloseButton = AddUIComponent<UIButton>();
            CloseButton.atlas = CustomAtlas.MbyronModsAtlas;
            CloseButton.size = ButtonSize;
            CloseButton.normalFgSprite = CustomAtlas.CloseButtonNormal;
            CloseButton.focusedFgSprite = CustomAtlas.CloseButtonNormal;
            CloseButton.hoveredFgSprite = CustomAtlas.CloseButtonHovered;
            CloseButton.pressedFgSprite = CustomAtlas.CloseButtonPressed;
            CloseButton.relativePosition = new Vector2(width - 6f - 28f, 6f);
            CloseButton.eventClicked += (c, p) => ControlPanelManager.Close();

            //ResetButton = AddUIComponent<UIButton>();
            //ResetButton.atlas = CustomAtlas.CommonAtlas;
            //ResetButton.size = ButtonSize;
            //ResetButton.normalFgSprite = CustomAtlas.ResetButtonNormal;
            //ResetButton.focusedFgSprite = CustomAtlas.ResetButtonNormal;
            //ResetButton.hoveredFgSprite = CustomAtlas.ResetButtonHovered;
            //ResetButton.pressedFgSprite = CustomAtlas.ResetButtonPressed;
            //ResetButton.relativePosition = new Vector2(width - 6f - 28f - 28f, 6f);
            //ResetButton.eventClicked += (c, p) => {

            //};
            ////ResetButton.tooltip = Localization.Localize.ControlPanel_Reset;
            //ResetButton.eventClicked += (c, v) => ResetButton.tooltipBox.Hide();

            DragBar = AddUIComponent<UIDragHandle>();
            DragBar.width = CloseButton.relativePosition.x;
            DragBar.height = CaptionHeight;
            DragBar.relativePosition = Vector2.zero;

            Title = DragBar.AddUIComponent<UILabel>();
            Title.textAlignment = UIHorizontalAlignment.Center;
            Title.verticalAlignment = UIVerticalAlignment.Middle;
            Title.text = ModMainInfo<Mod>.ModName;
            Title.CenterToParent();
        }
    }

    public class ControlPanelTabs : CustomTabs<ControlPanelScrollablePanel> {
        public ControlPanelTabs(UIComponent parent) : base(parent) { }
    }
    public class ControlPanelScrollablePanel : CustomScrollablePanelBase<AutoLayoutScrollablePanel> {
        public ControlPanelScrollablePanel() {
            clipChildren = true;
            size = new Vector2(430, 526);
            MainPanel.autoLayoutPadding = new RectOffset(0, 0, 0, 24);
        }
    }
}
