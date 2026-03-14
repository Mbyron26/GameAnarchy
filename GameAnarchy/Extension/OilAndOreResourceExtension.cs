using ColossalFramework;
using CSLModsCommon.Logging;
using CSLModsCommon.Manager;
using GameAnarchy.ModSettings;
using ICities;
using UnityEngine;

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

        var rate = type switch {
            NaturalResource.Oil => _modSetting.OilDepletionRate,
            NaturalResource.Ore => _modSetting.OreDepletionRate,
            _ => -1
        };
        if (rate is < 0 or 100) return;

        if (rate != 0) {
            if (Singleton<SimulationManager>.instance.m_randomizer.Int32(100u) < rate)
                return;
        }

        int current = resourceManager.GetResource(x, z, type);
        var delta = -amount;
        var newValue = Mathf.Clamp(current + delta, 0, 255);
        resourceManager.SetResource(x, z, type, (byte)newValue, false);
    }
}