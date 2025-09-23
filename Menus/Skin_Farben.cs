using Smake.io.Render;
using Smake.io.Speicher;
using Smake.io.Spiel;
using System.Drawing;

namespace Smake.io.Menus
{
    public class Skin_Farben : Screen
    {
        
        string[] skin_farben = {
                ($"Player 1 Tailskin ändern    [Aktuell: "),
                ($"Player 2 Tailskin ändern    [Aktuell: "),
                ($"Foodskin ändern             [Aktuell: "),
                ($"Randskin ändern             [Aktuell: "),
                ($"Player 1 Farbe ändern       [Aktuell: "),
                ($"Player 2 Farbe ändern       [Aktuell: "),
                ($"Player 1 Tailfarbe ändern   [Aktuell: "),
                ($"Player 2 Tailfarbe ändern   [Aktuell: "),
                ($"Foodfarbe ändern            [Aktuell: "),
                ($"Randfarbe ändern            [Aktuell: "),
                ("Zurück zum Hauptmenü")
        };

        bool[] Color =
        {
            false,
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            true,
            false,
        }; 

        private ConsoleKey input;
        public override ConsoleKey Input
        {
            get { return input; }
            set
            {
                input = value;

                switch (Input)
                {
                    case ConsoleKey.UpArrow:
                        MenuTracker--;
                        break;
                    case ConsoleKey.DownArrow:
                        MenuTracker++;
                        break;
                    case ConsoleKey.Escape:
                        Menu menu = new();
                        Thread.CurrentThread.Join();
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        SelectMenu();
                        break;
                    default:
                        break;
                }
                GameValue = BuildMenu();
                Render();
            }
        }

        private int menuTracker;
        public int MenuTracker
        {
            get { return menuTracker; }
            set
            {
                if (value != menuTracker) // loop index
                {
                    if (value > 10)
                    {
                        menuTracker = 1;
                    }
                    else if (value < 1)
                    {
                        menuTracker = 10;
                    }
                    else
                    {
                        menuTracker = value;
                    }
                    selected = MenuTracker;
                }
            }
        }

        public Skin_Farben()
        {
            title = "Skin_Farben";
            Display = skin_farben;
            IsColor = Color;
            GameValue = BuildMenu();
            MenuTracker = 1;
            InitialRender();
            StartInputstream();
        }

        private void SelectMenu()
        {
            switch (MenuTracker)
            {
                case 1: WechselSkin(ref Spiellogik.player.Skin, GameData.TailSkins, Menüsvalues.freigeschaltetTail, Spiellogik.player2.Skin); break;
                case 2: WechselSkin(ref Spiellogik.player2.Skin, GameData.TailSkins, Menüsvalues.freigeschaltetTail, Spiellogik.player.Skin); break;
                case 3: WechselSkin(ref Spiellogik.food, GameData.FoodSkins, Menüsvalues.freigeschaltetFood); break;
                case 4: WechselSkin(ref Spiellogik.rand, GameData.RandSkins, Menüsvalues.freigeschaltetRand); break;
                case 5: WechselFarbe(ref Spiellogik.player.Headfarbe); break;
                case 6: WechselFarbe(ref Spiellogik.player2.Headfarbe); break;
                case 7: WechselFarbe(ref Spiellogik.player.Farbe); break;
                case 8: WechselFarbe(ref Spiellogik.player2.Farbe); break;
                case 9: WechselFarbe(ref Spiellogik.foodfarbe, true); break;
                case 10: WechselFarbe(ref Spiellogik.randfarbe); break;
                case 11: 
                    Menu menu = new();
                    Thread.CurrentThread.Join();
                    break;
                default:
                    break;
            }
        }

        private object[] BuildMenu()
        {
            return new object[]
            {
                Spiellogik.player.Skin,
                Spiellogik.player2.Skin,
                Spiellogik.food,
                Spiellogik.rand,
                Spiellogik.player.Headfarbe,
                Spiellogik.player2.Headfarbe,
                Spiellogik.player.Farbe,
                Spiellogik.player2.Farbe,
                Spiellogik.foodfarbeRandom ? "Random" : (object)Spiellogik.foodfarbe,
                Spiellogik.randfarbe,
                null
            };
        }

        // Helper für Tail/Food/Rand
        void WechselSkin(ref char aktuellesSkin, char[] skins, bool[] freigeschaltet, char? verboteneSkin = null)
        {
            if (skins.Length == 0) return;
            int idx = Array.IndexOf(skins, aktuellesSkin);
            int start = idx;
            do
            {
                idx = (idx + 1) % skins.Length;
            } while ((!freigeschaltet[idx] || verboteneSkin.HasValue && skins[idx] == verboteneSkin.Value) && idx != start);
            aktuellesSkin = skins[idx];
        }

        // Helper für Farben
        void WechselFarbe(ref ConsoleColor aktuelleFarbe, bool isFood = false)
        {
            if (GameData.Farben.Length == 0) return;

            // Nächste freigeschaltete Farbe suchen
            int idx = Array.IndexOf(GameData.Farben, aktuelleFarbe);
            int start = idx;
            do
            {
                idx = (idx + 1) % GameData.Farben.Length;
            } while (!Menüsvalues.freigeschaltetFarben[idx] && idx != start);

            aktuelleFarbe = GameData.Farben[idx];

            if (isFood)
            {
                int lastIndex = -1;

                for (int i = 0; i < Menüsvalues.freigeschaltetFarben.Length; i++)
                {
                    if (Menüsvalues.freigeschaltetFarben[i])
                    {
                        lastIndex = i; // letzte Position merken
                    }
                }

                // Nur für foodfarbe: Random aktivieren, wenn letzte freigeschaltete Farbe erreicht
                if (!Spiellogik.foodfarbeRandom)
                {
                    if (lastIndex == idx)
                    {
                        Spiellogik.foodfarbeRandom = true;
                    }
                }
                else
                {
                    Spiellogik.foodfarbeRandom = false;
                }
            }
        }
    }
}
