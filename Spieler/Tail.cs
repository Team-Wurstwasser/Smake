using Smake.Spiel;
using Smake.Values;

namespace Smake.Spieler
{
    public class Tail
    {
        // Länge des Spielers
        public int TailLaenge;

        public char TailSkin;

        public ConsoleColor TailFarbe;

        // Tailkoordinaten berechnen
        public static void TailShift(Player p)
        {
            for (int i = p.PlayerX.Length - 1; i > 0; i--)
            {
                p.PlayerX[i] = p.PlayerX[i - 1];
            }

            for (int i = p.PlayerY.Length - 1; i > 0; i--)
            {
                p.PlayerY[i] = p.PlayerY[i - 1];
            }

        }

        public void TailBewegung(Player p)
        {
            int oldTailX = p.PlayerX[TailLaenge + 1];
            int oldTailY = p.PlayerY[TailLaenge + 1];

            // Spieler-Tail zeichnen
            for (int i = 0; i <= TailLaenge; i++)
            {
                if (p.PlayerX[i] >= 0 && p.PlayerY[i] >= 0)
                    Spiellogik.Grid[p.PlayerY[i], p.PlayerX[i]] = TailSkin;
            }

            // Altes Tail-Feld leeren (nicht Rand)
            if (oldTailX >= 0 && oldTailY >= 0 && Spiellogik.Grid[oldTailY, oldTailX] != Skinvalues.RandSkin)
            {
                Spiellogik.Grid[oldTailY, oldTailX] = ' ';
            }
        }

    }
}
