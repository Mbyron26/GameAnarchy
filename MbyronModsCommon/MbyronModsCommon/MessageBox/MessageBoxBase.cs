using MbyronModsCommon.UI;
using ColossalFramework.UI;
using ColossalFramework;
using UnityEngine;
using ICities;
namespace MbyronModsCommon;

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

public abstract class MessageBoxBase : CustomUIPanel {
    protected const int defaultHeight = 200;
    protected const float buttonHeight = 34f;
    protected AutoSizeUIScrollablePanel contentPanel;
    protected CustomUIPanel buttonPanel;
    protected UIDragHandle dragBar;
    protected UILabel title;

    public string TitleText { set => title.text = value; }
    protected CustomUIScrollablePanel MainPanel => contentPanel.MainPanel;

    public MessageBoxBase() {
        isVisible = true;
        canFocus = true;
        isInteractive = true;
        size = new Vector2(MessageBoxParm.Width, defaultHeight);
        atlas = CustomUIAtlas.MbyronModsAtlas;
        bgSprite = CustomUIAtlas.CustomBackground;
        InitComponets();
        Resize();
        contentPanel.eventSizeChanged += (component, size) => Resize();
    }

    protected void Close() => MessageBox.Hide(this);
    protected virtual void Resize() {
        height = MessageBoxParm.DragBarHeight + contentPanel.height + MessageBoxParm.ButtonPanelHeight;
        contentPanel.relativePosition = new Vector2(0f, MessageBoxParm.DragBarHeight);
        buttonPanel.relativePosition = new Vector2(0f, MessageBoxParm.DragBarHeight + contentPanel.height);
    }
    protected void InitComponets() {
        dragBar = AddUIComponent<UIDragHandle>();
        dragBar.size = new Vector2(MessageBoxParm.Width, MessageBoxParm.DragBarHeight);
        dragBar.relativePosition = Vector2.zero;

        title = dragBar.AddUIComponent<UILabel>();
        title.textAlignment = UIHorizontalAlignment.Center;
        title.verticalAlignment = UIVerticalAlignment.Middle;
        title.textScale = 1.3f;
        title.autoHeight = true;
        title.padding = new RectOffset(0, 0, 16, 0);
        title.font = CustomUIFontHelper.SemiBold;
        title.eventTextChanged += (c, v) => {
            title.CenterToParent();
        };

        contentPanel = AddUIComponent<AutoSizeUIScrollablePanel>();
        contentPanel.Size = new Vector2(MessageBoxParm.Width, 200);
        contentPanel.MaxSize = new Vector2(MessageBoxParm.Width, MaxScrollableContentHeight);
        contentPanel.MainPanel.autoLayoutPadding = new RectOffset(20, 20, 4, 10);
        contentPanel.MainPanel.verticalScrollbar.thumbObject.color = CustomUIColor.CPPrimaryBg;

        buttonPanel = AddUIComponent<CustomUIPanel>();
        buttonPanel.size = new Vector2(MessageBoxParm.Width, MessageBoxParm.ButtonPanelHeight);
    }
    private float MaxScrollableContentHeight => GetUIView().GetScreenResolution().y - 600f;
    protected CustomUIButton AddButtons(uint number, uint total, string text, OnButtonClicked callback) {
        var spacing = (total - 1) * MessageBoxParm.Padding;
        var buttonWidth = (MessageBoxParm.Width - 2 * MessageBoxParm.Padding - spacing) / total;
        var button = CustomUIButton.Add(buttonPanel, text, buttonWidth, buttonHeight, callback, 1f);
        button.OnBgSprites.SetColors(CustomUIColor.CPPrimaryBg, CustomUIColor.CPButtonHovered, CustomUIColor.CPButtonPressed, CustomUIColor.CPPrimaryBg, CustomUIColor.CPButtonDisabled);
        ArrangePosition(button, number, buttonWidth);
        return button;
    }
    private CustomUIButton ArrangePosition(CustomUIButton button, uint number, float buttonWidth) {
        button.name = "Button" + number.ToString();
        var posX = MessageBoxParm.Padding + (number - 1) * (buttonWidth + MessageBoxParm.Padding);
        float posY = (MessageBoxParm.ButtonPanelHeight - button.height) / 2;
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
    protected override void OnKeyDown(UIKeyEventParameter p) {
        if (!p.used) {
            if (p.keycode == KeyCode.Escape) {
                p.Use();
                Close();
            }
        }
    }
}

public record struct MessageBoxParm {
    public const float Width = 600;
    public const int Padding = 20;
    public const float ComponentWidth = 560;
    public const float DragBarHeight = 80;
    public const float ButtonPanelHeight = 74;
}