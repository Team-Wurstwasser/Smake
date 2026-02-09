using Smake.Enums;
using Smake.Render;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Game.Spieler
{
    public class Player(int StartX, int StartY, char Tailskin, ConsoleColor HeadFarbe, ConsoleColor TailFarbe)
    {
        // Eingabe-Richtung (durch Pfeiltasten)
        public int InputX;

        public int InputY;

        public bool Aenderung = true;

        // Position des Spielers (Startkoordinaten)
        public int[] PlayerX { get; private set; } = new int[(GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2)];

        public int[] PlayerY { get; private set; } = new int[(GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2)];

        // Kollisionsvariablen
        bool Kollision;

        //Punkte des Spielers
        public int Punkte;

        // Aussehen des Spielers  
        public readonly char TailSkin = Tailskin;
        public char HeadSkin = Tailskin;

        public readonly ConsoleColor HeadFarbe = HeadFarbe;
        public readonly ConsoleColor TailFarbe = TailFarbe;

        public readonly int StartX = StartX;
        public readonly int StartY = StartY;

        // Länge des Spielers
        public int TailLaenge { get; private set; } = GameData.TailStartLaenge;

        public void Start()
        {
            // Arrays zurücksetzen
            Array.Fill(PlayerX, -1);
            Array.Fill(PlayerY, -1);

            // Spieler-Positionen auf Startwerte setzen
            PlayerX[0] = StartX;
            PlayerY[0] = StartY;

            // Spielerzeichen auf Startposition setzen
            RendernSpielfeld.Grid[PlayerY[0], PlayerX[0]] = HeadSkin;
        }

        public (bool spielerTot, bool Maxpunkte) Update(Player p)
        {
            // Neue Zielkoordinaten berechnen
            int newPlayerX = PlayerX[0] + 2 * InputX;
            int newPlayerY = PlayerY[0] + InputY;

            Kollisioncheck(newPlayerX, newPlayerY, p);
            if (!Kollision || Spielvalues.Gamemode == Gamemodes.Babymode || Spielvalues.Gamemode == Gamemodes.BabymodeUnendlich)
            {
                TailShift();
                TailBewegung();
                Bewegung(newPlayerX, newPlayerY);

                foreach (var Futter in Spiellogik.Essen)
                {
                    Futter.EsseFutter(this);
                }
            }
            return GameoverChecker(p);
        }

        (bool spielerTot, bool Maxpunkte) GameoverChecker(Player p)
        {
            bool SpielerTot = false;
            bool Maxpunkte = false;

            if (Spielvalues.Gamemode == Gamemodes.Unendlich || Spielvalues.Gamemode == Gamemodes.BabymodeUnendlich)
            {
                if (Kollision && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich)
                    SpielerTot = true;
                else if (TailLaenge >= (GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2) - Spielvalues.Maxfutter - 1 && !Spielvalues.Multiplayer)
                    Maxpunkte = true;
                else if (TailLaenge + p.TailLaenge >= (GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2) - Spielvalues.Maxfutter - 2 && Spielvalues.Multiplayer)
                    Maxpunkte = true;
            }
            else if (Spielvalues.Gamemode == Gamemodes.Babymode)
            {
                if (Punkte >= GameData.MaxPunkte)
                {
                    Maxpunkte = true;
                }
            }
            else
            {
                if (Kollision)
                    SpielerTot = true;
                else if (Punkte >= GameData.MaxPunkte)
                    Maxpunkte = true;
            }

            return (SpielerTot, Maxpunkte);

        }

        // Prüft die Kollision
        void Kollisioncheck(int newPlayerX, int newPlayerY, Player p)
        {
            if (Spielvalues.Gamemode == Gamemodes.Babymode || Spielvalues.Gamemode == Gamemodes.BabymodeUnendlich)
            {
                if (RendernSpielfeld.Grid[newPlayerY, newPlayerX] == Skinvalues.RandSkin)
                {
                    Kollision = true;
                }
                else
                {
                    Kollision = false;
                }

            }
            else
            {
                if (Spielvalues.Multiplayer)
                {
                    int newPlayer2X = p.PlayerX[0] + 2 * p.InputX;
                    int newPlayer2Y = p.PlayerY[0] + p.InputY;

                    if (newPlayerX == newPlayer2X && newPlayerY == newPlayer2Y)
                    {
                        Kollision = true;
                        return;
                    }

                }

                if (RendernSpielfeld.Grid[newPlayerY, newPlayerX] == ' ' || RendernSpielfeld.Grid[newPlayerY, newPlayerX] == Skinvalues.FoodSkin || newPlayerX == PlayerX[0] && newPlayerY == PlayerY[0] || RendernSpielfeld.Grid[newPlayerY, newPlayerX] == Skinvalues.SchluesselSkin)
                {
                    Kollision = false;
                }
                else
                {
                    // Wenn das Feld nicht leer, nicht Food, nicht Schlüssel, nicht Head, nicht Rand
                    Kollision = true;

                }
            }
        }

        // Bewegt die Spieler
        void Bewegung(int newPlayerX, int newPlayerY)
        {
            // Babymode Wrap-around
            if (Spielvalues.Gamemode == Gamemodes.Babymode && Kollision || Spielvalues.Gamemode == Gamemodes.BabymodeUnendlich && Kollision)
            {
                if (InputX == 1) newPlayerX = 2;
                else if (InputX == -1) newPlayerX = Spielvalues.weite - 3;
                else if (InputY == -1) newPlayerY = Spielvalues.hoehe - 2;
                else if (InputY == 1) newPlayerY = 1;
            }

            // Kopf setzen
            RendernSpielfeld.Grid[newPlayerY, newPlayerX] = HeadSkin;

            // Spieler-Koordinaten aktualisieren
            PlayerX[0] = newPlayerX;
            PlayerY[0] = newPlayerY;

        }

        // Tailkoordinaten berechnen
        void TailShift()
        {
            TailLaenge = Punkte + tailStartLaenge;

            for (int i = TailLaenge + 1; i > 0; i--)
            {
                PlayerX[i] = PlayerX[i - 1];
            }

            for (int i = TailLaenge + 1; i > 0; i--)
            {
                PlayerY[i] = PlayerY[i - 1];
            }

        }

        void TailBewegung()
        {
            int oldTailX = PlayerX[TailLaenge + 1];
            int oldTailY = PlayerY[TailLaenge + 1];

            // Spieler-Tail zeichnen
            for (int i = 0; i <= TailLaenge; i++)
            {
                if (PlayerX[i] >= 0 && PlayerY[i] >= 0)
                    RendernSpielfeld.Grid[PlayerY[i], PlayerX[i]] = TailSkin;
            }

            // Prüfen, ob das alte Tail-Feld noch auf einem Player-Segment liegt
            bool isOnPlayer = false;
            for (int i = 0; i <= TailLaenge; i++)
            {
                if (PlayerX[i] == oldTailX && PlayerY[i] == oldTailY)
                {
                    isOnPlayer = true;
                    break;
                }
            }

            // Altes Tail-Feld nur leeren, wenn es kein Rand und nicht auf einem Spielersegment ist
            if (oldTailX >= 0 && oldTailY >= 0 && RendernSpielfeld.Grid[oldTailY, oldTailX] != Skinvalues.RandSkin && !isOnPlayer)
            {
                RendernSpielfeld.Grid[oldTailY, oldTailX] = ' ';
            }
        }
    }
}
