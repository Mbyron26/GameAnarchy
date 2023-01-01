﻿using ColossalFramework.UI;

namespace MbyronModsCommon {
    public class XMLWariningMessageBox : SimpleMessageBox {
        public UILabel WarningText { get; set; }
        public override void Initialize<Mod>() {
            base.Initialize<Mod>();
            WarningText = MainPanel.AddUIComponent<UILabel>();
            WarningText.autoSize = false;
            WarningText.autoHeight = true;
            WarningText.width = 560f;
            WarningText.textScale = 1.3f;
            WarningText.textAlignment = UIHorizontalAlignment.Center;
            WarningText.wordWrap = true;
            WarningText.text = CommonLocale.XMLWariningMessageBox_Warning;
        }
    }
    public class SimpleMessageBox : MessageBoxBase {
        //public AdvancedAutoFitChildrenVerticallyPanel Card { get; private set; }
        public SimpleMessageBox() {
            AddButtons(1, 1, CommonLocale.MessageBox_OK, Close);
        }
        public virtual void Initialize<Mod>() where Mod : IMod {
            TitleText = ModMainInfo<Mod>.ModName;
            //Card = MainPanel.AddCard();
        }
    }
}
