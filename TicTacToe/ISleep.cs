namespace TicTacToe
{
    /// <summary>
    /// Provider sleep services
    /// </summary>
    public interface ISleep
    {
        /// <summary>
        /// Pause thread for the specified amout of time
        /// </summary>
        /// <param name="sleep"></param>
        void Wait(int sleep);
    }
}