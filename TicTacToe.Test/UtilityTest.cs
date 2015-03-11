using System;
using System.IO;
using System.Text;
using Xunit;

namespace TicTacToe.Test
{
    public class UtilityTest
    {
        [Fact]
        public void OutputMessagePushMessageToConsole()
        {
            // arrange
            const string expected = "hello!";

            // test
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Utility.OutputMessage(expected);
                Assert.Equal(expected, sw.ToString());
            }
        }

        [Fact]
        public void OutputMessagePushMessageToConsoleWithNewLine()
        {
            // arrange
            const string expected = "hello!";
            StringBuilder sb = new StringBuilder(expected);
            sb.AppendLine();

            // test
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                Utility.OutputMessage(expected, true);
                Assert.Equal(sb.ToString(), sw.ToString());
            }
        }
    }
}
