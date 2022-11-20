using ColossalFramework.UI;
using ColossalFramework;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon {
    public class CompatibilityMessageBox : MessageBoxBase {
        public AdvancedAutoFitChildrenVerticallyPanel Card { get; private set; }
        public CompatibilityMessageBox() {
            AddButtons(1, 1, CommonLocale.MessageBox_OK, Close);
        }

        public void Initialize<Mod>() where Mod : IMod {
            TitleText = ModMainInfo<Mod>.ModName;
            Card = MainPanel.AddCard();
            if (CompatibilityCheck.IsConflict) {
                AddWarningFlag();
                AddPrompt(CommonLocale.MessageBox_WarningPrompt);
                AddList(Card, CompatibilityCheck.IncompatibleModsInfo);
            } else {
                AddNormalFlag();
                AddPrompt(CommonLocale.MessageBox_NormalPrompt);
            }
        }

        private void AddList(UIPanel root, List<string> list) {
            foreach (var item in list) {
                var label = root.AddUIComponent<UILabel>();
                label.autoSize = false;
                label.wordWrap = true;
                label.width = 560;
                label.autoHeight = true;
                label.text = item;
                label.autoHeight = false;
            }
        }

        private void AddPrompt(string _text) {
            UILabel prompt = Card.AddUIComponent<UILabel>();
            prompt.autoSize = false;
            prompt.width = 560;
            prompt.autoHeight = true;
            prompt.textScale = 1.1f;
            prompt.textColor = UIColor.Yellow;
            prompt.wordWrap = true;
            prompt.text = _text;
        }

        private UIPanel AddNormalFlag() => AddFlag(Card, NormalFlag);
        private UIPanel AddWarningFlag() => AddFlag(Card, WarningFlag);

        private FlagType NormalFlag = new(CommonLocale.MessageBox_NORMAL, UIColor.Green);
        private FlagType WarningFlag = new(CommonLocale.MessageBox_WARNING, UIColor.Red);

        private UIPanel AddFlag(UIPanel root, FlagType flagType) {
            UIPanel basePanel = root.AddUIComponent<UIPanel>();
            basePanel.width = basePanel.parent.width - 20;
            basePanel.height = 54f;
            basePanel.clipChildren = true;
            basePanel.autoLayout = false;
            UILabel label = basePanel.AddUIComponent<UILabel>();
            label.textScale = 1.8f;
            label.size = new Vector2(400f, 50f);
            label.textAlignment = UIHorizontalAlignment.Center;
            label.textColor = flagType.textColor;
            label.text = flagType.text;
            label.relativePosition = new Vector2(12f, 12f);
            return basePanel;
        }

        private struct FlagType {
            public string text;
            public Color32 textColor;
            public FlagType(string _text, Color32 _textColor) {
                text = _text;
                textColor = _textColor;
            }
        }

    }
}
