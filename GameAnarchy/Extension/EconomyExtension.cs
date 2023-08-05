namespace GameAnarchy;
using ColossalFramework;
using ICities;
using MbyronModsCommon;

public class EconomyExtension : EconomyExtensionBase {
    public override void OnCreated(IEconomy economy) {
        base.OnCreated(economy);
        InternalLogger.Log("Call economy extension OnCreated");
    }

    public override void OnReleased() =>InternalLogger.Log("Call economy extension OnReleased");

    public override long OnUpdateMoneyAmount(long internalMoneyAmount) {
        if (Singleton<EconomyManager>.exists && Singleton<EconomyManager>.instance.m_properties is not null)
            Singleton<EconomyManager>.instance.m_properties.m_bailoutLimit = Config.Instance.CityBankruptcyWarningThreshold * 100;
        if (Config.Instance.UnlimitedMoney)
            return long.MaxValue;
        managers.threading.QueueMainThread(() => SingletonManager<Manager>.Instance.AutoAddMoney(() => managers.economy.internalMoneyAmount));
        return internalMoneyAmount;
    }

    public override bool OverrideDefaultPeekResource => Config.Instance.UnlimitedMoney || Config.Instance.CashAnarchy || Config.Instance.RemoveNotEnoughMoney;

    public override int OnPeekResource(EconomyResource resource, int amount) => amount;

    public override int OnAddResource(EconomyResource resource, int amount, Service service, SubService subService, Level level) => service switch {
        Service.Residential => amount * Config.Instance.ResidentialMultiplierFactor,
        Service.Industrial => amount * Config.Instance.IndustrialMultiplierFactor,
        Service.Commercial => amount * Config.Instance.CommercialMultiplierFactor,
        Service.Office => amount * Config.Instance.OfficeMultiplierFactor,
        _ => amount
    };
}