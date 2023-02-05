using ColossalFramework.UI;
using System.ComponentModel;
using System;
using UnityEngine;
using ColossalFramework;

namespace MbyronModsCommon {
    public class CustomField {

        public static UIPanel AddOptionPanelValueField<TypeField, TypeValue>(UIComponent parent, string textLabel, TypeValue defaultValue, TypeValue minLimit, TypeValue maxLimit, out UILabel uiLabel, out TypeField valueField, float fieldWidth = 704, float fieldHeight = 32) where TypeField : CustomValueFieldBase<TypeValue> where TypeValue : IComparable {
            var panel = parent.AddUIComponent<UIPanel>();
            panel.autoLayout = true;
            panel.autoLayoutDirection = LayoutDirection.Vertical;
            panel.autoFitChildrenHorizontally = true;
            panel.autoFitChildrenVertically = true;
            uiLabel = panel.AddUIComponent<UILabel>();
            if (textLabel.IsNullOrWhiteSpace()) {
                uiLabel.Hide();
            } else {
                uiLabel.autoSize = false;
                uiLabel.autoHeight = true;
                uiLabel.wordWrap = true;
                uiLabel.width = 650;
                uiLabel.textScale = 1f;
                uiLabel.disabledTextColor = new Color32(71, 71, 71, 255);
                uiLabel.text = textLabel;
            }
            valueField = panel.AddUIComponent<TypeField>();
            valueField.horizontalAlignment = UIHorizontalAlignment.Left;
            valueField.numericalOnly = true;
            valueField.allowFloats = true;
            valueField.enabled = true;
            valueField.builtinKeyNavigation = true;
            valueField.isInteractive = true;
            valueField.readOnly = false;
            valueField.cursorWidth = 1;
            valueField.cursorBlinkTime = 0.45f;
            valueField.padding = new RectOffset(0, 0, 5, 0);
            valueField.UseWheel = false;
            valueField.width = fieldWidth;
            valueField.height = fieldHeight;
            valueField.disabledTextColor = new Color32(71, 71, 71, 255);
            valueField.atlas = CustomAtlas.CommonAtlas;
            valueField.normalBgSprite = CustomAtlas.TabButtonNormal;
            valueField.hoveredBgSprite = CustomAtlas.TabButtonNormal;
            valueField.disabledBgSprite = CustomAtlas.TabButtonDisabled;
            valueField.selectionSprite = CustomAtlas.EmptySprite;
            valueField.padding = new RectOffset(6, 6, 6, 6);
            valueField.textScale = 1f;
            valueField.MinValue = minLimit;
            valueField.MaxValue = maxLimit;
            valueField.Value = defaultValue;
            return panel;
        }

        public static UIPanel AddOptionPanelStringField(UIComponent parent, string textLabel, string text, out UILabel uiLabel, out UITextField uiTextField, float fieldWidth = 704, float fieldHeight = 32) {
            var panel = parent.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsTextfieldTemplate")) as UIPanel;
            panel.autoFitChildrenVertically = true;
            uiLabel = panel.Find<UILabel>("Label");
            if (textLabel.IsNullOrWhiteSpace()) {
                uiLabel.Hide();
            } else {
                uiLabel.autoSize = false;
                uiLabel.autoHeight = true;
                uiLabel.wordWrap = true;
                uiLabel.textScale = 1f;
                uiLabel.text = textLabel;
            }
            uiTextField = panel.Find<UITextField>("Text Field");
            uiTextField.width = fieldWidth;
            uiTextField.height = fieldHeight;
            uiTextField.atlas = CustomAtlas.CommonAtlas;
            uiTextField.normalBgSprite = CustomAtlas.TabButtonNormal;
            uiTextField.hoveredBgSprite = CustomAtlas.TabButtonNormal;
            uiTextField.selectionSprite = CustomAtlas.EmptySprite;
            uiTextField.padding = new RectOffset(8, 6, 8, 6);
            uiTextField.textScale = 1f;
            uiTextField.text = text;
            return panel;
        }

