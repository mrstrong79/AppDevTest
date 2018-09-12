using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Text;

namespace AppDevTest
{
    public class AppDomainClass
    {
        public static void ShowCurrentAppDomain()
        {
            Console.WriteLine("Host domain: " + AppDomain.CurrentDomain.FriendlyName);
        }

        public static void CreateNewApplicationDomain()
        {
            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase = "file://" + System.Environment.CurrentDirectory;
            ads.DisallowBindingRedirects = false;
            ads.DisallowCodeDownload = true;
            ads.DisallowPublisherPolicy = true;
            ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            AppDomain d = AppDomain.CreateDomain("New Domain", null, ads); // Use the static AppDomain.CreateDomain method to create a new AppDomain

            Console.WriteLine("New domain: " + d.FriendlyName);
            Console.WriteLine("Config File for the AppDomainSetup object: " + ads.ConfigurationFile);

            AppDomain.Unload(d);
        }

        /// <summary>
        /// Create an app domain with the SecurityZone.Internet security restrictions...
        /// </summary>
        public static void CreateNewAppDomainRestricted()
        {
            object[] z = { new Zone(SecurityZone.Internet) };
            Evidence e = new Evidence(z, null);
            AppDomain d = AppDomain.CreateDomain("MyDomain", e);

            Console.WriteLine("New domain: " + d.FriendlyName);

            AppDomain.Unload(d);
        }


    }
}
