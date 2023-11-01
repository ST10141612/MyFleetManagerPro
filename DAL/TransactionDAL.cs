using Microsoft.Data.SqlClient;
using MyFleetManagerPro.Models;
using System.Data;

namespace MyFleetManagerPro.DAL
{
    public class TransactionDAL
    {
        string connectionString = "Server=tcp:thefleetmanagerds.database.windows.net,1433;Initial Catalog=thefleetmanagerDB;Persist Security Info=False;User ID=tlotlo;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public IEnumerable<Transaction> GetAllTranaction()
        {
            List<Transaction> transactionlist = new List<Transaction>();

            using SqlConnection con = new SqlConnection(connectionString);
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Transaction t = new Transaction();
                    t.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                    t.TransactionID = Convert.ToInt32(dr["TransactionID"]);
                    t.VehicleID = Convert.ToInt32(dr["VehicleID"]);
                    t.Spent = Convert.ToDouble(dr["Spent"]);
                    t.Poured = Convert.ToDouble(dr["Poured"]);
                    t.OdometerReading = Convert.ToInt32(dr["OdometerReading"]);
                    t.Merchant = dr["Merchant"].ToString();

                    transactionlist.Add(t);
                }
                con.Close();
            }
            return transactionlist;
        }

        public void AddTransaction(Transaction t)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertTransaction", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionDate", t.TransactionDate);
                cmd.Parameters.AddWithValue("@VehicleID", t.VehicleID);
                cmd.Parameters.AddWithValue("@Spent", t.Spent);
                cmd.Parameters.AddWithValue("@Poured", t.Poured);
                cmd.Parameters.AddWithValue("@OdometerReading", t.OdometerReading);
                cmd.Parameters.AddWithValue("@Merchant", t.Merchant);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public Transaction GetTransactionById(int? transactionID)
        {
            Transaction t = new Transaction();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetTransactionByID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionID", transactionID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    t.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                    t.TransactionID = Convert.ToInt32(dr["TransactionID"]);
                    t.VehicleID = Convert.ToInt32(dr["VehicleID"]);
                    t.Spent = Convert.ToDouble(dr["Spent"]);
                    t.Poured = Convert.ToDouble(dr["Poured"]);
                    t.OdometerReading = Convert.ToInt32(dr["OdometerReading"]);
                    t.Merchant = dr["Merchant"].ToString();
                }
                con.Close();

            }
            return t;
        }
    }
}
