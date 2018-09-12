using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AppDevTest
{
    public class RegExUtil
    {

        public static void CheckIsMatch(string[] args)
        {
            if (Regex.IsMatch(args[1], args[0]))
            {
                Console.WriteLine("We have a match!!!");
            }
            else
            {
                Console.WriteLine("No Deal!!!");
            }
        }

        public static void ReturnMatch()
        {
            string input = "Company Name: Contoso, Inc.";
            string pattern = @"Company Name: (.*)Inc.$"; // whatever is matched between the () becomes a group
            Match m = Regex.Match(input, pattern); // returns 'Contoso,'
            Console.WriteLine(m.Groups[1]);
        }


        public static void Matct_EndAtLastCharOfString()
        {
            string input = "EndAtLastCharOfString";
            Console.WriteLine(string.Format("Does this match: {0}?  {1}", input, Regex.IsMatch(input,@"String\z").ToString())); // True

            input = "EndAtLastCharOfStringOK";
            Console.WriteLine(string.Format("Does this match: {0}?  {1}", input, Regex.IsMatch(input, @"String\z").ToString())); //False

            input = "EndAtLastCharOfString\n";
            Console.WriteLine(string.Format("Does this match: {0}?  {1}", input, Regex.IsMatch(input, @"String\Z").ToString())); //True -- /Z also matches new line

        }

        public static void Matct_WordBoundary()
        {
            string input = "Word String Boundary";
            Console.WriteLine(string.Format("Does this match: {0}?  {1}", input, Regex.IsMatch(input, @"String\b").ToString())); // True -- match must occuer on a boundary

            input = "Word String Boundary";
            Console.WriteLine(string.Format("Does this match: {0}?  {1}", input, Regex.IsMatch(input, @"String\B").ToString())); //False -- /B match must not occur on a boundary...

            input = "Word String\n Boundary";
            Console.WriteLine(string.Format("Does this match: {0}?  {1}", input, Regex.IsMatch(input, @"String\b").ToString())); // True

        }

        // Lessone 3 ex 1
        public static void TestPhoneORPostCode()
        {
            string[] input = { "(555)555-1212", "(555) 555-1212", "555-555-1212", "5555551212", "01111", "01111-1111", "47", "111-11-1111" };
            foreach (string s in input)
            {
                if (IsPhone(s)) Console.WriteLine(ReformatPhone(s) + " is a phone number");
                else if (IsZip(s)) Console.WriteLine(s + " is a zip code");
                else Console.WriteLine(s + " is unknown");
            }
        }

        static bool IsPhone(string s)
        {
            return Regex.IsMatch(s, @"^\(?\d{3}\)?[\s\-]?\d{3}\-?\d{4}$");
        }

        static bool IsZip(string s)
        {
            return Regex.IsMatch(s, @"^\d{5}(\-\d{4})?$"); // (\-\d{4})? -- optionally match what's inside the brackets...
        }

        public static void IsEQID()
        {
            string p = @"^\d{10}[a-zA-Z]{1}$";
            List<string> lst = new List<string>();
            lst.Add("1234567890N");
            lst.Add("12345678901");
            lst.Add("V234567890N");
            lst.Add("12345678901N");
            lst.Add("12890N");
            lst.Add("1234t67890N");
            lst.Add("1234567890n");

            bool match;
            foreach (string s in lst)
            {
                match = Regex.IsMatch(s, p);
                Console.WriteLine("{0}: {1}", s, match.ToString());
            }
        }

        static string ReformatPhone(string s)
        {
            Match m = Regex.Match(s, @"^\(?(\d{3})\)?[\s\-]?(\d{3})\-?(\d{4})$");
            return string.Format("{0} {1} {2}", m.Groups[1], m.Groups[2], m.Groups[3]);
        }

        public static void GetSchoolAndCentreCodeStringSplit()
        {
            string s = @"This is a state school (232)";
            string[] sa = s.Split('(');
            string schoolName = sa[0].Trim();
            string centreCode = sa[1].TrimEnd(')', ' ');

            Console.WriteLine(schoolName);
            Console.WriteLine(centreCode);
        }

        public static void GetSchoolAndCentreCode()
        {
            string contentIem = @"This is a state school (232)";
            string p = @"(?[A-Za-z]+)*\W(?\d{2,5})";
           // Regex r = new Regex(@"(?[A-Za-z]+)(?\d{2,5})");
             Match m = Regex.Match(contentIem, p, RegexOptions.Multiline);
            //Regex r = new Regex(@"\d{2,5}");
            //Regex r = new Regex(@"\[(?<myMatch>([^\]])*)\]"); // without square brackets...
            //MatchCollection m = r.Matches(contentIem);
            //foreach (Match match in m)
            //{
            //    //Console.WriteLine(match.Groups["myMatch"]);
            //    Console.WriteLine(match);
            //} 
            Console.WriteLine(m.Groups[0]);
            Console.WriteLine(m.Groups[1]);
            Console.WriteLine(m.Groups[2]);
            Console.WriteLine(m.Groups[3]);
            Console.WriteLine(m.Groups[4]);
        }

        static string MatchSchoolAndCentreCode()
        {
            string schoolRegionalCode = "This is my schhool name (523) ";
            Match m = Regex.Match(schoolRegionalCode, @"^\(?(\d{3})\)?[\s\-]?(\d{3})\-?(\d{4})$");
            return string.Format("{0} {1} {2}", m.Groups[1], m.Groups[2], m.Groups[3]);
        }

        public static void GetPlaceHolders()
        {
            string contentIem =
                @"<div class='contentItem'>[title]</div>[myotherplaceholder]<div>[thisisanotherplaceholder]</div>";


             Regex r = new Regex(@"\[(.*?)\]");
             //Regex r = new Regex(@"\[(?<myMatch>([^\]])*)\]"); // without square brackets...
             MatchCollection m = r.Matches(contentIem);
             foreach (Match match in m)
             {
                 //Console.WriteLine(match.Groups["myMatch"]);
                 Console.WriteLine(match);
             }

        //    Regex r = new Regex(@"\[(.*?)\]");
         //   MatchCollection m = r.Matches(contentIem);

            //for (int i = 0; i < m.Count; i++)
            //{
            //    Console.WriteLine(m[i].Value);
            //}
        }

        // Regex using variables...
        public static void ReformatCustomerName()
        {
            string s = "First Name: Tom\n" +
                       "Last Name: Perham\n" +
                       "Address: 1 Pine St.\n" +
                       "City: Springfield\n" +
                       "State: MA\n" +
                       "Zip: 01332";

            string p = @"First Name: (?<firstName>.*$)\n" +
                       @"Last Name: (?<lastName>.*$)\n" +
                       @"Address: (?<address>.*$)\n" +
                       @"City: (?<city>.*$)\n" +
                       @"State: (?<state>.*$)\n" +
                       @"Zip: (?<zip>.*$)";
            Match m = Regex.Match(s, p, RegexOptions.Multiline);
            string fullName = m.Groups["firstName"] + " " + m.Groups["lastName"];
            string address = m.Groups["address"].ToString();
            string city = m.Groups["city"].ToString();
            string state = m.Groups["state"].ToString();
            string zip = m.Groups["zip"].ToString();

            Console.WriteLine(string.Format("Full Name: {0}",fullName));
            Console.WriteLine(string.Format("Address: {0}", address));
            Console.WriteLine(string.Format("City: {0}", city));
            Console.WriteLine(string.Format("State Name: {0}", state));
            Console.WriteLine(string.Format("Zip Name: {0}", zip));
        }

        public static void TestRegEx()
        {
            string p = @"^a(mo)+t.*z$";
            Console.WriteLine(p);
            string s = "amotz";
            Console.WriteLine(string.Format("{0} {1}", s, Regex.IsMatch(s,p))); // True
            s = "amomtrewz";
            Console.WriteLine(string.Format("{0} {1}", s, Regex.IsMatch(s, p)));// False
            s = "amotmoz";
            Console.WriteLine(string.Format("{0} {1}", s, Regex.IsMatch(s, p)));// True
            s = "atrewz";
            Console.WriteLine(string.Format("{0} {1}", s, Regex.IsMatch(s, p)));// False
            s = "amomomottohez";
            Console.WriteLine(string.Format("{0} {1}", s, Regex.IsMatch(s, p)));// True
        }

        public static void GetExitCodeLines()
        {
            string pathToFile = @"c:\temp\testwindowsupdate.log";
            StreamReader sr = new StreamReader(pathToFile);
            string output;
            while ((output = sr.ReadLine()) != null)
            {
                if (Regex.IsMatch(output, "exit"))
                Console.WriteLine(output);
            }
            sr.Close();
        }

    }
}
