using Smake.Render;
using Smake.Speicher;
using Smake.Game;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;
using Smake.Game.Spieler;

namespace Smake.Gegenstaende
{
    public class Futter(char food, ConsoleColor foodfarbe) : Gegenstand(food)
    {
        public ConsoleColor FoodFarbe { get; private set; } = foodfarbe;

        Schluessel? Schluessel;
        Bombe? Bombe;

        // Für Sprungfutter-Modus
        int TeleportCounter;
        readonly int TeleportInterval = GameData.TeleportInterval;

        protected override void Setze()
        {
            base.Setze();

            if (Spielvalues.Gamemode == Gamemodes.SchluesselModus)
            {
                Schluessel = new();
            }

            if (Spielvalues.Gamemode == Gamemodes.SprungfutterModus)
            {
                TeleportCounter = 0;
            }

            if (Spielvalues.Gamemode == Gamemodes.BombenModus)
            {
                Bombe = new();
            }
        }

        protected override void Zeichne()
        {
            // Futter ins Spielfeld einzeichnen
            if (Spielvalues.Gamemode == Gamemodes.SchluesselModus)
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
            if (Spielvalues.Gamemode == Gamemodes.SchluesselModus)
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

                        if (Spielvalues.Gamemode == Gamemodes.MauerModus)
                        {
                            Spiellogik.Mauer.Add(new(Skinvalues.MauerSkin));
                        }

                        if(Spielvalues.Gamemode == Gamemodes.BombenModus)
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

        void Tick()
        {
            if (Spielvalues.Gamemode == Gamemodes.SprungfutterModus)
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
