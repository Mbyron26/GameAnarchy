namespace GameAnarchy;
using ColossalFramework;
using CSShared.Debug;
using ICities;

public class OilAndOreResourceExtension : ResourceExtensionBase {
    public override void OnCreated(IResource resource) {
        base.OnCreated(resource);
        LogManager.GetLogger().Info("Call resource extension OnCreated");
    }

    public override void OnReleased() => LogManager.GetLogger().Info("Call resource extension OnReleased");

    public override void OnAfterResourcesModified(int x, int z, NaturalResource type, int amount) {
        if (amount < 0) {
            if (type == NaturalResource.Oil) {
                if (Config.Instance.OilDepletionRate == 0) {
                    resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                } else if (Config.Instance.OilDepletionRate != 100 && Singleton<SimulationManager>.instance.m_randomizer.Int32(100u) >= Config.Instance.OilDepletionRate) {
                    resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                }
            } else if (type == NaturalResource.Ore) {
                if (Config.Instance.OreDepletionRate == 0) {
                    resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                } else if (Config.Instance.OreDepletionRate != 100 && Singleton<SimulationManager>.instance.m_randomizer.Int32(100u) >= Config.Instance.OreDepletionRate) {
                    resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
                }
            }
        }
    }
}
