using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Management;
using System.Diagnostics;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Reflection;
using System.Globalization;
using AppDevTest.DomainObjects;


namespace AppDevTest
{
    class Utilities
    {
        public static void UnaryOperators()
        {
            int unary = 0;
            int preIncrement;
            int preDecrement;
            int postIncrement;
            int postDecrement;
            int positive;
            int negative;
            sbyte bitNot;
            bool logNot;

            preIncrement = ++unary;
            Console.WriteLine("pre-Increment: {0}", preIncrement);

            preDecrement = --unary;
            Console.WriteLine("pre-Decrement: {0}", preDecrement);

            postDecrement = unary--;
            Console.WriteLine("Post-Decrement: {0}", postDecrement);

            postIncrement = unary++;
            Console.WriteLine("Post-Increment: {0}", postIncrement);

            Console.WriteLine("Final Value of Unary: {0}", unary);

            positive = -postIncrement;
            Console.WriteLine("Positive: {0}", positive);

            negative = +postIncrement;
            Console.WriteLine("Negative: {0}", negative);

            bitNot = 0;
            bitNot = (sbyte)(~bitNot);
            Console.WriteLine("Bitwise Not: {0}", bitNot);

            logNot = false;
            logNot = !logNot;
            Console.WriteLine("Logical Not: {0}", logNot);
        }








        public static void Writeln(object WriteString)
        {
            Console.WriteLine(WriteString);
        }

        public static void SpitOutContents(IEnumerable collection)
        {
            foreach (var obj in collection)
            {
                Console.WriteLine(obj);
            }
        }





        public static void testDelegate()
        {
            DelegateToMethod aDelegate = new DelegateToMethod(Math.Add);
            DelegateToMethod mDelegate = new DelegateToMethod(Math.Multiply);
            DelegateToMethod dDelegate = new DelegateToMethod(Math.Divide);
            Console.WriteLine("Calling the method Math.Add() through the aDelegate object");
            Console.WriteLine(aDelegate(5, 5));
            Console.WriteLine("Calling the method Math.Multiply() through the mDelegate object");
            Console.WriteLine(mDelegate(5, 5));
            Console.WriteLine("Calling the method Math.Divide() through the dDelegate object");
            Console.WriteLine(dDelegate(5, 5));
        }


        public static void ShowCurrentDomainName()
        {
            Console.WriteLine("Domain Name is: " + System.Environment.UserDomainName);
        }

        public static void testIntsAsBools()
        {
            int falseVal = 0; // false
            int trueVal = 1; // true
            int other = 234; // true
            int negVal = -1;

            ConvertIntToBool(falseVal);
            ConvertIntToBool(trueVal);
            ConvertIntToBool(other);
            ConvertIntToBool(negVal);

            // Produces the following output:
            //When converted to a bool this integer is 'False' : 0
            //When converted to a bool this integer is 'True' : 1
            //When converted to a bool this integer is 'True' : 234
            //When converted to a bool this integer is 'True' : -1
        }

        public static void ConvertIntToBool(int val)
        {
            if (Convert.ToBoolean(val))
            {
                Console.WriteLine("When converted to a bool this integer is 'True' : {0}", val.ToString());
                return;
            }
            Console.WriteLine("When converted to a bool this integer is 'False' : {0}", val.ToString());
        }


        public static void displayCurrencyValue()
        {
            int moneyValue = 10000;
            Console.WriteLine("This is the currency value: {0:C}", moneyValue);
            // Output:  This is the currency value: $10,000.00
        }



        public static void displayEven()
        {
            List<int> lst = new List<int>();
            lst.Add(1);
            lst.Add(2);
            lst.Add(3);
            lst.Add(4);
            lst.Add(5);
            lst.Add(6);
            List<int> lst2 = new List<int>();
            lst2 = lst.FindAll(FindValues);

            foreach (int i in lst2)
            {
                Console.WriteLine(i.ToString());
            }
        }

        public static bool FindValues(int value)
        {
            if ((value % 2) == 0)
                return true;
            else
                return false;
        }

        public static void showWindowsAccount()
        {
            string strUser = System.Security.Principal.WindowsIdentity.GetCurrent().Token.ToString();
            //string strUser = System.Security.Principal.WindowsPrincipal.
            Console.WriteLine(strUser);
        }

