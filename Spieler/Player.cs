using Smake.io.Speicher;
using Smake.io.Spiel;
using Smake.io.Values;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Smake.io.Spieler
{
    public class Player(int xstart, int ystart, string? Name) : Tail
    {
        // Eingabe-Richtung (durch Pfeiltasten)
        public int InputX;

        public int InputY;

        public bool Aenderung;

        // Position des Spielers (Startkoordinaten)
        public int[] PlayerX { get; private set; } = [];

        public int[] PlayerY { get; private set; } = [];

        // Kollisionsvariablen
        bool KollisionRand;

        bool KollisionPlayer;

        //Punkte des Spielers
        public int Punkte { get; set; }

        // Namen der Spieler
        public readonly string? Name = Name;

        // Aussehen des Spielers
        public char HeadSkin;

        public ConsoleColor HeadFarbe;

        readonly int xstart = xstart;
        readonly int ystart = ystart;

        void InitialisiereSpieler()
        {
            // Spielerzeichen auf Startposition setzen
            Spiellogik.Grid[PlayerY[0], PlayerX[0]] = HeadSkin;
        }

        public void Neustart()
        {

            KollisionPlayer = false;
            KollisionRand = false;

            // Taillängen zurücksetzen
            TailLaenge = 3;

            // Punkte zurücksetzen

            Punkte = 0;

            // Maximale Länge einstellen
            if (Spielvalues.Gamemode == "Normal" || Spielvalues.Gamemode == "Babymode")
            {
                PlayerX = new int[GameData.MaxPunkte + TailLaenge + 2];
                PlayerY = new int[GameData.MaxPunkte + TailLaenge + 2];
            }
            else
            {
                PlayerX = new int[Spielvalues.weite * Spielvalues.hoehe];
                PlayerY = new int[Spielvalues.weite * Spielvalues.hoehe];
            }


            // Arrays zurücksetzen
            Array.Fill(PlayerX, -1);
            Array.Fill(PlayerY, -1);

            // Spieler-Positionen auf Startwerte setzen
            PlayerX[0] = xstart;
            PlayerY[0] = ystart;

            // Aussehen einstellen
            HeadSkin = TailSkin;

            // Alle Eingabewerte zurücksetzen
            InputX = 0;
            InputY = 0;
            Aenderung = true;

            InitialisiereSpieler();
        }

        public (bool spielerTot, bool Maxpunkte) Update(Player p)
        {
            // Neue Zielkoordinaten berechnen
            int newPlayerX = PlayerX[0] + 2 * InputX;
            int newPlayerY = PlayerY[0] + InputY;

            Kollision(newPlayerX, newPlayerY,p);
            if (!KollisionPlayer && !KollisionRand || Spielvalues.Gamemode == "Babymode")
            {
                TailShift(this);
                TailBewegung(this);
                Bewegung(newPlayerX, newPlayerY);

                foreach (var Futter in Spiellogik.Essen)
                {
                    Futter.EsseFutter(this);
                }
            }
            return GameoverChecker();
        }

        (bool spielerTot, bool Maxpunkte) GameoverChecker()
        {
            bool SpielerTot = false;
            bool Maxpunkte = false;

            if (Spielvalues.Gamemode == "Unendlich")
            {
                if (KollisionPlayer || KollisionRand)
                    SpielerTot = true;
            }
            else if (Spielvalues.Gamemode == "Normal")
            {
                if (KollisionPlayer || KollisionRand)
                    SpielerTot = true;
                else if (Punkte >= GameData.MaxPunkte)
                    Maxpunkte = true;
            }
            else if (Spielvalues.Gamemode == "Babymode")
            {
                if (Punkte >= GameData.MaxPunkte)
                {
                    Maxpunkte = true;
                }
            }
            return (SpielerTot, Maxpunkte);

        }

        // Prüft die Kollision
        void Kollision(int newPlayerX, int newPlayerY, Player p)
        {
            if(Spielvalues.Multiplayer && Spielvalues.Gamemode != "Babymode")
            {
                int newPlayer2X = p.PlayerX[0] + 2 * p.InputX;
                int newPlayer2Y = p.PlayerY[0] + p.InputY;

                if(newPlayerX == newPlayer2X && newPlayerY == newPlayer2Y)
                {
                    KollisionPlayer = true;
                    return;
                }

            }
            
            
            if (Spiellogik.Grid[newPlayerY, newPlayerX] == ' ' || Spiellogik.Grid[newPlayerY, newPlayerX] == Skinvalues.FoodSkin || newPlayerX == PlayerX[0] && newPlayerY == PlayerY[0])
            {
                KollisionRand = false;
                KollisionPlayer = false;
            }
            else if (Spiellogik.Grid[newPlayerY, newPlayerX] == Skinvalues.RandSkin)
            {
                KollisionRand = true;
            }
            else
            {
                // Wenn das Feld nicht leer, nicht Food, nicht Head, nicht Rand → Kollision mit Spieler
                KollisionPlayer = true;

            }
        }

        // Bewegt die Spieler
        void Bewegung(int newPlayerX, int newPlayerY)
        {
            // Babymode Wrap-around
            if (Spielvalues.Gamemode == "Babymode")
            {
                if (KollisionRand)
                {
                    if (InputX == 1) newPlayerX = 2;
                    else if (InputX == -1) newPlayerX = Spielvalues.weite - 3;
                    else if (InputY == -1) newPlayerY = Spielvalues.hoehe - 2;
                    else if (InputY == 1) newPlayerY = 1;
                }
            }

            // Kopf setzen
            Spiellogik.Grid[newPlayerY, newPlayerX] = HeadSkin;

            // Spieler-Koordinaten aktualisieren
            PlayerX[0] = newPlayerX;
            PlayerY[0] = newPlayerY;

        }
    }
}
