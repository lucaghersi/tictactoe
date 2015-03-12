using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
    /// <summary>
    ///     Manage the board
    /// </summary>
    public class Board : IBoard
    {
        /// <summary>
        ///     The token for a free space on the board.
        /// </summary>
        private const string FREETOKEN = "-";

        /// <summary>
        ///     Represent a section of the game board.
        /// </summary>
        private const string BOARD_SEPARATION = "---|---|---";

        /// <summary>
        ///     Access the gameboard by index
        /// </summary>
        /// <param name="index">Index of cell to access</param>
        public int this[int index]
        {
            get { return GameBoard[index]; }
            set { GameBoard[index] = value; }
        }

        /// <summary>
        ///     This is the main game board
        /// </summary>
        public int[] GameBoard { get; private set; }

        /// <summary>
        ///     This is the list of the players
        /// </summary>
        public List<IPlayer> Players { get; private set; }

        /// <summary>
        ///     Initialize the board
        /// </summary>
        /// <param name="player1">The first player</param>
        /// <param name="player2">The second player</param>
        public void Initialize(IPlayer player1, IPlayer player2)
        {
            if (ReferenceEquals(player1, null))
                throw new ArgumentNullException("player1", "A player is required!");
            if (ReferenceEquals(player2, null))
                throw new ArgumentNullException("player1", "A player is required!");

            GameBoard = Enumerable.Repeat(0, 9).ToArray();
            Players = new List<IPlayer> {player1, player2};
        }

        /// <summary>
        ///     Draws the current board to standard console output.
        /// </summary>
        public virtual void DrawBoard()
        {
            Utility.OutputMessage(ToString(), true);
        }

        /// <summary>
        ///     Returns a string representation of this <see cref="Board" /> object
        /// </summary>
        /// <returns>The string that represent this board</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayerToken(GameBoard[0]), GetPlayerToken(GameBoard[1]),
                GetPlayerToken(GameBoard[2]));
            sb.AppendLine();
            sb.AppendFormat(BOARD_SEPARATION);
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayerToken(GameBoard[3]), GetPlayerToken(GameBoard[4]),
                GetPlayerToken(GameBoard[5]));
            sb.AppendLine();
            sb.AppendFormat(BOARD_SEPARATION);
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}", GetPlayerToken(GameBoard[6]), GetPlayerToken(GameBoard[7]),
                GetPlayerToken(GameBoard[8]));
            sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        ///     If present, will return a valid player from the players list
        /// </summary>
        /// <param name="code">The player code</param>
        /// <returns>The <see cref="IPlayer" /> with the matching code, if exists</returns>
        public virtual IPlayer GetPlayer(int code)
        {
            return Players.SingleOrDefault(p => p.Code == code);
        }

        /// <summary>
        ///     If a valid code is submitted, will return the player token
        /// </summary>
        /// <param name="code">The player code</param>
        /// <returns>The string representing the player token</returns>
        public virtual string GetPlayerToken(int code)
        {
            var player = GetPlayer(code);
            return player == null ? FREETOKEN : player.Token;
        }

        /// <summary>
        ///     Reset the game board
        /// </summary>
        /// <param name="code">If present, set all the board to the specific code (useful for testing purposes)</param>
        public virtual void ResetBoard(int code = 0)
        {
            GameBoard = Enumerable.Repeat(code, 9).ToArray();
        }

        /// <summary>
        ///     Calculate a random move (valid)
        /// </summary>
        /// <returns>A board position representing a valid move to make</returns>
        public virtual int GetRandomEmptyCell()
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
    }
}