        public static void showWindowsPrincipal()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            IPrincipal principal = Thread.CurrentPrincipal;
            Console.WriteLine("Current principal: " + principal.GetType());

            if (typeof(WindowsPrincipal).IsAssignableFrom(principal.GetType()))
            {
                WindowsPrincipal windowsPrinciple = (WindowsPrincipal)principal;
                Console.WriteLine("Principal identity is: " + windowsPrinciple.Identity.Name);
            }
            else
            {
                GenericPrincipal genericPrincipal = (GenericPrincipal)principal;
                Console.WriteLine("Principal identity is: " + genericPrincipal.Identity.Name);
            }
        }

        public static void showGroupsForCurrentUser()
        {
            // get the SIDs for my groups
            IdentityReferenceCollection myGroups = WindowsIdentity.GetCurrent().Groups;

            // translate them into NTaccount types
            IdentityReferenceCollection myGroups2 = myGroups.Translate(typeof(NTAccount));

            foreach (IdentityReference ir in myGroups2)
            {
                Console.WriteLine(ir.ToString());
            }
        }

        public static void encrypt()
        {
            string inFileName = @"C:\temp\SerializedString.Data";
            string outFileName = @"C:\temp\SerializedString.Data.enc";

            // Step 1: Create the Stream objects 
            FileStream inFile = new FileStream(inFileName, FileMode.Open, FileAccess.Read); 
            FileStream outFile = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
            
            // Step 2: Create the SymmetricAlgorithm object 
            RijndaelManaged myAlg = new RijndaelManaged();

            // Step 3: Specify a key (optional) 
            myAlg.GenerateKey();

            // Read the unencrypted file into fileData 
            byte[] fileData = new byte[inFile.Length];
            inFile.Read(fileData, 0, (int)inFile.Length);

            // Step 4: Create the ICryptoTransform object 
            ICryptoTransform encryptor = myAlg.CreateEncryptor();

            // Step 5: Create the CryptoStream object 
            CryptoStream encryptStream = new CryptoStream(outFile, encryptor, CryptoStreamMode.Write);

            // Step 6: Write the contents to the CryptoStream 
            encryptStream.Write(fileData, 0, fileData.Length);

            // Close the file handles 
            encryptStream.Close(); 
            inFile.Close(); 
            outFile.Close();
        }

        public static void createRSAKey()
        {
            // Create an instane of the RSA Algorithm object
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider();

            // Create a new RSAParameters object with only the public key
            RSAParameters publicKey = myRSA.ExportParameters(false);

            Console.WriteLine(publicKey.Exponent.ToString());
            Console.WriteLine(publicKey.Modulus.ToString());
            Console.WriteLine(publicKey.GetHashCode().ToString());

        }

