using ColossalFramework.Globalization;
using ColossalFramework.PlatformServices;
using ColossalFramework.UI;
using ColossalFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.ComponentModel;
namespace MbyronModsCommon.UI;

public static class UIValueFieldHelper {
    public static T AddOptionPanelValueField<T, V>(UIComponent parent, V defaultValue, V minValue, V maxValue, Action<V> callback = null, float fieldWidth = 100, float fieldHeight = 28) where T : CustomUIValueFieldBase<V> where V : IComparable<V> {
        var valueField = parent.AddUIComponent<T>();
        valueField.builtinKeyNavigation = true;
        valueField.TextPadding = new RectOffset(0, 0, 6, 0);
        valueField.size = new Vector2(fieldWidth, fieldHeight);
        valueField.UseValueLimit = true;
        valueField.MinValue = minValue;
        valueField.MaxValue = maxValue;
        valueField.Value = defaultValue;
        valueField.EventValueChanged += (v) => callback?.Invoke(v);
        return valueField;
    }
}

public class UIFloatValueField : CustomUIValueFieldBase<float> {
    protected override float ValueDecrease(SteppingRate steppingRate) {
        var rate = GetStep(steppingRate);
        return (float)Math.Round(Value - rate, 1);
    }
    protected override float ValueIncrease(SteppingRate steppingRate) {
        var rate = GetStep(steppingRate);
        return (float)Math.Round(Value + rate, 1);
    }
    protected override float GetStep(SteppingRate steppingRate) => steppingRate switch {
        SteppingRate.Fast => WheelStep * 10,
        SteppingRate.Slow => WheelStep / 10,
        _ => WheelStep,
    };
}

public class UILongValueField : CustomUIValueFieldBase<long> {
    protected override long ValueDecrease(SteppingRate steppingRate) {
        var rate = GetStep(steppingRate);
        return Value - rate;
    }
    protected override long ValueIncrease(SteppingRate steppingRate) {
        var rate = GetStep(steppingRate);
        return Value + rate;
    }
    protected override long GetStep(SteppingRate steppingRate) => steppingRate switch {
        SteppingRate.Fast => WheelStep * 10,
        SteppingRate.Slow => WheelStep / 10,
        _ => WheelStep,
    };
}

public class UIIntValueField : CustomUIValueFieldBase<int> {
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

public class UIStringField : CustomUIValueFieldBase<string> {
    public UIStringField() => builtinKeyNavigation = false;

    protected override string GetStep(SteppingRate steppingRate) => string.Empty;
    protected override string ValueDecrease(SteppingRate steppingRate) => string.Empty;
    protected override string ValueIncrease(SteppingRate steppingRate) => string.Empty;
}

public abstract class CustomUIValueFieldBase<T> : CustomUITextComponent where T : IComparable<T> {
    protected Sprites bgSprites = new();
    protected bool renderBg = true;
    private bool hasImeInput;
    private int selectionStart;
    private int selectionEnd;
    protected string selectionSprite = string.Empty;
    protected Color32 selectionBgColor = new(34, 42, 58, 255);
    protected int textMaxLength = 1024;
    protected bool readOnly;
    protected bool isPasswordField;
    protected string passwordCharacter = "*";
    private bool undoing;
    public const int UndoLimit = 20;
    private readonly List<UndoData> undoData = new(UndoLimit);
    private int undoCount;
    private int cursorIndex;
    private bool focusForced;
    private string undoText = "";
    private int scrollIndex;
    private bool cursorShown;
    private int mouseSelectionAnchor;
    private float[] charWidths;
    private float leftOffset;
    private int lineScrollIndex;
    private List<int> lines = new();
    private float lineHeight;
    private bool cursorAtEndOfLine;
    protected SpriteState state;
    protected T value;
    protected string format = "{0}";
    private bool showTooltip;

    public event Action<T> EventValueChanged;
    public event Action<SpriteState> EventStateChanged;
    public event PropertyChangedEventHandler<string> EventTextChanged;
    public event PropertyChangedEventHandler<string> EventPasswordCharacterChanged;
    public event PropertyChangedEventHandler<bool> EventReadOnlyChanged;
    public event PropertyChangedEventHandler<string> EventTextSubmitted;
    public event PropertyChangedEventHandler<string> EventTextCancelled;

