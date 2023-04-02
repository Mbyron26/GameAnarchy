using ColossalFramework.UI;
using ICities;
using MbyronModsCommon.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon {
    public static class OptionPanelTool {
        public static PropertyPanel Group { get; set; }
        public static RectOffset DefaultOffset => new(10, 10, 10, 10);
        static OptionPanelTool() => InternalLogger.Log("Initialize option panel tool.");

        public static PropertyPanel AddGroup(UIComponent parent, float width, string caption) {
            Group = parent.AddUIComponent<PropertyPanel>();
            Group.width = width;
            Group.Init(width, caption, new(10, 0, 0, 0), 0.8f, CustomColor.OffWhite, new(0, 0, 0, 4), SetMajorSprite, null);
            return Group;
        }

        private static UIPanel AddChildPanel() => Group.AddChildPanel(true/*SetMinorSprite*/);

        public static UILabel AddMinorLabel(string text) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                return null;
            }
            return Group.AddMinorLabel(text, new(10, 0, 0, 0), 0.8f, CustomColor.OffWhite);
        }

        public static UIPanel AddKeymapping(string text, KeyBinding keyBinding, string tooltip = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
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

        public static UIPanel AddCheckBox(string majorText, string minorText, bool isChecked, Action<bool> callback, out UILabel majorLabel, out UILabel minorLabel, out CheckBox checkBox, RectOffset allOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                checkBox = null;
                return null;
            }
            var panel = AddChildPanel();
            checkBox = CustomButton.AddCheckBox(panel, isChecked, callback);
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, textScale: 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, textScale: 0.8f, textColor: CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleDelta(panel, checkBox, majorLabel, minorLabel, allOffset);
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddStringField(string majorText, string stringField, string minorText, out UILabel majorLabel, out UILabel minorLabel, out UITextField textField, float width = 724, float height = 30, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                textField = null;
                majorLabel = null;
                minorLabel = null;
                return null;
            }
            var panel = AddChildPanel();
            textField = CustomField.AddTextField(panel, stringField, width, height);
            textField.horizontalAlignment = UIHorizontalAlignment.Left;
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleGamma(panel, textField, majorLabel, minorLabel, DefaultOffset) { Gap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddField<TypeField, TypeValue>(string majorText, string minorText, TypeValue defaultValue, TypeValue minLimit, TypeValue maxLimit, out UILabel majorLabel, out UILabel minorLabel, out TypeField valueField, Action<TypeValue> callback = null, float fieldWidth = 100, float fieldHeight = 28, RectOffset majorOffset = null, RectOffset minorOffset = null) where TypeField : CustomValueFieldBase<TypeValue> where TypeValue : IComparable {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                valueField = null;
                majorLabel = null;
                minorLabel = null;
                return null;
            }
            var panel = AddChildPanel();
            valueField = CustomField.AddOptionPanelValueField<TypeField, TypeValue>(panel, defaultValue, minLimit, maxLimit, callback, fieldWidth, fieldHeight);
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleAlpha(panel, valueField, majorLabel, minorLabel, DefaultOffset) { LabelGap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddButton(string majorText, string minorText, string buttonText, float? buttonWidth, float buttonHeight, OnButtonClicked callback, out UILabel majorLabel, out UILabel minorLabel, out UIButton button, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                button = null;
                majorLabel = null;
                minorLabel = null;
                return null;
            }
            var panel = AddChildPanel();
            button = CustomButton.AddClickButton(panel, buttonText, buttonWidth, buttonHeight, callback);
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleAlpha(panel, button, majorLabel, minorLabel, DefaultOffset) { LabelGap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddSliderGamma(string majorText, string minorText, float min, float max, float step, float defaultVal, Vector2 sliderSize, PropertyChangedEventHandler<float> callback, out UILabel majorLabel, out UILabel minorLabel, out UISlider slider, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                slider = null;
                return null;
            }
            var panel = AddChildPanel();
            slider = CustomSlider.AddSliderGamma(panel, sliderSize, min, max, step, defaultVal, callback);
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleGamma(panel, slider, majorLabel, minorLabel, DefaultOffset) { Gap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddSliderAlpha(string majorText, string minorText, string sliderText, float min, float max, float step, float defaultVal, SliderAlphaSize sliderSize, PropertyChangedEventHandler<float> callback, out UILabel majorLabel, out UILabel minorLabel, out SliderAlpha slider, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                slider = null;
                return null;
            }
            var panel = AddChildPanel();
            slider = CustomSlider.AddSliderAlpha(panel, sliderText, min, max, step, defaultVal, sliderSize, callback, min.ToString(), max.ToString(), new(0, 0, 4, 0));
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleGamma(panel, slider, majorLabel, minorLabel, DefaultOffset) { Gap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddLabel(string majorText, string minorText, out UILabel majorLabel, out UILabel minorLabel, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                return null;
            }
            var panel = AddChildPanel();
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, null, minorOffset, 1f, wordWrap: false);
                }
            }
            Group.UITool = new UIStyleAlpha(panel, minorLabel, majorLabel, null, DefaultOffset) { LabelGap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddDropDown(string majorText, string minorText, string[] options, int defaultSelection, float dropDownWidth, float dropDownHeight, out UILabel majorLabel, out UILabel minorLabel, out UIDropDown dropDown, OnDropdownSelectionChanged eventCallback = null, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                dropDown = null;
                return null;
            }
            var panel = AddChildPanel();
            dropDown = CustomDropDown.AddOPDropDown(panel, options, defaultSelection, dropDownWidth, dropDownHeight, eventCallback);
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleAlpha(panel, dropDown, majorLabel, minorLabel, DefaultOffset) { LabelGap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        public static UIPanel AddToggleButton(bool isChecked, string majorText, string minorText, Action<bool> callback, out UILabel majorLabel, out UILabel minorLabel, out ToggleButton button, RectOffset majorOffset = null, RectOffset minorOffset = null) {
            if (Group is null) {
                ExternalLogger.Error("ControlPanelTools_Group is null.");
                majorLabel = null;
                minorLabel = null;
                button = null;
                return null;
            }
            var panel = AddChildPanel();
            button = CustomButton.AddToggleButton(panel, isChecked, callback);
            majorLabel = null;
            minorLabel = null;
            if (majorText is not null) {
                majorLabel = CustomLabel.AddLabel(panel, majorText, 10, majorOffset, 1f);
                if (minorText is not null) {
                    minorLabel = CustomLabel.AddLabel(panel, minorText, 10, minorOffset, 0.8f, CustomColor.OffWhite);
                }
            }
            Group.UITool = new UIStyleAlpha(panel, button, majorLabel, minorLabel, DefaultOffset) { LabelGap = 4 };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
            return panel;
        }

        private static void SetMajorSprite(UIPanel panel) {
            panel.atlas = CustomAtlas.InGameAtlas;
            panel.backgroundSprite = "ButtonWhite";
            panel.color = new(50, 60, 74, 255);
        }
        private static void SetMinorSprite(UIPanel panel) {
            panel.atlas = CustomAtlas.CommonAtlas;
            panel.backgroundSprite = CustomAtlas.EmptySprite;
            panel.color = new Color32(0, 0, 0, 70);
            panel.disabledColor = new Color32(0, 0, 0, 70);
        }

        public static void Reset() {
            if (Group is not null)
                Group = null;
        }
    }

}
