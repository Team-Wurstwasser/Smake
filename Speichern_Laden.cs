using System.Text.Json;

namespace Snake.io
{
    public class Speicheraufbau
    {
        public bool[] FreigeschaltetTail { get; set; } = new bool[7];
        public bool[] FreigeschaltetRand { get; set; } = new bool[7];
        public bool[] FreigeschaltetFood { get; set; } = new bool[7];
        public bool[] FreigeschaltetFarben { get; set; } = new bool[15];
        public int[] Skin_Farbenzahl { get; set; } = new int[10];
        public bool Performancemode { get; set; }
        public int Coins { get; set; }
        public int Xp { get; set; }
        public string Difficulty { get; set; }
        public string Gamemode { get; set; }
        public bool Multiplayer { get; set; }
        public int Gesamtcoins { get; set; }
        public int Highscore { get; set; }
        public int SpieleGesamt { get; set; }


    }

    public class SpeicherSytem
    {

        // Speicher und Ladesystem
        public static void Speichern_Laden(string speicher_laden)
        {
            string pfad = "spielstand.json";

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
                Performancemode = Program.performancemode,
                Coins = Program.coins,
                Difficulty = Program.difficulty,
                Gamemode = Program.gamemode,
                Multiplayer = Program.multiplayer,
                Gesamtcoins = Program.gesamtcoins,
                Highscore = Program.highscore,
                SpieleGesamt = Program.spieleGesamt,
                FreigeschaltetTail = Menüs.freigeschaltetTail,
                FreigeschaltetRand = Menüs.freigeschaltetRand,
                FreigeschaltetFood = Menüs.freigeschaltetFood,
                FreigeschaltetFarben = Menüs.freigeschaltetFarben,
                Xp = Program.xp,
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

            Program.performancemode = speicheraufbau.Performancemode;
            Program.coins = speicheraufbau.Coins;
            Program.xp = speicheraufbau.Xp;
            Program.difficulty = speicheraufbau.Difficulty;
            Program.gamemode = speicheraufbau.Gamemode;
            Program.multiplayer = speicheraufbau.Multiplayer;
            Program.gesamtcoins = speicheraufbau.Gesamtcoins;
            Program.highscore = speicheraufbau.Highscore;
            Program.spieleGesamt = speicheraufbau.SpieleGesamt;

            Menüs.freigeschaltetTail = speicheraufbau.FreigeschaltetTail;
            Menüs.freigeschaltetRand = speicheraufbau.FreigeschaltetRand;
            Menüs.freigeschaltetFood = speicheraufbau.FreigeschaltetFood;
            Menüs.freigeschaltetFarben = speicheraufbau.FreigeschaltetFarben;

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