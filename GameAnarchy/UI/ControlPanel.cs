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
            ControlPanelTools.Instance.AddGroup(IncomeContainer, CardWidth, Localization.Localize.IncomeMultiplier, new RectOffset(6, 0, 0, 0), 0.9f, UIColor.White, new RectOffset(0, 0, 0, 4));
            ControlPanelTools.Instance.AddLabelWithField(Localization.Localize.Residential, 80, 20, Config.Instance.ResidentialMultiplierFactor, 10, 1, 100, true, out UILabel _, out CustomIntValueField field1);
            field1.OnValueChanged += (_) => Config.Instance.ResidentialMultiplierFactor = _;
            ControlPanelTools.Instance.AddLabelWithField(Localization.Localize.Industrial, 80, 20, Config.Instance.IndustrialMultiplierFactor, 10, 1, 100, true, out UILabel _, out CustomIntValueField field2);
            field2.OnValueChanged += (_) => Config.Instance.IndustrialMultiplierFactor = _;
            ControlPanelTools.Instance.AddLabelWithField(Localization.Localize.Commercial, 80, 20, Config.Instance.CommercialMultiplierFactor, 10, 1, 100, true, out UILabel _, out CustomIntValueField field3);
            field3.OnValueChanged += (_) => Config.Instance.CommercialMultiplierFactor = _;
            ControlPanelTools.Instance.AddLabelWithField(Localization.Localize.Office, 80, 20, Config.Instance.OfficeMultiplierFactor, 10, 1, 100, true, out UILabel _, out CustomIntValueField field4);
            field4.OnValueChanged += (_) => Config.Instance.OfficeMultiplierFactor = _;
            ControlPanelTools.Instance.Reset();
        }

        public void FillServiceContainer() {
            ServiceContainer.width -= 10;
            ControlPanelTools.Instance.AddGroup(ServiceContainer, CardWidth - 10, Localization.Localize.RemovePollution, new RectOffset(6, 0, 0, 0), 0.9f, UIColor.White, new RectOffset(0, 0, 0, 4));
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.NoisePollution, "I", "O", Config.Instance.RemoveNoisePollution, (v) => Config.Instance.RemoveNoisePollution = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.GroundPollution, "I", "O", Config.Instance.RemoveGroundPollution, (v) => Config.Instance.RemoveGroundPollution = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.WaterPollution, "I", "O", Config.Instance.RemoveWaterPollution, (v) => Config.Instance.RemoveWaterPollution = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.Reset();

            ControlPanelTools.Instance.AddGroup(ServiceContainer, CardWidth - 10, Localization.Localize.CityServiceOptions, new RectOffset(6, 0, 0, 0), 0.9f, UIColor.White, new RectOffset(0, 0, 0, 4));
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveDeath, "I", "O", Config.Instance.RemoveDeath, (v) => Config.Instance.RemoveDeath = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveCrime, "I", "O", Config.Instance.RemoveCrime, (v) => Config.Instance.RemoveCrime = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveGarbage, "I", "O", Config.Instance.RemoveGarbage, (v) => Config.Instance.RemoveGarbage = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.MaximizeAttractiveness, "I", "O", Config.Instance.MaximizeAttractiveness, (v) => Config.Instance.MaximizeAttractiveness = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.MaximizeEntertainment, "I", "O", Config.Instance.MaximizeEntertainment, (v) => Config.Instance.MaximizeEntertainment = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.MaximizeLandValue, "I", "O", Config.Instance.MaximizeLandValue, (v) => Config.Instance.MaximizeLandValue = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.MaximizeEducationCoverage, "I", "O", Config.Instance.MaximizeEducationCoverage, (v) => Config.Instance.MaximizeEducationCoverage = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.Reset();

            ControlPanelTools.Instance.AddGroup(ServiceContainer, CardWidth - 10, Localization.Localize.FireControl, new RectOffset(6, 0, 0, 0), 0.9f, UIColor.White, new RectOffset(0, 0, 0, 4));
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.MaximizeFireCoverage, "I", "O", Config.Instance.MaximizeFireCoverage, (v) => Config.Instance.MaximizeFireCoverage = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemovePlayerBuildingFire, "I", "O", Config.Instance.RemovePlayerBuildingFire, (v) => Config.Instance.RemovePlayerBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveResidentialBuildingFire, "I", "O", Config.Instance.RemoveResidentialBuildingFire, (v) => Config.Instance.RemoveResidentialBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveIndustrialBuildingFire, "I", "O", Config.Instance.RemoveIndustrialBuildingFire, (v) => Config.Instance.RemoveIndustrialBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveCommercialBuildingFire, "I", "O", Config.Instance.RemoveCommercialBuildingFire, (v) => Config.Instance.RemoveCommercialBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveOfficeBuildingFire, "I", "O", Config.Instance.RemoveOfficeBuildingFire, (v) => Config.Instance.RemoveOfficeBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveParkBuildingFire, "I", "O", Config.Instance.RemoveParkBuildingFire, (v) => Config.Instance.RemoveParkBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveMuseumFire, "I", "O", Config.Instance.RemoveMuseumFire, (v) => Config.Instance.RemoveMuseumFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveCampusBuildingFire, "I", "O", Config.Instance.RemoveCampusBuildingFire, (v) => Config.Instance.RemoveCampusBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.RemoveAirportBuildingFire, "I", "O", Config.Instance.RemoveAirportBuildingFire, (v) => Config.Instance.RemoveAirportBuildingFire = v == 0, out UILabel _, out PairButton _, 60, 20);

            UILabel label0 = null;
            ControlPanelTools.Instance.AddLabelWithSliderGamma(GetString(Localization.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla), new(408 - 10, 20), 0, 100, 1, Config.Instance.BuildingSpreadFireProbability, (_, value) => callback0(value), out label0, out UISlider _);
            void callback0(float value) {
                Config.Instance.BuildingSpreadFireProbability = (uint)value;
                label0.text = GetString(Localization.Localize.BuildingSpreadFireProbability, Config.Instance.BuildingSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla);
            }
            UILabel label1 = null;
            ControlPanelTools.Instance.AddLabelWithSliderGamma(GetString(Localization.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla), new(408 - 10, 20), 0, 100, 1, Config.Instance.TreeSpreadFireProbability, (_, value) => callback1(value), out label1, out UISlider _);
            void callback1(float value) {
                Config.Instance.TreeSpreadFireProbability = (uint)value;
                label1.text = GetString(Localization.Localize.TreeSpreadFireProbability, Config.Instance.TreeSpreadFireProbability, Localization.Localize.NoSpreadFire, Localization.Localize.Vanilla);
            }
            ControlPanelTools.Instance.Reset();
        }

        public void FillGeneralContainer() {
            ControlPanelTools.Instance.AddGroup(GeneralContainer, CardWidth, Localization.Localize.EnabledUnlimitedUniqueBuildings, new RectOffset(6, 0, 0, 0), 0.9f, UIColor.White, new RectOffset(0, 0, 0, 4));
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.PlayerBuilding, "I", "O", Config.Instance.UnlimitedPlayerBuilding, (v) => Config.Instance.UnlimitedPlayerBuilding = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.Monument, "I", "O", Config.Instance.UnlimitedMonument, (v) => Config.Instance.UnlimitedMonument = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.MainCampusBuilding, "I", "O", Config.Instance.UnlimitedMainCampusBuilding, (v) => Config.Instance.UnlimitedMainCampusBuilding = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.UniqueFactory, "I", "O", Config.Instance.UnlimitedUniqueFactory, (v) => Config.Instance.UnlimitedUniqueFactory = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.StockExchange, "I", "O", Config.Instance.UnlimitedStockExchange, (v) => Config.Instance.UnlimitedStockExchange = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.UniqueFaculty, "I", "O", Config.Instance.UnlimitedUniqueFaculty, (v) => Config.Instance.UnlimitedUniqueFaculty = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.WeatherRadar, "I", "O", Config.Instance.UnlimitedWeatherRadar, (v) => Config.Instance.UnlimitedWeatherRadar = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.AddLabelWithPairButton(Localization.Localize.SpaceRadar, "I", "O", Config.Instance.UnlimitedSpaceRadar, (v) => Config.Instance.UnlimitedSpaceRadar = v == 0, out UILabel _, out PairButton _, 60, 20);
            ControlPanelTools.Instance.Reset();

            ControlPanelTools.Instance.AddGroup(GeneralContainer, CardWidth, Localization.Localize.ResourceOptions, new RectOffset(6, 0, 0, 0), 0.9f, UIColor.White, new RectOffset(0, 0, 0, 4));
            UILabel label0 = null;
            ControlPanelTools.Instance.AddLabelWithSliderGamma(GetString(Localization.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla), new(408, 20), 0, 100, 1, Config.Instance.OilDepletionRate, (_, value) => callback0(value), out label0, out UISlider _);
            void callback0(float value) {
                Config.Instance.OilDepletionRate = (int)value;
                label0.text = GetString(Localization.Localize.OilDepletionRate, (uint)Config.Instance.OilDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla);
            }
            UILabel label1 = null;
            ControlPanelTools.Instance.AddLabelWithSliderGamma(GetString(Localization.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla), new(408, 20), 0, 100, 1, Config.Instance.OreDepletionRate, (_, value) => callback1(value), out label1, out UISlider _);
            void callback1(float value) {
                Config.Instance.OreDepletionRate = (int)value;
                label1.text = GetString(Localization.Localize.OreDepletionRate, (uint)Config.Instance.OreDepletionRate, Localization.Localize.Unlimited, Localization.Localize.Vanilla);
            }
            ControlPanelTools.Instance.Reset();
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
            Tabs.TabPanel.atlas = CustomAtlas.CommonAtlas;
            Tabs.TabPanel.backgroundSprite = CustomAtlas.TabNormal;
            Tabs.TabPanel.relativePosition = new Vector2(ElementOffset, CaptionHeight);
            Tabs.AddTab("General", CommonLocalize.OptionPanel_General, ElementOffset, CaptionHeight + 24, setSprite: SetSprite);
            Tabs.AddTab("Service", Localization.Localize.Service, ElementOffset, CaptionHeight + 24, setSprite: SetSprite);
            Tabs.AddTab("Income", Localization.Localize.Income, ElementOffset, CaptionHeight + 24, setSprite: SetSprite);
        }

        private static void SetSprite(TabButton tabButton) {
            tabButton.atlas = CustomAtlas.CommonAtlas;
            tabButton.normalBgSprite = CustomAtlas.TabNormal;
            tabButton.focusedBgSprite = CustomAtlas.TabFocused;
            tabButton.hoveredBgSprite = CustomAtlas.TabHovered;
            tabButton.pressedBgSprite = CustomAtlas.TabPressed;
        }

        private void AddCaption() {
            CloseButton = AddUIComponent<UIButton>();
            CloseButton.atlas = CustomAtlas.CommonAtlas;
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
