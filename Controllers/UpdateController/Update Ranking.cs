using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication4.Helpers;
using WebApplication4.Models;
using WebApplication4.Models.Files;

namespace WebApplication4.Controllers
{
    public partial class UpdateController : Controller
    {
        private const string Suffix_Path_To_Ranking_Folder = @"\seed\rankings\";
        private const string WebAddressOfRanking = "http://rlv.be/klassement-reeks-a-2/";
        private const string Extension_Of_Ranking_Files = "csv";

        public string Total_Path_To_Ranking_Folder => WebRootPath + Suffix_Path_To_Ranking_Folder;
        public string WebRootPath => _env.WebRootPath;

        private readonly ILogger<UpdateController> _logger;
        private readonly IWebHostEnvironment _env;

        public UpdateController(ILogger<UpdateController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            ViewBag.CurrentServerTime = "Current server time: " + DateTime.Now.ToString();
            ViewBag.LocalTime = "Local time: " + DateTimeExtensions.Now_In_European_Time_Zone().ToString();
            return View();
        }

        public IActionResult StoreCurrentRankingFromRLV()
        {
            try
            {
                UpdateResult lovReturnValue;
                var lovTuple_Test_Result = Is_It_Needed_To_Do_An_Update_For_The_Rankings(Total_Path_To_Ranking_Folder);

                if (lovTuple_Test_Result.Item1)
                {
                    string lovNewFullFileName = $"{Total_Path_To_Ranking_Folder}{lovTuple_Test_Result.Item2}.{Extension_Of_Ranking_Files}";
                    RankingFile lovRankingFile = RankingFile.From_Web_Address(WebAddressOfRanking);
                    lovRankingFile.Save(lovNewFullFileName);

                    lovReturnValue = new UpdateResult()
                    {
                        Success = true,
                        Message = $"Ranking updated, new file made with name '{Path.GetFileName(lovNewFullFileName)}'."
                    };
                }
                else
                {
                    lovReturnValue = new UpdateResult()
                    {
                        Success = true,
                        Message = $"Ranking not updated, was not needed."
                    };
                }

                return View("Result", lovReturnValue);
            }
            catch (Exception)
            {
                var lovUpdate_Result_Fail = new UpdateResult()
                {
                    Success = false,
                    Message = $"Something went wrong during the method '{Random_Methods.GetCurrentMethod()}'."
                };
                return View("Result", lovUpdate_Result_Fail);
            }
        }

        #region Helper Methods

        public static RankingFile Store_Ranking_To_File(string povTotal_Path_To_Ranking_Folder)
        {
            var lovTuple_Test_Result = Is_It_Needed_To_Do_An_Update_For_The_Rankings(povTotal_Path_To_Ranking_Folder);
            string lovNewFullFileName = $"{povTotal_Path_To_Ranking_Folder}{lovTuple_Test_Result.Item2}.{Extension_Of_Ranking_Files}";
            RankingFile lovRankingFile = RankingFile.From_Web_Address(WebAddressOfRanking);
            lovRankingFile.Save(lovNewFullFileName);
            return lovRankingFile;
        }

        public static (bool, string) Is_It_Needed_To_Do_An_Update_For_The_Rankings(string povTotal_Path_To_Ranking_Folder)
        {
            DateTime lovNow = DateTimeExtensions.Now_In_European_Time_Zone();
            DateTime? lovLatest_Date_Nullable = Get_Latest_Date_Of_Ranking_Stored(povTotal_Path_To_Ranking_Folder);
            var lovNew_File_Date = lovNow.StartOfWeek(DayOfWeek.Monday);

            if (lovLatest_Date_Nullable == null)
            {
                goto Yes;
            }
            else
            {
                DateTime lovLatest_Date = (DateTime)lovLatest_Date_Nullable;
                TimeSpan lovTimeSpan = lovNow - lovLatest_Date;
                if (lovTimeSpan > TimeSpan.FromDays(7))
                {
                    goto Yes;
                }
                else
                {
                    goto No;
                }
            }

        Yes:
            {
                return (true, lovNew_File_Date.To_File_Name_Without_Extension());
            }

        No:
            {
                return (false, lovNew_File_Date.To_File_Name_Without_Extension());
            }
        }

        public static DateTime? Get_Latest_Date_Of_Ranking_Stored(string povTotal_Path_To_Ranking_Folder)
        {
            var lovAll_Ranking_Files = Directory.GetFiles(povTotal_Path_To_Ranking_Folder);
            var lovAll_DateTimes = new List<DateTime>();
            foreach (var lovFile in lovAll_Ranking_Files)
            {
                var lovFile_Name = Path.GetFileName(lovFile);
                var lovStringSplit = lovFile_Name.Split(new char[] { '_', '.' });
                var lovIntSplit = new int[3] {
                Convert.ToInt32(lovStringSplit[0]),
                Convert.ToInt32(lovStringSplit[1]),
                Convert.ToInt32(lovStringSplit[2])
                };
                lovAll_DateTimes.Add(new DateTime(year: lovIntSplit[0], month: lovIntSplit[1], day: lovIntSplit[2]));
            }

            if (lovAll_DateTimes.Any())
            {
                lovAll_DateTimes.Sort();
                return lovAll_DateTimes.Last();
            }
            else
            {
                return null;
            }
        }

        public static string Get_Latest_Full_Path_File_Name_For_Ranking(string povTotal_Path_To_Ranking_Folder)
        {
            DateTime? lovDateTime_Nullable = Get_Latest_Date_Of_Ranking_Stored(povTotal_Path_To_Ranking_Folder);
            if (lovDateTime_Nullable == null)
            {
                return "";
            }
            else
            {
                DateTime lovDateTime = (DateTime)lovDateTime_Nullable;
                string ShortFileName = lovDateTime.To_File_Name_Without_Extension();
                return $"{povTotal_Path_To_Ranking_Folder}{ShortFileName}.{Extension_Of_Ranking_Files}";
            }
        }

        #endregion Helper Methods
    }
}