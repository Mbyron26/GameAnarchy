namespace GameAnarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ICities;

public class MilestonesExtension : MilestonesExtensionBase {
    public override void OnRefreshMilestones() {

        #region UnlockAllRoads
        if (Config.Instance.UnlockAllRoads && Config.Instance.CustomUnlock) {
            milestonesManager.UnlockMilestone("Basic Road Created");
            var netCount = PrefabCollection<NetInfo>.LoadedCount();
            var buildingCount = PrefabCollection<BuildingInfo>.LoadedCount();
            if (netCount == 0 || buildingCount == 0) {
                ExternalLogger.Error("Unlock all roads fail, base net road count and building type road count is empty.");
                return;
            }

            try {
                for (uint i = 0; i < netCount; i++) {
                    var netInfo = PrefabCollection<NetInfo>.GetLoaded(i);
                    if (netInfo is not null && netInfo.m_class is not null && netInfo.m_class.m_service == ItemClass.Service.Road) {
                        netInfo.m_UnlockMilestone = null;
                    }
                }
                ExternalLogger.Error("Unlock base net road succeed.");

                for (uint j = 0; j < buildingCount; j++) {
                    var buildingInfo = PrefabCollection<BuildingInfo>.GetLoaded(j);
                    if (buildingInfo is not null && buildingInfo.m_class is not null && buildingInfo.m_class.m_service == ItemClass.Service.Road) {
                        buildingInfo.m_UnlockMilestone = null;
                        var intersectionAI = buildingInfo.m_buildingAI as IntersectionAI;
                        if (intersectionAI is not null) {
                            UnityHelper.SetFieldValue<MilestoneInfo>(intersectionAI, "m_cachedUnlockMilestone", BindingFlags.NonPublic | BindingFlags.Instance, null);
                        }
                    }
                }

                ExternalLogger.Log("Unlock all type roads succeed.");
            } catch (Exception e) {
                ExternalLogger.Exception($"Unlock all road failed.", e);
            }
        }
        #endregion

        #region UnlockCommon
        if (Config.Instance.EnabledUnlockAll || (Config.Instance.UnlockTrainTrack && Config.Instance.CustomUnlock)) {
            milestonesManager.UnlockMilestone("Train Track Requirements");
        }
        if (Config.Instance.EnabledUnlockAll || (Config.Instance.UnlockMetroTrack && Config.Instance.CustomUnlock)) {
            milestonesManager.UnlockMilestone("Metro Track Requirements");
        }
        #endregion

        #region UnlockAll
        if (Config.Instance.EnabledUnlockAll) {
            milestonesManager.UnlockMilestone("Basic Road Created");

            if (managers.application.SupportsExpansion(Expansion.Hotels)) {
                milestonesManager.UnlockMilestone("2-Star Average Popularity");
                milestonesManager.UnlockMilestone("2-Star Hotel");
                milestonesManager.UnlockMilestone("2-Star Hotel Pofit");
                milestonesManager.UnlockMilestone("3-Star Average Popularity");
                milestonesManager.UnlockMilestone("3-Star Hotel");
                milestonesManager.UnlockMilestone("3-Star Hotel Pofit");
                milestonesManager.UnlockMilestone("4-Star Average Popularity");
                milestonesManager.UnlockMilestone("4-Star Hotel");
                milestonesManager.UnlockMilestone("4-Star Hotel Pofit");
                milestonesManager.UnlockMilestone("5-Star Average Popularity");
                milestonesManager.UnlockMilestone("5-Star Hotel");
                milestonesManager.UnlockMilestone("5-Star Hotel Pofit");
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
                UnlockBuildings(new List<string> {
                    "City Arch",
                "Clock Tower",
                "Old Market Street",
                "Sea Fortress",
                "Observation Tower",
                "Statue Of Colossalus",
                "Chirpwick Castle",
                });

            }

            if (managers.application.SupportsExpansion(Expansion.GreenCities)) {
                UnlockBuildings(new List<string>
                {
                "Bird And Bee Haven",
                "Floating Gardens",
                "Central Park",
                "Ziggurat Garden",
                "Climate Research Station",
                "Lungs of the City",
                "Ultimate Recycling Plant"
            });
            }

            if (managers.application.SupportsExpansion(Expansion.InMotion)) {
                UnlockBuildings(new List<string>
                {
                "Boat Museum",
                "Traffic Park",
                "Steam Train"
            });
            }

            if (managers.application.SupportsExpansion(Expansion.NaturalDisasters)) {
                UnlockBuildings(new List<string>
                {
                "Unicorn Park",
                "Sphinx Of Scenarios",
                "Pyramid Of Safety",
                "Doomsday Vault",
                "Disaster Memorial",
                "Helicopter Park",
                "Meteor Park"
            });
            }

            if (managers.application.SupportsExpansion(Expansion.Snowfall)) {
                UnlockBuildings(new List<string>
                {
                "Ice Hockey Arena",
                "Sleigh Ride",
                "Spa Hotel",
                "Snowcastle Restaurant",
                "Ski Resort",
                "Santa Claus Workshop",
                "Christmas Tree",
                "Arena",
                "Driving Range",
                "Igloo Hotel",
            });
            }

            if (managers.application.SupportsExpansion(Expansion.AfterDark)) {
                UnlockBuildings(new List<string>
                {
                "Fancy Fountain",
                "Casino",
                "Driving Range",
                "Luxury Hotel",
                "Zoo",
            });
            }

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
            "Theater of Wonders",
            });



            UnlockBuildings(new List<string>
            {
            //Wonders
            "Hadron Collider",
            "Medical Center",
            "Space Elevator",
            "Eden Project",
            "Fusion Power Plant",
        });

