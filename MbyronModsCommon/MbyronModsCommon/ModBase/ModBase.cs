using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using ColossalFramework.IO;
using System.IO;
using System.Globalization;
using ColossalFramework.Globalization;

namespace MbyronModsCommon {
    public class ModMainInfo<Mod> : SingletonMod<Mod> where Mod : IMod {
        public static string SolidModName => Instance.SolidModName;
        public static string ModName => Instance.ModName;
        public static Version ModVersion => Instance.ModVersion;
        public static ulong ModID => Instance.ModID;
        public static string VersionType => Instance.VersionType;
    }

    public abstract class ModBase<Mod, OptionPanel, Config> : IMod where Mod : ModBase<Mod, OptionPanel, Config> where OptionPanel : UIPanel where Config : ModConfigBase<Config>, new() {
        public abstract string SolidModName { get; }
        public abstract string ModName { get; }
        public abstract Version ModVersion { get; }
        public abstract ulong ModID { get; }
        public virtual ulong? BetaID { get; }
        public bool IsBeta => BetaID.HasValue;
        public string Name => IsBeta ? ModName + " Beta " + ModVersion : ModName + " " + ModVersion;
        public abstract string Description { get; }
        public abstract List<ModChangeLog> ChangeLog { get; }
        public string VersionType => IsBeta ? "Beta" : "Stable";
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
            SingletonMod<Mod>.Instance = (Mod)this;
            InternalLogger.Log($"Start initializing mod.");
            ExternalLogger.CreateDebugFile<Mod>();
            LoadConfig();
            CompatibilityCheck.ModName = ModName;
        }

        public void OnSettingsUI(UIHelperBase helper) {
            InternalLogger.Log($"Setting UI.");
            LoadLocale();
            LocaleManager.eventLocaleChanged += LoadLocale;
            OptionPanelManager<Mod, OptionPanel>.SettingsUI(helper);
            SettingsUI(helper);
        }

        protected virtual void SettingsUI(UIHelperBase helper) { }

        protected void LoadLocale() {
            CultureInfo locale;
            try {
                if (SingletonMod<Config>.Instance.ModLanguage == "GameLanguage") {
                    var culture = ModLocalize.UseGameLanguage();
                    locale = new CultureInfo(culture);
                    InternalLogger.Log($"Change mod locale, use game language: {locale}.");
                } else {
                    locale = new CultureInfo(SingletonMod<Config>.Instance.ModLanguage);
                    InternalLogger.Log($"Change mod locale, use custom language: {locale}.");
                }
                ModCulture = locale;
            }
            catch (Exception e) {
                InternalLogger.Exception($"Could't change mod locale", e);
            }
        }

        public void LoadConfig() => XMLUtils.LoadData<Config>(ConfigFilePath);
        public void SaveConfig() => XMLUtils.SaveData<Config>(ConfigFilePath);


        public virtual void OnEnabled() {
            if (UIView.GetAView() is not null) {
                OptionPanelManager<Mod, OptionPanel>.OptionsEventHook();
            } else {
                LoadingManager.instance.m_introLoaded += OptionPanelManager<Mod, OptionPanel>.OptionsEventHook;
            }
            LoadingManager.instance.m_introLoaded += IntroActions;
        }

        public virtual void IntroActions() => GetDeserializationState();

        public virtual void OnDisabled() { }
        public virtual void OnCreated(ILoading loading) { }
        public virtual void OnLevelLoaded(LoadMode mode) {
            ShowLogMessageBox();
            OptionPanelManager<Mod, OptionPanel>.OptionsEventHook();
        }
        public virtual void OnLevelUnloading() { }
        public virtual void OnReleased() { }

        private void ShowLogMessageBox() {
            if (IsBeta) {
                SingletonMod<Config>.Instance.ModVersion = ModVersion.ToString();
                SaveConfig();
                return;
            }
            if (!string.IsNullOrEmpty(SingletonMod<Config>.Instance.ModVersion)) {
                var lastVersion = new Version(SingletonMod<Config>.Instance.ModVersion);
                if ((lastVersion.Major == ModVersion.Major) && (lastVersion.Minor == ModVersion.Minor) && (lastVersion.Build == ModVersion.Build)) {
                    SingletonMod<Config>.Instance.ModVersion = ModVersion.ToString();
                    SaveConfig();
                    return;
                }
                if (lastVersion < ModVersion) {
                    var messageBox = MessageBox.Show<LogMessageBox>();
                    messageBox.Initialize<Mod>(true);
                }
                SingletonMod<Config>.Instance.ModVersion = ModVersion.ToString();
                SaveConfig();
            } else {
                InternalLogger.Error("Updated version failed, mod version is null or empty in config file.");
            }
        }

        private void GetDeserializationState() {
            if (!XMLUtils.DeserializationState) {
                var messageBox = MessageBox.Show<XMLWariningMessageBox>();
                messageBox.Initialize<Mod>();
            }
        }

    }

    public struct ModChangeLog {
        public Version ModVersion;
        public DateTime Date;
        public List<string> Log;
        public ModChangeLog(Version version, DateTime date, List<string> log) {
            ModVersion = version;
            Date = date;
            Log = log;

        }
    }

    public abstract class SingletonMod<Type> {
        public static Type Instance { get; set; }
    }

    public interface IMod : IUserMod, ILoadingExtension {
        string VersionType { get; }
        string SolidModName { get; }
        string ModName { get; }
        List<ModChangeLog> ChangeLog { get; }
        Version ModVersion { get; }
        ulong ModID { get; }
        CultureInfo ModCulture { get; set; }
        void SaveConfig();
        void LoadConfig();

    }
}
