using Microsoft.Data.SqlClient;
using System.Data;
using MyFleetManagerPro.Models;
using System.Data;

namespace MyFleetManagerPro.DAL
{
        public class DriverDAL
        {
            string connectionString = "Server=tcp:thefleetmanagerds.database.windows.net,1433;Initial Catalog=thefleetmanagerDB;Persist Security Info=False;User ID=tlotlo;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            public IEnumerable<Driver> GetAllDrivers()
            {
                List<Driver> drivers = new List<Driver>();
                using SqlConnection con = new SqlConnection(connectionString);
                {
                    SqlCommand cmd = new SqlCommand("SP_GetAllDrivers");
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Driver d = new Driver();
                        d.DriverID = Convert.ToInt32(dr["DriverID"]);
                        d.DriverName = dr["DriverName"].ToString();

                        drivers.Add(d);
                    }
                    con.Close();
                }
                return drivers;
            }

            public void AddDriver(Driver d)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertDriver", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DriverName", d.DriverName);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            public Driver GetDriverById(int? driveID)
            {
                Driver d = new Driver();

                 using (SqlConnection con = new SqlConnection(connectionString))
                 {
                    SqlCommand cmd = new SqlCommand("SP_GetDriverByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DriverID", driveID);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                         d.DriverID = Convert.ToInt32(dr["DriverID"]);
                         d.DriverName = dr["DriverName"].ToString();
                    }
                    con.Close();
                 }

               return d;
              }

 
        }
}
    


