using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

namespace AppDevTest
{
    public class WMIClass
    {
        // If searching locally do not need to create a ManagementScope or ObjectQuery object
        public static void ListNotStartedServices()
        {
            ManagementObjectSearcher DemoSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service WHERE Started = FALSE");
            ManagementObjectCollection AllObjects = DemoSearcher.Get();

            foreach (ManagementObject PausedService in AllObjects)
            {
                Console.WriteLine("Service = " + PausedService["Caption"]);
            }
        }

        public static void ListDisksConnectedToComputer()
        {
            ManagementScope scope = new ManagementScope(@"\\localhost\root\cimv2");
            scope.Connect();

            // Create the query
            ObjectQuery query = new ObjectQuery("Select * from Win32_LogicalDisk");

            // Perform the query
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection collection = searcher.Get();

            // Display the data from the query
            foreach(ManagementObject m in collection)
            {
                Console.WriteLine(string.Format("{0} {1}", m["Name"].ToString(), m["Description"]));
            }
        }

        public static void ListOSDetails()
        {
            ManagementScope scope = new ManagementScope(@"\\localhost\root\cimv2");
            scope.Connect();

            // Create the query
            ObjectQuery query = new ObjectQuery("Select * from Win32_OperatingSystem");

            // Perform the query
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection collection = searcher.Get();

            // Display the data from the query
            foreach (ManagementObject m in collection)
            {
                Console.WriteLine(string.Format("Computer Name: {0}", m["csname"].ToString()));
                Console.WriteLine(string.Format("Windows Directory: {0}", m["WindowsDirectory"].ToString()));
                Console.WriteLine(string.Format("Operating System: {0}", m["Caption"].ToString()));
                Console.WriteLine(string.Format("Version: {0}", m["Version"].ToString()));
                Console.WriteLine(string.Format("Manufacturer: {0}", m["Manufacturer"].ToString()));
            }
        }

        public static void ReadFreeSpaceOnNetworkDrives()
        {

            //query the win32_logicaldisk for type 4 (Network drive)
            SelectQuery query = new SelectQuery("select name, FreeSpace from win32_logicaldisk where drivetype=4");
            //execute the query using WMI
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            //loop through each drive found

            ManagementObjectCollection items = searcher.Get();

            foreach (ManagementObject drive in items)
            {
                UInt64 freesSpaceInBytes = (UInt64)drive["FreeSpace"];
                UInt64 freeSpaceInGigaBytes = freesSpaceInBytes / 1024 / 1024 /1024;
                Console.WriteLine("Drive: {0}   Space: {1}GB", drive["name"], freeSpaceInGigaBytes.ToString());
            }
        }

        public static void ShowNetorkDrives()
        {
            ManagementScope scope = new ManagementScope(@"\\localhost\root\cimv2");
            scope.Connect();

            // Create the query
            ObjectQuery query = new ObjectQuery("Select * from Win32_NetworkAdapter");

            // Perform the query
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection collection = searcher.Get();

            // Display the data from the query
            foreach (ManagementObject m in collection)
            {
                Console.WriteLine(m["Name"]);
                foreach (PropertyData i in m.Properties)
                {
                    Console.WriteLine(string.Format("    {0} : {1}", i.Name, i.Value ?? string.Empty));
                }

                //Console.WriteLine(string.Format("Name: {0}", m["Name"].ToString()));
                //Console.WriteLine(string.Format("Adapter Type: {0}", m["AdapterType"] ?? string.Empty));
                //Console.WriteLine(string.Format("InstallDate: {0}", m["InstallDate"] != null ? m["InstallDate"].ToString() : string.Empty));
                //Console.WriteLine(string.Format("MacAddress: {0}", m["MACAddress"].ToString()));
                //Console.WriteLine(string.Format("Physical Adapter: {0}", m["PhysicalAdapter"].ToString()));
            }

        }

        public static void EventWatcher()
        {
            // Create event query to be notified within 1 second of 
            // a change in a service
            WqlEventQuery query =
                new WqlEventQuery("__InstanceCreationEvent",
                new TimeSpan(0, 0, 1),
                "TargetInstance isa \"Win32_Process\"");

            // Initialize an event watcher and subscribe to events 
            // that match this query
            ManagementEventWatcher watcher =
                new ManagementEventWatcher();
            watcher.Query = query;
            // times out watcher.WaitForNextEvent in 5 seconds
            //watcher.Options.Timeout = new TimeSpan(0, 0, 10);

            // Block until the next event occurs 
            // Note: this can be done in a loop if waiting for 
            //        more than one occurrence
            Console.WriteLine(
                "Open an application (notepad.exe) to trigger an event.");
            //ManagementBaseObject e = watcher.WaitForNextEvent();
            watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            watcher.Start();

            //EventLog DemoLog = new EventLog("");
            //DemoLog.Source = "WMIClassDemo";
            //String EventName = ((ManagementBaseObject)e["TargetInstance"])["Name"].ToString();
            //Console.WriteLine(EventName);
            //DemoLog.WriteEntry(EventName);

            //Display information from the event
            //Console.WriteLine(
            //    "Process {0} has been created, path is: {1}",
            //    ((ManagementBaseObject)e
            //    ["TargetInstance"])["Name"],
            //    ((ManagementBaseObject)e
            //    ["TargetInstance"])["ExecutablePath"]);

            //Cancel the subscription
            watcher.Stop();
        }

        protected static void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            EventLog DemoLog = new EventLog("");
            DemoLog.Source = "WMIClassDemo";
            String EventName = "using watcher.Start: " + e.NewEvent.GetPropertyValue("TargetInstance").ToString();
            Console.WriteLine(EventName);
            DemoLog.WriteEntry(EventName);
        }
    }
}
