using Smake.Render;
using Smake.Spieler;
using Smake.Values;
using Smake.SFX;

namespace Smake.Gegenstaende
{
    public class Schluessel
    {
        // Position der Schluessel
        int SchluesselX;
        int SchluesselY;
        public bool Collected = false;

        private static readonly Random Rand = new();

        public Schluessel()
        {
            SetzeSchluessel();
            ZeichneSchluessel();
        }

        // Setzt das Schlüssel an eine zufällige, freie Position
        void SetzeSchluessel()
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
            SchluesselX = x;
            SchluesselY = y;
        }

        void ZeichneSchluessel()
        {
            // Schluessel ins Spielfeld einzeichnen
            RendernSpielfeld.Grid[SchluesselY, SchluesselX] = Skinvalues.SchluesselSkin;
        }

        public void EsseSchluessel(Player p)
        {
            if (!Collected)
            {
                // Überprüfe jedes Segment des Spielers
                for (int i = 0; i < p.TailLaenge; i++)
                {
                    if (p.PlayerX[i] == SchluesselX && p.PlayerY[i] == SchluesselY)
                    {
                        Collected = true;

                        Sounds.Playbeep();

                        // Wenn Schlussel gefunden, können wir die Schleife abbrechen
                        break;
                    }
                }
                ZeichneSchluessel();
            }
        }
    }
}
