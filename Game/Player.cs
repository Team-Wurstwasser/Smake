using Smake.Enums;
using Smake.Speicher;
using Smake.Values;
using Smake.Game.Struct;

namespace Smake.Game
{
    public class Player(Position startPosition, int tailStartLaenge)
    {
        public Position Richtung { get; set; }
        public bool Aenderung;

        // Ein einzelnes Array für alle Segmentpositionen
        public Position[] Positionen { get; private set; } = new Position[(GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2)];

        public bool IstKollidiert { get; set; }
        public int Punkte;
        public string? Name;

        public char HeadSkin;
        public char TailSkin;
        public ConsoleColor HeadFarbe;
        public ConsoleColor TailFarbe;

        public Position StartPosition { get; } = startPosition;

        public int TailLaenge { get; private set; }
        readonly int tailStartLaenge = tailStartLaenge;

        void InitialisiereSpieler()
        {
            RenderSpielfeld.Grid[Positionen[0].Y, Positionen[0].X] = HeadSkin;
        }

        public void Neustart()
        {
            IstKollidiert = false;
            TailLaenge = tailStartLaenge;
            Punkte = 0;

            // Array mit Invalid-Koordinaten (-1, -1) füllen
            Array.Fill(Positionen, Position.Invalid);

            Positionen[0] = StartPosition;

            HeadSkin = TailSkin;
            Richtung = new Position(0, 0);
            Aenderung = true;

            InitialisiereSpieler();
        }

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

        void Bewegung(int newPlayerX, int newPlayerY)
        {
            if (IstKollidiert && (Spielvalues.Gamemode == Gamemodes.Babymode || Spielvalues.Gamemode == Gamemodes.BabymodeUnendlich))
            {
                if (Richtung.X == 1) newPlayerX = 2;
                else if (Richtung.X == -1) newPlayerX = Spielvalues.weite - 3;
                else if (Richtung.Y == -1) newPlayerY = Spielvalues.hoehe - 2;
                else if (Richtung.Y == 1) newPlayerY = 1;
            }

            RenderSpielfeld.Grid[newPlayerY, newPlayerX] = HeadSkin;
            Positionen[0] = new Position(newPlayerX, newPlayerY);
        }

        void TailShift()
        {
            TailLaenge = Punkte + tailStartLaenge;

            for (int i = TailLaenge + 1; i > 0; i--)
            {
                Positionen[i] = Positionen[i - 1];
            }
        }

        void TailBewegung()
        {
            Position oldTail = Positionen[TailLaenge + 1];

            // Spieler-Tail zeichnen
            for (int i = 0; i <= TailLaenge; i++)
            {
                if (Positionen[i].X >= 0 && Positionen[i].Y >= 0)
                {
                    RenderSpielfeld.Grid[Positionen[i].Y, Positionen[i].X] = TailSkin;
                }
            }

            // Prüfen, ob das alte Tail-Feld noch auf einem Player-Segment liegt
            bool isOnPlayer = false;
            for (int i = 0; i <= TailLaenge; i++)
            {
                if (Positionen[i].X == oldTail.X && Positionen[i].Y == oldTail.Y)
                {
                    isOnPlayer = true;
                    break;
                }
            }

            // Altes Tail-Feld leeren
            if (oldTail.X >= 0 && oldTail.Y >= 0 && RenderSpielfeld.Grid[oldTail.Y, oldTail.X] != Skinvalues.RandSkin && !isOnPlayer)
            {
                RenderSpielfeld.Grid[oldTail.Y, oldTail.X] = ' ';
            }
        }
    }
}