using SharpDX.XInput;
using Smake.Enums;
using Smake.Game.Struct;
using Smake.Values;

namespace Smake.Game
{
    public class Steuerung
    {
        volatile bool DoReadInput = true;
        Thread? InputThread;

        // Controller-Instanzen definieren
        private readonly Controller? controller1;
        private readonly Controller? controller2;

        private const int StickDeadzone = 15000;

        public Steuerung()
        {
            // Controller initialisieren
            controller1 = new Controller(UserIndex.One);
            controller2 = new Controller(UserIndex.Two);

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

        // Läuft in einem eigenen Thread: verarbeitet Tasteneingaben und Controller-Eingaben
        void ReadInput()
        {
            while (DoReadInput)
            {
                bool inputDetected = false;

                if (Console.KeyAvailable)
                {
                    inputDetected = true;
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
                            {
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(0, -1), '^');
                            }
                            else
                            {
                                UpdatePlayerDirection(Spiellogik.Player, new Position(0, -1), '^');
                            }
                            break;
                        case ConsoleKey.S:
                            if (Spielvalues.Multiplayer)
                            {
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(0, 1), 'v');
                            }
                            else
                            {
                                UpdatePlayerDirection(Spiellogik.Player, new Position(0, 1), 'v');
                            }
                            break;
                        case ConsoleKey.A:
                            if (Spielvalues.Multiplayer)
                            {
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(-1, 0), '<');
                            }
                            else
                            {
                                UpdatePlayerDirection(Spiellogik.Player, new Position(-1, 0), '<');
                            }
                            break;
                        case ConsoleKey.D:
                            if (Spielvalues.Multiplayer)
                            {
                                UpdatePlayerDirection(Spiellogik.Player2, new Position(1, 0), '>');
                            }
                            else
                            {
                                UpdatePlayerDirection(Spiellogik.Player, new Position(1, 0), '>');
                            }
                            break;

                        // Spiel beenden
                        case ConsoleKey.Escape:
                            Spiellogik.Gameovertype = GameOverType.Exit;
                            break;
                    }
                }

                // Controller 1
                if (controller1 != null && controller1.IsConnected)
                {
                    inputDetected = true;
                    ProcessControllerInput(controller1, Spiellogik.Player);
                }

                // Controller 2
                if (controller2 != null && controller2.IsConnected)
                {
                    inputDetected = true;
                    if (Spielvalues.Multiplayer)
                    {
                        ProcessControllerInput(controller2, Spiellogik.Player2);
                    }
                    else
                    {
                        ProcessControllerInput(controller2, Spiellogik.Player);
                    }
                }

                if (!inputDetected)
                {
                    Thread.Sleep(5); // CPU schonen
                }
            }
        }

        // Verarbeitet die Buttons und den linken Analog-Stick eines Controllers für einen bestimmten Spieler
        private static void ProcessControllerInput(Controller controller, Player player)
        {
            try
            {
                State state = controller.GetState();
                Gamepad gamepad = state.Gamepad;

                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp))
                {
                    UpdatePlayerDirection(player, new Position(0, -1), '^');
                }
                else if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown))
                {
                    UpdatePlayerDirection(player, new Position(0, 1), 'v');
                }
                else if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft))
                {
                    UpdatePlayerDirection(player, new Position(-1, 0), '<');
                }
                else if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight))
                {
                    UpdatePlayerDirection(player, new Position(1, 0), '>');
                }

                short stickX = gamepad.LeftThumbX;
                short stickY = gamepad.LeftThumbY;

                // Horizontale Bewegung (X-Achse)
                if (Math.Abs(stickX) > StickDeadzone)
                {
                    if (stickX > 0)
                        UpdatePlayerDirection(player, new Position(1, 0), '>');
                    else
                        UpdatePlayerDirection(player, new Position(-1, 0), '<');
                }
                // Vertikale Bewegung (Y-Achse - Achtung: Y ist im XInput nach oben positiv, im Konsolen-Raster nach unten!)
                else if (Math.Abs(stickY) > StickDeadzone)
                {
                    if (stickY > 0)
                        UpdatePlayerDirection(player, new Position(0, -1), '^'); // Stick nach oben
                    else
                        UpdatePlayerDirection(player, new Position(0, 1), 'v');
                }

                //Button zum Beenden nutzen
                if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Start) || gamepad.Buttons.HasFlag(GamepadButtonFlags.Back) || (gamepad.Buttons.HasFlag(GamepadButtonFlags.B)))
                {
                    Spiellogik.Gameovertype = GameOverType.Exit;
                }
            }
            catch
            {
                // Falls der Controller genau in diesem Moment getrennt wird
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