using MbyronModsCommon.UI;
using ColossalFramework.UI;
using System;
using UnityEngine;
namespace MbyronModsCommon;

public static class ControlPanelHelper {
    public static float PropertyPanelWidth { get; set; }
    public static PropertyPanel Group { get; set; }
    public static RectOffset DefaultOffset => new(10, 10, 10, 10);

    public static PropertyPanel AddGroup(UIComponent parent, float width, string caption) {
        Group = parent.AddUIComponent<PropertyPanel>();
        Group.width = PropertyPanelWidth = width;
        Group.AutoLayout = true;
        Group.AutoFitChildrenVertically = true;
        Group.MajorLabelText = caption;
        Group.MajorLabelTextScale = 0.8f;
        Group.MajorLabelColor = CustomUIColor.OffWhite;
        Group.MajorLabelOffset = new(10, 0, 0, 0);
        Group.ItemGap = 4;
        Group.EventSetGroupPropertyPanelStyle += (c) => {
            c.Atlas = CustomUIAtlas.MbyronModsAtlas;
            c.BgSprite = CustomUIAtlas.RoundedRectangle2;
            c.BgNormalColor = CustomUIColor.CPPrimaryBg;
        };
        return Group;
    }
    private static T AddChildPanel<T>() where T : SinglePropertyPanelBase {
        var panel = Group.AddItemPanel<T>();
        panel.width = PropertyPanelWidth;
        panel.AutoFitChildrenVertically = true;
        panel.Atlas = CustomUIAtlas.MbyronModsAtlas;
        panel.FgSprite = CustomUIAtlas.LineBottom;
        panel.FgSpriteMode = ForegroundSpriteMode.Custom;
        panel.FgSize = new(panel.width - 20, 20);
        panel.FgDisabledColor = panel.FgNormalColor = CustomUIColor.CPPrimaryFg;
        panel.VerticalAlignment = UIVerticalAlignment.Bottom;
        panel.RenderForegroundSprite = true;
        return panel;
    }
    public static GammaSinglePropertyPanel AddSlider(string majorText, string minorText, float min, float max, float step, float rawValue, Vector2 sliderSize, Action<float> callback, RectOffset majorOffset = null, RectOffset minorOffset = null, bool gradientStyle = false) {
        if (Group is null) {
            ExternalLogger.Error("ControlPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<GammaSinglePropertyPanel>();
        var slider = CustomUISlider.Add(panel, sliderSize, min, max, step, rawValue, callback);
        panel.Child = slider;
        if (gradientStyle) {
            slider.SetGradientStyle();
        } else {
            slider.SetCPDefaultStyle();
        }
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            panel.MajorLabelTextScale = 0.8f;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.7f;
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
    //public static AlphaSinglePropertyPanel AddDropDown(string majorText, string minorText, string[] options, int defaultSelection, float dropDownWidth, float dropDownHeight = 20, OnDropdownSelectionChanged eventCallback = null, RectOffset majorOffset = null, RectOffset minorOffset = null) {
    //    if (Group is null) {
    //        ExternalLogger.Error("ControlPanelHelper_Group is null.");
    //        return null;
    //    }
    //    var panel = AddChildPanel<AlphaSinglePropertyPanel>();
    //    panel.Child = CustomDropDown.AddCPDropDown(panel, options, defaultSelection, dropDownWidth, dropDownHeight, eventCallback);
    //    if (majorText is not null) {
    //        panel.MajorLabelText = majorText;
    //        panel.MajorLabelTextScale = 0.8f;
    //        if (majorOffset is not null) {
    //            panel.MajorLabelOffset = majorOffset;
    //        }
    //        if (minorText is not null) {
    //            panel.MinorLabelText = minorText;
    //            panel.MinorLabelTextScale = 0.7f;
    //            if (minorOffset is not null) {
    //                panel.MinorLabelOffset = minorOffset;
    //            }
    //        }
    //    }
    //    panel.Padding = DefaultOffset;
    //    panel.LabelGap = 4;
    //    panel.StartLayout();
    //    return panel;
    //}

    public static AlphaSinglePropertyPanel AddToggle(bool isOn, string majorText, string minorText, Action<bool> callback, RectOffset majorOffset = null, RectOffset minorOffset = null) {
        if (Group is null) {
            ExternalLogger.Error("ControlPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<AlphaSinglePropertyPanel>();
        panel.Child = CustomUIToggleButton.Add(panel, isOn, callback, false);
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            panel.MajorLabelTextScale = 0.8f;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.7f;
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

    public static AlphaSinglePropertyPanel AddField<T, V>(string majorText, string minorText, float width, V defaultValue, V wheelStep, V minLimit, V maxLimit, Action<V> callback, RectOffset majorOffset = null, RectOffset minorOffset = null) where T : CustomUIValueFieldBase<V> where V : IComparable<V> {
        if (Group is null) {
            ExternalLogger.Error("ControlPanelHelper_Group is null.");
            return null;
        }
        var panel = AddChildPanel<AlphaSinglePropertyPanel>();
        var field = UIValueFieldHelper.AddOptionPanelValueField<T, V>(panel, defaultValue, minLimit, maxLimit, callback, width, 24);
        field.TextScale = 0.8f;
        field.SetControlPanelStyle();
        field.WheelStep = wheelStep;
        field.CanWheel = true;
        field.ShowTooltip = true;
        field.SelectOnFocus = true;
        field.CursorHeight = 14;
        field.CustomCursorHeight = true;
        panel.Child = field;
        if (majorText is not null) {
            panel.MajorLabelText = majorText;
            panel.MajorLabelTextScale = 0.8f;
            if (majorOffset is not null) {
                panel.MajorLabelOffset = majorOffset;
            }
            if (minorText is not null) {
                panel.MinorLabelText = minorText;
                panel.MinorLabelTextScale = 0.7f;
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
