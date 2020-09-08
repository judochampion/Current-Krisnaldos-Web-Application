using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public enum MatchSide { Thuis, Uit }

    public enum MatchType { Friendly, Competitie, Beker }

    public class CalendarEvent
    {
        public DateTime Tijdstip { get; }

        public MatchSide MatchSide { get; }
        public MatchType MatchType { get; }

        public string Tegenstander { get; }

        public CalendarEvent(string[] lovStringArray)
        {
            Tijdstip = new DateTime(
                year: Convert.ToInt32(lovStringArray[2]),
                month: Convert.ToInt32(lovStringArray[1]),
                day: Convert.ToInt32(lovStringArray[0]),
                hour: Convert.ToInt32(lovStringArray[3]),
                minute: Convert.ToInt32(lovStringArray[4]),
                second: 0);
            MatchType = (MatchType)Enum.Parse(typeof(MatchType), lovStringArray[5], true);
            MatchSide = (MatchSide)Enum.Parse(typeof(MatchSide), lovStringArray[6], true);
            Tegenstander = lovStringArray[7];
        }
    }
}