using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebApplication4.Controllers;

namespace WebApplication4.Models.Files
{
    public class RankingFile
    {
        private const string Suffix_Path_To_Ranking_Folder = @"\seed\rankings\";

        public List<RankingItem> RankingItems { get; set; } = new List<RankingItem>();

        public string Full_String => String.Join('\n', RankingItems.Select(ri => ri.Item_In_String).ToList());

        public void Save(string lovPathToSave)
        {
            File.WriteAllText(lovPathToSave, Full_String);
        }

        public RankingFile()
        {
        }

        public static RankingFile From_File_Path(string povFilePath)
        {
            RankingFile lovRanking = new RankingFile();
            var lovLines = File.ReadAllLines(povFilePath);
            foreach (var lovLine in lovLines)
            {
                lovRanking.RankingItems.Add(new RankingItem(lovLine));
            }
            return lovRanking;
        }

        public static RankingFile From_Web_Address(string povWebAddress)
        {
            WebRequest request = WebRequest.Create(povWebAddress);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            string html = String.Empty;
            using (StreamReader sr = new StreamReader(data))
            {
                html = sr.ReadToEnd();
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var node1A = doc.DocumentNode.SelectSingleNode("(//table[@id='tablepress-KlasA'])[1]");
            var node_Body = node1A.SelectSingleNode(".//tbody");
            var nodes = node_Body.SelectNodes(".//tr");

            RankingFile lovRankingFile = new RankingFile();

            foreach (var node in nodes)
            {
                var tdnodes = node.SelectNodes(".//td");
                string lovPositie = tdnodes[0].InnerText;
                string lovPloegNaam = tdnodes[1].InnerText.ToLower().Trim();
                string lovPuntenAantal = tdnodes[9].InnerText.ToLower().Trim();
                if (lovPloegNaam == "gz tube") lovPloegNaam = "geel zwart tube";
                if (lovPloegNaam == "keuken roberdo") lovPloegNaam = "keukens roberdo";
                if (lovPloegNaam == "roberdo keuken") lovPloegNaam = "keukens roberdo";
                if (lovPloegNaam == "kvk denys") lovPloegNaam = "vk denys";
                lovRankingFile.RankingItems.Add(new RankingItem()
                {
                    Ploeg_Naam_In_Lower_Case = lovPloegNaam,
                    Klassement_Positie = lovPositie,
                    Aantal_Punten = lovPuntenAantal
                });
            }

            return lovRankingFile;
        }

        public static string Total_Path_To_Ranking_Folder(string povWebRootPath) => povWebRootPath + Suffix_Path_To_Ranking_Folder;

        public static RankingFile Get_Ranking_File_Sync(string povWebRootPath)
        {
            var lovTuple_Test_Result = UpdateController.Is_It_Needed_To_Do_An_Update_For_The_Rankings(Total_Path_To_Ranking_Folder(povWebRootPath));
            if (lovTuple_Test_Result.Item1)
            {
                return UpdateController.Store_Ranking_To_File(Total_Path_To_Ranking_Folder(povWebRootPath));
            }
            else
            {
                return From_File_Path(UpdateController.Get_Latest_Full_Path_File_Name_For_Ranking(Total_Path_To_Ranking_Folder(povWebRootPath)));
            }
        }
    }
}