        public static CustomLongValueField AddLongValueField(UIComponent parent, float width, float height, long defaultValue, long wheelStep, long minLimit, long maxLimit, int round = 1, bool useWheel = true) => AddField<CustomLongValueField, long>(parent, width, height, defaultValue, wheelStep, minLimit, maxLimit, useWheel);

        public static CustomFloatValueField AddFloatValueField(UIComponent parent, float width, float height, float defaultValue, int wheelStep, int minLimit, int maxLimit, int round = 1, bool useWheel = true) {
            var floatValueField = AddField<CustomFloatValueField, float>(parent, width, height, defaultValue, wheelStep, minLimit, maxLimit, useWheel);
            floatValueField.Round = round;
            return floatValueField;
        }

        public static CustomIntValueField AddIntValueField(UIComponent parent, float width, float height, int defaultValue, int wheelStep, int minLimit, int maxLimit, bool useWheel = true) => AddField<CustomIntValueField, int>(parent, width, height, defaultValue, wheelStep, minLimit, maxLimit, useWheel);

        public static TypeValueField AddField<TypeValueField, TypeValue>(UIComponent parent, float width, float height, TypeValue defaultValue, TypeValue wheelStep, TypeValue minLimit, TypeValue maxLimit, bool useWheel) where TypeValueField : CustomValueFieldBase<TypeValue> where TypeValue : IComparable {
            TypeValueField valueField = parent.AddUIComponent<TypeValueField>();
            valueField.width = width;
            valueField.height = height;
            valueField.SetStyle();
            valueField.MinValue = minLimit;
            valueField.MaxValue = maxLimit;
            valueField.Value = defaultValue;
            valueField.UseWheel = useWheel;
            valueField.WheelStep = wheelStep;
            return valueField;
        }
    }

    public class CustomFloatValueField : CustomValueFieldBase<float> {
        protected override float ValueDecrease(SteppingRate steppingRate) {
            var rate = GetStep(steppingRate);
            return (float)Math.Round(Value - rate, round);
        }
        protected override float ValueIncrease(SteppingRate steppingRate) {
            var rate = GetStep(steppingRate);
            return (float)Math.Round(Value + rate, round);
        }

        protected override float GetStep(SteppingRate steppingRate) => steppingRate switch {
            SteppingRate.Fast => WheelStep * 10,
            SteppingRate.Slow => WheelStep / 10,
            _ => WheelStep,
        };

    }

    public class CustomLongValueField : CustomValueFieldBase<long> {
        protected override long GetStep(SteppingRate steppingRate) => steppingRate switch {
            SteppingRate.Fast => WheelStep * 10,
            SteppingRate.Slow => WheelStep / 10,
            _ => WheelStep,
        };

        protected override long ValueDecrease(SteppingRate steppingRate) {
            var rate = GetStep(steppingRate);
            return Value - rate;
        }
        protected override long ValueIncrease(SteppingRate steppingRate) {
            var rate = GetStep(steppingRate);
            return Value + rate;
        }
    }

    public class CustomIntValueField : CustomValueFieldBase<int> {
        protected override int ValueDecrease(SteppingRate steppingRate) {
            var rate = GetStep(steppingRate);
            return Value - rate;
        }
        protected override int ValueIncrease(SteppingRate steppingRate) {
            var rate = GetStep(steppingRate);
            return Value + rate;
        }

        protected override int GetStep(SteppingRate steppingRate) => steppingRate switch {
            SteppingRate.Fast => WheelStep * 10,
            SteppingRate.Slow => WheelStep / 10,
            _ => WheelStep,
        };

    }

