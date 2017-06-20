using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoKillSteal()
        {
            foreach (var enemy in Enemies)
            {
                if (Q.Ready && enemy.IsValidTarget(Q.Data.ChargedMaxRange) && Q.Data.GetDamage(enemy) > enemy.APHealth() && (QData.Active || myHero.ManaPercent >= myMenu.Get<MenuSlider>("ksMPQ").CurrentValue) && myMenu.Get<MenuCheckbox>("ksQ").Checked)
                {
                    CastQ(enemy);
                    return;
                }

                if (W.Ready && enemy.IsValidTarget(W.Data.Range) && W.Data.GetDamage(enemy) > enemy.APHealth() && myHero.ManaPercent >= myMenu.Get<MenuSlider>("ksMPW").CurrentValue && myMenu.Get<MenuCheckbox>("ksW").Checked)
                {
                    CastW(enemy);
                    return;
                }

                if (E.Ready && enemy.IsValidTarget(E.Data.Range) && E.Data.GetDamage(enemy) > enemy.APHealth() && myHero.ManaPercent >= myMenu.Get<MenuSlider>("ksMPE").CurrentValue && myMenu.Get<MenuCheckbox>("ksE").Checked)
                {
                    CastE(enemy);
                    return;
                }

                if (Ignite != null && Ignite.IsReady() && enemy.IsValidTarget(Ignite.Range) && myHero.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite) > enemy.ADHealth() + enemy.HPRegenPerSecond * 2.5f && myMenu.Get<MenuCheckbox>("ksIgnite").Checked)
                {
                    Ignite.Cast(enemy);
                }
            }
        }
    }
}
