using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TicTacToe
{
    /// <summary>
    ///     This class hosts the main game logic.
    /// </summary>
    public class Game
    {
        /// <summary>
        ///     This is the main game board
        /// </summary>
        public int[] GameBoard = Enumerable.Repeat(0, 9).ToArray();

        /// <summary>
        ///     This is the list of the players
        /// </summary>
        public List<Player> Players = new List<Player>
        {
            new Player("Alice", 1, "X"),
            new Player("Bob", -1, "O")
        };

        /// <summary>
        ///     This list contains all the possibile winning condition for a game
        /// </summary>
        public int[][] WinConditions =
        {
            new[] {0, 1, 2}, new[] {3, 4, 5}, new[] {6, 7, 8},
            new[] {0, 3, 6}, new[] {1, 4, 7}, new[] {2, 5, 8},
            new[] {0, 4, 8}, new[] {2, 4, 6}
        };

        /// <summary>
        ///     This property hosts the reference to the current player, if present
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        ///     This is the current round
        /// </summary>
        public int Round { get; private set; }

        /// <summary>
        ///     Writes the current board to standard console output
        /// </summary>
        public virtual void DrawBoard()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayerToken(GameBoard[0]), GetPlayerToken(GameBoard[1]),
                GetPlayerToken(GameBoard[2]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayerToken(GameBoard[3]), GetPlayerToken(GameBoard[4]),
                GetPlayerToken(GameBoard[5]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayerToken(GameBoard[6]), GetPlayerToken(GameBoard[7]),
                GetPlayerToken(GameBoard[8]));
            sb.AppendLine();

            OutputMessage(sb.ToString(), true);
        }

        /// <summary>
        ///     If present, will return a valid player from the players list
        /// </summary>
        /// <param name="code">The player code</param>
        /// <returns>The <see cref="Player" /> with the matching code, if exists</returns>
        public Player GetPlayer(int code)
        {
            return Players.SingleOrDefault(p => p.Code == code);
        }

        /// <summary>
        ///     If a valid code is submitted, will return the player token
        /// </summary>
        /// <param name="code">The player code</param>
        /// <returns>The string representing the player token</returns>
        public string GetPlayerToken(int code)
        {
            var player = GetPlayer(code);
            return player == null ? " " : player.Token;
        }

        /// <summary>
        ///     Write a message to the standard output
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="newLine">True to add a newline after the message, false otherwise</param>
        public virtual void OutputMessage(string message, bool newLine = false)
        {
            Console.Write(message);
            if (newLine) Console.WriteLine();
        }

        /// <summary>
        ///     Check the game board to see if the player wins the game
        /// </summary>
        /// <param name="player">The player code</param>
        /// <returns>True if the player makes a winning move, false otherwise</returns>
        public virtual bool IsTheGameWin(int player)
        {
            if (player == 0) return false;

            return WinConditions.Any(
                winCondition =>
                    GameBoard[winCondition[0]] + GameBoard[winCondition[1]] + GameBoard[winCondition[2]] == player*3);
        }

        /// <summary>
        ///     Check to game board to see if there's any move left
        /// </summary>
        /// <returns>True if the game runs out of valid moves, false otherwise</returns>
        public virtual bool IsTheGameTied()
        {
            return GameBoard.All(g => g != 0);
        }

        /// <summary>
        ///     Reset the game board
        /// </summary>
        public void ResetBoard()
        {
            GameBoard = Enumerable.Repeat(0, 9).ToArray();
        }

        /// <summary>
        ///     Calculate the next valid move for the current player.
        ///     This method will check for a winning move first, than will check if the other player has a valid winning move and
        ///     blocks that one.
        ///     If none of those conditions apply, a random move will be choosen.
        /// </summary>
        /// <returns>The board position representing the move.</returns>
        public int CalculateNextMove()
        {
            // get winning move for player, if available
            int? move = GetWinningMove(CurrentPlayer.Code);
            if (move != null)
            {
                return Move(move.Value);
            }

            // get winning move for the other guy, if available
            move = GetWinningMove(GetOtherPlayer(CurrentPlayer.Code));
            if (move != null)
            {
                return Move(move.Value);
            }

            //first available move
            return Move(null);
        }

        /// <summary>
        ///     Set a board position ad "occupied" by the current player
        /// </summary>
        /// <param name="move">The board position to fill; if null, a random valid position will be choosen.</param>
        /// <returns>The board position actually filled.</returns>
        public virtual int Move(int? move)
        {
            if (move == null)
            {
                move = GetRandomMove();
            }

            GameBoard[move.Value] = CurrentPlayer.Code;
            return move.Value;
        }

        /// <summary>
        ///     Check if the player can win the game with the current move.
        /// </summary>
        /// <param name="player">The player code</param>
        /// <returns>The move to make to win the game; if no move is available, null will be returned.</returns>
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

        /// <summary>
        ///     Calculate a random move (valid)
        /// </summary>
        /// <returns>A board position representing a valid move to make</returns>
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

        /// <summary>
        ///     Returns the other player in the game
        /// </summary>
        /// <param name="player">The player code</param>
        /// <returns>Returns the other player code</returns>
        public int GetOtherPlayer(int player)
        {
            if (player == 1) return -1;
            return 1;
        }

        /// <summary>
        ///     Main game cycle
        /// </summary>
        /// <param name="sleep">The time (in ms) to wait between two consecutive moves.</param>
        /// <remarks>
        ///     This method will calculate the next move for the current player. After the move, will check if that player is in a
        ///     winning condition.
        ///     If not, will check if the game is tie.
        ///     If the game is not tie, the other player will play.
        /// </remarks>
        public virtual void PlayGame(int sleep)
        {
            sleep = sleep < 1 ? 1000 : sleep;

            CurrentPlayer = Players[0];
            Round = 0;

            OutputMessage(
                String.Format("Hi! {0} and {1} will play TTT today!", Players[0].Name, Players[1].Name), true);

            while (true)
            {
                Round++;

                int move = CalculateNextMove();
                OutputMessage(
                    String.Format("{0} (sign {1}) moves to position {2}", CurrentPlayer.Name, CurrentPlayer.Token, move),
                    true);
                DrawBoard();

                // check is winning move
                if (IsTheGameWin(CurrentPlayer.Code))
                {
                    OutputMessage(String.Format("{0} wins the game!", CurrentPlayer.Name), true);
                    break;
                }

                // check is tie
                if (IsTheGameTied())
                {
                    OutputMessage(String.Format("The game goes tie!"), true);
                    break;
                }

                CurrentPlayer = Players.Single(p => p.Code == GetOtherPlayer(CurrentPlayer.Code));

                Thread.Sleep(sleep);
            }
        }
    }
}