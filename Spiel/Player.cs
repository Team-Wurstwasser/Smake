using System.ComponentModel;
using System.Numerics;

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
        public bool KollisionRand;

        public bool KollisionPlayer;

        // Länge des Spielers

        public int Tail;

        //Punkte des Spielers
        public int Punkte;

        // Namen der Spieler
        public string? Name;

        // Aussehen des Spielers
        public char Head;

        public char Skin;

        public ConsoleColor Farbe;

        public ConsoleColor Headfarbe;

        // Shop variablen
        public int Skinzahl;

        public int Farbezahl;

        public int Headfarbezahl;

        private int xstart = xstart;
        private int ystart = ystart;

        private void InitialisiereSpieler()
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
                PlayerX = new int[Spiellogik.maxpunkte + Tail + 2];
                PlayerY = new int[Spiellogik.maxpunkte + Tail + 2];
            }
            else
            {
                PlayerX = new int[Spiellogik.weite * Spiellogik.hoehe];
                PlayerY = new int[Spiellogik.weite * Spiellogik.hoehe];
            }


            // Arrays zurücksetzen
            Array.Clear(PlayerX, 0, PlayerX.Length);
            Array.Clear(PlayerY, 0, PlayerY.Length);

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

        public void Update()
        {

            // Neue Zielkoordinaten berechnen

            int newPlayerX = PlayerX[0] + 2 * InputX;
            int newPlayerY = PlayerY[0] + InputY;

            Kollision(newPlayerX, newPlayerY);
            TailShift();
            Bewegung(newPlayerX, newPlayerY);
            EsseFutter();
        }

        // Prüft die Kollision
        private void Kollision(int x, int y)
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
            else if (Spiellogik.grid[y, x] != ' ' || Spiellogik.grid[y, x] != Spiellogik.food || Spiellogik.grid[y, x] != Head || Spiellogik.grid[y, x] != Spiellogik.rand)
            {
                KollisionPlayer = true;
                KollisionRand = false;
            }
        }

        // Tailkoordinaten berechnen
        private void TailShift()
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
        private void Bewegung(int x, int y)
        {
            // Wenn das Zielfeld leer ist (kein Hindernis), bewege den Spieler
            if (Spiellogik.gamemode != "Babymode")
            {
                if (!KollisionPlayer && !KollisionRand)
                {
                    for (int i = 0; i <= Tail; i++)       // Tail des Spielers Zeichnen
                    {
                        Spiellogik.grid[PlayerY[i], PlayerX[i]] = Skin;
                    }
                    Spiellogik.grid[PlayerY[Tail + 1], PlayerX[Tail + 1]] = ' ';        // Altes Feld leeren

                    Spiellogik.grid[y, x] = Head;  // Spieler auf neues Feld setzen

                    PlayerX[0] = x;
                    PlayerY[0] = y;
                }
                else
                {
                    return;
                }

            }
            else
            {

                if (KollisionRand)
                {
                    if (InputX == 1)
                    {
                        x = 2;
                    }
                    else if (InputX == -1)
                    {
                        x = Spiellogik.weite - 3;
                    }
                    else if (InputY == -1)
                    {
                        y = Spiellogik.hoehe - 2;
                    }
                    else if (InputY == 1)
                    {
                        y = 1;
                    }
                }
            }

            for (int i = 0; i <= Tail; i++)       // Tail des Spielers Zeichnen
            {
                Spiellogik.grid[PlayerY[i], PlayerX[i]] = Skin;
            }
            
            Spiellogik.grid[PlayerY[Tail + 1], PlayerX[Tail + 1]] = ' ';        // Altes Feld leeren

            Spiellogik.grid[y, x] = Head;  // Spieler auf neues Feld setzen

            PlayerX[0] = x;
            PlayerY[0] = y;

        }

        // Der Spieler isst das Futter
        private void EsseFutter()
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

        public (bool spielerTot, bool gegnerTot) Gameover()
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
                else if (Punkte >= Spiellogik.maxpunkte)
                    gegnerTot = true;
            }
            else if (Spiellogik.gamemode == "Babymode")
            {
                if (Punkte >= Spiellogik.maxpunkte)
                {
                    gegnerTot = true;
                }
            }

            return (spielerTot, gegnerTot);
        }

    }
}
