using Smake.Render;
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
        public void TailShift(Player p)
        {
            for (int i = TailLaenge + 1; i > 0; i--)
            {
                p.PlayerX[i] = p.PlayerX[i - 1];
            }

            for (int i = TailLaenge + 1; i > 0; i--)
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
                    RendernSpielfeld.Grid[p.PlayerY[i], p.PlayerX[i]] = TailSkin;
            }

            // Prüfen, ob das alte Tail-Feld noch auf einem Player-Segment liegt
            bool isOnPlayer = false;
            for (int i = 0; i <=TailLaenge; i++)
            {
                if (p.PlayerX[i] == oldTailX && p.PlayerY[i] == oldTailY)
                {
                    isOnPlayer = true;
                    break;
                }
            }

            // Altes Tail-Feld nur leeren, wenn es kein Rand und nicht auf einem Spielersegment ist
            if (oldTailX >= 0 && oldTailY >= 0
                && RendernSpielfeld.Grid[oldTailY, oldTailX] != Skinvalues.RandSkin
                && !isOnPlayer)
            {
                RendernSpielfeld.Grid[oldTailY, oldTailX] = ' ';
            }
        }

    }
}
