using HesaEngine.SDK;

namespace Xerath
{
    internal partial class MyScript
    {
        static void UseUltimate()
        {
            if (!Enemies.Exists((x) => x.Distance3D(myHero) <= 1000))
            {
                Orbwalker.Move = false;
            }
            else
            {
                Orbwalker.Move = true;
            }

            switch (myMenu.Get<MenuCombo>("RMode").CurrentValue)
            {
                case 0:
                    {
                        if (myMenu.Get<MenuKeybind>("RKey").Active)
                        {
                            CastR(GetRTarget(myHero.Position, R.Data.Range));
                        }
                        break;
                    }

                case 1:
                    {
                        CastR(GetRTarget(Game.CursorPosition, R2Range));
                        break;
                    }

                case 2:
                    {
                        CastR(GetRTarget(myHero.Position, R.Data.Range));
                        break;
                    }

                default:
                    break;
            }
        }
    }
}
