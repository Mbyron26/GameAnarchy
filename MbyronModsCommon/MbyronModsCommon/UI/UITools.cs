using ColossalFramework.UI;
using System.Collections;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class UIStyleGamma : UIStyleBase {
        private float gap = 2;
        private UIHorizontalAlignment horizontalAlignment = UIHorizontalAlignment.Center;
        public float Gap {
            get => gap;
            set {
                if (!value.Equals(gap)) {
                    gap = value;
                    RefreshLayout();
                }
            }
        }
        public UIHorizontalAlignment HorizontalAlignment {
            get => horizontalAlignment;
            set {
                if (!value.Equals(horizontalAlignment)) {
                    horizontalAlignment = value;
                    RefreshLayout();
                }
            }
        }
        public UIStyleGamma(UIComponent parent) {
            Parent = parent;
            IsInitialized = true;
        }

        public UIStyleGamma(UIComponent parent, UIComponent child, UILabel major, UILabel minor) : this(parent) {
            Child = child;
            MajorLabel = major;
            MinorLabel = minor;
        }

        public UIStyleGamma(UIComponent parent, UIComponent child, UILabel major, UILabel minor, RectOffset padding) : this(parent, child, major, minor) {
            Padding = padding;
        }

        public override void RefreshLayout() {
            Processing = true;

            float height0 = padding.top;
            if (MajorLabel is not null) {
                MajorLabel.width = Parent.width - padding.left - padding.right;
                MajorLabel.relativePosition = new Vector2(padding.left, height0);
                height0 = MajorLabel.relativePosition.y + MajorLabel.size.y;
            }
            if (Child is not null) {
                height0 += gap;
                if (horizontalAlignment == UIHorizontalAlignment.Center) {
                    Child.relativePosition = new Vector2((Parent.width - Child.width) / 2, height0);
                } else if (horizontalAlignment == UIHorizontalAlignment.Left) {
                    Child.relativePosition = new Vector2(padding.left, height0);
                } else {
                    Child.relativePosition = new Vector2(Parent.width - padding.right - Child.width, height0);
                }
                height0 = Child.relativePosition.y + Child.size.y;
            }
            if (MinorLabel is not null) {
                height0 += gap;
                MinorLabel.width = Parent.width - padding.left - padding.right;
                MinorLabel.relativePosition = new Vector2(padding.left, height0);
                height0 = MinorLabel.relativePosition.y + MinorLabel.size.y;
            }
            height0 += padding.bottom;
            Parent.height = height0;

            Processing = false;
        }

        public override void Reset() {
            MajorLabel = null;
            MinorLabel = null;
            gap = 2;
            horizontalAlignment = UIHorizontalAlignment.Center;
            base.Reset();
        }
    }

    public class UIStyleAlpha : UIStyleBase, IEnumerable {
        protected float labelGap = default;
        protected float labelListBoxSpacing = 10;
        public float LabelGap {
            get => labelGap;
            set {
                if (!Equals(value, labelGap)) {
                    labelGap = value;
                    RefreshLayout();
                }
            }
        }
        public float LabelButtonSpacing {
            get => labelListBoxSpacing;
            set {
                if (!Equals(value, labelListBoxSpacing)) {
                    labelListBoxSpacing = value;
                    RefreshLayout();
                }
            }
        }

        public UIStyleAlpha(UIComponent parent) => Parent = parent;

        public UIStyleAlpha(UIComponent parent, UIComponent child, UILabel major, UILabel minor) : this(parent) {
            Child = child;
            MajorLabel = major;
            MinorLabel = minor;
        }
        public UIStyleAlpha(UIComponent parent, UIComponent child, UILabel major, UILabel minor, RectOffset padding) : this(parent, child, major, minor) {
            Padding = padding;
        }

        public virtual IEnumerator GetEnumerator() {
            if (MajorLabel is not null) {
                yield return MajorLabel;
            }
            if (MinorLabel is not null) {
                yield return MinorLabel;
            }
            if (Child is not null) {
                yield return Child;
            }
        }

        public override void RefreshLayout() {
            Processing = true;

            float height0 = default;
            float height1 = default;
            if (MajorLabel is not null) {
                MajorLabel.width = Child is null ? Parent.width - padding.left - padding.right : Parent.width - padding.left - padding.right - Child.width - labelListBoxSpacing;
                height0 = MajorLabel.height;
            }
            if (MinorLabel is not null) {
                MinorLabel.width = Child is null ? Parent.width - padding.left - padding.right : Parent.width - padding.left - padding.right - Child.width - labelListBoxSpacing;
                height1 = MinorLabel.height;
            }
            float height2 = height0 + (height1 == 0 ? 0 : (height1 + labelGap));
            float height3 = default;
            if (Child is not null) {
                height3 = Child.height;
            }
            Parent.height = Mathf.Max(height2, height3) + padding.top + padding.bottom;
            if (height2 > height3) {
                if (MajorLabel is not null) {
                    MajorLabel.relativePosition = new Vector2(padding.left, padding.top);
                }
                if (MinorLabel is not null) {
                    MinorLabel.relativePosition = new Vector2(padding.left, MajorLabel is null ? padding.top : MajorLabel.relativePosition.y + MajorLabel.size.y + labelGap);
                }
                if (Child is not null) {
                    Child.relativePosition = new Vector2(Parent.width - padding.right - Child.width, (Parent.height - Child.height) / 2);
                }
            } else {
                if (Child is not null) {
                    Child.relativePosition = new Vector2(Parent.width - padding.right - Child.width, padding.top);
                }
                if (MajorLabel is not null) {
                    if (MinorLabel is null)
                        MajorLabel.relativePosition = new Vector2(padding.left, (Parent.height - MajorLabel.height) / 2);
                    else {
                        MajorLabel.relativePosition = new Vector2(padding.left, (Parent.height - height2) / 2);
                        MinorLabel.relativePosition = new Vector2(padding.left, MajorLabel.relativePosition.y + MajorLabel.size.y + labelGap);
                    }
                }
            }

            Processing = false;
        }

        public override void Reset() {
            MajorLabel = null;
            MinorLabel = null;
            labelGap = default;
            labelListBoxSpacing = 10;
            base.Reset();
        }
    }
    public abstract class UIStyleBase : IUIStyle {
        protected RectOffset padding = new();
        public bool IsInitialized { get; set; }
        public UILabel MajorLabel { get; set; }
        public UILabel MinorLabel { get; set; }
        public UIComponent Parent { get; set; }
        public UIComponent Child { get; set; }
        public bool Processing { get; protected set; } = false;
        public virtual RectOffset Padding {
            get => padding;
            set {
                value = value.ConstrainPadding();
                if (!Equals(value, padding)) {
                    padding = value;
                    RefreshLayout();
                }
            }
        }

        public abstract void RefreshLayout();
        public virtual void Reset() {
            Parent = null;
            Child = null;
            padding = new RectOffset();
            IsInitialized = false;
        }
    }

    public interface IUIStyle {
        bool IsInitialized { get; set; }
        void RefreshLayout();
        void Reset();
    }

}
