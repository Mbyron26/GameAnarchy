using ColossalFramework;
using ICities;
using MbyronModsCommon;
using System;
using System.Reflection;

namespace GameAnarchy {
    public class EconomyExtension : EconomyExtensionBase {
        public override long OnUpdateMoneyAmount(long internalMoneyAmount) {
            if (Config.Instance.UnlimitedMoney) return long.MaxValue;
            managers.threading.QueueMainThread(() => AutoAddCash(Config.Instance.CashAnarchy, Config.Instance.DefaultGetCash, () => managers.economy.internalMoneyAmount));
            return internalMoneyAmount;
        }
        public override bool OverrideDefaultPeekResource {
            get {
                if (Config.Instance.UnlimitedMoney) return true;
                return false;
            }
        }

        public static void AutoAddCash(bool isEnabled, int amount, Func<long> getCurrentMoney = null) {
            if (!isEnabled) return;
            try {
                if (Singleton<EconomyManager>.exists) {
                    EconomyManager economyMgr = Singleton<EconomyManager>.instance;
                    getCurrentMoney ??= (() => economyMgr.LastCashAmount);
                    if (getCurrentMoney() >= Config.Instance.DefaultMinAmount * 100) return;
                    AddLoanResource(economyMgr, amount);
                }
            }
            catch (Exception e) {
                InternalLogger.Log($"Auto add cash failed.", e);
            }
        }

        public static void ManuallyAddCash() {
            if (Config.Instance.CashAnarchy) {
                if (Singleton<EconomyManager>.exists) {
                    EconomyManager economyManager = Singleton<EconomyManager>.instance;
                    AddLoanResource(economyManager, Config.Instance.DefaultGetCash);
                }
            };
        }

        public static void UpdateStartCash() {
            try {
                if (Singleton<EconomyManager>.exists && Config.Instance.EnabledInitialCash) {
                    var cashAmount = typeof(EconomyManager).GetField("m_cashAmount", BindingFlags.Instance | BindingFlags.NonPublic);
                    cashAmount.SetValue(Singleton<EconomyManager>.instance, Config.Instance.InitialCash * 100);
                }
            }
            catch (Exception e) {
                InternalLogger.Exception($"Update start cash fail, economyManager state: {Singleton<EconomyManager>.exists}, Initial Money function: {Config.Instance.EnabledInitialCash}.", e);
            }
        }

        private static void AddLoanResource(EconomyManager economyManager, int amount) {
            economyManager.AddResource(EconomyManager.Resource.LoanAmount, amount * 100, ItemClass.Service.None, ItemClass.SubService.None, ItemClass.Level.None);
        }

    }
}
