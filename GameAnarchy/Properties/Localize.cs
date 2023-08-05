namespace GameAnarchy
{
	public class Localize
	{
		public static System.Globalization.CultureInfo Culture {get; set;}
		public static MbyronModsCommon.LocalizeManager LocaleManager {get;} = new MbyronModsCommon.LocalizeManager("Localize", typeof(Localize).Assembly);

		/// <summary>
		/// Add money
		/// </summary>
		public static string AddMoney => LocaleManager.GetString("AddMoney", Culture);

		/// <summary>
		/// Add money amount
		/// </summary>
		public static string AddMoneyAmount => LocaleManager.GetString("AddMoneyAmount", Culture);

		/// <summary>
		/// Add money threshold
		/// </summary>
		public static string AddMoneyThreshold => LocaleManager.GetString("AddMoneyThreshold", Culture);

		/// <summary>
		/// Make sure Money Anarchy feature is enabled
		/// </summary>
		public static string AddMoneyTooltip => LocaleManager.GetString("AddMoneyTooltip", Culture);

		/// <summary>
		/// This option allows dynamic toggle within game
		/// </summary>
		public static string AllowsDynamicToggling => LocaleManager.GetString("AllowsDynamicToggling", Culture);

		/// <summary>
		/// Amount
		/// </summary>
		public static string Amount => LocaleManager.GetString("Amount", Culture);

		/// <summary>
		/// Annual interest rate
		/// </summary>
		public static string AnnualInterestRate => LocaleManager.GetString("AnnualInterestRate", Culture);

		/// <summary>
		/// The default value is 3%
		/// </summary>
		public static string AnnualInterestRateMinor => LocaleManager.GetString("AnnualInterestRateMinor", Culture);

		/// <summary>
		/// Building refund
		/// </summary>
		public static string BuildingRefund => LocaleManager.GetString("BuildingRefund", Culture);

		/// <summary>
		/// Building refund multiple factor
		/// </summary>
		public static string BuildingRefundMultipleFactor => LocaleManager.GetString("BuildingRefundMultipleFactor", Culture);

		/// <summary>
		/// Building spread fire probability
		/// </summary>
		public static string BuildingSpreadFireProbability => LocaleManager.GetString("BuildingSpreadFireProbability", Culture);

		/// <summary>
		/// Builtin Mod Check
		/// </summary>
		public static string BuiltinModCheck => LocaleManager.GetString("BuiltinModCheck", Culture);

		/// <summary>
		/// Game Anarchy already includes all the features of this mods, to ensure the normal operation of Game 
		/// </summary>
		public static string BuiltinModWarning => LocaleManager.GetString("BuiltinModWarning", Culture);

		/// <summary>
		/// Charge interest on negative balances
		/// </summary>
		public static string ChargeInterest => LocaleManager.GetString("ChargeInterest", Culture);

		/// <summary>
		/// City bankruptcy warning threshold
		/// </summary>
		public static string CityBankruptcyWarningThreshold => LocaleManager.GetString("CityBankruptcyWarningThreshold", Culture);

		/// <summary>
		/// Game default value is -10000
		/// </summary>
		public static string CityBankruptcyWarningThresholdMinor => LocaleManager.GetString("CityBankruptcyWarningThresholdMinor", Culture);

		/// <summary>
		/// City Service Options
		/// </summary>
		public static string CityServiceOptions => LocaleManager.GetString("CityServiceOptions", Culture);

		/// <summary>
		/// Commercial
		/// </summary>
		public static string Commercial => LocaleManager.GetString("Commercial", Culture);

		/// <summary>
		/// Cost
		/// </summary>
		public static string Cost => LocaleManager.GetString("Cost", Culture);

		/// <summary>
		/// Custom unlock
		/// </summary>
		public static string CustomUnlock => LocaleManager.GetString("CustomUnlock", Culture);

		/// <summary>
		/// These options are not available when “Unlock All” feature is enabled because Unlock All has unlocked
		/// </summary>
		public static string CustomUnlockMinor => LocaleManager.GetString("CustomUnlockMinor", Culture);

		/// <summary>
		/// Economy
		/// </summary>
		public static string Economy => LocaleManager.GetString("Economy", Culture);

		/// <summary>
		/// Achievements system is always available
		/// </summary>
		public static string EnableAchievements => LocaleManager.GetString("EnableAchievements", Culture);

		/// <summary>
		/// Skip game Intro interface
		/// </summary>
		public static string EnabledSkipIntro => LocaleManager.GetString("EnabledSkipIntro", Culture);

		/// <summary>
		/// Place unique buildings without limit
		/// </summary>
		public static string EnabledUnlimitedUniqueBuildings => LocaleManager.GetString("EnabledUnlimitedUniqueBuildings", Culture);

		/// <summary>
		/// Fast Return
		/// </summary>
		public static string FastReturn => LocaleManager.GetString("FastReturn", Culture);

		/// <summary>
		/// Festival area
		/// </summary>
		public static string FestivalArea => LocaleManager.GetString("FestivalArea", Culture);

		/// <summary>
		/// Fire Control
		/// </summary>
		public static string FireControl => LocaleManager.GetString("FireControl", Culture);

		/// <summary>
		/// Free
		/// </summary>
		public static string Free => LocaleManager.GetString("Free", Culture);

		/// <summary>
		/// Ground pollution
		/// </summary>
		public static string GroundPollution => LocaleManager.GetString("GroundPollution", Culture);

		/// <summary>
		/// Income
		/// </summary>
		public static string Income => LocaleManager.GetString("Income", Culture);

		/// <summary>
		/// Income multiplier
		/// </summary>
		public static string IncomeMultiplier => LocaleManager.GetString("IncomeMultiplier", Culture);

		/// <summary>
		/// Industrial
		/// </summary>
		public static string Industrial => LocaleManager.GetString("Industrial", Culture);

		/// <summary>
		/// Main campus building
		/// </summary>
		public static string MainCampusBuilding => LocaleManager.GetString("MainCampusBuilding", Culture);

		/// <summary>
		/// Maximize attractiveness, will atract more tourists
		/// </summary>
		public static string MaximizeAttractiveness => LocaleManager.GetString("MaximizeAttractiveness", Culture);

		/// <summary>
		/// Maximize education coverage, still need schools to get educated citizens
		/// </summary>
		public static string MaximizeEducationCoverage => LocaleManager.GetString("MaximizeEducationCoverage", Culture);

		/// <summary>
		/// Maximize entertainment, will improve citizens' happiness
		/// </summary>
		public static string MaximizeEntertainment => LocaleManager.GetString("MaximizeEntertainment", Culture);

		/// <summary>
		/// Maximize fire coverage
		/// </summary>
		public static string MaximizeFireCoverage => LocaleManager.GetString("MaximizeFireCoverage", Culture);

		/// <summary>
		/// Maximize the land value of city
		/// </summary>
		public static string MaximizeLandValue => LocaleManager.GetString("MaximizeLandValue", Culture);

		/// <summary>
		/// Big City
		/// </summary>
		public static string MilestonelevelName_BigCity => LocaleManager.GetString("MilestonelevelName_BigCity", Culture);

		/// <summary>
		/// Big Town
		/// </summary>
		public static string MilestonelevelName_BigTown => LocaleManager.GetString("MilestonelevelName_BigTown", Culture);

		/// <summary>
		/// Boom Town
		/// </summary>
		public static string MilestonelevelName_BoomTown => LocaleManager.GetString("MilestonelevelName_BoomTown", Culture);

		/// <summary>
		/// Busy Town
		/// </summary>
		public static string MilestonelevelName_BusyTown => LocaleManager.GetString("MilestonelevelName_BusyTown", Culture);

		/// <summary>
		/// Capital City
		/// </summary>
		public static string MilestonelevelName_CapitalCity => LocaleManager.GetString("MilestonelevelName_CapitalCity", Culture);

		/// <summary>
		/// Colossal City
		/// </summary>
		public static string MilestonelevelName_ColossalCity => LocaleManager.GetString("MilestonelevelName_ColossalCity", Culture);

		/// <summary>
		/// Grand City
		/// </summary>
		public static string MilestonelevelName_GrandCity => LocaleManager.GetString("MilestonelevelName_GrandCity", Culture);

		/// <summary>
		/// Little Hamlet
		/// </summary>
		public static string MilestonelevelName_LittleHamlet => LocaleManager.GetString("MilestonelevelName_LittleHamlet", Culture);

		/// <summary>
		/// Megalopolis
		/// </summary>
		public static string MilestonelevelName_Megalopolis => LocaleManager.GetString("MilestonelevelName_Megalopolis", Culture);

		/// <summary>
		/// Metropolis
		/// </summary>
		public static string MilestonelevelName_Metropolis => LocaleManager.GetString("MilestonelevelName_Metropolis", Culture);

		/// <summary>
		/// Milestone unlock level
		/// </summary>
		public static string MilestonelevelName_MilestoneUnlockLevel => LocaleManager.GetString("MilestonelevelName_MilestoneUnlockLevel", Culture);

		/// <summary>
		/// Small City
		/// </summary>
		public static string MilestonelevelName_SmallCity => LocaleManager.GetString("MilestonelevelName_SmallCity", Culture);

		/// <summary>
		/// Tiny Town
		/// </summary>
		public static string MilestonelevelName_TinyTown => LocaleManager.GetString("MilestonelevelName_TinyTown", Culture);

		/// <summary>
		/// Vanilla
		/// </summary>
		public static string MilestonelevelName_Vanilla => LocaleManager.GetString("MilestonelevelName_Vanilla", Culture);

		/// <summary>
		/// Worthy Village
		/// </summary>
		public static string MilestonelevelName_WorthyVillage => LocaleManager.GetString("MilestonelevelName_WorthyVillage", Culture);

		/// <summary>
		/// Extends and optimize game's functions.
		/// </summary>
		public static string MOD_Description => LocaleManager.GetString("MOD_Description", Culture);

		/// <summary>
		/// Money anarchy mode
		/// </summary>
		public static string MoneyAnarchyMode => LocaleManager.GetString("MoneyAnarchyMode", Culture);

		/// <summary>
		/// This option allows automatic (set the following 2 options)/manual (use hotkey) set money, allows dyn
		/// </summary>
		public static string MoneyAnarchyModeMinor => LocaleManager.GetString("MoneyAnarchyModeMinor", Culture);

		/// <summary>
		/// Monument
		/// </summary>
		public static string Monument => LocaleManager.GetString("Monument", Culture);

		/// <summary>
		/// Noise pollution
		/// </summary>
		public static string NoisePollution => LocaleManager.GetString("NoisePollution", Culture);

		/// <summary>
		/// Non-spread of fire
		/// </summary>
		public static string NoSpreadFire => LocaleManager.GetString("NoSpreadFire", Culture);

		/// <summary>
		/// Office
		/// </summary>
		public static string Office => LocaleManager.GetString("Office", Culture);

		/// <summary>
		/// Oil depletion rate
		/// </summary>
		public static string OilDepletionRate => LocaleManager.GetString("OilDepletionRate", Culture);

		/// <summary>
		/// Open Control Panel
		/// </summary>
		public static string OpenControlPanel => LocaleManager.GetString("OpenControlPanel", Culture);

		/// <summary>
		/// Optimize Options
		/// </summary>
		public static string OptimizeOptions => LocaleManager.GetString("OptimizeOptions", Culture);

		/// <summary>
		/// This option takes effect after adjustment
		/// </summary>
		public static string OptionPanelCategoriesHorizontalOffsetMinor => LocaleManager.GetString("OptionPanelCategoriesHorizontalOffsetMinor", Culture);

		/// <summary>
		/// Show mod updated date in options panel categories
		/// </summary>
		public static string OptionPanelCategoriesUpdated => LocaleManager.GetString("OptionPanelCategoriesUpdated", Culture);

		/// <summary>
		/// This option only takes effect after refreshing the plugins list or restarting the game
		/// </summary>
		public static string OptionPanelCategoriesUpdatedMinor => LocaleManager.GetString("OptionPanelCategoriesUpdatedMinor", Culture);

		/// <summary>
		/// Options panel categories bar horizontal offset
		/// </summary>
		public static string OptionsPanelHorizontalOffset => LocaleManager.GetString("OptionsPanelHorizontalOffset", Culture);

		/// <summary>
		/// Ore depletion rate
		/// </summary>
		public static string OreDepletionRate => LocaleManager.GetString("OreDepletionRate", Culture);

		/// <summary>
		/// Other functions
		/// </summary>
		public static string OtherFunctionsMajor => LocaleManager.GetString("OtherFunctionsMajor", Culture);

		/// <summary>
		/// Use the shortcut keys or the UUI button to invoke the Control Panel, if you want to adjust the optio
		/// </summary>
		public static string OtherFunctionsMinor => LocaleManager.GetString("OtherFunctionsMinor", Culture);

		/// <summary>
		/// Put out
		/// </summary>
		public static string PutOut => LocaleManager.GetString("PutOut", Culture);

		/// <summary>
		/// Put out burning buildings
		/// </summary>
		public static string PutOutBurningBuildings => LocaleManager.GetString("PutOutBurningBuildings", Culture);

		/// <summary>
		/// This is not a permanent fire removal feature and will only be called once to remove all buildings th
		/// </summary>
		public static string PutOutBurningBuildingsDescription => LocaleManager.GetString("PutOutBurningBuildingsDescription", Culture);

		/// <summary>
		/// Bulldozing refund
		/// </summary>
		public static string Refund => LocaleManager.GetString("Refund", Culture);

		/// <summary>
		/// Relocate building
		/// </summary>
		public static string RelocateBuilding => LocaleManager.GetString("RelocateBuilding", Culture);

		/// <summary>
		/// Construction cost * Multiplier factor = Cost
		/// </summary>
		public static string RelocateBuildingMinor => LocaleManager.GetString("RelocateBuildingMinor", Culture);

		/// <summary>
		/// Remove airport building fire
		/// </summary>
		public static string RemoveAirportBuildingFire => LocaleManager.GetString("RemoveAirportBuildingFire", Culture);

		/// <summary>
		/// Remove building refund time limitation
		/// </summary>
		public static string RemoveBuildingRefundTimeLimitation => LocaleManager.GetString("RemoveBuildingRefundTimeLimitation", Culture);

		/// <summary>
		/// Remove campus building fire
		/// </summary>
		public static string RemoveCampusBuildingFire => LocaleManager.GetString("RemoveCampusBuildingFire", Culture);

		/// <summary>
		/// Remove commercial building fire
		/// </summary>
		public static string RemoveCommercialBuildingFire => LocaleManager.GetString("RemoveCommercialBuildingFire", Culture);

		/// <summary>
		/// Remove criminals from all buildings and maximize police service coverage
		/// </summary>
		public static string RemoveCrime => LocaleManager.GetString("RemoveCrime", Culture);

		/// <summary>
		/// Remove dead citizens and maximize death care coverage
		/// </summary>
		public static string RemoveDeath => LocaleManager.GetString("RemoveDeath", Culture);

		/// <summary>
		/// Remove garbage
		/// </summary>
		public static string RemoveGarbage => LocaleManager.GetString("RemoveGarbage", Culture);

		/// <summary>
		/// Remove industrial building fire
		/// </summary>
		public static string RemoveIndustrialBuildingFire => LocaleManager.GetString("RemoveIndustrialBuildingFire", Culture);

		/// <summary>
		/// Remove museum fire
		/// </summary>
		public static string RemoveMuseumFire => LocaleManager.GetString("RemoveMuseumFire", Culture);

		/// <summary>
		/// Remove not enough money limitation
		/// </summary>
		public static string RemoveNotEnoughMoney => LocaleManager.GetString("RemoveNotEnoughMoney", Culture);

		/// <summary>
		/// When “Money Anarchy” or "Unlimited Money" is enabled, this option is enabled automatically enforced.
		/// </summary>
		public static string RemoveNotEnoughMoneyMinor => LocaleManager.GetString("RemoveNotEnoughMoneyMinor", Culture);

		/// <summary>
		/// Remove office building fire
		/// </summary>
		public static string RemoveOfficeBuildingFire => LocaleManager.GetString("RemoveOfficeBuildingFire", Culture);

		/// <summary>
		/// Remove park building fire
		/// </summary>
		public static string RemoveParkBuildingFire => LocaleManager.GetString("RemoveParkBuildingFire", Culture);

		/// <summary>
		/// Remove player building fire
		/// </summary>
		public static string RemovePlayerBuildingFire => LocaleManager.GetString("RemovePlayerBuildingFire", Culture);

		/// <summary>
		/// Remove pollution
		/// </summary>
		public static string RemovePollution => LocaleManager.GetString("RemovePollution", Culture);

		/// <summary>
		/// Remove residential building fire
		/// </summary>
		public static string RemoveResidentialBuildingFire => LocaleManager.GetString("RemoveResidentialBuildingFire", Culture);

		/// <summary>
		/// Remove Segment refund time limitation
		/// </summary>
		public static string RemoveSegmentRefundTimeLimitation => LocaleManager.GetString("RemoveSegmentRefundTimeLimitation", Culture);

		/// <summary>
		/// Residential
		/// </summary>
		public static string Residential => LocaleManager.GetString("Residential", Culture);

		/// <summary>
		/// Resource Options
		/// </summary>
		public static string ResourceOptions => LocaleManager.GetString("ResourceOptions", Culture);

		/// <summary>
		/// Segment refund
		/// </summary>
		public static string SegmentRefund => LocaleManager.GetString("SegmentRefund", Culture);

		/// <summary>
		/// Segment refund multiple factor
		/// </summary>
		public static string SegmentRefundMultipleFactor => LocaleManager.GetString("SegmentRefundMultipleFactor", Culture);

		/// <summary>
		/// Service
		/// </summary>
		public static string Service => LocaleManager.GetString("Service", Culture);

		/// <summary>
		/// It is recommended to set this before entering the game/save
		/// </summary>
		public static string SetBefore => LocaleManager.GetString("SetBefore", Culture);

		/// <summary>
		/// Sort mods names in options panel
		/// </summary>
		public static string SortSettings => LocaleManager.GetString("SortSettings", Culture);

		/// <summary>
		/// Space radar
		/// </summary>
		public static string SpaceRadar => LocaleManager.GetString("SpaceRadar", Culture);

		/// <summary>
		/// Game start money
		/// </summary>
		public static string StartMoneyMajor => LocaleManager.GetString("StartMoneyMajor", Culture);

		/// <summary>
		/// Set value before you start gaming. If you don't want money to be this value every time you load save
		/// </summary>
		public static string StartMoneyMinor => LocaleManager.GetString("StartMoneyMinor", Culture);

		/// <summary>
		/// Stock exchange
		/// </summary>
		public static string StockExchange => LocaleManager.GetString("StockExchange", Culture);

		/// <summary>
		/// Tree spread fire probability
		/// </summary>
		public static string TreeSpreadFireProbability => LocaleManager.GetString("TreeSpreadFireProbability", Culture);

		/// <summary>
		/// Unique factory
		/// </summary>
		public static string UniqueFactory => LocaleManager.GetString("UniqueFactory", Culture);

		/// <summary>
		/// Unique faculty
		/// </summary>
		public static string UniqueFaculty => LocaleManager.GetString("UniqueFaculty", Culture);

		/// <summary>
		/// Unlimited
		/// </summary>
		public static string Unlimited => LocaleManager.GetString("Unlimited", Culture);

		/// <summary>
		/// Library
		/// </summary>
		public static string UnlimitedLibraryAI => LocaleManager.GetString("UnlimitedLibraryAI", Culture);

		/// <summary>
		/// The Creator's Library
		/// </summary>
		public static string UnlimitedLibraryAIMinor => LocaleManager.GetString("UnlimitedLibraryAIMinor", Culture);

		/// <summary>
		/// Game Anarchy also extended money anarchy function, allowing manual add money, automatic add money, c
		/// </summary>
		public static string UnlimitedMoneyConflict => LocaleManager.GetString("UnlimitedMoneyConflict", Culture);

		/// <summary>
		/// Game Anarchy also can adjuster consumption rate, check Game Anarchy control panel in game.
		/// </summary>
		public static string UnlimitedOilAndOreConflict => LocaleManager.GetString("UnlimitedOilAndOreConflict", Culture);

		/// <summary>
		/// Park
		/// </summary>
		public static string UnlimitedParkAI => LocaleManager.GetString("UnlimitedParkAI", Culture);

		/// <summary>
		/// Plaza of the Future
		/// </summary>
		public static string UnlimitedParkAIMinor => LocaleManager.GetString("UnlimitedParkAIMinor", Culture);

		/// <summary>
		/// Space elevator
		/// </summary>
		public static string UnlimitedSpaceElevator => LocaleManager.GetString("UnlimitedSpaceElevator", Culture);

		/// <summary>
		/// Plaza of Transference
		/// </summary>
		public static string UnlimitedSpaceElevatorMinor => LocaleManager.GetString("UnlimitedSpaceElevatorMinor", Culture);

		/// <summary>
		/// Unlock all
		/// </summary>
		public static string UnlockAll => LocaleManager.GetString("UnlockAll", Culture);

		/// <summary>
		/// Game Anarchy also extend the custom unlock, check Game Anarchy option panel.
		/// </summary>
		public static string UnlockAllConflict => LocaleManager.GetString("UnlockAllConflict", Culture);

		/// <summary>
		/// It is recommended to set this before you start the game. If disabled this option still unlocks somet
		/// </summary>
		public static string UnlockAllMinor => LocaleManager.GetString("UnlockAllMinor", Culture);

		/// <summary>
		/// Unlock all roads
		/// </summary>
		public static string UnlockAllRoads => LocaleManager.GetString("UnlockAllRoads", Culture);

		/// <summary>
		/// Unlock basic roads
		/// </summary>
		public static string UnlockBasicRoads => LocaleManager.GetString("UnlockBasicRoads", Culture);

		/// <summary>
		/// Unlock some basic roads, including some multi-lane roads
		/// </summary>
		public static string UnlockBasicRoadsMinor => LocaleManager.GetString("UnlockBasicRoadsMinor", Culture);

		/// <summary>
		/// Unlock info views panel
		/// </summary>
		public static string UnlockInfoViews => LocaleManager.GetString("UnlockInfoViews", Culture);

		/// <summary>
		/// Unlock landscaping
		/// </summary>
		public static string UnlockLandscaping => LocaleManager.GetString("UnlockLandscaping", Culture);

		/// <summary>
		/// Unlock metro track
		/// </summary>
		public static string UnlockMetroTrack => LocaleManager.GetString("UnlockMetroTrack", Culture);

		/// <summary>
		/// Unlock Options
		/// </summary>
		public static string UnlockOptions => LocaleManager.GetString("UnlockOptions", Culture);

		/// <summary>
		/// Unlock policies panel
		/// </summary>
		public static string UnlockPolicies => LocaleManager.GetString("UnlockPolicies", Culture);

		/// <summary>
		/// Unlock public transport
		/// </summary>
		public static string UnlockPublicTransport => LocaleManager.GetString("UnlockPublicTransport", Culture);

		/// <summary>
		/// Unlock train track
		/// </summary>
		public static string UnlockTrainTrack => LocaleManager.GetString("UnlockTrainTrack", Culture);

		/// <summary>
		/// Unlock unique Building
		/// </summary>
		public static string UnlockUniqueBuilding => LocaleManager.GetString("UnlockUniqueBuilding", Culture);

		/// <summary>
		/// Unlock all unique buildings at levels 1 through 6. Due to game restrictions, if this option is force
		/// </summary>
		public static string UnlockUniqueBuildingMinor => LocaleManager.GetString("UnlockUniqueBuildingMinor", Culture);

		/// <summary>
		/// {0} days ago
		/// </summary>
		public static string Updated_DaysAgo => LocaleManager.GetString("Updated_DaysAgo", Culture);

		/// <summary>
		/// Last year
		/// </summary>
		public static string Updated_LastYear => LocaleManager.GetString("Updated_LastYear", Culture);

		/// <summary>
		/// {0} month ago
		/// </summary>
		public static string Updated_MonthAgo => LocaleManager.GetString("Updated_MonthAgo", Culture);

		/// <summary>
		/// {0} months ago
		/// </summary>
		public static string Updated_MonthsAgo => LocaleManager.GetString("Updated_MonthsAgo", Culture);

		/// <summary>
		/// Today at {0:HH:mm}
		/// </summary>
		public static string Updated_Today => LocaleManager.GetString("Updated_Today", Culture);

		/// <summary>
		/// {0} years ago
		/// </summary>
		public static string Updated_YearsAgo => LocaleManager.GetString("Updated_YearsAgo", Culture);

		/// <summary>
		/// Yesterday
		/// </summary>
		public static string Updated_Yesterday => LocaleManager.GetString("Updated_Yesterday", Culture);

		/// <summary>
		/// Yesterday at {0:HH:mm}
		/// </summary>
		public static string Updated_YesterdayAt => LocaleManager.GetString("Updated_YesterdayAt", Culture);

		/// <summary>
		/// Added unlimited placement of festival area buildings.
		/// </summary>
		public static string UpdateLog_V1_0_0ADD0 => LocaleManager.GetString("UpdateLog_V1_0_0ADD0", Culture);

		/// <summary>
		/// Fixed control panel settings sometimes not automatically save.
		/// </summary>
		public static string UpdateLog_V1_0_0FIX0 => LocaleManager.GetString("UpdateLog_V1_0_0FIX0", Culture);

		/// <summary>
		/// Updated to support game version 1.17.0
		/// </summary>
		public static string UpdateLog_V1_0_0UPT => LocaleManager.GetString("UpdateLog_V1_0_0UPT", Culture);

		/// <summary>
		/// Fixed an issue where normal buildings could only be placed once.
		/// </summary>
		public static string UpdateLog_V1_0_1FIX0 => LocaleManager.GetString("UpdateLog_V1_0_1FIX0", Culture);

		/// <summary>
		/// Fixed an issue where the mod would not work at all when loading the config file incorrectly.
		/// </summary>
		public static string UpdateLog_V1_0_1FIX1 => LocaleManager.GetString("UpdateLog_V1_0_1FIX1", Culture);

		/// <summary>
		/// Added unlock landscaping feature.
		/// </summary>
		public static string UpdateLog_V1_1ADD0 => LocaleManager.GetString("UpdateLog_V1_1ADD0", Culture);

		/// <summary>
		/// Added put out burning buildings feature.
		/// </summary>
		public static string UpdateLog_V1_1ADD1 => LocaleManager.GetString("UpdateLog_V1_1ADD1", Culture);

		/// <summary>
		/// Added unlimited place treasure hunt buildings feature.
		/// </summary>
		public static string UpdateLog_V1_1ADD2 => LocaleManager.GetString("UpdateLog_V1_1ADD2", Culture);

		/// <summary>
		/// Integrated [You Can Build It] mod functions.
		/// </summary>
		public static string UpdateLog_V1_1ADD3 => LocaleManager.GetString("UpdateLog_V1_1ADD3", Culture);

		/// <summary>
		/// Fixed an issue where unlock public transport was not fully unlocked.
		/// </summary>
		public static string UpdateLog_V1_1FIX0 => LocaleManager.GetString("UpdateLog_V1_1FIX0", Culture);

		/// <summary>
		/// Fixed an issue where unlimited oil, unlimited ore is not working properly.
		/// </summary>
		public static string UpdateLog_V1_1FIX1 => LocaleManager.GetString("UpdateLog_V1_1FIX1", Culture);

		/// <summary>
		/// Optimize unlock info view performance issues.
		/// </summary>
		public static string UpdateLog_V1_1OPT0 => LocaleManager.GetString("UpdateLog_V1_1OPT0", Culture);

		/// <summary>
		/// Thai localization support
		/// </summary>
		public static string UpdateLog_V1_1TRA => LocaleManager.GetString("UpdateLog_V1_1TRA", Culture);

		/// <summary>
		/// UUI button and separate tool button are now optional.
		/// </summary>
		public static string UpdateLog_V1_1UPT2 => LocaleManager.GetString("UpdateLog_V1_1UPT2", Culture);

		/// <summary>
		/// Fixed an issue where the remove not enough money limitation function could not take effect.
		/// </summary>
		public static string UpdateLog_V1_1_1FIX0 => LocaleManager.GetString("UpdateLog_V1_1_1FIX0", Culture);

		/// <summary>
		/// Fixed some issues with the control panel.
		/// </summary>
		public static string UpdateLog_V1_1_1FIX1 => LocaleManager.GetString("UpdateLog_V1_1_1FIX1", Culture);

		/// <summary>
		/// Fixed an issue where achievements didn't work properly.
		/// </summary>
		public static string UpdateLog_V1_1_2FIX => LocaleManager.GetString("UpdateLog_V1_1_2FIX", Culture);

		/// <summary>
		/// Updated localization.
		/// </summary>
		public static string UpdateLog_V1_1_2UPT => LocaleManager.GetString("UpdateLog_V1_1_2UPT", Culture);

		/// <summary>
		/// Added custom unlock - Basic Road.
		/// </summary>
		public static string UpdateLog_V1_1_3ADD => LocaleManager.GetString("UpdateLog_V1_1_3ADD", Culture);

		/// <summary>
		/// Optimize some functions.
		/// </summary>
		public static string UpdateLog_V1_1_3OPT => LocaleManager.GetString("UpdateLog_V1_1_3OPT", Culture);

		/// <summary>
		/// Updated NuGet CitiesHarmony.API to 2.2
		/// </summary>
		public static string UpdateLog_V1_1_3UPT => LocaleManager.GetString("UpdateLog_V1_1_3UPT", Culture);

		/// <summary>
		/// Updated to support game version 1.17.1
		/// </summary>
		public static string UpdateLog_V1_1_UPT0 => LocaleManager.GetString("UpdateLog_V1_1_UPT0", Culture);

		/// <summary>
		/// Updated compatibility check. When three built-in unlimited mods are detected to be enabled, they are
		/// </summary>
		public static string UpdateLog_V1_1_UPT1 => LocaleManager.GetString("UpdateLog_V1_1_UPT1", Culture);

		/// <summary>
		/// Vanilla
		/// </summary>
		public static string Vanilla => LocaleManager.GetString("Vanilla", Culture);

		/// <summary>
		/// Vanilla unlimited money mode
		/// </summary>
		public static string VanillaUnlimitedMoneyMode => LocaleManager.GetString("VanillaUnlimitedMoneyMode", Culture);

		/// <summary>
		/// Money in game only displays “∞”, this option allows dynamic toggle within game
		/// </summary>
		public static string VanillaUnlimitedMoneyModeMinor => LocaleManager.GetString("VanillaUnlimitedMoneyModeMinor", Culture);

		/// <summary>
		/// Water pollution
		/// </summary>
		public static string WaterPollution => LocaleManager.GetString("WaterPollution", Culture);

		/// <summary>
		/// Weather radar
		/// </summary>
		public static string WeatherRadar => LocaleManager.GetString("WeatherRadar", Culture);
	}
}