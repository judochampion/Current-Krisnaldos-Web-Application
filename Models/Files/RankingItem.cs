using System.Collections.Generic;

namespace WebApplication4.Models.Files
{
    public class RankingItem
    {
        public string Ploeg_Naam_In_Lower_Case { get; set; }

        public string Aantal_Punten { get; set; }

        public string Klassement_Positie { get; set; }

        public List<string> List_Of_Strings
        {
            get
            {
                return new List<string>()
                {
                    Klassement_Positie.ToString(),
                    Ploeg_Naam_In_Lower_Case,
                    Aantal_Punten.ToString(),
                };
            }
        }

        public string Item_In_String => string.Join(',', List_Of_Strings);

        public RankingItem(string lovLine)
        {
            var lovLineSplit = lovLine.Split(new char[] { ',' });
            Klassement_Positie = lovLineSplit[0];
            Ploeg_Naam_In_Lower_Case = lovLineSplit[1];
            Aantal_Punten = lovLineSplit[2];
        }

        public RankingItem()
        {
        }
    }
}