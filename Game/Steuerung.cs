using Smake.Enums;
using Smake.Game.Struct;
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
                            UpdatePlayerDirection(Spiellogik.Player, new Position(0, -1), '^');
                            break;
                        case ConsoleKey.DownArrow:
                            UpdatePlayerDirection(Spiellogik.Player, new Position(0, 1), 'v');
                            break;
                        case ConsoleKey.LeftArrow:
                            UpdatePlayerDirection(Spiellogik.Player, new Position(-1, 0), '<');
                            break;
                        case ConsoleKey.RightArrow:
                            UpdatePlayerDirection(Spiellogik.Player, new Position(1, 0), '>');
                            break;

                        // Player 2 Steuerung (WASD)
                        case ConsoleKey.W:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(0, -1), '^');
                            break;
                        case ConsoleKey.S:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(0, 1), 'v');
                            break;
                        case ConsoleKey.A:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(-1, 0), '<');
                            break;
                        case ConsoleKey.D:
                            if (Spielvalues.Multiplayer)
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(1, 0), '>');
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
        static void UpdatePlayerDirection(Player p, Position neueRichtung, char head)
        {
            int deltaX = neueRichtung.X;
            int deltaY = neueRichtung.Y;

            // Gespiegelte Steuerung bei Chaos-Steuerung
            if (Spielvalues.Gamemode == Gamemodes.ChaosSteuerung)
            {
                deltaX = -deltaX;
                deltaY = -deltaY;

                // Kopf-Symbol anpassen
                if (deltaX == 1) head = '>';
                else if (deltaX == -1) head = '<';
                else if (deltaY == 1) head = 'v';
                else if (deltaY == -1) head = '^';
            }

            // Verhindert Rückwärtsbewegung und doppelte Änderungen pro Tick
            if (p.Aenderung && (p.Richtung.X != -deltaX || p.Richtung.Y != -deltaY))
            {
                p.Richtung = new Position(deltaX, deltaY);
                p.HeadSkin = head;
                p.Aenderung = false;
            }
        }
    }
}