using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AppDevTest
{
    public class EncodingUtil
    {
        private const string tempDir = @"c:\temp\";

        public static void GetKoreanEncoding()
        {
            // C#
            // Get Korean encoding
            Encoding e = Encoding.GetEncoding("Korean");
            // Convert ASCII bytes to Korean encoding
            byte[] encoded = e.GetBytes("Hello, world!");
            // Display the byte codes
            for (int i = 0; i < encoded.Length; i++)
                Console.WriteLine("Byte {0}: {1}", i, encoded[i]);
        }

        public static void GetEncodings()
        {
            EncodingInfo[] ei = Encoding.GetEncodings();
            foreach (EncodingInfo e in ei)
                Console.WriteLine("{0}: {1}, {2}", e.CodePage, e.Name, e.DisplayName);

            Console.WriteLine(ei.Count().ToString());
        }

        public static void CreateEncodedFiles()
        {
            StreamWriter swUtf7 = new StreamWriter(tempDir + "utf7.txt",false, Encoding.UTF7);
            swUtf7.WriteLine("Hello, World!");
            swUtf7.Close();
            StreamWriter swUtf8 = new StreamWriter(tempDir + "utf8.txt", false, Encoding.UTF8);
            swUtf8.WriteLine("Hello, World!");
            swUtf8.Close();
            StreamWriter swUtf16 = new StreamWriter(tempDir + "utf16.txt", false, Encoding.Unicode);
            swUtf16.WriteLine("Hello, World!");
            swUtf16.Close();
            StreamWriter swUtf32 = new StreamWriter(tempDir + "utf32.txt", false, Encoding.UTF32);
            swUtf32.WriteLine("Hello, World!");
            swUtf32.Close();
        }

        public static void ShowEncodings() // MSDN example
        {

            // Print the header.
            Console.Write("Name               ");
            Console.Write("CodePage  ");
            Console.Write("BodyName           ");
            Console.Write("HeaderName         ");
            Console.Write("WebName            ");
            Console.WriteLine("Encoding.EncodingName");

            // For every encoding, compare the name properties with EncodingInfo.Name.
            // Display only the encodings that have one or more different names.
            foreach (EncodingInfo ei in Encoding.GetEncodings())
            {
                Encoding e = ei.GetEncoding();

                if ((ei.Name != e.BodyName) || (ei.Name != e.HeaderName) || (ei.Name != e.WebName))
                {
                    Console.Write("{0,-18} ", ei.Name);
                    Console.Write("{0,-9} ", e.CodePage);
                    Console.Write("{0,-18} ", e.BodyName);
                    Console.Write("{0,-18} ", e.HeaderName);
                    Console.Write("{0,-18} ", e.WebName);
                    Console.WriteLine("{0} ", e.EncodingName);
                }

            }

        }


    }
}
