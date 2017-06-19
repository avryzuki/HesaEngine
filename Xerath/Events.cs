using System;
using HesaEngine.SDK;
using HesaEngine.SDK.Args;
using HesaEngine.SDK.GameObjects;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void CreateEvents() {
            Game.OnTick += OnTick;
            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnBuffGained += OnBuffGained;
            Obj_AI_Base.OnBuffLost += OnBuffLost;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            AntiGC.OnSpellActive += OnAntiGapcloserActive;
            AntiChannel.OnSpellActive += OnAntiChannelActive;
        }

        static void OnTick()
        {
            int TickCount = Game.GameTimeTickCount;
            if (TickCount - tick < 250) return;
            tick = TickCount;

            UpdateValues();

            if (CastCheck.ShouldCast)
            {
                switch (OW.ActiveMode)
                {
                    case Orbwalker.OrbwalkingMode.Combo:
                        DoCombo();
                        break;

                    case Orbwalker.OrbwalkingMode.Harass:
                        DoHarass();
                        break;

                    case Orbwalker.OrbwalkingMode.LaneClear:
                        Minions = MinionManager.GetMinions(Q.Data.ChargedMaxRange);
                        DoLaneClear();
                        break;

                    case Orbwalker.OrbwalkingMode.JungleClear:
                        DoJungleClear();
                        break;

                    case Orbwalker.OrbwalkingMode.Flee:
                        Flee();
                        break;

                    default:
                        break;
                }
            }

            DoKillSteal();
 
            if (R.Ready && RData.Active) UseUltimate();
        }

        static void OnDraw(EventArgs e)
        {
            Drawings();
        }

        static void OnBuffGained(Obj_AI_Base unit, Obj_AI_BaseBuffGainedEventArgs buff)
        {
            if (unit.IsMe)
            {
                switch (buff.Buff.DisplayName)
                {
                    case "XerathArcanopulseChargeUp":
                        QData.Active = true;
                        QData.LastCastTime = Game.Time;
                        break;

                    case "XerathLocusOfPower2":
                        RData.Active = true;
                        RData.Count = R.Data.Level + 2;
                        RData.Delay[0] = Game.GameTimeTickCount;
                        break;

                    default:
                        break;
                }
            }
        }

        static void OnBuffLost(Obj_AI_Base unit, Obj_AI_BaseBuffLostEventArgs buff)
        {
            if (unit.IsMe)
            {
                switch (buff.Buff.DisplayName)
                {
                    case "XerathArcanopulseChargeUp":
                        QData.Active = false;
                        Q.Data.Range = Q.Data.ChargedMinRange;
                        break;

                    case "XerathLocusOfPower2":
                        RData.Active = false;
                        RData.Count = R.Data.Level + 2;
                        Orbwalker.Move = true;
                        break;

                    default:
                        break;
                }
            }
        }

        static void OnProcessSpellCast(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if (unit.IsMe && spell.SData.Name == "XerathLocusPulse")
            {
                if (--RData.Count > 0)
                {
                    RData.Delay[2 + R.Data.Level - RData.Count] = Game.GameTimeTickCount + 600;
                }
            }
        }

        static void OnAntiGapcloserActive(AIHeroClient unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if (E.Ready && AntiGC.MenuCheck(AntiGCMenu, spell) && unit.IsValidTarget(E.Data.Range))
                E.Cast(unit, false);
        }

        static void OnAntiChannelActive(AIHeroClient unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if (E.Ready && AntiGC.MenuCheck(InterrupterMenu, spell)  && unit.IsValidTarget(E.Data.Range))
                E.Cast(unit, false);
        }
    }
}
