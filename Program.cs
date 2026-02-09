using Smake.Menues;
using Smake.Speicher;
using Smake.Game;
using Smake.SFX;
using Smake.Enums;

namespace Smake
{
    public static class Program
    {
        public static ViewType CurrentView { private get; set; } = ViewType.MainMenu;

        // Main
        static void Main()
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Smake";

            // Sprache aus config.json laden
            LanguageManager.Speichern_Laden(StorageAction.Load);

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;
            GameData.LoadAllConfigs();

            SpeicherSystem.Speichern_Laden(StorageAction.Load);

            Eingaben();
            bool Exit = false;
            do
            {
                switch (CurrentView)
                {
                    case ViewType.MainMenu:
                        _ = new MainMenu();
                        break;
                    case ViewType.Game:
                        _ = new Spiellogik();
                        break;
                    case ViewType.Settings:
                        _ = new Settings();
                        break;
                    case ViewType.Shop:
                        _ = new Shop();
                        break;
                    case ViewType.SkinColors:
                        _ = new SkinColors();
                        break;
                    case ViewType.Statistics:
                        _ = new Statistics();
                        break;
                    case ViewType.Instructions:
                        _ = new Instructions();
                        break;
                    case ViewType.Exit:
                    default:
                        Exit = true;
                        break;
                }

            } while (!Exit);
        }

        // Eingaben für Spielernamen
        static void Eingaben()
        {
            Sounds.Melodie(GameData.MusikDaten.Menue?.Eingabe ?? 0);

            Console.Clear();

            Console.Write(LanguageManager.Get("input.player1"));
            Spiellogik.Player.Name = Console.ReadLine();

            Console.Clear();

            Console.Write(LanguageManager.Get("input.player2"));
            Spiellogik.Player2.Name = Console.ReadLine();

            Console.Clear();
        }
    }
}