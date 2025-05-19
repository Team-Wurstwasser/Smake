using System.Drawing;
using System.Numerics;

namespace Snake.io
{
    public class SpeicherSytem
    {
        // Speicher und Ladesystem
        public static void Speichern_Laden(string speicher_laden)
        {
            string pfad = "spielstand.txt";
            //Nur wenn File nicht gefunden wird
            if (!File.Exists(pfad))
            {
                Program.randzahl = 0;
                Program.foodzahl = 0;
                Program.skinzahl = 0;
                Program.skin2zahl = 1;
                Program.headfarbezahl = 0;
                Program.headfarbe2zahl = 0;
                Program.farbezahl = 0;
                Program.farbe2zahl = 0;
                Program.foodfarbezahl = 0;
                Program.randfarbezahl = 0;

                Program.freigeschaltetTail[0] = true;
                Program.freigeschaltetTail[1] = true;
                Program.freigeschaltetFood[0] = true;
                Program.freigeschaltetRand[0] = true;
                Program.freigeschaltetFarben[0] = true;

                Program.performancemode = false;

                // Startguthaben
                Program.coins = 0;
                Program.xp = 0;

                // Standard Modi
                Program.difficulty = "Mittel";
                Program.gamemode = "Normal";
                Program.multiplayer = false;

                // Standard Skins/Farben
                Program.rand = '?';
                Program.food = '*';
                Program.player.Skin = '+';
                Program.player2.Skin = 'x';
                Program.randfarbe = ConsoleColor.White;
                Program.foodfarbe = ConsoleColor.White;
                Program.farbe = ConsoleColor.White;
                Program.farbe = ConsoleColor.White;
                Program.farbe2 = ConsoleColor.White;
                Program.headfarbe = ConsoleColor.White;
                Program.headfarbe2 = ConsoleColor.White;


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
            //Liste was gespeichert wird
            var Zeilen = new List<string>
            {
                $"randzahl={Program.randzahl}",
                $"foodzahl={Program.foodzahl}",
                $"skinzahl={Program.skinzahl}",
                $"skin2zahl={Program.skin2zahl}",
                $"headfarbezahl={Program.headfarbezahl}",
                $"headfarbe2zahl={Program.headfarbe2zahl}",
                $"farbezahl={Program.farbezahl}",
                $"farbe2zahl={Program.farbe2zahl}",
                $"foodfarbezahl={Program.foodfarbezahl}",
                $"randfarbezahl={Program.randfarbezahl}",
                $"freigeschaltetTail0={Program.freigeschaltetTail[0]}",
                $"freigeschaltetTail1={Program.freigeschaltetTail[1]}",
                $"freigeschaltetTail2={Program.freigeschaltetTail[2]}",
                $"freigeschaltetTail3={Program.freigeschaltetTail[3]}",
                $"freigeschaltetTail4={Program.freigeschaltetTail[4]}",
                $"freigeschaltetTail5={Program.freigeschaltetTail[5]}",
                $"freigeschaltetTail6={Program.freigeschaltetTail[6]}",
                $"freigeschaltetFood0={Program.freigeschaltetFood[0]}",
                $"freigeschaltetFood1={Program.freigeschaltetFood[1]}",
                $"freigeschaltetFood2={Program.freigeschaltetFood[2]}",
                $"freigeschaltetFood3={Program.freigeschaltetFood[3]}",
                $"freigeschaltetFood4={Program.freigeschaltetFood[4]}",
                $"freigeschaltetFood5={Program.freigeschaltetFood[5]}",
                $"freigeschaltetFood6={Program.freigeschaltetFood[6]}",
                $"freigeschaltetRand0={Program.freigeschaltetRand[0]}",
                $"freigeschaltetRand1={Program.freigeschaltetRand[1]}",
                $"freigeschaltetRand2={Program.freigeschaltetRand[2]}",
                $"freigeschaltetRand3={Program.freigeschaltetRand[3]}",
                $"freigeschaltetRand4={Program.freigeschaltetRand[4]}",
                $"freigeschaltetRand5={Program.freigeschaltetRand[5]}",
                $"freigeschaltetRand6={Program.freigeschaltetRand[6]}",
                $"freigeschaltetFarben0={Program.freigeschaltetFarben[0]}",
                $"freigeschaltetFarben1={Program.freigeschaltetFarben[1]}",
                $"freigeschaltetFarben2={Program.freigeschaltetFarben[2]}",
                $"freigeschaltetFarben3={Program.freigeschaltetFarben[3]}",
                $"freigeschaltetFarben4={Program.freigeschaltetFarben[4]}",
                $"freigeschaltetFarben5={Program.freigeschaltetFarben[5]}",
                $"freigeschaltetFarben6={Program.freigeschaltetFarben[6]}",
                $"freigeschaltetFarben7={Program.freigeschaltetFarben[7]}",
                $"freigeschaltetFarben8={Program.freigeschaltetFarben[8]}",
                $"freigeschaltetFarben9={Program.freigeschaltetFarben[9]}",
                $"freigeschaltetFarben10={Program.freigeschaltetFarben[10]}",
                $"freigeschaltetFarben11={Program.freigeschaltetFarben[11]}",
                $"freigeschaltetFarben12={Program.freigeschaltetFarben[12]}",
                $"freigeschaltetFarben13={Program.freigeschaltetFarben[13]}",
                $"freigeschaltetFarben14={Program.freigeschaltetFarben[14]}",
                $"performancemode={Program.performancemode}",
                $"coins={Program.coins}",
                $"xp={Program.xp}",
                $"spieleGesamt={Program.spieleGesamt}",
                $"highscore={Program.highscore}",
                $"gesamtcoins={Program.gesamtcoins}",
                $"difficulty={Program.difficulty}",
                $"gamemode={Program.gamemode}",
                $"multiplayer={Program.multiplayer}",
                $"rand={Program.rand}",
                $"food={Program.food}",
                $"player.Skin={Program.player.Skin}",
                $"player2.Skin={Program.player2.Skin}",
                $"randfarbe={Program.randfarbe}",
                $"foodfarbe={Program.foodfarbe}",
                $"farbe={Program.farbe}",
                $"farbe2={Program.farbe2}",
                $"headfarbe={Program.headfarbe}",
                $"headfarbe2={Program.headfarbe2}"
            };

            File.WriteAllLines(pfad, Zeilen);
        }


        // Speicher laden Logik 
        public static void Laden(string pfad)
        {

            var Zeilen = File.ReadAllLines(pfad);

            foreach (var Zeile in Zeilen)
            {
                //Teilt was vor und nach dem = steht
                var Teil = Zeile.Split('=');

                string Variablenname = Teil[0];
                string Wert = Teil[1];

                //Entscheidet was in die Variablen eingespeichert wird
                switch (Variablenname)
                {
                    case "randzahl": Program.randzahl = int.Parse(Wert); break;
                    case "foodzahl": Program.foodzahl = int.Parse(Wert); break;
                    case "skinzahl": Program.skinzahl = int.Parse(Wert); break;
                    case "skin2zahl": Program.skin2zahl = int.Parse(Wert); break;
                    case "headfarbezahl": Program.headfarbezahl = int.Parse(Wert); break;
                    case "headfarbe2zahl": Program.headfarbe2zahl = int.Parse(Wert); break;
                    case "farbezahl": Program.farbezahl = int.Parse(Wert); break;
                    case "farbe2zahl": Program.farbe2zahl = int.Parse(Wert); break;
                    case "foodfarbezahl": Program.foodfarbezahl = int.Parse(Wert); break;
                    case "randfarbezahl": Program.randfarbezahl = int.Parse(Wert); break;

                    case "freigeschaltetTail0": Program.freigeschaltetTail[0] = bool.Parse(Wert); break;
                    case "freigeschaltetTail1": Program.freigeschaltetTail[1] = bool.Parse(Wert); break;
                    case "freigeschaltetTail2": Program.freigeschaltetTail[2] = bool.Parse(Wert); break;
                    case "freigeschaltetTail3": Program.freigeschaltetTail[3] = bool.Parse(Wert); break;
                    case "freigeschaltetTail4": Program.freigeschaltetTail[4] = bool.Parse(Wert); break;
                    case "freigeschaltetTail5": Program.freigeschaltetTail[5] = bool.Parse(Wert); break;
                    case "freigeschaltetTail6": Program.freigeschaltetTail[6] = bool.Parse(Wert); break;

                    case "freigeschaltetFood0": Program.freigeschaltetFood[0] = bool.Parse(Wert); break;
                    case "freigeschaltetFood1": Program.freigeschaltetFood[1] = bool.Parse(Wert); break;
                    case "freigeschaltetFood2": Program.freigeschaltetFood[2] = bool.Parse(Wert); break;
                    case "freigeschaltetFood3": Program.freigeschaltetFood[3] = bool.Parse(Wert); break;
                    case "freigeschaltetFood4": Program.freigeschaltetFood[4] = bool.Parse(Wert); break;
                    case "freigeschaltetFood5": Program.freigeschaltetFood[5] = bool.Parse(Wert); break;
                    case "freigeschaltetFood6": Program.freigeschaltetFood[6] = bool.Parse(Wert); break;

                    case "freigeschaltetRand0": Program.freigeschaltetRand[0] = bool.Parse(Wert); break;
                    case "freigeschaltetRand1": Program.freigeschaltetRand[1] = bool.Parse(Wert); break;
                    case "freigeschaltetRand2": Program.freigeschaltetRand[2] = bool.Parse(Wert); break;
                    case "freigeschaltetRand3": Program.freigeschaltetRand[3] = bool.Parse(Wert); break;
                    case "freigeschaltetRand4": Program.freigeschaltetRand[4] = bool.Parse(Wert); break;
                    case "freigeschaltetRand5": Program.freigeschaltetRand[5] = bool.Parse(Wert); break;
                    case "freigeschaltetRand6": Program.freigeschaltetRand[6] = bool.Parse(Wert); break;

                    case "freigeschaltetFarben0": Program.freigeschaltetFarben[0] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben1": Program.freigeschaltetFarben[1] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben2": Program.freigeschaltetFarben[2] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben3": Program.freigeschaltetFarben[3] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben4": Program.freigeschaltetFarben[4] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben5": Program.freigeschaltetFarben[5] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben6": Program.freigeschaltetFarben[6] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben7": Program.freigeschaltetFarben[7] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben8": Program.freigeschaltetFarben[8] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben9": Program.freigeschaltetFarben[9] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben10": Program.freigeschaltetFarben[10] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben11": Program.freigeschaltetFarben[11] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben12": Program.freigeschaltetFarben[12] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben13": Program.freigeschaltetFarben[13] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben14": Program.freigeschaltetFarben[14] = bool.Parse(Wert); break;

                    case "performancemode": Program.performancemode = bool.Parse(Wert); break;
                    case "coins": Program.coins = int.Parse(Wert); break;
                    case "xp": Program.xp = int.Parse(Wert); break;
                    case "gesamtcoins": Program.gesamtcoins = int.Parse(Wert); break;
                    case "highscore": Program.highscore = int.Parse(Wert); break;
                    case "spieleGesamt": Program.spieleGesamt = int.Parse(Wert); break;

                    case "difficulty": Program.difficulty = Wert; break;
                    case "gamemode": Program.gamemode = Wert; break;
                    case "multiplayer": Program.multiplayer = bool.Parse(Wert); break;

                    case "rand": Program.rand = char.Parse(Wert); break;
                    case "food": Program.food = char.Parse(Wert); break;
                    case "player.Skin": Program.player.Skin = char.Parse(Wert); break;
                    case "player2.Skin": Program.player2.Skin = char.Parse(Wert); break;

                    case "randfarbe": Program.randfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "foodfarbe": Program.foodfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "farbe": Program.farbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "farbe2": Program.farbe2 = Enum.Parse<ConsoleColor>(Wert); break;
                    case "headfarbe": Program.headfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "headfarbe2": Program.headfarbe2 = Enum.Parse<ConsoleColor>(Wert); break;
                }
            }
        }
    }
}