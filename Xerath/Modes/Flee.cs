using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void Flee()
        {
            if (W.Ready && myHero.ManaPercent >= myMenu.Get<MenuSlider>("fleeMPW").CurrentValue && myMenu.Get<MenuCheckbox>("fleeW").Checked)
            {
                AIHeroClient unit = null;
                foreach (var enemy in Enemies)
                {
                    if (enemy.IsValidTarget(W.Data.Range))
                    {
                        if (unit == null || (unit.Distance3D(myHero) > enemy.Distance3D(myHero)))
                            unit = enemy;
                    }
                }
                
                CastW(unit);
                return;
            }

            if (E.Ready && myHero.ManaPercent >= myMenu.Get<MenuSlider>("fleeMPE").CurrentValue && myMenu.Get<MenuCheckbox>("fleeE").Checked)
            {
                AIHeroClient unit = null;
                foreach (var enemy in Enemies)
                {
                    if (enemy.IsValidTarget(W.Data.Range))
                    {
                        if (unit == null || (unit.Distance3D(myHero) > enemy.Distance3D(myHero)))
                            unit = enemy;
                    }
                }
                CastE(unit);
            }
        }
    }
}
