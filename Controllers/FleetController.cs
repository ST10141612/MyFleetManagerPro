using Microsoft.AspNetCore.Mvc;
using MyFleetManagerPro.DAL;
using MyFleetManagerPro.Models;

namespace MyFleetManagerPro.Controllers
{
    public class FleetController : Controller
    {
        FleetDAL fDAL = new FleetDAL();
        public IActionResult Index()
        {
            List<Fleet> fleetList = new List<Fleet>();
            fleetList = fDAL.GetAllFleets().ToList();
            return View(fleetList);
        }

        // GET: FleetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FleetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind] Fleet fleetObj)
        {
            if (ModelState.IsValid)
            {
                fDAL.AddFleet(fleetObj);
                return RedirectToAction("Index");
            }
            return View(fleetObj);
        }

    }
}
