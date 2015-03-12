using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>
    ///     Manage the board
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        ///     Access the gameboard by index
        /// </summary>
        /// <param name="index">Index of cell to access</param>
        int this[int index] { get; set; }

        /// <summary>
        ///     This is the main game board
        /// </summary>
        int[] GameBoard { get; }

        /// <summary>
        ///     This is the list of the players
        /// </summary>
        List<IPlayer> Players { get; }

        /// <summary>
        ///     Initialize the board
        /// </summary>
        /// <param name="player1">The first player</param>
        /// <param name="player2">The second player</param>
        void Initialize(IPlayer player1, IPlayer player2);

        /// <summary>
        ///     Draws the current board to standard console output.
        /// </summary>
        void DrawBoard();

        /// <summary>
        ///     Returns a string representation of this <see cref="Board" /> object
        /// </summary>
        /// <returns>The string that represent this board</returns>
        string ToString();

        /// <summary>
        ///     If present, will return a valid player from the players list
        /// </summary>
        /// <param name="code">The player code</param>
        /// <returns>The <see cref="IPlayer" /> with the matching code, if exists</returns>
        IPlayer GetPlayer(int code);

        /// <summary>
        ///     If a valid code is submitted, will return the player token
        /// </summary>
        /// <param name="code">The player code</param>
        /// <returns>The string representing the player token</returns>
        string GetPlayerToken(int code);

        /// <summary>
        ///     Reset the game board
        /// </summary>
        /// <param name="code">If present, set all the board to the specific code (useful for testing purposes)</param>
        void ResetBoard(int code = 0);

        /// <summary>
        ///     Calculate a random move (valid)
        /// </summary>
        /// <returns>A board position representing a valid move to make</returns>
        int GetRandomEmptyCell();
    }
}