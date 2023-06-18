namespace GameAnarchy;
using ColossalFramework;
using System;
using System.Reflection;

public partial class Manager {
    private DateTime NextPayment { get; set; } = DateTime.MinValue;

    //Integrate [You Can Build It] mod from Jose Gaspar, thanks to Jose Gaspar.
    public void ChargeInterest() {
        if (!Config.Instance.ChargeInterest)
            return;
        var currentGameTime = Singleton<SimulationManager>.instance.m_currentGameTime;
        if (currentGameTime.Ticks > NextPayment.Ticks) {
            var currentRawMoney = EconomyManager.playerMoney;
            if (currentRawMoney < 0) {
                int fee = (int)Math.Ceiling(currentRawMoney * 100 * Config.Instance.AnnualInterestRate / 52.143);
                EconomyManager.instance.FetchResource(EconomyManager.Resource.LoanPayment, -fee, ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
            }
            NextPayment = currentGameTime.AddDays(7);
        }
    }

    public void AutoAddMoney(Func<long> getCurrentMoney = null) {
        if (!Config.Instance.CashAnarchy) 
            return;
        if (Singleton<EconomyManager>.exists) {
            EconomyManager economyManager = Singleton<EconomyManager>.instance;
            getCurrentMoney ??= (() => economyManager.LastCashAmount);
            if (getCurrentMoney() >= Config.Instance.DefaultMinAmount * 100) return;
            AddLoanAmount(economyManager, Config.Instance.DefaultGetCash);
            InternalLogger.DebugMode<Config>($"AutoAddMoney | GetMoney: {Config.Instance.DefaultGetCash * 100}, LastCashAmount: {economyManager.LastCashAmount}");
        } else {
            InternalLogger.Log($"Auto add money failed, EconomyManager doesn't exist.");
        }
    }

    public void SetStartMoney() {
        if (!Config.Instance.EnableStartMoney)
            return;
        if (Singleton<EconomyManager>.exists) {
            typeof(EconomyManager).GetField("m_cashAmount", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Singleton<EconomyManager>.instance, Config.Instance.StartMoneyAmount * 100);
            InternalLogger.Log($"Start money enabled, set start money to {Config.Instance.StartMoneyAmount}");
        } else {
            InternalLogger.Log($"Set start money failed, EconomyManager doesn't exist.");
        }
    }

    public void AddMoneyManually() {
        if (!Config.Instance.CashAnarchy)
            return;
        if (Singleton<EconomyManager>.exists) {
            var economyManager = Singleton<EconomyManager>.instance;
            AddLoanAmount(economyManager, Config.Instance.DefaultGetCash);
            InternalLogger.DebugMode<Config>($"AddMoneyManually | GetMoney: {Config.Instance.DefaultGetCash * 100}, LastCashAmount: {economyManager.LastCashAmount}");
        } else {
            InternalLogger.Error("Couldn't add money manually, EconomyManager doesn't exist");
        }
    }

    private void AddLoanAmount(EconomyManager economyManager, int amount) => economyManager.AddResource(EconomyManager.Resource.LoanAmount, amount * 100, ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
}
