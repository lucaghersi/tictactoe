namespace TicTacToe
{
    /// <summary>
    /// Represent a game player
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// The player's name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The player's code
        /// </summary>
        int Code { get; }

        /// <summary>
        /// The token used to represent the player on screen
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Return the hash code that represents this <see cref="Player"/> object
        /// </summary>
        /// <returns>The code that represent this object</returns>
        int GetHashCode();

        /// <summary>
        /// Verify equality of this <see cref="Player"/> object with another object
        /// </summary>
        /// <param name="obj">The other object to evaluatre</param>
        /// <returns>True if they are equals or the same object, false otherwise</returns>
        bool Equals(object obj);

        /// <summary>
        /// Returns a string representation of this <see cref="Player"/> object
        /// </summary>
        /// <returns>The string that represent this object</returns>
        string ToString();
    }
}