using System;
using SharpDX;
using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;
using System.Collections.Generic;

namespace Xerath
{
    internal partial class MyScript
    {
        static void UpdateValues()
        {
            if (QData.Active)
            {
                Q.Data.Range = Q.Data.ChargedMinRange + 500f * Math.Min(1.5f, (Game.Time - QData.LastCastTime));
            }

            if (R.Ready)
            {
                R.Data.Range = 2000 + 1200 * R.Data.Level;
            }
        }

        static AIHeroClient GetRTarget(Vector3 Position, float range)
        {
            AIHeroClient RTarget = null;
            foreach (var enemy in Enemies)
            {
                if (enemy.IsValidTarget(R.Data.Range) && enemy.Distance(Position) <= range)
                {
                    if (RTarget == null || (_TargetSelector.GetRealPriority(_TargetSelector.GetPriority(enemy)) * R.Data.GetDamage(enemy) / enemy.APHealth() > _TargetSelector.GetRealPriority(_TargetSelector.GetPriority(RTarget)) * R.Data.GetDamage(RTarget) / RTarget.APHealth()))
                        RTarget = enemy;
                }
            }
            return RTarget;
        }

        static void CastR(AIHeroClient unit)
        {
            int index = 2 + R.Data.Level - RData.Count;
            if (Game.GameTimeTickCount - RData.Delay[index] > myMenu.Get<MenuSlider>("R" + index.ToString()).CurrentValue)
            {
                R.Cast(unit, true);
            }
        }

        static void CastQ(Obj_AI_Base unit)
        {
            if (unit != null)
            {
                if (!QData.Active)
                {
                    Q.Data.Cast(Game.CursorPosition);
                }
                else
                {
                    Q.Cast(unit, Q.Data.Range, true);
                }
            }
        }

        static void CastW(Obj_AI_Base unit)
        {
            if (unit != null)
            {
                W.Cast(unit, true);
            }
        }

        static void CastE(Obj_AI_Base unit)
        {
            if (unit != null)
            {
                E.Cast(unit, false);
            }
        }

        static MinionFarm GetLineFarmPosition(Vector3 Pos, List<Obj_AI_Minion> Minions, float width)
        {
            Vector3 Position = new Vector3();
            int hits = 0;
            foreach (var enemy in Minions)
            {
                LineSegment line = new LineSegment(GetVec2(Pos), GetVec2(enemy.Position));
                int tmp = 0;
                foreach (var enemy2 in Minions)
                {
                    if (line.IsOnLine(GetVec2(enemy2.Position), width, enemy2.BoundingRadius)) ++tmp;
                }
                if (tmp > hits)
                {
                    hits = tmp;
                    Position = enemy.Position;
                }
            }
            return new MinionFarm(Position, hits);
        }

        static MinionFarm GetFarmPosition(Vector3 Pos, List<Obj_AI_Minion> Minions, float radius)
        {
            Vector3 Position = new Vector3();
            int hits = 0;
            foreach (var enemy in Minions)
            {
                int tmp = 0;
                foreach (var enemy2 in Minions)
                {
                    if ((enemy2.Position - enemy.Position).Length() < radius) ++tmp;
                }
                if (tmp > hits)
                {
                    hits = tmp;
                    Position = enemy.Position;
                }
            }
            return new MinionFarm(Position, hits);
        }

        static Vector2 GetVec2(Vector3 vec)
        {
            return new Vector2(vec.X, vec.Z);
        }
    }
}
