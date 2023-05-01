namespace MbyronModsCommon;
using ICities;
using System;
using System.Collections.Generic;
using ColossalFramework.IO;
using System.IO;
using System.Globalization;
using ColossalFramework.Globalization;

public class ModMainInfo<Mod> : SingletonMod<Mod> where Mod : IMod {
    public static string SolidModName => Instance.SolidModName;
    public static string ModName => Instance.ModName;
    public static Version ModVersion => Instance.ModVersion;
    public static ulong StableID => Instance.StableID;
    public static BuildVersion VersionType => Instance.VersionType;
    public static string Name => Instance.Name;
}

public abstract class ModBase<M, C> : IMod where M : ModBase<M, C> where C : ModConfigBase<C>, new() {
    public abstract string SolidModName { get; }
    public abstract string ModName { get; }
    public abstract Version ModVersion { get; }
    public abstract ulong StableID { get; }
    public virtual ulong? BetaID { get; }
    public string Name => VersionType switch {
        BuildVersion.Debug => ModName + " [DEBUG] " + ModVersion,
        BuildVersion.Beta => ModName + " [BETA] " + ModVersion,
        _ => ModName + ' ' + ModVersion,
    };
    public abstract string Description { get; }
    public abstract List<ModChangeLog> ChangeLog { get; }
    public abstract BuildVersion VersionType { get; }

    public string ConfigFilePath => Path.Combine(DataLocation.localApplicationData, $"{SolidModName}Config.xml");

    private CultureInfo modCulture;
    public CultureInfo ModCulture {
        get => modCulture;
        set {
            modCulture = value;
            CommonLocalize.Culture = value;
            SetModCulture(value);
        }
    }
    public virtual void SetModCulture(CultureInfo cultureInfo) { }
    public abstract string GetLocale(string text);

    public ModBase() {
        SingletonMod<M>.Instance = (M)this;
        InternalLogger.Log($"Start initializing mod.");
        ExternalLogger.CreateDebugFile<M>();
        LoadConfig();
        CompatibilityCheck.ModName = ModName;
    }

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
            if (SingletonMod<C>.Instance.ModLanguage == "GameLanguage") {
                var culture = ModLocalize.UseGameLanguage();
                locale = new CultureInfo(culture);
                InternalLogger.Log($"Change mod locale, use game language: {locale}.");
            } else {
                locale = new CultureInfo(SingletonMod<C>.Instance.ModLanguage);
                InternalLogger.Log($"Change mod locale, use custom language: {locale}.");
            }
            ModCulture = locale;
        }
        catch (Exception e) {
            InternalLogger.Exception($"Could't change mod locale", e);
        }
    }

    public void LoadConfig() => XMLUtils.LoadData<C>(ConfigFilePath);
    public void SaveConfig() => XMLUtils.SaveData<C>(ConfigFilePath);
    public virtual void OnEnabled() {
        LoadingManager.instance.m_introLoaded += IntroActions;
    }
    public virtual void IntroActions() => GetDeserializationState();

    public virtual void OnDisabled() { }
    public virtual void OnCreated(ILoading loading) { }
    public virtual void OnLevelLoaded(LoadMode mode) {
        ShowLogMessageBox();
    }
    public virtual void OnLevelUnloading() { }
    public virtual void OnReleased() { }

    private void ShowLogMessageBox() {
        if (VersionType != BuildVersion.Beta) {
            SingletonMod<C>.Instance.ModVersion = ModVersion.ToString();
            SaveConfig();
            return;
        }
        if (!string.IsNullOrEmpty(SingletonMod<C>.Instance.ModVersion)) {
            var lastVersion = new Version(SingletonMod<C>.Instance.ModVersion);
            if ((lastVersion.Major == ModVersion.Major) && (lastVersion.Minor == ModVersion.Minor) && (lastVersion.Build == ModVersion.Build)) {
                SingletonMod<C>.Instance.ModVersion = ModVersion.ToString();
                SaveConfig();
                return;
            }
            if (lastVersion < ModVersion) {
                var messageBox = MessageBox.Show<LogMessageBox>();
                messageBox.Initialize<M>(true);
            }
            SingletonMod<C>.Instance.ModVersion = ModVersion.ToString();
            SaveConfig();
        } else {
            InternalLogger.Error("Updated version failed, mod version is null or empty in config file.");
        }
    }
    private void GetDeserializationState() {
        if (!XMLUtils.DeserializationState) {
            var messageBox = MessageBox.Show<XMLWariningMessageBox>();
            messageBox.Initialize<M>();
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
    Debug,
    Beta,
    Stable
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

public class ModConfigBase<Config> : SingletonMod<Config>, IModConfig where Config : ModConfigBase<Config> {
    public string ModVersion { get; set; } = "0.0";
    public string ModLanguage { get; set; } = "GameLanguage";
    public bool DebugMode { get; set; } = false;
}

public interface IModConfig {
    string ModVersion { get; set; }
    string ModLanguage { get; set; }
}

public abstract class SingletonMod<Type> {
    public static Type Instance { get; set; }
}

public interface IMod : IUserMod, ILoadingExtension {
    BuildVersion VersionType { get; }
    string SolidModName { get; }
    string ModName { get; }
    List<ModChangeLog> ChangeLog { get; }
    Version ModVersion { get; }
    ulong StableID { get; }
    CultureInfo ModCulture { get; set; }
    void SaveConfig();
    void LoadConfig();
}