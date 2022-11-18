using ColossalFramework.UI;
using System;
using System.Diagnostics;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomSlider {
        public static CustomSliderBase AddCustomSliderStyleA(UIComponent parent, string text, float min, float max, float step, float defaultVal, PropertyChangedEventHandler<float> callback) {
            var customSliderStyleA = parent.AddUIComponent<CustomSliderBase>();
            customSliderStyleA.UseDefaultSize = false;
            customSliderStyleA.SliderSize = new CustomSliderBase.Size(60, 574, 30);
            customSliderStyleA.FillSliderValue(text, min, max, step, defaultVal, callback);
            return customSliderStyleA;
        }
    }

    public class CustomSliderBase : UIPanel {
        protected Size defaultSize = new(50f, 200f, 30f);
        private Size sliderSize;
        protected bool useDefaultSize = true;

        private UITextureAtlas Atlas { get; set; }
        protected UILabel MiddleLabel { get; set; }
        protected UILabel LeftLabel { get; set; }
        protected UILabel RightLabel { get; set; }
        protected UISlider Slider { get; set; }
        public float SliderValue { get => Slider.value; set => Slider.value = value; }
        public Size DefaultSize { get => defaultSize; }
        public Size SliderSize {
            get => sliderSize;
            set {
                sliderSize = value;
                StartLayout();
            }
        }
        public bool UseDefaultSize {
            get => useDefaultSize;
            set {
                if (value != useDefaultSize) {
                    useDefaultSize = value;
                }
            }
        }

        public CustomSliderBase() {
            Atlas = CustomAtlas.CommonAtlas;
            autoLayout = false;
            clipChildren = true;
            Initialize();
            StartLayout();
        }


        public virtual void FillSliderValue(string text, float min, float max, float step, float defaultVal, PropertyChangedEventHandler<float> callback) {
            Slider.minValue = min;
            Slider.maxValue = max;
            Slider.stepSize = step;
            Slider.value = defaultVal;
            Slider.eventValueChanged += callback;
            MiddleLabel.textScale = 0.85f;
            MiddleLabel.textColor = new Color32(0x00, 0x00, 0x00, 0x50); ;
            MiddleLabel.padding = new RectOffset(0, 0, 4, 0);
            MiddleLabel.text = text + ": " + defaultVal;
            Slider.eventValueChanged += (_, value) => {
                MiddleLabel.text = text + ": " + value;
            };
            FillLabelValue(LeftLabel, min.ToString(), 0.75f, UIColor.White, new RectOffset(0, 0, 4, 0));
            FillLabelValue(RightLabel, max.ToString(), 0.75f, UIColor.White, new RectOffset(0, 0, 4, 0));
            StartLayout();
        }

        protected UILabel FillLabelValue(UILabel label, string _text, float _textScale, Color32 _textColor = default, RectOffset _padding = null) {
            label.textScale = _textScale;
            label.textColor = _textColor;
            if (_padding != null)
                label.padding = _padding;
            label.text = _text;
            return label;
        }

        protected void StartLayout() {
            UpdateSize();
            UpdatePosition();
            UpdatePanelSize();
        }
        private void UpdatePanelSize() {
            var width = sliderSize.ClampedLabelWidth * 2 + sliderSize.SliderWidth;
            var height = sliderSize.SliderHeight;
            size = new Vector2(width, height);
            if (!string.IsNullOrEmpty(MiddleLabel.text)) {
                using (UIFontRenderer fontRenderer = MiddleLabel.ObtainRenderer()) {
                    Vector2 size = fontRenderer.MeasureString(MiddleLabel.text);
                    MiddleLabel.relativePosition = new Vector2((Slider.size.x - MiddleLabel.size.x) / 2f, (Slider.size.y - MiddleLabel.size.y) / 2f);
                }
            }
        }
        private void UpdatePosition() {
            LeftLabel.relativePosition = Vector2.zero;
            Slider.relativePosition = new Vector2(SliderSize.ClampedLabelWidth, 0);
            RightLabel.relativePosition = new Vector2(SliderSize.ClampedLabelWidth + SliderSize.SliderWidth, 0);
        }
        private void UpdateSize() {
            if (UseDefaultSize) {
                sliderSize = defaultSize;
                RightLabel.width = LeftLabel.width = defaultSize.ClampedLabelWidth;
                Slider.height = RightLabel.height = LeftLabel.height = defaultSize.SliderHeight;
                Slider.width = defaultSize.SliderWidth;
            } else {
                RightLabel.width = LeftLabel.width = sliderSize.ClampedLabelWidth;
                Slider.height = RightLabel.height = LeftLabel.height = sliderSize.SliderHeight;
                Slider.width = sliderSize.SliderWidth;
            }
        }

        private void Initialize() {
            LeftLabel = AddLabel(this, Atlas, "SliderLeftSprite");

            Slider = AddUIComponent<UISlider>();
            Slider.size = new Vector2(defaultSize.SliderWidth, defaultSize.SliderHeight);
            Slider.atlas = Atlas;
            Slider.backgroundSprite = "SliderMidSprite";
            Slider.scrollWheelAmount = 0;
            UISprite fillIndicator = Slider.AddUIComponent<UISprite>();
            fillIndicator.atlas = Atlas;
            fillIndicator.spriteName = "SliderSprite";
            Slider.fillIndicatorObject = fillIndicator;

            MiddleLabel = Slider.AddUIComponent<UILabel>();
            MiddleLabel.autoSize = true;
            MiddleLabel.wordWrap = false;
            MiddleLabel.textAlignment = UIHorizontalAlignment.Center;
            MiddleLabel.verticalAlignment = UIVerticalAlignment.Middle;

            RightLabel = AddLabel(this, Atlas, "SliderRightSprite");
        }

        private static UILabel AddLabel(UIComponent component, UITextureAtlas atlas, string _backgroundSprite) {
            UILabel label = component.AddUIComponent<UILabel>();
            label.autoSize = false;
            label.size = new Vector2(50, 30);
            label.autoHeight = false;
            label.wordWrap = false;
            label.atlas = atlas;
            label.backgroundSprite = _backgroundSprite;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.verticalAlignment = UIVerticalAlignment.Middle;
            label.dropShadowOffset = new Vector2(-1.2f, 1.2f);
            label.dropShadowColor = new Color32(0x00, 0x00, 0x00, 0x40);
            label.useDropShadow = true;
            label.outlineSize = 0;
            return label;
        }

        public struct Size {
            public float ClampedLabelWidth;
            public float SliderWidth;
            public float SliderHeight;
            public Size(float m_clampedLabelWidth, float m_sliderWidth, float m_sliderheight) {
                ClampedLabelWidth = m_clampedLabelWidth;
                SliderWidth = m_sliderWidth;
                SliderHeight = m_sliderheight;
            }
        }

    }
}
