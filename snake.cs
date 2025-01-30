using System;
using System.Collections.Generic;
using System.Threading;

class SnakeGame
{
    static int width = 20, height = 10; // Reduced size for better compatibility
    static int score = 0;
    static bool gameOver = false;
    static (int X, int Y) food;
    static List<(int X, int Y)> snake = new List<(int X, int Y)> { (10, 5) };
    static (int X, int Y) direction = (0, 1);
    
    static void Main()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(Math.Max(width + 5, 30), Math.Max(height + 5, 20)); // Ensure console is large enough
        GenerateFood();
        
        Thread gameLoop = new Thread(GameLoop);
        gameLoop.Start();
        
        while (!gameOver)
        {
            var key = Console.ReadKey(true).Key;
            ChangeDirection(key);
        }
        
        Console.SetCursorPosition(0, height + 3);
        Console.WriteLine("Game Over! Final Score: " + score);
    }

    static void GameLoop()
    {
        while (!gameOver)
        {
            Move();
            Draw();
            Thread.Sleep(100);
        }
    }
    
    static void Draw()
    {
        if (Console.WindowHeight < height + 5 || Console.WindowWidth < width + 5)
        {
            Console.Clear();
            Console.WriteLine("Please resize the console window and restart the game.");
            gameOver = true;
            return;
        }

        Console.Clear();
        
        for (int i = 0; i < width + 2; i++) Console.Write("#");
        Console.WriteLine();
        
        for (int y = 0; y < height; y++)
        {
            Console.Write("#");
            for (int x = 0; x < width; x++)
            {
                if (snake.Contains((x, y))) Console.Write("O");
                else if ((x, y) == food) Console.Write("X");
                else Console.Write(" ");
            }
            Console.WriteLine("#");
        }
        
        for (int i = 0; i < width + 2; i++) Console.Write("#");
        Console.WriteLine("\nScore: " + score);
    }
    
    static void Move()
    {
        (int X, int Y) newHead = (snake[0].X + direction.X, snake[0].Y + direction.Y);
        
        if (newHead.X < 0 || newHead.X >= width || newHead.Y < 0 || newHead.Y >= height || snake.Contains(newHead))
        {
            gameOver = true;
            return;
        }
        
        snake.Insert(0, newHead);
        
        if (newHead == food)
        {
            score++;
            GenerateFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }
    
    static void ChangeDirection(ConsoleKey key)
    {
        if (key == ConsoleKey.UpArrow && direction.Y != 1) direction = (0, -1);
        if (key == ConsoleKey.DownArrow && direction.Y != -1) direction = (0, 1);
        if (key == ConsoleKey.LeftArrow && direction.X != 1) direction = (-1, 0);
        if (key == ConsoleKey.RightArrow && direction.X != -1) direction = (1, 0);
    }
    
    static void GenerateFood()
    {
        Random rand = new Random();
        do { food = (rand.Next(0, width), rand.Next(0, height)); }
        while (snake.Contains(food));
    }
}