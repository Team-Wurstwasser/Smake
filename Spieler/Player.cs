using Smake.io.Speicher;
using Smake.io.Spiel;
using Smake.io.Values;
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
        public int[] PlayerX;

        public int[] PlayerY;

        // Kollisionsvariablen
        bool KollisionRand;

        bool KollisionPlayer;

        //Punkte des Spielers
        public int Punkte;

        // Namen der Spieler
        public readonly string? Name = Name;

        // Aussehen des Spielers
        public char HeadSkin;

        public ConsoleColor HeadFarbe;

        int xstart = xstart;
        int ystart = ystart;

        void InitialisiereSpieler()
        {
            // Spielerzeichen auf Startposition setzen
            Spiellogik.grid[PlayerY[0], PlayerX[0]] = HeadSkin;
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
            if (Spielvalues.gamemode == "Normal" || Spielvalues.gamemode == "Babymode")
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

        public (bool spielerTot, bool Maxpunkte) Update()
        {

            // Neue Zielkoordinaten berechnen

            int newPlayerX = PlayerX[0] + 2 * InputX;
            int newPlayerY = PlayerY[0] + InputY;

            Kollision(newPlayerX, newPlayerY);
            if(!KollisionPlayer && !KollisionRand || Spielvalues.gamemode == "Babymode")
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
            bool spielerTot = false;
            bool Maxpunkte = false;

            if (Spielvalues.gamemode == "Unendlich")
            {
                if (KollisionPlayer || KollisionRand)
                    spielerTot = true;
            }
            else if (Spielvalues.gamemode == "Normal")
            {
                if (KollisionPlayer || KollisionRand)
                    spielerTot = true;
                else if (Punkte >= GameData.MaxPunkte)
                    Maxpunkte = true;
            }
            else if (Spielvalues.gamemode == "Babymode")
            {
                if (Punkte >= GameData.MaxPunkte)
                {
                    Maxpunkte = true;
                }
            }
            return (spielerTot, Maxpunkte);

        }

        // Prüft die Kollision
        void Kollision(int x, int y)
        {
            if (Spiellogik.grid[y, x] == ' ' || Spiellogik.grid[y, x] == Skinvalues.food || x == PlayerX[0] && y == PlayerY[0])
            {
                return;
            }
            else if (Spiellogik.grid[y, x] == Skinvalues.rand)
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
        void Bewegung(int x, int y)
        {
            // Babymode Wrap-around
            if (Spielvalues.gamemode == "Babymode")
            {
                if (KollisionRand)
                {
                    if (InputX == 1) x = 2;
                    else if (InputX == -1) x = Spielvalues.weite - 3;
                    else if (InputY == -1) y = Spielvalues.hoehe - 2;
                    else if (InputY == 1) y = 1;
                }
            }

            // Kopf setzen
            Spiellogik.grid[y, x] = HeadSkin;

            // Spieler-Koordinaten aktualisieren
            PlayerX[0] = x;
            PlayerY[0] = y;

        }
    }
}
