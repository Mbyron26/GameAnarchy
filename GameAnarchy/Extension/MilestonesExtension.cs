using System.Collections.Generic;
using System.Linq;
using ICities;
namespace GameAnarchy {
    public class MilestonesExtension : MilestonesExtensionBase {
        public override void OnRefreshMilestones() {
            if (Config.Instance.EnabledUnlockAll) {
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


                milestonesManager.UnlockMilestone("Basic Road Created");
                milestonesManager.UnlockMilestone("Train Track Requirements");
                milestonesManager.UnlockMilestone("Metro Track Requirements");


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

                UnlockBuildings(new List<string>
                {
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
            if(Config.Instance.EnabledUnlockAll) return 0;
            else  return base.OnGetPopulationTarget(originalTarget, scaledTarget);
        }
    }
}

