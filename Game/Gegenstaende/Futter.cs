using Smake.Speicher;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Game.Gegenstaende
{
    public class Futter(char[,] grid, char food, ConsoleColor foodfarbe) : Gegenstand(grid, food)
    {
        public ConsoleColor FoodFarbe { get; private set; } = foodfarbe;

        Schluessel? Schluessel;
        Bombe? Bombe;

        // Für Sprungfutter-Modus
        int TeleportCounter;
        readonly int TeleportInterval = ConfigSystem.Game.TeleportInterval;

        protected override void Setze()
        {
            base.Setze();

            if (Spielvalues.Gamemode == Gamemodes.SchluesselModus)
            {
                Schluessel = new(grid);
            }

            if (Spielvalues.Gamemode == Gamemodes.SprungfutterModus)
            {
                TeleportCounter = 0;
            }

            if (Spielvalues.Gamemode == Gamemodes.BombenModus)
            {
                Bombe = new(grid);
            }
        }

        protected override void Zeichne()
        {
            // Futter ins Spielfeld einzeichnen
            if (Spielvalues.Gamemode == Gamemodes.SchluesselModus)
            {
                if (Schluessel != null)
                {
                    if (!Schluessel.Collected)
                    {
                        grid[Y, X] = Skinvalues.MauerSkin;
                    }
                    else
                    {
                        grid[Y, X] = Skin;
                    }
                }
            }
            else
            {
                grid[Y, X] = Skin;
            }
        }

        public void EsseFutter(char[,] grid,Player p)
        {
            if (Spielvalues.Gamemode == Gamemodes.SchluesselModus)
            {
                if (Schluessel != null && !Schluessel.Collected)
                {
                    Schluessel.EsseSchluessel(p);
                }
                else
                {
                    // Überprüfe jedes Segment des Spielers
                    for (int i = 0; i < p.TailLaenge; i++)
                    {
                        if (p.PlayerX[i] == X && p.PlayerY[i] == Y)
                        {
                            p.AddPunkt();

                            Sounds.Playbeep();

                            Setze();

                            // Wenn Futter gefunden, können wir die Schleife abbrechen
                            break;
                        }
                    }
                }
            }
            else
            {
                // Überprüfe jedes Segment des Spielers
                for (int i = 0; i < p.TailLaenge; i++)
                {
                    if (p.PlayerX[i] == X && p.PlayerY[i] == Y)
                    {
                        p.AddPunkt();

                        Sounds.Playbeep();

                        if (Spielvalues.Gamemode == Gamemodes.MauerModus)
                        {
                            Spiellogik.Mauer.Add(new(grid, Skinvalues.MauerSkin));
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
                    grid[Y, X] = ' ';
                    Setze();
                }
            }
        }
    }
}
