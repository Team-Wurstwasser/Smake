namespace Smake.Game.Struct
{
    public readonly struct Position(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        public static Position Invalid => new(-1, -1);
    }
}
