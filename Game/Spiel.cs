using Smake.Speicher;
using Smake.Values;

namespace Smake.Game
{
    public class Spiel
    {
        public static Player[] Player { get; set; } = [];
        public static RenderSpielfeld Render { get; set; } = new(ConfigSystem.Game.Hoehe, ConfigSystem.Game.Weite);

        public Spiel()
        {
            string?[] Namen = Spiellogik.Eingaben();

            Player = [new(Render.Grid, ConfigSystem.Game.Startpositionen.Spieler1.X, ConfigSystem.Game.Startpositionen.Spieler1.Y, Namen[0], Skinvalues.TailSkin[0], Skinvalues.HeadFarbe[0], Skinvalues.TailFarbe[0]), new(Render.Grid, ConfigSystem.Game.Startpositionen.Spieler2.X, ConfigSystem.Game.Startpositionen.Spieler2.Y, Namen[1], Skinvalues.TailSkin[1], Skinvalues.HeadFarbe[1], Skinvalues.TailFarbe[1])];

            Spiellogik.Start(Render.Grid);
            Spiellogik.Spielloop();
        }
    }
}