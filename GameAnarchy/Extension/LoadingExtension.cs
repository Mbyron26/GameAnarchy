namespace GameAnarchy;
using GameAnarchy.UI;
using ICities;

public class LoadingExtension : ModLoadingExtension<Mod> {
    public override void LevelLoaded(LoadMode mode) {
        SingletonManager<Manager>.Instance.SetStartMoney();
        SingletonManager<Manager>.Instance.InitAchievements(mode);
        if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame || mode == LoadMode.NewMap || mode == LoadMode.LoadMap || mode == LoadMode.NewAsset || mode == LoadMode.LoadAsset) {
            SingletonManager<ToolButtonManager>.Instance.Init();
            ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged += (_) => SingletonManager<ToolButtonManager>.Instance.UUIButtonIsPressed = _;
        }
    }

    public override void LevelUnloading() {
        SingletonManager<Manager>.Instance.DeInitAchievements();
        SingletonManager<ToolButtonManager>.Instance.DeInit();
        ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged -= (_) => SingletonManager<ToolButtonManager>.Instance.UUIButtonIsPressed = _;
    }

    public override void Released() => SingletonManager<Manager>.Instance.OutputFireSpreadCount();

}
