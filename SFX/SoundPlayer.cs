using Smake.Interfaces;
using Smake.SFX.Players;
using Smake.Speicher;
using System.Runtime.InteropServices;

namespace Smake.SFX
{
    public class SoundPlayer : IPlayer
    {
        public static bool NotSupported { get; private set; } = false;

        private readonly IPlayer _internalPlayer;

        public event EventHandler? PlaybackFinished;

        public bool Playing => _internalPlayer.Playing;

        public SoundPlayer()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _internalPlayer = new WindowsPlayer();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _internalPlayer = new LinuxPlayer();
            }
            else
            {
                NotSupported = true;
                _internalPlayer = new NullPlayer();
                return;
            }

            _internalPlayer.PlaybackFinished += OnPlaybackFinished;
        }

        public async Task Play(string fileName, bool loop = false)
        {
            await _internalPlayer.Play(fileName, loop);
        }

        public async Task Stop()
        {
            await _internalPlayer.Stop();
        }

        private void OnPlaybackFinished(object? sender, EventArgs e)
        {
            PlaybackFinished?.Invoke(this, e);
        }
    }
}