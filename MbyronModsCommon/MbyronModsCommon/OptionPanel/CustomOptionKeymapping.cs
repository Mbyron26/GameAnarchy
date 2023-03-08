using ColossalFramework.UI;

namespace MbyronModsCommon {
    public class CustomKeymapping {
        public static OptionKeymapping AddKeymapping(UIComponent parent, string text, KeyBinding binding, string tooltip = null) {
            var keymapping = parent.gameObject.AddComponent<OptionKeymapping>();
            keymapping.LabelText = text;
            keymapping.Binding = binding;
            if (tooltip is not null) {
                keymapping.Tooltip = tooltip;
            }
            return keymapping;
        }
    }
}
