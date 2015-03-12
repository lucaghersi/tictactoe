using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TicTacToe
{
    /// <summary>
    ///     This class hosts the main game logic.
    /// </summary>
    public class Game
    {
        /// <summary>
        ///     This list contains all the possibile winning condition for a game
        /// </summary>
        private readonly ImmutableList<ImmutableList<int>> _winConditions = ImmutableList.Create(
            ImmutableList.Create(0, 1, 2),
            ImmutableList.Create(3, 4, 5),
            ImmutableList.Create(6, 7, 8),
            ImmutableList.Create(0, 3, 6),
            ImmutableList.Create(1, 4, 7),
            ImmutableList.Create(2, 5, 8),
            ImmutableList.Create(0, 4, 8),
            ImmutableList.Create(2, 4, 6));

        /// <summary>
        ///     This is the main game board
        /// </summary>
        public Board Board;

        /// <summary>
        ///     This is the list of the players
        /// </summary>
        public List<Player> Players;

        /// <summary>
        /// </summary>
        public Game(List<Player> players, Board board, ISleep sleepService)
        {
            if (ReferenceEquals(players, null))
                throw new ArgumentNullException("players", "Players list is required!");
            if (ReferenceEquals(board, null))
                throw new ArgumentNullException("board", "You can't play without a board!");
            if (ReferenceEquals(sleepService, null))
                throw new ArgumentNullException("sleepService", "Sleep service is required!");

            Board = board;
            Players = players;
            SleepService = sleepService;
        }

        /// <summary>
        ///     This property hosts the reference to the current player, if present
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        ///     This is the current round
        /// </summary>
        public int Round { get; private set; }

        /// <summary>
        ///     This list contains all the possibile winning condition for a game
        /// </summary>
        public ImmutableList<ImmutableList<int>> WinConditions
        {
            get { return _winConditions; }
        }

        /// <summary>
        /// Sleep service instance
        /// </summary>
        public ISleep SleepService { get; private set; }

        /// <summary>
        ///     Check the game board to see if the player wins the game.
        /// </summary>
        /// <param name="player">The player code</param>
        /// <returns>True if the player makes a winning move, false otherwise</returns>
        public virtual bool IsTheGameWin(int player)
        {
            if (player == 0) return false;

            return _winConditions.Any(
                winCondition =>
                    Board[winCondition[0]] + Board[winCondition[1]] + Board[winCondition[2]] == player*3);
        }

        /// <summary>
        ///     Check to game board to see if there's any move left.
        /// </summary>
        /// <returns>True if the game runs out of valid moves, false otherwise</returns>
        public virtual bool IsTheGameTied()
        {
            return Board.GameBoard.All(g => g != 0);
        }

        /// <summary>
        ///     Calculate the next valid move for the current player.
        ///     This method will check for a winning move first, than will check if the other player has a valid winning move and
        ///     blocks that one.
        ///     If none of those conditions apply, a random move will be choosen.
        /// </summary>
        /// <returns>The board position representing the move.</returns>
        public virtual int CalculateNextMove()
        {
            // get winning move for player, if available
            int? move = GetWinningMove(CurrentPlayer.Code);
            if (move != null)
            {
                return Move(move.Value);
            }

            // get winning move for the other guy, if available (we try to block him)
            move = GetWinningMove(GetOtherPlayer(CurrentPlayer.Code));

            //first available move (null or move)
            return Move(move);
        }

        /// <summary>
        ///     Set a board position ad "occupied" by the current player.
        /// </summary>
        /// <param name="move">The board position to fill; if null, a random valid position will be choosen.</param>
        /// <returns>The board position actually filled.</returns>
        public virtual int Move(int? move)
        {
            if (move == null)
            {
                move = Board.GetRandomEmptyCell();
            }

            Board[move.Value] = CurrentPlayer.Code;
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
                _winConditions.Where(
                    winCondition =>
                        Board[winCondition[0]] + Board[winCondition[1]] + Board[winCondition[2]] ==
                        player*2))
            {
                if (Board[winCondition[0]] == 0) return winCondition[0];
                return Board[winCondition[1]] == 0 ? winCondition[1] : winCondition[2];
            }

            return null;
        }

        /// <summary>
        ///     Returns the other player in the game
        /// </summary>
        /// <param name="player">The player code</param>
        /// <returns>Returns the other player code</returns>
        public virtual int GetOtherPlayer(int player)
        {
            if (player == 1) return -1;
            return 1;
        }

        /// <summary>
        ///     Main game cycle.
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
            CurrentPlayer = Players[0];
            Round = 0;

            Utility.OutputMessage(
                String.Format("Hi! {0} and {1} will play TTT today!", Players[0].Name, Players[1].Name), true);
            Utility.OutputMessage(String.Empty, true);

            while (true)
            {
                Round++;

                int move = CalculateNextMove();
                Utility.OutputMessage(
                    String.Format("{0} (sign {1}) moves to position {2}", CurrentPlayer.Name, CurrentPlayer.Token, move),
                    true);
                Board.DrawBoard();

                // check is winning move
                if (IsTheGameWin(CurrentPlayer.Code))
                {
                    Utility.OutputMessage(String.Format("{0} wins the game!", CurrentPlayer.Name), true);
                    break;
                }

                // check is tie
                if (IsTheGameTied())
                {
                    Utility.OutputMessage(String.Format("The game goes tie!"), true);
                    break;
                }

                CurrentPlayer = Players.Single(p => p.Code == GetOtherPlayer(CurrentPlayer.Code));

                SleepService.Wait(sleep);
            }
        }
    }
}