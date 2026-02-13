using Smake.Speicher;
using Smake.Game;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Menues
{
    public class SkinColors : RenderMenue
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
            Sounds.Melodie(ConfigSystem.Sounds.Musik.Menue.SkinColors);
            Title = "SkinColors";
            Display = LanguageSystem.GetArray("skins.items");
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
                case 1: Skinvalues.TailSkin[0] = WechselSkin(Skinvalues.TailSkin[0], ConfigSystem.Skins.Tail, Menüsvalues.FreigeschaltetTail, Skinvalues.TailSkin[1]); break;
                case 2: Skinvalues.TailSkin[1] = WechselSkin(Skinvalues.TailSkin[1], ConfigSystem.Skins.Tail, Menüsvalues.FreigeschaltetTail, Skinvalues.TailSkin[0]); break;
                case 3: Skinvalues.FoodSkin = WechselSkin(Skinvalues.FoodSkin, ConfigSystem.Skins.Food, Menüsvalues.FreigeschaltetFood); break;
                case 4: Skinvalues.RandSkin = WechselSkin(Skinvalues.RandSkin, ConfigSystem.Skins.Rand, Menüsvalues.FreigeschaltetRand); break;

                case 5: Skinvalues.HeadFarbe[0] = WechselFarbe(Skinvalues.HeadFarbe[0]); break;
                case 6: Skinvalues.HeadFarbe[1] = WechselFarbe(Skinvalues.HeadFarbe[1]); break;
                case 7: Skinvalues.TailFarbe[0] = WechselFarbe(Skinvalues.TailFarbe[0]); break;
                case 8: Skinvalues.TailFarbe[1] = WechselFarbe(Skinvalues.TailFarbe[1]); break;

                case 9: Skinvalues.FoodFarbe = WechselFarbe(Skinvalues.FoodFarbe, true); break;
                case 10: Skinvalues.RandFarbe = WechselFarbe(Skinvalues.RandFarbe); break;

                case 11: StopInputstream(); break;
            }
        }

        static object?[] BuildMenu()
        {
            return [
                Skinvalues.TailSkin[0],
                Skinvalues.TailSkin[1],
                Skinvalues.FoodSkin,
                Skinvalues.RandSkin,
                Skinvalues.HeadFarbe[0],
                Skinvalues.HeadFarbe[1],
                Skinvalues.TailFarbe[0],
                Skinvalues.TailFarbe[1],
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
            if (ConfigSystem.Skins.Farben.Length == 0) return aktuelleFarbe;

            if (isFood && Skinvalues.FoodfarbeRandom)
            {
                Skinvalues.FoodfarbeRandom = false;
                return ConfigSystem.Skins.Farben[0];
            }

            int idx = Array.IndexOf(ConfigSystem.Skins.Farben, aktuelleFarbe);

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
                idx = (idx + 1) % ConfigSystem.Skins.Farben.Length;
            } while (!Menüsvalues.FreigeschaltetFarben[idx]);

            return ConfigSystem.Skins.Farben[idx];
        }
    }
}
