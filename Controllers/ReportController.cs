using Microsoft.AspNetCore.Mvc;
using MyFleetManagerPro.DAL;
using MyFleetManagerPro.Models;
using MyFleetManagerPro.ViewModels;

namespace MyFleetManagerPro.Controllers
{
    public class ReportController : Controller
    {
        FleetDAL fDAL = new FleetDAL();
        [HttpGet]
        public IActionResult Generate()
        {
            ViewBag.fleets = fDAL.GetAllFleets();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Generate([Bind] AddReportViewModel reportModel)
        {
            Console.WriteLine(reportModel.fleetID);
            return RedirectToAction("Index", reportModel);
           
        }
        
        public IActionResult Index(AddReportViewModel reportModel)
        {
            Console.WriteLine("Index");
            Console.WriteLine($"{reportModel.fleetID}");    
            List<Report> fleetReport = Report.FleetReport(reportModel.fleetID);
            foreach (Report report in fleetReport)
            {
                Console.WriteLine("+++++++++");
                Console.WriteLine(report.RegNo);
            }
            return View(fleetReport);
        }
    }
}
