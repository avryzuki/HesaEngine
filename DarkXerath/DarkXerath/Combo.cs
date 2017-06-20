using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoCombo()
        {
            TargetSelector.Mode = TargetSelector.TargetingMode.LessCast;
            var enemy = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);
            //var WTarget = TSW.GetTarget(myHero, W.Data.Range/*, (x) => W.Data.GetDamage(x)*/);
            if (W.Ready/* && myHero.ManaPercent >= myMenu.Get<MenuSlider>("cbMPW").CurrentValue */&& myMenu.Get<MenuCheckbox>("cbW").Checked)
            {
                CastW(enemy);
            }

            var ETarget = TSE.GetTarget(myHero, E.Data.Range/*, (x) => E.Data.GetDamage(x)*/);
            if (E.Ready/* && myHero.ManaPercent >= myMenu.Get<MenuSlider>("cbMPE").CurrentValue */&& myMenu.Get<MenuCheckbox>("cbE").Checked)
            {
                CastE(ETarget);
            }

            

            //var QTarget = TSQ.GetTarget(myHero, Q.Data.ChargedMaxRange/*, (x) => Q.Data.GetDamage(x)*/);
            if (Q.Ready && (QData.Active || myHero.ManaPercent >= myMenu.Get<MenuSlider>("cbMPQ").CurrentValue) && myMenu.Get<MenuCheckbox>("cbQ").Checked)
            {
                if (myMenu.Get<MenuCheckbox>("cbWE").Checked && !W.Ready && !E.Ready) return;
                CastQ(enemy);
            }
        }
    }
}
