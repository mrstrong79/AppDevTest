using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace AppDevTest.DomainObjects
{
    public delegate bool  PersonDelegate(Person p);

    [Serializable]
    public class Person : ICloneable, IComparable, IDeserializationCallback
    {
        public event PersonDelegate PersonCloned;
        public string Name { get; set; } // this is here only because some other classes were using it like this...
        [NonSerialized] public string fullName; // this field is not serialized
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(fullName))
                    return fullName;
                return GetFullName(FirstName, LastName);
            }
        }
        public string FirstName { get; set; }
        [XmlElement("Surname")] public string LastName { get; set; } // in the XML this will be called 'Surname'
        [XmlAttribute("AGE")] public int Age { get; set; } // with the [XmlAttribute(name)] applied, this will get added as an attribute to the Person element, and will be called 'AGE'
        [XmlAttribute] public DateTime DOB { get; set; } // with the [XmlAttribute] applied, this will get added as an attribute to the Person element
        public string EyeColour { get; set; }
        public bool HasSiblings { get; set; }

        public Person()
        {    
        }

        public Person(string fn, string ln)
        {
            FirstName = fn;
            LastName = ln;
            fullName = GetFullName(fn, ln);
        }

        private string GetFullName(string f, string l)
        {
            return string.Format("{0} {1}", f, l);
        }

        // implemented as part of ICloneable
        public object Clone()
        {
            Person clonePerson = (Person)MemberwiseClone();

            // Unomtest to see what happens if the last name is changed...
            // clonePerson.LastName = "Brown";

            if (PersonCloned != null) // check to see if our event is null...
            {
                PersonCloned(clonePerson); // this is all you have to do to fire the event, and run any handlers attached to the event

                // The following would also work if you needed to run each event handler individually and check return values for instance.
                //bool cloned = false;
                //foreach (PersonDelegate del in PersonCloned.GetInvocationList())
                //{
                //    cloned = del.Invoke(clonePerson);
                //    if (!cloned) break; // if cloned == false, skip out
                //}
            }

            return clonePerson;
        }



        // IComparable - this allows it to be used in a SortedList, and also ArrayList.Sort, or other <collection>.Sort methods...
        // Return values:
        //  Less than zero - This instance is less than obj.
        //  Zero - This instance is equal to obj.
        //  Greater than zero - This instance is greater than obj.
        public int CompareTo(object o)
        {
            if (o is Person) // check to ensure the object is as we expect
            {
                Person other = (Person) o;
                if (LastName != other.LastName)
                {
                    return LastName.CompareTo(other.LastName);  // If we have different surnames, sort via this first.
                }
                return FirstName.CompareTo(other.FirstName); // if our surnames are the same, then compare the firstnames
            }
            throw new ArgumentException("object is not a Person"); 
        }

        [XmlArrayItem("Nickname")]
        public string[] Nicknames { get; set; }

  // Exmample output:
  //   <Person AGE="30" DOB="1969-10-30T00:00:00">
  //  <FirstName>Tim</FirstName>
  //  <Surname>Smith</Surname>
  //  <EyeColour>green</EyeColour>
  //  <HasSiblings>true</HasSiblings>
  //  <Nicknames>
  //    <Nickname>Smithy</Nickname>
  //    <Nickname>Timbo</Nickname>
  //  </Nicknames>
  //</Person>


        // After deserialization, set the fullName field
        void IDeserializationCallback.OnDeserialization(Object sender)
        {
            fullName = GetFullName(FirstName, LastName);
        }
    }
}
