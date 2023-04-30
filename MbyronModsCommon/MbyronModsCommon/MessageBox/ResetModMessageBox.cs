using ColossalFramework.UI;
using MbyronModsCommon.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon {
    public class ResetModWarningMessageBox : MessageBoxBase {
        public void Init<Mod>(Action callback) where Mod : IMod {
            TitleText = $"{ModMainInfo<Mod>.ModName} {CommonLocalize.Reset}";
            var label = CustomUILabel.Add(MainPanel, CommonLocalize.ResetModWarning, MessageBoxParm.ComponentWidth);
            label.TextHorizontalAlignment = UIHorizontalAlignment.Center;
            label.TextVerticalAlignment = UIVerticalAlignment.Middle;
            var button1 = AddButtons(1, 2, CommonLocalize.MessageBox_OK, () => callback());
            button1.TextNormalColor = Color.red;
            var button2 = AddButtons(2, 2, "Cancel", Close);
        }
    }

    public class ResetModMessageBox : SimpleMessageBox {
        public void Init<Mod>(bool isSucceeded = true) where Mod : IMod {
            TitleText = $"{ModMainInfo<Mod>.ModName} {CommonLocalize.Reset}";
            if (isSucceeded) {
                CustomUILabel.Add(MainPanel, CommonLocalize.ResetModSucceeded, MessageBoxParm.ComponentWidth).TextHorizontalAlignment = UIHorizontalAlignment.Center;
            } else {
                CustomUILabel.Add(MainPanel, CommonLocalize.ResetModFailed, MessageBoxParm.ComponentWidth).TextHorizontalAlignment = UIHorizontalAlignment.Center;
            }
        }
    }

}
