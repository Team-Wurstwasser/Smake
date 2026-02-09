using Smake.Render;
using Smake.Speicher;
using Smake.Game;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Menues
{
    public class SkinColors : RendernMenue
    {
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

        void ProcessInput()
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

        int menuTracker;
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

        public SkinColors()
        {
            Menueloop();
        }

        void Menueloop()
        {
            Sounds.Melodie(GameData.MusikDaten.Menue?.SkinFarben ?? 0);
            Title = "Skin_Farben";
            Display = LanguageManager.GetArray("skins.items");
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
            Program.CurrentView = ViewType.MainMenu;
        }

        void SelectMenu()
        {
            switch (MenuTracker)
            {
                case 1: Spiellogik.Player.TailSkin = WechselSkin(Spiellogik.Player.TailSkin, GameData.TailSkins, Menüsvalues.FreigeschaltetTail, Spiellogik.Player2.TailSkin); break;
                case 2: Spiellogik.Player2.TailSkin = WechselSkin(Spiellogik.Player2.TailSkin, GameData.TailSkins, Menüsvalues.FreigeschaltetTail, Spiellogik.Player.TailSkin); break;
                case 3: Skinvalues.FoodSkin = WechselSkin(Skinvalues.FoodSkin, GameData.FoodSkins, Menüsvalues.FreigeschaltetFood); break;
                case 4: Skinvalues.RandSkin = WechselSkin(Skinvalues.RandSkin, GameData.RandSkins, Menüsvalues.FreigeschaltetRand); break;

                case 5: Spiellogik.Player.HeadFarbe = WechselFarbe(Spiellogik.Player.HeadFarbe); break;
                case 6: Spiellogik.Player2.HeadFarbe = WechselFarbe(Spiellogik.Player2.HeadFarbe); break;
                case 7: Spiellogik.Player.TailFarbe = WechselFarbe(Spiellogik.Player.TailFarbe); break;
                case 8: Spiellogik.Player2.TailFarbe = WechselFarbe(Spiellogik.Player2.TailFarbe); break;

                case 9: Skinvalues.FoodFarbe = WechselFarbe(Skinvalues.FoodFarbe, true); break;
                case 10: Skinvalues.RandFarbe = WechselFarbe(Skinvalues.RandFarbe); break;

                case 11: StopInputstream(); break;
            }
        }

        static object?[] BuildMenu()
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
        static char WechselSkin(char aktuellesSkin, char[] skins, bool[] freigeschaltet, char? verboteneSkin = null)
        {
            if (skins.Length == 0) return aktuellesSkin;

            int idx = Array.IndexOf(skins, aktuellesSkin);

            do
            {
                idx = (idx + 1) % skins.Length;
            } while ((!freigeschaltet[idx] || verboteneSkin.HasValue && skins[idx] == verboteneSkin.Value));

            return skins[idx];
        }

        // Helper für Farben
        static ConsoleColor WechselFarbe(ConsoleColor aktuelleFarbe, bool isFood = false)
        {
            if (GameData.Farben.Length == 0) return aktuelleFarbe;

            if (isFood && Skinvalues.FoodfarbeRandom)
            {
                Skinvalues.FoodfarbeRandom = false;
                return GameData.Farben[0];
            }

            int idx = Array.IndexOf(GameData.Farben, aktuelleFarbe);

            int lastUnlockedIndex = -1;
            for (int i = 0; i < Menüsvalues.FreigeschaltetFarben.Length; i++)
            {
                if (Menüsvalues.FreigeschaltetFarben[i]) lastUnlockedIndex = i;
            }

            if (isFood && idx == lastUnlockedIndex)
            {
                Skinvalues.FoodfarbeRandom = true;
                return aktuelleFarbe;
            }

            do
            {
                idx = (idx + 1) % GameData.Farben.Length;
            } while (!Menüsvalues.FreigeschaltetFarben[idx]);

            return GameData.Farben[idx];
        }
    }
}
