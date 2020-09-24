using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Novacode;
using Spire.Doc;
using Spire.Pdf;
using WebApplication4.Helpers;
using WebApplication4.Models;
using WebApplication4.Models.Files;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class DocumentenController : Controller
    {
        private readonly ILogger<LinksController> _logger;
        private readonly IWebHostEnvironment _env;

        private const string Suffix_Path_To_Documents_Folder = @"\seed\documents\";
        private const string Input_FileName_Uit_Blad_Front = @"input\blad_uit_front";
        private const string Input_FileName_Uit_Blad_Back = @"input\blad_uit_back";
        private const string Input_FileName_Thuis_Blad_Front = @"input\blad_thuis_front";
        private const string Input_FileName_Thuis_Blad_Back = @"input\blad_thuis_back";
        public string Total_Path_To_Document_Folder => WebRootPath + Suffix_Path_To_Documents_Folder;

        private const string Extension_Of_Document_Files = "docx";
        private const string Extension_Of_PDF_Files = "pdf";

        public string WebRootPath => _env.WebRootPath;

        public DocumentenController(ILogger<LinksController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> WedstrijdbladGenerator()
        {
            return View(await WedstrijdbladGeneratorViewModel.Init(WebRootPath));
        }

        [HttpPost]
        public async Task<IActionResult> WedstrijdbladGenerator(string[] povSelectedSpelerIDsArray, string povSelectedKapiteinID, string povSelectedLAVID)
        {
            int livNmbOfSelectedSpelers = povSelectedSpelerIDsArray.Count();

            Task<CalendarFile> lovKalenderFileTask = Task.Run(() => CalendarFile.Get_Kalender_File_Sync(WebRootPath));
            SpelerFile lovSpelerFile = await Task.Run(() => SpelerFile.GetSpelerFile_Sync(WebRootPath));
            List<SpelerItem> lovSelectedSpelerItems = new List<SpelerItem>();
            foreach (string lovSpelerIDFromForm in povSelectedSpelerIDsArray)
            {
                foreach (SpelerItem lovSpelerItem in lovSpelerFile.SpelerItems)
                {
                    if (lovSpelerIDFromForm == lovSpelerItem.ID)
                    {
                        lovSelectedSpelerItems.Add(lovSpelerItem);
                        continue;
                    }
                }
            }

            CalendarItem lovNextEvent = (await lovKalenderFileTask).First_Upcoming_Event;

            DocX lovDocument;

            if (lovNextEvent.MatchSide == MatchSide.Thuis)
            {
                lovDocument = await Task.Run(() => DocX.Load(Full_Path_From_File_Name(Input_FileName_Thuis_Blad_Front, Extension_Of_Document_Files)));
            }
            else
            {
                lovDocument = await Task.Run(() => DocX.Load(Full_Path_From_File_Name(Input_FileName_Uit_Blad_Front, Extension_Of_Document_Files)));
            }

            for (int i = 1; i <= 15; i++)
            {
                string lovFMT = "00";
                string lovNaamStringToReplace = $"NAAM{i.ToString(lovFMT)}";
                string lovStamnummerStringToReplace = $"SN{i.ToString(lovFMT)}";

                if (i <= livNmbOfSelectedSpelers)
                {
                    lovDocument.ReplaceText(lovStamnummerStringToReplace, lovSelectedSpelerItems[i - 1].Stamnummer);
                    lovDocument.ReplaceText(lovNaamStringToReplace, lovSelectedSpelerItems[i - 1].FullNameInCaps);
                }
                else
                {
                    lovDocument.ReplaceText(lovStamnummerStringToReplace, "");
                    lovDocument.ReplaceText(lovNaamStringToReplace, "");
                }
            }

            lovDocument.ReplaceText("DATE", lovNextEvent.Tijdstip.To_WedstrijdBlad_DateString());
            lovDocument.ReplaceText("WCO", lovNextEvent.MatchCode);
            var lovKapiteinItem = lovSpelerFile.SpelerItems.Where(lovSpelerItem => lovSpelerItem.ID == povSelectedKapiteinID).FirstOrDefault();
            lovDocument.ReplaceText("KAP-NAAM", lovKapiteinItem.FullNameInCaps);
            lovDocument.ReplaceText("KAP-NR", lovKapiteinItem.Stamnummer);
            lovDocument.ReplaceText("GRE-NAAM", "");
            lovDocument.ReplaceText("GRE-NR", "");
            var lovLAVItem = lovSpelerFile.SpelerItems.Where(lovSpelerItem => lovSpelerItem.ID == povSelectedLAVID).FirstOrDefault();
            lovDocument.ReplaceText("AFG-NAAM", lovLAVItem.FullNameInCaps);
            lovDocument.ReplaceText("AFG-NR", lovLAVItem.Stamnummer);
            lovDocument.ReplaceText("TRA-NAAM", "");
            lovDocument.ReplaceText("TRA-NR", "");

            if (lovNextEvent.MatchSide == MatchSide.Thuis)
            {
                lovDocument.ReplaceText("UITPLOEG", lovNextEvent.Tegenstander.ToUpper());
            }

            string lovFull_File_Name_DocX_Extension = Full_Path_From_File_Name(lovNextEvent.Tijdstip.To_File_Name_Without_Extension(), Extension_Of_Document_Files);
            lovDocument.SaveAs(lovFull_File_Name_DocX_Extension);

            Document lovSpireDocument = new Document(lovFull_File_Name_DocX_Extension);
            string lovFull_File_Name_PDF_Extension_Temp = Full_Path_From_File_Name(lovNextEvent.Tijdstip.To_File_Name_Without_Extension() + "_temp", Extension_Of_PDF_Files);
            lovSpireDocument.SaveToFile(lovFull_File_Name_PDF_Extension_Temp, Spire.Doc.FileFormat.PDF);

            string[] lovFiles;

            if (lovNextEvent.MatchSide == MatchSide.Thuis)
            {
                lovFiles = new string[] { lovFull_File_Name_PDF_Extension_Temp, Full_Path_From_File_Name(Input_FileName_Thuis_Blad_Back, Extension_Of_PDF_Files) };
            }
            else
            {
                lovFiles = new string[] { lovFull_File_Name_PDF_Extension_Temp, Full_Path_From_File_Name(Input_FileName_Uit_Blad_Back, Extension_Of_PDF_Files) };
            }

            string outputFile = Full_Path_From_File_Name($@"output\{lovNextEvent.Tijdstip.To_File_Name_Without_Extension()}", Extension_Of_PDF_Files);
            PdfDocumentBase doc = PdfDocument.MergeFiles(lovFiles);
            doc.Save(outputFile, Spire.Pdf.FileFormat.PDF);

            _ = Task.Run(() =>
              {
                  System.IO.File.Delete(lovFull_File_Name_PDF_Extension_Temp);
                  System.IO.File.Delete(lovFull_File_Name_DocX_Extension);
              });

            return Content("Succesvol gelukt op " + DateTimeExtensions.Now_In_European_Time_Zone().ToString() + "!");
        }

        public string Full_Path_From_File_Name(string lovFileName, string povExtension)
        {
            return $"{Total_Path_To_Document_Folder}{lovFileName}.{povExtension}";
        }
    }
}