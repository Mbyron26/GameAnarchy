using CSShared.Common;
using System.Xml.Serialization;
using UnityEngine;

namespace GameAnarchy;

[XmlRoot("ModConfig")]
public class Config : SingletonConfig<Config> {
    public bool EnabledAchievements { get; set; } = true;
    public bool EnabledSkipIntro { get; set; } = false;
    public uint OptionPanelCategoriesHorizontalOffset { get; set; } = 240;
    public bool OptionPanelCategoriesUpdated { get; set; } = true;

    public bool UnlockInfoViews { get; set; } = false;
    public bool EnabledUnlockAll { get; set; } = true;
    public bool CustomUnlock { get; set; } = false;
    public bool UnlockBasicRoads { get; set; } = false;
    public bool UnlockAllRoads { get; set; } = false;
    public bool UnlockTrainTrack { get; set; } = false;
    public bool UnlockMetroTrack { get; set; } = false;
    public int MilestoneLevel { get; set; } = 0;
    public bool UnlockPolicies { get; set; } = false;
    public bool UnlockPublicTransport { get; set; } = false;
    public bool UnlockUniqueBuildings { get; set; } = false;
    public bool UnlockLandscaping { get; set; } = false;

    public bool RemoveNoisePollution { get; set; } = false;
    public bool RemoveGroundPollution { get; set; } = false;
    public bool RemoveWaterPollution { get; set; } = false;
    public bool RemoveDeath { get; set; } = false;
    public bool RemoveGarbage { get; set; } = false;
    public bool RemoveCrime { get; set; } = false;
    public bool MaximizeAttractiveness { get; set; } = false;
    public bool MaximizeEntertainment { get; set; } = false;
    public bool MaximizeLandValue { get; set; } = false;
    public bool MaximizeEducationCoverage { get; set; } = false;

    public int OilDepletionRate { get; set; } = 0;
    public int OreDepletionRate { get; set; } = 0;
    public bool CashAnarchy { get; set; } = false;
    public bool UnlimitedMoney { get; set; } = false;
    public bool EnableStartMoney { get; set; } = false;
    public bool RemoveNotEnoughMoney { get; set; } = false;
    public int CityBankruptcyWarningThreshold { get; set; } = -10000;
    public long StartMoneyAmount { get; set; } = 715000;
    public int DefaultMinAmount { get; set; } = 50000;
    public int DefaultGetCash { get; set; } = 5000000;
    public bool ChargeInterest { get; set; } = false;
    public float AnnualInterestRate { get; set; } = 0.03f;
    public bool NoPoliciesCosts { get; set; } = false;

    public bool BuildingRefund { get; set; } = true;
    public bool RemoveBuildingRefundTimeLimitation { get; set; } = true;
    public float BuildingRefundMultipleFactor { get; set; } = 1;
    public float BuildingRelocationCostFactor { get; set; } = 0.2f;
    public bool SegmentRefund { get; set; } = true;
    public bool RemoveSegmentRefundTimeLimitation { get; set; } = true;
    public float SegmentRefundMultipleFactor { get; set; } = 1;

    public int ResidentialMultiplierFactor { get; set; } = 1;
    public int IndustrialMultiplierFactor { get; set; } = 1;
    public int CommercialMultiplierFactor { get; set; } = 1;
    public int OfficeMultiplierFactor { get; set; } = 1;

    public uint BuildingSpreadFireProbability { get; set; } = 0;
    public uint TreeSpreadFireProbability { get; set; } = 0;

    public bool UnlimitedMonument { get; set; } = true;
    public bool UnlimitedMainCampusBuilding { get; set; } = true;
    public bool UnlimitedUniqueFactory { get; set; } = true;
    public bool UnlimitedStockExchange { get; set; } = true;
    public bool UnlimitedUniqueFaculty { get; set; } = true;
    public bool UnlimitedWeatherRadar { get; set; } = true;
    public bool UnlimitedSpaceRadar { get; set; } = true;
    public bool UnlimitedFestivalArea { get; set; } = true;
    public bool UnlimitedLibraryAI { get; set; } = true;
    public bool UnlimitedSpaceElevator { get; set; } = true;
    public bool UnlimitedParkAI { get; set; } = true;

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
    public KeyBinding DecreaseMoney { get; set; } = new KeyBinding(KeyCode.N, true, true, false);
    public KeyBinding ControlPanelHotkey { get; set; } = new KeyBinding(KeyCode.G, true, true, false);
}