using Smake.Enums;
using Smake.SFX;
using Smake.Speicher;
using Smake.Game;
using Smake.Values;

namespace Smake.Menues
{
    public class MainMenu : RenderMenue
    {
        const int MinMenuIndex = 1;
        const int MaxMenuIndex = 7;
        const int XpPerLevel = 100;

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
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    SelectMenu();
                    break;
            }

            Input = 0;
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
                    if (value > MaxMenuIndex)
                    {
                        menuTracker = MinMenuIndex;
                    }
                    else if (value < MinMenuIndex)
                    {
                        menuTracker = MaxMenuIndex;
                    }
                    else
                    {
                        menuTracker = value;
                    }
                    Selected = MenuTracker;
                }
            }
        }

        public MainMenu()
        {
            Menueloop();
        }

        void Menueloop()
        {
            Sounds.Melodie(GameData.MusikDaten.Menue?.Main ?? 0);

            SpeicherSystem.Speichern_Laden(StorageAction.Save);

            // Level-Berechnung (1 Level pro 100 XP)
            Spielstatus.Level = Spielstatus.Xp / XpPerLevel + 1;

            if (RenderSpielfeld.Performancemode)
            {
                Skinvalues.FoodFarbe = GameData.Farben[0];
                Skinvalues.FoodfarbeRandom = false;
                Skinvalues.RandFarbe = GameData.Farben[0];
                Spiellogik.Player.TailFarbe = GameData.Farben[0];
                Spiellogik.Player.HeadFarbe = GameData.Farben[0];
                Spiellogik.Player2.TailFarbe = GameData.Farben[0];
                Spiellogik.Player2.HeadFarbe = GameData.Farben[0];
            }

            Title = "Menü";
            Display = LanguageManager.GetArray("menu.items"); ;
            MenuTracker = 1;
            InitialRender();
            StartInputstream();
            while (DoReadInput)
            {
                ProcessInput();
                Thread.Sleep(5); // kleine Pause, CPU schonen
            }
        }

        void SelectMenu()
        {
            switch (MenuTracker)
            {
                case 1:
                    Program.CurrentView = ViewType.Game;
                    break;
                case 2:
                    Program.CurrentView = ViewType.Settings;
                    break;
                case 3:
                    Program.CurrentView = ViewType.Shop;
                    break;
                case 4:
                    Program.CurrentView = ViewType.SkinColors;
                    break;
                case 5:
                    Program.CurrentView = ViewType.Statistics;
                    break;
                case 6:
                    Program.CurrentView = ViewType.Instructions;
                    break;
                case 7:
                    Program.CurrentView = ViewType.Exit;
                    break;
            }
            StopInputstream();
        }
    }
}
