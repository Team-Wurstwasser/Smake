using Smake.Render;
using Smake.Spieler;
using Smake.Values;

namespace Smake.Gegenstaende
{
    public class Bombe
    {
        // Position der Bombe
        int BombeX;
        int BombeY;

        private static readonly Random Rand = new();

        public Bombe()
        {
            SetzeBombe();
            ZeichneBombe();
        }

        // Setzt das Bombe an eine zufällige, freie Position
        void SetzeBombe()
        {
            int x, y;

            do
            {
                // Zufalls-X (immer gerade Zahl, damit zur Snake passt)
                x = Rand.Next(1, Spielvalues.weite - 2);
                if (x % 2 != 0 && x < Spielvalues.weite - 2)
                    x++;

                // Zufalls-Y
                y = Rand.Next(1, Spielvalues.hoehe - 2);

                // Wiederholen solange die Stelle nicht frei ist
            } while (RendernSpielfeld.Grid[y, x] != ' ');

            // Setze Position
            BombeX = x;
            BombeY = y;
        }

        public void ZeichneBombe()
        {
            // Bombe ins Spielfeld einzeichnen
            RendernSpielfeld.Grid[BombeY, BombeX] = Skinvalues.BombenSkin;
        }

        public void LöscheBombe()
        {
            RendernSpielfeld.Grid[BombeY, BombeX] = ' ';
        }
    }
}
