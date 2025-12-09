using CSLModsCommon.KeyBindings;
using CSLModsCommon.Setting;
using GameAnarchy.Data;
using UnityEngine;

namespace GameAnarchy.ModSettings;

[FileLocation(nameof(GameAnarchy) + nameof(ModSetting))]
public class ModSetting : ModSettingBase {
    public bool AchievementSystemEnabled { get; set; } = true;
    public bool SkipIntroEnabled { get; set; }
    public uint OptionPanelCategoriesHorizontalOffset { get; set; } = 240;
    public bool OptionPanelCategoriesUpdated { get; set; } = true;

    public bool UnlockInfoViews { get; set; }
    public UnlockMode CurrentUnlockMode { get; set; }
    public MilestoneLevel CurrentMilestoneLevel { get; set; }
    public MoneyMode CurrentMoneyMode { get; set; } = MoneyMode.Anarchy;
    public bool UnlockBasicRoads { get; set; }
    public bool UnlockAllRoads { get; set; }
    public bool UnlockTrainTrack { get; set; }
    public bool UnlockMetroTrack { get; set; }
    public bool UnlockPolicies { get; set; }
    public bool UnlockPublicTransport { get; set; }
    public bool UnlockUniqueBuildings { get; set; }
    public bool UnlockLandscaping { get; set; }

    public bool RemoveNoisePollution { get; set; }
    public bool RemoveGroundPollution { get; set; }
    public bool RemoveWaterPollution { get; set; }
    public bool RemoveDeath { get; set; }
    public bool RemoveGarbage { get; set; }
    public bool RemoveCrime { get; set; }
    public bool MaximizeAttractiveness { get; set; }
    public bool MaximizeEntertainment { get; set; }
    public bool MaximizeLandValue { get; set; }
    public bool MaximizeEducationCoverage { get; set; }

    public int OilDepletionRate { get; set; }
    public int OreDepletionRate { get; set; }
    public bool EnableStartMoney { get; set; }
    public bool RemoveNotEnoughMoney { get; set; }
    public int CityBankruptcyWarningThreshold { get; set; } = -10000;
    public long StartMoneyAmount { get; set; } = 715000;
    public int DefaultMinAmount { get; set; } = 50000;
    public int DefaultGetCash { get; set; } = 5000000;
    public bool ChargeInterest { get; set; }
    public float AnnualInterestRate { get; set; } = 0.03f;
    public bool NoPoliciesCosts { get; set; }

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

    public uint BuildingSpreadFireProbability { get; set; }
    public uint TreeSpreadFireProbability { get; set; }

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

    public bool MaximizeFireCoverage { get; set; }
    public bool RemovePlayerBuildingFire { get; set; }
    public bool RemoveResidentialBuildingFire { get; set; }
    public bool RemoveIndustrialBuildingFire { get; set; }
    public bool RemoveCommercialBuildingFire { get; set; }
    public bool RemoveOfficeBuildingFire { get; set; }
    public bool RemoveParkBuildingFire { get; set; }
    public bool RemoveMuseumFire { get; set; }
    public bool RemoveCampusBuildingFire { get; set; }
    public bool RemoveAirportBuildingFire { get; set; }

    public KeyBinding AddMoneyKeyBinding { get; set; } = new(new KeyCombination(KeyCode.M, true, true, false));
    public KeyBinding SubstrateMoneyKeyBinding { get; set; } = new(new KeyCombination(KeyCode.N, true, true, false));
    public KeyBinding ControlPanelToggleKeyBinding { get; set; } = new(new KeyCombination(KeyCode.G, true, true, false));

    public override void SetDefaults() {
        base.SetDefaults();
        AchievementSystemEnabled = true;
        SkipIntroEnabled = false;
        OptionPanelCategoriesHorizontalOffset = 240;
        OptionPanelCategoriesUpdated = true;
        UnlockInfoViews = false;
        UnlockBasicRoads = false;
        UnlockAllRoads = false;
        UnlockTrainTrack = false;
        UnlockMetroTrack = false;
        CurrentUnlockMode = UnlockMode.Vanilla;
        CurrentMilestoneLevel = MilestoneLevel.Vanilla;
        CurrentMoneyMode = MoneyMode.Anarchy;
        UnlockPolicies = false;
        UnlockPublicTransport = false;
        UnlockUniqueBuildings = false;
        UnlockLandscaping = false;
        RemoveNoisePollution = false;
        RemoveGroundPollution = false;
        RemoveWaterPollution = false;
        RemoveDeath = false;
        RemoveGarbage = false;
        RemoveCrime = false;
        MaximizeAttractiveness = false;
        MaximizeEntertainment = false;
        MaximizeLandValue = false;
        MaximizeEducationCoverage = false;
        OilDepletionRate = 0;
        OreDepletionRate = 0;
        EnableStartMoney = false;
        RemoveNotEnoughMoney = false;
        CityBankruptcyWarningThreshold = -10000;
        StartMoneyAmount = 715000;
        DefaultMinAmount = 50000;
        DefaultGetCash = 5000000;
        ChargeInterest = false;
        AnnualInterestRate = 0.03f;
        NoPoliciesCosts = false;
        BuildingRefund = false;
        RemoveBuildingRefundTimeLimitation = true;
        BuildingRefundMultipleFactor = 1;
        BuildingRelocationCostFactor = 0.2f;
        SegmentRefund = true;
        RemoveSegmentRefundTimeLimitation = true;
        SegmentRefundMultipleFactor = 1;
        ResidentialMultiplierFactor = 1;
        IndustrialMultiplierFactor = 1;
        CommercialMultiplierFactor = 1;
        OfficeMultiplierFactor = 1;
        BuildingSpreadFireProbability = 0;
        TreeSpreadFireProbability = 0;
        UnlimitedMonument = true;
        UnlimitedMainCampusBuilding = true;
        UnlimitedUniqueFactory = true;
        UnlimitedStockExchange = true;
        UnlimitedUniqueFaculty = true;
        UnlimitedWeatherRadar = true;
        UnlimitedSpaceRadar = true;
        UnlimitedFestivalArea = true;
        UnlimitedLibraryAI = true;
        UnlimitedSpaceElevator = true;
        UnlimitedParkAI = true;
        MaximizeFireCoverage = false;
        RemovePlayerBuildingFire = false;
        RemoveResidentialBuildingFire = false;
        RemoveIndustrialBuildingFire = false;
        RemoveCommercialBuildingFire = false;
        RemoveOfficeBuildingFire = false;
        RemoveParkBuildingFire = false;
        RemoveMuseumFire = false;
        RemoveCampusBuildingFire = false;
        RemoveAirportBuildingFire = false;
        AddMoneyKeyBinding.Reset();
        SubstrateMoneyKeyBinding.Reset();
        ControlPanelToggleKeyBinding.Reset();
    }
}