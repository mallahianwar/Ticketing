using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.elegalhubs.com.Helpers
{
    public class GeneralFunc
    {
        public static string GetInterval(DateTime Start ,DateTime End)
        {
            string result = "";
            TimeSpan span = (End - Start);
            if (span.Days > 0)
            {
               result =  span.Days + " day";
            }
            else if (span.Hours > 0)
            {
                result = span.Hours + " hour";
            }
            else if (span.Minutes > 0)
            {
                result = span.Minutes + " mins";
            }
            else
            {
                result = span.Seconds + " sec";
            }
            return result;
        }
    }
}
