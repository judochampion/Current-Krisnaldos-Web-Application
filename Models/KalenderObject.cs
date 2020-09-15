using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using WebApplication4.Controllers;
using WebApplication4.Helpers;
using WebApplication4.Models.Files;

namespace WebApplication4.Models
{
    public class KalenderObject
    {
        public List<CalendarEvent> KalenderEvents { get; }

        public List<Ploeg> Ploegen { get; }

        public RankingFile Ranking_File { get; }

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

        public KalenderObject(List<CalendarEvent> povCalendarEvents, List<Ploeg> povPloegen, RankingFile povCurrent_Ranking)
        {
            povCalendarEvents.Sort();
            KalenderEvents = povCalendarEvents;
            Ploegen = povPloegen;
            Ranking_File = povCurrent_Ranking;

            foreach (var lovRankingItem in Ranking_File.RankingItems)
            {
                foreach (Ploeg p in Ploegen)
                {
                    if (p.Ploegnaam.ToLower().Trim() == lovRankingItem.Ploeg_Naam_In_Lower_Case)
                    {
                        p.Positie_In_Klassement = lovRankingItem.Klassement_Positie;
                        p.Punten_Aantal = lovRankingItem.Aantal_Punten;
                    }
                }
            }

            foreach (CalendarEvent lovCE in KalenderEvents)
            {
                foreach (Ploeg p in Ploegen)
                {
                    if (lovCE.Tegenstander.ToLower().Trim() == p.Ploegnaam.ToLower().Trim())
                    {
                        lovCE.TegenstanderPloegObject = p;
                    }
                }
            }
        }
    }
}