using Smake.Values;

namespace Smake.Game.Gegenstaende
{
    public class Gegenstand
    {
        private static readonly Random Rand = new();

        public int X { get; private set; }
        public int Y { get; private set; }

        public char Skin;

        public Gegenstand(char skin) 
        {
            Skin = skin;
            Setze();
            Zeichne();
        }

        // Setzt das Gegendstand an eine zufällige, freie Position
        protected virtual void Setze()
        {
            int x, y;

            do
            {
                // Zufalls-X (immer gerade Zahl, damit zur Snake passt)
                x = Rand.Next(1, Spielvalues.weite - 2);
                if (x % 2 != 0 && x < Spielvalues.weite - 2)
                    x++;

                // Zufalls-Y
                y = Rand.Next(1, Spielvalues.hoehe - 2);

                // Wiederholen solange die Stelle nicht frei ist
            } while (RenderSpielfeld.Grid[y, x] != ' ');

            // Setze Position
            X = x;
            Y = y;
        }

        protected virtual void Zeichne()
        {
            // Gegendstand ins Spielfeld einzeichnen
            RenderSpielfeld.Grid[Y, X] = Skin;
        }
    }
}
