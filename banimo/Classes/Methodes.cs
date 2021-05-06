using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace banimo.Classes
{
    public class Methodes
    {
        public static string ConvertNumToEn(string srt)
        {
            string rt = srt.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
            return rt;
        }
        public static long GetTimestamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}