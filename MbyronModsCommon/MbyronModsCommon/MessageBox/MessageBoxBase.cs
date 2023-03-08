﻿global using MbyronModsCommon.UI;
using ColossalFramework.UI;
using ColossalFramework;
using System;
using UnityEngine;

namespace MbyronModsCommon {
    public static class MessageBox {
        public static T Show<T>() where T : MessageBoxBase {
            var uiObject = new GameObject();
            uiObject.transform.parent = UIView.GetAView().transform;
            var messageBox = uiObject.AddComponent<T>();

            UIView.PushModal(messageBox);
            messageBox.Show(true);
            messageBox.Focus();

            if (UIView.GetAView().panelsLibraryModalEffect is UIComponent modalEffect) {
                modalEffect.FitTo(null);
                if (!modalEffect.isVisible || modalEffect.opacity != 1f) {
                    modalEffect.Show(false);
                    ValueAnimator.Cancel("ModalEffect67419");
                    ValueAnimator.Animate("ModalEffect67419", val => modalEffect.opacity = val, new AnimatedFloat(0f, 1f, 0.7f, EasingType.CubicEaseOut));
                }
            }

            return messageBox;
        }
        public static void Hide(MessageBoxBase messageBox) {
            if (messageBox == null || UIView.GetModalComponent() != messageBox)
                return;

            UIView.PopModal();

            if (UIView.GetAView().panelsLibraryModalEffect is UIComponent modalEffect) {
                if (!UIView.HasModalInput()) {
                    ValueAnimator.Cancel("ModalEffect67419");
                    ValueAnimator.Animate("ModalEffect67419", val => modalEffect.opacity = val, new AnimatedFloat(1f, 0f, 0.7f, EasingType.CubicEaseOut), () => modalEffect.Hide());
                } else
                    modalEffect.zOrder = UIView.GetModalComponent().zOrder - 1;
            }

            messageBox.Hide();
            UnityEngine.Object.Destroy(messageBox.gameObject);
            UnityEngine.Object.Destroy(messageBox);
        }
    }

    public class MessageBoxParameters {
        public const float Width = 600;
        public const int Padding = 10;
        public const float ComponentWidth = 580;
    }

    public abstract class MessageBoxBase : UIPanel {
        private const int dragBarHeight = 40;
        protected const float defaultWidth = 600;
        protected const int defaultHeight = 200;
        protected const int DefaultPadding = 10;
        protected const float buttonHeight = 48f;
        protected const float buttonWidth = 580;
        protected const float buttonPanelHeight = 48f + 2 * DefaultPadding;
        public UIPanel Background { get; protected set; }
        public string TitleText { set => Title.text = value; }
        protected UIDragHandle DragBar { get; private set; }
        protected UILabel Title { get; private set; }
        protected UIButton CloseButton { get; private set; }
        public MessageBoxScrollablePanel ScrollableContentPanel { get; private set; }
        public AutoSizeScrollablePanel MainPanel => ScrollableContentPanel.MainPanel;
        protected UIPanel ButtonPanel { get; set; }
        public MessageBoxBase() {
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            size = new Vector2(defaultWidth, defaultHeight);
            AddBackgound();
            AddTitleBar();
            AddContentPanel();
            AddButtonPanel();
            Resize();
            ScrollableContentPanel.eventSizeChanged += (component, size) => Resize();
        }
        private void AddBackgound() {
            Background = AddUIComponent<UIPanel>();
            Background.name = nameof(Background);
            Background.size = new Vector2(defaultWidth, defaultHeight);
            Background.backgroundSprite = "TextFieldPanel";
            Background.color = new(43, 47, 64, 240);
            Background.relativePosition = Vector2.zero;
        }
        protected void Close() => MessageBox.Hide(this);
        private void AddTitleBar() {
            DragBar = AddUIComponent<UIDragHandle>();
            DragBar.size = new Vector2(defaultWidth, dragBarHeight);
            DragBar.relativePosition = Vector2.zero;
            Title = DragBar.AddUIComponent<UILabel>();
            Title.textAlignment = UIHorizontalAlignment.Center;
            Title.verticalAlignment = UIVerticalAlignment.Middle;
            Title.textScale = 1.3f;
            Title.font = CustomFont.SemiBold;
            Title.autoHeight = true;
            Title.eventTextChanged += (component, text) => {
                Title.CenterToParent();
            };

        }
        protected void AddCaptionCloseButton() {
            CloseButton = DragBar.AddUIComponent<UIButton>();
            CloseButton.normalBgSprite = "buttonclose";
            CloseButton.hoveredBgSprite = "buttonclosehover";
            CloseButton.pressedBgSprite = "buttonclosepressed";
            CloseButton.size = new Vector2(32f, 32f);
            CloseButton.relativePosition = new Vector2(defaultWidth - 32f - 4f, 4f);
            CloseButton.eventClicked += CloseButtonOnClicked;
        }

        private void CloseButtonOnClicked(UIComponent component, UIMouseEventParameter eventParam) => Close();

        private float MaxScrollableContentHeight {
            get {
                var resolution = GetUIView().GetScreenResolution();
                return resolution.y - 600f;
            }
        }

        private void AddContentPanel() {
            ScrollableContentPanel = AddUIComponent<MessageBoxScrollablePanel>();
            ScrollableContentPanel.relativePosition = new Vector2(0f, dragBarHeight);
            ScrollableContentPanel.MaxHeight = MaxScrollableContentHeight;
        }

        private void AddButtonPanel() {
            ButtonPanel = AddUIComponent<UIPanel>();
            ButtonPanel.size = new Vector2(defaultWidth, buttonPanelHeight);
        }

        protected void AddButtons(uint number, uint total, string _text, Action action) {
            var spacing = (total - 1) * DefaultPadding;
            var buttonWidth = (defaultWidth - 2 * DefaultPadding - spacing) / total;
            UIButton button = CustomButton.AddButton(ButtonPanel, 1f, _text, buttonWidth, buttonHeight);
            ArrangePosition(button, number, buttonWidth);
            button.eventClicked += (component, eventParam) => action?.Invoke();
        }

        private UIButton ArrangePosition(UIButton button, uint number, float buttonWidth) {
            button.name = "Button" + number.ToString();
            var posX = DefaultPadding + (number - 1) * (buttonWidth + DefaultPadding);
            float posY = 10f;
            button.relativePosition = new Vector2(posX, posY);
            return button;
        }

        private Vector2 SizeBefore { get; set; } = new Vector2();
        protected override void OnSizeChanged() {
            base.OnSizeChanged();

            var resolution = GetUIView().GetScreenResolution();
            var delta = (size - SizeBefore) / 2;
            SizeBefore = size;

            var newPosition = Vector2.Max(Vector2.Min((Vector2)relativePosition - delta, resolution - size), Vector2.zero);
            relativePosition = newPosition;
        }

        protected void Resize() {
            height = dragBarHeight + ScrollableContentPanel.height + buttonPanelHeight;
            ButtonPanel.relativePosition = new Vector2(0f, dragBarHeight + ScrollableContentPanel.height);
            Background.height = height;
        }

        protected override void OnKeyDown(UIKeyEventParameter p) {
            if (!p.used) {
                if (p.keycode == KeyCode.Escape) {
                    p.Use();
                    Close();
                }
            }
        }
    }
}
