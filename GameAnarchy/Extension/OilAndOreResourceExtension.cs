using ColossalFramework;
using CSLModsCommon.Logging;
using CSLModsCommon.Manager;
using GameAnarchy.ModSettings;
using ICities;

namespace GameAnarchy.Extension;

public class OilAndOreResourceExtension : ResourceExtensionBase {
    private ModSetting _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();

    public override void OnCreated(IResource resource) {
        base.OnCreated(resource);
        LogManager.GetLogger().Info("Call resource extension OnCreated");
    }

    public override void OnReleased() => LogManager.GetLogger().Info("Call resource extension OnReleased");

    public override void OnAfterResourcesModified(int x, int z, NaturalResource type, int amount) {
        if (amount >= 0) return;
        if (type == NaturalResource.Oil) {
            if (_modSetting.OilDepletionRate == 0)
                resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
            else if (_modSetting.OilDepletionRate != 100 && Singleton<SimulationManager>.instance.m_randomizer.Int32(100u) >= _modSetting.OilDepletionRate) resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
        }
        else if (type == NaturalResource.Ore) {
            if (_modSetting.OreDepletionRate == 0)
                resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
            else if (_modSetting.OreDepletionRate != 100 && Singleton<SimulationManager>.instance.m_randomizer.Int32(100u) >= _modSetting.OreDepletionRate) resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
        }
    }
}