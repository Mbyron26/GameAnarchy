using ColossalFramework;
using CSLModsCommon.Logging;
using CSLModsCommon.Manager;
using GameAnarchy.Data;
using GameAnarchy.Managers;
using GameAnarchy.ModSettings;
using ICities;

namespace GameAnarchy.Extension;

public class EconomyExtension : EconomyExtensionBase {
    private ModSetting _modSetting;

    public override void OnCreated(IEconomy economy) {
        base.OnCreated(economy);
        _modSetting = Domain.DefaultDomain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
        LogManager.GetLogger().Info("Call economy extension OnCreated");
    }

    public override void OnReleased() => LogManager.GetLogger().Info("Call economy extension OnReleased");

    public override long OnUpdateMoneyAmount(long internalMoneyAmount) {
        if (Singleton<EconomyManager>.exists && Singleton<EconomyManager>.instance.m_properties is not null)
            Singleton<EconomyManager>.instance.m_properties.m_bailoutLimit = _modSetting.CityBankruptcyWarningThreshold * 100;
        if (_modSetting.CurrentMoneyMode == MoneyMode.Unlimited)
            return long.MaxValue;
        managers.threading.QueueMainThread(() => Domain.DefaultDomain.GetOrCreateManager<ModEconomyManager>().AutoAddMoney(() => managers.economy.internalMoneyAmount));
        return internalMoneyAmount;
    }

    public override bool OverrideDefaultPeekResource => _modSetting.CurrentMoneyMode == MoneyMode.Unlimited || _modSetting.CurrentMoneyMode == MoneyMode.Anarchy || _modSetting.RemoveNotEnoughMoney;

    public override int OnPeekResource(EconomyResource resource, int amount) {
        if (resource == EconomyResource.PolicyCost && _modSetting.NoPoliciesCosts) return 0;

        return amount;
    }

    public override int OnAddResource(EconomyResource resource, int amount, Service service, SubService subService, Level level) {
        if (resource == EconomyResource.PolicyCost && _modSetting.NoPoliciesCosts) return 0;

        return service switch {
            Service.Residential => amount * _modSetting.ResidentialMultiplierFactor,
            Service.Industrial => amount * _modSetting.IndustrialMultiplierFactor,
            Service.Commercial => amount * _modSetting.CommercialMultiplierFactor,
            Service.Office => amount * _modSetting.OfficeMultiplierFactor,
            _ => amount
        };
    }

    public override int OnFetchResource(EconomyResource resource, int amount, Service service, SubService subService, Level level) {
        if (resource == EconomyResource.PolicyCost && _modSetting.NoPoliciesCosts) return 0;

        return amount;
    }
}