using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AppDevTest
{
    class AppDevGlobalization
    {
        public static void displayCurrencyValue()
        {
            int moneyValue = 10000;
            Console.WriteLine("This is the currency value: {0:C}", moneyValue);
            // Output:  This is the currency value: $10,000.00
        }

        public static void ShowCurrencyInEnglishPounds()
        {
            int val = 10000;
            //CultureInfo ci = CultureInfo.GetCultureInfo("en-GB"); // this works...
            CultureInfo ci = new CultureInfo("en-GB"); // this works too
            ShowCurrencyValue(ci, val);

            // Output: £10,000.00
        }

        public static void ShowCurrencyValue(CultureInfo ci, int val)
        {
            Console.WriteLine(val.ToString("C", ci));
        }

        public static void ShowCurrencyForDifferentCultures()
        {
            int i = 100;

            // Creates a CultureInfo for English in Belize.
            CultureInfo bz = new CultureInfo("en-BZ");
            // Displays i formatted as currency for the bz.
            Console.WriteLine(i.ToString("c", bz));

            // Creates a CultureInfo for English in the U.S.
            CultureInfo us = new CultureInfo("en-US");
            // Display i formatted as currency for us.
            Console.WriteLine(i.ToString("c", us));

            // Creates a CultureInfo for Danish in Denmark.
            CultureInfo dk = new CultureInfo("da-DK");
            // Displays i formatted as currency for dk.
            Console.WriteLine(i.ToString("c", dk));

            // Output
            /*
             * BZ$100.00
               $100.00
               kr. 100,00
             * 
             * 
             * 
             */
        }

        public static void ShowNegativeHongKongCurrency()
        {
            int c1, c2;
            NumberFormatInfo culture = new CultureInfo("zh-HK").NumberFormat;
            culture.CurrencyNegativePattern = 1;

            c1 = 33;
            c2 = -54;
            Console.WriteLine(c1.ToString("C", culture));
            Console.WriteLine(c2.ToString("C", culture));

            // Output:
            // HK$33.00
            // -HK$54.00

        }

        public static void ShowCurrentRegionName()
        {

            //int i = System.Globalization.RegionInfo
            string str = RegionInfo.CurrentRegion.Name;
            Console.WriteLine(str);

        }

        public static void ShowCutlureInformationForCanada()
        {

            CultureInfo ci = new CultureInfo("en-CA");
            Console.WriteLine(ci.DisplayName);
            
            // Output: English (Canada)
        }

        public static void ShowAllRegions()
        {
            //RegionInfo[] regions = RegionInfo.CurrentRegion;
        }

        public static void ShowRegionalInfoForUS()
        {

            // Displays the property values of the RegionInfo for "US".
            RegionInfo myRI1 = new RegionInfo("US");
            Console.WriteLine("   Name:                         {0}", myRI1.Name);
            Console.WriteLine("   DisplayName:                  {0}", myRI1.DisplayName);
            Console.WriteLine("   EnglishName:                  {0}", myRI1.EnglishName);
            Console.WriteLine("   IsMetric:                     {0}", myRI1.IsMetric);
            Console.WriteLine("   ThreeLetterISORegionName:     {0}", myRI1.ThreeLetterISORegionName);
            Console.WriteLine("   ThreeLetterWindowsRegionName: {0}", myRI1.ThreeLetterWindowsRegionName);
            Console.WriteLine("   TwoLetterISORegionName:       {0}", myRI1.TwoLetterISORegionName);
            Console.WriteLine("   CurrencySymbol:               {0}", myRI1.CurrencySymbol);
            Console.WriteLine("   ISOCurrencySymbol:            {0}", myRI1.ISOCurrencySymbol);
            Console.WriteLine();

            // Compares the RegionInfo above with another RegionInfo created using CultureInfo.
            RegionInfo myRI2 = new RegionInfo(new CultureInfo("en-US", false).LCID);
            if (myRI1.Equals(myRI2))
                Console.WriteLine("The two RegionInfo instances are equal.");
            else
                Console.WriteLine("The two RegionInfo instances are NOT equal.");

            /*
This code produces the following output.

   Name:                         US
   DisplayName:                  United States
   EnglishName:                  United States
   IsMetric:                     False
   ThreeLetterISORegionName:     USA
   ThreeLetterWindowsRegionName: USA
   TwoLetterISORegionName:       US
   CurrencySymbol:               $
   ISOCurrencySymbol:            USD

The two RegionInfo instances are equal.

*/

        }

        public static void ShowDaysSpanishNeutralCutlure()
        {
            CultureInfo culture = new CultureInfo("es");
            String[] days = culture.DateTimeFormat.DayNames;
            foreach (String day in days)
            {
                Console.WriteLine("Day Name for Spanish : " + day);
            }
        }

        public static void ShowDaysVenezuelanSpanishSpecificCutlure()
        {
            CultureInfo culture = new CultureInfo("es-VE");
            String[] days = culture.DateTimeFormat.DayNames;
            foreach (String day in days)
            {
                Console.WriteLine("Day Name for Venezuelan Spanish : " + day);
            }
        }

        public static void ShowNeutralCultures() // eg, en, es, fr
        {
            int count = 0;
            var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            foreach (var culture in cultures)
            {
                Console.WriteLine(culture.ToString());
                count++;
            }
            Console.WriteLine("There are {0} neutral cultures", count); // result 144
        }

        public static void ShowSpecificCultures() // eg en-US, en-AU, es-VE
        {
            int count = 0;
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (var culture in cultures)
            {
                Console.WriteLine(culture.ToString());
                count++;
            }
            Console.WriteLine("There are {0} specific cultures", count); // result 210
        }

        public static void ShowGermanDate()
        {
            DateTime dt = DateTime.Now;
            CultureInfo ci = new CultureInfo("de-DE");
            Console.WriteLine(dt.ToString("d", ci));

            //Output: 26.04.2012
        }

        public static void ShowDateInMexicanSpanishFormat()
        {
            DateTimeFormatInfo di = new CultureInfo("es-MX",false).DateTimeFormat;
            DateTime dateTime =
            new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            Console.WriteLine(dateTime.ToString(di));
            Console.WriteLine(dateTime.ToString(di.LongDatePattern));

            /* Output:
             * 
             * 08/07/2012 12:00:00 a.m.
             * Sunday, 08 de July de 2012
             * 
             */
        }

        public static void Compare()
        {
            string searchString = "This is the search string";
            string searchVal = "ing";
            CompareInfo comparer = new CultureInfo("it-IT").CompareInfo;
            Console.WriteLine(comparer.Compare(searchString, searchVal) > 0);
        }

    }
}
