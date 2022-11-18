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
    public abstract class MessageBoxBase : UIPanel {
        private const int dragBarHeight = 40;
        protected const float defaultWidth = 600;
        protected const int defaultHeight = 200;
        protected const int defaultPadding = 10;
        protected const float buttonHeight = 48f;
        protected const float buttonWidth = 580;
        protected const float buttonPanelHeight = 48f + 2 * defaultPadding;
        public string TitleText { set => Title.text = value; }
        protected UIDragHandle DragBar { get; private set; }
        protected UILabel Title { get; private set; }
        protected UIButton CloseButton { get; private set; }
        public MessageBoxScrollablePanel ScrollableContentPanel { get; private set; }
        public ScrollablePanelWithCard MainPanel => ScrollableContentPanel.MainPanel;
        protected UIPanel ButtonPanel { get; set; }
        public MessageBoxBase() {
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            size = new Vector2(defaultWidth, defaultHeight);
            backgroundSprite = "MenuPanel";
            color = new Color32(58, 88, 104, 255);

            AddTitleBar();
            AddContentPanel();
            AddButtonPanel();
            Resize();
            ScrollableContentPanel.eventSizeChanged += (component, size) => Resize();
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
            Title.eventTextChanged += (component, text) => {
                Title.CenterToParent();
            };
            CloseButton = DragBar.AddUIComponent<UIButton>();
            CloseButton.normalBgSprite = "buttonclose";
            CloseButton.hoveredBgSprite = "buttonclosehover";
            CloseButton.pressedBgSprite = "buttonclosepressed";
            CloseButton.size = new Vector2(32f, 32f);
            CloseButton.relativePosition = new Vector2(defaultWidth - 32f - 4f, 4f);
            CloseButton.eventClicked += CloseButtonOnClicked;
        }

        private void CloseButtonOnClicked(UIComponent component, UIMouseEventParameter eventParam) => Close();

        private Vector2 MaxScrollableContentSize {
            get {
                var resolution = GetUIView().GetScreenResolution();
                return new Vector2(defaultWidth, resolution.y - 600f);
            }
        }

        private void AddContentPanel() {
            ScrollableContentPanel = AddUIComponent<MessageBoxScrollablePanel>();
            ScrollableContentPanel.relativePosition = new Vector2(0f, dragBarHeight);
            ScrollableContentPanel.CustomMaxSize = MaxScrollableContentSize;
        }

        private void AddButtonPanel() {
            ButtonPanel = AddUIComponent<UIPanel>();
            ButtonPanel.size = new Vector2(defaultWidth, buttonPanelHeight);
        }

        protected void AddButtons(uint number, uint total, string _text, Action action) {
            var spacing = (total - 1) * defaultPadding;
            var buttonWidth = (defaultWidth - 2 * defaultPadding - spacing) / total;
            UIButton button = CustomButton.AddButton(ButtonPanel, 1f, _text, buttonWidth, buttonHeight);
            ArrangePosition(button, number, buttonWidth);
            button.eventClicked += (component, eventParam) => action?.Invoke();
        }

        private UIButton ArrangePosition(UIButton button, uint number, float buttonWidth) {
            button.name = "Button" + number.ToString();
            var posX = defaultPadding + (number - 1) * (buttonWidth + defaultPadding);
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
