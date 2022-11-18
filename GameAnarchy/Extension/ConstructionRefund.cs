using ICities;

namespace GameAnarchy {
    public class ConstructionRefund : EconomyExtensionBase {
        public override int OnGetRefundAmount(int constructionCost, int refundAmount, Service service, SubService subService, Level level) {
            if (Config.Instance.Refund) return constructionCost;
            return refundAmount;
        }
    }
}
