using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AppDevTest.DomainObjects;

namespace AppDevTest
{
    class AppDevSerialization
    {
        #region binary formatter
        public static void bfSerialize()
        {
            string data = "This must be stored in a file.";

            // Create file to save the data to 
            FileStream fs = new FileStream(@"c:\temp\SerializedString.Binary", FileMode.Create);

            // Create a BinaryFormatter object to perform the serialization 
            BinaryFormatter formatter = new BinaryFormatter();

            // Use the BinaryFormatter object to serialize the data to the file 
            formatter.Serialize(fs, data);

            // Close the file 
            fs.Close();
        }

        public static void bfSerializeDate()
        {
            // Create file to save the data to 
            FileStream fs = new FileStream(@"c:\temp\SerializedDate.Binary", FileMode.Create);

            // Create a BinaryFormatter object to perform the serialization 
            BinaryFormatter formatter = new BinaryFormatter();

            // Use the BinaryFormatter object to serialize the data to the file 
            formatter.Serialize(fs, DateTime.Now);

            // Close the file 
            fs.Close();
        }

        public static void bfDeSerialize()
        {
            FileStream fs = new FileStream(@"c:\temp\SerializedString.Binary", FileMode.Open);

            // Create a BinaryFormatter object to perform the serialization 
            BinaryFormatter formatter = new BinaryFormatter();

            // Use the BinaryFormatter object to serialize the data to the file 
            string data = (string)formatter.Deserialize(fs);

            Console.WriteLine(data);

            // Close the file 
            fs.Close();
        }

        #endregion

        #region Soap Serialization

        public static void soapSerializeData()
        {
            string data = "This must be stored in a file.";

            // Create file to save the data to 
            FileStream fs = new FileStream(@"c:\temp\SerializedString.Soap", FileMode.Create);

            // Create a Formatter object to perform the serialization 
            SoapFormatter formatter = new SoapFormatter();

            // Use the BinaryFormatter object to serialize the data to the file 
            formatter.Serialize(fs, data);

            // Close the file 
            fs.Close();
        }

        #endregion

        #region Xml Serialization

        public static void xSerialize()
        {
            Dude d = new Dude();
            d.FirstName = "Jeff";
            d.LastName = "Price";
            XmlSerializer x = new XmlSerializer(d.GetType());
            x.Serialize(Console.Out, d);
        }

        public static void SerializePersonToXML()
        {
            List<Person> people = new List<Person>();
            people.Add(new Person { Age = 40, DOB = DateTime.Parse("30/6/2010"), EyeColour = "blue", HasSiblings = false, FirstName = "John", LastName = "Smith" });
            people.Add(new Person { Age = 40, DOB = DateTime.Parse("1/2/1940"), EyeColour = "brown", HasSiblings = true, FirstName = "Jane", LastName = "Little" });
            people.Add(new Person { Age = 40, DOB = DateTime.Parse("30/6/1938"), EyeColour = "blue", HasSiblings = false, FirstName = "Ryan", LastName = "Brown" });
            people.Add(new Person { Age = 40, DOB = DateTime.Parse("3/6/1983"), EyeColour = "brown", HasSiblings = false, FirstName = "Peter", LastName = "Happer" });
            people.Add(new Person { Age = 40, DOB = DateTime.Parse("30/10/1967"), EyeColour = "blue", HasSiblings = true, FirstName = "Andrew", LastName = "Tomic" });
            people.Add(new Person { Age = 30, DOB = DateTime.Parse("30/10/1969"), EyeColour = "green", HasSiblings = true, FirstName = "Tim", LastName = "Smith", Nicknames = new string[]{"Smithy", "Timbo"}});


            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));
            TextWriter textWriter = new StreamWriter(@"C:\temp\people.xml");
            serializer.Serialize(textWriter, people);
            textWriter.Close();
        }

        public static void SerializeGroupToXML()
        {
            Group group = new Group();
            Employee[] employees = new Employee[] { new Employee {Name = "John Smith"}, new Employee {Name="Jane Ridington"}, new Manager {Level=3, Name="David Patterson"}};
            group.Employees = employees;
   
            XmlSerializer serializer = new XmlSerializer(typeof(Group));
            TextWriter textWriter = new StreamWriter(@"C:\temp\Group.xml");
            serializer.Serialize(textWriter, group);
            textWriter.Close();

        }

        #endregion

        /// <summary>
        /// Used to serialization formatting...
        ///// </summary>
        //public class Formatter : IFormatter
        //{
        //    SerializationBinder Binder { get; set; }
        //    StreamingContext Context { get; set; }
        //    ISurrogateSelector SurrogateSelector { get; set; }

        //    public object Deserialize(Stream s)
        //    {
        //        return null;
        //    }

        //    public void Serialize(Stream s, object o)
        //    {
        //        // do nothing
        //    }

        //}
    }
}
