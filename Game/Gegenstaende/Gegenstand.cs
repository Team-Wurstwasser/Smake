using Smake.Helper;

namespace Smake.Game.Gegenstaende
{
    public class Gegenstand
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public char Skin { get; }

        protected readonly char[,] grid;

        public Gegenstand(char[,] grid, char skin)
        {
            this.grid = grid;
            Skin = skin;
            Setze();
        }

        protected virtual void Setze()
        {
            int x, y;
            do
            {
                x = RandomHelper.Next(1, grid.GetLength(1) - 2);
                if (x % 2 != 0) x++;
                y = RandomHelper.Next(1, grid.GetLength(0) - 2);
            } while (grid[y, x] != ' ');

            X = x; Y = y;
            Zeichne();
        }

        protected virtual void Zeichne() => grid[Y, X] = Skin;
    }
}