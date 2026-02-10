using Smake.Values;

namespace Smake.Game.Gegenstaende
{
    public class Bombe() : Gegenstand(Skinvalues.BombenSkin)
    {
        public void LöscheBombe()
        {
            RenderSpielfeld.Grid[Y, X] = ' ';
        }
    }
}
