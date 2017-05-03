using System;
using System.Threading;

namespace ConsolePingPong
{
    class Program
    {
        public static int screenWidth = 100;//80
        public static int screenHeight = 30;//30
        public static int playerLength = 5;//10 
        public static int gameSpeed = 50;//100 p.s. in Mileseconds
        public static int playCount = 0;
        public static int wins = 0;
        public static Thread playerThread;
        public static Thread player2Thread;
        public static string text = "Debug";
        static void Main(string[] args)
        {
            if(args.Length == 2)
            {
                text = int.Parse(args[0]).ToString();
                if (!string.IsNullOrEmpty(args[0]))
                    gameSpeed = int.Parse(args[0]);
                if (!string.IsNullOrEmpty(args[1]))
                    playerLength = int.Parse(args[1]);
            }
            if(args.Length == 4)
            {
                if (!string.IsNullOrEmpty(args[0]))
                    gameSpeed = int.Parse(args[0]);
                if (!string.IsNullOrEmpty(args[1]))
                    playerLength = int.Parse(args[1]);
                if (!string.IsNullOrEmpty(args[2]))
                    screenWidth = int.Parse(args[2]);
                if (!string.IsNullOrEmpty(args[3]))
                    screenHeight = int.Parse(args[3]);
                text = gameSpeed.ToString();
            }
            Console.SetBufferSize(screenWidth, screenHeight);
            Renderer.Render();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("FirstPlayer(w/up,s/down) SecondPlayer(o/up,l/down).Press any key to start \n "+Console.LargestWindowHeight+"-"+Console.LargestWindowWidth);
            Console.ReadKey(true);
            playerThread = new Thread(Player.Proces);
            player2Thread = new Thread(Player2.Proces);
            Thread worldThread = new Thread(World.Proces);
            worldThread.Start();
            playerThread.Start();
            player2Thread.Start();
            Ball.vec.x = -1;
            Ball.vec.y = -1;
            Renderer.Render();
        }
    }

    static class Paint
    {
        public static void SetPixel(int x, int y, ConsoleColor col)
        {
            if (x >= 0 && y >= 0 && x < Program.screenWidth && y < Program.screenHeight)
            {
                Console.SetCursorPosition(x, y);
                Console.BackgroundColor = col;
                Console.Write(" ");
            }
        }

        public static void Line(int x, int y)
        {
            for (int i = 0; i < Program.playerLength; i++)
                SetPixel(x, i + y, ConsoleColor.Red);
        }
    }

    static class World
    {
        public static void Proces()
        {
            while (true)
            {
                Thread.Sleep(Program.gameSpeed);
                if (Ball.x == 1)
                    if (Player.y <= Ball.y && Player.y > Ball.y - Program.playerLength)
                    {
                        Ball.vec.x *= -1;
                        Player.work = false;
                        Player2.work = true;
                    }

                if (Ball.x == Program.screenWidth - 2)
                    if (Player2.y <= Ball.y && Player2.y > Ball.y - Program.playerLength)
                    {
                        Ball.vec.x *= -1;
                        Player2.work = false;
                        Player.work = true;
                    }

                if (Ball.y == 0)
                    Ball.vec.y *= -1;
                if (Ball.y == Program.screenHeight)
                    Ball.vec.y *= -1;

                if (!(Ball.x == 0 | Ball.x == Program.screenWidth))
                {
                    Ball.x += Ball.vec.x;
                    Ball.y += Ball.vec.y;
                }
                else
                {
                    Program.playCount++;
                    if (Ball.x == 0)
                    { Program.wins--; Player.work = false; Player2.work = true; }
                    else
                    { Program.wins++; Player.work = true; Player2.work = false; }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Press any key to start");
                    Ball.x = Program.screenWidth / 2;
                    Ball.y = Program.screenHeight / 2;
                    Player.y = (Program.screenHeight / 2) - Program.playerLength;
                    Player2.y = (Program.screenHeight / 2) - Program.playerLength;
                    Ball.vec.x *= -1;
                    Ball.vec.y *= -1;
                    Console.ReadKey(true);
                }
                Renderer.Render();
            }
        }
    }

    class Player
    {
        public static bool work = true;
        public static int y = 10;
        public static void Proces()
        {
            while (true)
            {
                while (work)
                {
                    ConsoleKeyInfo KI = Console.ReadKey(true);
                    if (KI.KeyChar.ToString() == "w" && y > 0)
                        y--;
                    if (KI.KeyChar.ToString() == "s" && y < Program.screenHeight - Program.playerLength)
                        y++;
                }
                Thread.Sleep(0100);
            }
        }
    }

    class Player2
    {
        public static bool work = true;
        public static int y = 10;
        public static void Proces()
        {
            while (true)
            {
                while (work)
                {
                    ConsoleKeyInfo KI = Console.ReadKey(true);
                    if (KI.KeyChar.ToString() == "o" && y > 0)
                        y--;
                    if (KI.KeyChar.ToString() == "l" && y < Program.screenHeight - Program.playerLength)
                        y++;
                }
                Thread.Sleep(0100);
            }
        }
    }

    class Ball
    {
        public static int x = 10;
        public static int y = 20;
        public static Vector vec = new Vector();
    }

    struct Vector
    {
        public int x;
        public int y;
    }

    class Renderer
    {
        public static void Render()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.SetCursorPosition(Program.screenWidth / 2, 0);
            Console.WriteLine(Program.text + "/PlayCount=" + Program.playCount + "/Wins=" + Program.wins);
            Paint.Line(0, Player.y);//Player
            Paint.Line(Program.screenWidth - 1, Player2.y);//Player2
            Paint.SetPixel(Ball.x, Ball.y, ConsoleColor.Green);//Ball
        }
    }
}
