using Smake.Helper;
using Smake.Render;
using Smake.Speicher;
using Smake.Spiel;
using Smake.Values;
using System.Diagnostics;

namespace Smake.Menues
{
    public class Menue : RendernMenue
    {
        readonly string[] menu = [
                "Spiel starten",
                "Einstellungen",
                "Shop",
                "Skins/Farben",
                "Statistiken",
                "Anleitung",
                "Beenden"
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
            Musik.Currentmusik = GameData.MusikDaten.Menue.Main;

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
            Display = menu;
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
                    StopInputstream();
                    _ = new Spiellogik();
                    break;
                case 2:
                    StopInputstream();
                    _ = new Einstellungen();
                    break;
                case 3:
                    StopInputstream();
                    _ = new Shop();
                    break;
                case 4:
                    StopInputstream();
                    _ = new Skin_Farben();
                    break;
                case 5:
                    StopInputstream();
                    _ = new Statistiken();
                    break;
                case 6:
                    StopInputstream();
                    _ = new Anleitung();
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
