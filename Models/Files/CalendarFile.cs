using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using WebApplication4.Controllers;
using WebApplication4.Helpers;

namespace WebApplication4.Models.Files
{
    public class CalendarFile
    {
        public List<CalendarItem> CalendarItems { get; set; } = new List<CalendarItem>();

        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private const string SpreadsheetId = "1a_nHEghoIFLrN7GLTRNiSxn5MNjsHyX7TTFg2WUdos0";
        private const string GoogleCredentialsFileName = "kwisnaldos2019-7f67ce0c108f.json";
        private const string Suffix_Path_To_Calendar_Folder = @"\seed\calendars\";

        /*
           Sheet1 - tab name in a spreadsheet
           A:B     - range of values we want to receive
        */
        private const string ReadRange = "Blad1!A:K";

        public string Full_String => String.Join('\n', CalendarItems.Select(ci => ci.Item_In_String).ToList());

        public CalendarItem First_Upcoming_Event
        {
            get
            {
                CalendarItems.Sort();
                return Future_Events_Sorted.First();
            }
        }

        public List<CalendarItem> Past_Events_Sorted
        {
            get
            {
                CalendarItems.Sort();
                var lovNow = DateTimeExtensions.Now_In_European_Time_Zone();
                return CalendarItems.Where(lovEvent => DateTime.Compare(lovEvent.Tijdstip, lovNow) < 0).ToList();
            }
        }

        public List<CalendarItem> Future_Events_Sorted
        {
            get
            {
                CalendarItems.Sort();
                var lovNow = DateTimeExtensions.Now_In_European_Time_Zone();
                return CalendarItems.Where(lovEvent => DateTime.Compare(lovNow, lovEvent.Tijdstip) < 0).ToList();
            }
        }

        public void Save(string lovPathToSave)
        {
            File.WriteAllText(lovPathToSave, Full_String);
        }

        public CalendarFile()
        {
        }

        public static CalendarFile From_File_Path(string povFilePath)
        {
            CalendarFile lovCalendar = new CalendarFile();
            var lovLines = File.ReadAllLines(povFilePath);
            foreach (var lovLine in lovLines)
            {
                lovCalendar.CalendarItems.Add(new CalendarItem(lovLine.Split(new char[] { ',' })));
            }
            lovCalendar.CalendarItems.Sort();
            return lovCalendar;
        }

        public static CalendarFile From_Google_Drive()
        {
            CalendarFile lovCalendarFile = new CalendarFile();

            var serviceValues = GetSheetsService().Spreadsheets.Values;
            var response = serviceValues.Get(SpreadsheetId, ReadRange).ExecuteAsync().Result;
            foreach (var lovRow in response.Values.Skip(1))
            {
                string[] lovString = new string[lovRow.Count];
                for (int i = 0; i < lovString.Count(); i++)
                {
                    lovString[i] = (string)lovRow[i];
                }
                lovCalendarFile.CalendarItems.Add(new CalendarItem(lovString));
            }
            lovCalendarFile.CalendarItems.Sort();
            return lovCalendarFile;
        }

        private static SheetsService GetSheetsService()
        {
            using (var stream = new FileStream(GoogleCredentialsFileName, FileMode.Open, FileAccess.Read))
            {
                var serviceInitializer = new BaseClientService.Initializer
                {
                    HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(Scopes)
                };
                return new SheetsService(serviceInitializer);
            }
        }

        public static string Total_Path_To_Calendar_Folder(string povWebRoothPath) => povWebRoothPath + Suffix_Path_To_Calendar_Folder;

        public static CalendarFile Get_Kalender_File_Sync(string povWebRootPath)
        {
            var lovTuple_Test_Result = UpdateController.Is_It_Needed_To_Do_An_Update_For_The_Calendars(Total_Path_To_Calendar_Folder(povWebRootPath));
            if (lovTuple_Test_Result.Item1)
            {
                return UpdateController.Store_Calendar_To_File(Total_Path_To_Calendar_Folder(povWebRootPath));
            }
            else
            {
                return From_File_Path(UpdateController.Get_Latest_Full_Path_File_Name_For_Calendar(Total_Path_To_Calendar_Folder(povWebRootPath)));
            }
        }
    }
}