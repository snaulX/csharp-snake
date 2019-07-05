using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Snake
{
    static class Program
    {
        static char[,] field;
        static int w, h, score = 0, fps;
        static Snake snake;
        static MoveVector vector = MoveVector.BOTTOM;
        static Point food_place;
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "results")
                {
                    using (TextReader reader = new StreamReader(new FileInfo("../../../results.txt").OpenRead()))
                    {
                        Console.Write(reader.ReadToEnd());
                    }
                    return;
                }
                else
                {
                    throw new Exception("Unknown argument");
                }
            }
            catch (Exception)
            {
                Console.Title = "Console Snake v 1.0.0 by snaulX";
                Console.Write("Input width (min 5) and height (min 6) of field:");
                w = int.Parse(Console.ReadLine());
                h = int.Parse(Console.ReadLine());
                field = new char[w, h];
                Console.Write("Input FPS:");
                fps = int.Parse(Console.ReadLine());
                snake = new Snake(new List<Point>
                {
                    new Point(w/2, h/2+1),
                    new Point(w/2, h/2),
                    new Point(w/2, h/2-1)
                }, 
                MoveVector.BOTTOM); //create snake and place by center
                Random random = new Random();
                food_place = new Point(random.Next(w - 2), random.Next(h - 2)); //generate place of food
                Thread createField = new Thread(new ThreadStart(CreateField));
                Console.WriteLine("Loading... (Create field)");
                createField.Start();
                createField.Join();
                Console.Title = "Console Snake v 1.0.0 by snaulX";
                Console.WriteLine("Field was creating. Press any key to start");
                Console.ReadKey();
                bool game = true;
                Thread main = new Thread(new ThreadStart(RefreshField));
                while (game)
                {
                    main.Start();
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.UpArrow) vector = MoveVector.TOP;
                    else if (key == ConsoleKey.DownArrow) vector = MoveVector.BOTTOM;
                    else if (key == ConsoleKey.RightArrow) vector = MoveVector.RIGHT;
                    else if (key == ConsoleKey.LeftArrow) vector = MoveVector.LEFT;
                    else if (key == ConsoleKey.Enter) game = false;
                }
                
                GameOver();
            }
        }

        static void CreateField()
        {
            Console.Title = "Field is creating";
            start:
            Random random = new Random();
            food_place = new Point(random.Next(w - 2), random.Next(h - 2));
            if (snake.has(food_place)) goto start;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((j == 0) || (j == w - 1) || (i == 0) || (i == h - 1)) field[j, i] = '#'; //create borders
                    else field[j, i] = ' '; //create empty
                }
            }
        }

        static void RefreshField()
        {
            start:
            if (snake.has(food_place))
            {
                score++;
                Random random = new Random();
                food_place = new Point(random.Next(w - 2), random.Next(h - 2));
                goto start;
            }
            Console.Clear();
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Console.Write(field[j, i]);
                }
                Console.WriteLine();
            } //(re)draw field
            Console.Title = $"Console Snake v 1.0.0 by snaulX. Your score = {score}";
            Thread.Sleep(1000 / fps);
        }

        static void GameOver()
        {
            Console.Title = $"Game over. Your score = {score}";
            Console.WriteLine("GAME OVER!!!");
            Console.WriteLine($"Your score is: {score}");
            Console.Write("If you want to save results write your name and enter. Else write no and enter.");
            string name = Console.ReadLine();
            if (name.ToLower() == "no")
            {
                Environment.Exit(0);
            }
            else
            {
                using (TextWriter writer = new StreamWriter(new FileInfo("../../../results.txt").OpenWrite()))
                {
                    writer.WriteLine(name + " score = " + score + " date of game = " + DateTime.Now + " fps = " + fps + " width = "
                        + w + " height = " + h);
                }
            }
        }
    }
}
