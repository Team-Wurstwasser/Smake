using Smake.Spiel;
using Smake.Speicher;
using Smake.Menues;

namespace Smake
{
    public class Program
    {
        public static int CurrentView = 7;
        public static bool Exit = false;
        // Main
        static void Main()
        {
            
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.Title = "Smake";

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;
            GameData.LoadAllConfigs();

            SpeicherSystem.Speichern_Laden("Laden");

            Eingaben();

            do
            {
                _ = CurrentView switch
                {
                    1 => (object)new Spiellogik(),
                    2 => (object)new Einstellungen(),
                    3 => (object)new Shop(),
                    4 => (object)new Skin_Farben(),
                    5 => (object)new Statistiken(),
                    6 => (object)new Anleitung(),
                    7 => (object)new Menue(),
                    _ => Exit = true,
                };

            }
            while (!Exit);
        }

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue.Eingabe;
            Musik.Melodie();

            Console.Clear();

            Console.Write("Spieler 1, gib deinen Namen ein: ");
            Spiellogik.Player.Name = Console.ReadLine();

            Console.Clear();

            Console.Write("Spieler 2, gib deinen Namen ein: ");
            Spiellogik.Player2.Name = Console.ReadLine(); 

            Console.Clear();
        }

    }

}