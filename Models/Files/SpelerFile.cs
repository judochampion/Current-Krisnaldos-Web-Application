﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApplication4.Models.Files
{
    public class SpelerFile
    {
        private const string Extension_Of_Player_File = "csv";
        private const string FileName_Player_File = "2021-2022";
        private const string Suffix_Path_To_Players_Folder = @"\seed\players\";

        public List<SpelerItem> SpelerItems { get; set; } = new List<SpelerItem>();

        public static string Total_Path_To_Players_File(string povWebRootPath)
        {
            return povWebRootPath + Suffix_Path_To_Players_Folder + FileName_Player_File + "." + Extension_Of_Player_File;
        }

        public static SpelerFile From_File_Path(string povFilePath)
        {
            SpelerFile lovSpelerFile = new SpelerFile();
            var lovLines = File.ReadAllLines(povFilePath);
            foreach (var lovLine in lovLines)
            {
                lovSpelerFile.SpelerItems.Add(new SpelerItem(lovLine));
            }
            lovSpelerFile.SpelerItems = lovSpelerFile.SpelerItems.OrderBy(lovSpeler => lovSpeler.DisplayName_In_Lower).ToList();
            return lovSpelerFile;
        }

        public static SpelerFile GetSpelerFile_Sync(string povWebRootPath)
        {
            return From_File_Path(Total_Path_To_Players_File(povWebRootPath));
        }
    }
}