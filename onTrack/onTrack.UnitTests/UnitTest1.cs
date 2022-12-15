using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace onTrack.UnitTests
{
    [TestClass]
    public class TimerTests
    {
        public string LimitTimerInput(string input)
        {
            string digits = input.Replace("m ", "");
            char[] digitArray = digits.ToCharArray();
            int lastIndex = digitArray.Length - 1;
            return digitArray[lastIndex - 3] + "" + digitArray[lastIndex - 2] + "m " + digitArray[lastIndex - 1] + "" + digitArray[lastIndex];
        }

        [TestMethod]
        public void LimitTimerInputTest()
        {
            string testInput = "00m 300";
            string sanitizedInput = LimitTimerInput(testInput);
            Assert.IsTrue(sanitizedInput.Equals("03m 00"));
        }
    }
}