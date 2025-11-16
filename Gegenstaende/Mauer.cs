using Smake.Render;
using Smake.Values;

namespace Smake.Gegenstaende
{
    public class Mauer
    {
        // Position der Mauer
        int MauerX;
        int MauerY;

        private static readonly Random Rand = new();

        public Mauer()
        {
            SetzeMauer();
            ZeichneMauer();
        }

        // Setzt das Mauer an eine zufällige, freie Position
        void SetzeMauer()
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
            MauerX = x;
            MauerY = y;
        }

        public void ZeichneMauer()
        {
            // Mauer ins Spielfeld einzeichnen
            RendernSpielfeld.Grid[MauerY, MauerX] = Skinvalues.MauerSkin;
        }
    }
}
