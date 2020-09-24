using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebApplication4.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime Now_In_European_Time_Zone()
        {
            DateTime lovServerTime = DateTime.Now;

            DateTime lovEuropeanTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(lovServerTime, TimeZoneInfo.Local.Id, "W. Europe Standard Time");

            return lovEuropeanTime;
        }

        public static string To_File_Name_Without_Extension(this DateTime lovDT)
        {
            return $"{lovDT.Year}_{lovDT.Month.ToString("00")}_{lovDT.Day.ToString("00")}";
        }

        public static string To_WedstrijdBlad_DateString(this DateTime lovDT)
        {
            return $"{lovDT.Day.ToString("00")}/{lovDT.Month.ToString("00")}/{lovDT.Year}";
        }
    }

    public static class Random_Methods
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}