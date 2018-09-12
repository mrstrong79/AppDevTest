using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AppDevTest
{
    public enum LogType { Information, Warning, Error};

    public class Logging
    {
        private const string SOURCE = "AppDevTest";

        public static void TestLog()
        {
            try
            {
                int b = 0;
                //throw new Exception("There appears to have been a mistake");
                int a = 7/b;
            }
            catch (Exception ex)
            {
                
               LogError(ex);
            }
        }

        public static void WriteToEventLog(EventLogEntryType type, string message)
        {
            // check to ensure that our source exists...
            CheckExists();
            ///EventLogEntry entry = new EventLogEntry();
            EventLog.WriteEntry(SOURCE, message, type, 222);
        }

        public static void LogError(Exception ex)
        {
            StringBuilder message = new StringBuilder();

            string targetSite = ex.TargetSite != null ? ex.TargetSite.ToString() : "null";
            string innerException = ex.InnerException != null ? ex.InnerException.ToString() : "null";

            message.AppendLine("Source: " + ex.Source);
            message.AppendLine("Message: " + ex.Message);
            message.AppendLine("TargetSite: " + targetSite);
            message.AppendLine("InnerException: " + innerException);
            message.AppendLine("Stacktrace: " + ex.StackTrace.ToString());

            LogError(message.ToString());
        }

        public static void LogError(string message)
        {
            WriteToEventLog(EventLogEntryType.Error, message);
        }

        public static void LogInformation(string message)
        {
            WriteToEventLog(EventLogEntryType.Information, message);
        }

        public static void LogWarning(string message)
        {
            WriteToEventLog(EventLogEntryType.Warning, message);
        }

        private static void CheckExists()
        {
            Debug.WriteLine(string.Format("Does '{0}' exist in the event log? {1}", SOURCE, EventLog.SourceExists(SOURCE).ToString()));

            if (!EventLog.SourceExists(SOURCE))
                EventLog.CreateEventSource(SOURCE, "Application");

            // The following would also do the same as the above but using the 'EventSourceCreationData' class
            //EventSourceCreationData escd = new EventSourceCreationData(SOURCE, "Application");
            //EventLog.CreateEventSource(escd);
            

        }

        public static void writeEventLog()
        {
            try
            {
                if (!EventLog.SourceExists("AppDevTestSecurity"))
                {
                    EventLog.CreateEventSource("AppDevTestSecurity", "Security"); // source, logname
                }

                EventLog myLog = new EventLog();
                myLog.Source = "AppDevTestSecurity";

                string msg = "This is a test from AppDevTest console app!!";
                myLog.WriteEntry(msg); // this would write to the 'Security' log...
            }
            catch (Exception e)
            {
                Console.WriteLine("There was a problem writing to the event log");
                Console.WriteLine(e.ToString());
            }
        }

        public static void readEventLog()
        {
            EventLog log = new EventLog("Application");
            //EventLog log = new EventLog("Application", "."); // "." indicates local machine - default if not specified

            int count = 0;

            foreach (EventLogEntry entry in log.Entries)
            {
                if (entry.EntryType == EventLogEntryType.Error)
                {
                    Console.WriteLine(count.ToString() + ": " + entry.Source);
                    count++;
                }
            }
            Console.WriteLine(count.ToString() + " log values read");
        }

        public static void ReadEventLogWarningsErrors()
        {
            EventLog log = new EventLog("Application", ".");
            foreach(EventLogEntry e in log.Entries)
            {
                if (e.Source == "MySource")
                {
                    if (e.EntryType == EventLogEntryType.Error || e.EntryType == EventLogEntryType.Warning)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }


    }
}
