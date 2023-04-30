using ColossalFramework;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class KeyMappingSinglePropertyPanel : AlphaSinglePropertyPanel {
    private KeyBinding binding;
    private bool isInitialized;

    public event Action<KeyBinding> EventBindingChanged;
    public KeyBinding Binding {
        get => binding;
        set {
            binding = value;
            (Child as CustomUIButton).Text = SavedInputKey.ToLocalizedString("KEYNAME", KeySetting);
            EventBindingChanged?.Invoke(Binding);
        }
    }
    public InputKey KeySetting {
        get => Binding.Encode();
        set {
            Binding.SetKey(value);
            (Child as CustomUIButton).Text = SavedInputKey.ToLocalizedString("KEYNAME", KeySetting);
        }
    }

    public KeyMappingSinglePropertyPanel() => OnChildAdded += OnButtonAdded;

    private void OnButtonAdded(UIComponent child) {
        child.eventKeyDown += OnBindingKeyDown;
        child.eventMouseDown += OnBindingMouseDown;
    }
    private void OnBindingKeyDown(UIComponent component, UIKeyEventParameter eventParam) {
        if (isInitialized && !IsModifierKey(eventParam.keycode)) {
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
        var button = component as CustomUIButton;
        if (!isInitialized) {
            button.ButtonMask = UIMouseButton.Left | UIMouseButton.Right | UIMouseButton.Middle | UIMouseButton.Special0 | UIMouseButton.Special1
                | UIMouseButton.Special2 | UIMouseButton.Special3;
            button.Text = CommonLocalize.KeyBinding_PressAnyKey;
            button.Focus();
            isInitialized = true;
            UIView.PushModal(button);
        } else {
            if (IsUnbindableMouseButton(eventParam.buttons)) {
                button.Text = SavedInputKey.ToLocalizedString("KEYNAME", KeySetting);
                UIView.PopModal();
                isInitialized = false;
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
        isInitialized = false;
    }
    private bool IsUnbindableMouseButton(UIMouseButton code) => code == UIMouseButton.Left || code == UIMouseButton.Right;
    private bool IsModifierKey(KeyCode code) => code == KeyCode.LeftControl || code == KeyCode.RightControl || code == KeyCode.LeftShift ||
        code == KeyCode.RightShift || code == KeyCode.LeftAlt || code == KeyCode.RightAlt;
    private bool IsControlDown() => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    private bool IsShiftDown() => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    private bool IsAltDown() => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

}

public class DeltaSinglePropertyPanel : SinglePropertyPanelBase {
    private float gap = 8;
    public float Gap {
        get => gap;
        set {
            if (!value.Equals(gap)) {
                gap = value;
                StartLayout();
            }
        }
    }
    public override void StartLayout() {
        float height0 = default;
        float height1 = default;
        float height2 = default;
        if (Child is not null) {
            Child.relativePosition = new Vector2(Padding.left, Padding.top);
            height0 = Child.height;
        }
        if (MajorLabel is not null) {
            var width = this.width - Padding.horizontal - gap - (Child is null ? 0 : Child.width);
            MajorLabel.width = width;
            MajorLabel.padding.top = 1;
            MajorLabel.relativePosition = new Vector2(Child.relativePosition.x + Child.size.x + gap, Padding.top);
            height1 = MajorLabel.height;
        }
        if (MinorLabel is not null) {
            MinorLabel.width = this.width - Padding.horizontal;
            MinorLabel.relativePosition = height0 > height1 ? new Vector2(Padding.left, Padding.top + height0 + gap) : (height0 < height1 ? new Vector2(Padding.left, Padding.top + height1 + gap) : new Vector2(Padding.left, height0 == 0 ? Padding.top + height1 + Padding.vertical : Padding.top + height0 + Padding.vertical));
            height2 = MinorLabel.height;
        }
        if (height0 > height1) {
            this.height = height0 + Padding.vertical + (height2 == 0 ? 0 : height2 + gap);
        } else if (height0 < height1) {
            this.height = height1 + Padding.vertical + (height2 == 0 ? 0 : height2 + gap); ;
        } else {
            this.height = height0 == 0 ? height1 + Padding.vertical : height0 + Padding.vertical + (height2 == 0 ? 0 : height2 + gap); ;
        }

    }
}

public class GammaSinglePropertyPanel : SinglePropertyPanelBase {
    private float gap = 2;
    private UIHorizontalAlignment childHorizontalAlignment = UIHorizontalAlignment.Center;
    public float Gap {
        get => gap;
        set {
            if (!value.Equals(gap)) {
                gap = value;
                StartLayout();
            }
        }
    }
    public UIHorizontalAlignment ChildHorizontalAlignment {
        get => childHorizontalAlignment;
        set {
            if (!value.Equals(childHorizontalAlignment)) {
                childHorizontalAlignment = value;
                StartLayout();
            }
        }
    }
    public override void StartLayout() {
        float height0 = Padding.top;
        if (MajorLabel is not null) {
            MajorLabel.width = width - Padding.horizontal;
            MajorLabel.relativePosition = new Vector2(Padding.left, height0);
            height0 = MajorLabel.relativePosition.y + MajorLabel.size.y;
        }
        if (Child is not null) {
            height0 += gap;
            if (childHorizontalAlignment == UIHorizontalAlignment.Center) {
                Child.relativePosition = new Vector2((width - Child.width) / 2, height0);
            } else if (childHorizontalAlignment == UIHorizontalAlignment.Left) {
                Child.relativePosition = new Vector2(Padding.left, height0);
            } else {
                Child.relativePosition = new Vector2(width - Padding.right - Child.width, height0);
            }
            height0 = Child.relativePosition.y + Child.size.y;
        }
        if (MinorLabel is not null) {
            height0 += gap;
            MinorLabel.width = width - Padding.left - Padding.right;
            MinorLabel.relativePosition = new Vector2(Padding.left, height0);
            height0 = MinorLabel.relativePosition.y + MinorLabel.size.y;
        }
        height0 += Padding.bottom;
        height = height0;
    }
}

public class AlphaSinglePropertyPanel : SinglePropertyPanelBase {
    protected float labelGap = 4;
    protected float labelChildGap = 10;
    public float LabelGap {
        get => labelGap;
        set {
            if (!Equals(value, labelGap)) {
                labelGap = value;
                StartLayout();
            }
        }
    }
    public float LabelChildGap {
        get => labelChildGap;
        set {
            if (!Equals(value, labelChildGap)) {
                labelChildGap = value;
                StartLayout();
            }
        }
    }

    public override void StartLayout() {
        float height0 = default;
        float height1 = default;
        if (majorLabel is not null) {
            MajorLabel.width = Child is null ? width - Padding.left - Padding.right : width - Padding.left - Padding.right - Child.width - labelChildGap;
            height0 = MajorLabel.height;
        }
        if (MinorLabel is not null) {
            MinorLabel.width = Child is null ? width - Padding.left - Padding.right : width - Padding.left - Padding.right - Child.width - labelChildGap;
            height1 = MinorLabel.height;
        }
        float height2 = height0 + (height1 == 0 ? 0 : (height1 + labelGap));
        float height3 = default;
        if (Child is not null) {
            height3 = Child.height;
        }
        height = Mathf.Max(height2, height3) + Padding.top + Padding.bottom;
        if (height2 > height3) {
            if (MajorLabel is not null) {
                MajorLabel.relativePosition = new Vector2(Padding.left, Padding.top);
            }
            if (MinorLabel is not null) {
                MinorLabel.relativePosition = new Vector2(Padding.left, MajorLabel is null ? Padding.top : MajorLabel.relativePosition.y + MajorLabel.size.y + LabelGap);
            }
            if (Child is not null) {
                Child.relativePosition = new Vector2(width - Padding.right - Child.width, (height - Child.height) / 2);
            }
        } else {
            if (Child is not null) {
                Child.relativePosition = new Vector2(width - Padding.right - Child.width, Padding.top);
            }
            if (MajorLabel is not null) {
                if (MinorLabel is null)
                    MajorLabel.relativePosition = new Vector2(Padding.left, (height - MajorLabel.height) / 2);
                else {
                    MajorLabel.relativePosition = new Vector2(Padding.left, (height - height2) / 2);
                    MinorLabel.relativePosition = new Vector2(Padding.left, MajorLabel.relativePosition.y + MajorLabel.size.y + labelGap);
                }
            }
        }
    }
}

public abstract class SinglePropertyPanelBase : CustomUIPanel {
    protected UILabel majorLabel;
    protected UILabel minorLabel;
    protected UIComponent child;

    public event Action<UIComponent> OnChildAdded;

    public UILabel MajorLabel => majorLabel;
    public UILabel MinorLabel => minorLabel;
    public virtual UIComponent Child {
        get => child;
        set {
            if (child != value) {
                child = value;
                OnChildAdded?.Invoke(value);
            }
        }
    }
    public string MajorLabelText {
        get => majorLabel is null ? string.Empty : majorLabel.text;
        set {
            if (majorLabel is not null) {
                majorLabel.text = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.text = value;
            }
        }
    }
    public string MinorLabelText {
        get => minorLabel is null ? string.Empty : minorLabel.text;
        set {
            if (minorLabel is not null) {
                minorLabel.text = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.textColor = CustomUIColor.OffWhite;
                minorLabel.text = value;
            }
        }
    }
    public float MajorLabelTextScale {
        get => majorLabel is null ? 0f : majorLabel.textScale;
        set {
            if (majorLabel is not null) {
                majorLabel.textScale = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.textScale = value;
            }
        }
    }
    public float MinorLabelTextScale {
        get => minorLabel is null ? 0f : minorLabel.textScale;
        set {
            if (minorLabel is not null) {
                minorLabel.textScale = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.textColor = CustomUIColor.OffWhite;
                minorLabel.textScale = value;
            }
        }
    }
    public Color32 MajorLabelTextColor {
        get => majorLabel is null ? CustomUIColor.White : majorLabel.textColor;
        set {
            if (majorLabel is not null) {
                majorLabel.textColor = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.textColor = value;
            }
        }
    }
    public Color32 MinorLabelTextColor {
        get => minorLabel is null ? CustomUIColor.White : minorLabel.textColor;
        set {
            if (minorLabel is not null) {
                minorLabel.textColor = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.textColor = CustomUIColor.OffWhite;
                minorLabel.textColor = value;
            }
        }
    }
    public Color32 MajorLabelDisabledColor {
        get => majorLabel is null ? CustomUIColor.White : majorLabel.disabledTextColor;
        set {
            if (majorLabel is not null) {
                majorLabel.disabledTextColor = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.disabledTextColor = value;
            }
        }
    }
    public Color32 MinorLabelDisabledColor {
        get => minorLabel is null ? CustomUIColor.White : minorLabel.disabledTextColor;
        set {
            if (minorLabel is not null) {
                minorLabel.disabledTextColor = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.textColor = CustomUIColor.OffWhite;
                minorLabel.disabledTextColor = value;
            }
        }
    }
    public bool ProcessMajorLabelMarkup {
        get => majorLabel is not null && majorLabel.processMarkup;
        set {
            if (majorLabel is not null) {
                majorLabel.processMarkup = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.processMarkup = value;
            }
        }
    }
    public bool ProcessMinorLabelMarkup {
        get => minorLabel is not null && minorLabel.processMarkup;
        set {
            if (minorLabel is not null) {
                minorLabel.processMarkup = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.textColor = CustomUIColor.OffWhite;
                minorLabel.processMarkup = value;
            }
        }
    }
    public RectOffset MajorLabelOffset {
        get => majorLabel is null ? new RectOffset() : majorLabel.padding;
        set {
            if (majorLabel is not null) {
                majorLabel.padding = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.padding = value;
            }
        }
    }
    public RectOffset MinorLabelOffset {
        get => minorLabel is null ? new RectOffset() : minorLabel.padding;
        set {
            if (minorLabel is not null) {
                minorLabel.padding = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.textColor = CustomUIColor.OffWhite;
                minorLabel.padding = value;
            }
        }
    }

    protected virtual UILabel AddLabel() {
        var label = AddUIComponent<UILabel>();
        label.autoSize = false;
        label.width = width;
        label.autoHeight = true;
        label.wordWrap = true;
        label.processMarkup = true;
        label.disabledTextColor = CustomUIColor.DisabledTextColor;
        return label;
    }
    public abstract void StartLayout();
}

public class PropertyPanel : CustomUIPanel {
    private UILabel majorLabel;
    private UILabel minorLabel;
    private CustomUIPanel groupPropertyPanel;
    private readonly List<SinglePropertyPanelBase> itemPanels = new();

    public event Action<SinglePropertyPanelBase, int> EventOnSinglePropertyPanelAdded;
    public event Action<CustomUIPanel> EventSetGroupPropertyPanelStyle;
    public UILabel MajorLabel => majorLabel;
    public UILabel MinorLabel => minorLabel;
    public string MajorLabelText {
        get => majorLabel is null ? string.Empty : majorLabel.text;
        set {
            if (majorLabel is not null) {
                majorLabel.text = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.text = value;
            }
        }
    }
    public string MinorLabelText {
        get => minorLabel is null ? string.Empty : minorLabel.text;
        set {
            if (minorLabel is not null) {
                minorLabel.text = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.text = value;
            }
        }
    }
    public float MajorLabelTextScale {
        get => majorLabel is null ? 0f : majorLabel.textScale;
        set {
            if (majorLabel is not null) {
                majorLabel.textScale = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.textScale = value;
            }
        }
    }
    public float MinorLabelTextScale {
        get => minorLabel is null ? 0f : minorLabel.textScale;
        set {
            if (minorLabel is not null) {
                minorLabel.textScale = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.textScale = value;
            }
        }
    }
    public Color32 MajorLabelColor {
        get => majorLabel is null ? CustomUIColor.White : majorLabel.color;
        set {
            if (majorLabel is not null) {
                majorLabel.color = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.color = value;
            }
        }
    }
    public Color32 MinorLabelColor {
        get => minorLabel is null ? CustomUIColor.White : minorLabel.color;
        set {
            if (minorLabel is not null) {
                minorLabel.color = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.color = value;
            }
        }
    }
    public Color32 MajorLabelDisabledColor {
        get => majorLabel is null ? CustomUIColor.White : majorLabel.disabledTextColor;
        set {
            if (majorLabel is not null) {
                majorLabel.disabledTextColor = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.disabledTextColor = value;
            }
        }
    }
    public Color32 MinorLabelDisabledColor {
        get => minorLabel is null ? CustomUIColor.White : minorLabel.disabledTextColor;
        set {
            if (minorLabel is not null) {
                minorLabel.disabledTextColor = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.disabledTextColor = value;
            }
        }
    }
    public bool ProcessMajorLabelMarkup {
        get => majorLabel is not null && majorLabel.processMarkup;
        set {
            if (majorLabel is not null) {
                majorLabel.processMarkup = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.processMarkup = value;
            }
        }
    }
    public bool ProcessMinorLabelMarkup {
        get => minorLabel is not null && minorLabel.processMarkup;
        set {
            if (minorLabel is not null) {
                minorLabel.processMarkup = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.processMarkup = value;
            }
        }
    }
    public RectOffset MajorLabelOffset {
        get => majorLabel is null ? new RectOffset() : majorLabel.padding;
        set {
            if (majorLabel is not null) {
                majorLabel.padding = value;
            } else {
                majorLabel = AddLabel();
                majorLabel.padding = value;
            }
        }
    }
    public RectOffset MinorLabelOffset {
        get => minorLabel is null ? new RectOffset() : minorLabel.padding;
        set {
            if (minorLabel is not null) {
                minorLabel.padding = value;
            } else {
                minorLabel = AddLabel();
                minorLabel.padding = value;
            }
        }
    }
    public bool GroupPropertyPanelStyleInitialized { get; private set; } = false;
    public List<SinglePropertyPanelBase> ItemPanels => itemPanels;
    public int ItemPanelsCount => itemPanels.Count;

    private UILabel AddLabel() {
        var label = AddUIComponent<UILabel>();
        label.autoSize = false;
        label.width = width - Padding.horizontal;
        label.autoHeight = true;
        label.wordWrap = true;
        label.processMarkup = true;
        label.disabledTextColor = CustomUIColor.DisabledTextColor;
        return label;
    }
    public TypePanel AddItemPanel<TypePanel>() where TypePanel : SinglePropertyPanelBase {
        if (groupPropertyPanel is null) {
            groupPropertyPanel = AddUIComponent<CustomUIPanel>();
            groupPropertyPanel.width = width;
            groupPropertyPanel.AutoLayout = true;
            groupPropertyPanel.AutoFitChildrenVertically = true;
            if (!GroupPropertyPanelStyleInitialized) {
                GroupPropertyPanelStyleInitialized = true;
                EventSetGroupPropertyPanelStyle?.Invoke(groupPropertyPanel);
            }
        }
        var panel = groupPropertyPanel.AddUIComponent<TypePanel>();
        itemPanels.Add(panel);
        EventOnSinglePropertyPanelAdded?.Invoke(panel, ItemPanelsCount);
        return panel;
    }
}


