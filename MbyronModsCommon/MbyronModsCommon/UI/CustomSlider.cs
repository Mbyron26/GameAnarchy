using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomSlider {
        public static UISlider AddSliderGamma(UIComponent parent, Vector2 size, float min, float max, float step, float defaultValue, PropertyChangedEventHandler<float> callback) {
            UISlider slider = parent.AddUIComponent<UISlider>();
            slider.size = size;
            UISlicedSprite sliderSprite = slider.AddUIComponent<UISlicedSprite>();
            sliderSprite.atlas = CustomAtlas.CommonAtlas;
            sliderSprite.spriteName = CustomAtlas.GradientSlider;
            sliderSprite.size = size;
            sliderSprite.relativePosition = new Vector2(0f, 0f);
            UISlicedSprite sliderThumb = slider.AddUIComponent<UISlicedSprite>();
            sliderThumb.atlas = CustomAtlas.CommonAtlas;
            sliderThumb.spriteName = CustomAtlas.SliderThumb;
            //sliderThumb.size = new(size.y / 2, size.y);
            slider.thumbObject = sliderThumb;
            slider.minValue = min;
            slider.maxValue = max;
            slider.stepSize = step;
            slider.value = defaultValue;
            slider.eventValueChanged += callback;
            slider.scrollWheelAmount = 0;
            return slider;
        }

        public static UISlider AddCustomSliderStyleB(UIComponent parent, string text, float min, float max, float step, float defaultValue, Vector2 size, PropertyChangedEventHandler<float> callback, out UILabel labelText) {
            UIPanel panel = parent.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsSliderTemplate")) as UIPanel;
            panel.autoLayoutPadding = new RectOffset(1, 0, 2, 0);
            panel.autoFitChildrenVertically = true;
            var label = panel.Find<UILabel>("Label");
            label.text = text;
            label.textScale = 1f;
            label.width = size.x;
            labelText = label;
            UISlider uislider = panel.Find<UISlider>("Slider");
            uislider.size = size;
            uislider.atlas = CustomAtlas.CommonAtlas;
            uislider.backgroundSprite = CustomAtlas.GradientSlider;
            UISprite sliderThumb = uislider.thumbObject as UISprite;
            sliderThumb.atlas = CustomAtlas.CommonAtlas;
            sliderThumb.spriteName = CustomAtlas.SliderThumb;
            sliderThumb.height = 20f;
            uislider.minValue = min;
            uislider.maxValue = max;
            uislider.stepSize = step;
            uislider.value = defaultValue;
            uislider.eventValueChanged += callback;
            uislider.scrollWheelAmount = 0;
            return uislider;
        }

        public static UISlider AddSlider(UIComponent parent, float min, float max, float step, float defaultVal, PropertyChangedEventHandler<float> callback) {
            UISlider slider = parent.AddUIComponent<UISlider>();
            slider.minValue = min;
            slider.maxValue = max;
            slider.stepSize = step;
            slider.value = defaultVal;
            slider.eventValueChanged += callback;
            return slider;
        }

        public static CustomSliderBase AddCustomSliderStyleA(UIComponent parent, string text, float min, float max, float step, float defaultVal, PropertyChangedEventHandler<float> callback, string leftLabel, string rightLabel) {
            var customSliderStyleA = parent.AddUIComponent<CustomSliderBase>();
            customSliderStyleA.FillSliderValue(text, min, max, step, defaultVal, callback, leftLabel, rightLabel);
            return customSliderStyleA;
        }

        public static CustomSliderBase AddCustomSliderStyleA(UIComponent parent, string text, float min, float max, float step, float defaultVal, CustomSliderBase.Size size, PropertyChangedEventHandler<float> callback) {
            var customSliderStyleA = parent.AddUIComponent<CustomSliderBase>();
            customSliderStyleA.SliderSize = size;
            customSliderStyleA.FillSliderValue(text, min, max, step, defaultVal, callback);
            return customSliderStyleA;
        }
    }

    public class CustomSliderBase : UIPanel {
        private Size sliderSize = new(50f, 200f, 30f);

        private UITextureAtlas Atlas { get; set; }
        protected UILabel MiddleLabel { get; set; }
        protected UILabel LeftLabel { get; set; }
        protected UILabel RightLabel { get; set; }
        protected UISlider Slider { get; set; }
        public float SliderValue { get => Slider.value; set => Slider.value = value; }
        public Size SliderSize {
            get => sliderSize;
            set {
                sliderSize = value;
                StartLayout();
            }
        }

        public CustomSliderBase() {
            Atlas = CustomAtlas.CommonAtlas;
            autoLayout = false;
            clipChildren = true;
            Initialize();
            StartLayout();
        }

        public void FillSliderValue(string text, float min, float max, float step, float defaultVal, PropertyChangedEventHandler<float> callback, string leftLabel, string rightLabel) {
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
            FillLabelValue(LeftLabel, leftLabel, 0.75f, UIColor.White, new RectOffset(0, 0, 4, 0));
            FillLabelValue(RightLabel, rightLabel, 0.75f, UIColor.White, new RectOffset(0, 0, 4, 0));
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
            RightLabel.width = LeftLabel.width = sliderSize.ClampedLabelWidth;
            Slider.height = RightLabel.height = LeftLabel.height = sliderSize.SliderHeight;
            Slider.width = sliderSize.SliderWidth;

        }

        private void Initialize() {
            LeftLabel = AddLabel(this, Atlas, "SliderLeftSprite");

            Slider = AddUIComponent<UISlider>();
            Slider.size = new Vector2(sliderSize.SliderWidth, sliderSize.SliderHeight);
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
