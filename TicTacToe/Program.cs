using System;

namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to TTT!");
            bool playAgain;

            do
            {
                Game game = new Game();
                game.PlayGame();

                Console.WriteLine("Do you want to play again? (Y to continue or any other key to exit...)");
                playAgain = Console.ReadKey().Key == ConsoleKey.Y;

            } while (playAgain);

            Console.WriteLine();
            Console.WriteLine("Goodbye!");
            Console.ReadLine();
        }
    }
}
