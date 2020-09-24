using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Helpers;
using WebApplication4.Models;
using WebApplication4.Models.Files;

namespace WebApplication4.Controllers
{
    public partial class UpdateController : Controller
    {
        private const string Suffix_Path_To_Calendar_Folder = @"\seed\calendars\";
        private const string Extension_Of_Calendar_Files = "csv";

        public string Total_Path_To_Calendar_Folder => WebRootPath + Suffix_Path_To_Calendar_Folder;

        public IActionResult StoreCurrentCalendarFromGoogleSheet()
        {
            try
            {
                var lovTuple_Test_Result = Is_It_Needed_To_Do_An_Update_For_The_Calendars(Total_Path_To_Calendar_Folder);
                string lovNewFullFileName = $"{Total_Path_To_Calendar_Folder}{lovTuple_Test_Result.Item2}.{Extension_Of_Calendar_Files}";
                CalendarFile lovCalendarFile = CalendarFile.From_Google_Drive();
                lovCalendarFile.Save(lovNewFullFileName);

                var lovReturnValue = new UpdateResult()
                {
                    Success = true,
                    Message = $"Calendar updated, new file made with name '{Path.GetFileName(lovNewFullFileName)}'. In the case of the calendar, we always update if this Action {Random_Methods.GetCurrentMethod()} was called."
                };

                return View("Result", lovReturnValue);
            }
            catch (Exception lovException)
            {
                var lovUpdate_Result_Fail = new UpdateResult()
                {
                    Success = false,
                    Message = $"Something went wrong during the method '{Random_Methods.GetCurrentMethod()}'. \n\n Exception :{lovException.Message}"
                };
                return View("Result", lovUpdate_Result_Fail);
            }
        }

        #region Helper Methods

        public static CalendarFile Store_Calendar_To_File(string povTotal_Path_To_Calendar_Folder)
        {
            var lovTuple_Test_Result = Is_It_Needed_To_Do_An_Update_For_The_Calendars(povTotal_Path_To_Calendar_Folder);
            string lovNewFullFileName = $"{povTotal_Path_To_Calendar_Folder}{lovTuple_Test_Result.Item2}.{Extension_Of_Calendar_Files}";
            CalendarFile lovCalendarFile = CalendarFile.From_Google_Drive();
            lovCalendarFile.Save(lovNewFullFileName);
            return lovCalendarFile;
        }

        public static (bool, string) Is_It_Needed_To_Do_An_Update_For_The_Calendars(string povTotal_Path_To_Calendar_Folder)
        {
            DateTime lovNow = DateTimeExtensions.Now_In_European_Time_Zone();
            DateTime? lovLatest_Date_Nullable = Get_Latest_Date_Of_Calendar_Stored(povTotal_Path_To_Calendar_Folder);
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

        public static DateTime? Get_Latest_Date_Of_Calendar_Stored(string povTotal_Path_To_Calendar_Folder)
        {
            var lovAll_Ranking_Files = Directory.GetFiles(povTotal_Path_To_Calendar_Folder);
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

        public static string Get_Latest_Full_Path_File_Name_For_Calendar(string povTotal_Path_To_Calendar_Folder)
        {
            DateTime? lovDateTime_Nullable = Get_Latest_Date_Of_Calendar_Stored(povTotal_Path_To_Calendar_Folder);
            if (lovDateTime_Nullable == null)
            {
                return "";
            }
            else
            {
                DateTime lovDateTime = (DateTime)lovDateTime_Nullable;
                string ShortFileName = lovDateTime.To_File_Name_Without_Extension();
                return $"{povTotal_Path_To_Calendar_Folder}{ShortFileName}.{Extension_Of_Calendar_Files}";
            }
        }

        #endregion Helper Methods
    }
}