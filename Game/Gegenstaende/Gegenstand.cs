using Smake.Values;
using Smake.Game.Struct;

namespace Smake.Game.Gegenstaende
{
    public class Gegenstand
    {
        private static readonly Random Rand = new();

        public Position Pos { get; private set; }

        public char Skin;

        public Gegenstand(char skin)
        {
            Skin = skin;
            Setze();
            Zeichne();
        }

        protected virtual void Setze()
        {
            int x, y;

            do
            {
                x = Rand.Next(1, Spielvalues.weite - 2);
                if (x % 2 != 0 && x < Spielvalues.weite - 2)
                    x++;

                y = Rand.Next(1, Spielvalues.hoehe - 2);
            } while (RenderSpielfeld.Grid[y, x] != ' ');

            Pos = new Position(x, y);
        }

        protected virtual void Zeichne()
        {
            RenderSpielfeld.Grid[Pos.Y, Pos.X] = Skin;
        }
    }
}