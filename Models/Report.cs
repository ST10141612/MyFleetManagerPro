using Microsoft.Data.Analysis;
using MyFleetManagerPro.DAL;
using MyFleetManagerPro.ViewModels;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace MyFleetManagerPro.Models
{
    public class Report
    {
        public string RegNo { get; set; }
        public double Spent { get; set; }
        public double Poured { get; set; }
        public double AvgPrice { get; set; }
        public double travelled { get; set; }
        public double consumption { get; set; }

        public static List<Report> FleetReport(int fleetID)
        {
            var report = new List<Report>();

            TransactionDAL tDAL = new TransactionDAL();
            VehicleDAL vDAL = new VehicleDAL();
            FleetDAL fDAL = new FleetDAL();

            List<Vehicle> VehicleList = new List<Vehicle>();
            List<Transaction> TransactionList = new List<Transaction>();
            List<Fleet> FleetList = new List<Fleet>();

            List<double> spentList = new List<double>();
            List<double> pouredList = new List<double>();
            List<string> regNoList = new List<string>();
            List<double> priceList = new List<double>();
            List<DateOnly> dateList = new List<DateOnly>();
            List<double> odoList = new List<double>();

            TransactionList = tDAL.GetAllTranaction().ToList();
            VehicleList = vDAL.GetAllVehicles().ToList();
            FleetList = fDAL.GetAllFleets().ToList();


            var TransactionDetailTable = from v in VehicleList
                                         join t in TransactionList on v.VehicleID equals t.VehicleID
                                         join f in FleetList on v.FleetID equals f.FleetID
                                         select new FleetTransactionsViewModel
                                         {
                                             TransactionID = t.TransactionID,
                                             TransactionDate = t.TransactionDate,
                                             Spent = t.Spent,
                                             Poured = t.Poured,
                                             OdometerReading = t.OdometerReading,
                                             Merchant = t.Merchant,
                                             VehicleID = v.VehicleID,
                                             Registration_Number = v.Registration_Number,
                                             FleetID = f.FleetID,
                                             FleetName = f.FleetName
                                         };

            var fleetTransactions = from t in TransactionDetailTable
                                    where t.FleetID == fleetID
                                    select t;



            foreach (var ft in fleetTransactions)
            {
                regNoList.Add(ft.Registration_Number.ToString());
                spentList.Add(ft.Spent);
                pouredList.Add(ft.Poured);
                priceList.Add(ft.Spent / ft.Poured);
                dateList.Add(DateOnly.FromDateTime(ft.TransactionDate));
                odoList.Add(ft.OdometerReading);
            }

            StringDataFrameColumn RegNo = new StringDataFrameColumn("Registration Number", regNoList);
            PrimitiveDataFrameColumn<double> Spent = new PrimitiveDataFrameColumn<double>("Spent", spentList);
            PrimitiveDataFrameColumn<double> Poured = new PrimitiveDataFrameColumn<double>("Poured", pouredList);
            PrimitiveDataFrameColumn<double> Price = new PrimitiveDataFrameColumn<double>("Cost Per Litre", priceList);
            PrimitiveDataFrameColumn<double> Odometer = new PrimitiveDataFrameColumn<double>("Odometer Reading", odoList);
            PrimitiveDataFrameColumn<DateOnly> Date = new PrimitiveDataFrameColumn<DateOnly>("Transaction Date", dateList);

            DataFrame fleet_transaction_df = new DataFrame(RegNo, Spent, Poured, Price, Odometer, Date);

            foreach (string regNo in regNoList.Distinct())
            {
                
                double vehiclePoured = 0;
                double vehicleSpent = 0;
                List<double> vehiclePrices = new List<double>();
                List<double> vehicleOdoReadings = new List<double>();

                foreach (var transaction in fleet_transaction_df.Rows)
                {
                    if (transaction[0].ToString() == regNo)
                    {
                        vehicleSpent += Convert.ToDouble(transaction[1]);
                        vehiclePoured += Convert.ToDouble(transaction[2]);
                        vehiclePrices.Add(Convert.ToDouble(transaction[3]));
                        vehicleOdoReadings.Add(Convert.ToDouble(transaction[4]));

                    }
                }
                double avgFuelPrice = (vehiclePrices.Sum())/(vehiclePrices.Count);
                double vehicleTravelled = (vehicleOdoReadings.Max() - vehicleOdoReadings.Min());
                double vehicleConsumption = vehiclePoured/vehicleTravelled*100;

                Report reportObj = new Report {RegNo = regNo, Spent = vehicleSpent, Poured = vehiclePoured, AvgPrice = avgFuelPrice, travelled = vehicleTravelled, consumption = vehicleConsumption };

                report.Add(reportObj);

            }
            return report;

        }
       

        
    }
}
