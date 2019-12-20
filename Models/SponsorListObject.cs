using System.Collections.Generic;

namespace WebApplication4.Models
{
    public class SponsorListObject
    {
        private List<Sponsor> movSponsors;
        public string WebRootPath { get; set; }

        public string RawPathNameToFolderAfterWWWRoot { get; set; }

        public List<Sponsor> Sponsors
        {
            get
            {
                if (movSponsors == null)
                {
                    movSponsors = new List<Sponsor>();
                    string csvData = System.IO.File.ReadAllText(WebRootPath + RawPathNameToFolderAfterWWWRoot);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrWhiteSpace(row))
                        {
                            string[] csvRow = row.Split(',');
                            if (csvRow[0].StartsWith('-'))
                            {
                                //This is the char value that we use to ignore csv-lines.
                            }
                            else
                            {
                                Sponsor lovNewSponsor = new Sponsor()
                                {
                                    DisplayName = csvRow[0],
                                    RawPictureName = csvRow[1],
                                    Link = csvRow[2]
                                };
                                movSponsors.Add(lovNewSponsor);
                            }
                        }
                    }
                }
                return movSponsors;
            }
        }

        public int NumberOfSponsors => Sponsors.Count;

        public SponsorListObject(string povWebRootPath, string povRawPathName)
        {
            WebRootPath = povWebRootPath;
            RawPathNameToFolderAfterWWWRoot = povRawPathName;
        }
    }
}