using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApplication4.Helpers;
using WebApplication4.Models.Files;

namespace WebApplication4.Models
{
    public class KalenderObject
    {
        public CalendarFile Calendar_File { get; }

        public List<Ploeg> Ploegen { get; }

        public RankingFile Ranking_File { get; }

        public List<CalendarItem> Past_Events => Calendar_File.Past_Events_Sorted;

        public List<CalendarItem> Future_Events => Calendar_File.Future_Events_Sorted;

        public int NumberOfKalenderEvents => Calendar_File.CalendarItems.Count;

        public KalenderObject(CalendarFile povCurrent_Calendar, List<Ploeg> povPloegen, RankingFile povCurrent_Ranking)
        {
            Calendar_File = povCurrent_Calendar;
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

            foreach (CalendarItem lovCE in Calendar_File.CalendarItems)
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