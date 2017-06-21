using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoCombo()
        {     
            if (W.Ready && myMenu.Get<MenuCheckbox>("cbW").Checked)
            {
                var WTarget = TSW.GetTarget(myHero, W.Data.Range, (x) => W.Data.GetDamage(x));
                CastW(WTarget);
            }
                        
            if (E.Ready && myMenu.Get<MenuCheckbox>("cbE").Checked)
            {
                var ETarget = TSE.GetTarget(myHero, E.Data.Range, (x) => E.Data.GetDamage(x));
                CastE(ETarget);
            }
                        
            if (Q.Ready && myMenu.Get<MenuCheckbox>("cbQ").Checked)
            {
                if (myMenu.Get<MenuCheckbox>("cbWE").Checked && !W.Ready && !E.Ready) return;
                var QTarget = TSQ.GetTarget(myHero, Q.Data.ChargedMaxRange, (x) => Q.Data.GetDamage(x));
                CastQ(QTarget);
            }
        }
    }
}
