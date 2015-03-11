using System;

namespace TicTacToe
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utility
    {
        /// <summary>
        ///     Write a message to the standard output.
        ///     This method is virtual to allow mocking during testing.
        /// </summary>
        /// <param name="message">The message to write</param>
        /// <param name="newLine">True to add a newline after the message, false otherwise</param>
        public static void OutputMessage(string message, bool newLine = false)
        {
            Console.Write(message);
            if (newLine) Console.WriteLine();
        }
    }
}
