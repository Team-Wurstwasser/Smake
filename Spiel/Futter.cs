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

        public Futter(char Food, ConsoleColor Foodfarbe)
        {
            this.Food = Food;
            this.Foodfarbe = Foodfarbe;
            SetzeFutter();
        }

        // Setzt das Futter an eine Zufällige Position
        void SetzeFutter()
        {
            Random rand = new();

            // Futter nur auf X-Positionen spawnen lassen, die durch 2 teilbar sind
            do
            {
                FutterX = rand.Next(1, Spielvalues.weite - 2);
                if (FutterX % 2 != 0) FutterX++; // Auf gerade X-Position korrigieren

                FutterY = rand.Next(1, Spielvalues.hoehe - 2);
            }
            while (Spiellogik.Grid[FutterY, FutterX] != ' '); // Stelle muss wirklich leer sein
            Spiellogik.Grid[FutterY, FutterX] = Food; // Setze Futter an die berechnete Position

        }

        public void EsseFutter(Player p)
        {
            // Spieler frisst Futter
            if (p.PlayerX[0] == FutterX && p.PlayerY[0] == FutterY)
            {
                p.TailLaenge++;
                p.Punkte++;
                if (Musik.Soundplay)
                {
                    Console.Beep(700, 100);
                }
                SetzeFutter();
            }
        }
    }
}
