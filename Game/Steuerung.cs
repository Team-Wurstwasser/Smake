using Smake.Enums;
using Smake.Game.Spieler;
using Smake.Values;

namespace Smake.Game
{
    public class Steuerung
    {
        volatile bool DoReadInput = true;
        Thread? InputThread;

        public Steuerung()
        {
            StartInputStream();
        }

        private void StartInputStream()
        {
            InputThread = new(ReadInput);
            InputThread.Start();
        }

        public void StopInputStream()
        {
            DoReadInput = false;
            InputThread?.Join();
        }

        // Läuft in einem eigenen Thread: verarbeitet Tasteneingaben und speichert diese
        void ReadInput()
        {
            while (DoReadInput)
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
                            Spiellogik.Gameovertype = GameOverType.Exit;
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
        static void UpdatePlayerDirection(Player p, int newX, int newY, char head)
        {
            // Gespiegelte Steuerung bei Chaos-Steuerung
            if (Spielvalues.Gamemode == Gamemodes.ChaosSteuerung)
            {
                newX = -newX;
                newY = -newY;

                // Kopf-Symbol anpassen
                if (newX == 1) head = '>';
                else if (newX == -1) head = '<';
                else if (newY == 1) head = 'v';
                else if (newY == -1) head = '^';
            }

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
