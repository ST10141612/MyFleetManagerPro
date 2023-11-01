using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFleetManagerPro.DAL;
using MyFleetManagerPro.Models;

namespace MyFleetManagerPro.Controllers
{
    public class DriverController : Controller
    {

        // GET: DriverController/Create
        DriverDAL dDAL = new DriverDAL();
        public ActionResult Create()
        {
            return View();
        }

        // POST: DriverController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Driver driverObj)
        {
            if (ModelState.IsValid)
            {
                dDAL.AddDriver(driverObj);
                return RedirectToAction("Index");
            }
            return View(driverObj);
        }

        public IActionResult Index ()
        {
            List<Driver> drivers = new List<Driver>();
            drivers = dDAL.GetAllDrivers().ToList();
            return View(drivers); 
        }  
    }
}
