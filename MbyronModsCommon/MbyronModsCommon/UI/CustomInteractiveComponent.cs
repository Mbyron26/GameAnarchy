using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class InteractiveComponent : UITextComponent {
        protected UITextureAtlas atlas;
        protected Sprites backgroundSprites = new();
        protected Sprites foregroundSprites = new();
        protected bool isRenderFgSprites = false;
        protected SpriteState state;
        protected UIHorizontalAlignment fgHorizontalAlignment = UIHorizontalAlignment.Center;
        protected UIVerticalAlignment fgVerticalAlignment = UIVerticalAlignment.Middle;
        protected UIForegroundSpriteMode foregroundSpriteMode;
        protected float fgScaleFactor = 1f;
        protected RectOffset fgSpritePadding;

        public event Action<SpriteState> EventStateChanged;

        public UITextureAtlas Atlas {
            get {
                if (atlas is null) {
                    UIView uiview = GetUIView();
                    if (uiview is not null) {
                        atlas = uiview.defaultAtlas;
                    }
                }
                return atlas;
            }
            set {
                if (!UITextureAtlas.Equals(value, atlas)) {
                    atlas = value;
                    Invalidate();
                }
            }
        }
        public bool IsRenderFgSprites {
            get => isRenderFgSprites;
            set {
                if (isRenderFgSprites != value) {
                    isRenderFgSprites = value;
                    Invalidate();
                }
            }
        }
        public SpriteState State {
            get => state;
            set {
                if (state != value) {
                    OnStateChanged(value);
                }
            }
        }

        protected virtual void OnStateChanged(SpriteState value) {
            if (!isEnabled && value != SpriteState.Disabled) {
                return;
            }
            state = value;
            EventStateChanged?.Invoke(value);
            Invalidate();
        }

        public string NormalBgSprite {
            get => backgroundSprites.NormalSprite;
            set {
                if (value != backgroundSprites.NormalSprite) {
                    backgroundSprites.NormalSprite = value;
                    SetDefaultSize(value);
                    Invalidate();
                }
            }
        }
        public string NormalFgSprite {
            get => foregroundSprites.NormalSprite;
            set {
                if (value != foregroundSprites.NormalSprite) {
                    foregroundSprites.NormalSprite = value;
                    SetDefaultSize(value);
                    Invalidate();
                }
            }
        }
        public Color32 NormalBgColor {
            get => backgroundSprites.NormalColor;
            set {
                backgroundSprites.NormalColor = value;
                Invalidate();
            }
        }
        public Color32 NormalFgColor {
            get => foregroundSprites.NormalColor;
            set {
                foregroundSprites.NormalColor = value;
                Invalidate();
            }
        }
        public string HoveredBgSprite {
            get => backgroundSprites.HoveredSprite;
            set {
                if (value != backgroundSprites.HoveredSprite) {
                    backgroundSprites.HoveredSprite = value;
                    Invalidate();
                }
            }
        }
        public string HoveredFgSprite {
            get => foregroundSprites.HoveredSprite;
            set {
                if (value != foregroundSprites.HoveredSprite) {
                    foregroundSprites.HoveredSprite = value;
                    Invalidate();
                }
            }
        }
        public Color32 HoveredBgColor {
            get => backgroundSprites.HoveredColor;
            set {
                backgroundSprites.HoveredColor = value;
                Invalidate();
            }
        }
        public Color32 HoveredFgColor {
            get => foregroundSprites.HoveredColor;
            set {
                foregroundSprites.HoveredColor = value;
                Invalidate();
            }
        }
        public string PressedBgSprite {
            get => backgroundSprites.PressedSprite;
            set {
                if (value != backgroundSprites.PressedSprite) {
                    backgroundSprites.PressedSprite = value;
                    Invalidate();
                }
            }
        }
        public string PressedFgSprite {
            get => foregroundSprites.PressedSprite;
            set {
                if (value != foregroundSprites.PressedSprite) {
                    foregroundSprites.PressedSprite = value;
                    Invalidate();
                }
            }
        }
        public Color32 PressedBgColor {
            get => backgroundSprites.PressedColor;
            set {
                backgroundSprites.PressedColor = value;
                Invalidate();
            }
        }
        public Color32 PressedFgColor {
            get => foregroundSprites.PressedColor;
            set {
                foregroundSprites.PressedColor = value;
                Invalidate();
            }
        }
        public string FocusedBgSprite {
            get => backgroundSprites.FocusedSprite;
            set {
                if (value != backgroundSprites.FocusedSprite) {
                    backgroundSprites.FocusedSprite = value;
                    Invalidate();
                }
            }
        }
        public string FocusedFgSprite {
            get => foregroundSprites.FocusedSprite;
            set {
                if (value != foregroundSprites.FocusedSprite) {
                    foregroundSprites.FocusedSprite = value;
                    Invalidate();
                }
            }
        }
        public Color32 FocusedBgColor {
            get => backgroundSprites.FocusedColor;
            set {
                backgroundSprites.FocusedColor = value;
                Invalidate();
            }
        }
        public Color32 FocusedFgColor {
            get => foregroundSprites.FocusedColor;
            set {
                foregroundSprites.FocusedColor = value;
                Invalidate();
            }
        }
        public string DisabledBgSprite {
            get => backgroundSprites.DisabledSprite;
            set {
                if (value != backgroundSprites.DisabledSprite) {
                    backgroundSprites.DisabledSprite = value;
                    Invalidate();
                }
            }
        }
        public string DisabledFgSprite {
            get => foregroundSprites.DisabledSprite;
            set {
                if (value != foregroundSprites.DisabledSprite) {
                    foregroundSprites.DisabledSprite = value;
                    Invalidate();
                }
            }
        }
        public Color32 DisabledBgColor {
            get => backgroundSprites.DisabledColor;
            set {
                backgroundSprites.DisabledColor = value;
                Invalidate();
            }
        }
        public Color32 DisabledFgColor {
            get => foregroundSprites.DisabledColor;
            set {
                foregroundSprites.DisabledColor = value;
                Invalidate();
            }
        }
        public UIForegroundSpriteMode ForegroundSpriteMode {
            get => foregroundSpriteMode;
            set {
                if (value != foregroundSpriteMode) {
                    foregroundSpriteMode = value;
                    Invalidate();
                }
            }
        }
        public float FgScaleFactor {
            get => fgScaleFactor;
            set {
                if (!Mathf.Approximately(value, fgScaleFactor)) {
                    fgScaleFactor = value;
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

        public virtual UIHorizontalAlignment FgHorizontalAlignment {
            get => fgHorizontalAlignment;
            set {
                if (value != fgHorizontalAlignment) {
                    fgHorizontalAlignment = value;
                    Invalidate();
                }
            }
        }

        public virtual UIVerticalAlignment FgVerticalAlignment {
            get => fgVerticalAlignment;
            set {
                if (value != fgVerticalAlignment) {
                    fgVerticalAlignment = value;
                    Invalidate();
                }
            }
        }

        public UIMouseButton ButtonsMask { get; set; } = UIMouseButton.Left;

        public override bool canFocus => (isEnabled && isVisible) || base.canFocus;

        private void SetDefaultSize(string spriteName) {
            if (atlas is null) {
                return;
            }
            var spriteInfo = atlas[spriteName];
            if (m_Size == Vector2.zero && spriteInfo is not null) {
                size = spriteInfo.pixelSize;
            }
        }

        public override Vector2 CalculateMinimumSize() {
            var backgroundSprite = GetBackgroundSprite();
            if (backgroundSprite == null) {
                return base.CalculateMinimumSize();
            }
            var border = backgroundSprite.border;
            if (border.horizontal > 0 || border.vertical > 0) {
                return Vector2.Max(base.CalculateMinimumSize(), new Vector2(border.horizontal, border.vertical));
            }
            return base.CalculateMinimumSize();
        }

        protected virtual void RenderBackground() {
            if (atlas is null) {
                return;
            }
            var backgroundSprite = GetBackgroundSprite();
            if (backgroundSprite is null) {
                return;
            }
            Color32 color = ApplyOpacity(GetBgActiveColor());
            RenderOptions options = new() {
                atlas = atlas,
                color = color,
                fillAmount = 1f,
                flip = UISpriteFlip.None,
                offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
                pixelsToUnits = PixelsToUnits(),
                size = size,
                spriteInfo = backgroundSprite
            };
            if (backgroundSprite.isSliced) {
                Render.UISlicedSpriteRender.RenderSprite(renderData, options);
                return;
            }
            Render.UISpriteRender.RenderSprite(renderData, options);
        }

        protected virtual Vector2 GetForegroundRenderOffset(Vector2 renderSize) {
            Vector2 result = pivot.TransformToUpperLeft(size, arbitraryPivotOffset);
            if (FgHorizontalAlignment == UIHorizontalAlignment.Left) {
                result.x += FgSpritePadding.left;
            } else if (FgHorizontalAlignment == UIHorizontalAlignment.Center) {
                result.x += (width - renderSize.x) * 0.5f;
                result.x += (FgSpritePadding.left - FgSpritePadding.right);
            } else if (FgHorizontalAlignment == UIHorizontalAlignment.Right) {
                result.x += width - renderSize.x;
                result.x -= FgSpritePadding.right;
            }
            if (FgVerticalAlignment == UIVerticalAlignment.Bottom) {
                result.y -= height - renderSize.y;
                result.y += FgSpritePadding.bottom;
            } else if (FgVerticalAlignment == UIVerticalAlignment.Middle) {
                result.y -= (height - renderSize.y) * 0.5f;
                result.y -= (FgSpritePadding.top - FgSpritePadding.bottom);
            } else if (FgVerticalAlignment == UIVerticalAlignment.Top) {
                result.y -= FgSpritePadding.top;
            }
            return result;
        }

        protected virtual Vector2 GetForegroundRenderSize(UITextureAtlas.SpriteInfo spriteInfo) {
            Vector2 vector = Vector2.zero;
            if (spriteInfo is null) {
                return vector;
            }
            if (ForegroundSpriteMode == UIForegroundSpriteMode.Stretch) {
                vector = size * FgScaleFactor;
            } else if (ForegroundSpriteMode == UIForegroundSpriteMode.Fill) {
                vector = spriteInfo.pixelSize;
            } else if (ForegroundSpriteMode == UIForegroundSpriteMode.Scale) {
                float num = Mathf.Min(width / spriteInfo.width, height / spriteInfo.height);
                vector = new Vector2(num * spriteInfo.width, num * spriteInfo.height);
                vector *= FgScaleFactor;
            }
            return vector;
        }

        protected virtual void RenderForeground() {
            if (!IsRenderFgSprites) {
                return;
            }
            if (atlas is null) {
                return;
            }
            var foregroundSprite = GetForegroundSprite();
            if (foregroundSprite is null) {
                return;
            }
            Vector2 foregroundRenderSize = GetForegroundRenderSize(foregroundSprite);
            Vector2 foregroundRenderOffset = GetForegroundRenderOffset(foregroundRenderSize);
            Color32 color = ApplyOpacity(GetFgActiveColor());
            RenderOptions options = new() {
                atlas = atlas,
                color = color,
                fillAmount = 1f,
                flip = UISpriteFlip.None,
                offset = foregroundRenderOffset,
                pixelsToUnits = base.PixelsToUnits(),
                size = foregroundRenderSize,
                spriteInfo = foregroundSprite
            };
            if (foregroundSprite.isSliced) {
                Render.UISlicedSpriteRender.RenderSprite(renderData, options);
                return;
            }
            Render.UISpriteRender.RenderSprite(renderData, options);
        }

        protected virtual Color32 GetBgActiveColor() => backgroundSprites.GetSpriteColor(State);
        protected virtual Color32 GetFgActiveColor() => foregroundSprites.GetSpriteColor(State);

        protected virtual UITextureAtlas.SpriteInfo GetBackgroundSprite() {
            if (atlas is null) {
                return null;
            }
            if (!isEnabled) {
                return atlas[DisabledBgSprite];
            }
            if (!hasFocus) {
                if (m_IsMouseHovering) {
                    var spriteInfo = atlas[HoveredBgSprite];
                    if (spriteInfo != null) {
                        return spriteInfo;
                    }
                }
                return atlas[NormalBgSprite];
            }
            var spriteInfo2 = atlas[FocusedBgSprite];
            if (spriteInfo2 != null) {
                return spriteInfo2;
            }
            return atlas[NormalBgSprite];
        }

        protected virtual UITextureAtlas.SpriteInfo GetForegroundSprite() {
            if (atlas is null) {
                return null;
            }
            if (isEnabled) {
                return atlas[DisabledFgSprite];
            }
            if (!hasFocus) {
                if (m_IsMouseHovering) {
                    var spriteInfo = atlas[HoveredFgSprite];
                    if (spriteInfo != null) {
                        return spriteInfo;
                    }
                }
                return atlas[NormalFgSprite];
            }
            var spriteInfo2 = atlas[FocusedFgSprite];
            if (spriteInfo2 != null) {
                return spriteInfo2;
            }
            return atlas[NormalFgSprite];
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

        protected override void OnGotFocus(UIFocusEventParameter p) {
            base.OnGotFocus(p);
            Invalidate();
        }

        protected override void OnLostFocus(UIFocusEventParameter p) {
            base.OnLostFocus(p);
            Invalidate();
        }

        protected override void OnLeaveFocus(UIFocusEventParameter p) {
            State = (containsMouse ? SpriteState.Hovered : SpriteState.Normal);
            base.OnLeaveFocus(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p) {
            if (m_IsMouseHovering) {
                State = SpriteState.Hovered;
            } else {
                State = SpriteState.Normal;
            }
            base.OnMouseUp(p);
        }

        protected override void OnMouseDown(UIMouseEventParameter p) {
            if ((p.buttons & ButtonsMask) != UIMouseButton.None) {
                if (isEnabled && State != SpriteState.Focused) {
                    State = SpriteState.Pressed;
                }
                base.OnMouseDown(p);
            }
        }

        protected override void OnMouseEnter(UIMouseEventParameter p) {
            if (isEnabled) {
                State = SpriteState.Hovered;
            }
            base.OnMouseEnter(p);
            Invalidate();
        }

        protected override void OnMouseLeave(UIMouseEventParameter p) {
            if (isEnabled) {
                State = SpriteState.Normal;
            }
            base.OnMouseLeave(p);
            Invalidate();
        }

    }

}

