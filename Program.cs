using Smake.io.Spiel;
using Smake.io.Speicher;
using Smake.io.Menues;

namespace Smake.io
{
    public class Program
    {
        // Main
        static void Main()
        {
            
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.Title = "Smake.io";

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;
            GameData.LoadAllConfigs();

            Eingaben();

            SpeicherSystem.Speichern_Laden("Laden");

            _ = new Menue();
        }

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.Eingabe;
            Musik.Melodie();

            Console.Clear();

            Console.Write("Spieler 1, gib deinen Namen ein: ");
            Spiellogik.player = new(GameData.Startpositionen.Spieler1.X, GameData.Startpositionen.Spieler1.Y, Console.ReadLine());

            Console.Clear();

            Console.Write("Spieler 2, gib deinen Namen ein: ");
            Spiellogik.player2 = new(GameData.Startpositionen.Spieler2.X, GameData.Startpositionen.Spieler2.Y, Console.ReadLine());

            Console.Clear();
        }

    }

}