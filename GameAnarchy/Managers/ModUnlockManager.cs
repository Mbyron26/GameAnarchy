using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSLModsCommon.Extension;
using CSLModsCommon.Manager;
using GameAnarchy.Data;
using GameAnarchy.ModSettings;
using ICities;

namespace GameAnarchy.Managers;

public class ModUnlockManager : ManagerBase {
    private ModSetting _modSetting;

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
    }

    private ItemClass.SubService[] PublicTransportSubService { get; set; } = new ItemClass.SubService[] {
        ItemClass.SubService.PublicTransportBus,
        ItemClass.SubService.PublicTransportTrolleybus,
        ItemClass.SubService.PublicTransportTram,
        ItemClass.SubService.PublicTransportMetro,
        ItemClass.SubService.PublicTransportTrain,
        ItemClass.SubService.PublicTransportShip,
        ItemClass.SubService.PublicTransportPlane,
        ItemClass.SubService.PublicTransportMonorail,
        ItemClass.SubService.PublicTransportCableCar,
        ItemClass.SubService.PublicTransportTaxi,
        ItemClass.SubService.PublicTransportTours
    };

    private UnlockManager.Feature[] PublicTransportFeature { get; set; } = new UnlockManager.Feature[] {
        UnlockManager.Feature.Trolleybus,
        UnlockManager.Feature.AirportAreas,
        UnlockManager.Feature.Ferry
    };

    private UnlockManager.Feature[] UniqueBuildingsFeature { get; set; } = new UnlockManager.Feature[] {
        UnlockManager.Feature.MonumentLevel2,
        UnlockManager.Feature.MonumentLevel3,
        UnlockManager.Feature.MonumentLevel4,
        UnlockManager.Feature.MonumentLevel5,
        UnlockManager.Feature.MonumentLevel6
    };

    private InfoManager.InfoMode[] InfoViewsModes { get; set; } = new InfoManager.InfoMode[] {
        InfoManager.InfoMode.Electricity,
        InfoManager.InfoMode.Water,
        InfoManager.InfoMode.CrimeRate,
        InfoManager.InfoMode.Health,
        InfoManager.InfoMode.Happiness,
        InfoManager.InfoMode.Density,
        InfoManager.InfoMode.NoisePollution,
        InfoManager.InfoMode.Transport,
        InfoManager.InfoMode.Pollution,
        InfoManager.InfoMode.NaturalResources,
        InfoManager.InfoMode.LandValue,
        InfoManager.InfoMode.Districts,
        InfoManager.InfoMode.Connections,
        InfoManager.InfoMode.Traffic,
        InfoManager.InfoMode.Wind,
        InfoManager.InfoMode.Garbage,
        InfoManager.InfoMode.BuildingLevel,
        InfoManager.InfoMode.FireSafety,
        InfoManager.InfoMode.Education,
        InfoManager.InfoMode.Entertainment,
        InfoManager.InfoMode.TerrainHeight,
        InfoManager.InfoMode.Heating,
        InfoManager.InfoMode.Maintenance,
        InfoManager.InfoMode.Snow,
        InfoManager.InfoMode.EscapeRoutes,
        InfoManager.InfoMode.Radio,
        InfoManager.InfoMode.Destruction,
        InfoManager.InfoMode.DisasterDetection,
        InfoManager.InfoMode.DisasterHazard,
        InfoManager.InfoMode.TrafficRoutes,
        InfoManager.InfoMode.Underground,
        InfoManager.InfoMode.Tours,
        InfoManager.InfoMode.ParkMaintenance,
        InfoManager.InfoMode.Tourism,
        InfoManager.InfoMode.Post,
        InfoManager.InfoMode.Industry,
        InfoManager.InfoMode.Fishing,
        InfoManager.InfoMode.ServicePoint,
        InfoManager.InfoMode.Financial,
        InfoManager.InfoMode.Hotel
    };

    public void CustomUnlock(IMilestones milestones) {
        if (_modSetting.CurrentUnlockMode != UnlockMode.CustomUnlock)
            return;
        UnlockMilestone(milestones);
        UnlockBasicRoad(milestones);
        UnlockAllRoads();
        UnlockTrainTrack(milestones);
        UnlockMetroTrack(milestones);
        UnlockPublicTransport();
        UnlockUniqueBuildings();
        UnlockLandscaping();
        UnlockPolicies();
        UnlockInfoViews();
    }

    public void UnlockBasicRoad(IMilestones milestones) {
        if (!_modSetting.UnlockBasicRoads)
            return;
        milestones.UnlockMilestone("Basic Road Created");
    }

    public void UnlockTrainTrack(IMilestones milestones) {
        if (!_modSetting.UnlockTrainTrack)
            return;
        milestones.UnlockMilestone("Train Track Requirements");
    }

    public void UnlockMetroTrack(IMilestones milestones) {
        if (!_modSetting.UnlockMetroTrack)
            return;
        milestones.UnlockMilestone("Metro Track Requirements");
    }

    public void UnlockInfoViews() {
        if (!_modSetting.UnlockInfoViews) return;

        if (!UnlockManager.instance.Unlocked(UnlockManager.Feature.InfoViews) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null) UnlockManager.instance.m_properties.m_FeatureMilestones[(int)UnlockManager.Feature.InfoViews] = null;

        foreach (var item in InfoViewsModes)
            if (!UnlockManager.instance.Unlocked(item))
                UnlockManager.instance.m_properties.m_InfoModeMilestones[(int)item] = null;

        Logger.Info("Custom unlock: Info Views");
    }

    public void UnlockPolicies() {
        if (!_modSetting.UnlockPolicies) return;

        if (!UnlockManager.instance.Unlocked(UnlockManager.Feature.Policies) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null) UnlockManager.instance.m_properties.m_FeatureMilestones[(int)UnlockManager.Feature.Policies] = null;

        var policyButton = UnlockManager.instance.m_properties.m_PolicyTypeMilestones;
        for (var i = 1; i < 4; i++)
            if (!UnlockManager.instance.Unlocked(policyButton[i]))
                policyButton[i] = null;

        var servicePanel = UnlockManager.instance.m_properties.m_ServicePolicyMilestones;
        for (var i = 0; i < servicePanel.Length - 2; i++)
            if (!UnlockManager.instance.Unlocked(servicePanel[i]))
                servicePanel[i] = null;

        var taxationPanel = UnlockManager.instance.m_properties.m_TaxationPolicyMilestones;
        for (var i = 0; i < taxationPanel.Length; i++)
            if (!UnlockManager.instance.Unlocked(taxationPanel[i]))
                taxationPanel[i] = null;

        var cityPlanningPanel = UnlockManager.instance.m_properties.m_CityPlanningPolicyMilestones;
        for (var i = 0; i < cityPlanningPanel.Length; i++)
            if (!UnlockManager.instance.Unlocked(cityPlanningPanel[i]))
                cityPlanningPanel[i] = null;

        Logger.Info("Custom unlock: Policies");
    }

    public void UnlockUniqueBuildings() {
        if (!_modSetting.UnlockUniqueBuildings) return;

        if (!UnlockManager.instance.Unlocked(ItemClass.Service.Monument) && UnlockManager.instance.m_properties.m_ServiceMilestones is not null) UnlockManager.instance.m_properties.m_ServiceMilestones[(int)ItemClass.Service.Monument] = null;

        foreach (var feature in UniqueBuildingsFeature)
            if (!UnlockManager.instance.Unlocked(feature) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null)
                UnlockManager.instance.m_properties.m_FeatureMilestones[(int)feature] = null;

        Logger.Info("Custom unlock: Unique Buildings");
    }

    public void UnlockAllRoads() {
        if (!_modSetting.UnlockAllRoads) return;

        for (uint i = 0; i < PrefabCollection<NetInfo>.LoadedCount(); i++) {
            var netInfo = PrefabCollection<NetInfo>.GetLoaded(i);
            if (netInfo is not null && netInfo.m_class is not null && netInfo.m_class.m_service == ItemClass.Service.Road) netInfo.m_UnlockMilestone = null;
        }

        for (uint j = 0; j < PrefabCollection<BuildingInfo>.LoadedCount(); j++) {
            var buildingInfo = PrefabCollection<BuildingInfo>.GetLoaded(j);
            if (buildingInfo is not null && buildingInfo.m_class is not null && buildingInfo.m_class.m_service == ItemClass.Service.Road) {
                buildingInfo.m_UnlockMilestone = null;
                var intersectionAI = buildingInfo.m_buildingAI as IntersectionAI;
                intersectionAI?.SetField<MilestoneInfo>("m_cachedUnlockMilestone", null, BindingFlags.NonPublic | BindingFlags.Instance);
                // TypeTools.SetFieldValue<MilestoneInfo>(intersectionAI, "m_cachedUnlockMilestone", BindingFlags.NonPublic | BindingFlags.Instance, null);
            }
        }

        Logger.Info("Custom unlock: All Roads");
    }

    public void UnlockMilestone(IMilestones milestones) {
        if (_modSetting.CurrentMilestoneLevel != MilestoneLevel.Vanilla) {
            milestones.UnlockMilestone($"Milestone{(int)_modSetting.CurrentMilestoneLevel}");
            Logger.Info($"Custom unlock milestone: {_modSetting.CurrentMilestoneLevel}");
        }
    }

    public void UnlockLandscaping() {
        if (!_modSetting.UnlockLandscaping) return;

        if (!UnlockManager.instance.Unlocked(UnlockManager.Feature.Landscaping) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null) {
            UnlockManager.instance.m_properties.m_FeatureMilestones[(int)UnlockManager.Feature.Landscaping] = null;
            Logger.Info("Custom unlock: Landscaping");
        }
    }

    public void UnlockPublicTransport() {
        if (!_modSetting.UnlockPublicTransport) return;

        UnlockPublicTransportService();
        UnlockPublicTransportFeature();
        UnlockPublicTransportSubService();
        UnlockPublicTransportBuildings();
        UnlockPublicTransportNet();
        Logger.Info("Custom unlock: Public Transport");
    }

    private void UnlockPublicTransportService() {
        if (!UnlockManager.instance.Unlocked(ItemClass.Service.PublicTransport) && UnlockManager.instance.m_properties.m_ServiceMilestones is not null) UnlockManager.instance.m_properties.m_ServiceMilestones[(int)ItemClass.Service.PublicTransport] = null;
    }

    private void UnlockPublicTransportFeature() {
        if (UnlockManager.instance.m_properties.m_FeatureMilestones is null) return;

        foreach (var item in PublicTransportFeature)
            if (!UnlockManager.instance.Unlocked(item))
                UnlockManager.instance.m_properties.m_FeatureMilestones[(int)item] = null;
    }

    private void UnlockPublicTransportSubService() {
        foreach (var item in PublicTransportSubService)
            if (!UnlockManager.instance.Unlocked(item) && UnlockManager.instance.m_properties.m_SubServiceMilestones is not null)
                UnlockManager.instance.m_properties.m_SubServiceMilestones[(int)item] = null;
    }

    private void UnlockPublicTransportBuildings() {
        for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
            var buildingInfo = PrefabCollection<BuildingInfo>.GetLoaded(i);
            if (buildingInfo is not null && buildingInfo.m_class.m_service == ItemClass.Service.PublicTransport) buildingInfo.m_UnlockMilestone = null;
        }

        for (uint j = 0; j < PrefabCollection<TransportInfo>.LoadedCount(); j++) {
            var transportInfo = PrefabCollection<TransportInfo>.GetLoaded(j);
            if (transportInfo is not null && transportInfo.m_class.m_service == ItemClass.Service.PublicTransport) transportInfo.m_UnlockMilestone = null;
        }
    }

    private void UnlockPublicTransportNet() {
        for (uint k = 0; k < PrefabCollection<NetInfo>.LoadedCount(); k++) {
            var netInfo = PrefabCollection<NetInfo>.GetLoaded(k);
            if (netInfo is null) continue;

            if (netInfo.m_class.m_service == ItemClass.Service.PublicTransport || IsTramBus(netInfo) || IsMonorail(netInfo) || IsTrolleybus(netInfo) || IsFerry(netInfo)) netInfo.m_UnlockMilestone = null;
        }
    }

    private bool IsFerry(NetInfo netInfo) => netInfo.m_vehicleTypes == VehicleInfo.VehicleType.Ferry;

    private bool IsTrolleybus(NetInfo netInfo) => netInfo.m_publicTransportCategory == ((int)VehicleInfo.VehicleCategory.Bus + VehicleInfo.VehicleCategory.Trolleybus) || netInfo.m_publicTransportCategory == VehicleInfo.VehicleCategory.Trolleybus;

    private bool IsTramBus(NetInfo netInfo) => netInfo.m_publicTransportCategory == VehicleInfo.VehicleCategory.Tram || netInfo.m_publicTransportCategory == VehicleInfo.VehicleCategory.Bus || netInfo.m_publicTransportCategory == ((int)VehicleInfo.VehicleCategory.Tram + VehicleInfo.VehicleCategory.Bus);

    private bool IsMonorail(NetInfo netInfo) => (netInfo.m_connectGroup == NetInfo.ConnectGroup.SingleMonorail || netInfo.m_connectGroup == NetInfo.ConnectGroup.DoubleMonorail || netInfo.m_connectGroup == NetInfo.ConnectGroup.MonorailStation);

    public void UnlockAll(IManagers managers, IMilestones milestonesManager) {
        if (_modSetting.CurrentUnlockMode != UnlockMode.UnlockAll)
            return;
        milestonesManager.UnlockMilestone("Basic Road Created");
        milestonesManager.UnlockMilestone("Train Track Requirements");
        milestonesManager.UnlockMilestone("Metro Track Requirements");
        if (managers.application.SupportsExpansion(Expansion.Hotels)) {
            milestonesManager.UnlockMilestone("2-Star Average Popularity");
            milestonesManager.UnlockMilestone("2-Star Hotel");
            milestonesManager.UnlockMilestone("2-Star Hotel Pofit");
            milestonesManager.UnlockMilestone("3-Star Average Popularity");
            milestonesManager.UnlockMilestone("3-Star Hotel");
            //milestonesManager.UnlockMilestone("3-Star Hotel Pofit");
            milestonesManager.UnlockMilestone("4-Star Average Popularity");
            milestonesManager.UnlockMilestone("4-Star Hotel");
            //milestonesManager.UnlockMilestone("4-Star Hotel Pofit");
            milestonesManager.UnlockMilestone("5-Star Average Popularity");
            milestonesManager.UnlockMilestone("5-Star Hotel");
            //milestonesManager.UnlockMilestone("5-Star Hotel Pofit");
        }

        if (managers.application.SupportsExpansion(Expansion.FinanceExpansion)) {
            milestonesManager.UnlockMilestone("Stock Exchange Level 1 Created");
            milestonesManager.UnlockMilestone("Stock Exchange Level 2 Created");
            milestonesManager.UnlockMilestone("Stock Exchange Level 3 Created");
            milestonesManager.UnlockMilestone("Stock Exchange Level 4 Created");
            milestonesManager.UnlockMilestone("Stock Exchange Level 5 Created");
            milestonesManager.UnlockMilestone("SE Localized Level 2");
            milestonesManager.UnlockMilestone("SE Localized Level 3");
            milestonesManager.UnlockMilestone("SE Localized Level 4");
            milestonesManager.UnlockMilestone("SE Localized Level 5");
        }

        if (managers.application.SupportsExpansion(Expansion.PlazasAndPromenades)) {
            milestonesManager.UnlockMilestone("Pedestrian Zone Created");
            milestonesManager.UnlockMilestone("Service Point Created");
            milestonesManager.UnlockMilestone("Flower Plaza Reqs");
            milestonesManager.UnlockMilestone("Large Fountain Plaza Reqs");
            milestonesManager.UnlockMilestone("Large PedArea Plaza Reqs");
            milestonesManager.UnlockMilestone("First SP Requirements");
            milestonesManager.UnlockMilestone("Large Cargo SP Reqs");
            milestonesManager.UnlockMilestone("Large Garbage SP Reqs");
            milestonesManager.UnlockMilestone("Large Service Point Reqs");
            milestonesManager.UnlockMilestone("Small Cargo SP Reqs");
            milestonesManager.UnlockMilestone("Small Garbage SP Reqs");
            milestonesManager.UnlockMilestone("Landmark Market Hall Reqs");
            milestonesManager.UnlockMilestone("Landmark MOPMA Reqs");
            milestonesManager.UnlockMilestone("Landmark Shopping Mall Reqs");
            milestonesManager.UnlockMilestone("Landmark Commercial");
            milestonesManager.UnlockMilestone("Landmark Office");
            milestonesManager.UnlockMilestone("Landmark Residential");
        }

        if (managers.application.SupportsExpansion(Expansion.Airport)) {
            milestonesManager.UnlockMilestone("Airport Requirements 1");
            milestonesManager.UnlockMilestone("Airport Requirements 2");
            milestonesManager.UnlockMilestone("Airport Requirements 3");
            milestonesManager.UnlockMilestone("Airport Terminal Requirements");
        }

        if (managers.application.SupportsExpansion(Expansion.Urban)) {
            milestonesManager.UnlockMilestone("Fishing Boat Harbor 02 Requirements");
            milestonesManager.UnlockMilestone("Fishing Boat Harbor 03 Requirements");
            milestonesManager.UnlockMilestone("Fishing Boat Harbor 04 Requirements");
            milestonesManager.UnlockMilestone("Fishing Boat Harbor 05 Requirements");
            milestonesManager.UnlockMilestone("Fish Farm 02 Requirements");
            milestonesManager.UnlockMilestone("Fish Farm 03 Requirements");
            milestonesManager.UnlockMilestone("Seafood Factory Requirements");
        }

        if (managers.application.SupportsExpansion(Expansion.Campuses)) {
            milestonesManager.UnlockMilestone("Campus Main Building Requirements");
            milestonesManager.UnlockMilestone("TradeSchool Requirements 1");
            milestonesManager.UnlockMilestone("TradeSchool Requirements 2");
            milestonesManager.UnlockMilestone("TradeSchool Requirements 3");
            milestonesManager.UnlockMilestone("TradeSchool Requirements 4");
            milestonesManager.UnlockMilestone("TradeSchool Requirements 5");
            milestonesManager.UnlockMilestone("LiberalArts Requirements 1");
            milestonesManager.UnlockMilestone("LiberalArts Requirements 2");
            milestonesManager.UnlockMilestone("LiberalArts Requirements 3");
            milestonesManager.UnlockMilestone("LiberalArts Requirements 4");
            milestonesManager.UnlockMilestone("LiberalArts Requirements 5");
            milestonesManager.UnlockMilestone("University Requirements 1");
            milestonesManager.UnlockMilestone("University Requirements 2");
            milestonesManager.UnlockMilestone("University Requirements 3");
            milestonesManager.UnlockMilestone("University Requirements 4");
            milestonesManager.UnlockMilestone("University Requirements 5");
        }

        if (managers.application.SupportsExpansion(Expansion.Industry)) {
            milestonesManager.UnlockMilestone("Main Building Requirements");
            milestonesManager.UnlockMilestone("Farming Requirements 1");
            milestonesManager.UnlockMilestone("Farming Requirements 2");
            milestonesManager.UnlockMilestone("Farming Requirements 3");
            milestonesManager.UnlockMilestone("Farming Requirements 4");
            milestonesManager.UnlockMilestone("Farming Requirements 5");
            milestonesManager.UnlockMilestone("Forestry Requirements 1");
            milestonesManager.UnlockMilestone("Forestry Requirements 2");
            milestonesManager.UnlockMilestone("Forestry Requirements 3");
            milestonesManager.UnlockMilestone("Forestry Requirements 4");
            milestonesManager.UnlockMilestone("Forestry Requirements 5");
            milestonesManager.UnlockMilestone("Oil Requirements 1");
            milestonesManager.UnlockMilestone("Oil Requirements 2");
            milestonesManager.UnlockMilestone("Oil Requirements 3");
            milestonesManager.UnlockMilestone("Oil Requirements 4");
            milestonesManager.UnlockMilestone("Oil Requirements 5");
            milestonesManager.UnlockMilestone("Ore Requirements 1");
            milestonesManager.UnlockMilestone("Ore Requirements 2");
            milestonesManager.UnlockMilestone("Ore Requirements 3");
            milestonesManager.UnlockMilestone("Ore Requirements 4");
            milestonesManager.UnlockMilestone("Ore Requirements 5");
        }

        if (managers.application.SupportsExpansion(Expansion.Parks)) {
            milestonesManager.UnlockMilestone("Park Gate Requirements");
            milestonesManager.UnlockMilestone("City Park Requirements 1");
            milestonesManager.UnlockMilestone("City Park Requirements 2");
            milestonesManager.UnlockMilestone("City Park Requirements 3");
            milestonesManager.UnlockMilestone("City Park Requirements 4");
            milestonesManager.UnlockMilestone("City Park Requirements 5");
            milestonesManager.UnlockMilestone("Amusement Park Requirements 1");
            milestonesManager.UnlockMilestone("Amusement Park Requirements 2");
            milestonesManager.UnlockMilestone("Amusement Park Requirements 3");
            milestonesManager.UnlockMilestone("Amusement Park Requirements 4");
            milestonesManager.UnlockMilestone("Amusement Park Requirements 5");
            milestonesManager.UnlockMilestone("Nature Reserve Requirements 1");
            milestonesManager.UnlockMilestone("Nature Reserve Requirements 2");
            milestonesManager.UnlockMilestone("Nature Reserve Requirements 3");
            milestonesManager.UnlockMilestone("Nature Reserve Requirements 4");
            milestonesManager.UnlockMilestone("Nature Reserve Requirements 5");
            milestonesManager.UnlockMilestone("Zoo Requirements 1");
            milestonesManager.UnlockMilestone("Zoo Requirements 2");
            milestonesManager.UnlockMilestone("Zoo Requirements 3");
            milestonesManager.UnlockMilestone("Zoo Requirements 4");
            milestonesManager.UnlockMilestone("Zoo Requirements 5");
            UnlockBuildings(new List<string> { "City Arch", "Clock Tower", "Old Market Street", "Sea Fortress", "Observation Tower", "Statue Of Colossalus", "Chirpwick Castle" });
        }

        if (managers.application.SupportsExpansion(Expansion.GreenCities))
            UnlockBuildings(new List<string> {
                "Bird And Bee Haven",
                "Floating Gardens",
                "Central Park",
                "Ziggurat Garden",
                "Climate Research Station",
                "Lungs of the City",
                "Ultimate Recycling Plant"
            });

        if (managers.application.SupportsExpansion(Expansion.InMotion))
            UnlockBuildings(new List<string> {
                "Boat Museum",
                "Traffic Park",
                "Steam Train"
            });

        if (managers.application.SupportsExpansion(Expansion.NaturalDisasters))
            UnlockBuildings(new List<string> {
                "Unicorn Park",
                "Sphinx Of Scenarios",
                "Pyramid Of Safety",
                "Doomsday Vault",
                "Disaster Memorial",
                "Helicopter Park",
                "Meteor Park"
            });

        if (managers.application.SupportsExpansion(Expansion.Snowfall))
            UnlockBuildings(new List<string> {
                "Ice Hockey Arena",
                "Sleigh Ride",
                "Spa Hotel",
                "Snowcastle Restaurant",
                "Ski Resort",
                "Santa Claus Workshop",
                "Christmas Tree",
                "Arena",
                "Driving Range",
                "Igloo Hotel"
            });

        if (managers.application.SupportsExpansion(Expansion.AfterDark))
            UnlockBuildings(new List<string> {
                "Fancy Fountain",
                "Casino",
                "Driving Range",
                "Luxury Hotel",
                "Zoo"
            });

        UnlockBuildings(new List<string> {
            //UB-I
            "Statue of Industry",
            "Statue of Wealth",
            "Lazaret Plaza",
            "Statue of Shopping",
            "Plaza of the Dead",
            //UB-II
            "Fountain of LifeDeath",
            "Friendly Neighborhood",
            "Transport Tower",
            "Trash Mall",
            "Posh Mall",
            //UB-III
            "Colossal Offices",
            "Official Park",
            "CourtHouse",
            "Grand Mall",
            "Cityhall",
            //UB-IV
            "Business Park",
            "Library",
            "Observatory",
            "Opera House",
            "Oppression Office",
            //UB-V
            "ScienceCenter",
            "Servicing Services",
            "SeaWorld",
            "Expocenter",
            "High Interest Tower",
            //UB-VI
            "Cathedral of Plentitude",
            "Stadium",
            "Modern Art Museum",
            "SeaAndSky Scraper",
            "Theater of Wonders"
        });

        UnlockBuildings(new List<string> {
            //Wonders
            "Hadron Collider",
            "Medical Center",
            "Space Elevator",
            "Eden Project",
            "Fusion Power Plant"
        });

        UnlockBuildings(new List<string> {
            //European
            "Arena",
            "Shopping Center",
            "Theatre",
            "London Eye",
            "Cinema",
            "City Hall",
            "Amsterdam Palace",
            "Cathedral",
            "Government Offices",
            "Hypermarket",
            "Department Store",
            "Gherkin"
        });

        UnlockBuildings(new List<string> {
            "Academic Library",
            "Aviation Club"
        }, "Prerequisites");

        UnlockBuildings(new List<string> {
            //concerts
            "Festival Fan Zone",
            "Broadcasting Studios",
            "Live Music Venue",
            "Festival Area"
        });

        UnlockBuildings(new List<string> {
            //Deluxe
            "Eiffel Tower",
            "Statue of Liberty",
            "Grand Central Terminal",
            "Brandenburg Gate",
            "Arc de Triomphe"
        });

        void UnlockBuildings(List<string> list, string postfix = "Requirements") {
            list.ForEach(s => {
                if (milestonesManager.EnumerateMilestones().Contains($"{s} {postfix}"))
                    milestonesManager.UnlockMilestone($"{s} {postfix}");
            });
        }
    }
}