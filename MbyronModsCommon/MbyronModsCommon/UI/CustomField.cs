using ColossalFramework.UI;
using System.ComponentModel;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomField {
        public static TypeField AddOptionPanelValueField<TypeField, TypeValue>(UIComponent parent, TypeValue defaultValue, TypeValue minLimit, TypeValue maxLimit, Action<TypeValue> callback = null, float fieldWidth = 100, float fieldHeight = 28) where TypeField : CustomValueFieldBase<TypeValue> where TypeValue : IComparable {
            var valueField = parent.AddUIComponent<TypeField>();
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
            valueField.atlas = CustomAtlas.MbyronModsAtlas;
            valueField.normalBgSprite = CustomAtlas.TabButtonNormal;
            valueField.hoveredBgSprite = CustomAtlas.TabButtonNormal;
            valueField.disabledBgSprite = CustomAtlas.TabButtonDisabled;
            valueField.selectionSprite = CustomAtlas.EmptySprite;
            valueField.padding = new RectOffset(6, 6, 6, 6);
            valueField.textScale = 1f;
            valueField.MinValue = minLimit;
            valueField.MaxValue = maxLimit;
            valueField.Value = defaultValue;
            valueField.OnValueChanged += (v) => callback?.Invoke(v);
            return valueField;
        }

        public static UITextField AddTextField(UIComponent parent, string text, float width, float height, float textScale = 1f, Action<UITextField> setSprite = null) {
            UITextField uiTextField = parent.AddUIComponent<UITextField>();
            uiTextField.width = width;
            uiTextField.height = height;
            if (setSprite is not null) {
                setSprite(uiTextField);
            } else {
                uiTextField.atlas = CustomAtlas.MbyronModsAtlas;
                uiTextField.normalBgSprite = CustomAtlas.TabButtonNormal;
                uiTextField.hoveredBgSprite = CustomAtlas.TabButtonNormal;
                uiTextField.selectionSprite = CustomAtlas.EmptySprite;
            }
            uiTextField.padding = new RectOffset(8, 6, 8, 6);
            uiTextField.textScale = 1f;
            uiTextField.text = text;
            return uiTextField;
        }

        public static TypeValueField AddField<TypeValueField, TypeValue>(UIComponent parent, float width, float height, TypeValue defaultValue, TypeValue wheelStep, TypeValue minLimit, TypeValue maxLimit, Action<TypeValue> callback, bool useWheel = true, bool showTooltip = true) where TypeValueField : CustomValueFieldBase<TypeValue> where TypeValue : IComparable {
            TypeValueField valueField = parent.AddUIComponent<TypeValueField>();
            valueField.width = width;
            valueField.height = height;
            valueField.SetStyle();
            valueField.MinValue = minLimit;
            valueField.MaxValue = maxLimit;
            valueField.Value = defaultValue;
            valueField.UseWheel = useWheel;
            valueField.WheelStep = wheelStep;
            valueField.ShowTooltip = showTooltip;
            valueField.OnValueChanged += (v) => callback?.Invoke(v);
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
        private bool showTooltip = false;
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
        public bool ShowTooltip {
            get => showTooltip;
            set {
                showTooltip = value;
                if (value)
                    tooltip = CommonLocalize.ScrollWheel;
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
            atlas = CustomAtlas.MbyronModsAtlas;
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
