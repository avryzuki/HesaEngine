using System;
using SharpDX;
using HesaEngine.SDK;

namespace DarkXerath
{
    internal partial class MyScript
    {
        static void Drawings()
        {
            Vector3 Position = myHero.Position;

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
                int tmp = -40;
                foreach (var enemy in Enemies)
                {
                    if (enemy.IsValidTarget(R.Data.Range))
                    {
                        int shot = (int) Math.Ceiling(enemy.APHealth() / R.Data.GetDamage(enemy));
                        string str = shot > 1 ? "Shots" : "Shot";
                        if (shot <= RData.Count)
                        {
                            tmp += 40;
                            RText.DrawText(null, enemy.ChampionName + " - " + shot.ToString() + " " + str, 0, tmp + RTextPosY, Color.Red);
                        }
                    }
                }
            }
        }
    }
}
