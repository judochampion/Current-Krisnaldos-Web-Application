using System.Collections.Generic;
using WebApplication4.Helpers;
using WebApplication4.Models;
using WebApplication4.Models.Files;

namespace WebApplication4.ViewModels
{
    public class WedstrijdbladGeneratorResultViewModel
    {
        public List<SpelerItem> Selected_Spelers { get; set; }
        public SpelerItem Kapitein { get; set; }
        public SpelerItem LAV { get; set; }
        public CalendarItem Next_Event { get; set; }
        public bool Was_It_Successful { get; set; }

        public string Complete_FilePath_To_Result_PDF { get; set; }

        public string FileName => $"Wedstrijdblad_Krisnaldos_Tegen_{Next_Event.Tegenstander.Trim().Replace(" ", "_")}_op_{Next_Event.Tijdstip.To_File_Name_Without_Extension()}.pdf";

        public string Message { get; set; }

        public string Disclaimer
        {
            get
            {
                if (Next_Event.MatchSide == MatchSide.Thuis)
                {
                    return "Dit is een thuismatch voor ons, print af op een oranje A4.";
                }
                else
                {
                    return "Dit is een uitmach voor ons, print af op een witte A4.";
                }
            }
        }
    }
}