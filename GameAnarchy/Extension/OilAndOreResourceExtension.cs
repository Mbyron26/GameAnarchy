using ICities;

namespace GameAnarchy {
    public class OilAndOreResourceExtension : ResourceExtensionBase {
        public override void OnAfterResourcesModified(int x, int z, NaturalResource type, int amount) {
            if (Config.Instance.UnlimitedOil) {
                if (type == NaturalResource.Oil && amount < 0)
                    resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
            }
            if (Config.Instance.UnlimitedOre) {
                if (type == NaturalResource.Ore && amount < 0)
                    resourceManager.SetResource(x, z, type, (byte)(resourceManager.GetResource(x, z, type) - amount), false);
            }

        }
    }
}
