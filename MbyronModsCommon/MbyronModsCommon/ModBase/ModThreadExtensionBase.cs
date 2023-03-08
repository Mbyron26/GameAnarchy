using ICities;
using System;

namespace MbyronModsCommon {
    public class ModThreadExtensionBase : ThreadingExtensionBase {
        public void AddCallOnceInvoke(bool target, ref bool flag, Action action) {
            if (target) {
                if (!flag) {
                    flag = true;
                    action.Invoke();
                }
            } else {
                flag = false;
            }
        }
    }
}
