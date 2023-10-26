using Microsoft.AspNetCore.Mvc;
using MyFleetManagerPro.ViewModels;
using MyFleetManagerPro.Models;

namespace MyFleetManagerPro.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TransactionController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransactionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddTransactionViewModel transactionModel)
        {

            var filedetails = UploadedFile(transactionModel);
            string fileName = filedetails["FileName"];
            string filePath = filedetails["FilePath"];

            Transaction.CurrentImage = filePath;

            var ScanData = Transaction.ProcessTransaction(filePath);

            Transaction transact = new Transaction
            {
                VehicleID = transactionModel.VehicleID,
                OdometerReading = transactionModel.OdometerReading,

                // Get Data from receipt, return it in a method, input it here
                TransactionDate = Convert.ToDateTime(ScanData["TransactionDate"]),
                Spent = Convert.ToDouble(ScanData["Spent"]),
                Poured = Convert.ToDouble(ScanData["Quantity"]),
                Merchant = ScanData["MerchantName"]
            };

            Console.WriteLine($"{transact.Merchant}, {transact.Spent}, {transact.TransactionDate}, {transact.Poured}");
            // tDAL.AddTransaction(transact);
            return RedirectToAction(nameof(Index));


            //return View();

        }

        private Dictionary<string, string> UploadedFile(AddTransactionViewModel model)
        {
            Dictionary<string, string> filedetails = new Dictionary<string, string>();
            string uniqueFileName = string.Empty;
            string filePath = string.Empty;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Pictures");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            filedetails["FileName"] = uniqueFileName;
            filedetails["FilePath"] = filePath;
            return filedetails;

        }

    }
}
