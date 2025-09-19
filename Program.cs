namespace Smake.io
{
    using System;
    using System.Numerics;
    using Smake.io.Spiel;

    public class Program
    {
        // Main
        static void Main()
        {
            
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;

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