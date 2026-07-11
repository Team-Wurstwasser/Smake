using Smake.Interfaces;
using Smake.SFX.Utils;
using System.Diagnostics;

namespace Smake.SFX.Players
{
    internal abstract class UnixPlayerBase : IPlayer
    {
        private static readonly TimeSpan MinPlaybackDuration = TimeSpan.FromMilliseconds(300);

        private Process? _process;
        private string? _fileName;
        private bool _loop;
        private readonly Stopwatch _playStopwatch = new();

        public event EventHandler? PlaybackFinished;

        public bool Playing { get; private set; }

        protected abstract string GetBashCommand(string fileName);

        public async Task Play(string fileName, bool loop = false)
        {
            _fileName = fileName;
            _loop = loop;

            await StartPlayback();
        }

        private async Task StartPlayback()
        {
            await KillCurrentProcess();

            var BashToolName = GetBashCommand(_fileName!);
            _process = BashUtil.StartBashProcess(
                $"{BashToolName} '{_fileName}'");
            _process.EnableRaisingEvents = true;
            _process.Exited += HandlePlaybackFinished;
            Playing = true;
            _playStopwatch.Restart();

            await Task.CompletedTask;
        }

        public async Task Stop()
        {
            _loop = false;
            await KillCurrentProcess();
            Playing = false;
        }

        private Task KillCurrentProcess()
        {
            if (_process != null)
            {
                _process.Exited -= HandlePlaybackFinished;

                if (!_process.HasExited)
                {
                    _process.Kill();
                }

                _process.Dispose();
                _process = null;
            }

            return Task.CompletedTask;
        }

        internal async void HandlePlaybackFinished(object? sender, EventArgs e)
        {
            if (!Playing)
                return;

            Playing = false;

            int exitCode = (sender as Process)?.ExitCode ?? 0;
            bool playbackFailed = exitCode != 0 || _playStopwatch.Elapsed < MinPlaybackDuration;

            if (playbackFailed)
            {
                _loop = false;

                PlaybackFinished?.Invoke(this, e);
                return;
            }

            if (_loop && _fileName != null)
            {
                await StartPlayback();
                return;
            }

            PlaybackFinished?.Invoke(this, e);
        }
    }
}