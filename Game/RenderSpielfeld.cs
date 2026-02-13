using Smake.Enums;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Game
{
    public abstract class RenderSpielfeld
    {
        // Das Spielfeld als zweidimensionales Zeichen-Array
        public static char[,] Grid { get; set; } = new char[Spielvalues.hoehe, Spielvalues.weite];

        // Vorheriges Frame für Performance-Rendering
        readonly char[,] PrevGrid = new char[Spielvalues.hoehe, Spielvalues.weite];

        // Initialisiert das Spielfeld: Rahmen, leere Fläche
        public void InitialisiereSpielfeld()
        {
            Console.Clear();

            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {

                    if (y == 0 || y == Grid.GetLength(0) - 1 || x == 0 || x >= Grid.GetLength(1) - 1)
                    {
                        Grid[y, x] = Skinvalues.RandSkin;
                        PrevGrid[y, x] = Skinvalues.RandSkin;
                    }
                    else
                    {
                        Grid[y, x] = ' ';
                        PrevGrid[y, x] = '\0';
                    }

                }
            }
            RenderRand();
        }

        static void RenderRand()
        {
            int rows = Grid.GetLength(0);
            int cols = Grid.GetLength(1);

            if (!Spielvalues.Performancemode)
            {
                Console.ForegroundColor = Skinvalues.RandFarbe;
            }
            else
            {
                Console.ResetColor();
            }

            // Obere und untere Randlinie
            for (int x = 0; x < cols; x++)
            {
                // Oben
                Console.SetCursorPosition(x, 0);
                Console.Write(Skinvalues.RandSkin);

                // Unten
                Console.SetCursorPosition(x, rows - 1);
                Console.Write(Skinvalues.RandSkin);
            }

            // Linke und rechte Randlinie
            for (int y = 1; y < rows - 1; y++)
            {
                int dicke = 1;
                // Links
                Console.SetCursorPosition(0, y);
                Console.Write(new string(Skinvalues.RandSkin, dicke)); // "██"

                // Rechts
                Console.SetCursorPosition(cols - dicke, y);
                Console.Write(new string(Skinvalues.RandSkin, dicke)); // "██"
            }
        }

        public void Render()
        {
            if (Spielvalues.Performancemode)
            {
                RenderPerformance();
            }
            else
            {
                RenderFull();
            }
        }

        // Normaler Modus: komplette Ausgabe mit Farben + Legende
        void RenderFull()
        {
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            int rows = Grid.GetLength(0) - 1;
            int cols = Grid.GetLength(1) - 1;

            for (int y = 1; y < rows; y++)
            {
                for (int x = 2; x < cols - 1; x++)
                {
                    bool IstStartposition = (x == Spiel.Player[0].StartX && y == Spiel.Player[0].StartY) || (x == Spiel.Player[1].StartX && y == Spiel.Player[1].StartY);

                    if (Grid[y, x] != PrevGrid[y, x] || IstStartposition)
                    {
                        char zeichen = Grid[y, x];
                        ConsoleColor neueFarbe = BestimmeFarbe(x, y, zeichen);
                        if (neueFarbe != aktuelleFarbe)
                        {
                            Console.ForegroundColor = neueFarbe;
                            aktuelleFarbe = neueFarbe;
                        }
                        Console.SetCursorPosition(x, y);
                        Console.Write(Grid[y, x]);
                        PrevGrid[y, x] = Grid[y, x];
                    }

                }

                aktuelleFarbe = RenderLegende(y, aktuelleFarbe);
                Console.WriteLine();
            }

            Console.ResetColor();
        }

        // Performance-Modus: nur geänderte Zeichen zeichnen
        void RenderPerformance()
        {
            int rows = Grid.GetLength(0);
            int cols = Grid.GetLength(1);

            for (int y = 1; y < rows; y++)
            {
                for (int x = 2; x < cols - 1; x++)
                {
                    if (Grid[y, x] != PrevGrid[y, x])
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(Grid[y, x]);
                        PrevGrid[y, x] = Grid[y, x];
                    }
                }

                // Legende separat behandeln
                string legende = RenderLegendeText(y);
                if (!string.IsNullOrEmpty(legende))
                {
                    Console.SetCursorPosition(cols + 1, y);
                    Console.Write(legende);
                }
            }
        }

        // Hilfsfunktion für Legende im Performance-Modus (nur Text)
        static string RenderLegendeText(int y)
        {
            switch (y)
            {
                case 1: return "  ══════════════════════════════";
                case 2: return LanguageSystem.Get("legende");
                case 3: return "  ══════════════════════════════";
                case 4:
                    string maxpunkte = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? ConfigSystem.Game.MaxPunkte.ToString() : "∞";
                    return $"  {Spiel.Player[0].Name}: {Spiel.Player[0].Punkte}/{maxpunkte}";
                case 5: return "  ══════════════════════════════";
                case 6:
                    if (Spielvalues.Multiplayer)
                    {
                        string maxpunkte2 = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? ConfigSystem.Game.MaxPunkte.ToString() : "∞";
                        return $"  {Spiel.Player[1].Name}: {Spiel.Player[1].Punkte}/{maxpunkte2}";
                    }
                    break;
                case 7:
                    if (Spielvalues.Multiplayer)
                        return "  ══════════════════════════════";
                    break;
            }
            return string.Empty;
        }

        // Farb-Bestimmung (nur für normalen Modus gebraucht)
        static ConsoleColor BestimmeFarbe(int x, int y, char zeichen)
        {
            int aktiveSpieler = Spielvalues.Multiplayer ? Spiel.Player.Length : 1;
            for (int i = 0; i < aktiveSpieler; i++)
            {
                if (x == Spiel.Player[i].PlayerX[0] && y == Spiel.Player[i].PlayerY[0])
                    return Spiel.Player[i].HeadFarbe;
            }

            ConsoleColor? objektFarbe = zeichen switch
            {
                _ when zeichen == Spiel.Player[0].TailSkin => Spiel.Player[0].TailFarbe,
                _ when Spielvalues.Multiplayer && zeichen == Spiel.Player[1].TailSkin => Spiel.Player[1].TailFarbe,
                _ when zeichen == Skinvalues.MauerSkin => Skinvalues.MauerFarbe,
                _ when zeichen == Skinvalues.BombenSkin => Skinvalues.BombenFarbe,
                _ when zeichen == Skinvalues.SchluesselSkin => Skinvalues.SchluesselFarbe,
                _ => null
            };

            if (objektFarbe.HasValue) return objektFarbe.Value;

            var essen = Spiellogik.Essen.FirstOrDefault(e => e.X == x && e.Y == y);
            if (essen != null) return essen.FoodFarbe;

            return ConsoleColor.White;
        }

        static ConsoleColor RenderLegende(int y, ConsoleColor aktuelleFarbe)
        {
            void SetFarbe(ConsoleColor farbe)
            {
                Console.SetCursorPosition(ConfigSystem.Game.Weite, y);

                if (farbe != aktuelleFarbe)
                {
                    Console.ForegroundColor = farbe;
                    aktuelleFarbe = farbe;
                }
            }

            switch (y)
            {
                case 1:
                    SetFarbe(Skinvalues.RandFarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 2:
                    SetFarbe(ConsoleColor.White);
                    Console.Write(LanguageSystem.Get("legende"));
                    break;
                case 3:
                    SetFarbe(Skinvalues.RandFarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 4:
                    SetFarbe(Spiel.Player[0].HeadFarbe);
                    string maxpunkte = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? ConfigSystem.Game.MaxPunkte.ToString() : "∞";
                    Console.Write($"  {Spiel.Player[0].Name}: {Spiel.Player[0].Punkte}/{maxpunkte}");
                    break;
                case 5:
                    SetFarbe(Skinvalues.RandFarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 6:
                    if (Spielvalues.Multiplayer)
                    {
                        SetFarbe(Spiel.Player[1].HeadFarbe);
                        string maxpunkte2 = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? ConfigSystem.Game.MaxPunkte.ToString() : "∞";
                        Console.Write($"  {Spiel.Player[1].Name}: {Spiel.Player[1].Punkte}/{maxpunkte2}");
                    }
                    break;
                case 7:
                    if (Spielvalues.Multiplayer)
                    {
                        SetFarbe(Skinvalues.RandFarbe);
                        Console.Write("  ══════════════════════════════");
                    }
                    break;
            }

            return aktuelleFarbe;
        }
    }
}
