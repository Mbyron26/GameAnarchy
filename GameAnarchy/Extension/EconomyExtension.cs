namespace GameAnarchy;
using ColossalFramework;
using ICities;
using MbyronModsCommon;
using System;
using System.Reflection;

public class EconomyExtension : EconomyExtensionBase {
    public override long OnUpdateMoneyAmount(long internalMoneyAmount) {
        if (Singleton<EconomyManager>.exists && Singleton<EconomyManager>.instance.m_properties is not null)
            Singleton<EconomyManager>.instance.m_properties.m_bailoutLimit = Config.Instance.CityBankruptcyWarningThreshold * 100;
        if (Config.Instance.UnlimitedMoney)
            return long.MaxValue;
        managers.threading.QueueMainThread(() => AutoAddMoney(() => managers.economy.internalMoneyAmount));
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

    public static void AutoAddMoney(Func<long> getCurrentMoney = null) {
        if (!Config.Instance.CashAnarchy) return;
        if (Singleton<EconomyManager>.exists) {
            EconomyManager economyMgr = Singleton<EconomyManager>.instance;
            getCurrentMoney ??= (() => economyMgr.LastCashAmount);
            if (getCurrentMoney() >= Config.Instance.DefaultMinAmount * 100) return;
            AddLoanAmount(economyMgr, Config.Instance.DefaultGetCash);
        } else {
            InternalLogger.Log($"Auto add money failed, EconomyManager doesn't exist.");
        }
    }
    public static void SetStartMoney() {
        if (!Config.Instance.EnableStartMoney) return;
        if (Singleton<EconomyManager>.exists) {
            typeof(EconomyManager).GetField("m_cashAmount", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Singleton<EconomyManager>.instance, Config.Instance.StartMoneyAmount * 100);
            InternalLogger.Log($"Start money enabled, set start money to {Config.Instance.StartMoneyAmount}.");
        } else {
            InternalLogger.Log($"Set start money failed, EconomyManager doesn't exist.");
        }
    }
    public static void AddMoneyManually() {
        if (!Config.Instance.CashAnarchy) return;
        if (Singleton<EconomyManager>.exists) {
            var economyManager = Singleton<EconomyManager>.instance;
            AddLoanAmount(economyManager, Config.Instance.DefaultGetCash);
            ExternalLogger.DebugMode($"AddMoneyManually | GetMoney: {Config.Instance.DefaultGetCash * 100}, LastCashAmount: {economyManager.LastCashAmount}", Config.Instance.DebugMode);
        } else {
            InternalLogger.Error("Couldn't add money manually, EconomyManager doesn't exist.");
        }
    }
    private static void AddLoanAmount(EconomyManager economyManager, int amount) => economyManager.AddResource(EconomyManager.Resource.LoanAmount, amount * 100, ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
}