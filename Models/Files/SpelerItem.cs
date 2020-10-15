using System;
using System.Linq;

namespace WebApplication4.Models.Files
{
    public class SpelerItem
    {
        public string Familienaam { get; }

        public string Voornaam { get; }

        public string FullNameInCaps => $"{Familienaam} {Voornaam}".ToUpper();

        public string DisplayName_In_Lower => $"{Familienaam} {Voornaam}";
        public string DisplayName_In_Lower_Met_Rugnummer => DisplayName_In_Lower + (HasVastRugnummer ? " (vast rugnummer: " + Rugnummer + ")" : "");
        public string ID => $"{Voornaam}{Familienaam}".ToLower().Replace(" ", "");

        public string Stamnummer { get; }

        public int Rugnummer { get; }
        public bool HasVastRugnummer { get; }

        public SpelerItem(string lovLine)
        {
            var lovLineSplit = lovLine.Split(new char[] { ',' });
            Familienaam = lovLineSplit[0];
            Voornaam = lovLineSplit[1];
            Stamnummer = lovLineSplit[2];
            if (lovLineSplit.Count() > 3)
            {
                HasVastRugnummer = true;
                Rugnummer = Convert.ToInt32(lovLineSplit[3]);
            }
            else
            {
                HasVastRugnummer = false;
            }
        }
    }
}