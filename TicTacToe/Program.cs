using System;
using System.Collections.Generic;

namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to TTT!");
            Console.WriteLine();
            Console.Write("Press P to start or any other key to exit... ");
            var playAgain = Console.ReadKey().Key == ConsoleKey.P;
            Console.WriteLine();
            Console.WriteLine();

            while(playAgain)
            {
                // replace with your favorite injection engine! :D
                List<Player> players = new List<Player>
                {
                    new Player("Alice", 1, "X"),
                    new Player("Bob", -1, "O")
                };
                var board = new Board();
                board.Initialize(players[0], players[1]);

                ISleep sleepService = new Sleep();

                // start a new game
                Game game = new Game(players, board, sleepService);
                game.PlayGame(1000);

                Console.Write("Do you want to play again (Y to continue or any other key to exit)? ");
                playAgain = Console.ReadKey().Key == ConsoleKey.Y;
                Console.WriteLine();
                Console.WriteLine();
            }            

            Console.WriteLine();
            Console.WriteLine("Goodbye!");
            Console.ReadLine();
        }
    }
}
