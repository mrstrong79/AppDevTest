using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using AppDevTest.DomainObjects;

namespace AppDevTest
{
    class AppDevCollections
    {
        # region collections

        // Collections - ArrayList, Queue, Stack, StringCollection, BitArray, BitVector32 (Structure)

        public static void showItemsInArrayList()
        {
            ArrayList strList = new ArrayList();

            for (int i = 0; i < 100000; i++)
            {
                strList.Add(i.ToString());
            }

            DateTime startTime = DateTime.Now;
            Utilities.SpitOutContents(strList);
            //for (int j = 0; j < strList.Count; j++)
            //{
            //    Console.WriteLine(strList[j]);
            //}

            DateTime endTime = DateTime.Now;

            TimeSpan time = endTime.Subtract(startTime);
            Console.WriteLine(time.ToString());
        }

        public static void nonGenericCollections()
        {
            Comparer c = new Comparer(CultureInfo.CurrentCulture);

            string a = "different";
            string b = "same";

            Utilities.Writeln(c.Compare(a, b).ToString());

            a = "same";

            Utilities.Writeln(c.Compare(a, b));
            Utilities.Writeln(c.Compare(a.GetHashCode(), b.GetHashCode()));

            b = "different";

            Utilities.Writeln(c.Compare(a, b).ToString());

        }

        public static void showItemsInQueue()
        {
            Queue<string> strQueue = new Queue<string>();

            for (int i = 0; i < 100000; i++)
            {
                //strQueue.Add(i.ToString());
                strQueue.Enqueue(i.ToString());
            }

            DateTime startTime = DateTime.Now;

            while (strQueue.Count > 0)
            {
                Console.WriteLine(strQueue.Dequeue());
            }


            DateTime endTime = DateTime.Now;

            TimeSpan time = endTime.Subtract(startTime);
            Console.WriteLine(time.ToString());
        }

        public static void showItemsInStack()
        {
            Stack<string> strStack = new Stack<string>();

            for (int i = 0; i < 100000; i++)
            {
                strStack.Push(i.ToString());
            }

            DateTime startTime = DateTime.Now;


            //foreach (string str in strStack)
            //{
            //    Console.WriteLine(str);
            //}

            while (strStack.Count > 0)
            {
                Console.WriteLine(strStack.Pop());
            }

            DateTime endTime = DateTime.Now;

            TimeSpan time = endTime.Subtract(startTime);
            Console.WriteLine(time.ToString());
        }

        public static void BitVector32Test()
        {
            // Creates and initializes a BitVector32 with all bit flags set to FALSE.
            BitVector32 myBV = new BitVector32(64);

            // Creates masks to isolate each of the first five bit flags.
            int myBit1 = BitVector32.CreateMask();
            int myBit2 = BitVector32.CreateMask(myBit1);
            int myBit3 = BitVector32.CreateMask(myBit2);
            int myBit4 = BitVector32.CreateMask(myBit3);
            int myBit5 = BitVector32.CreateMask(myBit4);

            // Sets the alternating bits to TRUE.
            Console.WriteLine("Setting alternating bits to TRUE:");
            Console.WriteLine("   Initial:         {0}", myBV.ToString());
            myBV[myBit1] = true;
            Console.WriteLine("   myBit1 = TRUE:   {0}", myBV.ToString());
            myBV[myBit3] = true;
            Console.WriteLine("   myBit3 = TRUE:   {0}", myBV.ToString());
            myBV[myBit5] = true;
            Console.WriteLine("   myBit5 = TRUE:   {0}", myBV.ToString());
        }

        public static void BitVector32InitialisationTest()
        {
            BitVector32 bvOff = new BitVector32(); // initialise all to false
            BitVector32 bvOn = new BitVector32(-1); // initialise all to true
            BitVector32 bvDemo15 = new BitVector32(15);
            Console.WriteLine("Output:");
            Console.WriteLine("bvOff    = {0}", bvOff.ToString());
            Console.WriteLine("bvOn     = {0}", bvOn.ToString());
            Console.WriteLine("bvDemo15 = {0}", bvDemo15.ToString());
            //Output:
            //bvOff    = BitVector32{00000000000000000000000000000000}
            //bvOn     = BitVector32{11111111111111111111111111111111}
            //bvDemo15 = BitVector32{00000000000000000000000000001111}

        }

        public static void BitArrayTest()
        {
            BitArray myBits = new BitArray(3);
            myBits.Set(0, true);
            myBits.Set(1, false);
            myBits.Set(2, true);
            myBits.Length = 5;
            myBits.Set(4, true);

            Console.WriteLine("3rd bit: " + myBits.Get(2));

            foreach (bool b in myBits)
            {
                Console.WriteLine(b);
            }

        }

#endregion

#region Dictionaries
        // Dictionaries - Hashtable, SortedList, StringDictionary, ListDictionary, HybridDictionary, NameValueCollection

        public static void CollectionsUtilExample()
        {
            Hashtable caseInsensitiveHashTable = CollectionsUtil.CreateCaseInsensitiveHashtable();
           // Hashtable t = new Hashtable();

            caseInsensitiveHashTable.Add("key", "value1");
            caseInsensitiveHashTable.Add("Key2", "value2");

            Utilities.Writeln(caseInsensitiveHashTable["Key"].ToString()); // outputs value1
        }

