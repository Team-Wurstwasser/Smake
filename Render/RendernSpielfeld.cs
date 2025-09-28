using Smake.io.Values;
using Smake.io.Speicher;
using Smake.io.Spiel;

namespace Smake.io.Render
{
    public class RendernSpielfeld
    {
        public static bool performancemode;

        // Das Spielfeld als zweidimensionales Zeichen-Array
        public static char[,] grid = new char[Spielvalues.hoehe, Spielvalues.weite];

        // Initialisiert das Spielfeld: Rahmen, leere Fläche
        public static void InitialisiereSpielfeld()
        {
            Console.Clear();

            for (int reihe = 0; reihe < grid.GetLength(0); reihe++)

            {

                for (int symbol = 0; symbol < grid.GetLength(1); symbol++)

                {

                    // Rand des Spielfelds mit RandSkin markieren

                    if (reihe == 0 || reihe == grid.GetLength(0) - 1 || symbol == 0 || symbol == grid.GetLength(1) - 1)

                    {

                        grid[reihe, symbol] = Skinvalues.rand;

                    }

                    else

                    {

                        grid[reihe, symbol] = ' ';

                    }

                }

            }

        }

        public static void Render()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    char zeichen = grid[y, x];

                    if (!performancemode)
                    {
                        ConsoleColor neueFarbe = BestimmeFarbe(x, y, zeichen);
                        if (neueFarbe != aktuelleFarbe)
                        {
                            Console.ForegroundColor = neueFarbe;
                            aktuelleFarbe = neueFarbe;
                        }
                    }

                    Console.Write(zeichen);
                }

                // Legende am Ende der Zeile hinzufügen
                if (!performancemode)
                {
                    aktuelleFarbe = RenderLegende(y, aktuelleFarbe);
                }
                else
                {
                    RenderLegende(y); // Performance: nur Text ausgeben, Farbe ignoriert
                }

                Console.WriteLine();
            }

            if (!performancemode)
            {
                Console.ResetColor();
            }
              
        }

        private static ConsoleColor BestimmeFarbe(int x, int y, char zeichen)
        {
            if (zeichen == ' ') return ConsoleColor.White;
            if (x == Spiellogik.player.PlayerX[0] && y == Spiellogik.player.PlayerY[0])
                return Spiellogik.player.HeadFarbe;
            if (Spielvalues.multiplayer && x == Spiellogik.player2.PlayerX[0] && y == Spiellogik.player2.PlayerY[0])
                return Spiellogik.player2.HeadFarbe;
            if (zeichen == Spiellogik.player.TailSkin)
                return Spiellogik.player.TailFarbe;
            if (zeichen == Spiellogik.player2.TailSkin)
                return Spiellogik.player2.TailFarbe;
            if (zeichen == Skinvalues.rand)
                return Skinvalues.randfarbe;
            // Alle Futter durchgehen
            foreach (var f in Spiellogik.Essen)
            {
                if (x == f.FutterX && y == f.FutterY && zeichen == f.Food)
                    return f.Foodfarbe;
            }

            return ConsoleColor.White;
        }
        private static void RenderLegende(int y)
        {
            if (performancemode)
            {
                // Nur Text ausgeben, keine Farbwechsel
                switch (y)
                {
                    case 1:
                        Console.Write("  ══════════════════════════════");
                        break;
                    case 2:
                        Console.Write("  Punkte:");
                        break;
                    case 3:
                        Console.Write("  ══════════════════════════════");
                        break;
                    case 4:
                        string maxpunkte = Spielvalues.gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                        Console.Write($"  {Spiellogik.player.Name}: {Spiellogik.player.Punkte}/{maxpunkte}");
                        break;
                    case 5:
                        Console.Write("  ══════════════════════════════");
                        break;
                    case 6:
                        if (Spielvalues.multiplayer)
                        {
                            string maxpunkte2 = Spielvalues.gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                            Console.Write($"  {Spiellogik.player2.Name}: {Spiellogik.player2.Punkte}/{maxpunkte2}");
                        }
                        break;
                    case 7:
                        if (Spielvalues.multiplayer)
                        {
                            Console.Write("  ══════════════════════════════");
                        }
                        break;
                }
            }
        }

        private static ConsoleColor RenderLegende(int y, ConsoleColor aktuelleFarbe)
        {
            // Normaler Modus mit Farben
            void SetFarbe(ConsoleColor farbe)
            {
                if (farbe != aktuelleFarbe)
                {
                    Console.ForegroundColor = farbe;
                    aktuelleFarbe = farbe;
                }
            }

            switch (y)
            {
                case 1:
                    SetFarbe(Skinvalues.randfarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 2:
                    SetFarbe(ConsoleColor.White);
                    Console.Write("  Punkte:");
                    break;
                case 3:
                    SetFarbe(Skinvalues.randfarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 4:
                    SetFarbe(Spiellogik.player.HeadFarbe);
                    string maxpunkte = Spielvalues.gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                    Console.Write($"  {Spiellogik.player.Name}: {Spiellogik.player.Punkte}/{maxpunkte}");
                    break;
                case 5:
                    SetFarbe(Skinvalues.randfarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 6:
                    if (Spielvalues.multiplayer)
                    {
                        SetFarbe(Spiellogik.player2.HeadFarbe);
                        string maxpunkte2 = Spielvalues.gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                        Console.Write($"  {Spiellogik.player2.Name}: {Spiellogik.player2.Punkte}/{maxpunkte2}");
                    }
                    break;
                case 7:
                    if (Spielvalues.multiplayer)
                    {
                        SetFarbe(Skinvalues.randfarbe);
                        Console.Write("  ══════════════════════════════");
                    }
                    break;
            }

            return aktuelleFarbe;
        }
    }
}
