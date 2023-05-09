namespace MbyronModsCommon;
using ICities;
using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.Globalization;

public class ModMainInfo<Mod> : SingletonMod<Mod> where Mod : IMod {
    public static string RawName => Instance.RawName;
    public static string ModName => Instance.ModName;
    public static Version ModVersion => Instance.ModVersion;
    public static BuildVersion VersionType => Instance.VersionType;
}

public abstract class ModBase<TypeMod, TypeConfig> : IMod where TypeMod : ModBase<TypeMod, TypeConfig> where TypeConfig : ModConfig<TypeConfig>, new() {
    private CultureInfo modCulture;

    public virtual string RawName => AssemblyUtils.CurrentAssemblyName;
    public abstract string ModName { get; }
    public virtual Version ModVersion => AssemblyUtils.CurrentAssemblyVersion;
    public abstract ulong StableID { get; }
    public virtual ulong? BetaID { get; }
    public abstract BuildVersion VersionType { get; }
    public string Name => VersionType switch {
        BuildVersion.BetaDebug or BuildVersion.BetaRelease => ModName + " [BETA] " + ModVersion,
        _ => ModName + ' ' + ModVersion,
    };
    public virtual string Description => string.Empty;
    public bool IsEnabled { get; private set; }
    public abstract List<ModChangeLog> ChangeLog { get; }
    public CultureInfo ModCulture {
        get => modCulture;
        set {
            modCulture = value;
            CommonLocalize.Culture = value;
            SetModCulture(value);
        }
    }

    public ModBase() {
        InternalLogger.Log($"Start initializing mod.");
        SingletonMod<TypeMod>.Instance = (TypeMod)this;
        ExternalLogger.CreateDebugFile<TypeMod>();
        LoadConfig();
        CompatibilityCheck.ModName = ModName;
    }

    public virtual void SetModCulture(CultureInfo cultureInfo) { }
    public virtual IEnumerable<string> GetSupportLocales() => CommonLocalize.LocaleManager.GetSupportLocales();
    public abstract string GetLocale(string text);
    public void OnSettingsUI(UIHelperBase helper) {
        InternalLogger.Log($"Setting UI.");
        LoadLocale();
        LocaleManager.eventLocaleChanged += LoadLocale;
        SettingsUI(helper);
    }
    protected virtual void SettingsUI(UIHelperBase helper) { }
    protected void LoadLocale() {
        CultureInfo locale;
        try {
            if (SingletonItem<TypeConfig>.Instance.ModLanguage == "GameLanguage") {
                var culture = Language.GetGameLanguage();
                locale = new CultureInfo(culture);
                InternalLogger.Log($"Change mod locale, use game language: {locale}");
            } else {
                locale = new CultureInfo(SingletonItem<TypeConfig>.Instance.ModLanguage);
                InternalLogger.Log($"Change mod locale, use custom language: {locale}");
            }
            ModCulture = locale;
        } catch (Exception e) {
            InternalLogger.Exception($"Could't change mod locale", e);
        }
    }
    public void LoadConfig() => ModConfig<TypeConfig>.Load();
    public void SaveConfig() => ModConfig<TypeConfig>.Save();
    public void OnEnabled() {
        InternalLogger.Log("Enabled");
        IsEnabled = true;
        Enable();
    }
    public void OnDisabled() {
        InternalLogger.Log("Disabled");
        IsEnabled = false;
        Disable();
    }

    protected virtual void Enable() => LoadingManager.instance.m_introLoaded += IntroActions;
    protected virtual void Disable() { }
    public virtual void IntroActions() { }

    public virtual void OnCreated(ILoading loading) { }
    public virtual void OnLevelLoaded(LoadMode mode) {
        ShowLogMessageBox();
    }
    public virtual void OnLevelUnloading() { }
    public virtual void OnReleased() { }

    private void ShowLogMessageBox() {
        if (VersionType != BuildVersion.StableRelease) {
            SingletonItem<TypeConfig>.Instance.ModVersion = ModVersion.ToString();
            SaveConfig();
            return;
        }
        if (!string.IsNullOrEmpty(SingletonItem<TypeConfig>.Instance.ModVersion)) {
            var lastVersion = new Version(SingletonItem<TypeConfig>.Instance.ModVersion);
            if ((lastVersion.Major == ModVersion.Major) && (lastVersion.Minor == ModVersion.Minor) && (lastVersion.Build == ModVersion.Build)) {
                SingletonItem<TypeConfig>.Instance.ModVersion = ModVersion.ToString();
                SaveConfig();
                return;
            }
            if (lastVersion < ModVersion) {
                var messageBox = MessageBox.Show<LogMessageBox>();
                messageBox.Initialize<TypeMod>(true);
            }
            SingletonItem<TypeConfig>.Instance.ModVersion = ModVersion.ToString();
            SaveConfig();
        } else {
            InternalLogger.Error("Updated version failed, mod version is null or empty in config file.");
        }
    }
}

public struct ModChangeLog {
    public Version ModVersion;
    public DateTime Date;
    public List<LogString> Log;
    public ModChangeLog(Version version, DateTime date, List<LogString> log) {
        ModVersion = version;
        Date = date;
        Log = log;
    }
}

public struct LogString {
    public LogFlag Flag = LogFlag.None;
    public string Content = string.Empty;
    public LogString(LogFlag logFlag, string content) {
        Flag = logFlag;
        Content = content;
    }
}
public enum LogFlag {
    None,
    Added,
    Removed,
    Updated,
    Fixed,
    Optimized,
    Translation,
    Attention,
}

public enum BuildVersion {
    BetaDebug,
    BetaRelease,
    StableDebug,
    StableRelease
}

public class ModThreadExtensionBase : ThreadingExtensionBase {
    public void AddCallOnceInvoke(bool target, ref bool flag, Action action) {
        if (target) {
            if (!flag) {
                flag = true;
                action.Invoke();
            }
        } else {
            flag = false;
        }
    }
}

public abstract class SingletonMod<Type> {
    public static Type Instance { get; set; }
}

public abstract class SingletonItem<T> {
    public static T Instance { get; set; }
}

public interface IMod : IUserMod, ILoadingExtension {
    BuildVersion VersionType { get; }
    string RawName { get; }
    string ModName { get; }
    List<ModChangeLog> ChangeLog { get; }
    Version ModVersion { get; }
    ulong StableID { get; }
    CultureInfo ModCulture { get; set; }
    void SaveConfig();
    void LoadConfig();
    IEnumerable<string> GetSupportLocales();
}