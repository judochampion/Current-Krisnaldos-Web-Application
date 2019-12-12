using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public AlbumController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        private AlbumInfo GetOrderedAlbumInfo()
        {
            string lovWebRootPath = _env.WebRootPath;
            string lovFullFilePath = lovWebRootPath + @"\seed\albums\albuminfo.xml";
            XmlSerializer ser = new XmlSerializer(typeof(AlbumInfo));
            AlbumInfo lovAlbumInfo = null;
            using (FileStream fs = new FileStream(lovFullFilePath, FileMode.Open))
            {
                lovAlbumInfo = ser.Deserialize(fs) as AlbumInfo;

                // Do your stuff. Using blocks will call Dispose() for
                // you even if something goes wrong, as it's equal to a try/finally!
                // Also check how using statements can be chained without extra { }
            }

            Dictionary<DateTime, List<AlbumInfoAlbum>> lovDic = new Dictionary<DateTime, List<AlbumInfoAlbum>>();
            foreach (AlbumInfoAlbum lovAlbum in lovAlbumInfo.Albums)
            {
                DateTime lovDT = new DateTime(lovAlbum.Time.Year, lovAlbum.Time.Month, lovAlbum.Time.Day);
                if (!lovDic.ContainsKey(lovDT))
                {
                    lovDic.Add(lovDT, new List<AlbumInfoAlbum>());
                }
                lovDic[lovDT].Add(lovAlbum);
            }
            List<DateTime> lovList = lovDic.Keys.ToList();
            lovList.Sort();
            lovList.Reverse();

            List<AlbumInfoAlbum> retList = new List<AlbumInfoAlbum>();

            foreach (DateTime lovDT in lovList)
            {
                retList.AddRange(lovDic[lovDT]);
            }

            int livCounter = 0;
            foreach (AlbumInfoAlbum lovAIA in retList)
            {
                lovAlbumInfo.Albums[livCounter] = lovAIA;
                livCounter++;
            }

            return lovAlbumInfo;
        }

        private List<string> GetFileNames(string povAlbumNaam)
        {
            string lovWebRootPath = _env.WebRootPath;
            string lovFullFolderPath = lovWebRootPath + @"\seed\albums\" + povAlbumNaam + @"\compressed\";
            List<string> lovArray = Directory.GetFiles(lovFullFolderPath).ToList();
            List<string> lovReturnList = new List<string>();
            lovArray.ForEach(s => lovReturnList.Add(Path.GetFileName(s)));
            return lovReturnList;
        }

        public IActionResult Index()
        {
            return View(GetOrderedAlbumInfo());
        }

        public IActionResult Details(string albumnaam)
        {
            if (String.IsNullOrWhiteSpace(albumnaam))
            {
                return View("Index", GetOrderedAlbumInfo());
            }

            AlbumModel lovAlbum = new AlbumModel(albumnaam);

            foreach (string s in GetFileNames(albumnaam))
            {
                lovAlbum.Pictures.Add(new Picture(lovAlbum, s));
            }

            //var album = await _context.Album
            //    .Include(a => a.Fotos)
            //    .SingleOrDefaultAsync(a => a.RuweNaam == albumnaam);
            //if (album == null)
            //{
            //    return NotFound();
            //}
            return View(lovAlbum);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}