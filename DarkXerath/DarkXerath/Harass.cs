using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoHarass()
        {            
            if (E.Ready && myHero.ManaPercent >= myMenu.Get<MenuSlider>("hrMPE").CurrentValue && myMenu.Get<MenuCheckbox>("hrE").Checked)
            {
                var ETarget = TSE.GetTarget(myHero.Position, E.Data.Range, (x) => E.Data.GetDamage(x));
                CastE(ETarget);
            }
                        
            if (W.Ready && myHero.ManaPercent >= myMenu.Get<MenuSlider>("hrMPW").CurrentValue && myMenu.Get<MenuCheckbox>("hrW").Checked)
            {
                var WTarget = TSW.GetTarget(myHero.Position, W.Data.Range, (x) => W.Data.GetDamage(x));
                CastW(WTarget);
            }
                        
            if (Q.Ready && (QData.Active || myHero.ManaPercent >= myMenu.Get<MenuSlider>("hrMPQ").CurrentValue) && myMenu.Get<MenuCheckbox>("hrQ").Checked)
            {
                if (myMenu.Get<MenuCheckbox>("hrWE").Checked && !W.Ready && !E.Ready) return;
                var QTarget = TSQ.GetTarget(myHero.Position, Q.Data.ChargedMaxRange + myMenu.Get<MenuSlider>("QExtend").CurrentValue, (x) => Q.Data.GetDamage(x));
                CastQ(QTarget);
            }
        }
    }
}
