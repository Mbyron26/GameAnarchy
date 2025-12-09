using CSLModsCommon.UI.Containers;
using GameAnarchy.Managers;

namespace GameAnarchy.UI;

public partial class OptionsPanel {
    protected override void FillDebugPage(ScrollContainer page) {
        base.FillDebugPage(page);
        var group = AddSection(page);

        group.AddButton("ControlPanel", null, "Open", null, 30, _ => _domain.GetOrCreateManager<ControlPanelManager>().TogglePanel());
    }
}