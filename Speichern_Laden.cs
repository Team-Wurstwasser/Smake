using System.Text.Json;

namespace Snake.io
{
    public class Speicheraufbau
    {
        public bool[] freigeschaltetTail { get; set; } = new bool[7];
        public bool[] freigeschaltetRand { get; set; } = new bool[7];
        public bool[] freigeschaltetFood { get; set; } = new bool[7];
        public bool[] freigeschaltetFarben { get; set; } = new bool[15];
        public int[] Skin_Farbenzahl { get; set; } = new int[10];
        public bool performancemode { get; set; }
        public int coins { get; set; }
        public int xp { get; set; }
        public string difficulty { get; set; }
        public string gamemode { get; set; }
        public bool multiplayer { get; set; }
        public int gesamtcoins { get; set; }
        public int highscore { get; set; }
        public int spieleGesamt { get; set; }


    }

    public class SpeicherSytem
    {

        // Speicher und Ladesystem
        public static void Speichern_Laden(string speicher_laden)
        {
            string pfad = "spielstand.txt";

            //Nur wenn File nicht gefunden wird
            if (!File.Exists(pfad))
            {
                Menüs.randzahl = 0;
                Menüs.foodzahl = 0;
                Program.player.Skinzahl = 0;
                Program.player2.Skinzahl = 1;
                Program.player.Headfarbezahl = 0;
                Program.player2.Headfarbezahl = 0;
                Program.player.Farbezahl = 0;
                Program.player2.Farbezahl = 0;
                Menüs.foodfarbezahl = 0;
                Menüs.randfarbezahl = 0;

                Menüs.freigeschaltetTail[0] = true;
                Menüs.freigeschaltetTail[1] = true;
                Menüs.freigeschaltetFood[0] = true;
                Menüs.freigeschaltetRand[0] = true;
                Menüs.freigeschaltetFarben[0] = true;

                Program.performancemode = false;

                // Startguthaben
                Program.coins = 0;
                Program.xp = 0;

                // Standard Modi
                Program.difficulty = "Mittel";
                Program.gamemode = "Normal";
                Program.multiplayer = false;

                // Standard Skins/Farben
                Program.rand = '█';
                Program.food = '*';
                Program.player.Skin = '+';
                Program.player2.Skin = 'x';
                Program.randfarbe = ConsoleColor.White;
                Program.foodfarbe = ConsoleColor.White;
                Program.player.Farbe = ConsoleColor.White;
                Program.player2.Farbe = ConsoleColor.White;
                Program.player.Headfarbe = ConsoleColor.White;
                Program.player2.Headfarbe = ConsoleColor.White;


                Program.gesamtcoins = 0;
                Program.highscore = 0;
                Program.spieleGesamt = 0;

                Speichern(pfad);
            }

            // Entscheidet ob geladen oder gespeichert wird
            switch (speicher_laden)
            {
                case "Speichern":
                    Speichern(pfad);
                    break;
                case "Laden":
                    Laden(pfad);
                    break;

            }
        }


        // Speicher Logik
        public static void Speichern(string pfad)
        {
            
            int[] Skin_Farbenzahlhaupt = [
                Menüs.randzahl,
                Menüs.foodzahl, 
                Program.player.Skinzahl,
                Program.player2.Skinzahl,
                Program.player.Headfarbezahl,
                Program.player2.Headfarbezahl,
                Program.player.Farbezahl,
                Program.player2.Farbezahl,
                Menüs.foodfarbezahl,
                Menüs.randfarbezahl
                ];

            var speicheraufbau = new Speicheraufbau
            {
                performancemode = Program.performancemode,
                coins = Program.coins,
                difficulty = Program.difficulty,
                gamemode = Program.gamemode,
                multiplayer = Program.multiplayer,
                gesamtcoins = Program.gesamtcoins,
                highscore = Program.highscore,
                spieleGesamt = Program.spieleGesamt,
                freigeschaltetTail = Menüs.freigeschaltetTail,
                freigeschaltetRand = Menüs.freigeschaltetRand,
                freigeschaltetFood = Menüs.freigeschaltetFood,
                freigeschaltetFarben = Menüs.freigeschaltetFarben,
                Skin_Farbenzahl = Skin_Farbenzahlhaupt

            };


            string jsonString = JsonSerializer.Serialize(speicheraufbau);

            File.WriteAllText(pfad, jsonString);
        }


        // Speicher laden Logik 
        public static void Laden(string pfad)
        {

            string jsonString = File.ReadAllText(pfad);
            var speicheraufbau = JsonSerializer.Deserialize<Speicheraufbau>(jsonString);

            Program.performancemode = speicheraufbau.performancemode;
            Program.coins = speicheraufbau.coins;
            Program.xp = speicheraufbau.xp;
            Program.difficulty = speicheraufbau.difficulty;
            Program.gamemode = speicheraufbau.gamemode;
            Program.multiplayer = speicheraufbau.multiplayer;
            Program.gesamtcoins = speicheraufbau.gesamtcoins;
            Program.highscore = speicheraufbau.highscore;
            Program.spieleGesamt = speicheraufbau.spieleGesamt;

            Menüs.freigeschaltetTail = speicheraufbau.freigeschaltetTail;
            Menüs.freigeschaltetRand = speicheraufbau.freigeschaltetRand;
            Menüs.freigeschaltetFood = speicheraufbau.freigeschaltetFood;
            Menüs.freigeschaltetFarben = speicheraufbau.freigeschaltetFarben;

            int[] Skin_Farbenzahlhaupt = speicheraufbau.Skin_Farbenzahl;

            Menüs.randzahl = Skin_Farbenzahlhaupt[0];
            Menüs.foodzahl = Skin_Farbenzahlhaupt[1];
            Program.player.Skinzahl = Skin_Farbenzahlhaupt[2];
            Program.player2.Skinzahl = Skin_Farbenzahlhaupt[3];
            Program.player.Headfarbezahl = Skin_Farbenzahlhaupt[4];
            Program.player2.Headfarbezahl = Skin_Farbenzahlhaupt[5];
            Program.player.Farbezahl = Skin_Farbenzahlhaupt[6];
            Program.player2.Farbezahl = Skin_Farbenzahlhaupt[7];
            Menüs.foodfarbezahl = Skin_Farbenzahlhaupt[8];
            Menüs.randfarbezahl = Skin_Farbenzahlhaupt[9];
        }
    }
}