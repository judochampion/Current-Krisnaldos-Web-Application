using System.Collections.Generic;
using System.IO;

namespace WebApplication4.Models
{
    public class Sponsor
    {
        public string DisplayName { get; set; }

        public string RawPictureName { get; set; }
        public string Link { get; set; }

        public string UltimateSource
        {
            get
            {
                return @"https://www.krisnaldos.be/seed/sponsors/compressed/" + RawPictureName;
            }
        }
    }
}