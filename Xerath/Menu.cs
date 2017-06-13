using System;
using HesaEngine.SDK;
using SharpDX.DirectInput;

namespace Xerath
{
    internal partial class MyScript
    {
        static void CreateMenu(out Menu q, out Menu w, out Menu e, out Menu r)
        {
            Action<Menu, string, string[], bool[], bool[], int> AddSpells = (menu, id, spell, enable, mana, mp) =>
            {
                for (int i = 0; i < spell.Length; ++i)
                {
                    menu.Boolean(id + spell[i], "Use " + spell[i], enable[i]);
                    if (mana[i]) menu.Slider(id + "MP" + spell[i], "Enable if %MP >= ", 1, 100, mp);
                }
            };

            myMenu = Menu.AddMenu("Xerath");

            var tmp = myMenu.AddSubMenu("Combo Mode");
            AddSpells(tmp, "cb", new string[] { "Q", "W", "E" }, new bool[] { true, true, true }, new bool[] { true, true, true }, 3);
            tmp.Boolean("cbWE", "Use Q if W or E is Ready");

            tmp = myMenu.AddSubMenu("Harass Mode");
            AddSpells(tmp, "hr", new string[] { "Q", "W", "E" }, new bool[] { true, true, false }, new bool[] { true, true, true }, 30);
            tmp.Boolean("hrWE", "Use Q if W or E is Ready");

            tmp = myMenu.AddSubMenu("LaneClear Mode");
            AddSpells(tmp, "lc", new string[] { "Q", "W" }, new bool[] { true, true }, new bool[] { true, true }, 35);
            tmp.Slider("lcHitQ", "Use Q if hit minions >=", 1, 10, 3);
            tmp.Slider("lcHitW", "Use W if hit minions >=", 1, 10, 3);

            AddSpells(myMenu.AddSubMenu("JungleClear Mode"), "jc", new string[] { "Q", "W", "E" }, new bool[] { true, true, false }, new bool[] { true, true, true }, 10);
            AddSpells(myMenu.AddSubMenu("KillSteal Mode"), "ks", new string[] { "Q", "W", "E", "Ignite" }, new bool[] { true, true, true, true }, new bool[] { true, true, true, false }, 20);

            var RMenu = myMenu.AddSubMenu("Ultimate Settings");
            RMenu.DropDown("RMode", "Choose Your Mode", new string[] { "Press Key", "Target In MouseRange", "Auto Cast" });
            RMenu.KeyBlind("RKey", "Select Key For PressKey Mode", Key.T, MenuKeybindType.Hold);
            RMenu.Slider("RMouse", "Set Range For MouseRange Mode", 200, 1500, 500, (x) => R2Range = x);
            RMenu.AddSubMenu("PressKey: Press a key anywhere for AutoCast");
            RMenu.AddSubMenu("MouseRange: AutoCast enemy in mouse range");
            RMenu.AddSubMenu("AutoCast: AutoCast enemy in R Range");
            RMenu.AddSubMenu("Recommned Mode: \"Press Key\"");
            RMenu.AddSeparator("You must active R -manually-");

            tmp = myMenu.AddSubMenu("Flee Mode");
            AddSpells(tmp, "flee", new string[] { "W", "E" }, new bool[] { true, true }, new bool[] { true, true }, 5);
            tmp.AddSeparator("Change key in Orbwalker menu");

            AntiGCMenu = myMenu.AddSubMenu("Anti-GapCloser");
            AntiGC.Add(AntiGCMenu, (x, y) =>
            {
                if (E.Ready && AntiGCMenu.Get<MenuCheckbox>(y.SData.Name.ToLower()).Checked && x.IsValidTarget(E.Data.Range)) E.Cast(x, false);
            });

            InterrupterMenu = myMenu.AddSubMenu("ChannelSpell Interrupter");
            AntiChannel.Add(InterrupterMenu, (x, y) =>
            {
                if (E.Ready && InterrupterMenu.Get<MenuCheckbox>(y.SData.Name).Checked && x.IsValidTarget(E.Data.Range)) E.Cast(x, false);
            });

            q = myMenu.AddSubMenu("Q Settings");
            w = myMenu.AddSubMenu("W Settings");
            e = myMenu.AddSubMenu("E Settings");
            r = myMenu.AddSubMenu("R Settings");
            r.Slider("R0", "R1 Cast Delays", 0, 1200, 230);
            r.Slider("R1", "R2 Cast Delays", 0, 1200, 250);
            r.Slider("R2", "R3 Cast Delays", 0, 1200, 270);
            r.Slider("R3", "R4 Cast Delays", 0, 1200, 290);
            r.Slider("R4", "R5 Cast Delays", 0, 1200, 310);

            var draw = myMenu.AddSubMenu("Drawings");
            draw.Boolean("drawQMin", "Draw Q Current Range", true);
            draw.Boolean("drawQMax", "Draw Q Max Range", true);
            draw.Boolean("drawWE", "Draw W, E Range", true);
            draw.Boolean("drawR", "Draw R Range", true);
            draw.Boolean("drawRText", "Draw Enemies R Killable", true);
            draw.Boolean("drawRMouse", "Draw Range of TargetInMouse", true);
        }
    }
}
