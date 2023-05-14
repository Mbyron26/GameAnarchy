namespace MbyronModsCommon;
using ColossalFramework.UI;
using MbyronModsCommon.UI;
using UnityEngine;

public abstract class ControlPanelBase<TypeMod, TypePanel> : CustomUIPanel where TypeMod : IMod where TypePanel : CustomUIPanel {
    private const string Name = nameof(AssemblyUtils.CurrentAssemblyName) + "ControlPanel";
    protected UIDragHandle dragBar;
    protected CustomUILabel title;
    protected CustomUIButton closeButton;

    public virtual float PanelWidth { get; protected set; } = 440;
    public virtual float PanelHeight { get; protected set; } = 600;
    public virtual float ElementOffset { get; protected set; } = 10;
    public virtual float CaptionHeight { get; protected set; } = 40;
    public static Vector2 PanelPosition { get; set; }
    public static Vector2 ButtonSize { get; } = new(28, 28);

    public ControlPanelBase() {
        name = Name;
        atlas = CustomUIAtlas.MbyronModsAtlas;
        bgSprite = CustomUIAtlas.CustomBackground;
        isVisible = true;
        canFocus = true;
        isInteractive = true;
        size = new Vector2(PanelWidth, PanelHeight);
        InitComponents();
        SetPosition();
        eventPositionChanged += (c, v) => PanelPosition = relativePosition;
    }

    protected virtual void InitComponents() => AddCaption();
    protected virtual void SetPosition() {
        if (PanelPosition == Vector2.zero) {
            Vector2 vector = GetUIView().GetScreenResolution();
            var x = vector.x - PanelWidth - 360;
            PanelPosition = relativePosition = new Vector3(x, 80);
        } else {
            relativePosition = PanelPosition;
        }
    }
    private void AddCaption() {
        closeButton = AddUIComponent<CustomUIButton>();
        closeButton.Atlas = CustomUIAtlas.MbyronModsAtlas;
        closeButton.size = ButtonSize;
        closeButton.OnBgSprites.SetSprites(CustomUIAtlas.CloseButton);
        closeButton.OnBgSprites.SetColors(CustomUIColor.White, CustomUIColor.OffWhite, new Color32(180, 180, 180, 255), CustomUIColor.White, CustomUIColor.White);
        closeButton.relativePosition = new Vector2(width - 6f - 28f, 6f);
        closeButton.eventClicked += (c, p) => ControlPanelManager<TypePanel>.Close();

        dragBar = AddUIComponent<UIDragHandle>();
        dragBar.width = closeButton.relativePosition.x;
        dragBar.height = CaptionHeight;
        dragBar.relativePosition = Vector2.zero;

        title = dragBar.AddUIComponent<CustomUILabel>();
        title.Text = ModMainInfo<TypeMod>.ModName;
        title.CenterToParent();
    }
}