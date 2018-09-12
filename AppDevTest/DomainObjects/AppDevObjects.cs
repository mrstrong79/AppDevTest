using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AppDevTest.DomainObjects
{
    /// <summary>
    /// Holds various miscelaneous classes used for testing various .Net framework functionality
    /// </summary>

    // Classes from: http://msdn.microsoft.com/en-us/library/2baksw0z(v=vs.80).aspx
    public class Group
    {
        [XmlArray("TeamMembers")]
        [XmlArrayItem("MemberName"), XmlArrayItem(Type = typeof(Employee)), XmlArrayItem(Type = typeof(Manager))] // NOTE: If the 'type' option is specified, it overrides whatever is specified for the name
        public Employee[] Employees;
    }

    public class Employee
    {
        public string Name;
    }

    public class Manager : Employee
    {
        public int Level;
    }
}
