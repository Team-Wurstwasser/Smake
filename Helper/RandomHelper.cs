namespace Smake.Helper
{
    public static class RandomHelper
    {
        static readonly Random random = new();

        public static int Next(int maxValue) => random.Next(maxValue);
        public static int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);
    }
}
