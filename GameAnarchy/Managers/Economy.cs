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
            Mod.Log.Info($"AutoAddMoney | GetMoney: {Config.Instance.DefaultGetCash * 100}, LastCashAmount: {economyManager.LastCashAmount}");
        }
        else {
            Mod.Log.Info($"Auto add money failed, EconomyManager doesn't exist.");
        }
    }

    public void SetStartMoney() {
        if (!Config.Instance.EnableStartMoney)
            return;
        if (Singleton<EconomyManager>.exists) {
            typeof(EconomyManager).GetField("m_cashAmount", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Singleton<EconomyManager>.instance, Config.Instance.StartMoneyAmount * 100);
            Mod.Log.Info($"Start money enabled, set start money to {Config.Instance.StartMoneyAmount}");
        }
        else {
            Mod.Log.Info($"Set start money failed, EconomyManager doesn't exist.");
        }
    }

    public void AddMoneyManually() => ModifyMoney(Config.Instance.DefaultGetCash);

    public void SubstrateMoneyManually() => ModifyMoney(-Config.Instance.DefaultGetCash);

    private void ModifyMoney(int amount) {
        if (!Config.Instance.CashAnarchy)
            return;
        if (!Singleton<EconomyManager>.exists) {
            Mod.Log.Error("Couldn't modify manually, EconomyManager doesn't exist");
            return;
        }
        var economyManager = Singleton<EconomyManager>.instance;
        AddLoanAmount(economyManager, amount);
        Mod.Log.Info($"ModifyMoney: {amount * 100}, LastCashAmount: {economyManager.LastCashAmount}");
    }

    private void AddLoanAmount(EconomyManager economyManager, int amount) => economyManager.AddResource(EconomyManager.Resource.LoanAmount, amount * 100, ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
}
