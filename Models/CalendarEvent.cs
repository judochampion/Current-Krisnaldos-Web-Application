using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public enum MatchSide { Thuis, Uit }

    public enum MatchType { Friendly, Competitie, Beker }

    public class CalendarEvent : IComparable
    {
        public DateTime Tijdstip { get; }

        public const string EigenNaam = "De Krisnaldo's";

        public MatchSide MatchSide { get; }
        public MatchType MatchType { get; }

        public string Tegenstander { get; }

        public string DagDisplay => Tijdstip.ToString("D", CultureInfo.CreateSpecificCulture("nl-NL"));
        public string DagDisplay_Short => Tijdstip.ToString("d", CultureInfo.CreateSpecificCulture("nl-NL"));

        public string UurDisplay => Tijdstip.ToString("t", CultureInfo.CreateSpecificCulture("nl-NL"));

        public string ThuisPloeg => MatchSide == MatchSide.Thuis ? "De Krisnaldo's" : Tegenstander;

        public string LocatieDisplay => String.IsNullOrWhiteSpace(Locatie) ? "?" : Locatie;
        public string Locatie => TegenstanderPloegObject?.Locatie;

        public string AdresDisplay => String.IsNullOrWhiteSpace(Adres) ? "?" : Adres;
        public string Adres => TegenstanderPloegObject?.Adres;

        public int Our_Score { get; }
        public int Their_Score { get; }

        public string ClassColor
        {
            get
            {
                if (Our_Score > Their_Score)
                {
                    return "circle_green";
                }
                else if (Our_Score == Their_Score)
                {
                    return "circle_yellow";
                }
                else
                {
                    return "circle_red";
                }
            }
        }

        public string MatchCode { get; }

        public Ploeg TegenstanderPloegObject { get; set; }

        public string TegenstanderPositieDisplay => String.IsNullOrWhiteSpace(TegenstanderPloegObject?.Positie_In_Klassement) ? "?" : (TegenstanderPloegObject.Positie_In_Klassement == "1" || TegenstanderPloegObject.Positie_In_Klassement == "8" ? TegenstanderPloegObject.Positie_In_Klassement + "ste" : TegenstanderPloegObject.Positie_In_Klassement + "de");

        public string TegenstanderPunten => String.IsNullOrWhiteSpace(TegenstanderPloegObject?.Punten_Aantal) ? "?" : (TegenstanderPloegObject.Punten_Aantal == "1" ? "(1 punt)" : "(" + TegenstanderPloegObject.Punten_Aantal + " punten)");

        public string TegenstanderKlassementDisplayTotaal => $"{TegenstanderPositieDisplay} {TegenstanderPunten}";

        public string MatchDisplay
        {
            get
            {
                if (MatchSide == MatchSide.Thuis)
                {
                    return EigenNaam + " - " + Tegenstander;
                }

                return Tegenstander + " - " + EigenNaam;
            }
        }

        public string ScoreDisplay
        {
            get
            {
                if (!Score_Known_Yet) return "? - ?";

                if (MatchSide == MatchSide.Thuis)
                {
                    return Our_Score + " - " + Their_Score;
                }

                return Their_Score + " - " + Our_Score;
            }
        }

        public bool Score_Known_Yet { get; }

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
            MatchCode = lovStringArray[8];
            if (lovStringArray.Length > 9)
            {
                Our_Score = Convert.ToInt32(lovStringArray[9]);
                Their_Score = Convert.ToInt32(lovStringArray[10]);
                Score_Known_Yet = true;
            }
            else
            {
                Score_Known_Yet = false;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is CalendarEvent)
            {
                return Tijdstip.CompareTo(((CalendarEvent)obj).Tijdstip);
            }
            return 0;
        }
    }
}