        public static void SortedListExample()
        {
            SortedList sl = new SortedList();
            sl.Add("Stack","Represents a LIFO Collection of objects");
            sl.Add("Queue", "Represents a FIFO Collection of objects");
            sl.Add("SortedList", "Represents a Collection of key/value pairs");

            foreach (DictionaryEntry d in sl)
            {
                Utilities.Writeln(d.Value); // prints out Queue, SortedList, Stack - in that order...
            }
        }

        public static void OrderedListExample()
        {
            OrderedDictionary od = new OrderedDictionary();
            od.Add("1", "One");
            od.Add("2", "Two");
            od.Add("4", "Four");
            od.Add("5", "Five");
            od.Insert(2, "3", "Three");
            od.Insert(od.Count, "6", "Six");

            for (int x = 0; x < od.Count; x++)
                Console.WriteLine("{0}", od[x]);

            foreach (DictionaryEntry de in od)
                Console.WriteLine("{0} = {1}", de.Key, de.Value);
        }

        public static void NameValuedCollectionExample()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("city", "New York");
            nvc.Add("country", "U.S.");
            nvc.Add("city", "California");
            nvc.Remove("country");
            nvc.Add("country", "United States");

            foreach (string key in nvc.AllKeys)
                Console.WriteLine("{0} = {1}", key, nvc.Get(key)); // City = New York,California\nCountry = United States

            // alternate way)
            for (int x = 0; x < nvc.Count; x++)
            {
                Console.WriteLine(nvc.Get(x));
            }
        }



#endregion

        public static void TestForEachLoop()
        {
            List<string> strings = new List<string>();
            foreach (string s in strings)
            {
                Console.WriteLine(s);
            }
        }

        public static void ShowOutPutInherit()
        {
            DerivedClass.ShowClassName();
        }

        public static void testGeneric()
        {
            //create a string version of our generic class
            Col<string> mystring = new Col<string>();
            //set the value
            mystring.Val = "hello";

            //output that value
            Console.WriteLine(mystring.Val);
            //output the value's type
            Console.WriteLine(mystring.Val.GetType());

            //create another instance of our generic class, using a different type
            Col<int> myint = new Col<int>();
            //load the value
            myint.Val = 5;
            //output the value
            Console.WriteLine(myint.Val);
            //output the value's type
            Console.WriteLine(myint.Val.GetType());
        }

        public static void showItemsInList()
        {
            List<string> strList = new List<string>();

            for (int i = 0; i < 100000; i++)
            {
                strList.Add(i.ToString());
            }

            DateTime startTime = DateTime.Now;

            for (int j = 0; j < strList.Count; j++)
            {
                Console.WriteLine(strList[j]);
            }

            DateTime endTime = DateTime.Now;

            TimeSpan time = endTime.Subtract(startTime);
            Console.WriteLine(time.ToString());
        }



        public static void GenericSortedListExample()
        {
            System.Collections.Generic.SortedList<string, string> sl = new SortedList<string, string>();
            sl.Add("MKT", "Marketing");
            sl.Add("SAL", "Sales");
            sl.Add("FIN", "Finance");
            sl.Add("LOG", "Logistics");

            foreach (var item in sl.Keys)
            {
                Utilities.Writeln(item);
            }
        }

        public static void LinkedListExample()
        {
            LinkedList<string> llString = new LinkedList<string>(new string[] {"to", "what" });
            llString.AddFirst("Test");
            llString.AddLast("Happens");
            llString.AddAfter(llString.Find("to"), "see");
            foreach(string s in llString.ToList())
            {
                Console.WriteLine(s);
            }

        }

        public static void LinkedListExample2()
        {
            string s1 = "nissan";
            string s2 = "honda";
            string s3 = "mit";
            string s4 = "maz";
            string s5 = "toyota";

            LinkedList<string> llString = new LinkedList<string>();
            LinkedListNode<string> node;
            node = llString.AddFirst(s1);
            node = llString.AddLast(s2);
            node = llString.AddAfter(node, s3);
            node = llString.AddBefore(node, s4);
            llString.AddLast(s5);

            foreach (string s in llString.ToList())
            {
                Console.Write(s);
            }

        }

        public static void SortedDictionaryExample()
        {
            SortedDictionary<string, Department> deptSD = new SortedDictionary<string, Department>();
            deptSD.Add("MKT", new Department("MKT", "Marketing"));
            deptSD.Add("SAL", new Department("SAL", "Sales"));
            deptSD.Add("FIN", new Department("FIN", "Finance"));
            deptSD.Add("LOG", new Department("LOG", "Logistics"));
            Department d;
            foreach (KeyValuePair<string, Department> kvp in deptSD)
            {
                d = kvp.Value;
                Console.WriteLine("{0}: {1}", d.Code, d.Name);
            }
        }

        public static void ListNumbersFromArray()
        {
            int[] intArray = { 3, 4, 2, 9, 4, 0 };

            Array.Sort(intArray);

            for (int i = 0; i < intArray.Length; i++)
            {
                Console.WriteLine(Convert.ToString(intArray[i]));
            }

            //foreach (int i in intArray)
            //{
            //    Console.WriteLine(Convert.ToString(i));
            //}
        }

        public static void StringArray()
        {
            string s = "Microsoft .Net Framework 2.0 Application Development Foundation";
            string[] strArr = s.Split(' ');

            Array.Sort(strArr);

            s = String.Join(" ", strArr);

            Console.WriteLine(s);
        }
    }

    public class CustomNonGenericDictionary : DictionaryBase
    {
        
    }

    public class CustomGenericDictionary : Dictionary<int, string>
    {
        
    }
}
