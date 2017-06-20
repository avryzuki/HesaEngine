using SharpDX;
using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void Drawings()
        {
            Vector3 Position = myHero.Position;
            /*var enemyq = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Magical);

            if (enemyq != null)
            {
                Drawing.DrawCircle(DarkPrediction.CirclerPrediction(Q.Data, enemyq, 1), 50, Color.Pink);
            }*/

            if (Q.Ready)
            {
                if (!QData.Active && myMenu.Get<MenuCheckbox>("drawQMin").Checked)
                    Drawing.DrawCircle(Position, Q.Data.ChargedMinRange, new ColorBGRA(0, 255, 255, 175), 1);

                if (myMenu.Get<MenuCheckbox>("drawQMax").Checked)
                    Drawing.DrawCircle(Position, Q.Data.ChargedMaxRange, new ColorBGRA(0, 255, 255, 175), 1);
            }

            if ((W.Ready || E.Ready) && myMenu.Get<MenuCheckbox>("drawWE").Checked)
            {
                Drawing.DrawCircle(Position, W.Data.Range, new ColorBGRA(124, 252, 0, 175), 1);
            }

            if (R.Ready && myMenu.Get<MenuCheckbox>("drawR").Checked)
            {
                Drawing.DrawCircle(Position, R.Data.Range, new ColorBGRA(255, 255, 0, 225), 1);
            }

            if (RData.Active && myMenu.Get<MenuCheckbox>("drawRMouse").Checked && myMenu.Get<MenuCombo>("RMode").CurrentValue == 1)
            {
                Drawing.DrawCircle(Game.CursorPosition, R2Range, new ColorBGRA(147, 112, 219, 255), 1);
            }

            if (R.Ready && myMenu.Get<MenuCheckbox>("drawRText").Checked)
            {
                int tmp = 0;
                foreach (var enemy in Enemies)
                {
                    if (enemy.IsValidTarget(R.Data.Range) && R.Data.GetDamage(enemy) * RData.Count > enemy.APHealth())
                    {
                        tmp += 15;
                        Drawing.DrawText(enemy.ChampionName + " Killable", 0f, Drawing.Height / 4f + tmp, Color.Red);
                    }
                }
            }
        }
    }
}
