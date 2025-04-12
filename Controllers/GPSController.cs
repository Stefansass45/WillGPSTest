using Microsoft.AspNetCore.Mvc;
using System.Text;
using WIllGPSTest.Models;
using System.Data;
using Newtonsoft.Json;

namespace WIllGPSTest.Controllers
{
    public class GpsController : Controller
    {
        private readonly string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "log.txt");
        public List<GPSModel> gpsData = new List<GPSModel>();
        public readonly IWebHostEnvironment _environment;

        public GpsController(IWebHostEnvironment environment)
        {
            _environment = environment;
            LoadDataFromFile();
        }

        [HttpPost("/gps")]
        public async Task<IActionResult> ReceiveData()
        {
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var jsonData = await reader.ReadToEndAsync();

                var newEntry = JsonConvert.DeserializeObject<GPSModel>(jsonData);

                if (newEntry != null)
                {
                    newEntry.Timestamp = DateTime.Now;
                    gpsData.Add(newEntry);

                    RewriteLogFile();
                }
            }

            return Ok("Data received");
        }

        [HttpGet("/logs")]
        public IActionResult ShowLogs()
        {
            if (!System.IO.File.Exists(logFilePath))
            {
                return Content("Geen logs nog nie.");
            }

            var logContent = System.IO.File.ReadAllText(logFilePath);
            return Content(logContent, "text/plain");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (gpsData.Count == 0)
            {
                InitialPopulation();
            }
            return View(gpsData);
        }

        public List<GPSModel> InitialPopulation()
        {
            gpsData.Add(new GPSModel(DateTime.Now, "Some raw data string"));
            gpsData.Add(new GPSModel(DateTime.Now, "Some raw data string"));

            RewriteLogFile();

            return gpsData;
        }

        private void LoadDataFromFile()
        {
            if (System.IO.File.Exists(logFilePath))
            {
                var content = System.IO.File.ReadAllText(logFilePath);
                gpsData = JsonConvert.DeserializeObject<List<GPSModel>>(content) ?? new List<GPSModel>();
            }
        }

        private void RewriteLogFile()
        {
            var jsonData = JsonConvert.SerializeObject(gpsData, Formatting.Indented);
            System.IO.File.WriteAllText(logFilePath, jsonData);
        }
    }
}