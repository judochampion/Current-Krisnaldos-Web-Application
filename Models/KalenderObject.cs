using System.Collections.Generic;

namespace WebApplication4.Models
{
    public class KalenderObject
    {
        public List<CalendarEvent> KalenderEvents { get; }

        public int NumberOfKalenderEvents => KalenderEvents.Count;

        public KalenderObject(List<CalendarEvent> povCalendarEvents)
        {
            KalenderEvents = povCalendarEvents;
        }
    }
}