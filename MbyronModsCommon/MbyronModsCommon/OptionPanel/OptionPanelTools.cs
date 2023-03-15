using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace MbyronModsCommon {
    public class OptionPanelTools {
        private static OptionPanelTools instance;
        public readonly Color White = Color.white;

        public static OptionPanelTools Instance {
            get => instance ??= new OptionPanelTools();
        }
        public CustomListBox Group { get; set; }

        private OptionPanelTools() { }
        public CustomListBox AddGroup(UIComponent parent, float width, string caption) {
            Group = parent.AddUIComponent<CustomListBox>();
            Group.width = width;
            Group.Init(width, caption, new(10, 0, 0, 0), 1.125f, White, new(0, 0, 0, 4), SetMajorSprite, null);
            return Group;
        }

        private UIPanel AddChildPanel() => Group.AddChildPanel(SetMinorSprite);

        public UILabel AddMinorLabel(string text) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                return null;
            }
            return Group.AddMinorLabel(text, new(10, 0, 0, 0), 0.8f, new Color32(200, 200, 200, 255));
        }

        public UIPanel AddKeymapping(string text, KeyBinding keyBinding, string tooltip = null) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                return null;
            }
            var panel = AddChildPanel();
            CustomKeymapping.AddKeymapping(panel, text, keyBinding, tooltip);
            UIPanel child = null;
            foreach (var item in panel.components) {
                if (item is UIPanel kmPanel)
                    child = kmPanel;
            }
            Group.UITool = new UIStyleGamma(panel) { Child = child };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public UIPanel AddField<TypeField, TypeValue>(string majorText, string minorText, TypeValue defaultValue, TypeValue minLimit, TypeValue maxLimit, out UILabel majorLabel, out UILabel minorLabel, out TypeField valueField, Action<TypeValue> callback = null, float fieldWidth = 100, float fieldHeight = 28, RectOffset majorOffset = null, RectOffset minorOffset = null) where TypeField : CustomValueFieldBase<TypeValue> where TypeValue : IComparable {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                valueField = null;
                return null;
            }
            var panel = AddChildPanel();
            valueField = CustomField.AddOptionPanelValueField<TypeField, TypeValue>(panel, defaultValue, minLimit, maxLimit, callback, fieldWidth, fieldHeight);
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f, White);
            } else {
                majorLabel = null;
            }
            if (minorText is not null) {
                minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, new Color32(200, 200, 200, 255));
            } else {
                minorLabel = null;
            }
            Group.UITool = new UIStyleAlpha(panel) { Child = valueField, MajorLabel = majorLabel, MinorLabel = minorLabel, Padding = new(10, 10, 10, 10), LabelGap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public UIPanel AddButton(string majorText, string minorText, string buttonText, float? buttonWidth, float buttonHeight, OnButtonClicked callback, out UILabel majorLabel, out UILabel minorLabel, out UIButton button, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                button = null;
                return null;
            }
            var panel = AddChildPanel();
            button = CustomButton.AddGreenButton(panel, buttonText, buttonHeight, buttonWidth, callback);
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f, White);
            } else {
                majorLabel = null;
            }
            if (minorText is not null) {
                minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, new Color32(200, 200, 200, 255));
            } else {
                minorLabel = null;
            }
            Group.UITool = new UIStyleAlpha(panel) { Child = button, MajorLabel = majorLabel, MinorLabel = minorLabel, Padding = new(10, 10, 10, 10), LabelGap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public void AddSliderGamma(string majorText, string minorText, float min, float max, float step, float defaultVal, Vector2 sliderSize, PropertyChangedEventHandler<float> callback, out UILabel majorLabel, out UILabel minorLabel, out UISlider slider, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                slider = null;
                return;
            }
            var panel = AddChildPanel();
            slider = CustomSlider.AddSliderGamma(panel, sliderSize, min, max, step, defaultVal, callback);
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f, White);
            } else {
                majorLabel = null;
            }
            if (minorText is not null) {
                minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, new Color32(200, 200, 200, 255));
            } else {
                minorLabel = null;
            }
            Group.UITool = new UIStyleGamma(panel) { Child = slider, MajorLabel = majorLabel, MinorLabel = minorLabel, Padding = new(10, 10, 10, 10), Gap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
        }

        public void AddSliderAlpha(string majorText, string minorText, string sliderText, float min, float max, float step, float defaultVal, CustomSliderBase.Size sliderSize, PropertyChangedEventHandler<float> callback, out UILabel majorLabel, out UILabel minorLabel, out CustomSliderBase slider, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                slider = null;
                return;
            }
            var panel = AddChildPanel();
            slider = CustomSlider.AddCustomSliderStyleA(panel, sliderText, min, max, step, defaultVal, sliderSize, callback);
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f, White);
            } else {
                majorLabel = null;
            }
            if (minorText is not null) {
                minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, new Color32(200, 200, 200, 255));
            } else {
                minorLabel = null;
            }
            Group.UITool = new UIStyleGamma(panel) { Child = slider, MajorLabel = majorLabel, MinorLabel = minorLabel, Padding = new(10, 10, 10, 10), Gap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;

        }

        public void AddLabel(string majorText, string minorText, out UILabel majorLabel, out UILabel minorLabel, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                return;
            }
            var panel = AddChildPanel();
            majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f, White);
            if (minorText is not null) {
                minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, new Color32(200, 200, 200, 255));
                Group.UITool = new UIStyleAlpha(panel) { MajorLabel = majorLabel, MinorLabel = minorLabel, Padding = new(10, 10, 10, 10), LabelGap = 4 };
            } else {
                minorLabel = null;
                Group.UITool = new UIStyleAlpha(panel) { MajorLabel = majorLabel, Padding = new(10, 10, 10, 10), LabelGap = 4 };
            }
            Group.UITool.RefreshLayout();
            Group.UITool = null;
        }

        public UIPanel AddDropDown(string majorText, string minorText, string[] options, int defaultSelection, float dropDownWidth, float dropDownHeight, float dropDownTextScale, out UILabel majorLabel, out UILabel minorLabel, out UIDropDown dropDown, RectOffset dropDownPadding = null, RectOffset dropDownItemPadding = null, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                dropDown = null;
                return null;
            }
            var panel = AddChildPanel();
            majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f, White);
            dropDown = CustomDropDown.AddDropDown(panel, options, defaultSelection, dropDownWidth, dropDownHeight, dropDownTextScale, dropDownPadding, dropDownItemPadding);
            if (minorText is not null) {
                minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, new Color32(200, 200, 200, 255));
                Group.UITool = new UIStyleAlpha(panel) { Child = dropDown, MajorLabel = majorLabel, MinorLabel = minorLabel, Padding = new(10, 10, 10, 10), LabelGap = 4 };
            } else {
                minorLabel = null;
                Group.UITool = new UIStyleAlpha(panel) { Child = dropDown, MajorLabel = majorLabel, Padding = new(10, 10, 10, 10), LabelGap = 4 };
            }
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public UIPanel AddToggleButton(bool isChecked, string majorText, string minorText, Action<bool> callback, out UILabel majorLabel, out UILabel minorLabel, out ToggleButton button, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                button = null;
                return null;
            }
            var panel = AddChildPanel();
            button = CustomButton.AddToggleButton(panel, isChecked, callback);
            majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f, White);
            majorLabel.name = "MajorLabel";
            if (minorText is not null) {
                minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, new Color32(200, 200, 200, 255));
                Group.UITool = new UIStyleAlpha(panel) { Child = button, MajorLabel = majorLabel, MinorLabel = minorLabel, Padding = new(10, 10, 10, 10), LabelGap = 4 };
            } else {
                minorLabel = null;
                Group.UITool = new UIStyleAlpha(panel) { Child = button, MajorLabel = majorLabel, Padding = new(10, 10, 10, 10) };
            }
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        private void SetMajorSprite(UIPanel panel) {
            panel.atlas = CustomAtlas.InGameAtlas;
            panel.backgroundSprite = "ButtonWhite";
            panel.color = new(50, 60, 74, 255);
        }
        private void SetMinorSprite(UIPanel panel) {
            panel.atlas = CustomAtlas.CommonAtlas;
            panel.backgroundSprite = CustomAtlas.EmptySprite;
            panel.color = new Color32(0, 0, 0, 70);
            panel.disabledColor = new Color32(0, 0, 0, 70);
        }

        public void Reset() {
            if (Group is not null)
                Group = null;
        }
    }

}
