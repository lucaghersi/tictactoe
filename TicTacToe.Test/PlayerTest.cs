using System;
using Xunit;

namespace TicTacToe.Test
{
    public class PlayerTest
    {
        [Fact]
        public void PlayersWithSameCodeAreTheSamePlayer()
        {
            // arrange
            Player alice = new Player("Alice", -1, "X");
            Player bob = new Player("Bob", -1, "X");

            // test
            Assert.Equal(alice, bob);
        }

        [Fact]
        public void PlayersWithDifferentCodesAreDifferentPlayers()
        {
            // arrange
            Player alice = new Player("Alice", -1, "X");
            Player bob = new Player("Bob", 1, "X");

            // test
            Assert.NotEqual(alice, bob);
        }

        [Fact]
        public void PlayerDoNotAcceptInvalidCodes()
        {
            Assert.Throws<InvalidOperationException>(() => new Player("Wrong", 10, "X"));
        }

        [Fact]
        public void PlayerToStringReturnPlayerToken()
        {
            // arrange
            Player alice = new Player("Alice", -1, "X");

            // assert
            Assert.Equal(alice.Token, alice.ToString());
        }
    }
}