    public abstract class CustomValueFieldBase<TypeValue> : UITextField where TypeValue : IComparable {
        private TypeValue value;
        private string format = "{0}";
        protected int round = 1;
        protected bool useWheel = true;
        public TypeValue Value {
            get => value;
            set => ValueChanged(value);
        }
        public string Format {
            get => format;
            set {
                format = value;
                RefreshText();
            }
        }
        public int Round {
            get => round;
            set => round = value;
        }
        public bool UseWheel {
            get => useWheel;
            set {
                if (value != useWheel) {
                    useWheel = value;
                }
            }
        }
        public virtual TypeValue MinValue { get; set; }
        public virtual TypeValue MaxValue { get; set; }
        protected bool WheelAvailable { get; set; }
        public TypeValue WheelStep { get; set; }

        public event Action<TypeValue> OnValueChanged;
        public virtual void ValueChanged(TypeValue _value) {
            if (_value.CompareTo(MinValue) < 0) {
                value = MinValue;
            } else if (_value.CompareTo(MaxValue) > 0) {
                value = MaxValue;
            } else {
                value = _value;
            }
            OnValueChanged?.Invoke(value);
            RefreshText();
        }

        protected virtual void RefreshText() {
            if (hasFocus) {
                if (value != null) {
                    text = string.Format(format, value.ToString());
                } else {
                    text = string.Empty;
                }
            } else {
                text = string.Format(format, value?.ToString() ?? string.Empty);
            }
        }
        protected abstract TypeValue ValueDecrease(SteppingRate steppingRate);
        protected abstract TypeValue ValueIncrease(SteppingRate steppingRate);
        protected abstract TypeValue GetStep(SteppingRate steppingRate);

        private SteppingRate GetSteppingRate() {
            if (KeyHelper.IsShiftDown()) return SteppingRate.Fast;
            else if (KeyHelper.IsControlDown()) return SteppingRate.Slow;
            else return SteppingRate.Normal;
        }

        protected override void OnSubmit() {
            var force = hasFocus;
            base.OnSubmit();

            if (!force && text == (Convert.ToString(Value) ?? string.Empty)) {
                RefreshText();
                return;
            }

            var newValue = default(TypeValue);
            try {
                if (typeof(TypeValue) == typeof(string))
                    newValue = (TypeValue)(object)text;
                else if (!string.IsNullOrEmpty(text))
                    newValue = (TypeValue)TypeDescriptor.GetConverter(typeof(TypeValue)).ConvertFromString(text);
            }
            catch { }

            ValueChanged(newValue);
        }
        protected override void OnMouseWheel(UIMouseEventParameter p) {
            base.OnMouseWheel(p);
            tooltipBox.Hide();
            if (UseWheel) {
                var typeRate = GetSteppingRate();
                if (p.wheelDelta < 0) {
                    ValueChanged(ValueDecrease(typeRate));
                } else {
                    ValueChanged(ValueIncrease(typeRate));
                }
            }
        }
        protected override void OnMouseLeave(UIMouseEventParameter p) {
            base.OnMouseLeave(p);
            WheelAvailable = false;
        }
        protected override void OnMouseMove(UIMouseEventParameter p) {
            base.OnMouseMove(p);
            WheelAvailable = true;
        }

        public void SetStyle() {
            atlas = CustomAtlas.CommonAtlas;
            selectionSprite = CustomAtlas.EmptySprite;
            normalBgSprite = CustomAtlas.FieldNormal;
            disabledBgSprite = CustomAtlas.FieldDisabled;
            focusedBgSprite = CustomAtlas.FieldNormal;
            hoveredBgSprite = CustomAtlas.FieldHovered;

            numericalOnly = true;
            allowFloats = true;
            enabled = true;
            builtinKeyNavigation = true;
            isInteractive = true;
            readOnly = false;
            cursorWidth = 1;
            cursorBlinkTime = 0.45f;
            textScale = 0.7f;
            selectOnFocus = true;
            verticalAlignment = UIVerticalAlignment.Middle;
            padding = new RectOffset(0, 0, 5, 0);
        }

    }

    public enum SteppingRate {
        Normal,
        Fast,
        Slow
    }
}
