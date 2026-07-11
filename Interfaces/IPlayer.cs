namespace Smake.Interfaces
{
    public interface IPlayer
    {
        event EventHandler PlaybackFinished;

        bool Playing { get; }

        Task Play(string fileName, bool loop = false);
        Task Stop();
    }
}
