namespace MbyronModsCommon;

public class CommonLocalize {
    public static System.Globalization.CultureInfo Culture { get; set; }
    public static MbyronModsCommon.LocalizeManager LocaleManager { get; } = new MbyronModsCommon.LocalizeManager("CommonLocalize", typeof(CommonLocalize).Assembly);

    /// <summary>
    /// Cancel
    /// </summary>
    public static string Cancel => LocaleManager.GetString("Cancel", Culture);

    /// <summary>
    /// Change log
    /// </summary>
    public static string ChangeLog => LocaleManager.GetString("ChangeLog", Culture);

    /// <summary>
    /// Mod change log
    /// </summary>
    public static string ChangeLog_Major => LocaleManager.GetString("ChangeLog_Major", Culture);

    /// <summary>
    /// Check
    /// </summary>
    public static string Check => LocaleManager.GetString("Check", Culture);

    /// <summary>
    /// This mod is incompatible.
    /// </summary>
    public static string CompatibilityCheck_Incompatible => LocaleManager.GetString("CompatibilityCheck_Incompatible", Culture);

    /// <summary>
    /// Mod compatibility check
    /// </summary>
    public static string CompatibilityCheck_Major => LocaleManager.GetString("CompatibilityCheck_Major", Culture);

    /// <summary>
    /// This function only detects compatibility issues related to this mod. To check compatibility for all 
    /// </summary>
    public static string CompatibilityCheck_Minor => LocaleManager.GetString("CompatibilityCheck_Minor", Culture);

    /// <summary>
    /// already includes the same functionality.
    /// </summary>
    public static string CompatibilityCheck_SameFunctionality => LocaleManager.GetString("CompatibilityCheck_SameFunctionality", Culture);

    /// <summary>
    /// Use {0} instead.
    /// </summary>
    public static string CompatibilityCheck_UseInstead => LocaleManager.GetString("CompatibilityCheck_UseInstead", Culture);

    /// <summary>
    /// Unsubscribe
    /// </summary>
    public static string CompatibilityMessageBox_Unsubscribe => LocaleManager.GetString("CompatibilityMessageBox_Unsubscribe", Culture);

    /// <summary>
    /// Press any key
    /// </summary>
    public static string KeyBinding_PressAnyKey => LocaleManager.GetString("KeyBinding_PressAnyKey", Culture);

    /// <summary>
    /// Language
    /// </summary>
    public static string Language => LocaleManager.GetString("Language", Culture);

    /// <summary>
    /// Czech
    /// </summary>
    public static string Language_cs => LocaleManager.GetString("Language_cs", Culture);

    /// <summary>
    /// German
    /// </summary>
    public static string Language_de => LocaleManager.GetString("Language_de", Culture);

    /// <summary>
    /// English
    /// </summary>
    public static string Language_en => LocaleManager.GetString("Language_en", Culture);

    /// <summary>
    /// Spanish
    /// </summary>
    public static string Language_es_ES => LocaleManager.GetString("Language_es_ES", Culture);

    /// <summary>
    /// French
    /// </summary>
    public static string Language_fr => LocaleManager.GetString("Language_fr", Culture);

    /// <summary>
    /// Game Language
    /// </summary>
    public static string Language_GameLanguage => LocaleManager.GetString("Language_GameLanguage", Culture);

    /// <summary>
    /// Italian
    /// </summary>
    public static string Language_it => LocaleManager.GetString("Language_it", Culture);

    /// <summary>
    /// Japanese
    /// </summary>
    public static string Language_ja => LocaleManager.GetString("Language_ja", Culture);

    /// <summary>
    /// Korean
    /// </summary>
    public static string Language_ko => LocaleManager.GetString("Language_ko", Culture);

    /// <summary>
    /// Malay
    /// </summary>
    public static string Language_ms => LocaleManager.GetString("Language_ms", Culture);

    /// <summary>
    /// Dutch
    /// </summary>
    public static string Language_nl => LocaleManager.GetString("Language_nl", Culture);

    /// <summary>
    /// Polish
    /// </summary>
    public static string Language_pl => LocaleManager.GetString("Language_pl", Culture);

    /// <summary>
    /// Portuguese-Brazil
    /// </summary>
    public static string Language_pt_BR => LocaleManager.GetString("Language_pt_BR", Culture);

    /// <summary>
    /// Russian
    /// </summary>
    public static string Language_ru => LocaleManager.GetString("Language_ru", Culture);

    /// <summary>
    /// Slovak
    /// </summary>
    public static string Language_sk => LocaleManager.GetString("Language_sk", Culture);

    /// <summary>
    /// Thai
    /// </summary>
    public static string Language_th => LocaleManager.GetString("Language_th", Culture);

    /// <summary>
    /// Turkish
    /// </summary>
    public static string Language_tr => LocaleManager.GetString("Language_tr", Culture);

    /// <summary>
    /// Simplified Chinese
    /// </summary>
    public static string Language_zh_CN => LocaleManager.GetString("Language_zh_CN", Culture);

    /// <summary>
    /// Traditional Chinese
    /// </summary>
    public static string Language_zh_TW => LocaleManager.GetString("Language_zh_TW", Culture);

    /// <summary>
    /// Added
    /// </summary>
    public static string LogMessageBox_Added => LocaleManager.GetString("LogMessageBox_Added", Culture);

    /// <summary>
    /// Adjusted
    /// </summary>
    public static string LogMessageBox_Adjusted => LocaleManager.GetString("LogMessageBox_Adjusted", Culture);

