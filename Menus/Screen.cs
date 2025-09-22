namespace Smake.io.Menus
{
    public class Screen
    {
        public string[] Display { get; set; }
        public int selected { get; set; }
        public string title { get; set; }
        public virtual ConsoleKey Input { get; set; }
        public bool DoReadInput { get; set; } = true;

        public Screen()
        {
            
        }
        public void InitialRender()
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            if(title == "Menü")
            {
                DrawTitle();
                RenderMainMenue();
            }
            else
            {
                Render();
            }
        }
        public void Render()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(title);
            Console.WriteLine("══════════════════════════════");

            for (int i = 0; i < Display.Length; i++)
            {
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {Display[i]}");
            }

            Console.WriteLine("══════════════════════════════");
        }

        void DrawTitle()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
  ██████  ███▄ ▄███▓ ▄▄▄       ██ ▄█▀▓█████ 
▒██    ▒ ▓██▒▀█▀ ██▒▒████▄     ██▄█▒ ▓█   ▀ 
░ ▓██▄   ▓██    ▓██░▒██  ▀█▄  ▓███▄░ ▒███   
  ▒   ██▒▒██    ▒██ ░██▄▄▄▄██ ▓██ █▄ ▒▓█  ▄ 
▒██████▒▒▒██▒   ░██▒ ▓█   ▓██▒▒██▒ █▄░▒████▒
▒ ▒▓▒ ▒ ░░ ▒░   ░  ░ ▒▒   ▓▒█░▒ ▒▒ ▓▒░░ ▒░ ░
░ ░▒  ░ ░░  ░      ░  ▒   ▒▒ ░░ ░▒ ▒░ ░ ░  ░
░  ░  ░  ░      ░     ░   ▒   ░ ░░ ░    ░   
      ░         ░         ░  ░░  ░      ░  ░
");
            Console.ResetColor();
        }

        public void RenderMainMenue()
        {
            Console.SetCursorPosition(0, 11);
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║       SMAKE MAIN MENU        ║");
            Console.WriteLine("╠══════════════════════════════╣");

            for (int i = 0; i < Display.Length; i++)
            {
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"║  {zeiger} {Display[i],-25}║");
            }

            Console.WriteLine("╚══════════════════════════════╝");
        }

        public void StartInputstream()
        {
            Thread InputThread = new Thread(Readinput);
            InputThread.Start();
        }
        private void Readinput()
        {
            while (DoReadInput)
            {
                if (Console.KeyAvailable)
                {
                    Input = Console.ReadKey().Key;
                }
            }
            Thread.CurrentThread.Join();
        }
    }
}
