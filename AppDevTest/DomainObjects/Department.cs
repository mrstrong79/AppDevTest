using System;
using System.Runtime.Serialization;

namespace AppDevTest.DomainObjects
{
    [Serializable]
    class Department : ISerializable
    {
        private string _code, _name, _fullName;
        public Department(string code, string name)
        {
            _code = code;
            _name = name;
            _fullName = code + name;
        }


        /// <summary>
        /// Used during deserialization
        /// </summary>
        /// <param name="info">The info. Stores all the data needed to serialize or deserialize an object.</param>
        /// <param name="cxt">The CXT. Describes the source and destination of a given serialized stream, and provides an additional caller-defined context</param>
        public Department(SerializationInfo info, StreamingContext cxt)
        {
            _code = info.GetString("Code"); // can also use info.GetValue("Code"), and cast this to a string
            _name = info.GetString("Name");
            _fullName = _code + _name;
        }

        /// <summary>
        /// Used during serialization
        /// </summary>
        /// <param name="info"> Stores all the data needed to serialize or deserialize an object</param>
        /// <param name="cxt">Describes the source and destination of a given serialized stream, and provides an additional caller-defined context</param>
        public void GetObjectData(SerializationInfo info, StreamingContext cxt)
        {
            info.AddValue("Code", _code);
            info.AddValue("Name", _name);
        }

        public string Code
        {
            get
            {
                return _code;
            }

        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string FullName
        {
            get { return _code + _name; }
        }
    }

}