    /// <summary>
    /// Attention
    /// </summary>
    public static string LogMessageBox_Attention => LocaleManager.GetString("LogMessageBox_Attention", Culture);

    /// <summary>
    /// Fixed
    /// </summary>
    public static string LogMessageBox_Fixed => LocaleManager.GetString("LogMessageBox_Fixed", Culture);

    /// <summary>
    /// Optimized
    /// </summary>
    public static string LogMessageBox_Optimized => LocaleManager.GetString("LogMessageBox_Optimized", Culture);

    /// <summary>
    /// Removed
    /// </summary>
    public static string LogMessageBox_Removed => LocaleManager.GetString("LogMessageBox_Removed", Culture);

    /// <summary>
    /// Translation
    /// </summary>
    public static string LogMessageBox_Translation => LocaleManager.GetString("LogMessageBox_Translation", Culture);

    /// <summary>
    /// Updated
    /// </summary>
    public static string LogMessageBox_Updated => LocaleManager.GetString("LogMessageBox_Updated", Culture);

    /// <summary>
    /// Did not detect any incompatible mods.
    /// </summary>
    public static string MessageBox_NormalPrompt => LocaleManager.GetString("MessageBox_NormalPrompt", Culture);

    /// <summary>
    /// OK
    /// </summary>
    public static string MessageBox_OK => LocaleManager.GetString("MessageBox_OK", Culture);

    /// <summary>
    /// Incompatible mods have been detected, in order to ensure this mod works properly, you need to unsubs
    /// </summary>
    public static string MessageBox_WarningPrompt => LocaleManager.GetString("MessageBox_WarningPrompt", Culture);

    /// <summary>
    /// No
    /// </summary>
    public static string No => LocaleManager.GetString("No", Culture);

    /// <summary>
    /// Advanced
    /// </summary>
    public static string OptionPanel_Advanced => LocaleManager.GetString("OptionPanel_Advanced", Culture);

    /// <summary>
    /// Builtin Function
    /// </summary>
    public static string OptionPanel_BuiltinFunction => LocaleManager.GetString("OptionPanel_BuiltinFunction", Culture);

    /// <summary>
    /// Compatibility Check
    /// </summary>
    public static string OptionPanel_CompatibilityCheck => LocaleManager.GetString("OptionPanel_CompatibilityCheck", Culture);

    /// <summary>
    /// Advanced functions. It is only used for debugging output. If something goes wrong, it is recommended
    /// </summary>
    public static string OptionPanel_DebugMinor => LocaleManager.GetString("OptionPanel_DebugMinor", Culture);

    /// <summary>
    /// Debug mode
    /// </summary>
    public static string OptionPanel_DebugMode => LocaleManager.GetString("OptionPanel_DebugMode", Culture);

    /// <summary>
    /// General
    /// </summary>
    public static string OptionPanel_General => LocaleManager.GetString("OptionPanel_General", Culture);

    /// <summary>
    /// Hotkeys
    /// </summary>
    public static string OptionPanel_Hotkeys => LocaleManager.GetString("OptionPanel_Hotkeys", Culture);

    /// <summary>
    /// Is Disabled
    /// </summary>
    public static string OptionPanel_IsDisabled => LocaleManager.GetString("OptionPanel_IsDisabled", Culture);

    /// <summary>
    /// Is Enabled
    /// </summary>
    public static string OptionPanel_IsEnabled => LocaleManager.GetString("OptionPanel_IsEnabled", Culture);

    /// <summary>
    /// Mod Information
    /// </summary>
    public static string OptionPanel_ModInfo => LocaleManager.GetString("OptionPanel_ModInfo", Culture);

    /// <summary>
    /// Version
    /// </summary>
    public static string OptionPanel_Version => LocaleManager.GetString("OptionPanel_Version", Culture);

    /// <summary>
    /// Reset
    /// </summary>
    public static string Reset => LocaleManager.GetString("Reset", Culture);

    /// <summary>
    /// Reset mod failed. It is recommended to send the log file to the developer.
    /// </summary>
    public static string ResetModFailed => LocaleManager.GetString("ResetModFailed", Culture);

    /// <summary>
    /// Reset mod config
    /// </summary>
    public static string ResetModMajor => LocaleManager.GetString("ResetModMajor", Culture);

    /// <summary>
    /// Attention! This option will reset all settings of this mod, and this operation is irreversible!
    /// </summary>
    public static string ResetModMinor => LocaleManager.GetString("ResetModMinor", Culture);

    /// <summary>
    /// Reset mod succeeded.
    /// </summary>
    public static string ResetModSucceeded => LocaleManager.GetString("ResetModSucceeded", Culture);

    /// <summary>
    /// Are you sure you want to reset the configuration of the mod?
    /// </summary>
    public static string ResetModWarning => LocaleManager.GetString("ResetModWarning", Culture);

    /// <summary>
    /// Scroll the wheel to change, Shift X10, Ctrl X0.1
    /// </summary>
    public static string ScrollWheel => LocaleManager.GetString("ScrollWheel", Culture);

    /// <summary>
    /// Show control panel
    /// </summary>
    public static string ShowControlPanel => LocaleManager.GetString("ShowControlPanel", Culture);

    /// <summary>
    /// Warning! Mod cannot normally read the old configuration data. Now default configuration has been loa
    /// </summary>
    public static string XMLWariningMessageBox_Warning => LocaleManager.GetString("XMLWariningMessageBox_Warning", Culture);

    /// <summary>
    /// Yes
    /// </summary>
    public static string Yes => LocaleManager.GetString("Yes", Culture);
}