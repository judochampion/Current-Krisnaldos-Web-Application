using System.Collections.Generic;
using System.IO;

namespace WebApplication4.Models
{
    public class AlbumModel
    {
        public string RawName { get; set; }
        public List<Picture> Pictures { get; set; } = new List<Picture>();

        public AlbumModel(string povRawName)
        {
            RawName = povRawName;
        }
    }

    public class Picture
    {
        public AlbumModel Album { get; set; }
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
                return @"http://www.krisnaldos.be/seed/albums/" + Album.RawName + @"/compressed/" + RawFileName;
            }
        }

        public Picture(AlbumModel povAlbum, string povRawFileName)
        {
            Album = povAlbum;
            RawFileName = povRawFileName;
        }
    }
}