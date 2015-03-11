using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    public class Game
    {
        public int[] GameBoard = Enumerable.Repeat(0, 9).ToArray();

        public int[][] WinConditions =
        {
            new[] {0, 1, 2}, new[] {3, 4, 5}, new[] {6, 7, 8},
            new[] {0, 3, 6}, new[] {1, 4, 7}, new[] {2, 5, 8},
            new[] {0, 4, 8}, new[] {2, 4, 6}
        };

        public List<Player> Players = new List<Player> {new Player("Alice", 1, "X"), new Player("Bob", -1, "O")};   
            
        public void DrawBoard()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayer(GameBoard[0]), GetPlayer(GameBoard[1]),
                GetPlayer(GameBoard[2]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayer(GameBoard[3]), GetPlayer(GameBoard[4]),
                GetPlayer(GameBoard[5]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayer(GameBoard[6]), GetPlayer(GameBoard[7]),
                GetPlayer(GameBoard[8]));
            sb.AppendLine();

            OutputMessage(sb.ToString(), true);
        }

        public Player GetPlayer(int code)
        {
             return Players.SingleOrDefault(p => p.Code == code);
        }

        public void OutputMessage(string message, bool newLine = false)
        {
            Console.Write(message);
            if (newLine) Console.WriteLine();
        }

        public bool IsTheGameWin(int player)
        {
            if (player == 0) return false;

            return WinConditions.Any(
                winCondition =>
                    GameBoard[winCondition[0]] + GameBoard[winCondition[1]] + GameBoard[winCondition[2]] == player*3);
        }

        public bool IsTheGameTied()
        {
            return GameBoard.All(g => g != 0);
        }

        public void ResetBoard()
        {
            GameBoard = Enumerable.Repeat(0, 9).ToArray();
        }

        public int CalculateNextMove(int player)
        {
            // get winning move for player, if available
            int? move = GetWinningMove(player);
            if (move != null)
            {
                return Move(player, move.Value);
            }

            // get winning move for the other guy, if available
            move = GetWinningMove(GetOtherPlayer(player));
            if (move != null)
            {
                return Move(player, move.Value);
            }

            //first available move
            return Move(player, null);
        }

        public virtual int Move(int player, int? move)
        {
            if (move == null)
            {
                move = GetRandomMove();
            }

            GameBoard[move.Value] = player;
            return move.Value;
        }

        public virtual int? GetWinningMove(int player)
        {
            foreach (var winCondition in
                WinConditions.Where(
                    winCondition =>
                        GameBoard[winCondition[0]] + GameBoard[winCondition[1]] + GameBoard[winCondition[2]] ==
                        player*2))
            {
                if (GameBoard[winCondition[0]] == 0) return winCondition[0];
                return GameBoard[winCondition[1]] == 0 ? winCondition[1] : winCondition[2];
            }

            return null;
        }

        public int GetRandomMove()
        {
            List<int> randomMoves = new List<int>();
            for (int index = 0; index < GameBoard.GetLength(0); index++)
            {
                if (GameBoard[index] == 0)
                {
                    randomMoves.Add(index);
                }
            }

            if (randomMoves.Count == 0)
                throw new InvalidOperationException("This condition cannot happen!");

            return randomMoves[(new Random()).Next(0, randomMoves.Count - 1)];
        }

        public int GetOtherPlayer(int player)
        {
            if (player == 1) return -1;
            return 1;
        }

        public void PlayGame()
        {
            Player player = Players[0];

            while (true)
            {
                int move = CalculateNextMove(player.Code);
                OutputMessage(
                    String.Format("{0} (sign {1}) moves to {2}", player.Name, player.Token, move), true);
                DrawBoard();

                // check is winning move
                if (IsTheGameWin(player.Code))
                {
                    OutputMessage(String.Format("{0} wins the game!", player.Name), true);
                    break;
                }

                // check is tie
                if (IsTheGameTied())
                {
                    OutputMessage(String.Format("The game goes tie!"), true);
                    break;
                }

                player = Players.Single(p => p.Code == GetOtherPlayer(player.Code));
            }
        }
    }
}