            UnlockBuildings(new List<string>
            {
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
            "Gherkin",
        });



            UnlockBuildings(new List<string> {
            "Academic Library",
            "Aviation Club",}, "Prerequisites");



            UnlockBuildings(new List<string>
            {
            //concerts
            "Festival Fan Zone",
            "Broadcasting Studios",
            "Live Music Venue",
            "Festival Area",
        });

            UnlockBuildings(new List<string>{
            //Deluxe
            "Eiffel Tower",
            "Statue of Liberty",
            "Grand Central Terminal",
            "Brandenburg Gate",
            "Arc de Triomphe", });
        }
        #endregion

        #region CustomUnlock
        if (Config.Instance.CustomUnlock && Config.Instance.MilestoneLevel != 0) {
            milestonesManager.UnlockMilestone("Milestone" + Config.Instance.MilestoneLevel.ToString());
        }
        #endregion

        #region Unlock Policies
        if (Config.Instance.CustomUnlock && Config.Instance.UnlockPolicies) {
            if (UnlockManager.exists) {
                try {
                    UnlockManager.instance.m_properties.m_FeatureMilestones[2] = null;
                    var icon = UnlockManager.instance.m_properties.m_PolicyTypeMilestones;
                    for (int i = 1; i < 4; i++) {
                        icon[i] = null;
                    }
                    var servicePanel = UnlockManager.instance.m_properties.m_ServicePolicyMilestones;
                    for (int i = 0; i < servicePanel.Length - 2; i++) {
                        servicePanel[i] = null;
                    }
                    var taxationPanel = UnlockManager.instance.m_properties.m_TaxationPolicyMilestones;
                    for (int i = 0; i < taxationPanel.Length; i++) {
                        taxationPanel[i] = null;
                    }
                    var cityPlanningPanel = UnlockManager.instance.m_properties.m_CityPlanningPolicyMilestones;
                    for (int i = 0; i < cityPlanningPanel.Length; i++) {
                        cityPlanningPanel[i] = null;
                    }
                } catch (Exception e) {
                    ExternalLogger.Exception($"Couldn't unlock policies.", e);
                }
            } else {
                ExternalLogger.Error("UnlockManager doesn't exist.");
            }
        }
        #endregion

        #region Unlock Transport
        if (Config.Instance.CustomUnlock && Config.Instance.UnlockTransport) {
            if (UnlockManager.exists) {
                try {
                    var subService = UnlockManager.instance.m_properties.m_SubServiceMilestones;
                    if (subService is not null) {
                        for (int i = 0; i < subService.Length; i++) {
                            if (subService[i] is not null) {
                                var cm = subService[i] as CombinedMilestone;
                                if (cm != null && (cm.name == "Milestone4" || cm.name == "Milestone5" || cm.name == "Milestone6" || cm.name == "Milestone7")) {
                                    cm.m_openUnlockPanel = false;
                                    cm.m_requirePassedLimit = 0;
                                }
                            }
                        }
                    }
                    ExternalLogger.Log("Unlock transport succeed.");
                } catch (Exception e) {
                    ExternalLogger.Exception($"Unlock transport failed.", e);
                }
            } else {
                ExternalLogger.Error($"UnlockManager doesn't exists, unlock transport failed.");
            }
        }
        #endregion

        #region Unlock Unique Building
        if (Config.Instance.UnlockUniqueBuilding && Config.Instance.CustomUnlock) {
            if (UnlockManager.instance.m_properties.m_ServiceMilestones[17] is not null) {
                UnlockManager.instance.m_properties.m_ServiceMilestones[17] = null;
                ExternalLogger.Log("Unlock unique building level 1 succeed.");
            }
            if (UnlockManager.instance.m_properties.m_FeatureMilestones[14] is not null) {
                UnlockManager.instance.m_properties.m_FeatureMilestones[14] = null;
                ExternalLogger.Log("Unlock unique building level 2 succeed.");
            }
            if (UnlockManager.instance.m_properties.m_FeatureMilestones[15] is not null) {
                UnlockManager.instance.m_properties.m_FeatureMilestones[15] = null;
                ExternalLogger.Log("Unlock unique building level 3 succeed.");
            }
            if (UnlockManager.instance.m_properties.m_FeatureMilestones[16] is not null) {
                UnlockManager.instance.m_properties.m_FeatureMilestones[16] = null;
                ExternalLogger.Log("Unlock unique building level 4 succeed.");
            }
            if (UnlockManager.instance.m_properties.m_FeatureMilestones[17] is not null) {
                UnlockManager.instance.m_properties.m_FeatureMilestones[17] = null;
                ExternalLogger.Log("Unlock unique building level 5 succeed.");
            }
            if (UnlockManager.instance.m_properties.m_FeatureMilestones[18] is not null) {
                UnlockManager.instance.m_properties.m_FeatureMilestones[18] = null;
                ExternalLogger.Log("Unlock unique building level 6 succeed.");
            }
        }
        #endregion
    }

    private void UnlockBuildings(List<string> list, string postfix = "Requirements") {
        foreach (var item in list) {
            var unlockItem = item + ' ' + postfix;
            if (milestonesManager.EnumerateMilestones().Contains(unlockItem)) {
                milestonesManager.UnlockMilestone(unlockItem);
            }
        }
    }

    public override int OnGetPopulationTarget(int originalTarget, int scaledTarget) {
        if (Config.Instance.EnabledUnlockAll) return 0;
        else return base.OnGetPopulationTarget(originalTarget, scaledTarget);
    }
}

