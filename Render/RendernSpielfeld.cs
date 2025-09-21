using System.Text;
using Smake.io.Spiel;
using Smake.io.Speicher;

namespace Smake.io.Render
{
    public class RendernSpielfeld
    {
        public static bool performancemode;

        public static void Render()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            int rows = Spiellogik.grid.GetLength(0);
            int cols = Spiellogik.grid.GetLength(1);

            for (int y = 0; y < rows; y++)
            {
                StringBuilder zeile = new();

                for (int x = 0; x < cols; x++)
                {
                    char zeichen = Spiellogik.grid[y, x];
                    ConsoleColor neueFarbe = BestimmeFarbe(x, y, zeichen);

                    if (neueFarbe != aktuelleFarbe)
                    {
                        Console.ForegroundColor = neueFarbe;
                        aktuelleFarbe = neueFarbe;
                    }

                    zeile.Append(zeichen);
                }

                Console.Write(zeile.ToString());
                aktuelleFarbe = RenderLegende(y, aktuelleFarbe);
                Console.WriteLine();
            }

            Console.ResetColor();
        }

        private static ConsoleColor BestimmeFarbe(int x, int y, char zeichen)
        {
            if (performancemode) return ConsoleColor.White;

            if (x == Spiellogik.player.PlayerX[0] && y == Spiellogik.player.PlayerY[0])
                return Spiellogik.player.Headfarbe;
            if (Spiellogik.multiplayer && x == Spiellogik.player2.PlayerX[0] && y == Spiellogik.player2.PlayerY[0])
                return Spiellogik.player2.Headfarbe;
            if (zeichen == Spiellogik.player.Skin)
                return Spiellogik.player.Farbe;
            if (zeichen == Spiellogik.player2.Skin)
                return Spiellogik.player2.Farbe;
            if (zeichen == Spiellogik.food)
                return Spiellogik.foodfarbe;
            if (zeichen == Spiellogik.rand)
                return Spiellogik.randfarbe;

            return ConsoleColor.White;
        }

        private static ConsoleColor RenderLegende(int y, ConsoleColor aktuelleFarbe)
        {
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
                    SetFarbe(Spiellogik.randfarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 2:
                    SetFarbe(ConsoleColor.White);
                    Console.Write("  Punkte:");
                    SetFarbe(Spiellogik.randfarbe);
                    break;
                case 3:
                    SetFarbe(Spiellogik.randfarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 4:
                    SetFarbe(Spiellogik.player.Headfarbe);
                    string maxpunkte = Spiellogik.gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                    Console.Write($"  {Spiellogik.player.Name}: {Spiellogik.player.Punkte}/{maxpunkte}");
                    SetFarbe(Spiellogik.randfarbe);
                    break;
                case 5:
                    SetFarbe(Spiellogik.randfarbe);
                    Console.Write("  ══════════════════════════════");
                    break;
                case 6:
                    if (Spiellogik.multiplayer)
                    {
                        SetFarbe(Spiellogik.player2.Headfarbe);
                        string maxpunkte2 = Spiellogik.gamemode != "Unendlich" ? GameData.MaxPunkte.ToString() : "∞";
                        Console.Write($"  {Spiellogik.player2.Name}: {Spiellogik.player2.Punkte}/{maxpunkte2}");
                        SetFarbe(Spiellogik.randfarbe);
                    }
                    break;
                case 7:
                    if (Spiellogik.multiplayer)
                    {
                        SetFarbe(Spiellogik.randfarbe);
                        Console.Write("  ══════════════════════════════");
                    }
                    break;
            }

            return aktuelleFarbe;
        }
    }
}
