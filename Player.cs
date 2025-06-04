namespace Snake.io
{
    public class Spieler
    {
        // Eingabe-Richtung (durch Pfeiltasten)
        public int InputX;

        public int InputY;

        public bool Aenderung;

        // Position des Spielers (Startkoordinaten)
        public int[] PlayerX;

        public int[] PlayerY;

        // Kollisionsvariablen
        public bool KollisionRand;

        public bool KollisionPlayer;

        // Länge des Spielers

        public int Tail;

        //Punkte des Spielers
        public int Punkte;

        // Namen der Spieler
        public string? Name;

        // Aussehen des Spielers
        public char Head;

        public char Skin;

        public ConsoleColor Farbe;

        public ConsoleColor Headfarbe;

        // Shop variablen
        public int Skinzahl;

        public int Farbezahl;

        public int Headfarbezahl;

    }
}
