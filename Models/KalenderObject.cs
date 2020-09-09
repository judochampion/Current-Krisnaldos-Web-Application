using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication4.Models
{
    public class KalenderObject
    {
        public List<CalendarEvent> KalenderEvents { get; }

        public List<CalendarEvent> Past_Events
        {
            get
            {
                var lovNow = DateTime.Now;
                return KalenderEvents.Where(lovEvent => DateTime.Compare(lovEvent.Tijdstip, lovNow)< 0).ToList();
            }
        }

        public List<CalendarEvent> Future_Events
        {
            get
            {
                var lovNow = DateTime.Now;
                return KalenderEvents.Where(lovEvent => DateTime.Compare(lovNow, lovEvent.Tijdstip) < 0).ToList();
            }
        }

        public int NumberOfKalenderEvents => KalenderEvents.Count;

        public KalenderObject(List<CalendarEvent> povCalendarEvents)
        {
            povCalendarEvents.Sort();
            KalenderEvents = povCalendarEvents;
        }
    }
}