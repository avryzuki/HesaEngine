using System;
using SharpDX;
using HesaEngine.SDK;
using SharpDX.DirectInput;
using HesaEngine.SDK.GameObjects;

namespace DarkXerath
{
    internal static class _Extensions
    {
        public static float ADHealth(this Obj_AI_Base unit) { return unit.Health + unit.ADShield; }

        public static float APHealth(this Obj_AI_Base unit) { return unit.Health + unit.ADShield + unit.MagicShield; }

        public static void Boolean(this Menu menu, string key, string text, bool boolean = false, Action<bool> cb = null)
        {
            var tmp = menu.Add(new MenuCheckbox(key, text, boolean));
            if (cb != null) tmp.OnValueChanged = (MenuCheckbox x, bool y) => cb(y);
        }

        public static void DropDown(this Menu menu, string key, string text, string[] list, int select = 0, Action<string[], int> cb = null)
        {
            var tmp = menu.Add(new MenuCombo(key, text, list, select));
            if (cb != null) tmp.OnValueChanged = (MenuCombo x, int y) => cb(x.Values, y);
        }

        public static void Slider(this Menu menu, string key, string text, int minValue, int maxValue, int currentValue, Action<int> cb = null)
        {
            var tmp = menu.Add(new MenuSlider(key, text, minValue, maxValue, currentValue));
            if (cb != null) tmp.OnValueChanged = (MenuSlider x, int y) => cb(y);
        }

        public static void KeyBlind(this Menu menu, string key, string text, Key k, MenuKeybindType state = MenuKeybindType.Hold, Action<bool> cb = null)
        {
            var tmp = menu.Add(new MenuKeybind(key, text, k, state));
            if (cb != null) tmp.OnValueChanged = (MenuKeybind x, bool y) => cb(y);
        }
    }
}
