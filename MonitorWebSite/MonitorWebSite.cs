using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.IO;
using System.Net;
using AppDevTest;

namespace MonitorWebSite
{
    public partial class MonitorWebSite : ServiceBase
    {
        private Timer t = null;
        private int userCount;

        public MonitorWebSite()
        {
            InitializeComponent();
            t= new Timer(10000); // every 10 seconds
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
        }

        protected override void OnStart(string[] args)
        {
            t.Start();
        }

        protected override void OnStop()
        {
            t.Stop();
        }

        protected override void OnPause()
        {
            t.Stop();
        }

        protected override void OnContinue()
        {
            t.Start();
        }

        protected override void OnShutdown()
        {
            t.Stop();
        }

        protected void t_Elapsed(object sender, EventArgs e)
        {
            try
            {
                string url = "http://www.microsoft.com";
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Log the response to a text file
                string logFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log.txt";
                TextWriter writer = new StreamWriter(logFile, true);
                writer.WriteLine(DateTime.Now.ToString() + " for " + url + ": " + response.StatusCode.ToString());
                writer.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                Logging.LogError(ex);        
            }
        }

        // Handle a session change notice
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
#if LOGEVENTS
            EventLog.WriteEntry("SimpleService.OnSessionChange", DateTime.Now.ToLongTimeString() +
                " - Session change notice received: " +
                changeDescription.Reason.ToString() + "  Session ID: " + 
                changeDescription.SessionId.ToString());
#endif

            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    userCount += 1;
#if LOGEVENTS
                    EventLog.WriteEntry("SimpleService.OnSessionChange", 
                        DateTime.Now.ToLongTimeString() +
                        " SessionLogon, total users: " +
                        userCount.ToString());
#endif
                    break;

                case SessionChangeReason.SessionLogoff:

                    userCount -= 1;
#if LOGEVENTS
                    EventLog.WriteEntry("SimpleService.OnSessionChange", 
                        DateTime.Now.ToLongTimeString() +
                        " SessionLogoff, total users: " +
                        userCount.ToString());
#endif
                    break;
                case SessionChangeReason.RemoteConnect:
                    userCount += 1;
#if LOGEVENTS
                    EventLog.WriteEntry("SimpleService.OnSessionChange", 
                        DateTime.Now.ToLongTimeString() +
                        " RemoteConnect, total users: " +
                        userCount.ToString());
#endif
                    break;

                case SessionChangeReason.RemoteDisconnect:

                    userCount -= 1;
#if LOGEVENTS
                    EventLog.WriteEntry("SimpleService.OnSessionChange", 
                        DateTime.Now.ToLongTimeString() +
                        " RemoteDisconnect, total users: " +
                        userCount.ToString());
#endif
                    break;
                case SessionChangeReason.SessionLock:
#if LOGEVENTS
                    EventLog.WriteEntry("SimpleService.OnSessionChange", 
                        DateTime.Now.ToLongTimeString() + 
                        " SessionLock");
#endif
                    break;

                case SessionChangeReason.SessionUnlock:
#if LOGEVENTS
                    EventLog.WriteEntry("SimpleService.OnSessionChange", 
                        DateTime.Now.ToLongTimeString() + 
                        " SessionUnlock");
#endif
                    break;

                default:

                    break;
            }
        }


    }
}
