using CSShared.Debug;
using CSShared.Tools;
using ICities;
using System.Reflection;

namespace GameAnarchy.Managers;

public partial class Manager {
    private ItemClass.SubService[] PublicTransportSubService { get; set; } = new ItemClass.SubService[]  {
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
    private UnlockManager.Feature[] PublicTransportFeature { get; set; } = new UnlockManager.Feature[]  {
        UnlockManager.Feature.Trolleybus,
        UnlockManager.Feature.AirportAreas,
        UnlockManager.Feature.Ferry,
    };
    private UnlockManager.Feature[] UniqueBuildingsFeature { get; set; } = new UnlockManager.Feature[]  {
        UnlockManager.Feature.MonumentLevel2,
        UnlockManager.Feature.MonumentLevel3,
        UnlockManager.Feature.MonumentLevel4,
        UnlockManager.Feature.MonumentLevel5,
        UnlockManager.Feature.MonumentLevel6,
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
        InfoManager.InfoMode.Hotel,
    };

    public void CustomUnlock(IMilestones milestones) {
        if (!Config.Instance.CustomUnlock)
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
        if (!Config.Instance.UnlockBasicRoads)
            return;
        milestones.UnlockMilestone("Basic Road Created");
    }

    public void UnlockTrainTrack(IMilestones milestones) {
        if (!Config.Instance.UnlockTrainTrack)
            return;
        milestones.UnlockMilestone("Train Track Requirements");
    }

    public void UnlockMetroTrack(IMilestones milestones) {
        if (!Config.Instance.UnlockMetroTrack)
            return;
        milestones.UnlockMilestone("Metro Track Requirements");
    }

    public void UnlockInfoViews() {
        if (!Config.Instance.UnlockInfoViews) {
            return;
        }
        if (!UnlockManager.instance.Unlocked(UnlockManager.Feature.InfoViews) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null) {
            UnlockManager.instance.m_properties.m_FeatureMilestones[(int)UnlockManager.Feature.InfoViews] = null;
        };
        foreach (var item in InfoViewsModes) {
            if (!UnlockManager.instance.Unlocked(item)) {
                UnlockManager.instance.m_properties.m_InfoModeMilestones[(int)item] = null;
            }
        }
        LogManager.GetLogger().Info("Custom unlock: Info Views");
    }

    public void UnlockPolicies() {
        if (!Config.Instance.UnlockPolicies) {
            return;
        }
        if (!UnlockManager.instance.Unlocked(UnlockManager.Feature.Policies) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null) {
            UnlockManager.instance.m_properties.m_FeatureMilestones[(int)UnlockManager.Feature.Policies] = null;
        };
        var policyButton = UnlockManager.instance.m_properties.m_PolicyTypeMilestones;
        for (int i = 1; i < 4; i++) {
            if (!UnlockManager.instance.Unlocked(policyButton[i])) {
                policyButton[i] = null;
            }
        }
        var servicePanel = UnlockManager.instance.m_properties.m_ServicePolicyMilestones;
        for (int i = 0; i < servicePanel.Length - 2; i++) {
            if (!UnlockManager.instance.Unlocked(servicePanel[i])) {
                servicePanel[i] = null;
            }
        }
        var taxationPanel = UnlockManager.instance.m_properties.m_TaxationPolicyMilestones;
        for (int i = 0; i < taxationPanel.Length; i++) {
            if (!UnlockManager.instance.Unlocked(taxationPanel[i])) {
                taxationPanel[i] = null;
            }
        }
        var cityPlanningPanel = UnlockManager.instance.m_properties.m_CityPlanningPolicyMilestones;
        for (int i = 0; i < cityPlanningPanel.Length; i++) {
            if (!UnlockManager.instance.Unlocked(cityPlanningPanel[i])) {
                cityPlanningPanel[i] = null;
            }
        }
        LogManager.GetLogger().Info("Custom unlock: Policies");
    }

    public void UnlockUniqueBuildings() {
        if (!Config.Instance.UnlockUniqueBuildings) {
            return;
        }
        if (!UnlockManager.instance.Unlocked(ItemClass.Service.Monument) && UnlockManager.instance.m_properties.m_ServiceMilestones is not null) {
            UnlockManager.instance.m_properties.m_ServiceMilestones[(int)ItemClass.Service.Monument] = null;
        };
        foreach (var feature in UniqueBuildingsFeature) {
            if (!UnlockManager.instance.Unlocked(feature) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null) {
                UnlockManager.instance.m_properties.m_FeatureMilestones[(int)feature] = null;
            };
        }
        LogManager.GetLogger().Info("Custom unlock: Unique Buildings");
    }

    public void UnlockAllRoads() {
        if (!Config.Instance.UnlockAllRoads) {
            return;
        }
        for (uint i = 0; i < PrefabCollection<NetInfo>.LoadedCount(); i++) {
            var netInfo = PrefabCollection<NetInfo>.GetLoaded(i);
            if (netInfo is not null && netInfo.m_class is not null && netInfo.m_class.m_service == ItemClass.Service.Road) {
                netInfo.m_UnlockMilestone = null;
            }
        }
        for (uint j = 0; j < PrefabCollection<BuildingInfo>.LoadedCount(); j++) {
            var buildingInfo = PrefabCollection<BuildingInfo>.GetLoaded(j);
            if (buildingInfo is not null && buildingInfo.m_class is not null && buildingInfo.m_class.m_service == ItemClass.Service.Road) {
                buildingInfo.m_UnlockMilestone = null;
                var intersectionAI = buildingInfo.m_buildingAI as IntersectionAI;
                if (intersectionAI is not null) {
                    TypeTools.SetFieldValue<MilestoneInfo>(intersectionAI, "m_cachedUnlockMilestone", BindingFlags.NonPublic | BindingFlags.Instance, null);
                }
            }
        }
        LogManager.GetLogger().Info("Custom unlock: All Roads");
    }

    public void UnlockMilestone(IMilestones milestones) {
        if (Config.Instance.MilestoneLevel != 0) {
            milestones.UnlockMilestone($"Milestone{Config.Instance.MilestoneLevel}");
            LogManager.GetLogger().Info($"Custom unlock milestone: {Config.Instance.MilestoneLevel}");
        }
    }

    public void UnlockLandscaping() {
        if (!Config.Instance.UnlockLandscaping) {
            return;
        }
        if (!UnlockManager.instance.Unlocked(UnlockManager.Feature.Landscaping) && UnlockManager.instance.m_properties.m_FeatureMilestones is not null) {
            UnlockManager.instance.m_properties.m_FeatureMilestones[(int)UnlockManager.Feature.Landscaping] = null;
            LogManager.GetLogger().Info("Custom unlock: Landscaping");
        };
    }

    public void UnlockPublicTransport() {
        if (!Config.Instance.UnlockPublicTransport) {
            return;
        }
        UnlockPublicTransportService();
        UnlockPublicTransportFeature();
        UnlockPublicTransportSubService();
        UnlockPublicTransportBuildings();
        UnlockPublicTransportNet();
        LogManager.GetLogger().Info("Custom unlock: Public Transport");
    }

    private void UnlockPublicTransportService() {
        if (!UnlockManager.instance.Unlocked(ItemClass.Service.PublicTransport) && UnlockManager.instance.m_properties.m_ServiceMilestones is not null) {
            UnlockManager.instance.m_properties.m_ServiceMilestones[(int)ItemClass.Service.PublicTransport] = null;
        };
    }

    private void UnlockPublicTransportFeature() {
        if (UnlockManager.instance.m_properties.m_FeatureMilestones is null) {
            return;
        }
        foreach (var item in PublicTransportFeature) {
            if (!UnlockManager.instance.Unlocked(item)) {
                UnlockManager.instance.m_properties.m_FeatureMilestones[(int)item] = null;
            }
        }
    }

    private void UnlockPublicTransportSubService() {
        foreach (var item in PublicTransportSubService) {
            if (!UnlockManager.instance.Unlocked(item) && UnlockManager.instance.m_properties.m_SubServiceMilestones is not null) {
                UnlockManager.instance.m_properties.m_SubServiceMilestones[(int)item] = null;
            };
        }
    }

    private void UnlockPublicTransportBuildings() {
        for (uint i = 0; i < PrefabCollection<BuildingInfo>.LoadedCount(); i++) {
            var buildingInfo = PrefabCollection<BuildingInfo>.GetLoaded(i);
            if (buildingInfo is not null && buildingInfo.m_class.m_service == ItemClass.Service.PublicTransport) {
                buildingInfo.m_UnlockMilestone = null;
            }
        }
        for (uint j = 0; j < PrefabCollection<TransportInfo>.LoadedCount(); j++) {
            var transportInfo = PrefabCollection<TransportInfo>.GetLoaded(j);
            if (transportInfo is not null && transportInfo.m_class.m_service == ItemClass.Service.PublicTransport) {
                transportInfo.m_UnlockMilestone = null;
            }
        }
    }

    private void UnlockPublicTransportNet() {
        for (uint k = 0; k < PrefabCollection<NetInfo>.LoadedCount(); k++) {
            var netInfo = PrefabCollection<NetInfo>.GetLoaded(k);
            if (netInfo is null) {
                continue;
            }
            if (netInfo.m_class.m_service == ItemClass.Service.PublicTransport || IsTramBus(netInfo) || IsMonorail(netInfo) || IsTrolleybus(netInfo) || IsFerry(netInfo)) {
                netInfo.m_UnlockMilestone = null;
            }
        }
    }

    private bool IsFerry(NetInfo netInfo) => netInfo.m_vehicleTypes == VehicleInfo.VehicleType.Ferry;
    private bool IsTrolleybus(NetInfo netInfo) => netInfo.m_publicTransportCategory == ((int)VehicleInfo.VehicleCategory.Bus + VehicleInfo.VehicleCategory.Trolleybus) || netInfo.m_publicTransportCategory == VehicleInfo.VehicleCategory.Trolleybus;
    private bool IsTramBus(NetInfo netInfo) => netInfo.m_publicTransportCategory == VehicleInfo.VehicleCategory.Tram || netInfo.m_publicTransportCategory == VehicleInfo.VehicleCategory.Bus || netInfo.m_publicTransportCategory == ((int)VehicleInfo.VehicleCategory.Tram + VehicleInfo.VehicleCategory.Bus);
    private bool IsMonorail(NetInfo netInfo) => (netInfo.m_connectGroup == NetInfo.ConnectGroup.SingleMonorail || netInfo.m_connectGroup == NetInfo.ConnectGroup.DoubleMonorail || netInfo.m_connectGroup == NetInfo.ConnectGroup.MonorailStation);

}
