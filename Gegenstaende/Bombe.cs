using Smake.Render;
using Smake.Values;

namespace Smake.Gegenstaende
{
    public class Bombe() : Gegenstand(Skinvalues.BombenSkin)
    {
        public void LöscheBombe()
        {
            RendernSpielfeld.Grid[Y, X] = ' ';
        }
    }
}
