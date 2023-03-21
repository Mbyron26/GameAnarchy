
namespace MbyronModsCommon {
    public class ModConfigBase<Config> : SingletonMod<Config>, IModConfig where Config : ModConfigBase<Config> {
        public string ModVersion { get; set; } = "0.0";
        public string ModLanguage { get; set; } = "GameLanguage";
        public bool DebugMode { get; set; } = false;
    }

    public interface IModConfig {
        string ModVersion { get; set; }
        string ModLanguage { get; set; }
    }
}
