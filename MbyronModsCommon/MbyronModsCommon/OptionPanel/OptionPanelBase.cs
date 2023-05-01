namespace MbyronModsCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using MbyronModsCommon.UI;

public class OptionPanelBase<TypeMod, TypeConfig, TypeOptionPanel> : CustomUIPanel where TypeMod : IMod where TypeConfig : ModConfigBase<TypeConfig>, new() where TypeOptionPanel : CustomUIPanel {
    public static readonly Vector2 Size = new(764, 773);
    public static readonly float MainPadding = 16;
    public static readonly float MainWidth = Size.x - 2 * MainPadding;
    public static readonly Vector2 TabSize = new(MainWidth, 30);
    public static readonly Vector2 ContainerSize = new(MainWidth, Size.y - 2 * MainPadding - 30 - 10);
    protected CustomUITabContainer tabContainer;

    protected CustomUIScrollablePanel GeneralContainer { get; private set; }
    protected CustomUIScrollablePanel HotkeyContainer { get; private set; }
    protected CustomUIScrollablePanel AdvancedContainer { get; private set; }

    public OptionPanelBase() {
        size = Size;
        atlas = CustomUIAtlas.MbyronModsAtlas;
        bgSprite = CustomUIAtlas.CustomBackground;
        bgNormalColor = new Color32(130, 130, 130, 255);
        tabContainer = AddUIComponent<CustomUITabContainer>();
        tabContainer.size = TabSize;
        tabContainer.Gap = 3;
        tabContainer.Atlas = CustomUIAtlas.MbyronModsAtlas;
        tabContainer.BgSprite = CustomUIAtlas.RoundedRectangle2;
        tabContainer.BgNormalColor = CustomUIColor.OPPrimaryBg;
        tabContainer.relativePosition = new(MainPadding, MainPadding);
        tabContainer.EventTabAdded += (_) => {
            _.SetDefaultOptionPanelStyle();
            _.TextPadding = new RectOffset(0, 0, 2, 0);
        };
        tabContainer.EventContainerAdded += (_) => {
            _.size = ContainerSize;
            _.autoLayoutPadding = new RectOffset(0, 0, 4, 20);
            var scrollbar0 = UIScrollbarHelper.AddScrollbar(this, _, new Vector2(8, ContainerSize.y));
            scrollbar0.thumbObject.color = CustomUIColor.OPPrimaryBg;
            scrollbar0.relativePosition = new Vector2(Size.x - 8, MainPadding + 40);
            _.relativePosition = new Vector2(MainPadding, MainPadding + 30 + 10);
        };

        AddGeneralContainer();
        AddExtraContainer();
        AddAdvancedContainer();
    }

    private void AddGeneralContainer() {
        GeneralContainer = AddTab(CommonLocalize.OptionPanel_General);
        OptionPanelHelper.AddGroup(GeneralContainer, CommonLocalize.OptionPanel_ModInfo);
        var flag = ModMainInfo<TypeMod>.VersionType switch {
            BuildVersion.Stable => "STABLE",
            BuildVersion.Beta => "BETA",
            _ => "DEBUG"
        };
        var panel0 = OptionPanelHelper.AddLabel($"{ModMainInfo<TypeMod>.ModName}", $"{ModMainInfo<TypeMod>.ModVersion} {flag}");
        var label0 = panel0.Child as CustomUILabel;
        label0.BgNormalColor = (ModMainInfo<TypeMod>.VersionType == BuildVersion.Stable) ? new Color32(76, 148, 10, 255) : ((ModMainInfo<TypeMod>.VersionType == BuildVersion.Beta) ? new Color32(188, 120, 6, 255) : new Color32(6, 132, 138, 255));
        label0.TextPadding = new(4, 4, 4, 2);
        label0.Atlas = CustomUIAtlas.MbyronModsAtlas;
        label0.BgSprite = CustomUIAtlas.RoundedRectangle2;
        label0.width += 8;
        panel0.StartLayout();
        AddExtraModInfoProperty();
        OptionPanelHelper.AddDropDown(CommonLocalize.Language, null, GetLanguages().ToArray(), LanguagesIndex, 310, 30, (v) => {
            OnLanguageSelectedIndexChanged<TypeConfig>(v);
            AddLanguageSelectedIndexChanged();
        });
        OptionPanelHelper.Reset();
        FillGeneralContainer();
    }

    protected virtual void AddExtraContainer() => FillHotkeyContainer();

