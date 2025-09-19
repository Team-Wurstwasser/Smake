namespace Smake.io
{
    using System;
    using System.Numerics;
    using Smake.io.Spiel;
    using Smake.io.Speicher;

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
    
            Thread melodieThread = new(Musik.Melodie);
            melodieThread.Start();

            Menüs.Eingaben();
            do
            {

                Menüs.ShowMainMenue();

            } while (!Spiellogik.exit);
            melodieThread.Join();
        }

    }

}