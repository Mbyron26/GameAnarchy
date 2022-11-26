﻿using ColossalFramework;
using ICities;

namespace GameAnarchy {
    public class OilAndOreResourceExtension : ResourceExtensionBase {
        public override void OnAfterResourcesModified(int x, int z, NaturalResource type, int amount) {
            if ((type == NaturalResource.Oil || type == NaturalResource.Ore) && amount < 0) {
                if (type == NaturalResource.Oil) {
                    if (Config.Instance.OilDepletionRate == 100) return;
                    if (Config.Instance.OilDepletionRate == 0) {
                        resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                    } else {
                        if (Singleton<SimulationManager>.instance.m_randomizer.Int32(100u) >= Config.Instance.OilDepletionRate) {
                            resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                        }
                    }
                } else if (type == NaturalResource.Ore) {
                    if (Config.Instance.OreDepletionRate == 100) return;
                    if (Config.Instance.OreDepletionRate == 0) {
                        resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                    } else {
                        if (Singleton<SimulationManager>.instance.m_randomizer.Int32(100u) >= Config.Instance.OreDepletionRate) {
                            resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                        }
                    }
                }
            }
        }
    }
}
