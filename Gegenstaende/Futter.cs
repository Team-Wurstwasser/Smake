using Smake.Render;
using Smake.Speicher;
using Smake.Spiel;
using Smake.Spieler;
using Smake.Values;
using Smake.SFX;

namespace Smake.Gegenstaende
{
    public class Futter(char food, ConsoleColor foodfarbe) : Gegenstand(food)
    {
        public ConsoleColor FoodFarbe { get; private set; } = foodfarbe;

        private Schluessel? Schluessel;
        private Bombe? Bombe;

        // Für Sprungfutter-Modus
        private int TeleportCounter = 0;
        private readonly int TeleportInterval = GameData.TeleportInterval;

        protected override void Setze()
        {
            base.Setze();

            if (Spielvalues.GamemodeInt == 6)
            {
                Schluessel = new();
            }

            if (Spielvalues.GamemodeInt == 8)
            {
                Bombe = new();
            }

            if (Spielvalues.GamemodeInt == 7)
            {
                TeleportCounter = 0;
            }
        }

        protected override void Zeichne()
        {
            // Futter ins Spielfeld einzeichnen
            if (Spielvalues.GamemodeInt == 6)
            {
                if (Schluessel != null && Schluessel.Collected)
                {
                    if (!Schluessel.Collected)
                    {
                        RendernSpielfeld.Grid[Y, X] = Skinvalues.MauerSkin;
                    }
                    else
                    {
                        RendernSpielfeld.Grid[Y, X] = Skin;
                    }
                }
            }
            else
            {
                RendernSpielfeld.Grid[Y, X] = Skin;
            }
        }

        public void EsseFutter(Player p)
        {
            if (Spielvalues.GamemodeInt == 6)
            {
                if (Schluessel != null && !Schluessel.Collected)
                {
                    Schluessel.EsseSchluessel(p);
                }
            }
            else
            {
                // Überprüfe jedes Segment des Spielers
                for (int i = 0; i < p.TailLaenge; i++)
                {
                    if (p.PlayerX[i] == X && p.PlayerY[i] == Y)
                    {

                        p.Punkte++;

                        Sounds.Playbeep();

                        if (Spielvalues.GamemodeInt == 5)
                        {
                            Spiellogik.Mauer.Add(new(Skinvalues.MauerSkin));
                        }

                        if(Spielvalues.GamemodeInt == 8)
                        {
                            Bombe?.LöscheBombe();
                        }

                        Setze();

                        // Wenn Futter gefunden, können wir die Schleife abbrechen
                        break;
                    }
                }
            }
            Tick();
            Zeichne();
        }

        private void Tick()
        {
            if (Spielvalues.GamemodeInt == 7)
            {
                TeleportCounter++;
                if (TeleportCounter >= TeleportInterval)
                {
                    // Alte Position löschen
                    RendernSpielfeld.Grid[Y, X] = ' ';
                    Setze();
                }
            }
        }
    }
}
