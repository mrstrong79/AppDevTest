using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace AppDevTest
{
    public class AppDevConfiguration
    {
        public static void WriteToAppConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add("MyKey", "MyValue");
            config.Save(ConfigurationSaveMode.Modified);
        }

        public static void GetAllAppSettings()
        {         
            for (int i=0; i < ConfigurationManager.AppSettings.Count; i++)
            {
                Console.WriteLine("{0} : {1}", ConfigurationManager.AppSettings.AllKeys[i], ConfigurationManager.AppSettings[i]);
            }
        }

        // Two ways to get app settings
        public static void GetAppSetting(string key)
        {
            Console.WriteLine(ConfigurationManager.AppSettings[key]);
            Console.WriteLine(ConfigurationManager.AppSettings.Get(key));
        }

        // Return a specific connection string using the ConfigurationManager class
        public static void GetConnectionString(string key)
        {
            Console.WriteLine(ConfigurationManager.ConnectionStrings[key].ConnectionString);
        }

        public static void GetAllConnectionStrings()
        {
            foreach(ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
            {
                Console.WriteLine(string.Format("Name: {0}, ConnectionString: {1}, Provider: {2}, Source: {3}", cs.Name, cs.ConnectionString, cs.ProviderName, cs.ElementInformation.Source));
            }
        }

        public static void GetAllConnectionStrings2()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringSettingsCollection csc = config.ConnectionStrings.ConnectionStrings;
            foreach (ConnectionStringSettings cs in csc)
            {
                Console.WriteLine(string.Format("Name: {0}, ConnectionString: {1}, Provider: {2}, Source: {3}", cs.Name, cs.ConnectionString, cs.ProviderName, cs.ElementInformation.Source));
            }
        }

        public static void WriteNewConnectionString()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("myNewConnectionString", "<the actual connection string>"));
            config.Save(ConfigurationSaveMode.Modified);
        }

        public static void ReadMachineConfig()
        {
            Configuration config = ConfigurationManager.OpenMachineConfiguration();
        }

        // Custom Configuration
        public static void ReadCustomSetting()
        {
            MyCustomConfigClass customConfig = (MyCustomConfigClass)ConfigurationManager.GetSection("CustomSettings");
            Console.WriteLine(customConfig.FirstName);
            Console.WriteLine(customConfig.LastName);
        }

        // Create a custom section.
        public static void CreateSection()
        {
            try
            {
                CustomSection customSection;

                // Get the current configuration file.
                Configuration config =
                        ConfigurationManager.OpenExeConfiguration(
                        ConfigurationUserLevel.None);

                // Create the section entry  
                // in <configSections> and the 
                // related target section in <configuration>.
                if (config.Sections["CustomSection"] == null)
                {
                    customSection = new CustomSection();
                    config.Sections.Add("CustomSection", customSection);
                    customSection.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Full);

                    Console.WriteLine("Section name: {0} created",
                        customSection.SectionInformation.Name);

                }
            }
            catch (ConfigurationErrorsException err)
            {
                Console.WriteLine(err.ToString());
            }
        }

    }

    /// <summary>
    /// Custom Configuration Class - inherit from 'ConfigurationSection'
    /// </summary>
    public class MyCustomConfigClass : ConfigurationSection
    {
        [ConfigurationProperty("firstName")]
        public string FirstName { get; set; }

        [ConfigurationProperty("lastName")]
        public string LastName { get; set; }
    }

    // Define a custom section.
    public sealed class CustomSection : ConfigurationSection
    {
        public enum Permissions
        {
            FullControl = 0,
            Modify = 1,
            ReadExecute = 2,
            Read = 3,
            Write = 4,
            SpecialPermissions = 5
        }

        [ConfigurationProperty("fileName",
            DefaultValue = "default.txt")]
        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\",
                   MinLength = 1, MaxLength = 60)]
        public String FileName
        {
            get
            {
                return (String)this["fileName"];
            }
            set
            {
                this["fileName"] = value;
            }
        }

        [ConfigurationProperty("maxIdleTime", DefaultValue = "1:30:30")]
        public TimeSpan MaxIdleTime
        {
            get
            {
                return (TimeSpan)this["maxIdleTime"];
            }
            set
            {
                this["maxIdleTime"] = value;
            }
        }


        [ConfigurationProperty("permission",
            DefaultValue = Permissions.Read)]
        public Permissions Permission
        {
            get
            {
                return (Permissions)this["permission"];
            }

            set
            {
                this["permission"] = value;
            }
        }
    }

    // From MSDN example for IConfigurationSectionHandler - http://msdn.microsoft.com/en-us/library/ms228056.aspx
    public class AnotherCustomConfiguration : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            // Creates the configuration object that this method will return.
            // This can be a custom configuration class.
            // In this example, we use a System.Collections.Hashtable.
            Hashtable myConfigObject = new Hashtable();

            // Gets any attributes for this section element.
            Hashtable myAttribs = new Hashtable();
            foreach (XmlAttribute attrib in section.Attributes)
            {
                if (XmlNodeType.Attribute == attrib.NodeType)
                    myAttribs.Add(attrib.Name, attrib.Value);
            }

            // Puts the section name and attributes as the first config object item.
            myConfigObject.Add(section.Name, myAttribs);

            // Gets the child element names and attributes.
            foreach (XmlNode child in section.ChildNodes)
            {
                if (XmlNodeType.Element == child.NodeType)
                {
                    Hashtable myChildAttribs = new Hashtable();

                    foreach (XmlAttribute childAttrib in child.Attributes)
                    {
                        if (XmlNodeType.Attribute == childAttrib.NodeType)
                            myChildAttribs.Add(childAttrib.Name, childAttrib.Value);
                    }
                    myConfigObject.Add(child.Name, myChildAttribs);
                }
            }

            return (myConfigObject);
        }
    }


}
