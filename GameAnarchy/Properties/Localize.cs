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
		public static string CustomUnlockPanelTooltip => LocaleManager.GetString("CustomUnlockPanelTooltip", Culture);

		/// <summary>
		/// Economy
		/// </summary>
		public static string Economy => LocaleManager.GetString("Economy", Culture);

		/// <summary>
		/// Achievements system is always available
		/// </summary>
		public static string EnableAchievements => LocaleManager.GetString("EnableAchievements", Culture);

		/// <summary>
		/// Info view is always available
		/// </summary>
		public static string EnabledInfoView => LocaleManager.GetString("EnabledInfoView", Culture);

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
		/// Buildtin mod, Game Anarchy already includes the same functionality. Please disabled this local mod i
		/// </summary>
		public static string LocalModWarning => LocaleManager.GetString("LocalModWarning", Culture);

		/// <summary>
		/// Buildtin mod, Game Anarchy already includes the same functionality. Please disable this local mod an
		/// </summary>
		public static string LocalUnlimitedMoneyWarning => LocaleManager.GetString("LocalUnlimitedMoneyWarning", Culture);

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
		/// Player building
		/// </summary>
		public static string PlayerBuilding => LocaleManager.GetString("PlayerBuilding", Culture);

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
		/// Remove not enough money warning
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
		/// Unlock all
		/// </summary>
		public static string UnlockAll => LocaleManager.GetString("UnlockAll", Culture);

		/// <summary>
		/// It is recommended to set this before you start the game. If disabled this option still unlocks somet
		/// </summary>
		public static string UnlockAllMinor => LocaleManager.GetString("UnlockAllMinor", Culture);

		/// <summary>
		/// Unlock all roads
		/// </summary>
		public static string UnlockAllRoads => LocaleManager.GetString("UnlockAllRoads", Culture);

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
		/// Unlock train track
		/// </summary>
		public static string UnlockTrainTrack => LocaleManager.GetString("UnlockTrainTrack", Culture);

		/// <summary>
		/// Unlock all transports
		/// </summary>
		public static string UnlockTransport => LocaleManager.GetString("UnlockTransport", Culture);

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
		/// [ADD]New control panel, use default hotkey Ctrl+Shift+G or use UUI panel to invoke new control panel
		/// </summary>
		public static string UpdateLog_V0_9_7ADD1 => LocaleManager.GetString("UpdateLog_V0_9_7ADD1", Culture);

		/// <summary>
		/// [ADD]Place unique buildings without limit features now split and integrated into control panel.
		/// </summary>
		public static string UpdateLog_V0_9_7ADD2 => LocaleManager.GetString("UpdateLog_V0_9_7ADD2", Culture);

		/// <summary>
		/// [ADD]Added fire spread control function.
		/// </summary>
		public static string UpdateLog_V0_9_7ADD3 => LocaleManager.GetString("UpdateLog_V0_9_7ADD3", Culture);

		/// <summary>
		/// [ADD]Remove fire function now split into multiple options, can control fire of different building ty
		/// </summary>
		public static string UpdateLog_V0_9_7ADD4 => LocaleManager.GetString("UpdateLog_V0_9_7ADD4", Culture);

		/// <summary>
		/// [ADD]Added Turkish localization.
		/// </summary>
		public static string UpdateLog_V0_9_7ADD5 => LocaleManager.GetString("UpdateLog_V0_9_7ADD5", Culture);

		/// <summary>
		/// [FIX]Fixed abnormal UI component location.
		/// </summary>
		public static string UpdateLog_V0_9_7FIX => LocaleManager.GetString("UpdateLog_V0_9_7FIX", Culture);

		/// <summary>
		/// [UPT]Updated framework, performance improvement.
		/// </summary>
		public static string UpdateLog_V0_9_7UPT1 => LocaleManager.GetString("UpdateLog_V0_9_7UPT1", Culture);

		/// <summary>
		/// [UPT]Updated Option Panel UI components.
		/// </summary>
		public static string UpdateLog_V0_9_7UPT2 => LocaleManager.GetString("UpdateLog_V0_9_7UPT2", Culture);

		/// <summary>
		/// [ADD]Added advanced option for reset mod config.
		/// </summary>
		public static string UpdateLog_V0_9_8ADD => LocaleManager.GetString("UpdateLog_V0_9_8ADD", Culture);

		/// <summary>
		/// [FIX]Fixed an issue where control panel did not follow the language toggle.
		/// </summary>
		public static string UpdateLog_V0_9_8FIX1 => LocaleManager.GetString("UpdateLog_V0_9_8FIX1", Culture);

		/// <summary>
		/// [FIX]Fixed an issue where the money option in options panel could not be changed.
		/// </summary>
		public static string UpdateLog_V0_9_8FIX2 => LocaleManager.GetString("UpdateLog_V0_9_8FIX2", Culture);

		/// <summary>
		/// [OPT]Optimized control panel invoke speed.
		/// </summary>
		public static string UpdateLog_V0_9_8OPT1 => LocaleManager.GetString("UpdateLog_V0_9_8OPT1", Culture);

		/// <summary>
		/// [OPT]Optimized control panel toggle button UI style.
		/// </summary>
		public static string UpdateLog_V0_9_8OPT2 => LocaleManager.GetString("UpdateLog_V0_9_8OPT2", Culture);

		/// <summary>
		/// [UPT]Updated compatibility check.
		/// </summary>
		public static string UpdateLog_V0_9_8UPT => LocaleManager.GetString("UpdateLog_V0_9_8UPT", Culture);

		/// <summary>
		/// Added show mods updated date for the category bar of the options panel function.
		/// </summary>
		public static string UpdateLog_V0_9_9ADD0 => LocaleManager.GetString("UpdateLog_V0_9_9ADD0", Culture);

		/// <summary>
		/// Added building, segment refund function, including remove time limitation, multiplier factor adjustm
		/// </summary>
		public static string UpdateLog_V0_9_9ADD1 => LocaleManager.GetString("UpdateLog_V0_9_9ADD1", Culture);

		/// <summary>
		/// Option panel added a entry button option to invoke the control panel.
		/// </summary>
		public static string UpdateLog_V0_9_9ADD2 => LocaleManager.GetString("UpdateLog_V0_9_9ADD2", Culture);

		/// <summary>
		/// Added the ability to adjust the cost of relocate building, included in the control panel.
		/// </summary>
		public static string UpdateLog_V0_9_9ADD3 => LocaleManager.GetString("UpdateLog_V0_9_9ADD3", Culture);

		/// <summary>
		/// Added Remove not enough money warning function.
		/// </summary>
		public static string UpdateLog_V0_9_9ADD4 => LocaleManager.GetString("UpdateLog_V0_9_9ADD4", Culture);

		/// <summary>
		/// Fixed an issue where still catch fire after remove fires.
		/// </summary>
		public static string UpdateLog_V0_9_9FIX0 => LocaleManager.GetString("UpdateLog_V0_9_9FIX0", Culture);

		/// <summary>
		/// Optimized the options panel categories horizontal offset option, which now takes effect after adjust
		/// </summary>
		public static string UpdateLog_V0_9_9OPT0 => LocaleManager.GetString("UpdateLog_V0_9_9OPT0", Culture);

		/// <summary>
		/// Optimized the start money setting logic so that the start money is the value you set when you start 
		/// </summary>
		public static string UpdateLog_V0_9_9OPT1 => LocaleManager.GetString("UpdateLog_V0_9_9OPT1", Culture);

		/// <summary>
		/// Added Slovak localization.
		/// </summary>
		public static string UpdateLog_V0_9_9TRAN => LocaleManager.GetString("UpdateLog_V0_9_9TRAN", Culture);

		/// <summary>
		/// Updated UI style.
		/// </summary>
		public static string UpdateLog_V0_9_9UPT => LocaleManager.GetString("UpdateLog_V0_9_9UPT", Culture);

		/// <summary>
		/// Updated framework.
		/// </summary>
		public static string UpdateLog_V0_9_9UPT1 => LocaleManager.GetString("UpdateLog_V0_9_9UPT1", Culture);

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