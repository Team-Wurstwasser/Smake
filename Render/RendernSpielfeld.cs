using Smake.io.Values;
using Smake.io.Speicher;
using Smake.io.Spiel;

namespace Smake.io.Render
{
    public class RendernSpielfeld
    {
        public static bool Performancemode { get; set; }

        // Das Spielfeld als zweidimensionales Zeichen-Array
        public static char[,] Grid { get; set; } = new char[Spielvalues.hoehe, Spielvalues.weite];

        // Initialisiert das Spielfeld: Rahmen, leere Fläche
        public static void InitialisiereSpielfeld()
        {
            Console.Clear();

            for (int reihe = 0; reihe < Grid.GetLength(0); reihe++)

            {

                for (int symbol = 0; symbol < Grid.GetLength(1); symbol++)

                {

                    // Rand des Spielfelds mit RandSkin markieren

                    if (reihe == 0 || reihe == Grid.GetLength(0) - 1 || symbol == 0 || symbol == Grid.GetLength(1) - 1)

                    {

                        Grid[reihe, symbol] = Skinvalues.RandSkin;

                    }

                    else

                    {

                        Grid[reihe, symbol] = ' ';

                    }

                }

            }

        }

        public static void Render()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            int rows = Grid.GetLength(0);
            int cols = Grid.GetLength(1);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    char zeichen = Grid[y, x];

                    if (!Performancemode)
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
                if (!Performancemode)
                {
                    aktuelleFarbe = RenderLegende(y, aktuelleFarbe);
                }
                else
                {
                    RenderLegende(y); // Performance: nur Text ausgeben, Farbe ignoriert
                }

                Console.WriteLine();
            }

            if (!Performancemode)
            {
                Console.ResetColor();
            }
              
        }

        private static ConsoleColor BestimmeFarbe(int x, int y, char zeichen)
        {
            if (zeichen == ' ') return ConsoleColor.White;
            if (x == Spiellogik.Player.PlayerX[0] && y == Spiellogik.Player.PlayerY[0])
                return Spiellogik.Player.HeadFarbe;
            if (Spielvalues.Multiplayer && x == Spiellogik.Player2.PlayerX[0] && y == Spiellogik.Player2.PlayerY[0])
                return Spiellogik.Player2.HeadFarbe;
            if (zeichen == Spiellogik.Player.TailSkin)
                return Spiellogik.Player.TailFarbe;
            if (zeichen == Spiellogik.Player2.TailSkin)
                return Spiellogik.Player2.TailFarbe;
            if (zeichen == Skinvalues.RandSkin)
                return Skinvalues.RandFarbe;
            // Alle Futter durchgehen
            foreach (var Essen in Spiellogik.Essen)
            {
                if (x == Essen.FutterX && y == Essen.FutterY)
                    return Essen.Foodfarbe;
            }

            return ConsoleColor.White;
        }
        private static void RenderLegende(int y)
        {
            if (Performancemode)
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
                        string maxpunkte = Spielvalues.Gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                        Console.Write($"  {Spiellogik.Player.Name}: {Spiellogik.Player.Punkte}/{maxpunkte}");
                        break;
                    case 5:
                        Console.Write("  ══════════════════════════════");
                        break;
                    case 6:
                        if (Spielvalues.Multiplayer)
                        {
                            string maxpunkte2 = Spielvalues.Gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                            Console.Write($"  {Spiellogik.Player2.Name}: {Spiellogik.Player2.Punkte}/{maxpunkte2}");
                        }
                        break;
                    case 7:
                        if (Spielvalues.Multiplayer)
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
                    SetFarbe(Skinvalues.RandFarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 2:
                    SetFarbe(ConsoleColor.White);
                    Console.Write("  Punkte:");
                    break;
                case 3:
                    SetFarbe(Skinvalues.RandFarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 4:
                    SetFarbe(Spiellogik.Player.HeadFarbe);
                    string maxpunkte = Spielvalues.Gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
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
                        string maxpunkte2 = Spielvalues.Gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
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
