using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using System.Collections.Generic;

namespace Xerath
{
    internal class SpellManager
    {
        public SpellManager(Menu menu, SpellSlot spellSlot, TargetSelector.DamageType damageType, CollisionableObjects[] col = null) {
            Data = new Spell(spellSlot);
            Data.DamageType = damageType;
            colTable = col;

            this.menu = menu;
            //menuID = spellSlot.ToString() + "hc";
            //menu.DropDown(menuID, "HitChance Option", new string[] { "Low", "Medium", "High", "Very High" }, 1);
        }

        public void SetValues(float delay, float speed, float width, int minRange, int maxRange, string buffName, SkillshotType type)
        {
            Data.SetSkillshot(delay, width, speed, (colTable != null), type);
            Data.ChargedBuffName = buffName;
            Data.ChargedMinRange = minRange;
            Data.ChargedMaxRange = maxRange;
            Data.Range = (float)maxRange;
        }

        public void SetValues(float delay, float speed, float width, float range, SkillshotType type)
        {
            Data.SetSkillshot(delay, width, speed, (colTable != null), type);
            Data.Range = range;
        }

        public void SetValues(float delay, float speed, float range)
        {
            Data.SetTargetted(delay, speed);
            Data.Range = range;
        }

        public void SetValues(float range)
        {
            Data.Range = range;
        }

        public PredictionOutput GetPrediction(Obj_AI_Base unit)
        {
            return Core.Prediction.GetPrediction(unit, Data.Delay + Data.Speed / unit.Distance3D(ObjectManager.Player));
        }

        public PredictionOutput GetPrediction(Obj_AI_Base unit, bool aoe)
        {
            return Data.GetPrediction(unit, aoe, Data.ChargedMaxRange > 0 ? Data.ChargedMaxRange : Data.Range, colTable);
        }

        public void Cast(Obj_AI_Base unit)
        {
            Data.Cast(unit);
        }

        public void Cast(Obj_AI_Base unit, bool aoe)
        {
            var pred = GetPrediction(unit, aoe);
            if ((int)pred.Hitchance >= menu.Get<MenuCombo>(menuID).CurrentValue + 3)
            {
                Data.Cast(pred.CastPosition);
            }
        }

        public void Cast(Obj_AI_Base unit, float currentRange, bool aoe)
        {
            var pred = GetPrediction(unit, aoe);
            if ((int)pred.Hitchance >= menu.Get<MenuCombo>(menuID).CurrentValue + 3 && ObjectManager.Player.Position.Distance(pred.CastPosition) <= currentRange)
            {
                ObjectManager.Player.Spellbook.UpdateChargedSpell(Data.Slot, pred.CastPosition, true);
            }
        }

        public static class CollisionTable
        {
            public static CollisionableObjects[] GetTable(params string[] table)
            {
                List<CollisionableObjects> result = new List<CollisionableObjects>();
                for (int i = 0; i < table.Length; ++i)
                {
                    switch (table[i])
                    {
                        case "Hero":
                            result.Add(CollisionableObjects.Heroes);
                            break;
                        case "Braum":
                            result.Add(CollisionableObjects.BraumShield);
                            break;
                        case "Minion":
                            result.Add(CollisionableObjects.Minions);
                            break;
                        case "Yasuo":
                            result.Add(CollisionableObjects.YasuoWall);
                            break;
                        case "Wall":
                            result.Add(CollisionableObjects.Walls);
                            break;
                        case "Ally":
                            result.Add(CollisionableObjects.Allies);
                            break;
                        default:
                            break;
                    }
                }
                return result.ToArray();
            }

            public static CollisionableObjects[] Ult
            {
                get { return GetTable("Braum", "Yasuo"); }
            }

            public static CollisionableObjects[] Normal
            {
                get { return GetTable("Braum", "Hero", "Yasuo", "Minion"); }
            }

            public static CollisionableObjects[] Wall
            {
                get { return GetTable("Braum", "Hero", "Yasuo", "Minion", "Wall"); }
            }
        }

        public Spell Data;

        public bool Ready
        {
            get { return Data.IsReady(); }
        }

        private CollisionableObjects[] colTable { get; set; }

        private readonly Menu menu;

        private string menuID { get; set; }
    }
}
