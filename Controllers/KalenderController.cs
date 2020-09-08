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

namespace WebApplication4.Controllers
{
    public class KalenderController : Controller
    {
        private readonly ILogger<KalenderController> _logger;
        private readonly IWebHostEnvironment _env;

        public KalenderController(ILogger<KalenderController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var lovList = await GetKalenderEvents();
            return View(new KalenderObject(lovList));
        }

        private async Task<List<CalendarEvent>> GetKalenderEvents()
        {
            var lovList = new List<CalendarEvent>();
            var serviceValues = GetSheetsService().Spreadsheets.Values;
            var response = await serviceValues.Get(SpreadsheetId, ReadRange).ExecuteAsync();
            foreach (var lovRow in response.Values)
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
        private const string ReadRange = "Blad1!A:H";

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
    }
}