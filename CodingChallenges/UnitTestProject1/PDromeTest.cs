using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodingChallenges;

namespace UnitTestProject1
{
    [TestClass]
    public class PDromeTest
    {
        [TestMethod]
        public void PTest1()
        {
            string a = "noon";
            bool expectedValue = true;
            bool actualValue;
            actualValue = Program.IsPalindrome(a);
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void PTest2()
        {
            string b = "2112";
            bool expectedValue = true;
            bool actualValue;
            actualValue = Program.IsPalindrome(b);
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
