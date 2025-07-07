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

            SpeicherSytem.Speichern_Laden("Laden");
    
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
            if (Spiellogik.gamemode != "babymode")
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

        }

        // Initialisiert das Spielfeld: Rahmen, leere Fläche, Spieler
        public static void InitialisiereSpiel()

        {

            Console.SetCursorPosition(0, 0);

            for (int reihe = 0; reihe < Spiellogik.grid.GetLength(0); reihe++)

            {

                for (int symbol = 0; symbol < Spiellogik.grid.GetLength(1); symbol++)

                {

                    // Rand des Spielfelds mit '#' markieren

                    if (reihe == 0 || reihe == Spiellogik.grid.GetLength(0) - 1 || symbol == 0 || symbol == Spiellogik.grid.GetLength(1) - 1)

                    {

                        Spiellogik.grid[reihe, symbol] = Spiellogik.rand;

                    }

                    else

                    {

                        Spiellogik.grid[reihe, symbol] = ' ';

                    }

                }

            }

            // Spielerzeichen auf Startposition setzen

            Spiellogik.grid[Spiellogik.player.PlayerY[0], Spiellogik.player.PlayerX[0]] = Spiellogik.player.Head;

            if (Spiellogik.multiplayer)
            {
                Spiellogik.grid[Spiellogik.player2.PlayerY[0], Spiellogik.player2.PlayerX[0]] = Spiellogik.player2.Head;
            }
        }

    }

}