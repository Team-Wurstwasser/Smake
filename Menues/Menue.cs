using Smake.Helper;
using Smake.Render;
using Smake.Speicher;
using Smake.Spiel;
using Smake.Values;

namespace Smake.Menues
{
    public class Menue : RendernMenue
    {
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

        private int menuTracker;
        public int MenuTracker
        {
            get { return menuTracker; }
            set
            {
                if (value != menuTracker) // loop index
                {
                    if (value > 7)
                    {
                        menuTracker = 1;
                    }
                    else if (value < 1)
                    {
                        menuTracker = 7;
                    }
                    else
                    {
                        menuTracker = value;
                    }
                    Selected = MenuTracker;
                }
            }
        }

        public Menue()
        {
            Menueloop();
        }

        private void Menueloop()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue?.Main ?? 0;

            SpeicherSystem.Speichern_Laden("Speichern");

            // Level-Berechnung (1 Level pro 100 XP)
            Spielstatus.Level = Spielstatus.Xp / 100 + 1;

            if (RendernSpielfeld.Performancemode)
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

        private void SelectMenu()
        {
            switch (MenuTracker)
            {
                case 1:
                    Musik.Currentmusik = MusikSelector.Selector();
                    Musik.Melodie();
                    Program.CurrentView = 1;
                    break;
                case 2:
                    Program.CurrentView = 2;
                    break;
                case 3:
                    Program.CurrentView = 3;
                    break;
                case 4:
                    Program.CurrentView = 4;
                    break;
                case 5:
                    Program.CurrentView = 5;
                    break;
                case 6:
                    Program.CurrentView = 6;
                    break;
                case 7:
                    Program.CurrentView = 0;
                    break;
            }
            StopInputstream();
        }
    }
}
