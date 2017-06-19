using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoJungleClear()
        {
            Obj_AI_Minion Mob = null;
            ObjectManager.MinionsAndMonsters.NeutralCamps.ForEach((x) =>
            {
                if (x.IsValidTarget(Q.Data.ChargedMaxRange))
                {
                    if (Mob == null || (x.MaxHealth > Mob.MaxHealth))
                        Mob = x;
                }
            });

            if (Q.Ready && Mob != null && (QData.Active || myHero.ManaPercent >= myMenu.Get<MenuSlider>("jcMPQ").CurrentValue) && myMenu.Get<MenuCheckbox>("jcQ").Checked)
            {
                if (!QData.Active)
                {
                    Q.Data.Cast(Game.CursorPosition);
                }
                else if (Mob.Distance3D(myHero) <= Q.Data.Range)
                {
                    myHero.Spellbook.UpdateChargedSpell(Q.Data.Slot, Mob.Position, true);
                }
                return;
            }

            if (W.Ready && Mob != null && Mob.Distance3D(myHero) <= W.Data.Range && myHero.ManaPercent >= myMenu.Get<MenuSlider>("jcMPW").CurrentValue && myMenu.Get<MenuCheckbox>("jcW").Checked)
            {
                W.Data.Cast(Mob.Position);
                return;
            }

            if (E.Ready && Mob != null && Mob.Distance3D(myHero) <= E.Data.Range && myHero.ManaPercent >= myMenu.Get<MenuSlider>("jcMPE").CurrentValue && myMenu.Get<MenuCheckbox>("jcE").Checked)
            {
                E.Data.Cast(Mob.Position);
                return;
            }
        }
    }
}
