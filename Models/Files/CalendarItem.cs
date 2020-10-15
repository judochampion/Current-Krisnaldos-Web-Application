using System;
using System.Collections.Generic;
using System.Globalization;

namespace WebApplication4.Models
{
    public enum MatchSide { Thuis, Uit }

    public enum MatchType { Friendly, Competitie, Beker }

    public class CalendarItem : IComparable
    {
        public const string EigenNaam = "De Krisnaldo's";

        #region Properties

        public DateTime Tijdstip { get; }

        public string[] Raw_String_Array_Of_Item { get; }

        public MatchSide MatchSide { get; }
        public MatchType MatchType { get; }

        public string Tegenstander { get; }

        public string DagDisplay => Tijdstip.ToString("D", CultureInfo.CreateSpecificCulture("nl-NL"));
        public string DagDisplay_Short => Tijdstip.ToString("d", CultureInfo.CreateSpecificCulture("nl-NL"));

        public string UurDisplay => Tijdstip.ToString("t", CultureInfo.CreateSpecificCulture("nl-NL"));

        public string ThuisPloeg => MatchSide == MatchSide.Thuis ? "De Krisnaldo's" : Tegenstander;

        public string LocatieDisplay => String.IsNullOrWhiteSpace(Locatie) ? "?" : Locatie;
        public string Locatie => ThuisPloegObject?.Locatie;

        public Ploeg KrisnaldoPloegObject { get; set; }

        public Ploeg ThuisPloegObject => (MatchSide == MatchSide.Thuis) ? KrisnaldoPloegObject : TegenstanderPloegObject;

        public string AdresDisplay => String.IsNullOrWhiteSpace(Adres) ? "?" : Adres;
        public string Adres => ThuisPloegObject?.Adres;

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

        public string Item_In_String => string.Join(',', Raw_String_Array_Of_Item);

        #endregion Properties

        public CalendarItem(string[] lovStringArray)
        {
            Raw_String_Array_Of_Item = lovStringArray;
            Tijdstip = new DateTime(
                year: Convert.ToInt32(Raw_String_Array_Of_Item[2]),
                month: Convert.ToInt32(Raw_String_Array_Of_Item[1]),
                day: Convert.ToInt32(Raw_String_Array_Of_Item[0]),
                hour: Convert.ToInt32(Raw_String_Array_Of_Item[3]),
                minute: Convert.ToInt32(Raw_String_Array_Of_Item[4]),
                second: 0);
            MatchType = (MatchType)Enum.Parse(typeof(MatchType), Raw_String_Array_Of_Item[5], true);
            MatchSide = (MatchSide)Enum.Parse(typeof(MatchSide), Raw_String_Array_Of_Item[6], true);
            Tegenstander = Raw_String_Array_Of_Item[7];
            MatchCode = Raw_String_Array_Of_Item[8];
            if (Raw_String_Array_Of_Item.Length > 9)
            {
                Our_Score = Convert.ToInt32(Raw_String_Array_Of_Item[9]);
                Their_Score = Convert.ToInt32(Raw_String_Array_Of_Item[10]);
                Score_Known_Yet = true;
            }
            else
            {
                Score_Known_Yet = false;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is CalendarItem)
            {
                return Tijdstip.CompareTo(((CalendarItem)obj).Tijdstip);
            }
            return 0;
        }
    }
}