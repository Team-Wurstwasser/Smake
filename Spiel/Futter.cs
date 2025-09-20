using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smake.io.Spiel
{
    public class Futter (char food)
    {
        // Position des Futters
        int FutterX;
        int FutterY;

        char food = food;

        // Setzt das Futter an eine Zufällige Position
        public void SetzeFutter()
        {
            Random rand = new();

            // Futter nur auf X-Positionen spawnen lassen, die durch 2 teilbar sind
            do
            {
                FutterX = rand.Next(1, Spiellogik.weite - 2);
                if (FutterX % 2 != 0) FutterX++; // Auf gerade X-Position korrigieren

                FutterY = rand.Next(1, Spiellogik.hoehe - 2);
            }
            while (Spiellogik.grid[FutterY, FutterX] != ' '); // Stelle muss wirklich leer sein
            Spiellogik.grid[FutterY, FutterX] = food; // Setze Futter an die berechnete Position

        }

        public void EsseFutter(Spieler p)
        {
            // Spieler frisst Futter
            if (p.PlayerX[0] == FutterX && p.PlayerY[0] == FutterY)
            {
                p.Tail++;
                p.Punkte++;
                if (Musik.soundplay)
                {
                    Console.Beep(700, 100);
                }
                SetzeFutter();
            }
        }
    }
}
