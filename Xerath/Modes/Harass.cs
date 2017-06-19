using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoHarass()
        {
            var QTarget = TSQ.GetTarget(myHero, Q.Data.ChargedMaxRange, (x) => Q.Data.GetDamage(x));
            if (Q.Ready && (QData.Active || myHero.ManaPercent >= myMenu.Get<MenuSlider>("hrMPQ").CurrentValue) && myMenu.Get<MenuCheckbox>("hrQ").Checked)
            {
                if (myMenu.Get<MenuCheckbox>("hrWE").Checked && !W.Ready && !E.Ready) return;
                CastQ(QTarget);
                return;
            }

            var WTarget = TSW.GetTarget(myHero, W.Data.Range, (x) => W.Data.GetDamage(x));
            if (W.Ready &&myHero.ManaPercent >= myMenu.Get<MenuSlider>("hrMPW").CurrentValue && myMenu.Get<MenuCheckbox>("hrW").Checked)
            {
                CastW(WTarget);
                return;
            }

            var ETarget = TSE.GetTarget(myHero, E.Data.Range, (x) => E.Data.GetDamage(x));
            if (E.Ready && myHero.ManaPercent >= myMenu.Get<MenuSlider>("hrMPE").CurrentValue && myMenu.Get<MenuCheckbox>("hrE").Checked)
            {
                CastE(ETarget);
                return;
            }
        }
    }
}
