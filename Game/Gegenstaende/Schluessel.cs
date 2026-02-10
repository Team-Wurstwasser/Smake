using Smake.Values;
using Smake.SFX;

namespace Smake.Game.Gegenstaende
{
    public class Schluessel() : Gegenstand(Skinvalues.SchluesselSkin)
    {
        public bool Collected;

        public void EsseSchluessel(Player p)
        {
            if (!Collected)
            {
                // Überprüfe jedes Segment des Spielers
                for (int i = 0; i < p.TailLaenge; i++)
                {
                    if (p.PlayerX[i] == X && p.PlayerY[i] == Y)
                    {
                        Collected = true;

                        Sounds.Playbeep();

                        // Wenn Schlussel gefunden, können wir die Schleife abbrechen
                        break;
                    }
                }
                Zeichne();
            }
        }
    }
}
