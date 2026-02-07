using Smake.Spieler;
using Smake.Values;
using Smake.SFX;

namespace Smake.Gegenstaende
{
    public class Schluessel : Gegenstand
    {
        public bool Collected = false;

        public Schluessel() : base(Skinvalues.SchluesselSkin) {}

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
