using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace MbyronModsCommon.UI;

public class CustomUITabContainer : UITabContainerBase<CustomUITabButton, CustomUIScrollablePanel> { }

public abstract class UITabContainerBase<TypeTab, TypeContainer> : UIComponent where TypeTab : CustomUITabButton where TypeContainer : UIComponent {
    protected UITextureAtlas atlas;
    protected string bgSprite;
    protected Color32 bgNormalColor = Color.white;
    protected Color32 bgDisabledColor = Color.white;
    protected float gap;
    protected List<TypeTab> tabs = new();
    protected List<TypeContainer> containers = new();
    protected int selectedIndex;

    public event Action<int> EventSelectedIndexChanged;
    public event Action<TypeTab> EventTabAdded;
    public event Action<TypeContainer> EventContainerAdded; 
    public event Action<Color32> EventColorChanged;

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
            if (!Equals(value, atlas)) {
                atlas = value;
                Invalidate();
            }
        }
    }
    public string BgSprite {
        get => bgSprite;
        set {
            if (value != bgSprite) {
                bgSprite = value;
                Invalidate();
            }
        }
    }
    public Color32 BgNormalColor {
        get => bgNormalColor;
        set {
            if (!bgNormalColor.Equals(value)) {
                bgNormalColor = value;
                Invalidate();
                EventColorChanged?.Invoke(value);
            }
        }
    }
    public Color32 BgDisabledColor {
        get => bgDisabledColor;
        set {
            if (!bgDisabledColor.Equals(value)) {
                bgDisabledColor = value;
                Invalidate();
                EventColorChanged?.Invoke(value);
            }
        }
    }
    public float Gap {
        get => gap;
        set {
            if (value != gap) {
                gap = value;
                Invalidate();
            }
        }
    }
    public List<TypeTab> Tabs => tabs;
    public List<TypeContainer> Containers => containers;
    public int TabsCount => Tabs.Count;
    public int ContainersCount => Containers.Count;
    public int SelectedIndex {
        get => selectedIndex;
        set {
            SelectTabByIndex(value);
        }
    }

    public TypeContainer GetContainer(int index) {
        if (index < 0 || index > TabsCount - 1) {
            return null;
        }
        return containers[index];
    }
    public TypeTab AddTab(string text = nameof(TypeTab), UIComponent containerParent = null) {
        var button = AddUIComponent<TypeTab>();
        button.eventClick += (c, e) => {
            if (c is TypeTab button)
                SelectedIndex = tabs.IndexOf(button);
        };
        EventTabAdded?.Invoke(button);
        button.Text = text;
        tabs.Add(button);
        TypeContainer containor;
        if (containerParent is not null) {
            containor = containerParent.AddUIComponent<TypeContainer>();
        } else {
            containor = AddUIComponent<TypeContainer>();
        }
        EventContainerAdded?.Invoke(containor);
        containers.Add(containor);
        ArrangeTabs();
        SelectedIndex = 0;
        return button;
    }
    public TypeContainer AddContainer(string text = nameof(TypeTab), UIComponent containerParent = null) {
        var button = AddUIComponent<TypeTab>();
        button.eventClick += (c, e) => {
            if (c is TypeTab button)
                SelectedIndex = tabs.IndexOf(button);
        };
        EventTabAdded?.Invoke(button);
        button.Text = text;
        tabs.Add(button);
        TypeContainer containor;
        if (containerParent is not null) {
            containor = containerParent.AddUIComponent<TypeContainer>();
        } else {
            containor = AddUIComponent<TypeContainer>();
        }
        EventContainerAdded?.Invoke(containor);
        containers.Add(containor);
        ArrangeTabs();
        SelectedIndex = 0;
        return containor;
    }
    public bool HideTab(string name) {
        for (int i = 0; i < TabsCount; i++) {
            if (tabs[i].name.Equals(name)) {
                tabs[i].isVisible = false;
                ArrangeTabs();
                return true;
            }
        }
        return false;
    }
    public bool ShowTab(string name) {
        for (int i = 0; i < TabsCount; i++) {
            if (tabs[i].name.Equals(name)) {
                tabs[i].isVisible = true;
                ArrangeTabs();
                return true;
            }
        }
        return false;
    }
    public void EnableTab(int index) {
        if (SelectedIndex >= 0 && SelectedIndex <= TabsCount - 1) {
            tabs[index].Enable();
        }
    }
    public void DisableTab(int index) {
        if (SelectedIndex >= 0 && SelectedIndex <= TabsCount - 1) {
            tabs[index].Disable();
        }
    }
    protected virtual void OnSelectedIndexChanged() {
        EventSelectedIndexChanged?.Invoke(SelectedIndex);
        InvokeUpward("OnSelectedIndexChanged", new object[] { SelectedIndex });
    }

    protected override void OnRebuildRenderData() {
        if (Atlas is null || string.IsNullOrEmpty(BgSprite)) {
            return;
        }
        var spriteInfo = Atlas[BgSprite];
        if (spriteInfo is null) {
            return;
        }
        renderData.material = Atlas.material;
        Color32 color = ApplyOpacity(isEnabled ? bgNormalColor : bgDisabledColor);
        RenderOptions options = new() {
            atlas = Atlas,
            color = color,
            fillAmount = 1f,
            flip = UISpriteFlip.None,
            offset = pivot.TransformToUpperLeft(size, arbitraryPivotOffset),
            pixelsToUnits = PixelsToUnits(),
            size = size,
            spriteInfo = spriteInfo
        };
        if (spriteInfo.isSliced) {
            UISlicedSpriteRender.RenderSprite(renderData, options);
            return;
        }
        UISpriteRender.RenderSprite(renderData, options);
    }

    public override void OnEnable() {
        base.OnEnable();
        if (size.sqrMagnitude < 1E-45f) {
            size = new Vector2(256f, 26f);
        }
    }
    public override void Update() {
        base.Update();
        if (m_IsComponentInvalidated) {
            ArrangeTabs();
        }
        ShowSelectedTab();
    }

    private void SelectTabByIndex(int value) {
        if (value < 0 || value > TabsCount - 1) {
            return;
        }
        selectedIndex = value;
        for (int i = 0; i < TabsCount; i++) {
            var button = tabs[i];
            if (button is not null) {
                if (i == value) {
                    button.State = SpriteState.Focused;
                } else {
                    button.State = (button.containsMouse ? SpriteState.Hovered : SpriteState.Normal);
                }
            }
        }
        for (int i = 0; i < ContainersCount; i++) {
            var container = containers[i];
            if (container is not null) {
                container.isVisible = false;
            }
        }
        containers[value].isVisible = true;
        Invalidate();
        OnSelectedIndexChanged();
    }
    private void ArrangeTabs() {
        if (TabsCount == 0 || size == Vector2.zero) {
            return;
        }
        int buttonWidth = Mathf.RoundToInt((width - (TabsCount + 1) * gap) / TabsCount);
        int buttonHeight = Mathf.RoundToInt(height - gap * 2);
        float offset = gap;
        for (int i = 0; i < TabsCount; i++) {
            var child = tabs[i];
            if (child.isVisible && child.enabled && child.gameObject.activeSelf) {
                child.size = new Vector2(buttonWidth, buttonHeight);
                child.relativePosition = new Vector2(offset, gap);
                offset += buttonWidth + gap;
            }
        }
    }
    protected virtual void ShowSelectedTab() {
        for (int i = 0; i < TabsCount; i++) {
            if (i == SelectedIndex)
                tabs[i].State = SpriteState.Focused;
            else if (!tabs[i].IsHovering)
                tabs[i].State = SpriteState.Normal;
        }
    }

}

