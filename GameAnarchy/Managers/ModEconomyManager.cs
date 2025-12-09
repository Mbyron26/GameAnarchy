using System;
using System.Reflection;
using ColossalFramework;
using CSLModsCommon;
using CSLModsCommon.KeyBindings;
using CSLModsCommon.Manager;
using GameAnarchy.Data;
using GameAnarchy.ModSettings;
using ICities;

namespace GameAnarchy.Managers;

public class ModEconomyManager : ManagerBase, ISimulation {
    private ModSetting _modSetting;
    private InGameToolButtonManager _toolButtonManager;
    private KeyBindingManager _keyBindingManager;
    private DateTime _nextPayment = DateTime.MinValue;

    protected override void OnCreate() {
        base.OnCreate();
        _modSetting = Domain.GetOrCreateManager<SettingManager>().GetSetting<ModSetting>();
        _toolButtonManager = Domain.GetOrCreateManager<InGameToolButtonManager>();
        _keyBindingManager = Domain.GetOrCreateManager<KeyBindingManager>();
    }

    protected override void OnGameLoaded(LoadContext context) {
        base.OnGameLoaded(context);
        SetStartMoney();
        _keyBindingManager.Register(nameof(_modSetting.AddMoneyKeyBinding), _modSetting.AddMoneyKeyBinding, AddMoneyManually, KeyBindingContext.InGame);
        _keyBindingManager.Register(nameof(_modSetting.SubstrateMoneyKeyBinding), _modSetting.SubstrateMoneyKeyBinding, SubstrateMoneyManually, KeyBindingContext.InGame);
        _keyBindingManager.Register(nameof(_modSetting.ControlPanelToggleKeyBinding), _modSetting.ControlPanelToggleKeyBinding, _toolButtonManager.OnKeyBindingToggle, KeyBindingContext.InGame);
    }

    protected override void OnGameUnloaded() {
        base.OnGameUnloaded();
        _keyBindingManager.Unregister(nameof(_modSetting.AddMoneyKeyBinding));
        _keyBindingManager.Unregister(nameof(_modSetting.SubstrateMoneyKeyBinding));
        _keyBindingManager.Unregister(nameof(_modSetting.ControlPanelToggleKeyBinding));
    }

    public void OnBindThreadingContext(IThreading threading) { }
    public void OnUnbindThreadingContext() { }
    public void OnThreadingUpdate(float realTimeDelta, float simulationTimeDelta) { }
    public void OnPreSimulationTick() { }

    public void OnPreSimulationFrame() => ChargeInterest();

    public void OnPostSimulationFrame() { }
    public void OnPostSimulationTick() { }

    public void ChargeInterest() {
        if (!_modSetting.ChargeInterest)
            return;
        var currentGameTime = Singleton<SimulationManager>.instance.m_currentGameTime;
        if (currentGameTime.Ticks > _nextPayment.Ticks) {
            var currentRawMoney = EconomyManager.playerMoney;
            if (currentRawMoney < 0) {
                var fee = (int)Math.Ceiling(currentRawMoney * 100 * _modSetting.AnnualInterestRate / 52.143);
                EconomyManager.instance.FetchResource(EconomyManager.Resource.LoanPayment, -fee, ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
            }

            _nextPayment = currentGameTime.AddDays(7);
        }
    }

    public void AutoAddMoney(Func<long> getCurrentMoney = null) {
        if (_modSetting.CurrentMoneyMode != MoneyMode.Anarchy)
            return;
        if (Singleton<EconomyManager>.exists) {
            var economyManager = Singleton<EconomyManager>.instance;
            getCurrentMoney ??= () => economyManager.LastCashAmount;
            if (getCurrentMoney() >= _modSetting.DefaultMinAmount * 100) return;
            AddLoanAmount(economyManager, _modSetting.DefaultGetCash);
            Logger.Info($"AutoAddMoney | GetMoney: {_modSetting.DefaultGetCash * 100}, LastCashAmount: {economyManager.LastCashAmount}");
        }
        else {
            Logger.Info($"Auto add money failed, EconomyManager doesn't exist.");
        }
    }

    private void SetStartMoney() {
        if (!_modSetting.EnableStartMoney)
            return;
        if (Singleton<EconomyManager>.exists) {
            typeof(EconomyManager).GetField("m_cashAmount", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(Singleton<EconomyManager>.instance, _modSetting.StartMoneyAmount * 100);
            Logger.Info($"Start money enabled, set start money to {_modSetting.StartMoneyAmount}");
        }
        else {
            Logger.Info($"Set start money failed, EconomyManager doesn't exist.");
        }
    }

    public void AddMoneyManually() => ModifyMoney(_modSetting.DefaultGetCash);

    public void SubstrateMoneyManually() => ModifyMoney(-_modSetting.DefaultGetCash);

    private void ModifyMoney(int amount) {
        if (_modSetting.CurrentMoneyMode != MoneyMode.Anarchy)
            return;
        if (!Singleton<EconomyManager>.exists) {
            Logger.Error("Couldn't modify manually, EconomyManager doesn't exist");
            return;
        }

        var economyManager = Singleton<EconomyManager>.instance;
        AddLoanAmount(economyManager, amount);
        Logger.Info($"ModifyMoney: {amount * 100}, LastCashAmount: {economyManager.LastCashAmount}");
    }

    private void AddLoanAmount(EconomyManager economyManager, int amount) => economyManager.AddResource(EconomyManager.Resource.LoanAmount, amount * 100, ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
}