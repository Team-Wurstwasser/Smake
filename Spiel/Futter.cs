using Smake.io.Values;
using Smake.io.Spieler;

namespace Smake.io.Spiel
{
    public class Futter
    {
        // Position des Futters
        public int FutterX { get; private set; }
        public int FutterY { get; private set; }

        public char Food { get; private set; }
        public ConsoleColor Foodfarbe { get; private set; }

        private static Random Rand = new();

        public Futter(char food, ConsoleColor foodfarbe)
        {
            this.Food = food;
            this.Foodfarbe = foodfarbe;
            SetzeFutter();
        }

        // Setzt das Futter an eine zufällige, freie Position
        void SetzeFutter()
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
            } while (Spiellogik.Grid[y, x] != ' ');

            // Setze Position
            FutterX = x;
            FutterY = y;

            // Futter ins Spielfeld einzeichnen
            Spiellogik.Grid[FutterY, FutterX] = Food;
        }

        public void EsseFutter(Player p)
        {
            // Spieler frisst Futter (Kopf-Kollision)
            if (p.PlayerX[0] == FutterX && p.PlayerY[0] == FutterY)
            {
                p.TailLaenge++;
                p.Punkte++;

                if (Musik.Soundplay)
                    Console.Beep(700, 100);

                SetzeFutter();
            }
        }
    }
}
