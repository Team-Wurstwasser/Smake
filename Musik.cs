namespace Smake.io
{
    using System.Media;
    using Smake.io.Spiel;
    using Smake.io.Speicher;

    public class Musik
    {
        public static bool musikplay;
        public static int currentmusik = 0;
        public static bool soundplay;

        static int lastmusik = -1;

        public static void Melodie()
        {
            bool musikda = false;


            while (!Spiellogik.exit)
            {
                string dateipfad = Path.Combine("Sounds", GameData.Filenames[currentmusik]);
                SoundPlayer musik = new(dateipfad);

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
