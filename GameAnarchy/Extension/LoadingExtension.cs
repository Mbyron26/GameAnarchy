using CSShared.Extension;
using CSShared.Manager;
using CSShared.UI.ControlPanel;
using GameAnarchy.Managers;
using GameAnarchy.UI;
using ICities;

namespace GameAnarchy;

public class LoadingExtension : ModLoadingExtension<Mod> {
    public override void LevelLoaded(LoadMode mode) {
        ManagerPool.GetOrCreateManager<Manager>().SetStartMoney();
        ManagerPool.GetOrCreateManager<Manager>().InitAchievements(mode);
        if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame || mode == LoadMode.NewMap || mode == LoadMode.LoadMap || mode == LoadMode.NewAsset || mode == LoadMode.LoadAsset) {
            ManagerPool.GetOrCreateManager<ToolButtonManager>().Enable();
            ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged += (_) => ManagerPool.GetOrCreateManager<ToolButtonManager>().UUIButtonIsPressed = _;
        }
    }

    public override void LevelUnloading() {
        ManagerPool.GetOrCreateManager<Manager>().DeInitAchievements();
        ManagerPool.GetOrCreateManager<ToolButtonManager>().Disable();
        ControlPanelManager<Mod, ControlPanel>.EventOnVisibleChanged -= (_) => ManagerPool.GetOrCreateManager<ToolButtonManager>().UUIButtonIsPressed = _;
    }

    public override void Released() => ManagerPool.GetOrCreateManager<Manager>().OutputFireSpreadCount();

}
