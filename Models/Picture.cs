using System.Collections.Generic;
using System.IO;

namespace WebApplication4.Models
{
    public class Picture
    {
        public Album Album { get; set; }
        public string RawFileName { get; set; }

        public string RawFileNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(RawFileName);
            }
        }

        public string UltimateSource
        {
            get
            {
                return @"http://www.krisnaldos.be/seed/albums/" + Album.RawName + @"/" + RawFileName;
            }
        }

        public Picture(Album povAlbum, string povRawFileName)
        {
            Album = povAlbum;
            RawFileName = povRawFileName;
        }
    }
}