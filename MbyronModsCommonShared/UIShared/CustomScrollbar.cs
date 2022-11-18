using ColossalFramework.UI;
using UnityEngine;

namespace MbyronModsCommon {
    public class CustomScrollbar {
        public static UIScrollbar AddScrollbar(UIComponent parent, UIScrollablePanel scrollablePanel) {
            UIScrollbar scrollbar = parent.AddUIComponent<UIScrollbar>();
            scrollbar.size = new Vector2(10f, parent.height - 10);
            scrollbar.orientation = UIOrientation.Vertical;
            scrollbar.pivot = UIPivotPoint.TopLeft;
            scrollbar.minValue = 0;
            scrollbar.value = 0;
            scrollbar.incrementAmount = 50f;
            scrollbar.autoHide = true;
            UISlicedSprite trackSprite = scrollbar.AddUIComponent<UISlicedSprite>();
            trackSprite.relativePosition = Vector2.zero;
            trackSprite.autoSize = true;
            trackSprite.anchor = UIAnchorStyle.All;
            trackSprite.size = trackSprite.parent.size;
            trackSprite.fillDirection = UIFillDirection.Vertical;
            trackSprite.spriteName = "ScrollbarTrack";
            scrollbar.trackObject = trackSprite;
            UISlicedSprite thumbSprite = trackSprite.AddUIComponent<UISlicedSprite>();
            thumbSprite.relativePosition = Vector2.zero;
            thumbSprite.fillDirection = UIFillDirection.Vertical;
            thumbSprite.autoSize = true;
            thumbSprite.width = thumbSprite.parent.width;
            thumbSprite.spriteName = "ScrollbarThumb";
            scrollbar.thumbObject = thumbSprite;
            parent.eventSizeChanged += (s, e) => scrollbar.height = parent.height - 10;
            scrollablePanel.verticalScrollbar = scrollbar;
            return scrollbar;
        }
    }
}
