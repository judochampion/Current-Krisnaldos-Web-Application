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
        private const string Suffix_Path_To_Locaties_File = @"\seed\stadia\locaties.txt";

        public string Total_Path_To_Locaties_File => WebRootPath + Suffix_Path_To_Locaties_File;
        public string WebRootPath => _env.WebRootPath;

        private readonly ILogger<KalenderController> _logger;
        private readonly IWebHostEnvironment _env;

        public KalenderController(ILogger<KalenderController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            Task<RankingFile> lovTaskRanking = Task.Run(() => RankingFile.Get_Ranking_File_Sync(WebRootPath));
            Task<CalendarFile> lovTaskCalendar = Task.Run(() => CalendarFile.Get_Kalender_File_Sync(WebRootPath));
            Task<List<Ploeg>> lovTaskGetPloegen = Get_Ploegen_Async();
            return View(new KalenderObject(await lovTaskCalendar, await lovTaskGetPloegen, await lovTaskRanking));
        }

        private async Task<List<Ploeg>> Get_Ploegen_Async()
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
    }
}