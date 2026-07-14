using Smake.Enums;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Game
{
    public class Player(int StartX, int StartY, int tailStartLaenge)
    {
        // Eingabe-Richtung (durch Pfeiltasten)
        public int InputX;

        public int InputY;

        public bool Aenderung;

        // Position des Spielers (Startkoordinaten)
        public int[] PlayerX { get; private set; } = new int[(GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2)];

        public int[] PlayerY { get; private set; } = new int[(GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2)];

        // Kollisionsstatus (wird nun von außen gesetzt)
        public bool IstKollidiert { get; set; }

        //Punkte des Spielers
        public int Punkte;

        // Namen der Spieler
        public string? Name;

        // Aussehen des Spielers
        public char HeadSkin;
        public char TailSkin;

        public ConsoleColor HeadFarbe;
        public ConsoleColor TailFarbe;

        public readonly int StartX = StartX;
        public readonly int StartY = StartY;

        // Länge des Spielers
        public int TailLaenge { get; private set; }
        readonly int tailStartLaenge = tailStartLaenge;

        void InitialisiereSpieler()
        {
            // Spielerzeichen auf Startposition setzen
            RenderSpielfeld.Grid[PlayerY[0], PlayerX[0]] = HeadSkin;
        }

        public void Neustart()
        {
            IstKollidiert = false;

            // Taillängen zurücksetzen
            TailLaenge = tailStartLaenge;

            // Punkte zurücksetzen
            Punkte = 0;

            // Arrays zurücksetzen
            Array.Fill(PlayerX, -1);
            Array.Fill(PlayerY, -1);

            // Spieler-Positionen auf Startwerte setzen
            PlayerX[0] = StartX;
            PlayerY[0] = StartY;

            // Aussehen einstellen
            HeadSkin = TailSkin;

            // Alle Eingabewerte zurücksetzen
            InputX = 0;
            InputY = 0;
            Aenderung = true;

            InitialisiereSpieler();
        }

        // Erwartet nun die vorberechneten Zielkoordinaten
        public (bool spielerTot, bool Maxpunkte) Update(int newPlayerX, int newPlayerY, Player p)
        {
            if (!IstKollidiert || Spielvalues.Gamemode == Gamemodes.Babymode || Spielvalues.Gamemode == Gamemodes.BabymodeUnendlich)
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
                if (IstKollidiert && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich)
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
                if (IstKollidiert)
                    SpielerTot = true;
                else if (Punkte >= GameData.MaxPunkte)
                    Maxpunkte = true;
            }

            return (SpielerTot, Maxpunkte);
        }

        // Bewegt die Spieler
        void Bewegung(int newPlayerX, int newPlayerY)
        {
            // Babymode Wrap-around
            if (IstKollidiert && (Spielvalues.Gamemode == Gamemodes.Babymode || Spielvalues.Gamemode == Gamemodes.BabymodeUnendlich))
            {
                if (InputX == 1) newPlayerX = 2;
                else if (InputX == -1) newPlayerX = Spielvalues.weite - 3;
                else if (InputY == -1) newPlayerY = Spielvalues.hoehe - 2;
                else if (InputY == 1) newPlayerY = 1;
            }

            // Kopf setzen
            RenderSpielfeld.Grid[newPlayerY, newPlayerX] = HeadSkin;

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
                    RenderSpielfeld.Grid[PlayerY[i], PlayerX[i]] = TailSkin;
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
            if (oldTailX >= 0 && oldTailY >= 0 && RenderSpielfeld.Grid[oldTailY, oldTailX] != Skinvalues.RandSkin && !isOnPlayer)
            {
                RenderSpielfeld.Grid[oldTailY, oldTailX] = ' ';
            }
        }
    }
}