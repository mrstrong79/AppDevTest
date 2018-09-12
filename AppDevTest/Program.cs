using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDevTest
{
    class Program
    {

       // [STAThread] -- run as single apartment thread (ie one thread)
        static void Main(string[] args)
        {
            Utilities.CloneTest();
            Console.ReadKey();
        }
    }
}
