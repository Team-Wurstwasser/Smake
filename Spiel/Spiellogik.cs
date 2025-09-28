using Smake.io.Values;
using Smake.io.Menues;
using Smake.io.Render;
using Smake.io.Speicher;
using Smake.io.Spieler;

namespace Smake.io.Spiel
{
    public class Spiellogik : RendernSpielfeld
    {
        public static bool Spiel { get; set; }
        int gameover;
        bool unentschieden;

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Fügen Sie ggf. den „erforderlichen“ Modifizierer hinzu, oder deklarieren Sie den Modifizierer als NULL-Werte zulassend.
        public static Player Player { get; set; }

        public static Player Player2 { get; set; }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Fügen Sie ggf. den „erforderlichen“ Modifizierer hinzu, oder deklarieren Sie den Modifizierer als NULL-Werte zulassend.

        public static List<Futter> Essen { get; private set; } = [];

        public Spiellogik()
        {
          Spielloop();
        }

        // Allen Variablen den Startwert geben
        void Neustart()
        {
            int? currentMusik = 0;

            if (Spielvalues.gamemode == "Normal")
            {
                if (Spielvalues.difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Normal.Langsam;
                else if (Spielvalues.difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Normal.Mittel;
                else if (Spielvalues.difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Normal.Schnell;
            }
            else if (Spielvalues.gamemode == "Unendlich")
            {
                if (Spielvalues.difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Unendlich.Langsam;
                else if (Spielvalues.difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Unendlich.Mittel;
                else if (Spielvalues.difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Unendlich.Schnell;
            }
            else if (Spielvalues.gamemode == "Babymode")
            {
                if (Spielvalues.difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Babymode.Langsam;
                else if (Spielvalues.difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Babymode.Mittel;
                else if (Spielvalues.difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Babymode.Schnell;
            }

            // Zuweisung an dein Musiksystem
            Musik.Currentmusik = currentMusik;
            Musik.Melodie();
            SpeicherSystem.Speichern_Laden("Speichern");

            Spiel = true;

            gameover = 0;

            unentschieden = false;

            // Zeit einstellen
            if (Spielvalues.difficulty == "Langsam") Spielvalues.zeit = GameData.SpielSchwierigkeit.Langsam;
            else if (Spielvalues.difficulty == "Mittel") Spielvalues.zeit = GameData.SpielSchwierigkeit.Mittel;
            else Spielvalues.zeit = GameData.SpielSchwierigkeit.Schnell;

            // Initialisiere das Spielfeld mit Rahmen
            InitialisiereSpielfeld();

            Player.Neustart();

            if (Spielvalues.multiplayer)
            {
                Player2.Neustart();
            }

            for (int i = 0; i < Spielvalues.maxfutter; i++)
            {
                if(!Skinvalues.foodfarbeRandom)
                {
                    Essen.Add(new Futter(Skinvalues.food, Skinvalues.foodfarbe));
                }
                else
                {
                    List<int> freigeschalteteFarbenIndex = [0]; // Weiß ist immer dabei

                    for (int t = 1; t < Menüsvalues.freigeschaltetFarben.Length; t++)
                    {
                        if (Menüsvalues.freigeschaltetFarben[t])
                            freigeschalteteFarbenIndex.Add(t);
                    }

                    // Zufällige Auswahl aus den freigeschalteten Farben
                    Random random = new();
                    int zufallIndex = random.Next(freigeschalteteFarbenIndex.Count);
                    int farbenIndex = freigeschalteteFarbenIndex[zufallIndex];

                    Essen.Add(new Futter(Skinvalues.food, GameData.Farben[farbenIndex]));
                }

            }
        }

        // Spielablauf
        void Spielloop()
        {
            Essen = [];

            Neustart();
            Render();

            Thread.Sleep(5);

            Thread inputThread = new(Steuerung.ReadInput);

            inputThread.Start();

            // Game Loop 
            while (Spiel)
            {

                Update();   // Spielerposition aktualisieren

                Render();   // Spielfeld neu zeichnen

                Thread.Sleep(Spielvalues.zeit); // Spieltempo regulieren

                Player.Aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                if (Spielvalues.multiplayer)
                {
                    Player2.Aenderung = true;
                }

            }

            Coins();

            inputThread.Join(); // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            ShowGameOverScreen(); // Spielende-Bildschirm

        }

        // Aktualisiert die Position des Spielers anhand der Eingabe
        void Update()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            {
                // Update Player 1
                var (spielerTot, Maxpunkte) = Player.Update();
                spieler1Tot |= spielerTot;
                spieler2Tot |= Maxpunkte;  // Falls MaxPunkte
            }

            // Update Player 2
            if (Spielvalues.multiplayer)
            {
                var (spielerTot, Maxpunkte) = Player2.Update();
                spieler2Tot |= spielerTot;
                spieler1Tot |= Maxpunkte;  // Falls MaxPunkte
            }

            GameoverCheck(spieler1Tot, spieler2Tot);
        }

        // Prüft, ob das Spiel vorbei ist
        void GameoverCheck(bool spieler1Tot, bool spieler2Tot)
        {

            if (spieler1Tot && spieler2Tot)
            {
                unentschieden = true;
                Spiel = false;
            }
            else if (spieler1Tot)
            {
                gameover = 1;
                Spiel = false;
            }
            else if (spieler2Tot)
            {
                gameover = 2;
                Spiel = false;
            }

        }

        // Zeigt den Game-Over-Screen an
        void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("              GAME OVER              ");
            Console.WriteLine("═════════════════════════════════════");

            if (Spielvalues.multiplayer)
            {
                if (unentschieden)
                {
                    Console.WriteLine();
                    Console.WriteLine("Unentschieden!");
                    Console.WriteLine($"{Player.Name} hat {Player   .Punkte} Punkte erreicht.");
                    Console.WriteLine($"{Player2.Name} hat {Player2.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{Player2.Name} gewinnt!");
                    Console.WriteLine($"Punkte: {Player2.Punkte}");
                    Console.WriteLine($"{Player.Name} hat {Player.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{Player.Name} gewinnt!");
                    Console.WriteLine($"Punkte: {Player.Punkte}");
                    Console.WriteLine($"{Player2.Name} hat {Player2.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
            }
            else
            {
                if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Leider verloren – versuch's noch einmal!");
                    Console.WriteLine($"Du hast {Player.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("Glückwunsch! Du hast gewonnen!");
                    Console.WriteLine($"Deine Punktzahl: {Player.Punkte}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("Drücke [Esc],   um zum Menü zurückzukehren\n" +
                              "oder   [Enter], um ein neues Spiel zu starten ");
            bool check = false;
            do
            {
                while (Console.KeyAvailable) Console.ReadKey(true);
                var key2 = Console.ReadKey(true).Key;
                switch (key2)
                {
                    case ConsoleKey.Enter:
                        check = true;
                        _ = new Spiellogik();
                        break;

                    case ConsoleKey.Escape:
                        check = true;
                        _ = new Menue();
                        break;

                }
            }
            while (!check);
            while (Console.KeyAvailable) Console.ReadKey(true);   // Leere Eingabepuffer vollständig
        }

        // Coins und xp hinzufügen
        static void Coins()
        {
            if (Spielvalues.gamemode != "Babymode")
            {
                if (Menüsvalues.highscore < Player.Punkte)
                { Menüsvalues.highscore = Player.Punkte; }
                else if (Menüsvalues.highscore < Player2.Punkte)
                { Menüsvalues.highscore = Player2.Punkte; }

                Menüsvalues.spieleGesamt++;

                switch (Spielvalues.difficulty)
                {
                    case "Langsam":
                        Menüsvalues.gesamtcoins = Player.Punkte + Player2.Punkte + Menüsvalues.gesamtcoins;
                        Spielstatus.coins = Player.Punkte + Player2.Punkte + Spielstatus.coins;
                        Spielstatus.xp = Player.Punkte + Player2.Punkte + Spielstatus.xp;
                        break;

                    case "Mittel":
                        Menüsvalues.gesamtcoins = 2 * (Player.Punkte + Player2.Punkte) + Menüsvalues.gesamtcoins;
                        Spielstatus.coins = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.coins;
                        Spielstatus.xp = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.xp;
                        break;

                    case "Schnell":
                        Menüsvalues.gesamtcoins = 3 * (Player.Punkte + Player2.Punkte) + Menüsvalues.gesamtcoins;
                        Spielstatus.coins = 3 * (Player.Punkte + Player2.Punkte) + Spielstatus.coins;
                        Spielstatus.xp = 3 * (Player.Punkte + Player2.Punkte) + Spielstatus.xp;
                        break;
                }
            }
            else
            {
                Menüsvalues.gesamtcoins = (GameData.MaxPunkte) / 2 + Menüsvalues.gesamtcoins;
                Spielstatus.coins = (GameData.MaxPunkte) / 2 + Spielstatus.coins;
            }

        }
    }
}
