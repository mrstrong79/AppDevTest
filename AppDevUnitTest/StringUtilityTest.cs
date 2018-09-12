using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppDevTest;
using NUnit.Framework;

namespace AppDevUnitTest
{
    [TestFixture]
    public class StringUtilityTest
    {


        [Test]
        public void StringReverseTest()
        {
            string testString = "StringToReverse";
            string result = StringUtility.ReverseString(testString);
            Assert.IsTrue(result == "esreveRoTgnirtS");
            Console.WriteLine(result);
        }
    }

}
