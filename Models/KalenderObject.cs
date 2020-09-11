using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication4.Models
{
    public class KalenderObject
    {
        public string WebRootPath { get; set; }

        public string RawPathNameToFolderAfterWWWRoot { get; set; }

        public List<CalendarEvent> KalenderEvents { get; }

        public List<Ploeg> Ploegen { get; }

        public List<CalendarEvent> Past_Events
        {
            get
            {
                var lovNow = DateTime.Now;
                return KalenderEvents.Where(lovEvent => DateTime.Compare(lovEvent.Tijdstip, lovNow) < 0).ToList();
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

        public KalenderObject(List<CalendarEvent> povCalendarEvents, string povWebRootPath, string povRawPathName)
        {
            povCalendarEvents.Sort();
            KalenderEvents = povCalendarEvents;
            WebRootPath = povWebRootPath;
            RawPathNameToFolderAfterWWWRoot = povRawPathName;

            //movSponsors = new List<Sponsor>();

            string csvData = System.IO.File.ReadAllText(WebRootPath + RawPathNameToFolderAfterWWWRoot);

            Ploegen = new List<Ploeg>();

            //Execute a loop over the rows.
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(row))
                {
                    string[] csvRow = row.Split(':');
                    if (csvRow[0].StartsWith('-'))
                    {
                        //This is the char value that we use to ignore csv-lines.
                    }
                    else
                    {
                        Ploegen.Add(new Ploeg()
                        {
                            Ploegnaam = csvRow[0],
                            Locatie = csvRow[1],
                            Adres = csvRow[2]
                        });
                    }
                }
            }

            foreach (CalendarEvent lovCE in KalenderEvents)
            {
                foreach (Ploeg p in Ploegen)
                {
                    if (lovCE.ThuisPloeg.ToLower().Trim() == p.Ploegnaam.ToLower().Trim())
                    {
                        lovCE.Adres = p.Adres;
                        lovCE.Locatie = p.Locatie;
                    }
                }
            }
        }
    }
}