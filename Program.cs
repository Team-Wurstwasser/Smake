using Smake.Spiel;
using Smake.Speicher;
using Smake.Menues;

namespace Smake
{
    public class Program
    {
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

            _ = new Menue();
        }

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            // Zuweisung an dein Musiksystem
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