using HesaEngine.SDK;
using HesaEngine.SDK.Enums;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void DoLaneClear()
        {
            if (Q.Ready && (QData.Active || myHero.ManaPercent >= myMenu.Get<MenuSlider>("lcMPQ").CurrentValue) && myMenu.Get<MenuCheckbox>("lcQ").Checked)
            {
                var pred = GetLineFarmPosition(myHero.Position, Minions, 90f);
                if (pred.Hits >= myMenu.Get<MenuSlider>("lcHitQ").CurrentValue)
                {
                    if (!QData.Active)
                    {
                        Q.Data.Cast(Game.CursorPosition);
                    }
                    else if (Q.Data.Range >= myHero.Distance(pred.Position))
                    {
                        myHero.Spellbook.UpdateChargedSpell(SpellSlot.Q, pred.Position, true);
                    }
                    return;
                }
            }

            if (W.Ready && myHero.ManaPercent >= myMenu.Get<MenuSlider>("lcMPW").CurrentValue && myMenu.Get<MenuCheckbox>("lcW").Checked)
            {
                var pred = GetFarmPosition(myHero.Position, Minions.FindAll((x) => x.Distance3D(myHero) <= W.Data.Range), 250f);
                if (pred.Hits >= myMenu.Get<MenuSlider>("lcHitW").CurrentValue)
                {
                    W.Data.Cast(pred.Position);
                }
            }
        }
    }
}
