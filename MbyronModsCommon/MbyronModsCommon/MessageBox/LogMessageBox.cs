using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace MbyronModsCommon {
    public class LogMessageBox : MessageBoxBase {
        public LogMessageBox() {
            AddButtons(1, 1, CommonLocalize.MessageBox_OK, Close);
        }

        public void Initialize<Mod>(bool maximizeFirst = true) where Mod : IMod {
            TitleText = $"{ModMainInfo<Mod>.ModName} {CommonLocalize.ChangeLog}";
            MainPanel.autoLayoutPadding = new RectOffset(10, 10, 10, 0);
            if (SingletonMod<Mod>.Instance.ChangeLog.Count == 0) {
                CustomPanel.AddSpace(MainPanel, buttonWidth, 30);
                return;
            }
            var first = default(VersionPanel);
            foreach (var list in SingletonMod<Mod>.Instance.ChangeLog) {
                var versionPaenl = MainPanel.AddUIComponent<VersionPanel>();
                versionPaenl.InitValue(list.ModVersion.ToString(), list.Date.ToString("yyyy/MM/dd"), list.Log);
                if (first == null) first = versionPaenl;
            }
            if (maximizeFirst)
                first.IsMinimize = false;
        }

        public class VersionPanel : AutoLayoutPanel {
            private UILabel versionNumber;
            private UILabel versionDate;
            private UILabel arrowMark;
            private AutoLayoutPanel container;
            public List<LogPanel> LogLists { get; private set; } = new List<LogPanel>();
            public VersionPanel() {
                name = nameof(VersionPanel);
                autoLayoutPadding = new RectOffset(0, 0, 0, 5);
                autoLayoutDirection = LayoutDirection.Vertical;
                autoFitChildrenVertically = true;
                width = 570;
                InitTitle();
                InitContainer();
            }

            public bool IsMinimize {
                get => !container.isVisible;
                set {
                    container.isVisible = !value;
                    arrowMark.text = value ? "▶" : "▼";
                }
            }

            public void InitValue(string _versionNumber, string _versionDate, List<string> _logLists) {
                versionNumber.text = _versionNumber;
                versionDate.text = _versionDate;
                foreach (var logList in _logLists) {
                    if (logList != null) {
                        var logPanel = container.AddUIComponent<LogPanel>();
                        //logPanel.width = width - autoLayoutPadding.horizontal;
                        LogLists.Add(logPanel);
                        var total = ObtainSplitLog(logList);
                        var type = total[0];
                        var log = total[1];
                        logPanel.FillValue(type, log);
                    } else return;
                }
                if (LogLists.Count > 0) {
                    container.isVisible = true;
                    IsMinimize = true;
                } else container.isVisible = false;

            }

            private void InitTitle() {
                var titlePanel = AddUIComponent<UIPanel>();
                titlePanel.autoLayout = false;
                titlePanel.size = new Vector2(260, 40);
                titlePanel.clipChildren = true;
                titlePanel.eventClicked += (c, e) => IsMinimize = !IsMinimize;

                var backgroundField = titlePanel.AddUIComponent<UIPanel>();
                backgroundField.autoLayout = false;
                backgroundField.size = new Vector2(220, 40);
                backgroundField.clipChildren = true;
                backgroundField.backgroundSprite = "TextFieldPanel";
                backgroundField.color = new Color32(89, 115, 234, 255);
                backgroundField.relativePosition = Vector2.zero;

                versionNumber = backgroundField.AddUIComponent<UILabel>();
                versionNumber.textAlignment = UIHorizontalAlignment.Left;
                versionNumber.width = 150f;
                versionNumber.autoHeight = true;
                versionNumber.textScale = 1.7f;
                versionNumber.relativePosition = new Vector2(10f, 5f);

                versionDate = backgroundField.AddUIComponent<UILabel>();
                versionDate.textAlignment = UIHorizontalAlignment.Right;
                versionDate.width = 120f;
                versionDate.autoHeight = true;
                versionDate.textScale = 1.3f;
                versionDate.relativePosition = new Vector2(90f, 10f);

                arrowMark = titlePanel.AddUIComponent<UILabel>();
                arrowMark.autoSize = false;
                arrowMark.autoHeight = true;
                arrowMark.textScale = 1.5f;
                arrowMark.textAlignment = UIHorizontalAlignment.Right;
                arrowMark.relativePosition = new Vector2(titlePanel.width - 25f, 6f);
            }

            private void InitContainer() {
                container = AddUIComponent<AutoLayoutPanel>();
                container.autoLayoutDirection = LayoutDirection.Vertical;
                container.autoLayoutPadding = new RectOffset(0, 0, 0, 5);
                container.autoFitChildrenVertically = true;
                container.width = 570f;
            }

            public List<string> ObtainSplitLog(string log) {
                if (log != null) {
                    var raw = log.Trim();
                    var type = raw.Substring(0, 5).Remove(0, 1).Remove(3, 1);
                    var info = raw.Substring(5, raw.Length - 5).Trim();
                    return new List<string> { type, info };
                }
                return null;
            }
        }

        public class LogPanel : AutoLayoutPanel {
            public LogPanel() {
                name = nameof(LogPanel);
                autoLayoutDirection = LayoutDirection.Horizontal;
                autoFitChildrenVertically = true;
                autoLayoutPadding = new RectOffset(0, 5, 0, 0);
                width = 570f;
                InitComponents();
            }

            private UILabel Category { get; set; }
            private UILabel Info { get; set; }

            private void InitComponents() {
                Category = AddUIComponent<UILabel>();
                Category.autoSize = false;
                Category.size = new Vector2(100, 22);
                Category.padding = new RectOffset(0, 0, 4, 0);
                Category.textScale = 0.8f;
                Category.textAlignment = UIHorizontalAlignment.Center;
                Category.verticalAlignment = UIVerticalAlignment.Middle;
                Category.atlas = CustomAtlas.InGameAtlas;
                Category.backgroundSprite = "TextFieldPanel";
                Info = AddUIComponent<UILabel>();
                Info.autoSize = false;
                Info.autoHeight = true;
                //Info.width = 465;
                Info.wordWrap = true;
                Info.textScale = 0.9f;
                Info.textAlignment = UIHorizontalAlignment.Left;
                Info.padding = new RectOffset(0, 0, 4, 0);
            }

            public void FillValue(string c, string i) {
                if (!string.IsNullOrEmpty(c)) {
                    Category.isVisible = true;
                    switch (c) {
                        case "ADD":
                            Category.color = new Color32(0, 176, 73, 255);
                            Category.text = CommonLocalize.LogMessageBox_Added;
                            break;
                        case "REM":
                            Category.color = new Color32(250, 67, 47, 255);
                            Category.text = CommonLocalize.LogMessageBox_Removed;
                            break;
                        case "UPT":
                            Category.color = new Color32(74, 105, 240, 255);
                            Category.text = CommonLocalize.LogMessageBox_Updated;
                            break;
                        case "FIX":
                            Category.color = new Color32(255, 162, 41, 255);
                            Category.text = CommonLocalize.LogMessageBox_Fixed;
                            break;
                        case "OPT":
                            Category.color = new Color32(131, 46, 184, 255);
                            Category.text = CommonLocalize.LogMessageBox_Optimized;
                            break;
                        case "ADJ":
                            Category.color = new Color32(2, 194, 184, 255);
                            Category.text = CommonLocalize.LogMessageBox_Adjusted;
                            break;
                        default:
                            Category.color = new Color32(204, 204, 204, 255);
                            Category.text = c;
                            break;
                    }
                } else Category.isVisible = false;
                Info.text = i;
                SelfAdaptive();
            }
            protected override void OnSizeChanged() {
                base.OnSizeChanged();
                SelfAdaptive();
            }
            protected override void OnVisibilityChanged() {
                base.OnVisibilityChanged();
                SelfAdaptive();
            }
            private void SelfAdaptive() {
                if (Info != null)
                    Info.width = width - (Category?.isVisible == true ? Category.width + autoLayoutPadding.right : 0f);
            }

        }
    }
}
