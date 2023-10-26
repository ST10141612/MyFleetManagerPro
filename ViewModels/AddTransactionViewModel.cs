namespace MyFleetManagerPro.ViewModels
{
    public class AddTransactionViewModel
    {
        public int VehicleID { get; set; }
        public int OdometerReading { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Photo { get; set; }
    }
}
