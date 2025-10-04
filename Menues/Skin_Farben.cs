using Smake.Values;
using Smake.Render;
using Smake.Speicher;
using Smake.Spiel;

namespace Smake.Menues
{
    public class Skin_Farben : RendernMenue
    {
        
        readonly string[] skin_farben = [
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
        ];

        readonly bool[] Color =
        [
            false,
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            true,
            true,
            false,
        ];

        public void ProcessInput()
        {
            switch (Input)
            {
                case ConsoleKey.UpArrow:
                    MenuTracker--;
                    break;
                case ConsoleKey.DownArrow:
                    MenuTracker++;
                    break;
                case ConsoleKey.Escape:
                    StopInputstream();
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    SelectMenu();
                    break;
            }

            Input = 0;
            GameValue = BuildMenu();
            Render();

        }

        private int menuTracker;
        public int MenuTracker
        {
            get { return menuTracker; }
            set
            {
                if (value != menuTracker) // loop index
                {
                    if (value > 11)
                    {
                        menuTracker = 1;
                    }
                    else if (value < 1)
                    {
                        menuTracker = 11;
                    }
                    else
                    {
                        menuTracker = value;
                    }
                    Selected = MenuTracker;
                }
            }
        }

        public Skin_Farben()
        {
            Menueloop();
        }

        private void Menueloop()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue.Main;

            Title = "Skin_Farben";
            Display = skin_farben;
            IsColor = Color;
            GameValue = BuildMenu();
            MenuTracker = 1;
            InitialRender();
            StartInputstream();
            while (DoReadInput)
            {
                ProcessInput();
                Thread.Sleep(5); // kleine Pause, CPU schonen
            }
            Program.CurrentView = 7;
        }

        private void SelectMenu()
        {
            switch (MenuTracker)
            {
                case 1: WechselSkin(ref Spiellogik.Player.TailSkin, GameData.TailSkins, Menüsvalues.FreigeschaltetTail, Spiellogik.Player2.TailSkin); break;
                case 2: WechselSkin(ref Spiellogik.Player2.TailSkin, GameData.TailSkins, Menüsvalues.FreigeschaltetTail, Spiellogik.Player.TailSkin); break;
                case 3: WechselSkin(ref Skinvalues.FoodSkin, GameData.FoodSkins, Menüsvalues.FreigeschaltetFood); break;
                case 4: WechselSkin(ref Skinvalues.RandSkin, GameData.RandSkins, Menüsvalues.FreigeschaltetRand); break;
                case 5: WechselFarbe(ref Spiellogik.Player.HeadFarbe); break;
                case 6: WechselFarbe(ref Spiellogik.Player2.HeadFarbe); break;
                case 7: WechselFarbe(ref Spiellogik.Player.TailFarbe); break;
                case 8: WechselFarbe(ref Spiellogik.Player2.TailFarbe); break;
                case 9: WechselFarbe(ref Skinvalues.FoodFarbe, true); break;
                case 10: WechselFarbe(ref Skinvalues.RandFarbe); break;
                case 11:
                    StopInputstream();
                    break;
            }
        }

        private static object?[] BuildMenu()
        {
            return [
                Spiellogik.Player.TailSkin,
                Spiellogik.Player2.TailSkin,
                Skinvalues.FoodSkin,
                Skinvalues.RandSkin,
                Spiellogik.Player.HeadFarbe,
                Spiellogik.Player2.HeadFarbe,
                Spiellogik.Player.TailFarbe,
                Spiellogik.Player2.TailFarbe,
                Skinvalues.FoodfarbeRandom ? "Random" : (object?)Skinvalues.FoodFarbe,
                Skinvalues.RandFarbe,
                null
            ];
        }



        // Helper für Tail/Food/Rand
        static void WechselSkin(ref char aktuellesSkin, char[] skins, bool[] freigeschaltet, char? verboteneSkin = null)
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
        static void WechselFarbe(ref ConsoleColor aktuelleFarbe, bool isFood = false)
        {
            if (GameData.Farben.Length == 0) return;

            // Nächste freigeschaltete Farbe suchen
            int idx = Array.IndexOf(GameData.Farben, aktuelleFarbe);
            int start = idx;
            do
            {
                idx = (idx + 1) % GameData.Farben.Length;
            } while (!Menüsvalues.FreigeschaltetFarben[idx] && idx != start);

            aktuelleFarbe = GameData.Farben[idx];

            if (isFood)
            {
                int lastIndex = -1;

                for (int i = 0; i < Menüsvalues.FreigeschaltetFarben.Length; i++)
                {
                    if (Menüsvalues.FreigeschaltetFarben[i])
                    {
                        lastIndex = i; // letzte Position merken
                    }
                }

                // Nur für foodfarbe: Random aktivieren, wenn letzte freigeschaltete Farbe erreicht
                if (!Skinvalues.FoodfarbeRandom)
                {
                    if (lastIndex == idx)
                    {
                        Skinvalues.FoodfarbeRandom = true;
                    }
                }
                else
                {
                    Skinvalues.FoodfarbeRandom = false;
                }
            }
        }
    }
}
