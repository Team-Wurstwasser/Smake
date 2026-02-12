using Smake.Values;

namespace Smake.Game
{
    public class Spiel
    {
        public Spiel()
        {
            Spiellogik.Player.TailFarbe = Skinvalues.TailFarbe[0];
            Spiellogik.Player.HeadFarbe = Skinvalues.HeadFarbe[0];
            Spiellogik.Player2.TailFarbe = Skinvalues.TailFarbe[1];
            Spiellogik.Player2.HeadFarbe = Skinvalues.HeadFarbe[1];

            Spiellogik.Player.TailSkin = Skinvalues.TailSkin[0];
            Spiellogik.Player.HeadSkin = Skinvalues.HeadSkin[0];
            Spiellogik.Player2.TailSkin = Skinvalues.TailSkin[1];
            Spiellogik.Player2.HeadSkin = Skinvalues.HeadSkin[1];

            Spiellogik spiel = new();
            spiel.Spielloop();
        }
    }
}