        public static void CryptoStreamExample()
        {
            string cardInfo = "Hall, Don; 4455-5566-6677-8899; 09/2008";
            //RC2CryptoServiceProvider encrypter = new RC2CryptoServiceProvider();
            RijndaelManaged encrypter = new RijndaelManaged();
            FileStream fileStream = File.Open(@"c:\temp\ProtectedCardInfo2.txt",
            FileMode.OpenOrCreate);
            CryptoStream cryptoStream = new CryptoStream(fileStream,
            encrypter.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            try
            {
                writer.WriteLine(cardInfo);
                Console.WriteLine("Card information successfully written to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred when writing to the file.");
            }
            finally
            {
                cryptoStream.Close();
                cryptoStream.Dispose();
                cryptoStream = null;
                encrypter.Clear();
                encrypter = null;
                fileStream.Close();
                fileStream.Dispose();
                fileStream = null;
            }

        }

        public static void StringBuilderExample()
        {
            StringBuilder sb = new StringBuilder(5);
            Console.WriteLine(sb.ToString());
            Console.WriteLine("Length = {0}", sb.Length);
            Console.WriteLine("Capacity = {0}", sb.Capacity);
            sb.Append("01234567890123456789");
            Console.WriteLine(sb.ToString());
            Console.WriteLine("Length = {0}", sb.Length);
            Console.WriteLine("Capacity = {0}", sb.Capacity);
            sb.Length = 10;
            Console.WriteLine(sb.ToString());
            Console.WriteLine("Length = {0}", sb.Length);
            Console.WriteLine("Capacity = {0}", sb.Capacity);
            sb.Append("AB");
            Console.WriteLine(sb.ToString());
            Console.WriteLine("Length = {0}", sb.Length);
            Console.WriteLine("Capacity = {0}", sb.Capacity);

        // Produces the following output:
        //
        //Length = 0
        //Capacity = 5
        //01234567890123456789
        //Length = 20
        //Capacity = 20
        //0123456789
        //Length = 10
        //Capacity = 20
        //0123456789AB
        //Length = 12
        //Capacity = 20
        }

        public static void StringBuilderStringWriter()
        {
            StringBuilder sb = new StringBuilder("string");
            char[] c = new char[] { 'a', 'b', 'c', 'd', 'e', 'f'};
            StringWriter sw = new StringWriter(sb);
            sw.Write(c, 0, 3);
            Console.WriteLine(sw.ToString());

            // Output:
            // stringabc

        }

        public static void TestReferenceTypeCopy()
        {
            Person p1 = new Person { Age = 40, DOB = DateTime.Parse("30/6/2010"), EyeColour = "blue", HasSiblings = false, FirstName = "John", LastName = "Smith"};
            Person p2 = p1;

            p2.FirstName = "Jane";
            p2.LastName = "Smith";
            p2.Age = 38;

            Writeln(p1.Name);
            Writeln(p1.Age);
            Writeln(p2.Name);
            Writeln(p2.Age);
        }

        public static void TestValueTypeCopy()
        {
            int i = 99;
            int j = i;

            j++;
            Writeln(i.ToString());
            Writeln(j.ToString());
        }

        public static void CloneTest()
        {
            Person p1 = new Person { Age = 40, DOB = DateTime.Parse("30/6/2010"), EyeColour = "blue", HasSiblings = false, FirstName = "John", LastName = "Smith" };
            Person p2 = (Person)p1.Clone();

            p2.FirstName = "Jane";
            p2.LastName = "Smith";
            p2.Age = 38;



            Writeln(p1.FullName);
            Writeln(p1.Age);
            Writeln(p2.FullName);
            Writeln(p2.Age);
            SetCloneTest(p2);
            Writeln(p2.FullName);
            Writeln(p2.Age);

            //Produces:
            //John Smith
            //40
            //Jane Smith
            //38
            //Janet Brown
            //38
        }

        public static void SetCloneTest(Person p)
        {
            p.FirstName = "Janet";
            p.LastName = "Brown";
        }

        public static void TestMethod()
        {
            string s = "This is set from the originating method";
            Writeln(s);
            SetMethod(s);
            Writeln(s);

            // Produces output:
            //This is set from the originating method
            //This is set from the originating method
        }

        public static void SetMethod(string s)
        {
            s = "This is set from the set method";
        }



    }


    public delegate int DelegateToMethod(int x, int y);

    public class Dude
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private int _age;

        public string FirstName
        {
            get {return _firstName;}
            set {_firstName = value;}
        }

        public string LastName
        {
            get {return _lastName;}
            set {_lastName = value;}
        }
    }

    public struct PersonStruct
    {
        string _firstName;
        string _lastName;
        int _age;
        Genders _gender;

        public PersonStruct(string firstName, string lastName, int age, Genders gender)
        {
            _firstName = firstName;
            _lastName = lastName;
            _age = age;
            _gender = gender;
        }

        public override string ToString()
        {
            string returnVal = _firstName + _lastName + _age.ToString() + _gender.ToString();
            return returnVal;
        }
    }


    public enum Genders : int { Male, Female };

    interface IMessage
    {
        bool Send();
        string Message { get; set; }
        string To { get; set; }
    }

    public class InterFaceClass : IDisposable, IMessage
    {

        public InterFaceClass()
        {

        }

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMessage Members

        public bool Send()
        {
            throw new NotImplementedException();
        }

        public string Message
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string To
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    public class Col<T>
    {
        T t;
        public T Val { get { return t; } set { t = value; } }
    }

    public class Math
    {
        public static int Add(int first, int second)
        {
            return first + second;
        }

        public static int Multiply(int first, int second)
        {
            return first * second;
        }

        public static int Divide(int first, int second)
        {
            return first / second;
        }
    }


    public class myReverserClass : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer.Compare(Object x, Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(y, x));
        }
    }






}
