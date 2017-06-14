using System;
using SharpDX;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using System.Collections.Generic;

namespace Xerath
{
    internal partial class MyScript
    {
        static Menu myMenu, AntiGCMenu, InterrupterMenu;

        static int tick = Game.GameTimeTickCount;
        static int R2Range { get; set; }

        static AIHeroClient myHero = ObjectManager.Player;

        static List<AIHeroClient> Enemies = ObjectManager.Heroes.Enemies;
        static List<Obj_AI_Minion> Minions { get; set; }

        static Orbwalker.OrbwalkerInstance OW = Core.Orbwalker;

        static SpellManager Q, W, E, R;
        static Spell Ignite = new Spell(myHero.GetSpellSlotFromName(SummonerSpells.Ignite), 600f);

        static _TargetSelector TSQ, TSW, TSE;

        internal static void LoadData()
        {
            Menu q, w, e, r;
            CreateMenu(out q, out w, out e, out r);

            Q = new SpellManager(q, SpellSlot.Q, TargetSelector.DamageType.Magical);
            Q.SetValues(0.7f, float.MaxValue, 90f, 750, 1500, "XerathArcanopulseChargeUp", SkillshotType.SkillshotLine);

            W = new SpellManager(w, SpellSlot.W, TargetSelector.DamageType.Magical);
            W.SetValues(0.8f, float.MaxValue, 250f, 1100f, SkillshotType.SkillshotCircle);
            
            E = new SpellManager(e, SpellSlot.E, TargetSelector.DamageType.Magical, SpellManager.CollisionTable.Normal);
            E.SetValues(0.3f, 1400f, 70f, 1050f, SkillshotType.SkillshotLine);

            R = new SpellManager(r, SpellSlot.R, TargetSelector.DamageType.Magical);
            R.SetValues(0.7f, float.MaxValue, 200f, 0f, SkillshotType.SkillshotCircle);

            TSQ = new _TargetSelector(q, TargetSelector.DamageType.Magical);
            TSW = new _TargetSelector(w, TargetSelector.DamageType.Magical);
            TSE = new _TargetSelector(e, TargetSelector.DamageType.Magical, _TargetSelector.Mode.Cloest);

            RData.Count = Math.Max(3, R.Data.Level + 2);

            R2Range = myMenu.Get<MenuSlider>("RMouse").CurrentValue;

            if (Ignite.Slot == SpellSlot.Unknown) Ignite = null;

            if (myHero.HasBuff("XerathArcanopulseChargeUp"))
            {
                QData.Active = true;
                QData.LastCastTime = myHero.GetBuff("XerathArcanopulseChargeUp").StartTime;
            }

            if (myHero.HasBuff("XerathLocusOfPower2"))
            {
                RData.Active = true;
                RData.Delay[0] = Game.GameTimeTickCount;
            }

            CastCheck.Initialize();

            CreateEvents();
        }

        internal static class QData
        {
            public static bool Active { get; set; }
            public static float LastCastTime { get; set; }
        }

        internal static class RData
        {
            public static bool Active { get; set; }
            public static int Count { get; set; }
            public static int[] Delay = { 0, 0, 0, 0, 0 };
        }

        internal class MinionFarm
        {
            public MinionFarm(Vector3 pos, int hit)
            {
                Position = pos;
                Hits = hit;
            }

            public readonly Vector3 Position;
            public readonly int Hits;
        }
    }
}
