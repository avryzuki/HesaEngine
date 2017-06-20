using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoCombo()
        {
            var WTarget = TSW.GetTarget(myHero, W.Data.Range, (x) => W.Data.GetDamage(x));
            if (W.Ready && myMenu.Get<MenuCheckbox>("cbW").Checked)
            {
                CastW(WTarget);
            }

            var ETarget = TSE.GetTarget(myHero, E.Data.Range, (x) => E.Data.GetDamage(x));
            if (E.Ready&& myMenu.Get<MenuCheckbox>("cbE").Checked)
            {
                CastE(ETarget);
            }

            var QTarget = TSQ.GetTarget(myHero, Q.Data.ChargedMaxRange, (x) => Q.Data.GetDamage(x));
            if (Q.Ready && myMenu.Get<MenuCheckbox>("cbQ").Checked)
            {
                if (myMenu.Get<MenuCheckbox>("cbWE").Checked && !W.Ready && !E.Ready) return;
                CastQ(QTarget);
            }
        }
    }
}
