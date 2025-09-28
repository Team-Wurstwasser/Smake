using Smake.io.Values;
using Smake.io.Spieler;

namespace Smake.io.Spiel
{
    public class Steuerung 
    {
        // Läuft in einem eigenen Thread: verarbeitet Tasteneingaben und speichert diese
        public static void ReadInput()
        {
            while (Spiellogik.spiel)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        // Player 1 Steuerung (Pfeiltasten)
                        case ConsoleKey.UpArrow:
                            UpdatePlayerDirection(Spiellogik.player, 0, -1, '^');
                            break;
                        case ConsoleKey.DownArrow:
                            UpdatePlayerDirection(Spiellogik.player, 0, 1, 'v');
                            break;
                        case ConsoleKey.LeftArrow:
                            UpdatePlayerDirection(Spiellogik.player, -1, 0, '<');
                            break;
                        case ConsoleKey.RightArrow:
                            UpdatePlayerDirection(Spiellogik.player, 1, 0, '>');
                            break;

                        // Player 2 Steuerung (WASD) im Multiplayer
                        case ConsoleKey.W:
                            if (Spielvalues.multiplayer)
                                UpdatePlayerDirection(Spiellogik.player2, 0, -1, '^');
                            else
                                UpdatePlayerDirection(Spiellogik.player, 0, -1, '^');
                            break;
                        case ConsoleKey.S:
                            if (Spielvalues.multiplayer)
                                UpdatePlayerDirection(Spiellogik.player2, 0, 1, 'v');
                            else
                                UpdatePlayerDirection(Spiellogik.player, 0, 1, 'v');
                            break;
                        case ConsoleKey.A:
                            if (Spielvalues.multiplayer)
                                UpdatePlayerDirection(Spiellogik.player2, -1, 0, '<');
                            else
                                UpdatePlayerDirection(Spiellogik.player, -1, 0, '<');
                            break;
                        case ConsoleKey.D:
                            if (Spielvalues.multiplayer)
                                UpdatePlayerDirection(Spiellogik.player2, 1, 0, '>');
                            else
                                UpdatePlayerDirection(Spiellogik.player, 1, 0, '>');
                            break;

                        // Spiel beenden
                        case ConsoleKey.Escape:
                            Spiellogik.spiel = false;
                            break;
                    }
                }
                else
                {
                    Thread.Sleep(5); // CPU schonen
                }
            }
        }

        // Hilfsmethode zum Setzen der neuen Richtung eines Spielers
        private static void UpdatePlayerDirection(Player p, int newX, int newY, char head)
        {
            // Verhindert Rückwärtsbewegung und doppelte Änderungen pro Tick
            if (p.Aenderung && (p.InputX != -newX || p.InputY != -newY))
            {
                p.InputX = newX;
                p.InputY = newY;
                p.HeadSkin = head;
                p.Aenderung = false;
            }
        }

    }

}
