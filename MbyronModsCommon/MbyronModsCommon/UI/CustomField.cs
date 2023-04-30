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
            valueField.atlas = CustomUIAtlas.MbyronModsAtlas;
            valueField.normalBgSprite = CustomUIAtlas.RoundedRectangle3;
            valueField.hoveredBgSprite = CustomUIAtlas.RoundedRectangle3;
            valueField.disabledBgSprite = CustomUIAtlas.RoundedRectangle3;
            valueField.selectionSprite = CustomUIAtlas.EmptySprite;
            valueField.color = CustomUIColor.OPButtonNormal;
            valueField.HoveredColor = CustomUIColor.OPButtonHovered;
            valueField.FocusedColor = CustomUIColor.OPButtonFocused;
            valueField.PressedColor = CustomUIColor.OPButtonPressed;
            valueField.disabledColor = CustomUIColor.OPButtonDisabled;
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
                uiTextField.atlas = CustomUIAtlas.MbyronModsAtlas;
                uiTextField.normalBgSprite = CustomUIAtlas.RoundedRectangle2;
                uiTextField.hoveredBgSprite = CustomUIAtlas.RoundedRectangle2;
                uiTextField.selectionSprite = CustomUIAtlas.EmptySprite;
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
            if (isEnabled) {
                if (containsFocus) {
                    m_State = UIButton.ButtonState.Focused;
                } else {
                    m_State = UIButton.ButtonState.Normal;
                }
            }
            base.OnMouseLeave(p);
            WheelAvailable = false;
        }
        protected override void OnMouseMove(UIMouseEventParameter p) {
            base.OnMouseMove(p);
            WheelAvailable = true;
        }

        protected UIButton.ButtonState m_State;
        public UIButton.ButtonState State {
            get {
                return m_State;
            }
            set {
                if (value != m_State) {
                    OnButtonStateChanged(value);
                    Invalidate();
                }
            }
        }

        protected Color32 m_HoveredColor = Color.white;
        protected Color32 m_PressedColor = Color.white;
        protected Color32 m_FocusedColor = Color.white;

        public Color32 HoveredColor {
            get => m_HoveredColor;
            set {
                m_HoveredColor = value;
                Invalidate();
            }
        }

        public Color32 PressedColor {
            get => m_PressedColor;
            set {
                m_PressedColor = value;
                Invalidate();
            }
        }

        public Color32 FocusedColor {
            get => m_FocusedColor;
            set {
                m_FocusedColor = value;
                Invalidate();
            }
        }

        protected override Color32 GetActiveColor() => State switch {
            UIButton.ButtonState.Focused => m_FocusedColor,
            UIButton.ButtonState.Hovered => m_HoveredColor,
            UIButton.ButtonState.Pressed => m_PressedColor,
            UIButton.ButtonState.Disabled => m_DisabledColor,
            _ => color,
        };

        protected virtual void OnButtonStateChanged(UIButton.ButtonState value) {
            if (!isEnabled && value != UIButton.ButtonState.Disabled) {
                return;
            }
            m_State = value;
            Invoke("OnButtonStateChanged", new object[] { value });
            Invalidate();
        }

        protected override void OnMouseEnter(UIMouseEventParameter p) {
            if(isEnabled) {
                if (m_State != UIButton.ButtonState.Focused) {
                    m_State = UIButton.ButtonState.Hovered;
                }
            }
            base.OnMouseEnter(p);
        }

        protected override void OnMouseDown(UIMouseEventParameter p) {
            if (m_State != UIButton.ButtonState.Focused) {
                m_State = UIButton.ButtonState.Pressed;
            }
            base.OnMouseDown(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p) {
            if (m_IsMouseHovering) {
                if (containsFocus) {
                    m_State = UIButton.ButtonState.Focused;
                } else {
                    m_State = UIButton.ButtonState.Hovered;
                }
            } else if (hasFocus) {
                m_State = UIButton.ButtonState.Focused;
            } else {
                m_State = UIButton.ButtonState.Normal;
            }

            base.OnMouseUp(p);
        }

        protected override void OnIsEnabledChanged() {
            m_State = isEnabled ? UIButton.ButtonState.Normal : UIButton.ButtonState.Disabled;
            base.OnIsEnabledChanged();
        }

        protected override void OnEnterFocus(UIFocusEventParameter p) {
            if (m_State != UIButton.ButtonState.Pressed) {
                m_State = UIButton.ButtonState.Focused;
            }
            base.OnEnterFocus(p);
        }

        protected override void OnLeaveFocus(UIFocusEventParameter p) {
            m_State = containsMouse ? UIButton.ButtonState.Hovered : UIButton.ButtonState.Normal;
            base.OnLeaveFocus(p);
        }

        public void SetStyle() {
            atlas = CustomUIAtlas.MbyronModsAtlas;
            selectionSprite = CustomUIAtlas.EmptySprite;
            normalBgSprite = CustomUIAtlas.FieldNormal;
            disabledBgSprite = CustomUIAtlas.FieldDisabled;
            focusedBgSprite = CustomUIAtlas.FieldNormal;
            hoveredBgSprite = CustomUIAtlas.FieldHovered;

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
