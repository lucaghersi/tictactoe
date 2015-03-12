using System.Threading;

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

    /// <summary>
    /// Provider sleep services
    /// </summary>
    public class Sleep : ISleep
    {
        /// <summary>
        /// Pause thread for the specified amout of time
        /// </summary>
        /// <param name="sleep"></param>
        public void Wait(int sleep)
        {
            sleep = sleep < 1 ? 1000 : sleep;

            Thread.Sleep(sleep);
        }
    }
}
