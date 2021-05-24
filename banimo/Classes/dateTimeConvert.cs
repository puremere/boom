using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace banimo.Classes
{
    public static class dateTimeConvert
    {

        public static double ConvertDateTimeToTimestamp(DateTime value)
        {
            TimeSpan epoch = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            //return the total seconds (which is a UNIX timestamp)
            return (double)epoch.TotalSeconds;
        }
        /// <summary>
        /// یک استرینگ تاریخ شمسی را به معادل میلادی تبدیل میکند
        /// </summary>
        /// <param name="persianDate">تاریخ شمسی</param>
        /// <returns>تاریخ میلادی</returns>
        public static DateTime ToGeorgianDateTime(this string persianDate)
        {

            List<string> list = persianDate.Split('/').ToList();
            int year = 0;
            int month = 01;
            int day = 01;
            
            if (list.Count() >0)
            {
               year = Convert.ToInt32(list[0]);
            }
            if ((list.Count() > 1))
            {
                month = Convert.ToInt32(list[1]);
            }
            if (list.Count() > 2)
            {
                day = Convert.ToInt32(list[2]);
            }
            DateTime georgianDateTime = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
            return georgianDateTime;
        }

        

        /// <summary>
        /// یک تاریخ میلادی را به معادل فارسی آن تبدیل میکند
        /// </summary>
        /// <param name="georgianDate">تاریخ میلادی</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ToPersianDateString(this DateTime georgianDate)
        {
            System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();

            string year = persianCalendar.GetYear(georgianDate).ToString();
            string month = persianCalendar.GetMonth(georgianDate).ToString().PadLeft(2, '0');
            string day = persianCalendar.GetDayOfMonth(georgianDate).ToString().PadLeft(2, '0');
            string persianDateString = string.Format("{0}/{1}/{2}", year, month, day);
            return persianDateString;
        }

        /// <summary>
        /// یک تعداد روز را از یک تاریخ شمسی کم میکند یا به آن آضافه میکند
        /// </summary>
        /// <param name="georgianDate">تاریخ شمسی اول</param>
        /// <param name="days">تعداد روزی که میخواهیم اضافه یا کم کنیم</param>
        /// <returns>تاریخ شمسی به اضافه تعداد روز</returns>
        public static string AddDaysToShamsiDate(this string persianDate, int days)
        {
            DateTime dt = persianDate.ToGeorgianDateTime();
            dt = dt.AddDays(days);
            return dt.ToPersianDateString();
        }
    }
    
}
