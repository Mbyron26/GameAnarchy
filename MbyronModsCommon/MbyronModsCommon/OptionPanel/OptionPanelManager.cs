﻿using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ICities;
using System;
using System.Diagnostics;
using UnityEngine;

namespace MbyronModsCommon {
    public class OptionPanelManager<Mod, OptionPanel> where OptionPanel : UIPanel where Mod : IMod {
        private static OptionPanel Panel { get; set; }
        private static UIScrollablePanel BaseScrollablePanel { get; set; }
        private static UIPanel BasePanel { get; set; }
        private static GameObject ContainerGameObject { get; set; }
        private static UIPanel Container { get; set; }
        public static void OptionsEventHook() {
            Container = UIView.library.Get<UIPanel>("OptionsPanel").Find<UITabContainer>("OptionsContainer").Find<UIPanel>(ModMainInfo<IMod>.Name);
            if (Container is null) {
                InternalLogger.Error("Couldn't find game options panel.");
            } else {
                Container.eventVisibilityChanged += (c, isVisible) => {
                    if (isVisible) {
                        Create();
                        //SingletonMod<Mod>.Instance.SaveConfig();
                    } else {
                        Destroy();
                    }
                };
                LocaleManager.eventLocaleChanged += LocaleChanged;
            }
        }

        public static void LocaleChanged() {
            if (Container is not null && Container.isVisible) {
                Destroy();
                Create();
            }
        }

        private static void Destroy() {
            if (ContainerGameObject is not null) {
                SingletonMod<Mod>.Instance.SaveConfig();
                //UnityEngine.Object.Destroy(Panel.gameObject);
                UnityEngine.Object.Destroy(Panel);
                UnityEngine.Object.Destroy(ContainerGameObject);
                Panel = null;
                ContainerGameObject = null;
            }
        }

        public static void Create() {
            try {
                if (ContainerGameObject is null) {
                    ContainerGameObject = new(typeof(OptionPanel).Name);
                    ContainerGameObject.transform.parent = BasePanel.transform;
                    Panel = ContainerGameObject.AddComponent<OptionPanel>();
                    Panel.relativePosition = Vector2.zero;
                }
            }
            catch (Exception e) {
                InternalLogger.Exception("Create option panel object failed.", e);
            }
        }


        public static void SettingsUI(UIHelperBase helper) {
            BaseScrollablePanel = ((UIHelper)helper).self as UIScrollablePanel;
            BaseScrollablePanel.autoLayout = false;
            BasePanel = BaseScrollablePanel.parent as UIPanel;
            foreach (var components in BasePanel.components)
                components.isVisible = false;
            BasePanel.autoLayout = false;
        }

    }
}
