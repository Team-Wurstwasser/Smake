using Smake.Speicher;
using Smake.Values;

namespace Smake.Game
{
    public class RenderSpielfeld(int hoehe, int weite)
    {
        public char[,] Grid { get; set; } = new char[hoehe, weite];
        readonly char[,] PrevGrid = new char[hoehe, weite];
        readonly int Hoehe = hoehe;
        readonly int Weite = weite;

        // Initialisiert das Spielfeld: Rahmen und leere Fläche
        public void InitialisiereSpielfeld()
        {
            Console.Clear();
            Console.CursorVisible = false;

            for (int y = 0; y < Hoehe; y++)
            {
                for (int x = 0; x < Weite; x++)
                {
                    if (y == 0 || y == Hoehe - 1 || x == 0 || x >= Weite - 1)
                    {
                        Grid[y, x] = Skinvalues.RandSkin;
                        PrevGrid[y, x] = Skinvalues.RandSkin;
                    }
                    else
                    {
                        Grid[y, x] = ' ';
                        PrevGrid[y, x] = '\0';
                    }
                }
            }
            RenderRand();
        }

        private void RenderRand()
        {
            if (!Spielvalues.Performancemode)
                Console.ForegroundColor = Skinvalues.RandFarbe;
            else
                Console.ResetColor();

            // Oben und Unten
            for (int x = 0; x < Weite; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write(Skinvalues.RandSkin);
                Console.SetCursorPosition(x, Hoehe - 1);
                Console.Write(Skinvalues.RandSkin);
            }

            // Links und Rechts
            for (int y = 1; y < Hoehe - 1; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(Skinvalues.RandSkin);
                Console.SetCursorPosition(Weite - 1, y);
                Console.Write(Skinvalues.RandSkin);
            }
        }

        public void Render(Player[] players)
        {
            if (Spielvalues.Performancemode)
            {
                RenderPerformance(players);
            }
            else
            {
                RenderFull(players);
            }
        }

        private void RenderFull(Player[] players)
        {
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            for (int y = 1; y < Hoehe - 1; y++)
            {
                for (int x = 1; x < Weite - 1; x++)
                {
                    bool IstStartposition = players.Any(p => x == p.StartX && y == p.StartY);

                    if (Grid[y, x] != PrevGrid[y, x] || IstStartposition)
                    {
                        ConsoleColor neueFarbe = BestimmeFarbe(x, y, Grid[y, x], players);

                        if (neueFarbe != aktuelleFarbe)
                        {
                            Console.ForegroundColor = neueFarbe;
                            aktuelleFarbe = neueFarbe;
                        }

                        Console.SetCursorPosition(x, y);
                        Console.Write(Grid[y, x]);
                        PrevGrid[y, x] = Grid[y, x];
                    }
                }
                aktuelleFarbe = RenderLegende(y, aktuelleFarbe, players);
            }
        }

        private void RenderPerformance(Player[] players)
        {
            Console.ResetColor();
            for (int y = 1; y < Hoehe - 1; y++)
            {
                for (int x = 1; x < Weite - 1; x++)
                {
                    if (Grid[y, x] != PrevGrid[y, x])
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(Grid[y, x]);
                        PrevGrid[y, x] = Grid[y, x];
                    }
                }

                // Legende ohne Farben
                string legende = RenderLegende(y, players);
                if (!string.IsNullOrEmpty(legende))
                {
                    Console.SetCursorPosition(Weite + 1, y);
                    Console.Write(legende);
                }
            }
        }

        static ConsoleColor BestimmeFarbe(int x, int y, char zeichen, Player[] players)
        {
            foreach (var p in players)
            {
                if (x == p.PlayerX[0] && y == p.PlayerY[0]) return p.HeadFarbe;
            }

            return zeichen switch
            {
                _ when zeichen == players[0].TailSkin => players[0].TailFarbe,
                _ when players.Length > 1 && zeichen == players[1].TailSkin => players[1].TailFarbe,
                _ when zeichen == Skinvalues.MauerSkin => Skinvalues.MauerFarbe,
                _ when zeichen == Skinvalues.BombenSkin => Skinvalues.BombenFarbe,
                _ when zeichen == Skinvalues.SchluesselSkin => Skinvalues.SchluesselFarbe,
                _ => BestimmeItemFarbe(x, y)
            };
        }

        static ConsoleColor BestimmeItemFarbe(int x, int y)
        {
            var essen = Spiellogik.Essen.FirstOrDefault(e => e.X == x && e.Y == y);
            return essen?.FoodFarbe ?? ConsoleColor.White;
        }

        ConsoleColor RenderLegende(int y, ConsoleColor aktuelleFarbe, Player[] players)
        {
            void SetLegendeFarbe(ConsoleColor farbe)
            {
                if (farbe != aktuelleFarbe)
                {
                    Console.ForegroundColor = farbe;
                    aktuelleFarbe = farbe;
                }
            }

            Console.SetCursorPosition(Weite + 2, y);
            switch (y)
            {
                case 1:
                    SetLegendeFarbe(Skinvalues.RandFarbe);
                    Console.Write("══════════════════════════════"); break;
                case 2:
                    SetLegendeFarbe(ConsoleColor.White);
                    Console.Write(LanguageSystem.Get("legende")); break;
                case 3:
                    SetLegendeFarbe(Skinvalues.RandFarbe);
                    Console.Write("══════════════════════════════"); break;
                case 4:
                    SetLegendeFarbe(players[0].HeadFarbe);
                    Console.Write($"{players[0].Name}: {players[0].Punkte}"); break;
                case 5:
                    SetLegendeFarbe(Skinvalues.RandFarbe);
                    Console.Write("══════════════════════════════"); break;
                case 6:
                    if (Spielvalues.Multiplayer && players.Length > 1)
                    {
                        SetLegendeFarbe(players[1].HeadFarbe);
                        Console.Write($"{players[1].Name}: {players[1].Punkte}");
                    }
                    break;
                case 7:
                    if (Spielvalues.Multiplayer)
                    {
                        SetLegendeFarbe(Skinvalues.RandFarbe);
                        Console.Write("══════════════════════════════");
                    }
                    break;
            }
            return aktuelleFarbe;
        }

        static string RenderLegende(int y, Player[] players)
        {
            return y switch
            {
                1 => "══════════════════════════════",
                2 => LanguageSystem.Get("legende"),
                3 => "══════════════════════════════",
                4 => $"{players[0].Name}: {players[0].Punkte}",
                5 => "══════════════════════════════",
                6 when Spielvalues.Multiplayer => $"{players[1].Name}: {players[1].Punkte}",
                7 when Spielvalues.Multiplayer => "══════════════════════════════",
                _ => ""
            };
        }
    }
}