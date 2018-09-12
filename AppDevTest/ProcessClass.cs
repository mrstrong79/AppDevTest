using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace AppDevTest
{
    public class ProcessClass
    {

        public static void ProcessorMonitor()
        {
            PerformanceCounter myCounter;

            if (!PerformanceCounterCategory.Exists("Processor"))
            {
                Console.WriteLine("Object Processor does not exist!");
                return;
            }

            if (!PerformanceCounterCategory.CounterExists(@"% Processor Time", "Processor"))
            {
                Console.WriteLine(@"Counter % Processor Time does not exist!");
                return;
            }

            myCounter = new PerformanceCounter("Processor", @"% Processor Time", @"_Total");

            // The raw value of a counter can be set in your applications as shown below
            // if the object is not read-only
            try
            {
                myCounter.RawValue = 19;
            }

            catch
            {
                Console.WriteLine(@"Processor, % Processor Time, _Total instance is READONLY!");
            }

            Console.WriteLine(@"Press 'CTRL+C' to quit...");

            while (true)
            {
                Console.WriteLine("@");

                try
                {
                    Console.WriteLine(@"Current value of Processor, %Processor Time, _Total= " + myCounter.NextValue().ToString());
                }

                catch
                {
                    Console.WriteLine(@"_Total instance does not exist!");
                    return;
                }

                Thread.Sleep(1000);
                Console.WriteLine(@"Press 'CTRL+C' to quit...");
            }
        }

        public static void GetProcessInfo()
        {
            // Get the current process.
            Process proc = Process.GetCurrentProcess();
            Console.WriteLine("Current Process:");
            Console.WriteLine(string.Format("{0} : {1}", proc.Id, proc.ProcessName));


            // Get all instances of Notepad running on the local
            // computer.
            Process[] localByName = Process.GetProcessesByName("notepad");
            Console.WriteLine("localByName:");
            foreach (Process p in localByName)
            {
                Console.WriteLine(string.Format("{0} : {1}", p.Id, p.ProcessName));
            }


            // Get all instances of Notepad running on the specifiec
            // computer.
            // 1. Using the computer alias (do not precede with "\\").
            try
            {

                Process[] remoteByName = Process.GetProcessesByName("notepad", "myComputer");
                Console.WriteLine("remoteByName:");
                foreach (Process p in remoteByName)
                {
                    Console.WriteLine(string.Format("{0} : {1}", p.Id, p.ProcessName));
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }


            // 2. Using an IP address to specify the machineName parameter. 
            try
            {
                Process[] ipByName = Process.GetProcessesByName("notepad", "169.0.0.0");
                Console.WriteLine("ipByName:");
                foreach (Process p in ipByName)
                {
                    Console.WriteLine(string.Format("{0} : {1}", p.Id, p.ProcessName));
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }


            // Get a process on the local computer, using the process id.
            try
            {
                Process localById = Process.GetProcessById(1234);
                Console.WriteLine("localById:");
                Console.WriteLine(string.Format("{0} : {1}", localById.Id, localById.ProcessName));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }


            // Get a process on a remote computer, using the process id.
            try
            {
                Process remoteById = Process.GetProcessById(2345, "myComputer");
                Console.WriteLine("remoteById:");

                Console.WriteLine(string.Format("{0} : {1}", remoteById.Id, remoteById.ProcessName));
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetProcessInfo2()
        {
            // Get the current process.
            Process currentProcess = Process.GetCurrentProcess();


            // Get all instances of Notepad running on the local
            // computer.
            Process[] localByName = Process.GetProcessesByName("notepad");


            // Get all instances of Notepad running on the specifiec
            // computer.
            // 1. Using the computer alias (do not precede with "\\").
            Process[] remoteByName = Process.GetProcessesByName("notepad", "myComputer");

            // 2. Using an IP address to specify the machineName parameter. 
            Process[] ipByName = Process.GetProcessesByName("notepad", "169.0.0.0");


            // Get all processes running on the local computer.
            Process[] localAll = Process.GetProcesses();


            // Get all processes running on the remote computer.
            Process[] remoteAll = Process.GetProcesses("myComputer");


            // Get a process on the local computer, using the process id.
            Process localById = Process.GetProcessById(1234);


            // Get a process on a remote computer, using the process id.
            Process remoteById = Process.GetProcessById(2345, "myComputer");

        }

        public static void GetCurrentProcess()
        {
            if (Process.GetCurrentProcess().BasePriority >= 8)
                Console.WriteLine(
                    "For best results, run this application at low priority.");

            // 4 = low
            // 8 = normal
            // 13 = high
            // 24 = realtime
        }

        public static void GetAllProcesses()
        {
            int count = 0;
            foreach(Process p in Process.GetProcesses())
            {
                Console.WriteLine(string.Format("{0} : {1}", p.Id, p.ProcessName));
                count++;
            }
            Console.WriteLine(string.Format("{0} number of processes", count));

        }

        /// <summary>
        /// Lists all current processes, aswell as any loaded modules, eg dll's or exe's that have been loaded into memory
        /// </summary>
        public static void GetAllProcessesAndModules()
        {
            int count = 0;
            foreach (Process p in Process.GetProcesses())
            {
                Console.WriteLine(string.Format("{0} : {1}", p.Id, p.ProcessName));

                try
                {
                    foreach(ProcessModule m in p.Modules)
                    {
                        Console.WriteLine(string.Format("        {0} : {1}", m.ModuleName, m.ModuleMemorySize.ToString()));    
                    }
                }
                catch (Exception ex)
                {     
                   Console.WriteLine(string.Format("Caught excpetion of type {0} with message: {1}", ex.GetType(), ex.Message));
                }

                count++;
            }
            Console.WriteLine(string.Format("{0} number of processes", count));

        }

        public static void StartProcess()
        {
            Process.Start("Notepad.exe", @"c:\temp\test.txt");
        }

        public static void StartProcess2()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("notepad.exe");
            p.StartInfo.FileName = @"c:\temp\test.txt";
            //p.StartInfo.Arguments = @"c:\temp\test.txt"; // this also works
            p.Start();
        }
    }
}
