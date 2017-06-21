using System;
using SharpDX;
using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;
using System.Collections.Generic;

namespace DarkXerath
{
    internal class _TargetSelector
    {
        public _TargetSelector(Menu subMenu, TargetSelector.DamageType damageType, Mode mode, bool includeShields = true)
        {
            menu = subMenu.AddSubMenu("Target Selector");
            menu.DropDown("TSMode", "Targetting Mode", new string[] {"Auto Priority", "Lowest HP", "LessCast", "Most AD", "Most AP", "Closest", "Closest Mouse", "LessCast"}, (int) mode);
            menu.Boolean("selected", "Priority Selected Target");
            if (Enemies.Count == 0) menu.AddSeparator("- No Enemy Avaliable -");
            else
            {
                menu.AddSeparator("Settings Enemies's Priority");
                foreach (var enemy in Enemies)
                {
                    menu.Slider(enemy.ChampionName, enemy.ChampionName, 1, 5, GetPriority(enemy));
                }
            }

            if (damageType == TargetSelector.DamageType.Magical)
            {
                IsValid = (unit) =>
                {
                    foreach (var buff in unit.Buffs)
                    {
                        if (buff.IsActive)
                        {
                            if (!ShouldAttack(unit, buff.DisplayName)) return false;
                            switch (buff.DisplayName)
                            {
                                case "SivirShield":
                                    {
                                        return false;
                                    }
                                case "ShroudofDarkness":
                                    {
                                        return false;
                                    }
                                default:
                                    break;
                            }
                        }
                    }
                    return true;
                };
            }
            else
            {
                IsValid = (unit) =>
                {
                    foreach (var buff in unit.Buffs)
                    {
                        if (buff.IsActive && !ShouldAttack(unit, buff.DisplayName)) return false;
                    }
                    return true;
                };
            }
        }

        public AIHeroClient GetTarget(Vector3 Position, float Range, Func<AIHeroClient, float> damage = null)
        {
            var list = Enemies.FindAll((x) => x.IsValidTarget(Range) && IsValid(x));
            AIHeroClient result = null;
            if (menu.Get<MenuCheckbox>("selected").Checked)
            {
                var ts = GetSelectedTarget(Position, Range);
                if (ts != null) return ts;
            }

            switch (menu.Get<MenuCombo>("TSMode").CurrentValue)
            {
                case 0:
                    {
                        foreach (var enemy in list)
                        {
                            if (result == null || (GetRealPriority(menu.Get<MenuSlider>(enemy.ChampionName).CurrentValue) * damage(enemy) / enemy.ADHealth() > GetRealPriority(menu.Get<MenuSlider>(result.ChampionName).CurrentValue) * damage(result) / result.ADHealth()))
                                result = enemy;
                        }
                        break;
                    }
                case 1:
                    {
                        foreach (var enemy in list)
                        {
                            if (result == null || (enemy.ADHealth() < result.ADHealth()))
                                result = enemy;
                        }
                        break;
                    }
                case 2:
                    {
                        foreach (var enemy in list)
                        {
                            if (result == null || (enemy.APHealth() - damage(enemy) < result.APHealth() - damage(result)))
                                result = enemy;
                        }
                        break;
                    }
                case 3:
                    {
                        foreach (var enemy in list)
                        {
                            if (result == null || (enemy.TotalAttackDamage > result.TotalAttackDamage))
                                result = enemy;
                        }
                        break;
                    }
                case 4:
                    {
                        foreach (var enemy in list)
                        {
                            if (result == null || (enemy.BonusAbilityPower > result.BonusAbilityPower))
                                result = enemy;
                        }
                        break;
                    }
                case 5:
                    {
                        foreach (var enemy in list)
                        {
                            if (result == null || (enemy.Distance(Position) < result.Distance(Position)))
                                result = enemy;
                        }
                        break;
                    }
                case 6:
                    {
                        foreach (var enemy in list)
                        {
                            if (result == null || (enemy.Distance(Game.CursorPosition) < result.Distance(Game.CursorPosition)))
                                result = enemy;
                        }
                        break;
                    }
                default:
                    break;
            }

            return result;
        }

        public static AIHeroClient GetSelectedTarget(Vector3 Position, float Range)
        {
            var target = TargetSelector.GetSelectedTarget();
            if (target != null && !target.IsDead && target.Distance(Position) <= Range) return target;
            return null;
        }

        public static int GetPriority(AIHeroClient unit)
        {
            if (priority.ContainsKey(unit.ChampionName)) return priority[unit.ChampionName];
            return 0;
        }

        public static float GetRealPriority(int p)
        {
            switch (p)
            {
                case 2:
                    return 1.5f;
                case 3:
                    return 1.75f;
                case 4:
                    return 2f;
                case 5:
                    return 2.5f;
                default:
                    return (float)p;
            }
        }

        private static bool ShouldAttack(AIHeroClient unit, string name)
        {
            switch (name)
            {
                case "KindredRNoDeathBuff":
                    {
                        if (unit.HealthPercent <= 10) return false;
                        break;
                    }
                case "FioraW":
                    {
                        return false;
                    }
                case "JudicatorIntervention":
                    {
                        return false;
                    }
                default:
                    break;
            }
            return true;
        }

