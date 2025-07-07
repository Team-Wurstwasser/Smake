using System.Numerics;
using Smake.io.Spiel;

namespace Smake.io.Render
{
    public class RendernSpielfeld 
    {
        public static bool performancemode;

        // Zeichnet das gesamte Spielfeld auf der Konsole
        public static void Render()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            for (int y = 0; y < Spiellogik.grid.GetLength(0); y++)
            {
                for (int x = 0; x < Spiellogik.grid.GetLength(1); x++)
                {
                    char zeichen = Spiellogik.grid[y, x];
                    ConsoleColor neueFarbe = ConsoleColor.White;

                    if (!performancemode)
                    {
                        // Farbwahl je nach Position oder Zeichen
                        if (x == Spiellogik.player.PlayerX[0] && y == Spiellogik.player.PlayerY[0])
                            neueFarbe = Spiellogik.player.Headfarbe;
                        else if (x == Spiellogik.player2.PlayerX[0] && y == Spiellogik.player2.PlayerY[0] && Spiellogik.multiplayer)
                            neueFarbe = Spiellogik.player2.Headfarbe;
                        else if (zeichen == Spiellogik.player.Skin)
                            neueFarbe = Spiellogik.player.Farbe;
                        else if (zeichen == Spiellogik.player2.Skin)
                            neueFarbe = Spiellogik.player2.Farbe;
                        else if (zeichen == Spiellogik.food)
                            neueFarbe = Spiellogik.foodfarbe;
                        else if (zeichen == Spiellogik.rand)
                            neueFarbe = Spiellogik.randfarbe;
                    }

                    // Nur Farbe wechseln, wenn nötig
                    if (neueFarbe != aktuelleFarbe)
                    {
                        Console.ForegroundColor = neueFarbe;
                        aktuelleFarbe = neueFarbe;
                    }

                    Console.Write(zeichen);
                }

                // → Legende auf bestimmten Zeilen ausgeben
                Console.ForegroundColor = Spiellogik.randfarbe;
                if (y == 1)
                {
                    Console.Write("  ══════════════════════════════");
                }
                else if (y == 2)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("  Punkte:");
                    Console.ForegroundColor = Spiellogik.randfarbe;
                }
                else if (y == 3)
                {
                    Console.Write("  ══════════════════════════════");
                }
                else if (y == 4)
                {
                    Console.ForegroundColor = Spiellogik.player.Headfarbe;
                    if (Spiellogik.gamemode != "Unendlich")
                    {
                        Console.Write($"  {Spiellogik.player.Name}: {Spiellogik.player.Punkte}/{Spiellogik.maxpunkte}");
                    }
                    else
                    {
                        Console.Write($"  {Spiellogik.player.Name}: {Spiellogik.player.Punkte}/∞");
                    }
                    Console.ForegroundColor = Spiellogik.randfarbe;
                }
                else if (y == 5)
                {
                    Console.Write("  ══════════════════════════════");
                }
                else if (y == 6)
                {
                    if (Spiellogik.multiplayer)
                    {
                        Console.ForegroundColor = Spiellogik.player2.Headfarbe;
                        if (Spiellogik.gamemode != "Unendlich")
                        {
                            Console.Write($"  {Spiellogik.player2.Name}: {Spiellogik.player2.Punkte}/{Spiellogik.maxpunkte}");
                        }
                        else
                        {
                            Console.Write($"  {Spiellogik.player2.Name}: {Spiellogik.player2.Punkte}/∞");
                        }
                        Console.ForegroundColor = Spiellogik.randfarbe;
                    }
                }
                else if (y == 7)
                {
                    if (Spiellogik.multiplayer)
                        Console.Write("  ══════════════════════════════");
                }

                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}
