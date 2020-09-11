using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace WebApplication4.Models
{
    public class KalenderObject
    {
        public string WebRootPath { get; set; }

        public string RawPathNameToFolderAfterWWWRoot { get; set; }

        public List<CalendarEvent> KalenderEvents { get; }

        public List<Ploeg> Ploegen { get; }

        public List<CalendarEvent> Past_Events
        {
            get
            {
                var lovNow = DateTime.Now;
                return KalenderEvents.Where(lovEvent => DateTime.Compare(lovEvent.Tijdstip, lovNow) < 0).ToList();
            }
        }

        public List<CalendarEvent> Future_Events
        {
            get
            {
                var lovNow = DateTime.Now;
                return KalenderEvents.Where(lovEvent => DateTime.Compare(lovNow, lovEvent.Tijdstip) < 0).ToList();
            }
        }

        public int NumberOfKalenderEvents => KalenderEvents.Count;

        public KalenderObject(List<CalendarEvent> povCalendarEvents, string povWebRootPath, string povRawPathName)
        {
            povCalendarEvents.Sort();
            KalenderEvents = povCalendarEvents;
            WebRootPath = povWebRootPath;
            RawPathNameToFolderAfterWWWRoot = povRawPathName;

            //movSponsors = new List<Sponsor>();

            string csvData = System.IO.File.ReadAllText(WebRootPath + RawPathNameToFolderAfterWWWRoot);

            Ploegen = new List<Ploeg>();

            //Execute a loop over the rows.
            foreach (string row in csvData.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(row))
                {
                    string[] csvRow = row.Split(':');
                    if (csvRow[0].StartsWith('-'))
                    {
                        //This is the char value that we use to ignore csv-lines.
                    }
                    else
                    {
                        Ploegen.Add(new Ploeg()
                        {
                            Ploegnaam = csvRow[0],
                            Locatie = csvRow[1],
                            Adres = csvRow[2]
                        });
                    }
                }
            }

            WebRequest request = WebRequest.Create("http://rlv.be/klassement-reeks-a-2/");
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string html = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }
            _ = html;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var node1A = doc.DocumentNode.SelectSingleNode("(//table[@id='tablepress-KlasA'])[1]");
            var node_Body = node1A.SelectSingleNode(".//tbody");
            var nodes = node_Body.SelectNodes(".//tr");

            foreach (var node in nodes)
            {
                var tdnodes = node.SelectNodes(".//td");
                string lovPositie = tdnodes[0].InnerText;
                string lovPloegNaam = tdnodes[1].InnerText.ToLower().Trim();
                string lovPuntenAantal = tdnodes[9].InnerText.ToLower().Trim();
                if (lovPloegNaam == "geel zwart tube") lovPloegNaam = "gz tube";
                if (lovPloegNaam == "keukens roberdo") lovPloegNaam = "keuken roberdo";


                foreach (Ploeg p in Ploegen)
                {
                    if (p.Ploegnaam.ToLower().Trim() == lovPloegNaam)
                    {
                        p.Positie_In_Klassement = lovPositie;
                        p.Punten_Aantal = lovPuntenAantal;
                    }
                }
            }

            foreach (CalendarEvent lovCE in KalenderEvents)
            {
                foreach (Ploeg p in Ploegen)
                {
                    if (lovCE.Tegenstander.ToLower().Trim() == p.Ploegnaam.ToLower().Trim())
                    {
                        lovCE.TegenstanderPloegObject = p;
                    }
                }
            }
        }
    }
}