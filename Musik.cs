namespace Smake.io
{
    using System.Media;
    using Smake.io.Spiel;

    public class Musik
    {
        public static bool musikplay = true;
        public static string[] filenames = ["Smake.wav", "Smake2.wav"];
        public static int currentmusik = 0;
        public static bool soundplay;

        static int lastmusik = -1;

        public static void Melodie()
        {
            bool musikda = false;


            while (!Spiellogik.exit)
            {
                SoundPlayer musik = new(filenames[currentmusik]);

                if (musikplay)
                {

                    if (!musikda || currentmusik != lastmusik)
                    {
                        musik.PlayLooping();

                        musikda = true;
                        lastmusik = currentmusik;
                    }
                }
                else
                {
                    musik.Stop();
                    musikda = false;
                    lastmusik = -1;

                }

                Thread.Sleep(100);
            }
        }
    }
}
