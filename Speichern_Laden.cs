using Smake.io.Render;
using Smake.io.Spiel;

namespace Smake.io
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
                Menüs.randzahl = 0;
                Menüs.foodzahl = 0;
                Spiellogik.player.Skinzahl = 0;
                Spiellogik.player2.Skinzahl = 1;
                Spiellogik.player.Headfarbezahl = 0;
                Spiellogik.player2.Headfarbezahl = 0;
                Spiellogik.player.Farbezahl = 0;
                Spiellogik.player2.Farbezahl = 0;
                Menüs.foodfarbezahl = 0;
                Menüs.randfarbezahl = 0;

                Musik.musikplay = true;
                Musik.soundplay = true;

                Menüs.freigeschaltetTail[0] = true;
                Menüs.freigeschaltetTail[1] = true;
                Menüs.freigeschaltetFood[0] = true;
                Menüs.freigeschaltetRand[0] = true;
                Menüs.freigeschaltetFarben[0] = true;

                RendernSpielfeld.performancemode = false;

                // Startguthaben
                Program.coins = 0;
                Program.xp = 0;

                // Standard Modi
                Spiellogik.difficulty = "Mittel";
                Spiellogik.gamemode = "Normal";
                Spiellogik.multiplayer = false;

                // Standard Skins/Farben
                Spiellogik.rand = '█';
                Spiellogik.food = '*';
                Spiellogik.player.Skin = '+';
                Spiellogik.player2.Skin = 'x';
                Spiellogik.randfarbe = ConsoleColor.White;
                Spiellogik.foodfarbe = ConsoleColor.White;
                Spiellogik.player.Farbe = ConsoleColor.White;
                Spiellogik.player2.Farbe = ConsoleColor.White;
                Spiellogik.player.Headfarbe = ConsoleColor.White;
                Spiellogik.player2.Headfarbe = ConsoleColor.White;


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
        static void Speichern(string pfad)
        {
            //Liste was gespeichert wird
            var Zeilen = new List<string>
            {
                $"randzahl={Menüs.randzahl}",
                $"foodzahl={Menüs.foodzahl}",
                $"Program.player.Skinzahl={Spiellogik.player.Skinzahl}",
                $"Program.player2.Skinzahl={Spiellogik.player2.Skinzahl}",
                $"Program.player.headfarbezahl={Spiellogik.player.Headfarbezahl}",
                $"Program.player2.headfarbezahl={Spiellogik.player.Headfarbezahl}",
                $"Program.player.Farbezahl={Spiellogik.player.Farbezahl}",
                $"Program.player2.Farbezahl={Spiellogik.player2.Farbezahl}",
                $"foodfarbezahl={Menüs.foodfarbezahl}",
                $"randfarbezahl={Menüs.randfarbezahl}",
                $"freigeschaltetTail0={Menüs.freigeschaltetTail[0]}",
                $"freigeschaltetTail1={Menüs.freigeschaltetTail[1]}",
                $"freigeschaltetTail2={Menüs.freigeschaltetTail[2]}",
                $"freigeschaltetTail3={Menüs.freigeschaltetTail[3]}",
                $"freigeschaltetTail4={Menüs.freigeschaltetTail[4]}",
                $"freigeschaltetTail5={Menüs.freigeschaltetTail[5]}",
                $"freigeschaltetTail6={Menüs.freigeschaltetTail[6]}",
                $"freigeschaltetFood0={Menüs.freigeschaltetFood[0]}",
                $"freigeschaltetFood1={Menüs.freigeschaltetFood[1]}",
                $"freigeschaltetFood2={Menüs.freigeschaltetFood[2]}",
                $"freigeschaltetFood3={Menüs.freigeschaltetFood[3]}",
                $"freigeschaltetFood4={Menüs.freigeschaltetFood[4]}",
                $"freigeschaltetFood5={Menüs.freigeschaltetFood[5]}",
                $"freigeschaltetFood6={Menüs.freigeschaltetFood[6]}",
                $"freigeschaltetRand0={Menüs.freigeschaltetRand[0]}",
                $"freigeschaltetRand1={Menüs.freigeschaltetRand[1]}",
                $"freigeschaltetRand2={Menüs.freigeschaltetRand[2]}",
                $"freigeschaltetRand3={Menüs.freigeschaltetRand[3]}",
                $"freigeschaltetRand4={Menüs.freigeschaltetRand[4]}",
                $"freigeschaltetRand5={Menüs.freigeschaltetRand[5]}",
                $"freigeschaltetRand6={Menüs.freigeschaltetRand[6]}",
                $"freigeschaltetFarben0={Menüs.freigeschaltetFarben[0]}",
                $"freigeschaltetFarben1={Menüs.freigeschaltetFarben[1]}",
                $"freigeschaltetFarben2={Menüs.freigeschaltetFarben[2]}",
                $"freigeschaltetFarben3={Menüs.freigeschaltetFarben[3]}",
                $"freigeschaltetFarben4={Menüs.freigeschaltetFarben[4]}",
                $"freigeschaltetFarben5={Menüs.freigeschaltetFarben[5]}",
                $"freigeschaltetFarben6={Menüs.freigeschaltetFarben[6]}",
                $"freigeschaltetFarben7={Menüs.freigeschaltetFarben[7]}",
                $"freigeschaltetFarben8={Menüs.freigeschaltetFarben[8]}",
                $"freigeschaltetFarben9={Menüs.freigeschaltetFarben[9]}",
                $"freigeschaltetFarben10={Menüs.freigeschaltetFarben[10]}",
                $"freigeschaltetFarben11={Menüs.freigeschaltetFarben[11]}",
                $"freigeschaltetFarben12={Menüs.freigeschaltetFarben[12]}",
                $"freigeschaltetFarben13={Menüs.freigeschaltetFarben[13]}",
                $"freigeschaltetFarben14={Menüs.freigeschaltetFarben[14]}",
                $"performancemode={RendernSpielfeld.performancemode}",
                $"coins={Program.coins}",
                $"xp={Program.xp}",
                $"spieleGesamt={Program.spieleGesamt}",
                $"highscore={Program.highscore}",
                $"gesamtcoins={Program.gesamtcoins}",
                $"difficulty={Spiellogik.difficulty}",
                $"gamemode={Spiellogik.gamemode}",
                $"multiplayer={Spiellogik.multiplayer}",
                $"rand={Spiellogik.rand}",
                $"food={Spiellogik.food}",
                $"player.Skin={Spiellogik.player.Skin}",
                $"player2.Skin={Spiellogik.player2.Skin}",
                $"randfarbe={Spiellogik.randfarbe}",
                $"foodfarbe={Spiellogik.foodfarbe}",
                $"player.Farbe={Spiellogik.player.Farbe}",
                $"player2.Farbe={Spiellogik.player2.Farbe}",
                $"player.Headfarbe={Spiellogik.player.Headfarbe}",
                $"player2.Headfarbe={Spiellogik.player2.Headfarbe}",
                $"Musik={Musik.musikplay }",
                $"Sound={Musik.soundplay}"
            };

            File.WriteAllLines(pfad, Zeilen);
        }


        // Speicher laden Logik 
        static void Laden(string pfad)
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
                    case "randzahl": Menüs.randzahl = int.Parse(Wert); break;
                    case "foodzahl": Menüs.foodzahl = int.Parse(Wert); break;
                    case "Program.player.Skinzahl": Spiellogik.player.Skinzahl = int.Parse(Wert); break;
                    case "Program.player2.Skinzahl": Spiellogik.player2.Skinzahl = int.Parse(Wert); break;
                    case "Program.player.headfarbezahl": Spiellogik.player.Headfarbezahl = int.Parse(Wert); break;
                    case "Program.player2.headfarbezahl": Spiellogik.player2.Headfarbezahl = int.Parse(Wert); break;
                    case "Program.player.Farbezahl": Spiellogik.player.Farbezahl = int.Parse(Wert); break;
                    case "Program.player2.farbe2zahl": Spiellogik.player2.Farbezahl = int.Parse(Wert); break;
                    case "foodfarbezahl": Menüs.foodfarbezahl = int.Parse(Wert); break;
                    case "randfarbezahl": Menüs.randfarbezahl = int.Parse(Wert); break;

                    case "freigeschaltetTail0": Menüs.freigeschaltetTail[0] = bool.Parse(Wert); break;
                    case "freigeschaltetTail1": Menüs.freigeschaltetTail[1] = bool.Parse(Wert); break;
                    case "freigeschaltetTail2": Menüs.freigeschaltetTail[2] = bool.Parse(Wert); break;
                    case "freigeschaltetTail3": Menüs.freigeschaltetTail[3] = bool.Parse(Wert); break;
                    case "freigeschaltetTail4": Menüs.freigeschaltetTail[4] = bool.Parse(Wert); break;
                    case "freigeschaltetTail5": Menüs.freigeschaltetTail[5] = bool.Parse(Wert); break;
                    case "freigeschaltetTail6": Menüs.freigeschaltetTail[6] = bool.Parse(Wert); break;

                    case "freigeschaltetFood0": Menüs.freigeschaltetFood[0] = bool.Parse(Wert); break;
                    case "freigeschaltetFood1": Menüs.freigeschaltetFood[1] = bool.Parse(Wert); break;
                    case "freigeschaltetFood2": Menüs.freigeschaltetFood[2] = bool.Parse(Wert); break;
                    case "freigeschaltetFood3": Menüs.freigeschaltetFood[3] = bool.Parse(Wert); break;
                    case "freigeschaltetFood4": Menüs.freigeschaltetFood[4] = bool.Parse(Wert); break;
                    case "freigeschaltetFood5": Menüs.freigeschaltetFood[5] = bool.Parse(Wert); break;
                    case "freigeschaltetFood6": Menüs.freigeschaltetFood[6] = bool.Parse(Wert); break;

                    case "freigeschaltetRand0": Menüs.freigeschaltetRand[0] = bool.Parse(Wert); break;
                    case "freigeschaltetRand1": Menüs.freigeschaltetRand[1] = bool.Parse(Wert); break; 
                    case "freigeschaltetRand2": Menüs.freigeschaltetRand[2] = bool.Parse(Wert); break;
                    case "freigeschaltetRand3": Menüs.freigeschaltetRand[3] = bool.Parse(Wert); break;
                    case "freigeschaltetRand4": Menüs.freigeschaltetRand[4] = bool.Parse(Wert); break;
                    case "freigeschaltetRand5": Menüs.freigeschaltetRand[5] = bool.Parse(Wert); break;
                    case "freigeschaltetRand6": Menüs.freigeschaltetRand[6] = bool.Parse(Wert); break;

                    case "freigeschaltetFarben0": Menüs.freigeschaltetFarben[0] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben1": Menüs.freigeschaltetFarben[1] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben2": Menüs.freigeschaltetFarben[2] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben3": Menüs.freigeschaltetFarben[3] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben4": Menüs.freigeschaltetFarben[4] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben5": Menüs.freigeschaltetFarben[5] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben6": Menüs.freigeschaltetFarben[6] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben7": Menüs.freigeschaltetFarben[7] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben8": Menüs.freigeschaltetFarben[8] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben9": Menüs.freigeschaltetFarben[9] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben10": Menüs.freigeschaltetFarben[10] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben11": Menüs.freigeschaltetFarben[11] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben12": Menüs.freigeschaltetFarben[12] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben13": Menüs.freigeschaltetFarben[13] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben14": Menüs.freigeschaltetFarben[14] = bool.Parse(Wert); break;

                    case "performancemode": RendernSpielfeld.performancemode = bool.Parse(Wert); break;
                    case "coins": Program.coins = int.Parse(Wert); break;
                    case "xp": Program.xp = int.Parse(Wert); break;
                    case "gesamtcoins": Program.gesamtcoins = int.Parse(Wert); break;
                    case "highscore": Program.highscore = int.Parse(Wert); break;
                    case "spieleGesamt": Program.spieleGesamt = int.Parse(Wert); break;

                    case "difficulty": Spiellogik.difficulty = Wert; break;
                    case "gamemode": Spiellogik.gamemode = Wert; break;
                    case "multiplayer": Spiellogik.multiplayer = bool.Parse(Wert); break;

                    case "rand": Spiellogik.rand = char.Parse(Wert); break;
                    case "food": Spiellogik.food = char.Parse(Wert); break;
                    case "player.Skin": Spiellogik.player.Skin = char.Parse(Wert); break;
                    case "player2.Skin": Spiellogik.player2.Skin = char.Parse(Wert); break;

                    case "randfarbe": Spiellogik.randfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "foodfarbe": Spiellogik.foodfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "player.Farbe": Spiellogik.player.Farbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "player2.Farbe": Spiellogik.player2.Farbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "player.Headfarbe": Spiellogik.player.Headfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "player2.Headfarbe": Spiellogik.player2.Headfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "Sound": Musik.soundplay = bool.Parse(Wert); break;
                    case "Musik": Musik.musikplay = bool.Parse(Wert); break;
                }
            }
        }
    }
}