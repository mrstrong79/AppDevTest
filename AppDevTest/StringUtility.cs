using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDevTest
{
    public class StringUtility
    {
        public static void StringBuilderEx()
        {
            string s = null;
            StringBuilder sb = null;
            sb.Append("Hello");
            sb.Append("World");
            Console.WriteLine(sb.ToString());
        }

        public static void splitExample()
        {
            string url = @"/thisismyurl/asdfasdf/blahblah/thisshouldgetreturned";
            string[] strArr = url.Split('/');
            Console.WriteLine(strArr.Last());
        }

        public static void stringAppendExample()
        {
            string myString = "Testsdfasdfasdfa.aspx";

            if (!myString.EndsWith(".aspx"))
            {
                myString += ".aspx";
            }
            Console.WriteLine(myString);

        }

        public static string ReverseString(string input)
        {
            char[] chars = input.ToCharArray();
            string retVal = string.Empty;
            for (int i = (chars.Length - 1); i >= 0;i--)
            {
                retVal = retVal + chars[i].ToString();
            }
            return retVal;
        }
    }
}
