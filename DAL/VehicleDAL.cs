using System.Data;
using Microsoft.Data.SqlClient;
using MyFleetManagerPro.Models;
namespace MyFleetManagerPro.DAL
{
    public class VehicleDAL
    {
        string connectionString = "Server=tcp:thefleetmanagerds.database.windows.net,1433;Initial Catalog=thefleetmanagerDB;Persist Security Info=False;User ID=tlotlo;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehiclelist = new List<Vehicle>();

            using SqlConnection con = new SqlConnection(connectionString);
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllVehicle", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Vehicle v = new Vehicle();
                    v.VehicleID = Convert.ToInt32(dr["VehicleID"]);
                    v.Registration_Number = dr["Registration_Number"].ToString();
                    v.DriverID = Convert.ToInt32(dr["DriverID"]);
                    v.FleetID = Convert.ToInt32(dr["FleetID"]);

                    vehiclelist.Add(v);
                }
                con.Close();
            }
            return vehiclelist;
        }

        public void AddVehicle(Vehicle v)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertVehicle", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Registration_Number", v.Registration_Number);
                cmd.Parameters.AddWithValue("@DriverID", v.DriverID);
                cmd.Parameters.AddWithValue("@FleetID", v.FleetID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public Vehicle GetVehicleById(int? vehicleID)
        {
            Vehicle v = new Vehicle();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetVehicleByID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@VehicleID", vehicleID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    v.VehicleID = Convert.ToInt32(dr["VehicleID"]);
                    v.Registration_Number = dr["Registration_Number"].ToString();
                    v.DriverID = Convert.ToInt32(dr["DriverID"]);
                    v.FleetID = Convert.ToInt32(dr["FleetID"]);
                }
                con.Close();

            }
            return v;
        }
    }
}
