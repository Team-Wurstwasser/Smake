using Smake.Menues;
using Smake.Speicher;
using Smake.Spiel;

namespace Smake
{
    public class Program
    {
        public static int CurrentView = 7;

        // Main
        static void Main()
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.Title = "Smake";
            LanguageManager.Load("de");

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
            Musik.Currentmusik = GameData.MusikDaten.Menue.Eingabe;
            Musik.Melodie();

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