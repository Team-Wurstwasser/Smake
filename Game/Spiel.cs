using Smake.Game.Spieler;
using Smake.Speicher;

namespace Smake.Game
{
    public class Spiel
    {
        public bool Game { get; set; }
        public readonly Player[] Player = [];
        public readonly Spiellogik Logik = new(this);
        public static readonly string[] Name = [];
    }
}
