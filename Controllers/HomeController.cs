using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using opg_201910_interview.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;
namespace opg_201910_interview.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var fileLocation = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ClientSettings")["FileDirectoryPath"];
            var fixedOrder = new List<string>();
            string[] files = Directory.GetFiles(fileLocation);
            List<XMLInfo> c = new List<XMLInfo>();
            if (fileLocation.Contains("ClientA"))
            {
                fixedOrder = new List<string> { "shovel", "waghor", "blaze", "discus" };
            }
            else if (fileLocation.Contains("ClientB"))
            {
                fixedOrder = new List<string> { "orca", "widget", "eclair", "talon" };
            }
            foreach (string file in files)
            {
                if (fixedOrder.Count() > 0 && fixedOrder.Any(file.Contains))
                {
                    XMLInfo n = new XMLInfo();
                    string fileName = file.Replace(@"\", "/").Replace(fileLocation + "/", "");
                    n.OrderNumber = GetOrderNumber(fixedOrder, fileName);
                    n.Filename = fileName;
                    c.Add(n);
                }
            }
            var result = c.OrderBy(c => c.OrderNumber).ThenBy(c => c.Filename);
            var json = JsonSerializer.Serialize(result.Select(w => w.Filename));
            ViewData["Filenames"] = json;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public class XMLInfo
        {
            public string Filename { get; set; }
            public int OrderNumber { get; set; }
        }
        private static int GetOrderNumber(List<string> stringOrder, string fileName)
        {
            int length = stringOrder.Count();
            for (int index = 0; index < length; index++)
            {
                if (fileName.Contains(stringOrder[index]))
                    return index;
            }
            return 0;
        }
    }
}
