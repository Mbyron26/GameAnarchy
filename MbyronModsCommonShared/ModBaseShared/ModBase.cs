using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using ColossalFramework.IO;
using System.IO;
using System.Globalization;
using MessageBoxShared;
using ColossalFramework.Globalization;
using ColossalFramework;

namespace MbyronModsCommon {
    public class ModMainInfo<Mod> : SingletonMod<Mod> where Mod : IMod {
        public static string SolidModName => Instance.SolidModName;
        public static string ModName => Instance.ModName;
        public static Version ModVersion => Instance.ModVersion;
        public static ulong ModID => Instance.ModID;
    }

    public abstract class ModBase<Mod, OptionPanel, Config> : IMod where Mod : ModBase<Mod, OptionPanel, Config> where OptionPanel : UIPanel where Config : ModConfigBase<Config>, new() {
        public abstract string SolidModName { get; }
        public abstract string ModName { get; }
        public abstract Version ModVersion { get; }
        public abstract ulong ModID { get; }
        public string Name => ModName + " " + ModVersion;
        public abstract string Description { get; }

        private string configFilePath;
        public string ConfigFilePath {
            get {
                if (configFilePath.IsNullOrWhiteSpace()) {
                    var path = Path.Combine(DataLocation.localApplicationData, $"{SolidModName}Config.xml");
                    configFilePath = path;
                    return path;
                }
                return configFilePath;
            }
        }

        private CultureInfo modCulture;
        public CultureInfo ModCulture {
            get => modCulture;
            set {
                modCulture = value;
                CommonLocale.Culture = value;
                SetModCulture(value);
            }
        }
        public virtual void SetModCulture(CultureInfo cultureInfo) { }
        public abstract string GetLocale(string text);

        public ModBase() {
            SingletonMod<Mod>.Instance = (Mod)this;
            ModLogger.GameLog($"Start initializing mod.");
            ModLogger.CreateDebugFile<Mod>();
            LoadConfig();
        }

        public void OnSettingsUI(UIHelperBase helper) {
            ModLogger.GameLog($"Setting UI.");
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
                    ModLogger.GameLog($"Change mod locale, use game language: {locale}.");
                } else {
                    locale = new CultureInfo(SingletonMod<Config>.Instance.ModLanguage);
                    ModLogger.GameLog($"Change mod locale, use custom language: {locale}.");
                }
                ModCulture = locale;
            }
            catch (Exception e) {
                ModLogger.GameLog($"Could't change mod locale", e);
            }
        }

        public void LoadConfig() => XMLUtils.LoadData<Config>(ConfigFilePath);
        public void SaveConfig() => XMLUtils.SaveData<Config>(ConfigFilePath);

        public abstract List<ModUpdateInfo> ModUpdateLogs { get; set; }
        public List<ModUpdateInfo> GetUpdateLogs() {
            if (ModUpdateLogs.Count == 0) return null;
            List<ModUpdateInfo> list = new();
            for (int i = 0; i < ModUpdateLogs.Count; i++) {
                ModUpdateInfo info = ModUpdateLogs[i];
                List<string> log = new();
                for (int j = 0; j < info.Log.Count; j++) {
                    log.Add(GetLocale(info.Log[j]));
                }
                list.Add(new ModUpdateInfo(info.ModVersion, info.Date, log));
            }
            return list;
        }

        public virtual void OnEnabled() {
            if (UIView.GetAView() is not null) {
                OptionPanelManager<Mod, OptionPanel>.OptionsEventHook();
            } else {
                LoadingManager.instance.m_introLoaded += OptionPanelManager<Mod, OptionPanel>.OptionsEventHook;
            }
            LoadingManager.instance.m_introLoaded += IntroActions;
        }

        public virtual void IntroActions() {
            GetDeserializationState();
        }

        public virtual void OnDisabled() { }
        public virtual void OnCreated(ILoading loading) { }
        public virtual void OnLevelLoaded(LoadMode mode) {
            ShowLogMessageBox();
            OptionPanelManager<Mod, OptionPanel>.OptionsEventHook();
        }
        public virtual void OnLevelUnloading() { }
        public virtual void OnReleased() { }

        private void ShowLogMessageBox() {
            if (!string.IsNullOrEmpty(SingletonMod<Config>.Instance.ModVersion)) {
                var lastVersion = new Version(SingletonMod<Config>.Instance.ModVersion);
                var nowVersion = ModVersion;
                if (lastVersion < nowVersion) {
                    SingletonMod<Config>.Instance.ModVersion = ModVersion.ToString();
                    SaveConfig();
                    var messageBox = MessageBox.Show<LogMessageBox>();
                    messageBox.Initialize<Mod>(true);
                }
            }
        }

        private void GetDeserializationState() {
            if (!XMLUtils.DeserializationState) {
                var messageBox = MessageBox.Show<XMLWariningMessageBox>();
                messageBox.Initialize<Mod>();
            }
        }

    }

    public struct ModUpdateInfo {
        public Version ModVersion;
        public string Date;
        public bool IsBeta;
        public List<string> Log;
        public ModUpdateInfo(Version version, string date, List<string> log) {
            ModVersion = version;
            Date = date;
            Log = log;
            IsBeta = false;
        }
        public ModUpdateInfo(Version version, string date, List<string> log, bool isBeta = true) {
            ModVersion = version;
            Date = date;
            Log = log;
            IsBeta = isBeta;
        }
    }

    public abstract class SingletonMod<Type> {
        public static Type Instance { get; set; }
    }

    public interface IMod : IUserMod, ILoadingExtension {
        string SolidModName { get; }
        string ModName { get; }
        Version ModVersion { get; }
        ulong ModID { get; }
        CultureInfo ModCulture { get; set; }
        void SaveConfig();
        void LoadConfig();
        List<ModUpdateInfo> GetUpdateLogs();
    }
}
