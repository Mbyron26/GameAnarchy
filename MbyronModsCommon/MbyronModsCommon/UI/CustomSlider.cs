using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public class CustomSlider {
        public static UISlider AddSliderGamma(UIComponent parent, Vector2 size, float min, float max, float step, float defaultValue, PropertyChangedEventHandler<float> callback) {
            UISlider slider = parent.AddUIComponent<UISlider>();
            slider.size = size;
            UISlicedSprite sliderSprite = slider.AddUIComponent<UISlicedSprite>();
            sliderSprite.atlas = CustomAtlas.CommonAtlas;
            sliderSprite.spriteName = CustomAtlas.SliderGamma;
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

        public static SliderAlpha AddSliderAlpha(UIComponent parent, string text, float min, float max, float step, float defaultValue, SliderAlphaSize size, PropertyChangedEventHandler<float> callback, string leftLabel, string rightLabel, RectOffset padding = null, float textScale = 0.75f) {
            var customSliderStyleA = parent.AddUIComponent<SliderAlpha>();
            customSliderStyleA.SliderSize = size;
            customSliderStyleA.FillValue(text, min, max, step, defaultValue, callback, leftLabel, rightLabel, textScale, padding);
            return customSliderStyleA;
        }
    }

    public sealed class SliderAlpha : UIPanel {
        private SliderAlphaSize sliderSize = new(60f, 200f, 30f);
        public UILabel MiddleLabel { get; private set; }
        public UILabel LeftLabel { get; private set; }
        public UILabel RightLabel { get; private set; }
        public UISlider Slider { get; private set; }
        public float SliderValue { get => Slider.value; set => Slider.value = value; }
        public SliderAlphaSize SliderSize {
            get => sliderSize;
            set {
                sliderSize = value;
                RefreshLayout();
            }
        }

        public SliderAlpha() {
            autoLayout = false;
            clipChildren = true;
            LeftLabel = AddLabel(CustomAtlas.SliderAlphaLeftSprite);
            Slider = AddUIComponent<UISlider>();
            Slider.size = new Vector2(sliderSize.SliderWidth, sliderSize.SliderHeight);
            Slider.atlas = CustomAtlas.CommonAtlas;
            Slider.backgroundSprite = CustomAtlas.SliderAlphaMidSprite;
            Slider.scrollWheelAmount = 0;
            UISprite fillIndicator = Slider.AddUIComponent<UISprite>();
            fillIndicator.atlas = CustomAtlas.CommonAtlas;
            fillIndicator.spriteName = CustomAtlas.SliderAlphaSprite;
            Slider.fillIndicatorObject = fillIndicator;
            MiddleLabel = Slider.AddUIComponent<UILabel>();
            RightLabel = AddLabel(CustomAtlas.SliderAlphaRightSprite);
            RefreshLayout();
        }

        public void FillValue(string text, float min, float max, float step, float defaultValue, PropertyChangedEventHandler<float> callback, string leftLabel, string rightLabel, float textScale = 0.75f, RectOffset padding = null) {
            Slider.minValue = min;
            Slider.maxValue = max;
            Slider.stepSize = step;
            Slider.value = defaultValue;
            Slider.eventValueChanged += callback;
            MiddleLabel.textScale = 0.85f;
            MiddleLabel.textColor = new Color32(0x00, 0x00, 0x00, 0x50);
            if (padding is not null) {
                MiddleLabel.padding = padding;
            }
            MiddleLabel.text = text + ": " + defaultValue;
            Slider.eventValueChanged += (_, value) => {
                MiddleLabel.text = text + ": " + value;
            };
            FillLabelValue(LeftLabel, leftLabel, textScale, padding);
            FillLabelValue(RightLabel, rightLabel, textScale, padding);
            RefreshLayout();
        }

        private UILabel FillLabelValue(UILabel label, string text, float textScale, RectOffset padding) {
            label.textScale = textScale;
            if (padding is not null) {
                label.padding = padding;
            }
            label.text = text;
            return label;
        }

        private void RefreshLayout() {
            RightLabel.width = LeftLabel.width = sliderSize.ClampedLabelWidth;
            Slider.height = RightLabel.height = LeftLabel.height = sliderSize.SliderHeight;
            Slider.width = sliderSize.SliderWidth;
            LeftLabel.relativePosition = Vector2.zero;
            Slider.relativePosition = new Vector2(SliderSize.ClampedLabelWidth, 0);
            RightLabel.relativePosition = new Vector2(SliderSize.ClampedLabelWidth + SliderSize.SliderWidth, 0);
            size = new Vector2(sliderSize.ClampedLabelWidth * 2 + sliderSize.SliderWidth, sliderSize.SliderHeight);
            if (!string.IsNullOrEmpty(MiddleLabel.text)) {
                using UIFontRenderer fontRenderer = MiddleLabel.ObtainRenderer();
                Vector2 size = fontRenderer.MeasureString(MiddleLabel.text);
                MiddleLabel.relativePosition = new Vector2((Slider.size.x - MiddleLabel.size.x) / 2f, (Slider.size.y - MiddleLabel.size.y) / 2f);
            }
        }

        private UILabel AddLabel(string backgroundSprite) {
            var label = AddUIComponent<UILabel>();
            label.autoSize = false;
            label.size = new Vector2(sliderSize.ClampedLabelWidth, sliderSize.SliderHeight);
            label.autoHeight = false;
            label.wordWrap = true;
            label.atlas = CustomAtlas.CommonAtlas;
            label.backgroundSprite = backgroundSprite;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.verticalAlignment = UIVerticalAlignment.Middle;
            label.dropShadowOffset = new Vector2(-1.2f, 1.2f);
            label.dropShadowColor = new Color32(0x00, 0x00, 0x00, 0x40);
            label.useDropShadow = true;
            label.outlineSize = 0;
            return label;
        }
    }

    public struct SliderAlphaSize {
        public float ClampedLabelWidth;
        public float SliderWidth;
        public float SliderHeight;
        public SliderAlphaSize(float clampedLabelWidth, float sliderWidth, float sliderheight) {
            ClampedLabelWidth = clampedLabelWidth;
            SliderWidth = sliderWidth;
            SliderHeight = sliderheight;
        }
    }

}
