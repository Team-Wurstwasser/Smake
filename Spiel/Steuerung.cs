using Smake.Values;
using Smake.Spieler;

namespace Smake.Spiel
{
    public class Steuerung 
    {
        // Läuft in einem eigenen Thread: verarbeitet Tasteneingaben und speichert diese
        public static void ReadInput()
        {
            while (Spiellogik.Spiel)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        // Player 1 Steuerung (Pfeiltasten)
                        case ConsoleKey.UpArrow:
                            UpdatePlayerDirection(Spiellogik.Player, 0, -1, '^');
                            break;
                        case ConsoleKey.DownArrow:
                            UpdatePlayerDirection(Spiellogik.Player, 0, 1, 'v');
                            break;
                        case ConsoleKey.LeftArrow:
                            UpdatePlayerDirection(Spiellogik.Player, -1, 0, '<');
                            break;
                        case ConsoleKey.RightArrow:
                            UpdatePlayerDirection(Spiellogik.Player, 1, 0, '>');
                            break;

                        // Player 2 Steuerung (WASD) im Multiplayer
                        case ConsoleKey.W:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, 0, -1, '^');
                            else
                                UpdatePlayerDirection(Spiellogik.Player, 0, -1, '^');
                            break;
                        case ConsoleKey.S:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, 0, 1, 'v');
                            else
                                UpdatePlayerDirection(Spiellogik.Player, 0, 1, 'v');
                            break;
                        case ConsoleKey.A:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, -1, 0, '<');
                            else
                                UpdatePlayerDirection(Spiellogik.Player, -1, 0, '<');
                            break;
                        case ConsoleKey.D:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, 1, 0, '>');
                            else
                                UpdatePlayerDirection(Spiellogik.Player, 1, 0, '>');
                            break;

                        // Spiel beenden
                        case ConsoleKey.Escape:
                            Spiellogik.Spiel = false;
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
