using System;
using System.Collections.Generic;
using System.Data;

namespace PodCatcher.API.Models
{
    public class DateParser
    {
        public static DateTime ParseDate(string date)
        {
            try
            {
                return Convert.ToDateTime(date);
            }
            catch (Exception exception)
            {
                return NormalizeDate(date);  
            
            }
        }

        private static DateTime NormalizeDate(string date)
        {
            try
            {
                Dictionary<string, string> timeZones = new Dictionary<string, string>();
                timeZones.Add("EST", "-05:00");
                timeZones.Add("CST", "-06:00");
                timeZones.Add("MST", "-07:00");
                timeZones.Add("PST", "-08:00");
                timeZones.Add("EDT", "-04:00");

                string inputDate = date;
                string modifiedInputDate = inputDate.Substring(0, inputDate.LastIndexOf(" "));
                string timeZoneIdentifier = inputDate.Substring(inputDate.LastIndexOf(" ") + 1);
                string timeZoneOffset = timeZones[timeZoneIdentifier];
                string dateForParsing = modifiedInputDate + " " + timeZoneOffset;

                return DateTime.ParseExact(dateForParsing, "ddd, dd MMM yyyy HH:mm:ss zzz", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                throw new DataException();
            }
        }
    }
}