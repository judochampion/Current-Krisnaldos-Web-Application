using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.Models.Files;

namespace WebApplication4.ViewModels
{
    public class WedstrijdbladGeneratorViewModel
    {
        public SpelerFile SpelerFile { get; set; }
        public CalendarFile CalendarFile { get; set; }

        public static async Task<WedstrijdbladGeneratorViewModel> Init(string povWebRootPath)
        {
            var lovCalendarFile = Task.Run(() => CalendarFile.Get_Kalender_File_Sync(povWebRootPath));
            var lovSpelerFile = Task.Run(() => SpelerFile.GetSpelerFile_Sync(povWebRootPath));
            return new WedstrijdbladGeneratorViewModel()
            {
                SpelerFile = await lovSpelerFile,
                CalendarFile = await lovCalendarFile
            };
        }
    }
}