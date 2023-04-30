using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;
using MbyronModsCommon.UI;
namespace MbyronModsCommon;

public static class OptionPanelHelper {
    public static float PropertyPanelWidth => 732;
    public static PropertyPanel Group { get; set; }
    public static RectOffset DefaultOffset => new(16, 16, 14, 14);

    public static PropertyPanel AddGroup(UIComponent parent, string caption) {
        Group = parent.AddUIComponent<PropertyPanel>();
        Group.width = PropertyPanelWidth;
        Group.AutoLayout = true;
        Group.AutoFitChildrenVertically = true;
        Group.MajorLabelText = caption;
        Group.MajorLabelTextScale = 0.8f;
        Group.MajorLabelColor = CustomUIColor.OffWhite;
        Group.MajorLabelOffset = new(16, 0, 0, 0);
        Group.ItemGap = 4;
        Group.EventSetGroupPropertyPanelStyle += (c) => {
            c.Atlas = CustomUIAtlas.MbyronModsAtlas;
            c.BgSprite = CustomUIAtlas.RoundedRectangle5;
            c.BgNormalColor = CustomUIColor.OPPrimaryBg;
        };
        Group.EventOnSinglePropertyPanelAdded += (c, v) => {
            c.Atlas = CustomUIAtlas.MbyronModsAtlas;
            c.RenderForegroundSprite = true;
            c.FgSprite = CustomUIAtlas.LineBottom;
            c.FgSpriteMode = ForegroundSpriteMode.Custom;
            c.FgSize = new Vector2(PropertyPanelWidth - 32, 20);
            c.FgNormalColor = CustomUIColor.OPPrimaryFg;
            c.FgDisabledColor = CustomUIColor.OPPrimaryFg;
            c.VerticalAlignment = UIVerticalAlignment.Bottom;
        };
        return Group;
    }
    private static T AddChildPanel<T>(bool renderLine = true) where T : SinglePropertyPanelBase {
        var panel = Group.AddItemPanel<T>();
        panel.width = PropertyPanelWidth;
        panel.AutoFitChildrenVertically = true;
        if (!renderLine) {
            panel.RenderForegroundSprite = false;
        }
        return panel;
    }
    public static UILabel AddMinorLabel(string text) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        Group.MinorLabelText = text;
        Group.MinorLabelOffset = new(16, 0, 0, 0);
        Group.MinorLabelTextScale = 0.8f;
        Group.MinorLabelColor = CustomUIColor.OffWhite;
        return Group.MinorLabel;
    }

    public static KeyMappingSinglePropertyPanel AddKeymapping(string majorText, KeyBinding keyBinding, string tooltip = null, RectOffset majorOffset = null, string minorText = null, RectOffset minorOffset = null) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<KeyMappingSinglePropertyPanel>();
        var button = CustomUIButton.Add(panel, "Template", 280, 30, null);
        button.SetOptionPanelStyle();
        button.canFocus = true;
        panel.Child = button;
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
                if (minorOffset is not null) {
                    panel.MinorLabelOffset = minorOffset;
                }
            }
        }
        if (tooltip is not null) {
            panel.tooltip = tooltip;
        }
        panel.Binding = keyBinding;
        panel.Padding = DefaultOffset;
        panel.StartLayout();
        return panel;
    }
    public static GammaSinglePropertyPanel AddStringField(string majorText, string text, string minorText, float width = 700, float height = 30, RectOffset majorOffset = null, RectOffset minorOffset = null) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<GammaSinglePropertyPanel>();
        var textField = panel.AddUIComponent<UIStringField>();
        textField.SetOptionPanelStyle();
        textField.size = new Vector2(width, height);
        textField.TextPadding = new RectOffset(0, 0, 6, 0);
        textField.Text = text;
        panel.Child = textField;
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
                if (minorOffset is not null) {
                    panel.MinorLabelOffset = minorOffset;
                }
            }
        }
        panel.Padding = DefaultOffset;
        panel.Gap = 4;
        panel.StartLayout();
        return panel;
    }
    public static AlphaSinglePropertyPanel AddField<TypeField, TypeValue>(string majorText, string minorText, TypeValue defaultValue, TypeValue minLimit, TypeValue maxLimit, Action<TypeValue> callback = null, float fieldWidth = 100, float fieldHeight = 28, RectOffset majorOffset = null, RectOffset minorOffset = null) where TypeField : CustomUIValueFieldBase<TypeValue> where TypeValue : IComparable<TypeValue> {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<AlphaSinglePropertyPanel>();
        var field = UIValueFieldHelper.AddOptionPanelValueField<TypeField, TypeValue>(panel, defaultValue, minLimit, maxLimit, callback, fieldWidth, fieldHeight);
        field.SetOptionPanelStyle();
        panel.Child = field;
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
                if (minorOffset is not null) {
                    panel.MinorLabelOffset = minorOffset;
                }
            }
        }
        panel.Padding = DefaultOffset;
        panel.LabelGap = 4;
        panel.StartLayout();
        return panel;
    }
    public static AlphaSinglePropertyPanel AddButton(string majorText, string minorText, string buttonText, float? buttonWidth, float buttonHeight, OnButtonClicked callback, RectOffset majorOffset = null, RectOffset minorOffset = null) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<AlphaSinglePropertyPanel>();
        panel.Child = CustomUIButton.Add(panel, buttonText, buttonWidth, buttonHeight, callback);
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
                if (minorOffset is not null) {
                    panel.MinorLabelOffset = minorOffset;
                }
            }
        }
        panel.Padding = DefaultOffset;
        panel.LabelGap = 4;
        panel.StartLayout();
        return panel;
    }
    public static GammaSinglePropertyPanel AddSlider(string majorText, string minorText, float min, float max, float step, float rawValue, Vector2 sliderSize, Action<float> callback, RectOffset majorOffset = null, RectOffset minorOffset = null, bool gradientStyle = false) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<GammaSinglePropertyPanel>();
        var slider = CustomUISlider.Add(panel, sliderSize, min, max, step, rawValue, callback, !gradientStyle);
        panel.Child = slider;
        if (gradientStyle) {
            slider.SetGradientStyle();
        }
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
                if (minorOffset is not null) {
                    panel.MinorLabelOffset = minorOffset;
                }
            }
        }
        panel.Padding = DefaultOffset;
        panel.Gap = 8;
        panel.StartLayout();
        return panel;
    }
    public static AlphaSinglePropertyPanel AddLabel(string majorText, string minorText, RectOffset majorOffset = null, RectOffset minorOffset = null) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<AlphaSinglePropertyPanel>();
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                var minorLabel = panel.AddUIComponent<CustomUILabel>();
                minorLabel.AutoHeight = true;
                minorLabel.WordWrap = false;
                minorLabel.Text = minorText;
                if (minorOffset is not null) {
                    minorLabel.TextPadding = minorOffset;
                }
                panel.Child = minorLabel;
            }
        }
        panel.Padding = DefaultOffset;
        panel.LabelGap = 8;
        panel.StartLayout();
        return panel;
    }
    public static AlphaSinglePropertyPanel AddDropDown(string majorText, string minorText, string[] options, int defaultSelection, float dropDownWidth, float dropDownHeight, Action<int> callback = null, RectOffset majorOffset = null, RectOffset minorOffset = null) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<AlphaSinglePropertyPanel>();
        panel.Child = CustomUIDropDown.AddOPDropDown(panel, new Vector2(dropDownWidth, dropDownHeight), options, defaultSelection, callback);
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
                if (minorOffset is not null) {
                    panel.MinorLabelOffset = minorOffset;
                }
            }
        }
        panel.Padding = DefaultOffset;
        panel.LabelGap = 4;
        panel.StartLayout();
        return panel;
    }
    public static DeltaSinglePropertyPanel AddCheckBox(string majorText, string minorText, bool isChecked, Action<bool> callback, RectOffset Padding = null, bool renderLine = true) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<DeltaSinglePropertyPanel>(renderLine);
        panel.Child = CustomUICheckBox.Add(panel, isChecked, callback);
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
            }
        }
        if (Padding != null)
            panel.Padding = Padding;
        panel.StartLayout();
        return panel;
    }
    public static AlphaSinglePropertyPanel AddToggle(bool isOn, string majorText, string minorText, Action<bool> callback, RectOffset majorOffset = null, RectOffset minorOffset = null) {
        if (Group is null) {
            ExternalLogger.Error("OptionPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<AlphaSinglePropertyPanel>();
        panel.Child = CustomUIToggleButton.Add(panel, isOn, callback);
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.8f;
                if (minorOffset is not null) {
                    panel.MinorLabelOffset = minorOffset;
                }
            }
        }
        panel.Padding = DefaultOffset;
        panel.LabelGap = 4;
        panel.StartLayout();
        return panel;
    }
    public static void Reset() {
        if (Group is not null) {
            Group.ItemPanels[Group.ItemPanels.Count - 1].RenderForegroundSprite = false;
            Group = null;
        }
    }
}


