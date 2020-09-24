using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Ploeg
    {
        public string Ploegnaam { get; set; }

        public string Ploegnaam_In_Caps => Ploegnaam.ToUpper();

        public string Locatie { get; set; }

        public string Adres { get; set; }

        public string Positie_In_Klassement { get; set; }

        public string Punten_Aantal { get; set; }

        public Ploeg()
        {
        }
    }
}