namespace Smake.io
{
    using System;
    using System.Numerics;
    using Smake.io.Spiel;

    public class Program
    {
        // Level und Experience
        public static int coins;
        public static int xp;
        public static int level;


        // Statistik
        public static int spieleGesamt;
        public static int highscore;
        public static int gesamtcoins;

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

        // Coins und xp hinzufügen
        public static void Coins()
        {
            if (Spiellogik.gamemode != "Babymode")
            {
                if (highscore < Spiellogik.player.Punkte)
                { highscore = Spiellogik.player.Punkte; }
                else if (highscore < Spiellogik.player2.Punkte)
                { highscore = Spiellogik.player2.Punkte; }

                spieleGesamt++;

                switch (Spiellogik.difficulty)
                {
                    case "Langsam":
                        gesamtcoins = Spiellogik.player.Punkte + Spiellogik.player2.Punkte + gesamtcoins;
                        coins = Spiellogik.player.Punkte + Spiellogik.player2.Punkte + coins;
                        xp = Spiellogik.player.Punkte + Spiellogik.player2.Punkte + xp;
                        break;

                    case "Mittel":
                        gesamtcoins = 2 * (Spiellogik.player.Punkte + Spiellogik.player2.Punkte) + gesamtcoins;
                        coins = 2 * (Spiellogik.player.Punkte + Spiellogik.player2.Punkte) + coins;
                        xp = 2 * (Spiellogik.player.Punkte + Spiellogik.player2.Punkte) + xp;
                        break;

                    case "Schnell":
                        gesamtcoins = 3 * (Spiellogik.player.Punkte + Spiellogik.player2.Punkte) + gesamtcoins;
                        coins = 3 * (Spiellogik.player.Punkte + Spiellogik.player2.Punkte) + coins;
                        xp = 3 * (Spiellogik.player.Punkte + Spiellogik.player2.Punkte) + xp;
                        break;
                }
            }
            else
            {
                gesamtcoins = (Spiellogik.maxpunkte) / 2 + gesamtcoins;
                coins = (Spiellogik.maxpunkte) / 2 + coins;
            }

        }

    }

}