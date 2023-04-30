using ColossalFramework.UI;
using UnityEngine;
namespace MbyronModsCommon.UI;

public static class UIScrollbarHelper {
    public static UIScrollbar AddScrollbar(UIComponent parent, UIScrollablePanel scrollablePanel, Vector2 size) {
        var scrollbar = parent.AddUIComponent<UIScrollbar>();
        scrollbar.size = size;
        scrollbar.orientation = UIOrientation.Vertical;
        scrollbar.pivot = UIPivotPoint.TopLeft;
        scrollbar.minValue = 0;
        scrollbar.value = 0;
        scrollbar.incrementAmount = 50f;
        scrollbar.autoHide = true;
        var trackSprite = scrollbar.AddUIComponent<UISlicedSprite>();
        trackSprite.relativePosition = Vector2.zero;
        trackSprite.autoSize = true;
        trackSprite.anchor = UIAnchorStyle.All;
        trackSprite.size = trackSprite.parent.size;
        trackSprite.fillDirection = UIFillDirection.Vertical;
        scrollbar.trackObject = trackSprite;
        var thumbSprite = scrollbar.AddUIComponent<UISlicedSprite>();
        thumbSprite.relativePosition = Vector2.zero;
        thumbSprite.fillDirection = UIFillDirection.Vertical;
        thumbSprite.autoSize = true;
        thumbSprite.width = thumbSprite.parent.width;
        thumbSprite.atlas = CustomUIAtlas.MbyronModsAtlas;
        thumbSprite.spriteName = CustomUIAtlas.RoundedRectangle1;
        thumbSprite.color = CustomUIColor.White;
        scrollbar.thumbObject = thumbSprite;
        scrollablePanel.verticalScrollbar = scrollbar;
        return scrollbar;
    }
}

