using Smake.Values;
using Smake.Render;
using Smake.Speicher;
using Smake.Spiel;

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
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        SelectMenu();
                        break;
                    default:
                        break;
                }
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
                    Render();
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
        }

        private void SelectMenu()
        {
            switch (MenuTracker)
            {
                case 1:
                    _ = new Spiellogik();
                    StopInputstream();
                    break;
                case 2:
                    _ = new Einstellungen();
                    StopInputstream();
                    break;
                case 3:
                    _ = new Shop();
                    StopInputstream();
                    break;
                case 4:
                    _ = new Skin_Farben();
                    StopInputstream();
                    break;
                case 5:
                    _ = new Statistiken();
                    StopInputstream();
                    break;
                case 6:
                    _ = new Anleitung();
                    StopInputstream();
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
