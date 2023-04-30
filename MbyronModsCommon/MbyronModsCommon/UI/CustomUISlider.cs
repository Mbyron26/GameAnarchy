using ColossalFramework;
using ColossalFramework.UI;
using System;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class CustomUISlider : UIComponent {
    protected UITextureAtlas atlas;
    protected string bgSprite;
    protected float minValue;
    protected float maxValue = 100f;
    protected float rawValue = 10f;
    protected float stepSize = 1f;
    protected bool canWheel = false;
    protected float wheelStep = 1f;
    protected UIComponent thumbObject;
    protected Vector2 thumbOffset;
    protected RectOffset thumbPadding;
    protected UIComponent fillIndicatorObject;
    protected RectOffset fillPadding;
    protected UIFillMode fillMode = UIFillMode.Fill;
    protected UIOrientation orientation;

    public event Action<float> EventValueChanged;

    public UITextureAtlas Atlas {
        get {
            if (atlas is null) {
                var uiview = GetUIView();
                if (uiview is not null) {
                    atlas = uiview.defaultAtlas;
                }
            }
            return atlas;
        }
        set {
            if (!Equals(value, atlas)) {
                atlas = value;
                Invalidate();
            }
        }
    }
    public string BgSprite {
        get => bgSprite;
        set {
            if (value != bgSprite) {
                bgSprite = value;
                Invalidate();
            }
        }
    }
    public float MinValue {
        get => minValue;
        set {
            if (value != minValue) {
                minValue = value;
                if (rawValue < value) {
                    minValue = value;
                }
                Invalidate();
            }
        }
    }
    public float MaxValue {
        get => maxValue;
        set {
            if (value != maxValue) {
                maxValue = value;
                if (rawValue > value) {
                    RawValue = value;
                }
                Invalidate();
            }
        }
    }
    public float RawValue {
        get => rawValue;
        set {

            value = Mathf.Max(MinValue, Mathf.Min(MaxValue, value)).Quantize(StepSize);
            if (!Mathf.Approximately(value, rawValue)) {
                rawValue = value;
                OnRawValueChanged();
            }
        }
    }
    public float StepSize {
        get {
            return stepSize;
        }
        set {
            value = Mathf.Max(0f, value);
            if (value != stepSize) {
                stepSize = value;
                RawValue = rawValue.Quantize(value);
                Invalidate();
            }
        }
    }
    public bool CanWheel {
        get => canWheel;
        set {
            if (canWheel != value) {
                canWheel = value;
                Invalidate();
            }
        }
    }
    public float WheelStep {
        get => wheelStep;
        set {
            value = Mathf.Max(0f, value);
            if (value != wheelStep) {
                wheelStep = value;
                Invalidate();
            }
        }
    }
    public UIOrientation Orientation {
        get => orientation;
        set {
            if (value != orientation) {
                orientation = value;
                Invalidate();
                UpdateValueIndicators(rawValue);
            }
        }
    }
    public UIComponent ThumbObject {
        get => thumbObject;
        set {
            if (value != thumbObject) {
                thumbObject = value;
                Invalidate();
                UpdateValueIndicators(rawValue);
            }
        }
    }
    public Vector2 ThumbOffset {
        get => thumbOffset;
        set {
            if (Vector2.Distance(value, thumbOffset) > 1E-45f) {
                thumbOffset = value;
                UpdateValueIndicators(rawValue);
            }
        }
    }
    public RectOffset ThumbPadding {
        get {
            thumbPadding ??= new RectOffset();
            return thumbPadding;
        }
        set {
            if (!Equals(value, thumbPadding)) {
                thumbPadding = value;
                UpdateValueIndicators(rawValue);
                Invalidate();
            }
        }
    }
    public UIComponent FillIndicatorObject {
        get => fillIndicatorObject;
        set {
            if (value != fillIndicatorObject) {
                fillIndicatorObject = value;
                Invalidate();
                UpdateValueIndicators(rawValue);
            }
        }
    }
    public RectOffset FillPadding {
        get {
            fillPadding ??= new RectOffset();
            return fillPadding;
        }
        set {
            if (!Equals(value, fillPadding)) {
                fillPadding = value;
                UpdateValueIndicators(rawValue);
                Invalidate();
            }
        }
    }
    public UIFillMode FillMode {
        get => fillMode;
        set {
            if (value != fillMode) {
                fillMode = value;
                Invalidate();
            }
        }
    }
    public override bool canFocus => (isEnabled && isVisible) || base.canFocus;

    public static CustomUISlider Add(UIComponent parent, Vector2 size, float minValue, float maxValue, float stepSize, float rawValue, Action<float> callback = null, bool defaultStyle = true) {
        var slider = parent.AddUIComponent<CustomUISlider>();
        slider.size = size;
        slider.MinValue = minValue;
        slider.MaxValue = maxValue;
        slider.StepSize = stepSize;
        slider.RawValue = rawValue;
        slider.EventValueChanged += callback;
        if (defaultStyle)
            slider.SetDefaultStyle();
        return slider;
    }
    protected virtual void OnRawValueChanged() {
        Invalidate();
        UpdateValueIndicators(rawValue);
        EventValueChanged?.Invoke(RawValue);
        InvokeUpward("OnValueChanged", new object[] { RawValue });
    }
    public void SetGradientStyle() {
        atlas = CustomUIAtlas.MbyronModsAtlas;
        bgSprite = CustomUIAtlas.GradientSlider;
        disabledColor = new Color32(130, 130, 130, 255);
        var thumbObject = AddUIComponent<UISprite>();
        thumbObject.atlas = CustomUIAtlas.MbyronModsAtlas;
        thumbObject.spriteName = CustomUIAtlas.Circle;
        thumbObject.color = new(220, 220, 220, 255);
        thumbObject.disabledColor = new Color32(80, 80, 80, 255);
        thumbObject.size = new Vector2(size.y + 4, size.y + 4);
        ThumbObject = thumbObject;
        ThumbPadding = new RectOffset((int)thumbObject.size.y / 2, (int)thumbObject.size.y / 2, 0, 0);
    }
    public void SetDefaultStyle() {
        Atlas = CustomUIAtlas.MbyronModsAtlas;
        BgSprite = CustomUIAtlas.RoundedRectangle3;
        color = CustomUIColor.OPButtonNormal;
        disabledColor = CustomUIColor.OPButtonDisabled;
        var fillIndicator = AddUIComponent<UISlicedSprite>();
        fillIndicator.atlas = CustomUIAtlas.MbyronModsAtlas;
        fillIndicator.spriteName = CustomUIAtlas.RoundedRectangle3;
        fillIndicator.color = CustomUIColor.BlueNormal;
        fillIndicator.disabledColor = CustomUIColor.BlueDisabled;
        fillIndicator.size = size;
        var thumbObject = AddUIComponent<UISprite>();
        thumbObject.atlas = CustomUIAtlas.MbyronModsAtlas;
        thumbObject.spriteName = CustomUIAtlas.Circle;
        thumbObject.color = new(220, 220, 220, 255);
        thumbObject.disabledColor = new Color32(72, 80, 96, 255);
        thumbObject.size = new Vector2(size.y + 4, size.y + 4);
        ThumbObject = thumbObject;
        ThumbPadding = new RectOffset((int)thumbObject.size.y / 2, (int)thumbObject.size.y / 2, 0, 0);
        FillIndicatorObject = fillIndicator;
    }

    private SteppingRate GetSteppingRate() {
        if (KeyHelper.IsShiftDown()) return SteppingRate.Fast;
        else if (KeyHelper.IsControlDown()) return SteppingRate.Slow;
        else return SteppingRate.Normal;
    }
    protected virtual float ValueDecrease(SteppingRate steppingRate) {
        var rate = GetStep(steppingRate);
        return (float)Math.Round(RawValue - rate, 1);
    }
    protected virtual float ValueIncrease(SteppingRate steppingRate) {
        var rate = GetStep(steppingRate);
        return (float)Math.Round(RawValue + rate, 1);
    }

    protected virtual float GetStep(SteppingRate steppingRate) => steppingRate switch {
        SteppingRate.Fast => WheelStep * 10,
        SteppingRate.Slow => WheelStep / 10,
        _ => WheelStep,
    };

    protected override void OnKeyDown(UIKeyEventParameter p) {
        if (builtinKeyNavigation) {
            if (Orientation == UIOrientation.Horizontal) {
                if (p.keycode == KeyCode.LeftArrow) {
                    RawValue -= WheelStep;
                    p.Use();
                    return;
                }
                if (p.keycode == KeyCode.RightArrow) {
                    RawValue += WheelStep;
                    p.Use();
                    return;
                }
            } else {
                if (p.keycode == KeyCode.UpArrow) {
                    RawValue -= WheelStep;
                    p.Use();
                    return;
                }
                if (p.keycode == KeyCode.DownArrow) {
                    RawValue += WheelStep;
                    p.Use();
                    return;
                }
            }
        }
        base.OnKeyDown(p);
    }

    protected override void OnMouseWheel(UIMouseEventParameter p) {
        if (canWheel) {
            p.Use();
            var typeRate = GetSteppingRate();
            ExternalLogger.Log(p.wheelDelta.ToString());
            if (p.wheelDelta < 0) {
                RawValue = ValueDecrease(typeRate);
            } else {
                RawValue = ValueIncrease(typeRate);
            }
            Invoke("OnMouseWheel", new object[] { p });
        }
    }

    protected override void OnMouseMove(UIMouseEventParameter p) {
        if (!p.buttons.IsFlagSet(UIMouseButton.Left)) {
            base.OnMouseMove(p);
            return;
        }
        RawValue = GetValueFromMouseEvent(p);
        p.Use();
        Invoke("OnMouseMove", new object[] { p });
    }

    protected override void OnMouseDown(UIMouseEventParameter p) {
        if (!p.buttons.IsFlagSet(UIMouseButton.Left)) {
            base.OnMouseMove(p);
            return;
        }
        Focus();
        RawValue = GetValueFromMouseEvent(p);
        p.Use();
        Invoke("OnMouseDown", new object[] { p });
    }

    protected override void OnSizeChanged() {
        base.OnSizeChanged();
        UpdateValueIndicators(rawValue);
    }

    public override void Start() {
        base.Start();
        UpdateValueIndicators(rawValue);
    }

    public override void OnEnable() {
        if (size.magnitude < 1E-45f) {
            size = new Vector2(100f, 25f);
        }
        base.OnEnable();
        UpdateValueIndicators(rawValue);
    }

    protected override void OnRebuildRenderData() {
        if (atlas == null) {
            return;
        }
        renderData.material = atlas.material;
        RenderBackground();
    }

    protected override void OnVisibilityChanged() {
        UpdateValueIndicators(rawValue);
    }

    protected virtual void RenderBackground() {
        if (atlas == null) {
            return;
        }
        UITextureAtlas.SpriteInfo spriteInfo = atlas[bgSprite];
        if (spriteInfo == null) {
            return;
        }
        Color32 color = ApplyOpacity(isEnabled ? base.color : disabledColor);
        RenderOptions options = new() {
            atlas = atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = pivot.TransformToUpperLeft(base.size, arbitraryPivotOffset),
            pixelsToUnits = PixelsToUnits(),
            size = base.size,
            spriteInfo = spriteInfo
        };
        if (spriteInfo.isSliced) {
            UISlicedSpriteRender.RenderSprite(renderData, options);
            return;
        }
        UISpriteRender.RenderSprite(renderData, options);
    }

    private void GetBoundingPoints(bool convertToWorld, out Vector3 start, out Vector3 end) {
        Vector3 vector = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        if (Orientation == UIOrientation.Vertical) {
            end = new Vector3(vector.x + size.x * 0.5f, vector.y);
            start = end - new Vector3(0f, size.y);
        } else {
            start = new Vector3(vector.x, vector.y - size.y * 0.5f);
            end = start + new Vector3(size.x, 0f);
        }
        if (convertToWorld) {
            float d = PixelsToUnits();
            Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
            start = localToWorldMatrix.MultiplyPoint(start * d);
            end = localToWorldMatrix.MultiplyPoint(end * d);
        }
    }

    protected virtual void UpdateValueIndicators(float rawValue) {
        if (ThumbObject != null) {
            GetBoundingPoints(true, out Vector3 vector, out Vector3 a);
            vector.x += ThumbPadding.left * PixelsToUnits();
            a.x -= ThumbPadding.right * PixelsToUnits();
            Vector3 vector2 = a - vector;
            float d = (rawValue - MinValue) / (MaxValue - MinValue) * vector2.magnitude;
            Vector3 b = ThumbOffset * PixelsToUnits();
            Vector3 position = vector + vector2.normalized * d + b;
            ThumbObject.pivot = UIPivotPoint.MiddleCenter;
            ThumbObject.transform.position = position;
            ThumbObject.ResetLayout();
        }
        if (FillIndicatorObject == null) {
            return;
        }
        RectOffset fillPadding = FillPadding;
        float num = (rawValue - MinValue) / (MaxValue - MinValue);
        Vector3 relativePosition = new(fillPadding.left, fillPadding.top);
        Vector2 size = base.size - new Vector2(fillPadding.horizontal, fillPadding.vertical);
        UISprite uisprite = FillIndicatorObject as UISprite;
        if (uisprite != null && FillMode == UIFillMode.Fill) {
            if (Orientation == UIOrientation.Horizontal) {
                uisprite.fillDirection = UIFillDirection.Horizontal;
            } else {
                uisprite.fillDirection = UIFillDirection.Vertical;
            }
            uisprite.fillAmount = num;
        } else if (Orientation == UIOrientation.Horizontal) {
            size.x = width * num - fillPadding.horizontal;
        } else {
            size.y = height * num - fillPadding.vertical;
        }
        FillIndicatorObject.size = size;
        FillIndicatorObject.relativePosition = relativePosition;
    }

    protected virtual float GetValueFromMouseEvent(UIMouseEventParameter p) {
        GetBoundingPoints(true, out Vector3 vector, out Vector3 vector2);
        Plane plane = new(transform.TransformDirection(Vector3.back), vector);
        Ray ray = p.ray;
        if (!plane.Raycast(ray, out float d)) {
            return rawValue;
        }
        Vector3 test = ray.origin + ray.direction * d;
        Vector3 a = IntersectionTest.ClosestPointOnLine(vector, vector2, test, true);
        float num = (a - vector).magnitude / (vector2 - vector).magnitude;
        return MinValue + (MaxValue - MinValue) * num;
    }
}
