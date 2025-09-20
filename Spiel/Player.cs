using System.ComponentModel;
using System.Numerics;
using Smake.io.Speicher;

namespace Smake.io.Spiel
{
    public class Spieler(int xstart, int ystart)
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

        // Länge des Spielers
        int Tail;

        //Punkte des Spielers
        public int Punkte;

        // Namen der Spieler
        public string? Name;

        // Aussehen des Spielers
        public char Head;

        public char Skin;

        public ConsoleColor Farbe;

        public ConsoleColor Headfarbe;

        int xstart = xstart;
        int ystart = ystart;

        void InitialisiereSpieler()
        {
            // Spielerzeichen auf Startposition setzen
            Spiellogik.grid[PlayerY[0], PlayerX[0]] = Head;
        }

        public void Neustart()
        {

            KollisionPlayer = false;
            KollisionRand = false;

            // Taillängen zurücksetzen
            Tail = 3;

            // Punkte zurücksetzen

            Punkte = 0;

            // Maximale Länge einstellen
            if (Spiellogik.gamemode == "Normal" || Spiellogik.gamemode == "Babymode")
            {
                PlayerX = new int[GameData.MaxPunkte + Tail + 2];
                PlayerY = new int[GameData.MaxPunkte + Tail + 2];
            }
            else
            {
                PlayerX = new int[Spiellogik.weite * Spiellogik.hoehe];
                PlayerY = new int[Spiellogik.weite * Spiellogik.hoehe];
            }


            // Arrays zurücksetzen
            Array.Fill(PlayerX, -1);
            Array.Fill(PlayerY, -1);

            // Spieler-Positionen auf Startwerte setzen
            PlayerX[0] = xstart;
            PlayerY[0] = ystart;

            // Aussehen einstellen
            Head = Skin;

            // Alle Eingabewerte zurücksetzen
            InputX = 0;
            InputY = 0;
            Aenderung = true;

            InitialisiereSpieler();
        }

        public (bool spielerTot, bool gegnerTot) Update()
        {

            // Neue Zielkoordinaten berechnen

            int newPlayerX = PlayerX[0] + 2 * InputX;
            int newPlayerY = PlayerY[0] + InputY;

            Kollision(newPlayerX, newPlayerY);
            TailShift();
            Bewegung(newPlayerX, newPlayerY);
            EsseFutter();

            return GameoverChecker();
        }


        (bool spielerTot, bool gegnerTot) GameoverChecker()
        {
            bool spielerTot = false;
            bool gegnerTot = false;

            if (Spiellogik.gamemode == "Unendlich")
            {
                if (KollisionPlayer || KollisionRand)
                    spielerTot = true;
            }
            else if (Spiellogik.gamemode == "Normal")
            {
                if (KollisionPlayer || KollisionRand)
                    spielerTot = true;
                else if (Punkte >= GameData.MaxPunkte)
                    gegnerTot = true;
            }
            else if (Spiellogik.gamemode == "Babymode")
            {
                if (Punkte >= GameData.MaxPunkte)
                {
                    gegnerTot = true;
                }
            }

            return (spielerTot, gegnerTot);
        }

        // Prüft die Kollision
        void Kollision(int x, int y)
        {
            if (Spiellogik.grid[y, x] == ' ' || Spiellogik.grid[y, x] == Spiellogik.food || Spiellogik.grid[y, x] == Head)
            {
                KollisionPlayer = false;
                KollisionRand = false;
            }
            else if (Spiellogik.grid[y, x] == Spiellogik.rand)
            {
                KollisionPlayer = false;
                KollisionRand = true;
            }
            else
            {
                // Wenn das Feld nicht leer, nicht Food, nicht Head, nicht Rand → Kollision mit Spieler
                KollisionPlayer = true;
                KollisionRand = false;
            }
        }


        // Tailkoordinaten berechnen
        void TailShift()
        {
            for (int i = PlayerX.Length - 1; i > 0; i--)
            {
                PlayerX[i] = PlayerX[i - 1];
            }

            for (int i = PlayerY.Length - 1; i > 0; i--)
            {
                PlayerY[i] = PlayerY[i - 1];
            }

        }

        // Bewegt die Spieler
        void Bewegung(int x, int y)
        {
            int oldTailX = PlayerX[Tail + 1];
            int oldTailY = PlayerY[Tail + 1];

            // Babymode Wrap-around
            if (Spiellogik.gamemode == "Babymode")
            {
                if (KollisionRand)
                {
                    if (InputX == 1) x = 2;
                    else if (InputX == -1) x = Spiellogik.weite - 3;
                    else if (InputY == -1) y = Spiellogik.hoehe - 2;
                    else if (InputY == 1) y = 1;
                }
            }
            else
            {
                // Standard: Bewegung nur wenn keine Kollision
                if (KollisionPlayer || KollisionRand)
                    return;
            }

            // Spieler-Tail zeichnen
            for (int i = 0; i <= Tail; i++)
            {
                if (PlayerX[i] >= 0 && PlayerY[i] >= 0)
                    Spiellogik.grid[PlayerY[i], PlayerX[i]] = Skin;
            }

            // Altes Tail-Feld leeren (nicht Rand)
            if (oldTailX >= 0 && oldTailY >= 0 && Spiellogik.grid[oldTailY, oldTailX] != Spiellogik.rand)
            {
                Spiellogik.grid[oldTailY, oldTailX] = ' ';
            }

            // Kopf setzen
            Spiellogik.grid[y, x] = Head;

            // Spieler-Koordinaten aktualisieren
            PlayerX[0] = x;
            PlayerY[0] = y;
        }

        // Der Spieler isst das Futter
        void EsseFutter()
        {
            // Spieler frisst Futter
            if (PlayerX[0] == Spiellogik.futterX && PlayerY[0] == Spiellogik.futterY)
            {
                Tail++;
                Punkte++;
                if (Musik.soundplay)
                {
                    Console.Beep(700, 100);
                }
                Spiellogik.SetzeFutter();
            }
        }
    }
}
