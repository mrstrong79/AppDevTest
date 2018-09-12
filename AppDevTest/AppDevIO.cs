using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace AppDevTest
{
    public class AppDevIO
    {
        private const string pathToFile = @"c:\temp\test.txt";
       // private const string pathToLogFile = @"c:\temp\TestWindowsUpdate.log";

        # region Drives, Directories, & Files

        public static void GetFileInfo(string strFile)
        {
            FileInfo ourFile = new FileInfo(strFile);

            if (ourFile.Exists)
            {
                //Console.WriteLine("Filename : {0}", ourFile.Name);
                //Console.WriteLine("Path : {0}", ourFile.FullName);
                Console.WriteLine("Filename : {0}, Size: {1}b", Path.GetFileNameWithoutExtension(strFile), ourFile.Length);
            }
        }

        public static void GetAllDrives()
        {
            foreach(DriveInfo di in DriveInfo.GetDrives())
            {
                Console.WriteLine(String.Format("Drive Name: {0}  Drive Type: {1} {2}", di.Name, di.DriveType, di.VolumeLabel));
            }
        }

        public static void ShowDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine("Drive: {0}", drive.Name);
                Console.WriteLine("Type: {0}", drive.DriveType);
            }
        }

        public static void ShowDriveMsdnEx()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  File type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
        }

        public static void GetAllSubDirectories()
        {
            string path = @"c:\";

            if (Directory.Exists(path))
            {
                IEnumerable<string> dirs = Directory.EnumerateDirectories(@"c:\");

                foreach (var dir in dirs)
                {
                    Utilities.Writeln(dir);
                }
            }
        }

        public static void ShowTotalFilesInTopDirectories()
        {
            string path = @"C:\Program Files";
            foreach(string dir in Directory.GetDirectories(path))
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                Console.WriteLine("Directory: {0}    Number of files: {1}", dir, di.GetFiles().Length);
            }
        }

        public static void CreateDirectory()
        {
            string path = @"c:\test";
            DirectoryInfo d = Directory.CreateDirectory(path);
            Console.WriteLine(d.CreationTime.ToString());
        }

        public static void CreateRemoveDirectory()
        {
            // Specify the directories you want to manipulate.
            DirectoryInfo di = new DirectoryInfo(@"c:\MyDir");
            try
            {
                // Determine whether the directory exists.
                if (di.Exists)
                {
                    // Indicate that the directory already exists.
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                di.Create();
                Console.WriteLine("The directory was created successfully.");

                // Delete the directory.
                di.Delete();
                Console.WriteLine("The directory was deleted successfully.");

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }


        #endregion

        #region Read & Write Files

        public static void ReadFileSR()
        {
            StreamReader sr = new StreamReader(pathToFile);
            string output;
            while ((output = sr.ReadLine()) != null)
            {
                Console.WriteLine(output);
            }
            sr.Close();
        }

        public static void ReadFileTR()
        {
            TextReader tr = File.OpenText(pathToFile);
            Console.WriteLine(tr.ReadToEnd());
            tr.Close();
        }

        public static void ReadFileFromFileStream()
        {
            FileStream fs = new FileStream(pathToFile, FileMode.Open);
            byte[] buffer = new byte[1023];
            fs.Read(buffer, 0, buffer.Length); // reads a block of bytes from the stream into the buffer
            fs.Close();
            for (int i=0; i<buffer.Length;i++)
            {
                Console.WriteLine("{0} : {1}", i, buffer[i]);
            }
        }

        public static void WriteFileSR()
        {
            if (File.Exists(pathToFile))
            {
                Console.WriteLine("File already exists!!");
            }
            else
            {
                StreamWriter sw = new StreamWriter(pathToFile);
                sw.WriteLine("This is a test");
                sw.Close();
            }         
        }

        public static void WriteFileTR()
        {
            if (File.Exists(pathToFile))
            {
                Console.WriteLine("File already exists!!");
            }
            else
            {
                TextWriter tw = File.CreateText(pathToFile);
                tw.WriteLine("This is a test");
                tw.Close();
            }
        }

        public static void WriteBinaryData()
        {
            FileStream fs = new FileStream(@"c:\temp\testBinaryData.bin", FileMode.Create); // Overwrites if already exists...
            BinaryWriter bw = new BinaryWriter(fs);
            for (int i=0;i<11;i++)
            {
                bw.Write(i);
            }
            bw.Close();
            fs.Close();
        }

        public static void ReadBinaryData()
        {
            FileStream fs = new FileStream(@"c:\temp\testBinaryData.bin", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            while (br.PeekChar() != -1)
            {
                Console.WriteLine(br.ReadInt32());
            }
            br.Close();
            fs.Close();
        }

        public static void MemoryStream()
        {
            System.IO.MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);

            sw.WriteLine("Hello this is a test");
            sw.Flush();

            ms.WriteTo(File.Create(pathToFile));
            sw.Close();
            ms.Close();
        }


        /// <summary>
        /// This method uses file streams to compress data
        /// </summary>
        /// <param name="inFile"></param>
        /// <param name="outFile"></param>
        public static void CompressFile(string inFile, string outFile)
        {
            if (File.Exists(inFile))
            {
                using (FileStream sourceFile = File.OpenRead(inFile))
                { 
                    using (FileStream destFile = File.OpenWrite(outFile))
                    {
                        using (GZipStream compStream = new GZipStream(destFile, CompressionMode.Compress))
                        {
                            // this example uses the .WriteByte() method of the GZipStream class to write individual bytes to the underlying stream (destfile). 
                            // It would be more efficient to use the .Write() method -- see the example below

                            int theByte = sourceFile.ReadByte();
                            int count = 0;

                            while (theByte != -1)
                            {
                                compStream.WriteByte((byte) theByte);
                                theByte = sourceFile.ReadByte();
                                count++;
                            }
                            Console.WriteLine("Compressed {0} bytes", count.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method uses a Memory Stream and compresses a byte array
        /// </summary>
        public static void CompressFile()
        {
            byte[] bytesToCompress = GetFileAsByeArray(pathToFile);
            Console.WriteLine(Encoding.Unicode.GetString(bytesToCompress));
            System.IO.MemoryStream ms = new MemoryStream(); // Create a MemoryStream object that our compressed bytes will get written to...
            GZipStream gz = new GZipStream(ms, CompressionMode.Compress); // Create a GZipStream object, passing in our memory stream and setting the CompressionMode
            gz.Write(bytesToCompress, 0, bytesToCompress.Length); // Write our compressed bytes to our MemoryStream object
            byte[] compressedBytes = ms.ToArray(); // Calling the .ToArray() method on our MemoryStream object returns a byte[]
            Console.WriteLine(Encoding.Unicode.GetString(compressedBytes));
            string compressedFile = pathToFile + ".zip";
            ms.WriteTo(File.Create(compressedFile)); // Use the WriteTo() method of our MemoryStream object to write to a FileStream object
            gz.Close();
            ms.Close();
        }

        public static byte[] GetFileAsByeArray(string path)
        {
            byte[] bytes;
            string fileText = string.Empty;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    fileText = sr.ReadToEnd();
                }
            }
            return Encoding.Unicode.GetBytes(fileText);
        }


        #endregion

        public static void EnumerateAllLogicalDrives()
        {
            //ConnectionOptions DemoOptions = new ConnectionOptions();
            //DemoOptions.Username = "domainname\\username";a
            //DemoOptions.Password = "password";
            //ManagementScope DemoScope = new ManagementScope("\\\\machinename\\root\\cimv2", DemoOptions);
            ObjectQuery DemoQuery = new ObjectQuery("SELECT * FROM Win32_LogicalDisk where DriveType=3");
            //ManagementObjectSearcher DemoSearcher = new ManagementObjectSearcher(DemoQuery,DemoScope);
            ManagementObjectSearcher DemoSearcher = new ManagementObjectSearcher(DemoQuery);
            ManagementObjectCollection AllObjects = DemoSearcher.Get();
            foreach (ManagementObject DemoObject in AllObjects)
            {
                Console.WriteLine("Resource Name: " + DemoObject["Name"].ToString()); Console.WriteLine("Resource Size: " + DemoObject["Size"].ToString());
            }


            // Alternative Method:
            //string[] drives = Directory.GetLogicalDrives();
            //for (int i = 0; i < drives.Length ; i++)
            //{
            //    Console.WriteLine(drives[i]);
            //}
        }

        public static void EnumerateAllNetworkAdapters()
        {
            string IP_Enabled = "IPEnabled"; 
            string IP_Address = "IPAddress"; 
            string IP_Subnet = "IPSubnet"; 
            string DNS_HostName = "DNSHostName"; 
            string DNS_Domain = "DNSDomain";

            ManagementObjectSearcher DemoQuery = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection DemoQueryCollection = DemoQuery.Get();
            foreach (ManagementObject DemoManager in DemoQueryCollection)
            {
                Console.WriteLine("Description : " + DemoManager["Description"]); Console.WriteLine("MacAddress : " + DemoManager["MacAddress"]);
                if (Convert.ToBoolean(DemoManager[IP_Enabled]) == true)
                {

                    String[] IPAddresses = DemoManager[IP_Address] as String[];
                    String[] IPSubnets = DemoManager[IP_Subnet] as String[];
                    Console.WriteLine(DNS_HostName + " : " + DemoManager[DNS_HostName]);
                    Console.WriteLine(DNS_Domain + " : " + DemoManager[DNS_Domain]);
                    foreach (String IPAddress in IPAddresses)
                    {
                        Console.WriteLine(IP_Address + " : " + IPAddress);
                    }
                    foreach (String IPSubnet in IPSubnets)
                    {
                        Console.WriteLine(IP_Subnet + " : " + IPSubnet);
                    }
                }
            }
        }

        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account,
                                                             Rights,
                                                             ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);

        }

        public static void RemoveDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings. 
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(Account,
                                                                Rights,
                                                                ControlType));

            // Set the new access settings.
            dInfo.SetAccessControl(dSecurity);

        }




        public static void useIsolatedStorage()
        {
            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForAssembly();

            IsolatedStorageFileStream userStream = new IsolatedStorageFileStream("UserSettings.set",
                                                                                 FileMode.Create, userStore);

            StreamWriter writer = new StreamWriter(userStream);

            writer.WriteLine("User Preferences");

            writer.Close();
            userStream.Close();
            userStore.Close();

        }

        public static void useIsolatedStorageWriteXml()
        {
            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForAssembly();

            IsolatedStorageFileStream userStream = new IsolatedStorageFileStream("UserSettings.xml",
                                                                                 FileMode.Create, userStore);

            XmlTextWriter writer = new XmlTextWriter(userStream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement("UserSettings");
            writer.WriteStartElement("BackColor");
            writer.WriteString("red");
            writer.WriteEndElement();
            writer.WriteStartElement("Width");
            writer.WriteString("180");
            writer.WriteEndElement();
            writer.WriteStartElement("Height");
            writer.WriteString("200");
            writer.WriteEndElement();
            writer.WriteStartElement("SampleData");
            writer.WriteString("5000M");
            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.Flush();
            writer.Close();
            userStream.Close();
            userStore.Close();

        }

        public static void SerializeData()
        {
            string data = "This must be stored in a file.";

            // Create file to save the data to 
            FileStream fs = new FileStream(@"c:\temp\SerializedString.Data", FileMode.Create);

            // Create a BinaryFormatter object to perform the serialization 
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //System.Runtime.Serialization.Formatters. bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //System.Runtime.Serialization.Formatters.Soap.SoapFormatter sf = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();

            // Use the BinaryFormatter object to serialize the data to the file 
            //sf.Serialize(fs, data);
            bf.Serialize(fs, DateTime.Now);

            // Close the file 
            fs.Close();
        }

        public static void XMLSerializeData()
        {
            string data = "This must be stored in a file.";

            // Create file to save the data to 
            FileStream fs = new FileStream(@"c:\temp\SerializedString.Data", FileMode.Create);

            // Create a XML Serializer object to perform the serialization 
            XmlSerializer xs = new XmlSerializer(typeof(DateTime));


            // Use the BinaryFormatter object to serialize the data to the file 
            //sf.Serialize(fs, data);
            xs.Serialize(fs, DateTime.Now);

            // Close the file 
            fs.Close();
        }
    }
}
