namespace AppDevTest.DomainObjects
{
    class DerivedClass : BaseClass
    {
        private static string name = "Derived Class";
        public static void ShowClassName()
        {
            ShowClassName(name);
        }
    }
}
