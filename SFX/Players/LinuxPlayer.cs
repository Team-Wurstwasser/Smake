using Smake.Interfaces;

namespace Smake.SFX.Players
{
    internal class LinuxPlayer : UnixPlayerBase, IPlayer
    {
        protected override string GetBashCommand(string fileName)
        {
            return Path.GetExtension(fileName).Equals(".mp3", StringComparison.OrdinalIgnoreCase)? "mpg123 -q" : "aplay -q";
        }
    }
}