using Microsoft.AspNetCore.Mvc;
using MyFleetManagerPro.DAL;
using MyFleetManagerPro.Models;
using MyFleetManagerPro.ViewModels;

namespace MyFleetManagerPro.Controllers
{
    public class VehicleController : Controller
    {
        VehicleDAL vDAL = new VehicleDAL();
        FleetDAL fDAL = new FleetDAL();
        DriverDAL dDAL = new DriverDAL();
        public IActionResult Index()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            List<Fleet> fleets = new List<Fleet>();
            List<Driver> drivers = new List<Driver>();

            vehicles = vDAL.GetAllVehicles().ToList();
            fleets = fDAL.GetAllFleets().ToList();
            drivers = dDAL.GetAllDrivers().ToList();

            var vehicleView = from v in vehicles
                              join f in fleets on v.FleetID equals f.FleetID
                              join d in drivers on v.DriverID equals d.DriverID
                              select new AllVehiclesViewModel { vehicle = v, driver = d, fleet = f };
            List<AllVehiclesViewModel> viewVehicles = vehicleView.ToList();
            return View(vehicleView.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.drivers = dDAL.GetAllDrivers();
            ViewBag.fleets = fDAL.GetAllFleets();
            return View();
        }

        // POST: VehicleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] Vehicle vehicleObj)
        {
            if (ModelState.IsValid)
            {
                vDAL.AddVehicle(vehicleObj);
                return RedirectToAction("Index");
            }
            return View(vehicleObj);
        }
    }
}
