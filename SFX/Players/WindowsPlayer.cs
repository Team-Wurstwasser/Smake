using Smake.Interfaces;
using Smake.SFX.Utils;
using System.Diagnostics;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Smake.SFX.Players
{
    internal class WindowsPlayer : IPlayer
    {
        private readonly string _alias = "smake_" + Guid.NewGuid().ToString("N");

        private Timer? _playbackTimer;
        private Stopwatch? _playStopwatch;
        private string? _fileName;
        private bool _loop;

        public event EventHandler? PlaybackFinished;

        public bool Playing { get; private set; }

        public async Task Play(string fileName, bool loop = false)
        {
            _loop = loop;
            FileUtil.ClearTempFiles();
            _fileName = $"\"{FileUtil.CheckFileToPlay(fileName)}\"";

            await StartPlayback();
        }

        private async Task StartPlayback()
        {
            _playbackTimer = new Timer
            {
                AutoReset = false
            };
            _playStopwatch = new Stopwatch();

            await CloseOwnDevice();

            await WindowsUtil.ExecuteMciCommand($"Open {_fileName} Alias {_alias}");
            await WindowsUtil.ExecuteMciCommand($"Status {_alias} Length", _playbackTimer);
            await WindowsUtil.ExecuteMciCommand($"Play {_alias}");
            Playing = true;
            _playbackTimer.Elapsed += HandlePlaybackFinished;
            _playbackTimer.Start();
            _playStopwatch.Start();
        }

        public async Task Stop()
        {
            _loop = false;

            if (Playing)
            {
                await WindowsUtil.ExecuteMciCommand($"Stop {_alias}");
                Playing = false;
                _playbackTimer?.Stop();
                _playStopwatch?.Stop();
                FileUtil.ClearTempFiles();
            }

            await CloseOwnDevice();
        }

        private async Task CloseOwnDevice()
        {
            try
            {
                await WindowsUtil.ExecuteMciCommand($"Close {_alias}");
            }
            catch
            {

            }
        }

        private async void HandlePlaybackFinished(object? sender, ElapsedEventArgs e)
        {
            Playing = false;
            _playbackTimer?.Dispose();
            _playbackTimer = null;

            if (_loop && _fileName != null)
            {
                await StartPlayback();
                return;
            }

            PlaybackFinished?.Invoke(this, e);
            await CloseOwnDevice();
        }
    }
}