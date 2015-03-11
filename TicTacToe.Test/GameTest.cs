using System;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Xunit;

namespace TicTacToe.Test
{
    public class GameTest
    {
        [Fact]
        public void NewGameStartsInitialized()
        {
            // arrange
            Game sut = new Game();

            // assert
            Assert.Equal(Enumerable.Repeat(0, 9).ToArray(), sut.GameBoard);
        }

        [Fact]
        public void GetPlayerReturnsValidPlayerFromList()
        {
            // arrange
            Game sut = new Game();

            // assert
            Assert.Equal(sut.Players.Single(p => p.Code == -1), sut.GetPlayer(-1));
        }

        [Fact]
        public void DrawBoardDrawsCorrectBoard()
        {
            // arrange
            Game sut = new Game();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" {0} | {1} | {2}",
                sut.GetPlayer(sut.GameBoard[0]),
                sut.GetPlayer(sut.GameBoard[1]),
                sut.GetPlayer(sut.GameBoard[2]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}",
                sut.GetPlayer(sut.GameBoard[3]),
                sut.GetPlayer(sut.GameBoard[4]),
                sut.GetPlayer(sut.GameBoard[5]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}",
                sut.GetPlayer(sut.GameBoard[6]),
                sut.GetPlayer(sut.GameBoard[7]),
                sut.GetPlayer(sut.GameBoard[8]));
            sb.AppendLine();
            sb.AppendLine();

            // test
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                sut.DrawBoard();

