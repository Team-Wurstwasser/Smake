using Smake.Enums;
using Smake.Menues;
using Smake.Speicher;
using Smake.Spiel;
using Smake.Values;

namespace Smake.Render
{
    public class RendernSpielfeld
    {
        public static bool Performancemode { get; set; } 

        // Das Spielfeld als zweidimensionales Zeichen-Array
        public static char[,] Grid { get; set; } = new char[Spielvalues.hoehe, Spielvalues.weite];

        // Vorheriges Frame für Performance-Rendering
        private readonly char[,] PrevGrid = new char[Spielvalues.hoehe, Spielvalues.weite];

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

        private static void RenderRand()
        {
            int rows = Grid.GetLength(0);
            int cols = Grid.GetLength(1);

            if (!Performancemode)
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
            if (Performancemode)
            {
                RenderPerformance();
            }
            else
            {
                RenderFull();
            }
        }

        // Normaler Modus: komplette Ausgabe mit Farben + Legende
        private void RenderFull()
        {
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            int rows = Grid.GetLength(0) - 1;
            int cols = Grid.GetLength(1) - 1;

            for (int y = 1; y < rows; y++)
            {
                for (int x = 2; x < cols - 1; x++)
                {
                    bool IstStartposition = (x == Spiellogik.Player.xstart && y == Spiellogik.Player.ystart) || (x == Spiellogik.Player2.xstart && y == Spiellogik.Player2.ystart);

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
        private void RenderPerformance()
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
        private static string RenderLegendeText(int y)
        {
            switch (y)
            {
                case 1: return "  ══════════════════════════════";
                case 2: return LanguageManager.Get("legende");
                case 3: return "  ══════════════════════════════";
                case 4:
                    string maxpunkte = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? GameData.MaxPunkte.ToString() : "∞";
                    return $"  {Spiellogik.Player.Name}: {Spiellogik.Player.Punkte}/{maxpunkte}";
                case 5: return "  ══════════════════════════════";
                case 6:
                    if (Spielvalues.Multiplayer)
                    {
                        string maxpunkte2 = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? GameData.MaxPunkte.ToString() : "∞";
                        return $"  {Spiellogik.Player2.Name}: {Spiellogik.Player2.Punkte}/{maxpunkte2}";
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
        private static ConsoleColor BestimmeFarbe(int x, int y, char zeichen)
        {
            if (x == Spiellogik.Player.PlayerX[0] && y == Spiellogik.Player.PlayerY[0])
                return Spiellogik.Player.HeadFarbe;
            if (Spielvalues.Multiplayer && x == Spiellogik.Player2.PlayerX[0] && y == Spiellogik.Player2.PlayerY[0]) 
                return Spiellogik.Player2.HeadFarbe;
            if (zeichen == Spiellogik.Player.TailSkin)
                return Spiellogik.Player.TailFarbe;
            if (zeichen == Spiellogik.Player2.TailSkin)
                return Spiellogik.Player2.TailFarbe;
            if (zeichen == Skinvalues.MauerSkin)
                return Skinvalues.MauerFarbe;
            if (zeichen == Skinvalues.BombenSkin)
                return Skinvalues.BombenFarbe;
            if (zeichen == Skinvalues.SchluesselSkin)
                return Skinvalues.SchluesselFarbe;
            foreach (var Essen in Spiellogik.Essen)
            {
                if (x == Essen.X && y == Essen.Y)
                    return Essen.FoodFarbe;
            }
            return ConsoleColor.White;
        }

        // RenderLegende mit Farben für den Full-Mode
        private static ConsoleColor RenderLegende(int y, ConsoleColor aktuelleFarbe)
        {
            void SetFarbe(ConsoleColor farbe)
            {
                Console.SetCursorPosition(GameData.Weite, y);

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
                    Console.Write(LanguageManager.Get("legende"));
                    break;
                case 3:
                    SetFarbe(Skinvalues.RandFarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 4:
                    SetFarbe(Spiellogik.Player.HeadFarbe);
                    string maxpunkte = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? GameData.MaxPunkte.ToString() : "∞";
                    Console.Write($"  {Spiellogik.Player.Name}: {Spiellogik.Player.Punkte}/{maxpunkte}");
                    break;
                case 5:
                    SetFarbe(Skinvalues.RandFarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 6:
                    if (Spielvalues.Multiplayer)
                    {
                        SetFarbe(Spiellogik.Player2.HeadFarbe);
                        string maxpunkte2 = (Spielvalues.Gamemode != Gamemodes.Unendlich && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich) ? GameData.MaxPunkte.ToString() : "∞";
                        Console.Write($"  {Spiellogik.Player2.Name}: {Spiellogik.Player2.Punkte}/{maxpunkte2}");
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
