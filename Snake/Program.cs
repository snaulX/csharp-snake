using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Snake
{
    static class Program
    {
        static bool game;
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
                    using (TextReader reader = new StreamReader(new FileInfo("Resources/Results.txt").OpenRead()))
                    {
                        Console.Write(reader.ReadToEnd());
                    }
                    return;
                }
                else if (args[0] == "help")
                {
                    Process.Start("help.cmd");
                }
                else
                {
                    throw new Exception("Unknown argument");
                }
            }
            catch (Exception)
            {
                Console.Clear(); //clear console before game
                Console.Title = $"Console Snake v {Assembly.GetEntryAssembly().GetName().Version} by snaulX"; //set title
                input:
                Console.Write("Input width (min 5, norm 35, very big 50) and height (min 6, norm 15, very big 30) of field:");
                try
                {
                    w = int.Parse(Console.ReadLine()); //set width
                    h = int.Parse(Console.ReadLine()); //set height
                    field = new char[w, h]; //create empty field
                    Console.Write("Input FPS (easy 4, medium 6, hard 9 (for 35x15)):");
                    fps = int.Parse(Console.ReadLine()); //set fps (frames per second)
                }
                catch (FormatException)
                {
                    Console.WriteLine("Non-right input. Please input again.");
                    goto input;
                }
                snake = new Snake(new List<Point>
                {
                    new Point(w/2, h/2+1),
                    new Point(w/2, h/2),
                    new Point(w/2, h/2-1)
                }, 
                MoveVector.BOTTOM); //create snake and place by center
                Random random = new Random();
                food_place = new Point(random.Next(1, w - 2), random.Next(1, h - 2)); //generate place of food
                Thread createField = new Thread(new ThreadStart(CreateField));
                Console.WriteLine("Loading... (Create field)");
                createField.Start(); //create field
                createField.Join(); //wait when field creating
                Console.Title = $"Console Snake v {Assembly.GetEntryAssembly().GetName().Version} by snaulX";
                Console.WriteLine("Field was creating. Press any key to start");
                Console.ReadKey();
                game = true;
                TimerCallback tm = new TimerCallback(RefreshField); //make thread with refresh field
                Timer timer = new Timer(tm, null, 0, 1000/fps); //make Timer
                Console.CursorVisible = false; //off cursor

                while (game) //main cycle
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    if (key == ConsoleKey.UpArrow) vector = MoveVector.TOP; //change vector of move to top
                    else if (key == ConsoleKey.DownArrow) vector = MoveVector.BOTTOM; //change vector of move to bottom
                    else if (key == ConsoleKey.RightArrow) vector = MoveVector.RIGHT; //change vector of move to right
                    else if (key == ConsoleKey.LeftArrow) vector = MoveVector.LEFT; //change vector of move to left
                    else if (key == ConsoleKey.Enter) game = false; //break the game
                }

                timer.Change(Timeout.Infinite, 1000 / fps); //stop refresh field
                Console.CursorVisible = true; //on cursor
                GameOver();
            }
        }

        static void CreateField()
        {
            Console.Title = "Field is creating";
            start:
            Random random = new Random();
            food_place = new Point(random.Next(1, w - 2), random.Next(1, h - 2));
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

        static void RefreshField(object x)
        {
            if (snake.Over(w, h))
            {
                game = false; //change continue of game to false
                return;
            }
            if (snake.has(food_place))
            {
                score++;
                Point point = snake.body[snake.body.Count - 1];
                switch (snake.vectors[snake.vectors.Count - 1])
                {
                    case MoveVector.BOTTOM:
                        point.y--;
                        break;
                    case MoveVector.LEFT:
                        point.x++;
                        break;
                    case MoveVector.RIGHT:
                        point.x--;
                        break;
                    case MoveVector.TOP:
                        point.y++;
                        break;
                }
                snake.Add(point, snake.vectors[snake.vectors.Count - 1]); //there are bug
                while (snake.has(food_place))
                {
                    Random random = new Random();
                    food_place = new Point(random.Next(1, w - 2), random.Next(1, h - 2));
                } //check that any snake body element on place of food (sry for my English)
            }
            Console.Clear();
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (new Point(j, i) == food_place) Console.Write('&'); //draw food
                    else if (snake.has(new Point(j, i))) Console.Write('@'); //draw snake
                    else Console.Write(field[j, i]); //draw default
                }
                Console.WriteLine();
            } //(re)draw field
            Console.Title = $"Console Snake v {Assembly.GetEntryAssembly().GetName().Version} by snaulX. Your score = {score}"; //refresh title
            snake.Move(vector); //snake move
        }

        static void GameOver()
        {
            Console.Clear();
            Console.Title = $"Game over. Your score = {score}"; //change title of console
            Console.WriteLine("GAME OVER!!!");
            Console.WriteLine($"Your score is: {score}");
            Console.Write("If you want to save results write your name and enter. Else write no and enter."); //write message
            string name = Console.ReadLine(); //get name
            Console.Clear();
            if (name.Trim().ToLower() == "no" || name.Trim().Length == 0)
            {
                Environment.Exit(0); //just exit
            }
            else
            {
                using (TextWriter writer = new StreamWriter(new FileInfo("/Resources/Results.txt").OpenWrite()))
                {
                    writer.WriteLine(name + " score = " + score + " date of game = " + DateTime.Now + " fps = " + fps + " width = "
                        + w + " height = " + h); //write score to results.txt
                }
            }
        }
    }
}
