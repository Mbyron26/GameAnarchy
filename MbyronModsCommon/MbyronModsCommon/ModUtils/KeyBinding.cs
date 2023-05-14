namespace MbyronModsCommon;
using ColossalFramework;
using System.Xml.Serialization;
using UnityEngine;

public class KeyBinding {

    [XmlAttribute("Key")]
    public KeyCode Key { get; private set; }
    [XmlAttribute("Control")]
    public bool Control { get; private set; }
    [XmlAttribute("Shift")]
    public bool Shift { get; private set; }
    [XmlAttribute("Alt")]
    public bool Alt { get; private set; }

    public KeyBinding() { }
    public KeyBinding(KeyCode keyCode, bool control, bool shift, bool alt) {
        Key = keyCode;
        Control = control;
        Shift = shift;
        Alt = alt;
    }

    public InputKey Encode() => SavedInputKey.Encode(Key, Control, Shift, Alt);
    public void SetKey(InputKey inputKey) {
        Key = (KeyCode)(inputKey & 0xFFFFFFF);
        Control = (inputKey & 0x40000000) != 0;
        Shift = (inputKey & 0x20000000) != 0;
        Alt = (inputKey & 0x10000000) != 0;
    }
    public bool IsPressed() {
        if (!Input.GetKey(Key)) {
            return false;
        }
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) != Control) {
            return false;
        }
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) != Shift) {
            return false;
        }
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.AltGr)) != Alt) {
            return false;
        }
        return true;
    }
}

public class KeyHelper {
    public static bool IsControlDown() => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    public static bool IsShiftDown() => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    public static bool IsAltDown() => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
}
