using Smake.Render;
using Smake.Spiel;
using Smake.Spieler;
using Smake.Values;

namespace Smake.Gegenstaende
{
    public class Futter
    {
        // Position des Futters
        public int FutterX { get; private set; }
        public int FutterY { get; private set; }

        public char FoodSkin { get; private set; }
        public ConsoleColor FoodFarbe { get; private set; }

        private static Random Rand = new();
        private Schluessel? Schluessel;

        // Für Sprungfutter-Modus
        private int TeleportCounter = 0;
        private readonly int TeleportInterval = 25;
        public Futter(char food, ConsoleColor foodfarbe)
        {
            FoodSkin = food;
            FoodFarbe = foodfarbe;
            SetzeFutter();
            ZeichneFutter();

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
            } while (RendernSpielfeld.Grid[y, x] != ' ');

            // Setze Position
            FutterX = x;
            FutterY = y;

            if (Spielvalues.Gamemode == "Schlüssel-Modus")
            {
                Schluessel = new();
            }

            if (Spielvalues.Gamemode == "Sprungfutter-Modus")
            {
                TeleportCounter = 0;
            }

        }

        private void ZeichneFutter()
        {
            // Futter ins Spielfeld einzeichnen
            if (Spielvalues.Gamemode == "Schlüssel-Modus")
            {
                if (!Schluessel.Collected)
                {
                    RendernSpielfeld.Grid[FutterY, FutterX] = Skinvalues.MauerSkin;
                }
                else
                {
                    RendernSpielfeld.Grid[FutterY, FutterX] = FoodSkin;
                }

            }
            else
            {
                RendernSpielfeld.Grid[FutterY, FutterX] = FoodSkin;
            }
        }

        public void EsseFutter(Player p)
        {
            if (Spielvalues.Gamemode == "Schlüssel-Modus" && !Schluessel.Collected)
            {
                Schluessel.EsseSchluessel(p);
            }
            else
            {
                // Überprüfe jedes Segment des Spielers
                for (int i = 0; i < p.TailLaenge; i++)
                {
                    if (p.PlayerX[i] == FutterX && p.PlayerY[i] == FutterY)
                    {

                        p.Punkte+=20;

                        if (Musik.Soundplay)
                        {
                            Console.Beep(700, 100);
                        }

                        if (Spielvalues.Gamemode == "Mauer-Modus")
                        {
                            Spiellogik.Mauer.Add(new());
                        }

                        SetzeFutter();

                        // Wenn Futter gefunden, können wir die Schleife abbrechen
                        break;
                    }
                }
            }
            Tick();
            ZeichneFutter();
        }

        private void Tick()
        {
            if (Spielvalues.Gamemode == "Sprungfutter-Modus")
            {
                TeleportCounter++;
                if (TeleportCounter >= TeleportInterval)
                {
                    // Alte Position löschen
                    RendernSpielfeld.Grid[FutterY, FutterX] = ' ';
                    SetzeFutter();
                }
            }
        }
    }
}