        public enum Mode
        {
            AutoPriority,
            LowestHP,
            LessCast,
            MostAD,
            MostAP,
            Cloest,
            CloestMouse
        }

        private static Dictionary<string, int> priority = new Dictionary<string, int>()
        {
            { "Aatrox",       2 },
            { "Ahri",         4 },
            { "Akali",        4 },
            { "Alistar",      1 },
            { "Amumu",        1 },
            { "Anivia",       4 },
            { "Annie",        4 },
            { "Ashe",         4 },
            { "AurelionSol",  4 },
            { "Azir",         4 },
            { "Bard",         1 },
            { "Blitzcrank",   1 },
            { "Brand",        4 },
            { "Braum",        1 },
            { "Caitlyn",      4 },
            { "Camille",      3 },
            { "Cassiopeia",   4 },
            { "ChoGath",      2 },
            { "Corki",        4 },
            { "Darius",       2 },
            { "Diana",        3 },
            { "DrMundo",      2 },
            { "Draven",       4 },
            { "Ekko",         4 },
            { "Elise",        2 },
            { "Evelynn",      3 },
            { "Ezreal",       4 },
            { "FiddleSticks", 3 },
            { "Fiora",        3 },
            { "Fizz",         4 },
            { "Galio",        3 },
            { "Gangplank",    3 },
            { "Garen",        2 },
            { "Gnar",         3 },
            { "Gragas",       2 },
            { "Graves",       4 },
            { "Hecarim",      2 },
            { "Heimerdinger", 4 },
            { "Illaoi",       3 },
            { "Irelia",       3 },
            { "Ivern",        2 },
            { "Janna",        1 },
            { "JarvanIV",     2 },
            { "Jax",          3 },
            { "Jayce",        3 },
            { "Jhin",         4 },
            { "Jinx",         4 },
            { "Kalista",      4 },
            { "Karma",        3 },
            { "Karthus",      4 },
            { "Kassadin",     4 },
            { "Katarina",     4 },
            { "Kayle",        3 },
            { "Kennen",       3 },
            { "KhaZix",       3 },
            { "Kindred",      3 },
            { "Kled",         3 },
            { "KogMaw",       4 },
            { "LeBlanc",      4 },
            { "LeeSin",       3 },
            { "Leona",        1 },
            { "Lissandra",    4 },
            { "Lucian",       4 },
            { "Lulu",         3 },
            { "Lux",          4 },
            { "Malphite",     2 },
            { "Malzahar",     4 },
            { "Maokai",       2 },
            { "MasterYi",     4 },
            { "MissFortune",  4 },
            { "MonkeyKing",   2 },
            { "Mordekaiser",  3 },
            { "Morgana",      3 },
            { "Nami",         1 },
            { "Nasus",        2 },
            { "Nautilus",     1 },
            { "Nidalee",      3 },
            { "Nocturne",     3 },
            { "Nunu",         2 },
            { "Olaf",         3 },
            { "Orianna",      4 },
            { "Pantheon",     3 },
            { "Poppy",        2 },
            { "Quinn",        4 },
            { "Rakan",        1 },
            { "Rammus",       1 },
            { "RekSai",       2 },
            { "Renekton",     2 },
            { "Rengar",       3 },
            { "Riven",        3 },
            { "Rumble",       3 },
            { "Ryze",         4 },
            { "Sejuani",      2 },
            { "Shaco",        3 },
            { "Shen",         2 },
            { "Shyvana",      3 },
            { "Singed",       2 },
            { "Sion",         2 },
            { "Sivir",        4 },
            { "Skarner",      2 },
            { "Sona",         1 },
            { "Soraka",       3 },
            { "Swain",        3 },
            { "Syndra",       4 },
            { "TahmKench",    2 },
            { "Taliyah",      4 },
            { "Talon",        4 },
            { "Taric",        1 },
            { "Teemo",        3 },
            { "Thresh",       1 },
            { "Tristana",     4 },
            { "Trundle",      1 },
            { "Tryndamere",   3 },
            { "TwistedFate",  4 },
            { "Twitch",       4 },
            { "Udyr",         3 },
            { "Urgot",        3 },
            { "Varus",        4 },
            { "Vayne",        4 },
            { "Veigar",       4 },
            { "VelKoz",       4 },
            { "Vi",           3 },
            { "Viktor",       4 },
            { "Vladimir",     3 },
            { "Volibear",     3 },
            { "Warwick",      2 },
            { "Xayah",        4 },
            { "Xerath",       4 },
            { "XinZhao",      3 },
            { "Yasuo",        4 },
            { "Yorick",       2 },
            { "Zac",          2 },
            { "Zed",          4 },
            { "Ziggs",        4 },
            { "Zilean",       2 },
            { "Zyra",         3 }
        };


        private static readonly List<AIHeroClient> Enemies = ObjectManager.Heroes.Enemies;
        private readonly Menu menu;
        private readonly Predicate<AIHeroClient> IsValid;
    }
}
