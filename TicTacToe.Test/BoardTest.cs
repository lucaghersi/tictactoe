using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace TicTacToe.Test
{
    public class BoardTest
    {
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
        public void InitializeCorrectlyInitializeBoard()
        {
            // arrange
            Board sut = new Board();

            // test
            sut.Initialize(GetPlayersList()[0], GetPlayersList()[1]);

            // assert
            Assert.Equal(Enumerable.Repeat(0, 9).ToArray(), sut.GameBoard);
            Assert.Equal(GetPlayersList(), sut.Players);
        }

        [Fact]
        public void InitializeBlocksWrongInitialization()
        {
            // arrange
            Board sut = new Board();

            // assert
            Assert.Throws<ArgumentNullException>(() => sut.Initialize(null, GetPlayersList()[1]));
            Assert.Throws<ArgumentNullException>(() => sut.Initialize(GetPlayersList()[0], null));
        }

        [Fact]
        public void ResetBoardWillRestoreEmptyBoard()
        {
            // arrange
            Board sut = GetBoard();
            sut[0] = 1;

            // test
            sut.ResetBoard();

            // assert
            Assert.Equal(Enumerable.Repeat(0, 9).ToArray(), sut.GameBoard);
        }

        [Fact]
        public void ResetBoardWithCodeWillSetThatValueOnAllBoard()
        {
            // arrange
            Board sut = GetBoard();            

            // test
            sut.ResetBoard(1);

            // assert
            Assert.Equal(Enumerable.Repeat(1, 9).ToArray(), sut.GameBoard);
        }

        [Fact]
        public void GetPlayerReturnsValidPlayerFromList()
        {
            // arrange
            Board sut = GetBoard();

            // assert
            Assert.Equal(GetPlayersList().Single(p => p.Code == -1), sut.GetPlayer(-1));
        }

        [Fact]
        public void GetPlayerTokenReturnsCorrectPlayerToken()
        {
            // arrange
            var players = GetPlayersList();
            string expect = players[0].Token;

            Board sut = GetBoard();

            // assert
            Assert.Equal(expect, sut.GetPlayerToken(players[0].Code));
        }


        [Fact]
        public void DrawBoardDrawsCorrectBoard()
        {
            // arrange
            Board sut = GetBoard();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" {0} | {1} | {2}",
                sut.GetPlayerToken(sut[0]),
                sut.GetPlayerToken(sut[1]),
                sut.GetPlayerToken(sut[2]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}",
                sut.GetPlayerToken(sut[3]),
                sut.GetPlayerToken(sut[4]),
                sut.GetPlayerToken(sut[5]));
            sb.AppendLine();
            sb.AppendFormat("---|---|---");
            sb.AppendLine();
            sb.AppendFormat(" {0} | {1} | {2}",
                sut.GetPlayerToken(sut[6]),
                sut.GetPlayerToken(sut[7]),
                sut.GetPlayerToken(sut[8]));
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
        public void GetRandomEmptyCellReturnsRandomUnplayedCell()
        {
            // arrange
            Board sut = GetBoard();

            int position = (new Random().Next(0, 8));
            sut.ResetBoard(1);
            sut[position] = 0;

            // test
            int result = sut.GetRandomEmptyCell();

            // assert
            Assert.Equal(position, result);
        }
    }
}
