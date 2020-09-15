using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication4.Models;
using WebApplication4.Models.Files;

namespace WebApplication4.Controllers
{
    public class KalenderController : Controller
    {
        private const string Suffix_Path_To_Ranking_Folder = @"\seed\rankings\";
        private const string Suffix_Path_To_Locaties_File = @"\seed\stadia\locaties.txt";
        public string Total_Path_To_Ranking_Folder => WebRootPath + Suffix_Path_To_Ranking_Folder;
        public string Total_Path_To_Locaties_File => WebRootPath + Suffix_Path_To_Locaties_File;
        public string WebRootPath => _env.WebRootPath;
        private const string Extension_Of_Ranking_Files = "csv";

        private readonly ILogger<KalenderController> _logger;
        private readonly IWebHostEnvironment _env;

        public KalenderController(ILogger<KalenderController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            string lovWebRootPath = _env.WebRootPath;

            Task<RankingFile> lovTaskRanking = Task.Run(() => GetRanking_Sync());
            Task<List<CalendarEvent>> lovTaskKalenderEvents = GetKalenderEvents_Async();
            Task<List<Ploeg>> lovTaskGetPloegen = GetPloegen_Async();
            return View(new KalenderObject(await lovTaskKalenderEvents, await lovTaskGetPloegen, await lovTaskRanking));
        }

        private async Task<List<Ploeg>> GetPloegen_Async()
        {
            var lovPloegenList = new List<Ploeg>();

            string csvData = await System.IO.File.ReadAllTextAsync(Total_Path_To_Locaties_File);
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
                        lovPloegenList.Add(new Ploeg()
                        {
                            Ploegnaam = csvRow[0],
                            Locatie = csvRow[1],
                            Adres = csvRow[2]
                        });
                    }
                }
            }

            return lovPloegenList;
        }

        private async Task<List<CalendarEvent>> GetKalenderEvents_Async()
        {
            var lovList = new List<CalendarEvent>();
            var serviceValues = GetSheetsService().Spreadsheets.Values;
            var response = await serviceValues.Get(SpreadsheetId, ReadRange).ExecuteAsync();
            foreach (var lovRow in response.Values.Skip(1))
            {
                string[] lovString = new string[lovRow.Count];
                for (int i = 0; i < lovString.Count(); i++)
                {
                    lovString[i] = (string)lovRow[i];
                }
                lovList.Add(new CalendarEvent(lovString));
            }
            return lovList;
        }

        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private const string SpreadsheetId = "1a_nHEghoIFLrN7GLTRNiSxn5MNjsHyX7TTFg2WUdos0";
        private const string GoogleCredentialsFileName = "kwisnaldos2019-7f67ce0c108f.json";
        /*
           Sheet1 - tab name in a spreadsheet
           A:B     - range of values we want to receive
        */
        private const string ReadRange = "Blad1!A:K";

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

        private RankingFile GetRanking_Sync()
        {
            var lovTuple_Test_Result = UpdateController.Is_It_Needed_To_Do_An_Update_For_The_Rankings(Total_Path_To_Ranking_Folder);
            if (lovTuple_Test_Result.Item1)
            {
                return UpdateController.Store_Ranking_To_File(Total_Path_To_Ranking_Folder);
            }
            else
            {
                return RankingFile.From_File_Path(UpdateController.Get_Latest_Full_Path_File_Name_For_Ranking(Total_Path_To_Ranking_Folder));
            }
        }
    }
}