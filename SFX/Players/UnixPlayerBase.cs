using Smake.Interfaces;
using Smake.SFX.Utils;
using System.Diagnostics;

namespace Smake.SFX.Players
{
    internal abstract class UnixPlayerBase : IPlayer
    {
        private Process? _process = null;
        private string? _fileName;
        private bool _loop;

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
            _process.ErrorDataReceived += HandlePlaybackFinished;
            _process.Disposed += HandlePlaybackFinished;
            Playing = true;

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
                _process.ErrorDataReceived -= HandlePlaybackFinished;
                _process.Disposed -= HandlePlaybackFinished;

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

            if (_loop && _fileName != null)
            {
                await StartPlayback();
                return;
            }

            PlaybackFinished?.Invoke(this, e);
        }
    }
}