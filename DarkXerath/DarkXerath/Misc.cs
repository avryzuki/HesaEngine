using System;
using HesaEngine.SDK;
using HesaEngine.SDK.Args;
using HesaEngine.SDK.GameObjects;
using System.Collections.Generic;

namespace DarkXerath
{
    internal static class CastCheck
    {
        public static void Initialize()
        {
            Orbwalker.AfterAttack += OnAfterAttack;
            Obj_AI_Base.OnPlayAnimation += OnAnimation;
        }

        private static void OnAfterAttack(AttackableUnit sender, AttackableUnit target)
        {
            if (sender.IsMe) ShouldCast = true;
        }

        private static void OnAnimation(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs name)
        {
            if (sender.IsMe)
            {
                string animation = name.Animation;
                switch (animation)
                {
                    case "Run":
                        {
                            ShouldCast = true;
                            break;
                        }

                    case "Idle1":
                        {
                            ShouldCast = true;
                            break;
                        }

                    default:
                        {
                            if (animation.StartsWith("Attack")) ShouldCast = false;
                            else if (animation.StartsWith("Spell")) ShouldCast = true;
                            break;
                        }
                }
            }
        }

        public static bool ShouldCast = true;

        private static AIHeroClient myHero = ObjectManager.Player;
    }

    internal static class AntiGC
    {
        public static void AddMenu(Menu menu)
        {
            Core.DelayAction(() =>
            {
                bool exist = false;
                foreach (var spell in Database.GapCloserSpells)
                {
                    foreach (var enemy in ObjectManager.Heroes.Enemies)
                    {
                        if (spell.ChampionName == enemy.ChampionName)
                        {
                            if (!SpellData.ContainsKey(spell.SpellName))
                            {
                                SpellData[spell.SpellName] = true;
                                menu.Boolean(spell.SpellName, spell.ChampionName + " " + spell.Slot.ToString(), true);
                                exist = true;
                                break;
                            }
                        }
                    }
                }
                if (!exist) menu.AddSeparator("- No Spell Avaliable -");
            }, 10);

            if (!load)
            {
                load = true;

                Obj_AI_Base.OnProcessSpellCast += (unit, spell) =>
                {
                    if (unit.IsEnemy && SpellData.ContainsKey(spell.SData.Name.ToLower()))
                    {
                        AntiSpellObject.GapcloseSpells.AddLast(new AntiSpellObject((AIHeroClient)unit, spell));
                    }
                };

                Game.OnTick += () =>
                {
                    if (AntiSpellObject.GapcloseSpells.Count == 0) return;
                    var node = AntiSpellObject.GapcloseSpells.First;
                    for (int i = 1; i <= AntiSpellObject.GapcloseSpells.Count; ++i)
                    {
                        if (Game.Time > node.Value.end)
                        {
                            AntiSpellObject.GapcloseSpells.Remove(node);
                            break;
                        }
                        OnSpellActive(node.Value.unit, node.Value.spell);
                        if (i < AntiSpellObject.GapcloseSpells.Count) node = node.Next;
                    }
                };
            }
        }

        public static bool MenuCheck(Menu menu, GameObjectProcessSpellCastEventArgs spell)
        {
            return menu.Get<MenuCheckbox>(spell.SData.Name.ToLower()).Checked;
        }

        private static bool load = false;

        private static Dictionary<string, bool> SpellData = new Dictionary<string, bool>();

        public static event Action<AIHeroClient, GameObjectProcessSpellCastEventArgs> OnSpellActive;
    }
    
    internal static class AntiChannel
    {
        public static void AddMenu(Menu menu)
        {
            Core.DelayAction(() =>
            {
                bool exist = false;
                foreach (var spell in Database.InterruptableSpells)
                {
                    foreach (var enemy in ObjectManager.Heroes.Enemies)
                    {
                        if (spell.ChampionName == enemy.ChampionName)
                        {
                            if (!SpellData.ContainsKey(spell.SpellName))
                            {
                                SpellData[spell.SpellName] = true;
                                menu.Boolean(spell.SpellName, spell.ChampionName + " " + spell.Slot.ToString(), true);
                                exist = true;
                                break;
                            }
                        }
                    }
                }
                if (!exist) menu.AddSeparator("- No Spell Avaliable -");
            }, 10);

            if (!load)
            {
                load = true;

                Obj_AI_Base.OnProcessSpellCast += (unit, spell) =>
                {
                    if (unit.IsEnemy && SpellData.ContainsKey(spell.SData.Name))
                    {
                        AntiSpellObject.ChannelSpells.AddLast(new AntiSpellObject((AIHeroClient)unit, spell));
                    }
                };

                Game.OnTick += () =>
                {
                    if (AntiSpellObject.ChannelSpells.Count == 0) return;
                    var node = AntiSpellObject.ChannelSpells.First;
                    for (int i = 1; i <= AntiSpellObject.ChannelSpells.Count; ++i)
                    {
                        if (Game.Time > node.Value.end)
                        {
                            AntiSpellObject.ChannelSpells.Remove(node);
                            break;
                        }
                        OnSpellActive(node.Value.unit, node.Value.spell);
                        if (i < AntiSpellObject.ChannelSpells.Count) node = node.Next;
                    }
                };
            }
        }

        public static bool MenuCheck(Menu menu, GameObjectProcessSpellCastEventArgs spell)
        {
            return menu.Get<MenuCheckbox>(spell.SData.Name).Checked;
        }

        private static bool load = false;

        private static Dictionary<string, bool> SpellData = new Dictionary<string,bool>();

        public static event Action<AIHeroClient, GameObjectProcessSpellCastEventArgs> OnSpellActive;
    }

    internal class AntiSpellObject
    {
        public static LinkedList<AntiSpellObject> ChannelSpells = new LinkedList<AntiSpellObject>();
        public static LinkedList<AntiSpellObject> GapcloseSpells = new LinkedList<AntiSpellObject>();

        public AntiSpellObject(AIHeroClient unit, GameObjectProcessSpellCastEventArgs spell)
        {
            this.unit = unit;
            this.spell = spell;
            end = spell.TimeCast + 1.25f;
        }

        public float end;
        public AIHeroClient unit;
        public GameObjectProcessSpellCastEventArgs spell;
    }
}