    protected virtual void FillGeneralContainer() { }
    protected virtual void FillHotkeyContainer() => HotkeyContainer = AddTab(CommonLocalize.OptionPanel_Hotkeys);
    protected virtual void FillAdvancedContainer() { }
    protected virtual void AddExtraModInfoProperty() { }
    protected virtual void AddLanguageSelectedIndexChanged() { }
    private void AddAdvancedContainer() {
        AdvancedContainer = AddTab(CommonLocalize.OptionPanel_Advanced);
        OptionPanelHelper.AddGroup(AdvancedContainer, CommonLocalize.OptionPanel_Advanced);
        OptionPanelHelper.AddToggle(SingletonMod<TypeConfig>.Instance.DebugMode, CommonLocalize.OptionPanel_DebugMode, CommonLocalize.OptionPanel_DebugMinor, _ => SingletonMod<TypeConfig>.Instance.DebugMode = _);
        OptionPanelHelper.AddButton(CommonLocalize.ChangeLog_Major, null, CommonLocalize.ChangeLog, 250, 30, ShowLog);
        OptionPanelHelper.AddButton(CommonLocalize.CompatibilityCheck_Major, CommonLocalize.CompatibilityCheck_Minor, CommonLocalize.Check, 250, 30, ShowCompatibility);
        OptionPanelHelper.AddButton(CommonLocalize.ResetModMajor, CommonLocalize.ResetModMinor, CommonLocalize.Reset, 250, 30, ResetSettings);
        OptionPanelHelper.Reset();
    }
    protected CustomUIScrollablePanel AddTab(string text) => tabContainer.AddContainer(text, this);

    protected static void OnLanguageSelectedIndexChanged<Panel>(int value) {
        if (value == 0) {
            SingletonMod<TypeMod>.Instance.ModCulture = new CultureInfo(ModLocalize.LocalizationExtension());
            SingletonMod<TypeConfig>.Instance.ModLanguage = "GameLanguage";
        } else {
            SingletonMod<TypeMod>.Instance.ModCulture = new CultureInfo(ModLocalize.ModLanguageOptionIDList[value]);
            SingletonMod<TypeConfig>.Instance.ModLanguage = ModLocalize.ModLanguageOptionIDList[value];
        }
        OptionPanelManager<TypeMod, TypeOptionPanel>.LocaleChanged();
    }

    protected static int LanguagesIndex => ModLocalize.ModLanguageOptionIDList.FindIndex(x => x == SingletonMod<TypeConfig>.Instance.ModLanguage);
    protected static List<string> GetLanguages() {
        List<string> result = new();
        var IDs = ModLocalize.ModLanguageOptionIDList;
        var currentCulture = SingletonMod<TypeMod>.Instance.ModCulture;
        result.Add(CommonLocalize.ResourceManager.GetString($"Language_GameLanguage", currentCulture));
        for (int i = 1; i < IDs.Count; i++) {
            var prefix = CommonLocalize.ResourceManager.GetString($"Language_{IDs[i].Replace('-', '_')}", new CultureInfo(IDs[i]));
            string suffix = "(" + CommonLocalize.ResourceManager.GetString($"Language_{IDs[i].Replace('-', '_')}", currentCulture) + ")";
            var total = prefix + suffix;
            if (IDs[i] == currentCulture.Name) {
                result.Add(prefix);
            } else {
                result.Add(total);
            }
        }
        return result;
    }

    ResetModWarningMessageBox messageBox;
    ResetModMessageBox messageBox1;

    protected void ResetSettings() {
        try {
            messageBox = MessageBox.Show<ResetModWarningMessageBox>();
            messageBox.Init<TypeMod>(First);
        }
        catch (Exception e) {
            InternalLogger.Exception($"Reset settings failed:", e);
            MessageBox.Show<ResetModMessageBox>().Init<TypeMod>(false);
        }

        void First() {
            InternalLogger.Log($"Start resetting mod config.");
            SingletonMod<TypeConfig>.Instance = null;
            SingletonMod<TypeConfig>.Instance = new();
            OptionPanelManager<TypeMod, TypeOptionPanel>.LocaleChanged();
            InternalLogger.Log($"Reset mod config succeeded.");
            MessageBox.Hide(messageBox);
            messageBox1 = MessageBox.Show<ResetModMessageBox>();
            messageBox1.Init<TypeMod>(true);
        }
    }

    private static void ShowLog() => MessageBox.Show<LogMessageBox>().Initialize<TypeMod>(false);
    private static void ShowCompatibility() => MessageBox.Show<CompatibilityMessageBox>().Initialize(ModMainInfo<TypeMod>.ModName);

}


