using Smake.Render;
using Smake.Speicher;
using Smake.Spiel;
using Smake.Values;

namespace Smake.Spieler
{
    public class Player(int xstart, int ystart) : Tail
    {
        // Eingabe-Richtung (durch Pfeiltasten)
        public int InputX;

        public int InputY;

        public bool Aenderung;

        // Position des Spielers (Startkoordinaten)
        public int[] PlayerX { get; private set; } = new int[(GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2)];

        public int[] PlayerY { get; private set; } = new int[(GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2)];

        // Kollisionsvariablen
        bool Kollision;

        //Punkte des Spielers
        public int Punkte { get; set; }

        // Namen der Spieler
        public string? Name;

        // Aussehen des Spielers
        public char HeadSkin;

        public ConsoleColor HeadFarbe;

        public readonly int xstart = xstart;
        public readonly int ystart = ystart;

        void InitialisiereSpieler()
        {
            // Spielerzeichen auf Startposition setzen
            RendernSpielfeld.Grid[PlayerY[0], PlayerX[0]] = HeadSkin;
        }

        public void Neustart()
        {
            Kollision = false;

            // Taillängen zurücksetzen
            TailLaenge = TailStartLaenge;

            // Punkte zurücksetzen
            Punkte = 0;

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

            Kollisioncheck(newPlayerX, newPlayerY, p);
            if (!Kollision || Spielvalues.GamemodeInt == 3 || Spielvalues.GamemodeInt == 4)
            {
                TailShift(this);
                TailBewegung(this);
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

            if (Spielvalues.GamemodeInt == 2 || Spielvalues.GamemodeInt == 4)
            {
                if (Kollision && Spielvalues.GamemodeInt != 4)
                    SpielerTot = true;
                else if (TailLaenge >= (GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2) - Spielvalues.Maxfutter - 1 && !Spielvalues.Multiplayer)
                    Maxpunkte = true;
                else if (TailLaenge + p.TailLaenge >= (GameData.Hoehe - 2) * ((GameData.Weite - 2) / 2) - Spielvalues.Maxfutter - 2 && Spielvalues.Multiplayer)
                    Maxpunkte = true;
            }
            else if (Spielvalues.GamemodeInt == 3)
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
            if (Spielvalues.GamemodeInt == 3 || Spielvalues.GamemodeInt == 4)
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
            if (Spielvalues.GamemodeInt == 3 && Kollision || Spielvalues.GamemodeInt == 4 && Kollision)
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
    }
}
