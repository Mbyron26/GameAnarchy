using ColossalFramework.UI;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class AutoSizeUIScrollablePanel : CustomUIPanel {
    public CustomUIScrollablePanel MainPanel { get; private set; }
    public Vector2 MaxSize {
        get => maximumSize;
        set {
            if (value != maximumSize) {
                maximumSize = value;
                MainPanel.maximumSize = value;
            }
        }
    }
    public Vector2 Size {
        get => size;
        set {
            if (value != size) {
                size = value;
                if (MainPanel is null) {
                    MainPanel.size = value;
                }

            }
        }
    }

    public AutoSizeUIScrollablePanel() {
        MainPanel = AddUIComponent<CustomUIScrollablePanel>();
        MainPanel.relativePosition = Vector2.zero;
        MainPanel.eventComponentAdded += (p, c) => {
            FitContentChildren();
            c.eventVisibilityChanged += OnChildVisibilityChanged;
            c.eventSizeChanged += OnChildSizeChanged;
        };
        MainPanel.eventComponentRemoved -= (p, c) => {
            FitContentChildren();
            c.eventVisibilityChanged -= OnChildVisibilityChanged;
            c.eventSizeChanged -= OnChildSizeChanged;
        };
        UIScrollbarHelper.AddScrollbar(this, MainPanel, new Vector2(8, 20));
        MainPanel.eventSizeChanged += (s, e) => MainPanel.verticalScrollbar.height = MainPanel.height;
    }

    private void OnChildVisibilityChanged(UIComponent component, bool value) => FitContentChildren();
    private void OnChildSizeChanged(UIComponent component, Vector2 value) => FitContentChildren();
    private void FitContentChildren() {
        var height = 0f;
        foreach (var component in MainPanel.components) {
            if (component.isVisibleSelf) {
                height += component.height;
                height += MainPanel.autoLayoutPadding.vertical;
            }
        }
        MainPanel.height = this.height = height;
        if (MainPanel.verticalScrollbar is not null) {
            MainPanel.verticalScrollbar.relativePosition = new Vector2(width - MainPanel.verticalScrollbar.width, 0);
        }
    }

}

public class CustomUIScrollablePanel : UIScrollablePanel {
    public CustomUIScrollablePanel() {
        autoLayout = true;
        autoLayoutDirection = LayoutDirection.Vertical;
        scrollWheelDirection = UIOrientation.Vertical;
        builtinKeyNavigation = true;
        clipChildren = true;
    }

    protected override void OnVisibilityChanged() {
        base.OnVisibilityChanged();
        if (verticalScrollbar != null) {
            verticalScrollbar.isVisible = isVisibleSelf;
        }
    }

}


