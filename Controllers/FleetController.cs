using Microsoft.AspNetCore.Mvc;

namespace MyFleetManagerPro.Controllers
{
    public class FleetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: FleetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FleetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
