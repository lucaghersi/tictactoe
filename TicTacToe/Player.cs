using System;

namespace TicTacToe
{
    /// <summary>
    /// Represent a game player
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Initializes a new instace of <see cref="Player"/>
        /// </summary>
        /// <param name="name">The name of the player</param>
        /// <param name="code">The code of the player</param>
        /// <param name="token">The token used to represent the player on screen</param>
        public Player(string name, int code, string token)
        {
            if (code != 1 && code != -1)
                throw new InvalidOperationException("This is an invalid code!");

            Name = name;
            Code = code;
            Token = token;
        }

        /// <summary>
        /// The player's name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The player's code
        /// </summary>
        public int Code { get; private set; }
        /// <summary>
        /// The token used to represent the player on screen
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Check if this player equals another player
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected bool Equals(Player other)
        {
            return Code == other.Code;
        }

        /// <summary>
        /// Return the hash code that represents this <see cref="Player"/> object
        /// </summary>
        /// <returns>The code that represent this object</returns>
        public override int GetHashCode()
        {
            return Code;
        }

        /// <summary>
        /// Verify equality of this <see cref="Player"/> object with another object
        /// </summary>
        /// <param name="obj">The other object to evaluatre</param>
        /// <returns>True if they are equals or the same object, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Player) obj);
        }

        /// <summary>
        /// Returns a string representation of this <see cref="Player"/> object
        /// </summary>
        /// <returns>The string that represent this object</returns>
        public override string ToString()
        {
            return Token;
        }
    }
}