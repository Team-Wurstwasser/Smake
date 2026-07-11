using System.Runtime.InteropServices;
using System.Text;
using Timer = System.Timers.Timer;

namespace Smake.SFX.Utils
{
    internal static partial class WindowsUtil
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetShortPathName(
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder shortPath,
            uint shortPathLength);

        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        private static extern int mciSendString(
            string command,
            StringBuilder stringReturn,
            int returnLength,
            IntPtr hwndCallback);

        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        private static extern int mciGetErrorString(
            int errorCode,
            StringBuilder errorText,
            int errorTextSize);

        public static Task ExecuteMciCommand(string commandString, Timer? playbackTimer = null)
        {
            var sb = new StringBuilder();

            var result = mciSendString(commandString, sb, 1024 * 1024, IntPtr.Zero);

            if (result != 0)
            {
                var errorSb = new StringBuilder(
                    $"Error executing MCI command '{commandString}'. Error code: {result}.");
                var sb2 = new StringBuilder(128);

                _ = mciGetErrorString(result, sb2, 128);
                errorSb.Append($" Message: {sb2}");

                throw new Exception(errorSb.ToString());
            }

            if (playbackTimer != null &&
                int.TryParse(sb.ToString(), out var length))
                playbackTimer.Interval = length;

            return Task.CompletedTask;
        }

        internal static bool TryGetShortPath(string path, out string shortPath)
        {
            shortPath = string.Empty;

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return false;

            var initialLength = 260u;
            var buffer = new StringBuilder((int)initialLength);
            var result = GetShortPathName(path, buffer, initialLength);
            if (result > initialLength)
            {
                buffer = new StringBuilder((int)result);
                result = GetShortPathName(path, buffer, result);
            }

            if (result == 0)
                return false;

            shortPath = buffer.ToString();
            return !string.IsNullOrWhiteSpace(shortPath);
        }
    }
}