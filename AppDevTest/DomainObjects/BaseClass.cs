using System;

namespace AppDevTest.DomainObjects
{
    class BaseClass
    {
        private const string name = "Base Class";

        public static void ShowClassName()
        {
            ShowClassName(name);
        }

        public static void ShowClassName(string str)
        {
            Console.WriteLine(str);
        }

        }
}
