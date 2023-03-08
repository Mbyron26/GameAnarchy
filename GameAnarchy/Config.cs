using System.Xml.Serialization;
using UnityEngine;

namespace GameAnarchy {
    [XmlRoot("ModConfig")]
    public class Config : ModConfigBase<Config> {
        public bool EnabledAchievements { get; set; } = true;
        public bool EnabledSkipIntro { get; set; } = false;
        public bool EnabledUnlimitedUniqueBuildings { get; set; } = true;
        public uint OptionPanelCategoriesOffset { get; set; } = 0;

        public bool EnabledInfoView { get; set; } = false;
        public bool EnabledUnlockAll { get; set; } = true;
        public bool CustomUnlock { get; set; } = false;
        public bool UnlockAllRoads { get; set; } = false;
        public bool UnlockTrainTrack { get; set; } = false;
        public bool UnlockMetroTrack { get; set; } = false;
        public int MilestoneLevel { get; set; } = 0;
        public bool UnlockPolicies { get; set; } = false;
        public bool UnlockTransport { get; set; } = false;
        public bool UnlockUniqueBuilding { get; set; } = false;

        public bool RemoveNoisePollution { get; set; } = false;
        public bool RemoveGroundPollution { get; set; } = false;
        public bool RemoveWaterPollution { get; set; } = false;
        public bool RemoveDeath { get; set; } = false;
        public bool RemoveGarbage { get; set; } = false;
        public bool RemoveCrime { get; set; } = false;
        public bool RemoveFire { get; set; } = false;
        public bool MaximizeAttractiveness { get; set; } = false;
        public bool MaximizeEntertainment { get; set; } = false;
        public bool MaximizeLandValue { get; set; } = false;
        public bool MaximizeEducationCoverage { get; set; } = false;

        public int OilDepletionRate { get; set; } = 100;
        public int OreDepletionRate { get; set; } = 100;
        public bool CashAnarchy { get; set; } = false;
        public bool UnlimitedMoney { get; set; } = false;
        public bool EnabledInitialCash { get; set; } = false;
        public long InitialCash { get; set; } = 715000;

        public int DefaultMinAmount { get; set; } = 50000;
        public int DefaultGetCash { get; set; } = 5000000;
        public bool Refund { get; set; } = true;


        public int ResidentialMultiplierFactor { get; set; } = 1;
        public int IndustrialMultiplierFactor { get; set; } = 1;
        public int CommercialMultiplierFactor { get; set; } = 1;
        public int OfficeMultiplierFactor { get; set; } = 1;

        public uint BuildingSpreadFireProbability { get; set; } = 0;
        public uint TreeSpreadFireProbability { get; set; } = 0;

        public bool UnlimitedPlayerBuilding { get; set; } = false;
        public bool UnlimitedMonument { get; set; } = false;
        public bool UnlimitedMainCampusBuilding { get; set; } = false;
        public bool UnlimitedUniqueFactory { get; set; } = false;
        public bool UnlimitedStockExchange { get; set; } = false;
        public bool UnlimitedUniqueFaculty { get; set; } = false;
        public bool UnlimitedWeatherRadar { get; set; } = false;
        public bool UnlimitedSpaceRadar { get; set; } = false;

        public bool MaximizeFireCoverage { get; set; } = false;
        public bool RemovePlayerBuildingFire { get; set; } = false;
        public bool RemoveResidentialBuildingFire { get; set; } = false;
        public bool RemoveIndustrialBuildingFire { get; set; } = false;
        public bool RemoveCommercialBuildingFire { get; set; } = false;
        public bool RemoveOfficeBuildingFire { get; set; } = false;
        public bool RemoveParkBuildingFire { get; set; } = false;
        public bool RemoveMuseumFire { get; set; } = false;
        public bool RemoveCampusBuildingFire { get; set; } = false;
        public bool RemoveAirportBuildingFire { get; set; } = false;

        public KeyBinding AddCash { get; set; } = new KeyBinding(KeyCode.M, true, true, false);
        public KeyBinding ShowControlPanelHotkey { get; set; } = new KeyBinding(KeyCode.G, true, true, false);
    }
}
