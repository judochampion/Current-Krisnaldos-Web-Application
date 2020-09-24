namespace WebApplication4.Models.Files
{
    public class SpelerItem
    {
        public string Familienaam { get; }

        public string Voornaam { get; }

        public string FullNameInCaps => $"{Familienaam} {Voornaam}".ToUpper();

        public string DisplayName_In_Lower => $"{Voornaam} {Familienaam}";

        public string ID => $"{Voornaam}{Familienaam}".ToLower().Replace(" ","");

        public string Stamnummer { get; }

        public SpelerItem(string lovLine)
        {
            var lovLineSplit = lovLine.Split(new char[] { ',' });
            Familienaam = lovLineSplit[0];
            Voornaam = lovLineSplit[1];
            Stamnummer = lovLineSplit[2];
        }
    }
}