using Smake.Menues;
using Smake.Speicher;
using Smake.Spiel;
using Smake.SFX;

namespace Smake
{
    public class Program
    {
        private static int currentView = 7;

        public static int CurrentView { get => currentView; set => currentView = value; }

        // Main
        static void Main()
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.Title = "Smake";

            // Sprache aus config.json laden
            LanguageManager.Speichern_Laden("Laden");

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;
            GameData.LoadAllConfigs();

            SpeicherSystem.Speichern_Laden("Laden");

            Eingaben();
            bool Exit = false;
            do
            {
                switch (CurrentView)
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
                        _ = new Menue();
                        break;
                    default:
                        Exit = true;
                        break;
                }

            } while (!Exit);

        }

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            Sounds.Currentmusik = GameData.MusikDaten.Menue?.Eingabe ?? 0;
            Sounds.Melodie();

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