                Assert.Equal(sb.ToString(), sw.ToString());
            }
        }

        [Fact]
        public void OutputMessagePushMessageToConsole()
        {
            // arrange
            Game sut = new Game();
            const string expected = "hello!";

            // test
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                sut.OutputMessage(expected, false);
                Assert.Equal(expected, sw.ToString());
            }
        }

        [Fact]
        public void OutputMessagePushMessageToConsoleWithNewLine()
        {
            // arrange
            Game sut = new Game();

            const string expected = "hello!";
            StringBuilder sb = new StringBuilder(expected);
            sb.AppendLine();

            // test
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                sut.OutputMessage(expected, true);
                Assert.Equal(sb.ToString(), sw.ToString());
            }
        }

        [Fact]
        public void ResetBoardWillRestoreEmptyBoard()
        {
            // arrange
            Game sut = new Game();
            sut.GameBoard[0] = 1;

            // test
            sut.ResetBoard();

            // assert
            Assert.Equal(Enumerable.Repeat(0, 9).ToArray(), sut.GameBoard);
        }

        [Fact]
        public void IsWinningConditionReturnsTrueWhenThePlayerWins()
        {
            // arrange
            Game sut = new Game();

            // test
            foreach (int[] winCondition in sut.WinConditions)
            {
                sut.ResetBoard();
                sut.GameBoard[winCondition[0]] = 1;
                sut.GameBoard[winCondition[1]] = 1;
                sut.GameBoard[winCondition[2]] = 1;

                // assert
                Assert.True(sut.IsTheGameWin(1));
                Assert.False(sut.IsTheGameWin(0));
                Assert.False(sut.IsTheGameWin(-1));
            }

            // test
            sut.ResetBoard();
            foreach (int[] winCondition in sut.WinConditions)
            {
                sut.ResetBoard();
                sut.GameBoard[winCondition[0]] = -1;
                sut.GameBoard[winCondition[1]] = -1;
                sut.GameBoard[winCondition[2]] = -1;

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
            Game sut = new Game();
            Assert.False(sut.IsTheGameTied());

            // test
            sut.GameBoard = Enumerable.Repeat(1, 9).ToArray();

            // assert
            Assert.True(sut.IsTheGameTied());

            // test
            sut.GameBoard[0] = 0;

            // assert
            Assert.False(sut.IsTheGameTied());
        }

        [Fact]
        public void GetWinningMoveReturnWinningMoveIfAvailable()
        {
            // arrange
            Game sut = new Game();

            const int player = 1;
            int otherPlayer = sut.GetOtherPlayer(player);

            Assert.Null(sut.GetWinningMove(player));

            foreach (int[] winCondition in sut.WinConditions)
            {
                sut.ResetBoard();

                // no winning
                sut.GameBoard[winCondition[0]] = player;
                Assert.Null(sut.GetWinningMove(player));

                // winning
                sut.GameBoard[winCondition[1]] = player;
                Assert.Equal(winCondition[2], sut.GetWinningMove(player));

                // no winning
                sut.GameBoard[winCondition[2]] = otherPlayer;
                Assert.Null(sut.GetWinningMove(player));
            }
        }

        [Fact]
        public void MoveWillExecuteAMoveOnGameBoard()
        {
            // arrange
            Game sut = new Game();
            const int player = 1;
            const int position = 0;

            Assert.Equal(0, sut.GameBoard[position]);

            // test
            var move = sut.Move(player, position);

            Assert.Equal(position, move);
            Assert.Equal(player, sut.GameBoard[position]);
        }

        [Fact]
        public void MoveWillExecuteRandomValidMoveWhenMoveIsNull()
        {
            // arrange
            Game sut = new Game();
            const int player = 1;

            // test
            var move = sut.Move(player, null);
            Assert.Equal(player, sut.GameBoard[move]);

            // no valid moves - invalid condition
            sut.GameBoard = Enumerable.Repeat(1, 9).ToArray();
            Assert.Throws<InvalidOperationException>(() => sut.Move(player, null));
        }

        [Fact]
        public void GetOtherPlayerReturnsOtherPlayer()
        {
            // arrange
            Game sut = new Game();

            // test
            Assert.Equal(-1, sut.GetOtherPlayer(1));
            Assert.Equal(1, sut.GetOtherPlayer(-1));
        }

        [Fact]
        public void CalculateNextMoveWillMakeRandomMoveIfNoWinningConditionIsAvailable()
        {
            // arrange
            const int player = 1;
            var gameMock = new Mock<Game> {CallBase = true};

            gameMock.Setup(g => g.GetWinningMove(player)).Returns(() => null);
            gameMock.Setup(g => g.GetWinningMove(gameMock.Object.GetOtherPlayer(player))).Returns(() => null);
            gameMock.Setup(g => g.Move(player, null)).Verifiable();

            // test
            gameMock.Object.CalculateNextMove(player);

            // assert
            gameMock.Verify(game => game.Move(player, null), Times.Once());
        }

        [Fact]
        public void CalculateNextMoveWillExecuteWinningMoveForPlayerIfAvailable()
        {
            // arrange
            const int player = 1;
            const int move = 5;
            var gameMock = new Mock<Game> { CallBase = true };

            gameMock.Setup(g => g.GetWinningMove(player)).Returns(() => move);

            // test
            gameMock.Object.CalculateNextMove(player);

            // assert
            gameMock.Verify(game => game.GetWinningMove(gameMock.Object.GetOtherPlayer(player)), Times.Never);
            gameMock.Verify(game => game.Move(player, move), Times.Once());
            gameMock.Verify(game => game.Move(gameMock.Object.GetOtherPlayer(player), move), Times.Never());
        }

        [Fact]
        public void CalculateNextMoveWillExecuteBlockingMoveForPlayerIfAvailable()
        {
            // arrange
            const int player = 1;
            const int move = 5;
            var gameMock = new Mock<Game> { CallBase = true };

            gameMock.Setup(g => g.GetWinningMove(player)).Returns(() => null);
            gameMock.Setup(g => g.GetWinningMove(gameMock.Object.GetOtherPlayer(player))).Returns(() => move);

            // test
            gameMock.Object.CalculateNextMove(player);

            // assert
            gameMock.Verify(game => game.Move(player, move), Times.Once());
            gameMock.Verify(game => game.Move(gameMock.Object.GetOtherPlayer(player), move), Times.Never());
        }

        [Fact]
        public void GetRandomMoveReturnsRandomUnplayedCell()
        {
            // arrange
            Game sut = new Game();

            int position = (new Random().Next(0, 8));
            sut.GameBoard = Enumerable.Repeat(1, 9).ToArray();
            sut.GameBoard[position] = 0;

            // test
            int result = sut.GetRandomMove();

            // assert
            Assert.Equal(position, result);
        }
    }
}
