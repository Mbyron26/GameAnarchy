using ICities;

namespace GameAnarchy {
    public class EconomicIncomeExtension : EconomyExtensionBase {
        public override int OnAddResource(EconomyResource resource, int amount, Service service, SubService subService, Level level) {
            if (service == Service.Residential) {
                int residentialAmount = amount * Config.Instance.ResidentialMultiplierFactor;
                return residentialAmount;
            }
            if (service == Service.Industrial) {
                int industrialAmount = amount * Config.Instance.IndustrialMultiplierFactor;
                return industrialAmount;
            }
            if (service == Service.Commercial) {
                int commercialAmount = amount * Config.Instance.CommercialMultiplierFactor;
                return commercialAmount;
            }
            if (service == Service.Office) {
                int officeAmount = amount * Config.Instance.OfficeMultiplierFactor;
                return officeAmount;
            }
            return amount;
        }
    }
}