    public Sprites BgSprites => bgSprites;
    public bool RenderBg {
        get => renderBg;
        set {
            if (renderBg != value) {
                renderBg = value;
                Invalidate();
            }
        }
    }
    public Color32 BgNormalColor {
        get => bgSprites.NormalColor;
        set {
            if (!bgSprites.NormalColor.Equals(value)) {
                bgSprites.NormalColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgHoveredColor {
        get => bgSprites.HoveredColor;
        set {
            if (!bgSprites.HoveredColor.Equals(value)) {
                bgSprites.HoveredColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgPressedColor {
        get => bgSprites.PressedColor;
        set {
            if (!bgSprites.PressedColor.Equals(value)) {
                bgSprites.PressedColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgFocusedColor {
        get => bgSprites.FocusedColor;
        set {
            if (!bgSprites.FocusedColor.Equals(value)) {
                bgSprites.FocusedColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgDisabledColor {
        get => bgSprites.DisabledColor;
        set {
            if (!bgSprites.DisabledColor.Equals(value)) {
                bgSprites.DisabledColor = value;
                Invalidate();
            }
        }
    }
    private string CompositionString => hasImeInput ? Input.compositionString : string.Empty;
    public override string Text {
        get => text;
        set {
            if (value.Length > TextMaxLength) {
                value = value.Substring(0, TextMaxLength);
            }
            value = value.Replace("\t", " ");
            if (value != text) {
                text = value;
                scrollIndex = (cursorIndex = 0);
                OnTextChanged();
                Invalidate();
            }
        }
    }
    public int SelectionStart {
        get => selectionStart;
        set {
            if (value != selectionStart) {
                selectionStart = Mathf.Max(0, Mathf.Min(value, text.Length));
                selectionEnd = Mathf.Max(selectionEnd, selectionStart);
                Invalidate();
            }
        }
    }
    public int SelectionEnd {
        get => selectionEnd;
        set {
            if (value != selectionEnd) {
                selectionEnd = Mathf.Max(0, Mathf.Min(value, text.Length));
                selectionStart = Mathf.Max(selectionStart, selectionEnd);
                Invalidate();
            }
        }
    }
    public int SelectionLength => selectionEnd - selectionStart;
    public string SelectedText => (selectionEnd == selectionStart) ? string.Empty : text.Substring(SelectionStart, SelectionLength);
    public string SelectionSprite {
        get => selectionSprite;
        set {
            if (value != selectionSprite) {
                selectionSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 SelectionBgColor {
        get => selectionBgColor;
        set {
            selectionBgColor = value;
            Invalidate();
        }
    }
    public int TextMaxLength {
        get => textMaxLength;
        set {
            if (value != textMaxLength) {
                textMaxLength = Mathf.Max(0, value);
                if (TextMaxLength < text.Length) {
                    Text = text.Substring(0, TextMaxLength);
                }
                Invalidate();
            }
        }
    }
    public bool ReadOnly {
        get => readOnly;
        set {
            if (value != readOnly) {
                readOnly = value;
                OnReadOnlyChanged();
                Invalidate();
            }
        }
    }
    public bool IsPasswordField {
        get => isPasswordField;
        set {
            if (value != isPasswordField) {
                isPasswordField = value;
                Invalidate();
            }
        }
    }
    public string PasswordCharacter {
        get => passwordCharacter;
        set {
            if (!string.IsNullOrEmpty(value)) {
                passwordCharacter = value[0].ToString();
            } else {
                passwordCharacter = value;
            }
            OnPasswordCharacterChanged();
            Invalidate();
        }
    }
    public bool Multiline { get; set; }
    public bool SubmitOnFocusLost { get; set; } = true;
    public bool SelectOnFocus { get; set; }
    public bool NumericalOnly { get; set; }
    public bool AllowFloats { get; set; }
    public bool AllowNegative { get; set; }
    public float CursorBlinkTime { get; set; } = 0.8f;
    public int CursorWidth { get; set; } = 1;
    public SpriteState State {
        get => state;
        set {
            if (state != value) {
                OnStateChanged(value);
            }
        }
    }
    public T Value {
        get => value;
        set => OnValueChanged(value);
    }
    public T MinValue { get; set; }
    public T MaxValue { get; set; }
    public bool UseMinValueLimit { get; set; }
    public bool UseMaxValueLimit { get; set; }
    public bool UseValueLimit { set => UseMinValueLimit = UseMaxValueLimit = value; }
    public bool CanWheel { get; set; }
    public T WheelStep { get; set; }
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
            if (value) {
                tooltip = CommonLocalize.ScrollWheel;
            }
        }
    }
    protected bool WheelAvailable { get; set; }
    public float CursorHeight { get; set; } = 1;
    public bool CustomCursorHeight { get; set; }

    public CustomUIValueFieldBase() => m_CanFocus = true;

    public virtual void SetOptionPanelStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        bgSprites.SetSprites(CustomUIAtlas.RoundedRectangle3);
        bgSprites.SetColors(CustomUIColor.OPButtonNormal, CustomUIColor.OPButtonHovered, CustomUIColor.OPButtonPressed, CustomUIColor.BlueNormal, CustomUIColor.OPButtonDisabled);
        selectionSprite = CustomUIAtlas.Rectangle;
    }
    public virtual void SetControlPanelStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        bgSprites.SetSprites(CustomUIAtlas.RoundedRectangle2);
        bgSprites.SetColors(CustomUIColor.CPButtonNormal, CustomUIColor.CPButtonHovered, CustomUIColor.CPButtonPressed, CustomUIColor.GreenNormal, CustomUIColor.CPButtonDisabled);
        selectionSprite = CustomUIAtlas.Rectangle;
    }
    protected virtual void RefreshText() {
        if (hasFocus) {
            if (value != null) {
                Text = string.Format(format, value.ToString());
            } else {
                Text = string.Empty;
            }
        } else {
            Text = string.Format(format, value?.ToString() ?? string.Empty);
        }
    }
    protected virtual void OnValueChanged(T value) {
        if (UseMinValueLimit && value.CompareTo(MinValue) < 0) {
            this.value = MinValue;
        } else if (UseMaxValueLimit && value.CompareTo(MaxValue) > 0) {
            this.value = MaxValue;
        } else {
            this.value = value;
        }
        RefreshText();
        EventValueChanged?.Invoke(value);
    }
    protected virtual void OnStateChanged(SpriteState value) {
        if (!isEnabled && value != SpriteState.Disabled) {
            return;
        }
        state = value;
        EventStateChanged?.Invoke(value);
        Invalidate();
    }
    protected virtual void OnTextChanged() {
        if (!undoing) {
            undoData.RemoveRange(undoData.Count - undoCount, undoCount);
            undoData.Add(new UndoData { Text = Text, Position = cursorIndex });
            undoCount = 0;
            if (UndoLimit != 0 && UndoLimit <= undoData.Count) {
                undoData.RemoveAt(0);
            }
        }
        FilterText();
        EventTextChanged?.Invoke(this, Text);
        Invoke("OnTextChanged", new object[] { Text });
    }
    private void FilterText() {
        if (PlatformService.apiBackend == APIBackend.Rail && !string.IsNullOrEmpty(text) && !NumericalOnly && !IsPasswordField) {
            string text = PlatformService.DirtyWordsFilter(this.text, true);
            if (text != null) {
                this.text = text;
            }
        }
    }
    protected virtual void OnPasswordCharacterChanged() {
        EventPasswordCharacterChanged?.Invoke(this, PasswordCharacter);
        Invoke("OnPasswordCharacterChanged", new object[] { PasswordCharacter });
    }
    protected virtual void OnReadOnlyChanged() {
        EventReadOnlyChanged?.Invoke(this, ReadOnly);
        Invoke("OnReadOnlyChanged", new object[] { ReadOnly });
    }
    protected virtual void OnSubmit() {
        focusForced = true;
        Unfocus();
        EventTextSubmitted?.Invoke(this, Text);
        if (!hasFocus && text == Convert.ToString(Value)) {
            RefreshText();
            return;
        }
        T newValue = value;
        try {
            if (typeof(T) == typeof(string)) {
                newValue = (T)(object)text;
            } else if (!string.IsNullOrEmpty(text))
                newValue = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(text);
        }
        catch { }
        OnValueChanged(newValue);
        InvokeUpward("OnTextSubmitted", new object[] { Text });
    }
    protected virtual void OnCancel() {
        focusForced = true;
        text = undoText;
        Unfocus();
        EventTextCancelled?.Invoke(this, Text);
        InvokeUpward("OnTextCancelled", new object[] { this, Text });
    }
    private bool IsDigit(char character) {
        string numberDecimalSeparator = LocaleManager.cultureInfo.NumberFormat.NumberDecimalSeparator;
        string negativeSign = LocaleManager.cultureInfo.NumberFormat.NegativeSign;
        return char.IsDigit(character) || (AllowFloats && character.ToString() == numberDecimalSeparator && !text.Contains(numberDecimalSeparator)) || ((AllowFloats || AllowNegative) && character.ToString() == negativeSign && !text.Contains(negativeSign));
    }
    protected override void OnKeyPress(UIKeyEventParameter p) {
        if (!builtinKeyNavigation) {
            base.OnKeyPress(p);
            return;
        }
        if (ReadOnly || char.IsControl(p.character)) {
            base.OnKeyPress(p);
            return;
        }
        if (NumericalOnly && !IsDigit(p.character)) {
            base.OnKeyPress(p);
            return;
        }
        base.OnKeyPress(p);
        if (p.used) {
            return;
        }
        DeleteSelection();
        SetIMEPosition();
        if (text.Length < TextMaxLength) {
            if (cursorIndex == text.Length) {
                text += p.character;
            } else {
                text = text.Insert(cursorIndex, p.character.ToString());
            }
            cursorIndex++;
            OnTextChanged();
            Invalidate();
        }
        p.Use();
    }
    public void Cancel() {
        ClearSelection();
        cursorIndex = (scrollIndex = 0);
        Invalidate();
        OnCancel();
    }
    protected override void OnKeyDown(UIKeyEventParameter p) {
        if (ReadOnly) {
            return;
        }
        if (p.used) {
            return;
        }
        KeyCode keycode = p.keycode;
        if (keycode <= KeyCode.Escape) {
            if (keycode != KeyCode.Backspace) {
                if (keycode != KeyCode.Return) {
                    if (keycode == KeyCode.Escape) {
                        ClearSelection();
                        cursorIndex = (scrollIndex = 0);
                        Invalidate();
                        OnCancel();
                        goto IL_45E;
                    }
                } else {
                    if (Multiline) {
                        AddLineBreak();
                        goto IL_45E;
                    }
                    OnSubmit();
                    goto IL_45E;
                }
            } else {
                if (p.control) {
                    DeletePreviousWord();
                    goto IL_45E;
                }
                DeletePreviousChar();
                goto IL_45E;
            }
        } else if (keycode <= KeyCode.Z) {
            switch (keycode) {
                case KeyCode.A:
                    if (p.control) {
                        SelectAll();
                        goto IL_45E;
                    }
                    goto IL_45E;
                case KeyCode.B:
                    break;
                case KeyCode.C:
                    if (p.control) {
                        CopySelectionToClipboard();
                        goto IL_45E;
                    }
                    goto IL_45E;
                default:
                    switch (keycode) {
                        case KeyCode.V: {
                                if (!p.control) {
                                    goto IL_45E;
                                }
                                string text = Clipboard.text;
                                if (!string.IsNullOrEmpty(text)) {
                                    PasteAtCursor(text);
                                    goto IL_45E;
                                }
                                goto IL_45E;
                            }
                        case KeyCode.X:
                            if (p.control) {
                                CutSelectionToClipboard();
                                goto IL_45E;
                            }
                            goto IL_45E;
                        case KeyCode.Y:
                            if (p.control) {
                                undoing = true;
                                try {
                                    undoCount--;
                                    ClearSelection();
                                    Text = undoData[undoData.Count - undoCount - 1].Text;
                                    cursorIndex = undoData[undoData.Count - undoCount - 1].Position;
                                }
                                catch {
                                    undoCount++;
                                }
                                undoing = false;
                                goto IL_45E;
                            }
                            goto IL_45E;
                        case KeyCode.Z:
                            if (p.control) {
                                undoing = true;
                                try {
                                    undoCount++;
                                    ClearSelection();
                                    Text = undoData[undoData.Count - undoCount - 1].Text;
                                    cursorIndex = undoData[undoData.Count - undoCount - 1].Position;
                                }
                                catch {
                                    undoCount--;
                                }
                                undoing = false;
                                goto IL_45E;
                            }
                            goto IL_45E;
                    }
                    break;
            }
        } else if (keycode != KeyCode.Delete) {
            switch (keycode) {
                case KeyCode.UpArrow:
                    if (!Multiline) {
                        goto IL_45E;
                    }
                    if (p.shift) {
                        MoveSelectionPointUp();
                        goto IL_45E;
                    }
                    MoveToUpChar();
                    goto IL_45E;
                case KeyCode.DownArrow:
                    if (!Multiline) {
                        goto IL_45E;
                    }
                    if (p.shift) {
                        MoveSelectionPointDown();
                        goto IL_45E;
                    }
                    MoveToDownChar();
                    goto IL_45E;
                case KeyCode.RightArrow:
                    if (p.control) {
                        if (p.shift) {
                            MoveSelectionPointRightWord();
                            goto IL_45E;
                        }
                        MoveToNextWord();
                        goto IL_45E;
                    } else {
                        if (p.shift) {
                            MoveSelectionPointRight();
                            goto IL_45E;
                        }
                        if (SelectionLength > 0) {
                            MoveToSelectionEnd();
                            goto IL_45E;
                        }
                        MoveToNextChar();
                        goto IL_45E;
                    }
                case KeyCode.LeftArrow:
                    if (p.control) {
                        if (p.shift) {
                            MoveSelectionPointLeftWord();
                            goto IL_45E;
                        }
                        MoveToPreviousWord();
                        goto IL_45E;
                    } else {
                        if (p.shift) {
                            MoveSelectionPointLeft();
                            goto IL_45E;
                        }
                        if (SelectionLength > 0) {
                            MoveToSelectionStart();
                            goto IL_45E;
                        }
                        MoveToPreviousChar();
                        goto IL_45E;
                    }
                case KeyCode.Insert: {
                        if (!p.shift) {
                            goto IL_45E;
                        }
                        string text2 = Clipboard.text;
                        if (!string.IsNullOrEmpty(text2)) {
                            PasteAtCursor(text2);
                            goto IL_45E;
                        }
                        goto IL_45E;
                    }
                case KeyCode.Home:
                    if (p.shift) {
                        SelectToStart();
                        goto IL_45E;
                    }
                    MoveToStart();
                    goto IL_45E;
                case KeyCode.End:
                    if (p.shift) {
                        SelectToEnd();
                        goto IL_45E;
                    }
                    MoveToEnd();
                    goto IL_45E;
            }
        } else {
            if (selectionStart != selectionEnd) {
                DeleteSelection();
                goto IL_45E;
            }
            if (p.control) {
                DeleteNextWord();
                goto IL_45E;
            }
            DeleteNextChar();
            goto IL_45E;
        }
        base.OnKeyDown(p);
        return;
    IL_45E:
        p.Use();
    }
    protected override void OnGotFocus(UIFocusEventParameter p) {
        base.OnGotFocus(p);
        hasImeInput = true;
        Input.imeCompositionMode = IMECompositionMode.On;
        SetIMEPosition();
        undoText = Text;
        if (!ReadOnly) {
            StartCoroutine(MakeCursorBlink());
            if (SelectOnFocus) {
                selectionStart = 0;
                selectionEnd = text.Length;
            }
        }
        Invalidate();
    }
    protected override void OnLostFocus(UIFocusEventParameter p) {
        base.OnLostFocus(p);
        hasImeInput = false;
        Input.imeCompositionMode = IMECompositionMode.Auto;
        if (!focusForced) {
            if (SubmitOnFocusLost) {
                OnSubmit();
            } else {
                OnCancel();
            }
        }
        focusForced = false;
        cursorShown = false;
        ClearSelection();
        Invalidate();
    }
    protected override void OnDoubleClick(UIMouseEventParameter p) {
        if (!ReadOnly && p.buttons.IsFlagSet(UIMouseButton.Left)) {
            int charIndexAt = GetCharIndexAt(p);
            SelectWordAtIndex(charIndexAt);
        }
        base.OnDoubleClick(p);
    }
    protected override void OnClick(UIMouseEventParameter p) {
        p.Use();
        base.OnClick(p);
    }
    protected override void OnMouseDown(UIMouseEventParameter p) {
        if (isEnabled && State != SpriteState.Focused) {
            State = SpriteState.Pressed;
        }
        if (!ReadOnly && p.buttons.IsFlagSet(UIMouseButton.Left)) {
            int charIndexAt = GetCharIndexAt(p);
            if (charIndexAt != cursorIndex) {
                cursorIndex = charIndexAt;
                cursorShown = true;
                Invalidate();
                p.Use();
            }
            mouseSelectionAnchor = cursorIndex;
            selectionStart = (selectionEnd = cursorIndex);
        }
        base.OnMouseDown(p);
    }
    protected override void OnIsEnabledChanged() {
        if (!isEnabled) {
            State = SpriteState.Disabled;
        } else {
            State = SpriteState.Normal;
        }
        base.OnIsEnabledChanged();
    }
    protected override void OnEnterFocus(UIFocusEventParameter p) {
        if (State != SpriteState.Pressed) {
            State = SpriteState.Focused;
        }
        base.OnEnterFocus(p);
    }
    protected override void OnLeaveFocus(UIFocusEventParameter p) {
        State = (containsMouse ? SpriteState.Hovered : SpriteState.Normal);
        base.OnLeaveFocus(p);
    }
    protected override void OnMouseLeave(UIMouseEventParameter p) {
        if (isEnabled) {
            if (containsFocus) {
                State = SpriteState.Focused;
            } else {
                State = SpriteState.Normal;
            }
        }
        WheelAvailable = false;
        Invalidate();
        base.OnMouseLeave(p);
    }
    protected override void OnMouseEnter(UIMouseEventParameter p) {
        if (isEnabled && !hasFocus) {
            State = SpriteState.Hovered;
        }
        WheelAvailable = true;
        Invalidate();
        base.OnMouseEnter(p);
    }
    protected override void OnMouseWheel(UIMouseEventParameter p) {
        base.OnMouseWheel(p);
        tooltipBox.Hide();
        if (CanWheel) {
            var typeRate = GetSteppingRate();
            if (p.wheelDelta < 0) {
                OnValueChanged(ValueDecrease(typeRate));
            } else {
                OnValueChanged(ValueIncrease(typeRate));
            }
        }
    }
    protected abstract T ValueDecrease(SteppingRate steppingRate);
    protected abstract T ValueIncrease(SteppingRate steppingRate);
    protected abstract T GetStep(SteppingRate steppingRate);
    private SteppingRate GetSteppingRate() {
        if (KeyHelper.IsShiftDown()) return SteppingRate.Fast;
        else if (KeyHelper.IsControlDown()) return SteppingRate.Slow;
        else return SteppingRate.Normal;
    }

    private void SetIMEPosition() {
        UIView uiview = GetUIView();
        float num = uiview.PixelsToUnits();
        float num2 = 0f;
        int num3 = scrollIndex;
        while (num3 < cursorIndex && num3 < charWidths.Length) {
            num2 += charWidths[num3] / num;
            num3++;
        }
        Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 vector2 = new(vector.x + TextPadding.left, vector.y - TextPadding.top, 0f);
        float num4 = num2 + leftOffset / num + TextPadding.left;
        float num5 = uiview.uiCamera.pixelWidth / uiview.fixedWidth;
        float num6 = uiview.uiCamera.pixelHeight / uiview.fixedHeight;
        Vector3 vector3 = uiview.uiCamera.WorldToScreenPoint(transform.position);
        vector3.y = Screen.height - vector3.y;
        Vector2 compositionCursorPos = new(vector3.x + (vector2.x + num4) * num5, vector3.y + (vector2.y + size.y * 1.5f) * num6);
        Input.compositionCursorPos = compositionCursorPos;
    }
    protected override void OnMouseUp(UIMouseEventParameter p) {
        if (m_IsMouseHovering) {
            if (containsFocus) {
                State = SpriteState.Focused;
            } else {
                State = SpriteState.Hovered;
            }
        } else if (hasFocus) {
            State = SpriteState.Focused;
        } else {
            State = SpriteState.Normal;
        }
        if (!ReadOnly && p.buttons.IsFlagSet(UIMouseButton.Left) && PlatformService.ShowGamepadTextInput(IsPasswordField ? GamepadTextInputMode.TextInputModePassword : GamepadTextInputMode.TextInputModeNormal, GamepadTextInputLineMode.TextInputLineModeSingleLine, "Input", TextMaxLength, Text)) {
            p.Use();
            PlatformService.eventSteamGamepadInputDismissed += OnSteamInputDismissed;
        }
        base.OnMouseUp(p);
    }
    private void OnSteamInputDismissed(string str) {
        PlatformService.eventSteamGamepadInputDismissed -= OnSteamInputDismissed;
        if (str != null) {
            Text = str;
            OnSubmit();
        }
        MoveToEnd();
        Unfocus();
    }
    protected override void OnMouseMove(UIMouseEventParameter p) {
        if (!ReadOnly && hasFocus && p.buttons.IsFlagSet(UIMouseButton.Left)) {
            int charIndexAt = GetCharIndexAt(p);
            if (charIndexAt != cursorIndex) {
                cursorIndex = charIndexAt;
                cursorShown = true;
                Invalidate();
                p.Use();
                selectionStart = Mathf.Min(mouseSelectionAnchor, charIndexAt);
                selectionEnd = Mathf.Max(mouseSelectionAnchor, charIndexAt);
                return;
            }
        }
        base.OnMouseMove(p);
    }

    private int GetCharIndexAt(UIMouseEventParameter p) {
        Vector2 hitPosition = GetHitPosition(p);
        float num = PixelsToUnits();
        int num2;
        if (Multiline) {
            int lineByVerticalPosition = GetLineByVerticalPosition(hitPosition.y);
            num2 = GetIndexByHorizontalPosition(hitPosition.x, lineByVerticalPosition);
        } else {
            num2 = scrollIndex;
            float num3 = leftOffset / num + TextPadding.left;
            for (int i = scrollIndex; i < charWidths.Length; i++) {
                num3 += charWidths[i] / num;
                if (num3 < hitPosition.x) {
                    num2++;
                }
            }
        }
        return num2;
    }
    private IEnumerator MakeCursorBlink() {
        if (Application.isPlaying) {
            cursorShown = true;
            while (hasFocus) {
                yield return new WaitForSeconds(CursorBlinkTime);
                cursorShown = !cursorShown;
                Invalidate();
            }
            cursorShown = false;
        }
        yield break;
    }
    public void ClearSelection() {
        selectionStart = 0;
        selectionEnd = 0;
        mouseSelectionAnchor = 0;
    }
    public void MoveToStart() {
        ClearSelection();
        SetCursorPos(0);
    }
    public void MoveToEnd() {
        ClearSelection();
        SetCursorPos(text.Length);
    }
    public void MoveToNextChar() {
        ClearSelection();
        SetCursorPos(cursorIndex + 1);
    }
    public void MoveToPreviousChar() {
        ClearSelection();
        SetCursorPos(cursorIndex - 1);
    }
    public void MoveToNextWord() {
        ClearSelection();
        if (cursorIndex == text.Length) {
            return;
        }
        int cursorPos = FindNextWord(cursorIndex);
        SetCursorPos(cursorPos);
    }
    public void MoveToPreviousWord() {
        ClearSelection();
        if (cursorIndex == 0) {
            return;
        }
        int cursorPos = FindPreviousWord(cursorIndex);
        SetCursorPos(cursorPos);
    }
    public int FindPreviousWord(int startIndex) {
        int i;
        for (i = startIndex; i > 0; i--) {
            char c = text[i - 1];
            if (!char.IsWhiteSpace(c) && !char.IsSeparator(c) && !char.IsPunctuation(c)) {
                break;
            }
        }
        for (int j = i; j >= 0; j--) {
            if (j == 0) {
                i = 0;
                break;
            }
            char c2 = text[j - 1];
            if (char.IsWhiteSpace(c2) || char.IsSeparator(c2) || char.IsPunctuation(c2)) {
                i = j;
                break;
            }
        }
        return i;
    }
    public int FindNextWord(int startIndex) {
        int length = text.Length;
        int i = startIndex;
        for (int j = i; j < length; j++) {
            char c = text[j];
            if (char.IsWhiteSpace(c) || char.IsSeparator(c) || char.IsPunctuation(c)) {
                i = j;
                while (i < length) {
                    char c2 = text[i];
                    if (!char.IsWhiteSpace(c2) && !char.IsSeparator(c2) && !char.IsPunctuation(c2)) {
                        break;
                    }
                    i++;
                }
                return i;
            }
            if (j == length - 1) {
                i = length;
            }
        }
        while (i < length) {
            char c2 = text[i];
            if (!char.IsWhiteSpace(c2) && !char.IsSeparator(c2) && !char.IsPunctuation(c2)) {
                break;
            }
            i++;
        }
        return i;
    }
    public void MoveSelectionPointRightWord() {
        if (cursorIndex == text.Length) {
            return;
        }
        int num = FindNextWord(cursorIndex);
        if (selectionEnd == selectionStart) {
            selectionStart = cursorIndex;
            selectionEnd = num;
        } else if (selectionEnd == cursorIndex) {
            selectionEnd = num;
        } else if (selectionStart == cursorIndex) {
            selectionStart = num;
        }
        SetCursorPos(num);
    }
    public void MoveSelectionPointLeftWord() {
        if (cursorIndex == 0) {
            return;
        }
        int num = FindPreviousWord(cursorIndex);
        if (selectionEnd == selectionStart) {
            selectionEnd = cursorIndex;
            selectionStart = num;
        } else if (selectionEnd == cursorIndex) {
            selectionEnd = num;
        } else if (selectionStart == cursorIndex) {
            selectionStart = num;
        }
        SetCursorPos(num);
    }
    public void MoveSelectionPointRight() {
        if (cursorIndex == text.Length) {
            return;
        }
        if (selectionEnd == selectionStart) {
            selectionEnd = cursorIndex + 1;
            selectionStart = cursorIndex;
        } else if (selectionEnd == cursorIndex) {
            selectionEnd++;
        } else if (selectionStart == cursorIndex) {
            selectionStart++;
        }
        SetCursorPos(cursorIndex + 1);
    }
    public void MoveSelectionPointLeft() {
        if (cursorIndex == 0) {
            return;
        }
        if (selectionEnd == selectionStart) {
            selectionEnd = cursorIndex;
            selectionStart = cursorIndex - 1;
        } else if (selectionEnd == cursorIndex) {
            selectionEnd--;
        } else if (selectionStart == cursorIndex) {
            selectionStart--;
        }
        SetCursorPos(cursorIndex - 1);
    }
    public void MoveToSelectionEnd() {
        ClearSelection();
        SetCursorPos(selectionEnd);
    }
    public void MoveToSelectionStart() {
        ClearSelection();
        SetCursorPos(selectionStart);
    }
    public void SelectAll() {
        selectionStart = 0;
        selectionEnd = text.Length;
        scrollIndex = 0;
        SetCursorPos(0);
    }
    public void SelectToStart() {
        if (cursorIndex == 0) {
            return;
        }
        if (selectionEnd == selectionStart) {
            selectionEnd = cursorIndex;
        } else if (selectionEnd == cursorIndex) {
            selectionEnd = selectionStart;
        }
        selectionStart = 0;
        SetCursorPos(0);
    }
    public void SelectToEnd() {
        if (cursorIndex == text.Length) {
            return;
        }
        if (selectionEnd == selectionStart) {
            selectionStart = cursorIndex;
        } else if (selectionStart == cursorIndex) {
            selectionStart = selectionEnd;
        }
        selectionEnd = text.Length;
        SetCursorPos(text.Length);
    }
    public void SelectWordAtIndex(int index) {
        if (text.Length == 0) {
            return;
        }
        index = Mathf.Max(Mathf.Min(text.Length - 1, index), 0);
        char c = text[index];
        if (!char.IsLetterOrDigit(c)) {
            selectionStart = index;
            selectionEnd = index + 1;
            mouseSelectionAnchor = 0;
        } else {
            selectionStart = index;
            int num = index;
            while (num > 0 && char.IsLetterOrDigit(text[num - 1])) {
                selectionStart--;
                num--;
            }
            selectionEnd = index;
            int num2 = index;
            while (num2 < text.Length && char.IsLetterOrDigit(text[num2])) {
                selectionEnd = num2 + 1;
                num2++;
            }
        }
        cursorIndex = selectionStart;
        Invalidate();
    }
    private void CutSelectionToClipboard() {
        CopySelectionToClipboard();
        DeleteSelection();
    }
    private void CopySelectionToClipboard() {
        if (selectionStart == selectionEnd) {
            return;
        }
        Clipboard.text = text.Substring(selectionStart, SelectionLength);
    }
    private void PasteAtCursor(string clipData) {
        DeleteSelection();
        StringBuilder stringBuilder = new(text.Length + clipData.Length);
        stringBuilder.Append(text);
        for (int i = 0; i < clipData.Length; i++) {
            char c = clipData[i];
            if (c >= ' ') {
                stringBuilder.Insert(cursorIndex++, c);
            }
        }
        stringBuilder.Length = Mathf.Min(stringBuilder.Length, TextMaxLength);
        text = stringBuilder.ToString();
        OnTextChanged();
        SetCursorPos(cursorIndex);
    }
    private void SetCursorPos(int index) {
        index = Mathf.Max(0, Mathf.Min(text.Length, index));
        if (index == cursorIndex) {
            return;
        }
        cursorIndex = index;
        cursorShown = hasFocus;
        scrollIndex = Mathf.Min(scrollIndex, cursorIndex);
        Invalidate();
    }
    private void DeleteSelection() {
        if (selectionStart == selectionEnd) {
            return;
        }
        text = text.Remove(selectionStart, SelectionLength);
        SetCursorPos(selectionStart);
        ClearSelection();
        OnTextChanged();
        Invalidate();
    }
    private void DeleteNextChar() {
        ClearSelection();
        if (cursorIndex >= text.Length) {
            return;
        }
        text = text.Remove(cursorIndex, 1);
        cursorShown = true;
        OnTextChanged();
        Invalidate();
    }
    private void DeletePreviousChar() {
        if (selectionStart != selectionEnd) {
            DeleteSelection();
            SetCursorPos(selectionStart);
            return;
        }
        ClearSelection();
        if (cursorIndex == 0) {
            return;
        }
        text = text.Remove(cursorIndex - 1, 1);
        cursorIndex--;
        cursorShown = true;
        OnTextChanged();
        Invalidate();
    }
    private void DeleteNextWord() {
        ClearSelection();
        if (cursorIndex == text.Length) {
            return;
        }
        int num = FindNextWord(cursorIndex);
        if (num == cursorIndex) {
            num = text.Length;
        }
        text = text.Remove(cursorIndex, num - cursorIndex);
        OnTextChanged();
        Invalidate();
    }
    private void DeletePreviousWord() {
        ClearSelection();
        if (cursorIndex == 0) {
            return;
        }
        int num = FindPreviousWord(cursorIndex);
        if (num == cursorIndex) {
            num = 0;
        }
        text = text.Remove(num, cursorIndex - num);
        OnTextChanged();
        SetCursorPos(num);
    }
    public override void OnEnable() {
        TextPadding ??= new RectOffset();
        base.OnEnable();
        if (size.magnitude == 0f) {
            size = new Vector2(100f, 20f);
        }
        cursorShown = false;
        cursorIndex = (scrollIndex = (lineScrollIndex = 0));
        bool flag = font != null && font.isValid;
        if (Application.isPlaying && !flag) {
            font = GetUIView().defaultFont;
        }
    }
    public override void Update() {
        base.Update();
        if (!string.IsNullOrEmpty(Input.compositionString)) {
            Invalidate();
        }
    }
    protected override void OnRebuildRenderData() {
        if (atlas == null || font == null || !font.isValid) {
            return;
        }
        if (textRenderData != null) {
            textRenderData.Clear();
        } else {
            textRenderData = UIRenderData.Obtain();
            m_RenderData.Add(textRenderData);
        }
        textRenderData.material = atlas.material;
        renderData.material = atlas.material;
        RenderBackground();
        WrapText();
        RenderText();
    }
    private string PasswordDisplayText(string text) => new(PasswordCharacter[0], text.Length);
    protected virtual void RenderBackground() {
        if (!RenderBg || Atlas is null) {
            return;
        }
        var spriteInfo = GetBgSprite();
        if (spriteInfo is null) {
            return;
        }
        Color32 color = ApplyOpacity(GetBgActiveColor());
        if (spriteInfo is null) {
            return;
        }
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
            pixelsToUnits = PixelsToUnits(),
            size = size,
            spriteInfo = spriteInfo
        };
        if (spriteInfo.isSliced) {
            UISlicedSpriteRender.RenderSprite(renderData, options);
            return;
        }
        UISpriteRender.RenderSprite(renderData, options);
    }
    protected virtual UITextureAtlas.SpriteInfo GetBgSprite() => Atlas?[BgSprites.GetSprite(State)];
    protected virtual Color32 GetBgActiveColor() => BgSprites.GetColor(State);
    private void RenderText() {
        float num = PixelsToUnits();
        Vector2 vector = new(size.x - TextPadding.horizontal, size.y - TextPadding.vertical);
        Vector3 vector2 = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 vector3 = new Vector3(vector2.x + TextPadding.left, vector2.y - TextPadding.top, 0f) * num;
        string text = this.text + CompositionString;
        string text2 = (IsPasswordField && !string.IsNullOrEmpty(PasswordCharacter)) ? PasswordDisplayText(text) : text;
        Color32 defaultColor = isEnabled ? textNormalColor : textDisabledColor;
        float textScaleMultiplier = GetTextScaleMultiplier();
        Vector2 vector4 = vector * num;
        int num2 = 0;
        if (Multiline) {
            cursorIndex = Mathf.Min(cursorIndex, text2.Length);
            scrollIndex = 0;
            int num3 = Mathf.Max(1, Mathf.CeilToInt((size.y - TextPadding.vertical) / lineHeight) - 1);
            int lineByIndex = GetLineByIndex(cursorIndex, true);
            lineScrollIndex = Mathf.Min(lineScrollIndex, lineByIndex);
            lineScrollIndex = Mathf.Max(lineScrollIndex, lineByIndex - (num3 - 1));
            num2 = Mathf.Min(lineScrollIndex + num3 - 1, lines.Count - 1);
        } else {
            cursorIndex = Mathf.Min(cursorIndex, text2.Length);
            scrollIndex = Mathf.Min(Mathf.Min(scrollIndex, cursorIndex), text2.Length);
            lineScrollIndex = 0;
            leftOffset = 0f;
            if (textHorizontalAlignment == UIHorizontalAlignment.Left) {
                float num4 = 0f;
                for (int i = scrollIndex; i < cursorIndex; i++) {
                    num4 += charWidths[i];
                }
                while (num4 >= vector4.x) {
                    if (scrollIndex >= cursorIndex) {
                        break;
                    }
                    num4 -= charWidths[scrollIndex++];
                }
            } else {
                scrollIndex = Mathf.Max(0, Mathf.Min(cursorIndex, text2.Length - 1));
                float num5 = 0f;
                float num6 = font.size * 1.25f * num;
                while (scrollIndex > 0 && num5 < vector4.x - num6) {
                    num5 += charWidths[scrollIndex--];
                }
                float num7 = (text2.Length > 0) ? TextWidth(scrollIndex, text.Length) : 0f;
                switch (textHorizontalAlignment) {
                    case UIHorizontalAlignment.Center:
                        leftOffset = Mathf.Max(0f, (vector4.x - num7) * 0.5f);
                        break;
                    case UIHorizontalAlignment.Right:
                        leftOffset = Mathf.Max(0f, vector4.x - num7);
                        break;
                }
            }
        }
        if (selectionEnd != selectionStart) {
            RenderSelection();
        }
        for (int j = lineScrollIndex; j <= num2; j++) {
            using UIFontRenderer uifontRenderer = font.ObtainRenderer();
            uifontRenderer.wordWrap = false;
            uifontRenderer.maxSize = vector;
            uifontRenderer.pixelRatio = num;
            uifontRenderer.textScale = textScale * textScaleMultiplier;
            uifontRenderer.characterSpacing = characterSpacing;
            uifontRenderer.vectorOffset = vector3;
            uifontRenderer.multiLine = false;
            uifontRenderer.textAlign = UIHorizontalAlignment.Left;
            uifontRenderer.processMarkup = processMarkup;
            uifontRenderer.colorizeSprites = colorizeSprites;
            uifontRenderer.defaultColor = defaultColor;
            uifontRenderer.bottomColor = useTextGradient ? new Color32?(GetGradientBottomColorForState()) : default;
            uifontRenderer.overrideMarkupColors = false;
            uifontRenderer.opacity = CalculateOpacity();
            uifontRenderer.outline = useOutline;
            uifontRenderer.outlineSize = outlineSize;
            uifontRenderer.outlineColor = outlineColor;
            uifontRenderer.shadow = useDropShadow;
            uifontRenderer.shadowColor = dropShadowColor;
            uifontRenderer.shadowOffset = dropShadowOffset;
            if (Multiline) {
                string text3 = text2.Substring(lines[j], LineLenght(j));
                Vector3 b = new(CalculateLineLeftOffset(j), (float)-(float)(j - lineScrollIndex) * lineHeight * num);
                Vector3 vectorOffset = vector3 + b;
                uifontRenderer.vectorOffset = vectorOffset;
                uifontRenderer.Render(text3, textRenderData);
            } else {
                vector3.x += leftOffset;
                uifontRenderer.vectorOffset = vector3;
                uifontRenderer.Render(text2.Substring(scrollIndex), textRenderData);
            }
        }
        if (cursorShown && selectionEnd == selectionStart) {
            RenderCursor();
        }
    }
    private void RenderSelection() {
        if (string.IsNullOrEmpty(SelectionSprite) || atlas is null || atlas[SelectionSprite] is null) {
            return;
        }
        float num = PixelsToUnits();
        Vector3 vector = (pivot.TransformToUpperLeft(size, arbitraryPivotOffset) + new Vector3(TextPadding.left, (float)-(float)TextPadding.top)) * num;
        Vector3 vector2 = new Vector3(size.x - TextPadding.horizontal, size.y - TextPadding.vertical) * num;
        int num2 = Mathf.Max(GetLineByIndex(selectionStart, false), lineScrollIndex);
        int b = Mathf.Min(Mathf.FloorToInt((size.y - TextPadding.vertical) / lineHeight) + lineScrollIndex, lines.Count - 1);
        int num3 = Mathf.Min(GetLineByIndex(selectionEnd, false), b);
        using UIFontRenderer uIFontRenderer = font.ObtainRenderer();
        var textHeight = uIFontRenderer.MeasureString(Text).y;
        for (int i = num2; i <= num3; i++) {
            float num4 = CalculateLineLeftOffset(i);
            int begin = lines[i] + scrollIndex;
            float left;
            if (selectionStart < lines[i]) {
                left = vector.x + num4;
            } else {
                left = vector.x + num4 + TextWidth(begin, selectionStart);
            }
            float num5 = vector.y - (i - lineScrollIndex) * lineHeight * num;
            float bottom = CustomCursorHeight ? (vector.y - CursorHeight * num) : (Multiline ? Mathf.Max(vector.y - vector2.y, num5 - lineHeight * num) : (vector.y - textHeight * num));
            float right;
            if (selectionEnd > lines[i] + LineLenght(i)) {
                right = vector.x + num4 + TextWidth(begin, lines[i] + LineLenght(i));
            } else {
                right = Mathf.Min(vector.x + num4 + TextWidth(begin, selectionEnd), vector.x + vector2.x);
            }
            RenderSelectionBox(left, num5, right, bottom);
        }
    }
    private void RenderSelectionBox(float left, float top, float right, float bottom) {
        AddTriangles(renderData.triangles, renderData.vertices.Count);
        Vector3 item = new(left, top);
        Vector3 item2 = new(right, top);
        Vector3 item3 = new(left, bottom);
        Vector3 item4 = new(right, bottom);
        renderData.vertices.Add(item);
        renderData.vertices.Add(item2);
        renderData.vertices.Add(item4);
        renderData.vertices.Add(item3);
        Color32 item5 = ApplyOpacity(SelectionBgColor);
        renderData.colors.Add(item5);
        renderData.colors.Add(item5);
        renderData.colors.Add(item5);
        renderData.colors.Add(item5);
        UITextureAtlas.SpriteInfo spriteInfo = atlas[SelectionSprite];
        Rect region = spriteInfo.region;
        float num = region.width / spriteInfo.pixelSize.x;
        float num2 = region.height / spriteInfo.pixelSize.y;
        renderData.uvs.Add(new Vector2(region.x + num, region.yMax - num2));
        renderData.uvs.Add(new Vector2(region.xMax - num, region.yMax - num2));
        renderData.uvs.Add(new Vector2(region.xMax - num, region.y + num2));
        renderData.uvs.Add(new Vector2(region.x + num, region.y + num2));
    }
    private void RenderCursor() {
        if (string.IsNullOrEmpty(SelectionSprite) || atlas is null) {
            return;
        }
        float num = PixelsToUnits();
        using UIFontRenderer uIFontRenderer = font.ObtainRenderer();
        Vector3 b = m_Pivot.TransformToUpperLeft(size, arbitraryPivotOffset) * num;
        int lineByIndex = GetLineByIndex(cursorIndex, true);
        float num2 = (float)-(float)TextPadding.top * num - (lineByIndex - lineScrollIndex) * lineHeight * num;
        float num3 = (CalculateLineLeftOffset(lineByIndex) + TextWidth(lines[lineByIndex] + scrollIndex, cursorIndex) + TextPadding.left * num).Quantize(num);
        float num4 = num * GetUIView().ratio * CursorWidth;
        float num5 = CustomCursorHeight ? (CursorHeight * num) : (Multiline ? Mathf.Min(lineHeight * num, (size.y - TextPadding.vertical) * num) : (uIFontRenderer.MeasureString(Text).y * num));
        Vector3 tl = new Vector3(num3, num2) + b;
        Vector3 tr = new Vector3(num3 + num4, num2) + b;
        Vector3 bl = new Vector3(num3, num2 - num5) + b;
        Vector3 br = new Vector3(num3 + num4, num2 - num5) + b;
        RenderCursorAt(tl, tr, bl, br);
    }
    private void RenderCursorAt(Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br) {
        PoolList<Vector3> vertices = renderData.vertices;
        PoolList<int> triangles = renderData.triangles;
        PoolList<Vector2> uvs = renderData.uvs;
        PoolList<Color32> colors = renderData.colors;
        AddTriangles(triangles, vertices.Count);
        vertices.Add(tl);
        vertices.Add(tr);
        vertices.Add(br);
        vertices.Add(bl);
        Color32 item = ApplyOpacity(textNormalColor);
        colors.Add(item);
        colors.Add(item);
        colors.Add(item);
        colors.Add(item);
        UITextureAtlas.SpriteInfo spriteInfo = atlas[SelectionSprite];
        Rect region = spriteInfo.region;
        uvs.Add(new Vector2(region.x, region.yMax));
        uvs.Add(new Vector2(region.xMax, region.yMax));
        uvs.Add(new Vector2(region.xMax, region.y));
        uvs.Add(new Vector2(region.x, region.y));
    }
    private void AddTriangles(PoolList<int> triangles, int baseIndex) {
        for (int i = 0; i < UISpriteRender.kTriangleIndices.Length; i++) {
            triangles.Add(UISpriteRender.kTriangleIndices[i] + baseIndex);
        }
    }
    private void WrapText() {
        lineHeight = font.lineHeight;
        float d = PixelsToUnits();
        Vector2 maxSize = new(size.x - TextPadding.horizontal, size.y - TextPadding.vertical);
        Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        Vector3 vectorOffset = new Vector3(vector.x + TextPadding.left, vector.y - TextPadding.top, 0f) * d;
        string text = this.text + CompositionString;
        string text2 = (IsPasswordField && !string.IsNullOrEmpty(PasswordCharacter)) ? PasswordDisplayText(text) : text;
        Color32 defaultColor = isEnabled ? textNormalColor : textDisabledColor;
        float textScaleMultiplier = GetTextScaleMultiplier();
        using (UIFontRenderer uifontRenderer = font.ObtainRenderer()) {
            uifontRenderer.wordWrap = false;
            uifontRenderer.maxSize = maxSize;
            uifontRenderer.pixelRatio = PixelsToUnits();
            uifontRenderer.textScale = textScale * textScaleMultiplier;
            uifontRenderer.characterSpacing = characterSpacing;
            uifontRenderer.vectorOffset = vectorOffset;
            uifontRenderer.multiLine = false;
            uifontRenderer.textAlign = UIHorizontalAlignment.Left;
            uifontRenderer.processMarkup = processMarkup;
            uifontRenderer.colorizeSprites = colorizeSprites;
            uifontRenderer.defaultColor = defaultColor;
            uifontRenderer.bottomColor = (useTextGradient ? gradientBottomNormalColor : gradientBottomDisabledColor);
            uifontRenderer.overrideMarkupColors = false;
            uifontRenderer.opacity = CalculateOpacity();
            uifontRenderer.outline = useOutline;
            uifontRenderer.outlineSize = outlineSize;
            uifontRenderer.outlineColor = outlineColor;
            uifontRenderer.shadow = useDropShadow;
            uifontRenderer.shadowColor = dropShadowColor;
            uifontRenderer.shadowOffset = dropShadowOffset;
            charWidths = uifontRenderer.GetCharacterWidths(text2);
            lineHeight = font.lineHeight;
        }
        if (Multiline) {
            lines = CalculateLineBreaks(GetWords());
            return;
        }
        lines = new List<int> { 0 };
    }
    private List<int> CalculateLineBreaks(List<int> words) {
        List<int> list = new() { 0 };
        int num = 0;
        float num2 = (size.x - TextPadding.horizontal) * PixelsToUnits();
        int count = words.Count;
        int num3 = 0;
        while (num3 < count && words[num3] != text.Length) {
            int num4 = (num3 == count - 1) ? text.Length : words[num3 + 1];
            if (text[words[num3]] == '\n') {
                list.Add(words[num3] + 1);
                num++;
            } else if (TextWidth(list[num], num4) >= num2) {
                if (words[num3] != list[num]) {
                    list.Add(words[num3]);
                    num++;
                }
                int num5 = list[num];
                for (int i = num5; i < num4; i++) {
                    if (TextWidth(list[num], i + 1) >= num2) {
                        list.Add(i);
                        num++;
                    }
                }
            }
            num3++;
        }
        return list;
    }
    private List<int> GetWords() {
        List<int> list = new() { 0 };
        int num = 0;
        bool flag = false;
        while (!flag) {
            int num2 = FindNextWord(num);
            for (int i = FindNextLineBreak(num); i < num2; i = FindNextLineBreak(i + 1)) {
                if (i != 0) {
                    list.Add(i);
                }
            }
            if (num2 == text.Length) {
                flag = true;
            } else {
                list.Add(num2);
                num = num2;
            }
        }
        return list;
    }
    private int FindNextLineBreak(int start) {
        int num = start;
        while (num < text.Length && text[num] != '\n') {
            num++;
        }
        return num;
    }
    private int LineLenght(int line) {
        int count = lines.Count;
        if (line < 0 || line >= count) {
            return 0;
        }
        if (line == count - 1) {
            return text.Length - lines[line];
        }
        return lines[line + 1] - lines[line];
    }
    private float TextWidth(int begin, int end) {
        if (begin < 0 || end > text.Length || end <= begin) {
            return 0f;
        }
        float num = 0f;
        int num2 = begin;
        while (num2 < end && num2 != text.Length) {
            num += charWidths[num2];
            num2++;
        }
        return num;
    }
    private int GetLineByIndex(int index, bool cursor = false) {
        int count = lines.Count;
        if (index == text.Length) {
            return count - 1;
        }
        int num = 0;
        while (num < count && index >= lines[num] + LineLenght(num)) {
            num++;
        }
        if (cursor && cursorAtEndOfLine && index == lines[num] && LineLenght(num) > 0) {
            return Mathf.Max(0, num - 1);
        }
        cursorAtEndOfLine = false;
        return num;
    }
    private float CalculateLineLeftOffset(int line) {
        if (!Multiline) {
            return leftOffset;
        }
        float num = TextWidth(lines[line], lines[line] + LineLenght(line));
        float num2 = (size.x - TextPadding.horizontal) * PixelsToUnits();
        float result = 0f;
        switch (textHorizontalAlignment) {
            case UIHorizontalAlignment.Left:
                result = 0f;
                break;
            case UIHorizontalAlignment.Center:
                result = Mathf.Max(0f, (num2 - num) * 0.5f);
                break;
            case UIHorizontalAlignment.Right:
                result = Mathf.Max(0f, num2 - num);
                break;
        }
        return result;
    }
    public void MoveToUpChar() {
        ClearSelection();
        SetCursorPos(FindUpperIndex(cursorIndex));
    }
    public void MoveToDownChar() {
        ClearSelection();
        SetCursorPos(FindLowerIndex(cursorIndex));
    }
    public void MoveSelectionPointDown() {
        int num = FindLowerIndex(cursorIndex);
        if (selectionEnd == selectionStart) {
            selectionEnd = num;
            selectionStart = cursorIndex;
        } else if (selectionEnd == cursorIndex) {
            selectionEnd = num;
        } else if (selectionStart == cursorIndex) {
            if (num <= selectionEnd) {
                selectionStart = num;
            } else {
                selectionStart = selectionEnd;
                selectionEnd = num;
            }
        }
        SetCursorPos(num);
    }
    public void MoveSelectionPointUp() {
        int num = FindUpperIndex(cursorIndex);
        if (selectionEnd == selectionStart) {
            selectionStart = num;
            selectionEnd = cursorIndex;
        } else if (selectionStart == cursorIndex) {
            selectionStart = num;
        } else if (selectionEnd == cursorIndex) {
            if (num >= selectionStart) {
                selectionEnd = num;
            } else {
                selectionEnd = selectionStart;
                selectionStart = num;
            }
        }
        SetCursorPos(num);
    }
    private int FindLowerIndex(int index) {
        int lineByIndex = GetLineByIndex(index, true);
        if (lineByIndex >= lines.Count - 1) {
            return text.Length;
        }
        return GetIndexByHorizontalPosition(GetHorizontalPositionByIndex(index), lineByIndex + 1);
    }
    private int FindUpperIndex(int index) {
        int lineByIndex = GetLineByIndex(index, true);
        if (lineByIndex <= 0) {
            return 0;
        }
        return GetIndexByHorizontalPosition(GetHorizontalPositionByIndex(index), lineByIndex - 1);
    }
    private int GetIndexByHorizontalPosition(float position, int line) {
        if (line < 0) {
            return 0;
        }
        if (line >= lines.Count) {
            return text.Length;
        }
        float num = PixelsToUnits();
        float num2 = TextPadding.left + CalculateLineLeftOffset(line) / num;
        int i = lines[line];
        int num3 = lines[line] + LineLenght(line);
        while (i < num3) {
            num2 += charWidths[i] / num;
            if (num2 > position || text[i] == '\n') {
                break;
            }
            i++;
        }
        if (i == num3) {
            cursorAtEndOfLine = true;
        }
        return i;
    }
    private float GetHorizontalPositionByIndex(int index) {
        int lineByIndex = GetLineByIndex(index, false);
        float num = PixelsToUnits();
        float num2 = CalculateLineLeftOffset(lineByIndex) / num + TextPadding.left;
        for (int i = lines[lineByIndex]; i < index; i++) {
            num2 += charWidths[i] / num;
        }
        return num2;
    }
    private void AddLineBreak() {
        if (text.Length < TextMaxLength) {
            DeleteSelection();
            if (cursorIndex == text.Length) {
                text += '\n';
            } else {
                text = text.Insert(cursorIndex, '\n'.ToString());
            }
            cursorIndex++;
            OnTextChanged();
            Invalidate();
        }
    }
    private int GetLineByVerticalPosition(float position) {
        int a = lineScrollIndex + Mathf.FloorToInt((position - TextPadding.top) / lineHeight) + scrollIndex;
        a = Mathf.Min(a, lines.Count - 1);
        return Mathf.Max(a, 0);
    }

    public class UndoData {
        public string Text;
        public int Position;
    }
}

