using System.Media;
using System.IO;
using System.Threading;
using Smake.io.Spiel;
using Smake.io.Speicher;

namespace Smake.io
{
    public class Musik
    {
        public static bool musikplay;
        public static int currentmusik = 0;
        public static bool soundplay;

        private static SoundPlayer? currentPlayer;
        private static int lastmusik = -1;
        private static readonly object lockObj = new();

        public static void Melodie()
        {
            while (!Spiellogik.exit)
            {
                lock (lockObj)
                {
                    // Prüfen, ob wir die Musik wechseln oder starten müssen
                    if (musikplay)
                    {
                        if (currentmusik != lastmusik)
                        {
                            // Stoppe vorherige Musik, falls vorhanden
                            currentPlayer?.Stop();

                            // Erstelle neuen SoundPlayer für die aktuelle Musik
                            string dateipfad = Path.Combine("Sounds", GameData.Filenames[currentmusik]);
                            if (File.Exists(dateipfad))
                            {
                                currentPlayer = new SoundPlayer(dateipfad);
                                currentPlayer.PlayLooping();
                                lastmusik = currentmusik;
                            }
                        }
                    }
                    else
                    {
                        // Musik stoppen, wenn play deaktiviert ist
                        if (currentPlayer != null)
                        {
                            currentPlayer.Stop();
                            currentPlayer = null;
                            lastmusik = -1;
                        }
                    }
                }

                Thread.Sleep(100); // CPU schonen
            }

            // Beim Beenden sicherstellen, dass Musik gestoppt wird
            currentPlayer?.Stop();
        }
    }
}
