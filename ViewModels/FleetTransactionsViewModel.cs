using MyFleetManagerPro.Models;

namespace MyFleetManagerPro.ViewModels
{
    public class FleetTransactionsViewModel
    {
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Spent { get; set; }
        public double Poured { get; set; }
        public int OdometerReading { get; set; }
        public string Merchant { get; set; }
        public int VehicleID { get; set; }
        public string Registration_Number { get; set; } = string.Empty;
        public int FleetID { get; set; }
        public string FleetName { get; set; }

       
      
    }
}
