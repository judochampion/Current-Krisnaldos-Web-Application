using System.Collections.Generic;

namespace WebApplication4.Models
{
    public class Album
    {
        public string RawName { get; set; }
        public List<Picture> Pictures { get; set; } = new List<Picture>();

        public Album(string povRawName)
        {
            RawName = povRawName;
        }
    }
}