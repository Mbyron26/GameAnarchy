using ColossalFramework;
using CSShared.Debug;
using CSShared.Manager;
using GameAnarchy.Managers;
using ICities;

namespace GameAnarchy.Extension;

public class EconomyExtension : EconomyExtensionBase {
    public override void OnCreated(IEconomy economy) {
        base.OnCreated(economy);
        LogManager.GetLogger().Info("Call economy extension OnCreated");
    }

    public override void OnReleased() => LogManager.GetLogger().Info("Call economy extension OnReleased");

    public override long OnUpdateMoneyAmount(long internalMoneyAmount) {
        if (Singleton<EconomyManager>.exists && Singleton<EconomyManager>.instance.m_properties is not null)
            Singleton<EconomyManager>.instance.m_properties.m_bailoutLimit = Config.Instance.CityBankruptcyWarningThreshold * 100;
        if (Config.Instance.UnlimitedMoney)
            return long.MaxValue;
        managers.threading.QueueMainThread(() => ManagerPool.GetOrCreateManager<Manager>().AutoAddMoney(() => managers.economy.internalMoneyAmount));
        return internalMoneyAmount;
    }

    public override bool OverrideDefaultPeekResource => Config.Instance.UnlimitedMoney || Config.Instance.CashAnarchy || Config.Instance.RemoveNotEnoughMoney;

    public override int OnPeekResource(EconomyResource resource, int amount) {
        if (resource == EconomyResource.PolicyCost && Config.Instance.NoPoliciesCosts) {
            return 0;
        }
        return amount;
    }

    public override int OnAddResource(EconomyResource resource, int amount, Service service, SubService subService, Level level) {
        if (resource == EconomyResource.PolicyCost && Config.Instance.NoPoliciesCosts) {
            return 0;
        }
        return service switch {
            Service.Residential => amount * Config.Instance.ResidentialMultiplierFactor,
            Service.Industrial => amount * Config.Instance.IndustrialMultiplierFactor,
            Service.Commercial => amount * Config.Instance.CommercialMultiplierFactor,
            Service.Office => amount * Config.Instance.OfficeMultiplierFactor,
            _ => amount
        };
    }
    public override int OnFetchResource(EconomyResource resource, int amount, Service service, SubService subService, Level level) {
        if (resource == EconomyResource.PolicyCost && Config.Instance.NoPoliciesCosts) {
            return 0;
        }
        return amount;
    }
}