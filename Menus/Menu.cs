using Smake.io.Values;
using Smake.io.Render;
using Smake.io.Speicher;
using Smake.io.Spiel;

namespace Smake.io.Menus
{
    public class Menu : Screen
    {
        string[] menu = [
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

        public Menu()
        {
            Console.Clear();
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.Main;

            SpeicherSystem.Speichern_Laden("Speichern");

            // Level-Berechnung (1 Level pro 100 XP)
            Spielstatus.level = Spielstatus.xp / 100 + 1;

            if (RendernSpielfeld.performancemode)
            {
                Skinvalues.foodfarbe = GameData.Farben[0];
                Skinvalues.foodfarbeRandom = false;
                Skinvalues.randfarbe = GameData.Farben[0];
                Spiellogik.player.Farbe = GameData.Farben[0];
                Spiellogik.player.Headfarbe = GameData.Farben[0];
                Spiellogik.player2.Farbe = GameData.Farben[0];
                Spiellogik.player2.Headfarbe = GameData.Farben[0];
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
                    break;
                case 2:
                    _ = new Einstellungen();
                    break;
                case 3:
                    _ = new Shop();
                    break;
                case 4:
                    _ = new Skin_Farben();
                    break;
                case 5:
                    _ = new Statistiken();
                    break;
                case 6:
                    _ = new Anleitung();
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            DoReadInput = false;
            StopInputstream();
        }
    }
}
