using Smake.io.Spiel;
using Smake.io.Speicher;
using Smake.io.Menus;

namespace Smake.io
{
    public class Program
    {
        // Main
        static void Main()
        {
            
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;
            GameData.LoadAllConfigs();

            SpeicherSystem.Speichern_Laden("Laden");

            //Thread melodieThread = new(Musik.Melodie);
            //melodieThread.Start();

            Eingaben();

            Menu menu = new();
        }

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.Eingabe;

            Console.Clear();
            Console.Write("Spieler 1, gib deinen Namen ein: ");
            Spiellogik.player.Name = Console.ReadLine();
            Console.Clear();

            Console.Clear();
            Console.Write("Spieler 2, gib deinen Namen ein: ");
            Spiellogik.player2.Name = Console.ReadLine();
            Console.Clear();
        }

    }

}