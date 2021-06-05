using System;
using System.Runtime.InteropServices;

namespace PiApprox {
    class Program {
        const int SWP_NOSIZE = 0x0001;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static IntPtr MyConsole = GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        static void Main(string[] args) {

            // var init
            int res = 51200;
            int squareSize = Console.LargestWindowHeight-7;
            int radius = res / 2;
            int pointsCircle = 0;
            int pointsSquare = 0;
            Random rand = new Random();

            // console window init
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetWindowSize(squareSize + 4, squareSize + 6);
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            SetWindowPos(MyConsole, 0, 0, 0, 0, 0, SWP_NOSIZE);

            // caption draw
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Approximated Pi is:");
            Console.WriteLine($"    Nº of elements:");
            // square frame draw
            // -beginning line
            string line = "╔";
            for(int i = 0; i < squareSize; ++i)
                line += "═";
            line += "╗";
            Console.WriteLine(line);

            // -inside
            for(int i = 0; i < squareSize; ++i) {
                Console.Write("║");
                for(int j = 0; j < squareSize; ++j) {
                    Console.Write(" ");
                }
                Console.Write("║\n");
            }
            // -ending line
            line = "╚";
            for(int i = 0; i < squareSize; ++i)
                line += "═";
            line += "╝";
            Console.WriteLine(line);
            // -info
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Simulation resolution is {res}x{res} mapped to {squareSize}x{squareSize} grid.");

            // main loop throwing and drawing points
            while(true) {
                int x = rand.Next(0, res);
                int y = rand.Next(0, res);
                bool inCircle = Math.Sqrt(Math.Pow((x - radius), 2) + Math.Pow((y - radius), 2)) <= radius ? true : false;
                ++pointsSquare;
                if(inCircle) {
                    ++pointsCircle;
                }
                // Ratio = points in circle / points in square (all points)
                // Pi is 4 * ratio
                float ratio = (float)pointsCircle / pointsSquare;
                Console.SetCursorPosition(19, 0);
                Console.Write($"{4 * ratio}");
                Console.SetCursorPosition(19, 1);
                Console.Write($"{pointsSquare}");
                Console.SetCursorPosition(map(x,0,res,0,squareSize) + 1, map(y, 0, res, 0, squareSize) + 3);
                if(inCircle)
                    Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("█");
                Console.ForegroundColor = ConsoleColor.White;
            }

            // helper function
            static int map(int value, int fromLow, int fromHigh, int toLow, int toHigh) {
                return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
            }
        }
    }
}
