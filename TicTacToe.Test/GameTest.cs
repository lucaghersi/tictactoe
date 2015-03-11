using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace TicTacToe.Test
{
    public class GameTest
    {
        private static Game GetGame()
        {
            return new Game(GetPlayersList(), GetBoard());
        }

        private static Board GetBoard()
        {
            Board sut = new Board();
            sut.Initialize(GetPlayersList()[0], GetPlayersList()[1]);
            return sut;
        }

        private static List<Player> GetPlayersList()
        {
            return new List<Player> { new Player("Alice", 1, "X"), new Player("Bob", -1, "O") };
        }

        [Fact]
        public void NewGameStartsInitialized()
        {
            // arrange
            Game sut = GetGame();

            // assert
            Assert.Equal(Enumerable.Repeat(0, 9).ToArray(), sut.Board.GameBoard);
        }

        [Fact]
        public void IsWinningConditionReturnsTrueWhenThePlayerWins()
        {
            // arrange
            Game sut = GetGame();

            // test
            foreach (var winCondition in sut.WinConditions)
            {
                sut.Board.ResetBoard();
                sut.Board[winCondition[0]] = 1;
                sut.Board[winCondition[1]] = 1;
                sut.Board[winCondition[2]] = 1;

                // assert
                Assert.True(sut.IsTheGameWin(1));
                Assert.False(sut.IsTheGameWin(0));
                Assert.False(sut.IsTheGameWin(-1));
            }

            // test
            foreach (var winCondition in sut.WinConditions)
            {
                sut.Board.ResetBoard();
                sut.Board[winCondition[0]] = -1;
                sut.Board[winCondition[1]] = -1;
                sut.Board[winCondition[2]] = -1;

                // assert
                Assert.False(sut.IsTheGameWin(1));
                Assert.False(sut.IsTheGameWin(0));
                Assert.True(sut.IsTheGameWin(-1));
            }
        }

        [Fact]
        public void IsTheGameTiedReturnsTrueWhenTheAreNoMoreValidMoves()
        {
            // arrange
            Game sut = GetGame();

            Assert.False(sut.IsTheGameTied());

            // test
            sut.Board.ResetBoard(1);

            // assert
            Assert.True(sut.IsTheGameTied());

            // test
            sut.Board[0] = 0;

            // assert
            Assert.False(sut.IsTheGameTied());
        }

        [Fact]
        public void GetWinningMoveReturnWinningMoveIfAvailable()
        {
            // arrange
            Game sut = GetGame();

            int player = 1;
            int otherPlayer = sut.GetOtherPlayer(player);

            Assert.Null(sut.GetWinningMove(player));

            foreach (var winCondition in sut.WinConditions)
            {
                sut.Board.ResetBoard();

                // no winning
                sut.Board[winCondition[0]] = player;
                Assert.Null(sut.GetWinningMove(player));

                // winning
                sut.Board[winCondition[1]] = player;
                Assert.Equal(winCondition[2], sut.GetWinningMove(player));

                // no winning
                sut.Board[winCondition[2]] = otherPlayer;
                Assert.Null(sut.GetWinningMove(player));
            }

            player = -1;
            otherPlayer = sut.GetOtherPlayer(player);

            foreach (var winCondition in sut.WinConditions)
            {
                sut.Board.ResetBoard();

                // no winning
                sut.Board[winCondition[0]] = player;
                Assert.Null(sut.GetWinningMove(player));

                // winning
                sut.Board[winCondition[1]] = player;
                Assert.Equal(winCondition[2], sut.GetWinningMove(player));

                // no winning
                sut.Board[winCondition[2]] = otherPlayer;
                Assert.Null(sut.GetWinningMove(player));
            }
        }

        [Fact]
        public void MoveWillExecuteAMoveOnGameBoard()
        {
            // arrange
            Game sut = GetGame();

            sut.CurrentPlayer = sut.Players[0];
            const int position = 0;

            Assert.Equal(0, sut.Board[position]);

            // test
            var move = sut.Move(position);

            Assert.Equal(position, move);
            Assert.Equal(sut.CurrentPlayer.Code, sut.Board[position]);
        }

        [Fact]
        public void MoveWillExecuteRandomValidMoveWhenMoveIsNull()
        {
            // arrange
            Game sut = GetGame();

            sut.CurrentPlayer = sut.Players[0];

            // test
            var move = sut.Move(null);
            Assert.Equal(sut.CurrentPlayer.Code, sut.Board[move]);

            // no valid moves - invalid condition
            sut.Board.ResetBoard(1);
            Assert.Throws<InvalidOperationException>(() => sut.Move(null));
        }

        [Fact]
        public void GetOtherPlayerReturnsOtherPlayer()
        {
            // arrange
            Game sut = GetGame();

            // test
            Assert.Equal(-1, sut.GetOtherPlayer(1));
            Assert.Equal(1, sut.GetOtherPlayer(-1));
        }

        [Fact]
        public void CalculateNextMoveWillMakeRandomMoveIfNoWinningConditionIsAvailable()
        {
            // arrange
            var gameMock = new Mock<Game>(GetPlayersList(), GetBoard()) { CallBase = true };
            gameMock.Object.CurrentPlayer = gameMock.Object.Players[0];

            gameMock.Setup(g => g.GetWinningMove(gameMock.Object.CurrentPlayer.Code)).Returns(() => null);
            gameMock.Setup(g => g.GetWinningMove(gameMock.Object.GetOtherPlayer(gameMock.Object.CurrentPlayer.Code))).Returns(() => null);
            gameMock.Setup(g => g.Move(null)).Verifiable();

            // test
            gameMock.Object.CalculateNextMove();

            // assert
            gameMock.Verify(game => game.Move(null), Times.Once());
        }

        [Fact]
        public void CalculateNextMoveWillExecuteWinningMoveForPlayerIfAvailable()
        {
            // arrange
            const int move = 5;
            var gameMock = new Mock<Game>(GetPlayersList(), GetBoard()) { CallBase = true };
            gameMock.Object.CurrentPlayer = gameMock.Object.Players[0];

            gameMock.Setup(g => g.GetWinningMove(gameMock.Object.CurrentPlayer.Code)).Returns(() => move);

            // test
            gameMock.Object.CalculateNextMove();

            // assert
            gameMock.Verify(game => game.GetWinningMove(gameMock.Object.GetOtherPlayer(gameMock.Object.CurrentPlayer.Code)), Times.Never);
            gameMock.Verify(game => game.Move(move), Times.Once());
        }

        [Fact]
        public void CalculateNextMoveWillExecuteBlockingMoveForPlayerIfAvailable()
        {
            // arrange
            const int move = 5;
            var gameMock = new Mock<Game>(GetPlayersList(), GetBoard()) { CallBase = true };
            gameMock.Object.CurrentPlayer = gameMock.Object.Players[0];

            gameMock.Setup(g => g.GetWinningMove(gameMock.Object.CurrentPlayer.Code)).Returns(() => null);
            gameMock.Setup(g => g.GetWinningMove(gameMock.Object.GetOtherPlayer(gameMock.Object.CurrentPlayer.Code))).Returns(() => move);

            // test
            gameMock.Object.CalculateNextMove();

            // assert
            gameMock.Verify(game => game.Move(move), Times.Once());
        }

        [Fact]
        public void PlayGameWillExitWhenTheGameIsWon()
        {
            // arrange
            const int player = 1;

            var gameMock = new Mock<Game>(GetPlayersList(), GetBoard()) { CallBase = true };

            gameMock.Setup(g => g.IsTheGameWin(player)).Returns(() => true);
            gameMock.Setup(g => g.IsTheGameTied());

            // test
            gameMock.Object.PlayGame(1);

            // assert
            gameMock.Verify(g => g.IsTheGameWin(player), Times.Once);
            gameMock.Verify(g => g.IsTheGameTied(), Times.Never);
        }

        [Fact]
        public void PlayGameWillExitWhenTheGameIsTied()
        {
            // arrange
            const int player = 1;

            var gameMock = new Mock<Game>(GetPlayersList(), GetBoard()) { CallBase = true };

            gameMock.Setup(g => g.IsTheGameWin(player)).Returns(() => false);
            gameMock.Setup(g => g.IsTheGameTied()).Returns(() => true);

            // test
            gameMock.Object.PlayGame(1);

            // assert
            gameMock.Verify(g => g.IsTheGameWin(player), Times.Once);
            gameMock.Verify(g => g.IsTheGameTied(), Times.Once);
        }

        [Fact]
        public void PlayGameWillCheckForWinOrTieAfterEveryMove()
        {
            // arrange
            var gameMock = new Mock<Game>(GetPlayersList(), GetBoard()) { CallBase = true };

            // test
            gameMock.Object.PlayGame(1);

            // assert
            gameMock.Verify(g => g.IsTheGameWin(It.IsAny<int>()), Times.AtLeast(gameMock.Object.Round));
            gameMock.Verify(g => g.IsTheGameTied(), Times.AtLeast(gameMock.Object.Round - 1));
        }
    }
}