using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class ControlPanelTools {
        private static ControlPanelTools instance;

        public static ControlPanelTools Instance {
            get => instance ??= new ControlPanelTools();
        }
        public CustomListBox Group { get; set; }

        private ControlPanelTools() { }

        public void AddGroup(UIComponent parent, float width, string caption, RectOffset captionRectOffset, float textScale, Color32 captionColor, RectOffset groupPanelPadding) {
            Group = parent.AddUIComponent<CustomListBox>();
            Group.width = width;
            Group.Init(width, caption, captionRectOffset, textScale, captionColor, groupPanelPadding, null);
        }
        public void AddLabelWithSliderGamma(string text, Vector2 siderSize, float min, float max, float step, float defaultValue, PropertyChangedEventHandler<float> callback, out UILabel label, out UISlider slider) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null");
                label = null;
                slider = null;
                return;
            }
            var panel = Group.AddChildPanel();
            label = CustomLabel.AddLabel(panel, text, 10, new(), 0.8f, Color.white);
            slider = CustomSlider.AddSliderGamma(panel, siderSize, min, max, step, defaultValue, callback);
            Group.UITool = new UIStyleGamma(panel) { MajorLabel = label, Child = slider, Padding = new(6, 6, 6, 6)};
            Group.UITool.RefreshLayout();
            Group.UITool = null;
        }

        public void AddLabelWithPairButton(string text, string leftButtonText, string rightButtonText, bool defaultValue, Action<int> callback, out UILabel label, out PairButton pairButton, float buttonWidth = 140, float buttonHeight = 20) {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null");
                label = null;
                pairButton = null;
                return;
            }
            var panel = Group.AddChildPanel();
            label = CustomLabel.AddLabel(panel, text, 10, new RectOffset(), 0.8f, Color.white);
            pairButton = CustomButton.AddPairButton(panel, leftButtonText, rightButtonText, defaultValue, buttonWidth, buttonHeight, callback);
            Group.UITool = new UIStyleAlpha(panel) { MajorLabel = label, Child = pairButton, Padding = new RectOffset(6, 6, 6, 6) };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
        }

        public void AddLabelWithField<TypeValueField, TypeValue>(string text, float width, float height, TypeValue defaultValue, TypeValue wheelStep, TypeValue minLimit, TypeValue maxLimit, bool useWheel, out UILabel label, out TypeValueField typeValueField) where TypeValueField : CustomValueFieldBase<TypeValue> where TypeValue : IComparable {
            if (Group is null) {
                ModLogger.ModLog("ControlPanelTools_Group is null");
                label = null;
                typeValueField = null;
                return;
            }
            var panel = Group.AddChildPanel();
            label = CustomLabel.AddLabel(panel, text, 10, new RectOffset(), 0.8f, Color.white);
            typeValueField = CustomField.AddField<TypeValueField, TypeValue>(panel, width, height, defaultValue, wheelStep, minLimit, maxLimit, useWheel);
            typeValueField.ShowTooltip = true;
            Group.UITool = new UIStyleAlpha(panel) { MajorLabel = label, Child = typeValueField, Padding = new RectOffset(6, 6, 6, 6) };
            Group.UITool.RefreshLayout();
            Group.UITool = null;
        }

        public void Reset() {
            if (Group is not null)
                Group = null;
        }
    }

}
