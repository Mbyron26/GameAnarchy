using ColossalFramework;
using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon {
    public class OptionKeymapping : UICustomControl {
        private KeyBinding binding;
        private UILabel Label { get; set; }
        private UIButton Button { get; set; }
        private bool IsInitialized { get; set; }
        public UIPanel Panel { get; set; }
        public string Tooltip { get => Panel.tooltip; set => Panel.tooltip = value; }

        public event Action<KeyBinding> EventBindingChanged;
        public KeyBinding Binding {
            get => binding;
            set {
                binding = value;
                Button.text = SavedInputKey.ToLocalizedString("KEYNAME", KeySetting);
                EventBindingChanged?.Invoke(Binding);
            }
        }
        public InputKey KeySetting {
            get => Binding.Encode();
            set {
                Binding.SetKey(value);
                Button.text = SavedInputKey.ToLocalizedString("KEYNAME", KeySetting);
            }
        }
        public string LabelText { set => Label.text = value; }
        public string ButtonText { set => Button.text = value; }

        public OptionKeymapping() {
            Panel = component.AddUIComponent<UIPanel>();
            Panel.width = 724;
            Panel.height = 30;
            Panel.autoLayout = false;
            Label = CustomLabel.AddLabel(Panel, "Template", 100, color: Color.white);
            Button = CustomButton.AddGreenButton(Panel, "Template", 30, 280, null);
            IUIStyle uiTool = new UIStyleAlpha(Panel) { Child = Button, MajorLabel = Label, Padding = new(0, 0, 10, 10) };
            uiTool.RefreshLayout();
            Button.eventKeyDown += OnBindingKeyDown;
            Button.eventMouseDown += OnBindingMouseDown;
        }

        private void OnBindingKeyDown(UIComponent component, UIKeyEventParameter eventParam) {
            if (IsInitialized && !IsModifierKey(eventParam.keycode)) {
                eventParam.Use();
                InputKey inputKey;
                if (eventParam.keycode is KeyCode.Escape) {
                    inputKey = KeySetting;
                } else {
                    inputKey = (eventParam.keycode == KeyCode.Backspace) ? SavedInputKey.Empty : SavedInputKey.Encode(eventParam.keycode, eventParam.control, eventParam.shift, eventParam.alt);
                }
                ApplyKey(inputKey);
            }
        }
        private void OnBindingMouseDown(UIComponent component, UIMouseEventParameter eventParam) {
            eventParam.Use();
            if (!IsInitialized) {
                Button.buttonsMask = UIMouseButton.Left | UIMouseButton.Right | UIMouseButton.Middle | UIMouseButton.Special0 | UIMouseButton.Special1
                    | UIMouseButton.Special2 | UIMouseButton.Special3;
                Button.text = CommonLocalize.KeyBinding_PressAnyKey;
                Button.Focus();
                IsInitialized = true;
                UIView.PushModal(Button);
            } else {
                if (IsUnbindableMouseButton(eventParam.buttons)) {
                    Button.text = SavedInputKey.ToLocalizedString("KEYNAME", KeySetting);
                    UIView.PopModal();
                    IsInitialized = false;
                } else {
                    KeyCode keyCode = eventParam.buttons switch {
                        UIMouseButton.Middle => KeyCode.Mouse2,
                        UIMouseButton.Special0 => KeyCode.Mouse3,
                        UIMouseButton.Special1 => KeyCode.Mouse4,
                        UIMouseButton.Special2 => KeyCode.Mouse5,
                        UIMouseButton.Special3 => KeyCode.Mouse6,
                        _ => KeyCode.None
                    };
                    ApplyKey(SavedInputKey.Encode(keyCode, IsControlDown(), IsShiftDown(), IsAltDown()));
                }
            }
        }

        private void ApplyKey(InputKey key) {
            KeySetting = key;
            UIView.PopModal();
            IsInitialized = false;
        }
        private bool IsUnbindableMouseButton(UIMouseButton code) => code == UIMouseButton.Left || code == UIMouseButton.Right;
        private bool IsModifierKey(KeyCode code) => code == KeyCode.LeftControl || code == KeyCode.RightControl || code == KeyCode.LeftShift ||
            code == KeyCode.RightShift || code == KeyCode.LeftAlt || code == KeyCode.RightAlt;
        private bool IsControlDown() => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        private bool IsShiftDown() => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        private bool IsAltDown() => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

    }
}
