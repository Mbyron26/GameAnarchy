using ColossalFramework.UI;
using System;
using UnityEngine;

namespace MbyronModsCommon.UI {
    public abstract class MultiStateButtonBase : UIMultiStateButton {
        public event Action<bool> EventOnCheckedChanged;

        public SpriteSetState FgSpriteSetState => foregroundSprites;
        public SpriteSetState BgSpriteSetState => backgroundSprites;

        public SpriteSet FgSpriteSet0 {
            get {
                if (FgSpriteSetState[0] is null) {
                    FgSpriteSetState.AddState();
                }
                return FgSpriteSetState[0];
            }
        }

        public SpriteSet BgSpriteSet0 {
            get {
                if (BgSpriteSetState[0] is null) {
                    BgSpriteSetState.AddState();
                }
                return BgSpriteSetState[0];
            }
        }

        public SpriteSet FgSpriteSet1 {
            get {
                if (FgSpriteSetState.Count == 1) {
                    FgSpriteSetState.AddState();
                }
                return FgSpriteSetState[1];
            }
        }

        public SpriteSet BgSpriteSet1 {
            get {
                if (BgSpriteSetState.Count == 1) {
                    BgSpriteSetState.AddState();
                }
                return BgSpriteSetState[1];
            }
        }

        public virtual bool IsChecked {
            get => activeStateIndex == 1;
            set {
                if (value) {
                    activeStateIndex = 1;
                } else {
                    activeStateIndex = 0;
                }
            }
        }

        public MultiStateButtonBase() {
            state = ButtonState.Normal;
            foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            spritePadding = new RectOffset(0, 0, 0, 0);
            canFocus = false;
            isInteractive = true;
            eventActiveStateIndexChanged += OnEventActiveStateIndexChanged;
        }

        protected virtual void OnEventActiveStateIndexChanged(UIComponent component, int value) => EventOnCheckedChanged?.Invoke(value != 0);

        public abstract void SetSprite();
    }
}
