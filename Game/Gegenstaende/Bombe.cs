using Smake.Values;

namespace Smake.Game.Gegenstaende
{
    public class Bombe(char[,] grid) : Gegenstand(grid, Skinvalues.BombenSkin)
    {
        public void LöscheBombe()
        {
            grid[Y, X] = ' ';
        }
    }
}
