using ColossalFramework.UI;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class CustomUIPanel : UIComponent {
    protected UITextureAtlas atlas;
    protected UIRenderData bgRenderData;
    protected UIRenderData fgRenderData;
    protected string bgSprite = string.Empty;
    protected bool renderFg;
    protected string fgSprite = string.Empty;
    protected RectOffset fgSpritePadding;
    protected ForegroundSpriteMode fgSpriteMode;
    protected float scaleFactor = 1f;
    protected Vector2 fgSize = Vector2.zero;
    protected Color32 bgNormalColor = Color.white;
    protected Color32 bgDisabledColor = Color.white;
    protected Color32 fgNormalColor = Color.white;
    protected Color32 fgDisabledColor = Color.white;
    protected UIHorizontalAlignment horizontalAlignment = UIHorizontalAlignment.Center;
    protected UIVerticalAlignment verticalAlignment = UIVerticalAlignment.Middle;
    protected RectOffset padding;
    protected float itemGap;
    protected bool autoLayout = false;
    protected LayoutDirection autoLayoutDirection = LayoutDirection.Vertical;
    protected LayoutStart autoLayoutStart;
    protected bool autoFitChildrenVertically = false;
    protected bool autoFitChildrenHorizontally = false;

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
            if (!value.Equals(atlas)) {
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
    public bool RenderFg {
        get => renderFg;
        set {
            if (value != renderFg) {
                renderFg = value;
                Invalidate();
            }
        }
    }
    public string FgSprite {
        get => fgSprite;
        set {
            if (value != fgSprite) {
                fgSprite = value;
                Invalidate();
            }
        }
    }
    public ForegroundSpriteMode FgSpriteMode {
        get => fgSpriteMode;
        set {
            if (value != fgSpriteMode) {
                fgSpriteMode = value;
                Invalidate();
            }
        }
    }
    public float ScaleFactor {
        get {
            return scaleFactor;
        }
        set {
            if (!Mathf.Approximately(value, scaleFactor)) {
                scaleFactor = value;
                Invalidate();
            }
        }
    }
    public Vector2 FgSize {
        get => fgSize;
        set {
            if (value != fgSize) {
                fgSize = value;
                Invalidate();
            }
        }
    }
    public Color32 BgNormalColor {
        get => bgNormalColor;
        set {
            if (!bgNormalColor.Equals(value)) {
                bgNormalColor = value;
                Invalidate();
            }
        }
    }
    public Color32 FgNormalColor {
        get => fgNormalColor;
        set {
            if (!fgNormalColor.Equals(value)) {
                fgNormalColor = value;
                Invalidate();
            }
        }
    }
    public Color32 BgDisabledColor {
        get => bgDisabledColor;
        set {
            if (!bgDisabledColor.Equals(value)) {
                bgDisabledColor = value;
                Invalidate();
            }
        }
    }
    public Color32 FgDisabledColor {
        get => fgDisabledColor;
        set {
            if (!fgDisabledColor.Equals(value)) {
                fgDisabledColor = value;
                Invalidate();
            }
        }
    }

    public virtual UIHorizontalAlignment HorizontalAlignment {
        get => horizontalAlignment;
        set {
            if (value != horizontalAlignment) {
                horizontalAlignment = value;
                Invalidate();
            }
        }
    }
    public virtual UIVerticalAlignment VerticalAlignment {
        get => verticalAlignment;
        set {
            if (value != verticalAlignment) {
                verticalAlignment = value;
                Invalidate();
            }
        }
    }
    public RectOffset FgSpritePadding {
        get {
            fgSpritePadding ??= new RectOffset();
            return fgSpritePadding;
        }
        set {
            if (!Equals(value, fgSpritePadding)) {
                fgSpritePadding = value;
                Invalidate();
            }
        }
    }
    public RectOffset Padding {
        get {
            padding ??= new RectOffset();
            return padding;
        }
        set {
            value = value.ConstrainPadding();
            if (!Equals(padding, value)) {
                padding = value;
                Invalidate();
                if (autoLayout) {
                    Arrange();
                }
            }
        }
    }
    public float ItemGap {
        get => itemGap;
        set {
            if (!Equals(value, itemGap)) {
                itemGap = value;
                Invalidate();
                Arrange();
            }
        }
    }
    public virtual bool AutoLayout {
        get => autoLayout;
        set {
            if (!Equals(value, autoLayout)) {
                autoLayout = value;
                Invalidate();
                Arrange();
            }
        }
    }
    public LayoutDirection AutoLayoutDirection {
        get => autoLayoutDirection;
        set {
            if (value != autoLayoutDirection) {
                autoLayoutDirection = value;
                Invalidate();
                Arrange();
            }
        }
    }
    public LayoutStart AutoLayoutStart {
        get => autoLayoutStart;
        set {
            if (value != autoLayoutStart) {
                autoLayoutStart = value;
                Invalidate();
            }
        }
    }
    public bool AutoFitChildrenVertically {
        get => autoFitChildrenVertically;
        set {
            if (value != autoFitChildrenVertically) {
                autoFitChildrenVertically = value;
                Invalidate();
                Arrange();
            }
        }
    }
    public bool AutoFitChildrenHorizontally {
        get => autoFitChildrenHorizontally;
        set {
            if (value != autoFitChildrenHorizontally) {
                autoFitChildrenHorizontally = value;
                Invalidate();
                Arrange();
            }
        }
    }

    public virtual void Arrange() {
        if (!autoLayout) {
            return;
        }
        if (m_CachedChildCount == 0) {
            return;
        }
        if (AutoLayoutStart.StartsAtRight()) {
            var offset0 = width - Padding.right;
            var count = 0;
            if (autoLayoutDirection == LayoutDirection.Vertical) {
                float offset1 = Padding.top;
                for (int i = 0; i < m_CachedChildCount; i++) {
                    if (m_ChildComponents[i].isVisible && m_ChildComponents[i].gameObject.activeSelf) {
                        count++;
                        if (m_ChildComponents[i].name != "DummyBackground") {
                            offset0 -= m_ChildComponents[i].size.x;
                            m_ChildComponents[i].relativePosition = new Vector2(offset0 - m_ChildComponents[i].width, offset1);
                            offset1 += m_ChildComponents[i].height;
                            if (m_CachedChildCount - 1 != i) {
                                offset1 += ItemGap;
                            }
                        }
                    }
                }
            } else {
                for (int i = 0; i < m_CachedChildCount; i++) {
                    if (m_ChildComponents[i].isVisible && m_ChildComponents[i].gameObject.activeSelf) {
                        if (m_ChildComponents[i].name != "DummyBackground") {
                            offset0 -= m_ChildComponents[i].width;
                            m_ChildComponents[i].relativePosition = new Vector2(offset0, Padding.top);
                            if (m_CachedChildCount - 1 != i) {
                                offset0 -= ItemGap;
                            }
                        }
                    }
                }
            }
        } else {
            float offset2 = Padding.left;
            float offset3 = Padding.top;
            if (autoLayoutDirection == LayoutDirection.Vertical) {
                for (int i = 0; i < m_CachedChildCount; i++) {
                    if (m_ChildComponents[i].isVisible && m_ChildComponents[i].gameObject.activeSelf) {
                        if (m_ChildComponents[i].name != "DummyBackground") {
                            m_ChildComponents[i].relativePosition = new Vector2(offset2, offset3);
                            offset3 += m_ChildComponents[i].height;
                            if (m_CachedChildCount - 1 != i) {
                                offset3 += ItemGap;
                            }
                        }
                    }
                }
            } else {
                for (int i = 0; i < m_CachedChildCount; i++) {
                    if (m_ChildComponents[i].isVisible && m_ChildComponents[i].gameObject.activeSelf) {
                        if (m_ChildComponents[i].name != "DummyBackground") {
                            m_ChildComponents[i].relativePosition = new Vector2(offset2, offset3);
                            offset2 += m_ChildComponents[i].width;
                            if (m_CachedChildCount - 1 != i) {
                                offset2 += ItemGap;
                            }
                        }
                    }
                }
            }
        }
        if (autoFitChildrenHorizontally && autoFitChildrenVertically) {
            FitChildrenVertically();
            FitChildrenHorizontally();
        } else if (autoFitChildrenHorizontally) {
            FitChildrenHorizontally();
        } else if (autoFitChildrenVertically) {
            FitChildrenVertically();
        }
    }
    public new void FitChildrenVertically() => FitChildrenVertically(ItemGap);
    public new void FitChildrenVertically(float itemGap) {
        if (autoLayoutDirection == LayoutDirection.Horizontal) {
            float height = default;
            for (int i = 0; i < childCount; i++) {
                if (components[i].isVisibleSelf) {
                    height = Mathf.Max(components[i].height, height);
                }
            }
            height += Padding.vertical;
            this.height = height;
        } else {
            float height = default;
            int count = 0;
            for (int i = 0; i < childCount; i++) {
                if (components[i].isVisibleSelf) {
                    height += components[i].height;
                    count++;
                }
            }
            height += Padding.vertical;
            height += (count - 1) * itemGap;
            this.height = height;
        }
    }
    public new void FitChildrenHorizontally() => FitChildrenHorizontally(ItemGap);
    public new void FitChildrenHorizontally(float itemGap) {
        if (autoLayoutDirection == LayoutDirection.Horizontal) {
            float width = default;
            for (int i = 0; i < childCount; i++) {
                if (components[i].isVisibleSelf) {
                    width += components[i].width;
                }
            }
            width += (childCount - 1) * itemGap;
            width += Padding.horizontal;
            this.width = width;
        } else {
            float width = default;
            for (int i = 0; i < childCount; i++) {
                if (components[i].isVisibleSelf) {
                    width = Mathf.Max(components[i].width, width);
                }
            }
            width += Padding.horizontal;
            this.width = width;
        }
    }

    protected override Plane[] GetClippingPlanes() {
        if (!clipChildren) {
            return null;
        }
        Vector3[] corners = GetCorners();
        Vector3 vector = transform.TransformDirection(Vector3.right);
        Vector3 vector2 = transform.TransformDirection(Vector3.left);
        Vector3 vector3 = transform.TransformDirection(Vector3.up);
        Vector3 vector4 = transform.TransformDirection(Vector3.down);
        float d = PixelsToUnits();
        RectOffset padding = Padding;
        corners[0] += vector * padding.left * d + vector4 * padding.top * d;
        corners[1] += vector2 * padding.right * d + vector4 * padding.top * d;
        corners[2] += vector2 * padding.right * d + vector3 * padding.bottom * d;
        m_CachedClippingPlanes[0] = new Plane(vector, corners[0]);
        m_CachedClippingPlanes[1] = new Plane(vector2, corners[1]);
        m_CachedClippingPlanes[2] = new Plane(vector3, corners[2]);
        m_CachedClippingPlanes[3] = new Plane(vector4, corners[0]);
        return m_CachedClippingPlanes;
    }

    protected override void OnRebuildRenderData() {
        if (Atlas is null) {
            return;
        }
        if (bgRenderData is not null) {
            bgRenderData.Clear();
        } else {
            bgRenderData = UIRenderData.Obtain();
            m_RenderData.Add(bgRenderData);
        }
        bgRenderData.material = Atlas.material;
        RenderBackground();
        if (fgRenderData is not null) {
            fgRenderData.Clear();
        } else {
            fgRenderData = UIRenderData.Obtain();
            m_RenderData.Add(fgRenderData);
        }
        fgRenderData.material = Atlas.material;
        RenderForeground();
    }
    protected virtual void RenderForeground() {
        if (!renderFg) {
            return;
        }
        if (Atlas is null) {
            return;
        }
        var foregroundSprite = GetForegroundSprite();
        if (foregroundSprite is null) {
            return;
        }
        Vector2 foregroundRenderSize = GetForegroundRenderSize(foregroundSprite);
        Vector2 foregroundRenderOffset = GetForegroundRenderOffset(foregroundRenderSize);
        Color32 color = ApplyOpacity(isEnabled ? FgNormalColor : FgDisabledColor);
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = foregroundRenderOffset,
            pixelsToUnits = PixelsToUnits(),
            size = foregroundRenderSize,
            spriteInfo = foregroundSprite
        };
        if (foregroundSprite.isSliced) {
            UISlicedSpriteRender.RenderSprite(fgRenderData, options);
            return;
        }
        UISpriteRender.RenderSprite(fgRenderData, options);
    }
    protected virtual void RenderBackground() {
        if (Atlas is null) {
            return;
        }
        var backgroundSprite = GetBackgroundSprite();
        if (backgroundSprite is null) {
            return;
        }
        Color32 color = ApplyOpacity(isEnabled ? bgNormalColor : bgDisabledColor);
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
            pixelsToUnits = PixelsToUnits(),
            size = size,
            spriteInfo = backgroundSprite
        };
        if (backgroundSprite.isSliced) {
            UISlicedSpriteRender.RenderSprite(bgRenderData, options);
            return;
        }
        UISpriteRender.RenderSprite(bgRenderData, options);
    }
    protected virtual UITextureAtlas.SpriteInfo GetForegroundSprite() {
        if (Atlas is null) {
            return null;
        }
        return Atlas[fgSprite];
    }
    protected virtual UITextureAtlas.SpriteInfo GetBackgroundSprite() {
        if (Atlas is null) {
            return null;
        }
        return Atlas[bgSprite];
    }
    protected virtual Vector2 GetForegroundRenderOffset(Vector2 renderSize) {
        Vector2 result = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
        if (HorizontalAlignment == UIHorizontalAlignment.Left) {
            result.x += FgSpritePadding.left;
        } else if (HorizontalAlignment == UIHorizontalAlignment.Center) {
            result.x += (width - renderSize.x) * 0.5f;
            result.x += (FgSpritePadding.left - FgSpritePadding.right);
        } else if (HorizontalAlignment == UIHorizontalAlignment.Right) {
            result.x += width - renderSize.x;
            result.x -= FgSpritePadding.right;
        }
        if (VerticalAlignment == UIVerticalAlignment.Bottom) {
            result.y -= height - renderSize.y;
            result.y += FgSpritePadding.bottom;
        } else if (VerticalAlignment == UIVerticalAlignment.Middle) {
            result.y -= (height - renderSize.y) * 0.5f;
            result.y -= (FgSpritePadding.top - FgSpritePadding.bottom);
        } else if (VerticalAlignment == UIVerticalAlignment.Top) {
            result.y -= FgSpritePadding.top;
        }
        return result;
    }
    protected virtual Vector2 GetForegroundRenderSize(UITextureAtlas.SpriteInfo spriteInfo) {
        Vector2 vector = Vector2.zero;
        if (spriteInfo == null) {
            return vector;
        }
        if (FgSpriteMode == ForegroundSpriteMode.Custom) {
            vector = fgSize;
        } else if (FgSpriteMode == ForegroundSpriteMode.Fill) {
            vector = spriteInfo.pixelSize;
        } else if (FgSpriteMode == ForegroundSpriteMode.Scale) {
            float num = Mathf.Min(width / spriteInfo.width, height / spriteInfo.height);
            vector = new Vector2(num * spriteInfo.width, num * spriteInfo.height);
            vector *= scaleFactor;
        } else {
            vector = size * scaleFactor;
        }
        return vector;
    }

    private void ChildIsVisibleChanged(UIComponent child, bool value) => ChildInvalidatedLayout();
    private void ChildZOrderChanged(UIComponent child, int value) => ChildInvalidatedLayout();
    private void ChildInvalidated(UIComponent child, Vector2 value) => ChildInvalidatedLayout();
    private void ChildInvalidatedLayout() {
        Arrange();
        Invalidate();
    }
    private void DetachEvents(UIComponent child) {
        child.eventVisibilityChanged -= ChildIsVisibleChanged;
        child.eventPositionChanged -= ChildInvalidated;
        child.eventSizeChanged -= ChildInvalidated;
        child.eventZOrderChanged -= ChildZOrderChanged;
    }
    private void AttachEvents(UIComponent child) {
        child.eventVisibilityChanged += ChildIsVisibleChanged;
        child.eventPositionChanged += ChildInvalidated;
        child.eventSizeChanged += ChildInvalidated;
        child.eventZOrderChanged += ChildZOrderChanged;
    }
    protected virtual void OnComponentAddedInvoke(UIComponent child) {
        if (autoLayout) {
            AttachEvents(child);
            Arrange();
        }
    }
    protected virtual void OnComponentRemovedInvoke(UIComponent child) {
        if (autoLayout) {
            if (child != null) {
                DetachEvents(child);
            }
        }
    }
    protected override void OnComponentAdded(UIComponent child) {
        base.OnComponentAdded(child);
        OnComponentAddedInvoke(child);
    }
    protected override void OnComponentRemoved(UIComponent child) {
        base.OnComponentRemoved(child);
        OnComponentRemovedInvoke(child);
    }
}

public enum ForegroundSpriteMode {
    Stretch,
    Scale,
    Fill,
    Custom
}