using System;
using System.Collections.Generic;

namespace WebApplication4.Models
{
    public class Album
    {
        public string RawName { get; set; }
        public List<Picture> Pictures { get; set; } = new List<Picture>();

        public string DisplayName { get; set; }

        public Album(string povRawName)
        {
            RawName = povRawName;
        }

        public string EffectiveDisplayName
        {
            get
            {
                return (string.IsNullOrEmpty(DisplayName)) ? "Onbekend" : DisplayName;
            }
        }
    }
}