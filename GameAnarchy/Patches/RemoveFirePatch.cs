using HarmonyLib;

namespace GameAnarchy {
    [HarmonyPatch(typeof(PlayerBuildingAI), "GetFireParameters")]
    internal static class PlayerBuildingAIFirePatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                if (fireHazard != 0) {
                    fireHazard = 0;
                }
                fireTolerance = 1000;
            }
        }
    }

    [HarmonyPatch(typeof(ResidentialBuildingAI), "GetFireParameters")]
    internal static class ResidentialBuildingAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) {
                fireHazard = 80;
                fireTolerance = 8;
            }
        }
    }

    [HarmonyPatch(typeof(IndustrialBuildingAI), "GetFireParameters")]
    internal static class IndustrialBuildingAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) {
                fireHazard = 200;
                fireTolerance = 10;
            }
        }
    }

    [HarmonyPatch(typeof(CommercialBuildingAI), "GetFireParameters")]
    internal static class CommercialBuildingAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) {
                fireHazard = 150;
                fireTolerance = 10;
            }
        }
    }

    [HarmonyPatch(typeof(OfficeBuildingAI), "GetFireParameters")]
    internal static class OfficeBuildingAIPatch {
        public static void Postfix(ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) {
                fireTolerance = 10;
            }
        }
    }

    [HarmonyPatch(typeof(IndustrialExtractorAI), "GetFireParameters")]
    internal static class IndustrialExtractorAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            }
        }
    }

    [HarmonyPatch(typeof(ParkBuildingAI), "GetFireParameters")]
    internal static class ParkBuildingAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) {
                fireTolerance = 20;
            }
        }
    }

    [HarmonyPatch(typeof(MuseumAI), "GetFireParameters")]
    internal static class MuseumAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) {
                fireTolerance = 20;
            }
        }
    }

    [HarmonyPatch(typeof(DummyBuildingAI), "GetFireParameters")]
    internal static class DummyBuildingAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            }
        }
    }

    [HarmonyPatch(typeof(CampusBuildingAI), "GetFireParameters")]
    internal static class CampusBuildingAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) {
                fireTolerance = 20;
            }
        }
    }

    [HarmonyPatch(typeof(AirportGateAI), "GetFireParameters")]
    internal static class AirportGateAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

    [HarmonyPatch(typeof(AirportCargoGateAI), "GetFireParameters")]
    internal static class AirportCargoGateAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

    [HarmonyPatch(typeof(AirportBuildingAI), "GetFireParameters")]
    internal static class AirportBuildingAIPatch {
        public static void Postfix(ref int fireHazard, ref int fireTolerance) {
            if (Config.Instance.RemoveFire) {
                fireHazard = 0;
                fireTolerance = 1000;
            } else if (!Config.Instance.RemoveFire) fireTolerance = 20;
        }
    }

}
