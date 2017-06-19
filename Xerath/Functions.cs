using System;
using SharpDX;
using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;
using System.Collections.Generic;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void UpdateValues()
        {
            if (QData.Active)
            {
                Q.Data.Range = Math.Min(Q.Data.ChargedMaxRange, 700f + 500f * (Game.Time - QData.LastCastTime));
            }

            if (R.Ready)
            {
                R.Data.Range = 2000 + 1200 * R.Data.Level;
            }
        }

        static AIHeroClient GetRTarget(Vector3 Position, float range)
        {
            AIHeroClient RTarget = null;
            float ratio = 0;
            foreach (var enemy in Enemies)
            {
                if (enemy.IsValidTarget(R.Data.Range) && enemy.Distance(Position) <= range)
                {
                    if (RTarget == null || (_TargetSelector.GetRealPriority(_TargetSelector.GetPriority(enemy)) * R.Data.GetDamage(enemy) / enemy.APHealth() > ratio))
                    {
                        RTarget = enemy;
                        ratio = _TargetSelector.GetRealPriority(_TargetSelector.GetPriority(enemy)) * R.Data.GetDamage(enemy) / enemy.APHealth();
                    }
                }
            }
            return RTarget;
        }

        static void CastR(AIHeroClient unit)
        {
            int index = 2 + R.Data.Level - RData.Count;
            if (Game.GameTimeTickCount - RData.Delay[index] > myMenu.Get<MenuSlider>("R" + index.ToString()).CurrentValue && unit != null)
            {
                var predPos = DarkPrediction.CirclerPrediction(R.Data, unit);
                if (predPos != DarkPrediction.empt && predPos.Distance(myHero) <= R.Data.Range)
                {
                    R.Data.Cast(predPos);
                }
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
                    var predPos = DarkPrediction.CirclerPrediction(Q.Data, (AIHeroClient)unit);
                    if (predPos != DarkPrediction.empt && myHero.Distance(predPos) <= Q.Data.Range)
                    {
                        myHero.Spellbook.UpdateChargedSpell(Q.Data.Slot, predPos, true);
                    }
                }
            }
        }

        static void CastW(Obj_AI_Base unit)
        {
            if (unit != null)
            {
                var predPos = DarkPrediction.CirclerPrediction(W.Data, (AIHeroClient)unit);
                if (predPos != DarkPrediction.empt && predPos.Distance(myHero) <= W.Data.Range)
                {
                    W.Data.Cast(predPos);
                }
            }
        }

        static void CastE(Obj_AI_Base unit)
        {
            if (unit != null)
            {
                var predPos = DarkPrediction.LinearPrediction(myHero.Position, E.Data, (AIHeroClient)unit);
                if (predPos != DarkPrediction.empt && !DarkPrediction.CollisionChecker(predPos, myHero.Position, E.Data) && predPos.Distance(myHero) <= E.Data.Range)
                {
                    E.Data.Cast(predPos);
                }
            }
        }

        static MinionFarm GetLineFarmPosition(Vector3 Pos, List<Obj_AI_Minion> Minions, float halfWidth)
        {
            Vector3 Position = new Vector3();
            int hits = 0;
            foreach (var enemy in Minions)
            {
                LineSegment line = new LineSegment(GetVec2(Pos), GetVec2(enemy.Position));
                int tmp = 0;
                foreach (var enemy2 in Minions)
                {
                    if (line.IsOnLine(GetVec2(enemy2.Position), halfWidth, enemy2.BoundingRadius)) ++tmp;
                }
                if (tmp > hits)
                {
                    hits = tmp;
                    Position = enemy.Position;
                }
            }
            return new MinionFarm(Position, hits);
        }

        static MinionFarm GetFarmPosition(Vector3 Pos, List<Obj_AI_Minion> Minions, float halfWidth)
        {
            Vector3 Position = new Vector3();
            int hits = 0;
            foreach (var enemy in Minions)
            {
                int tmp = 0;
                foreach (var enemy2 in Minions)
                {
                    if ((enemy2.Position - enemy.Position).Length() < halfWidth) ++tmp;
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
