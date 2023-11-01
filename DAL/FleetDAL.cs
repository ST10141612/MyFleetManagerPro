using System.Data;
using Microsoft.Data.SqlClient;
using MyFleetManagerPro.Models;

namespace MyFleetManagerPro.DAL
{
    public class FleetDAL
    {
        string connectionString = "Server=tcp:thefleetmanagerds.database.windows.net,1433;Initial Catalog=thefleetmanagerDB;Persist Security Info=False;User ID=tlotlo;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public IEnumerable<Fleet> GetAllFleets()
        {
            List<Fleet> fleetlist = new List<Fleet>();

            using SqlConnection con = new SqlConnection(connectionString);
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllFleet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Fleet f = new Fleet();
                    f.FleetID = Convert.ToInt32(dr["FleetID"]);
                    f.Manager = dr["Manager"].ToString();
                    f.FleetName = dr["FleetName"].ToString();

                    fleetlist.Add(f);
                }
                con.Close();
            }
            return fleetlist;
        }

        public void AddFleet(Fleet f)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertFleet", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Manager", f.Manager);
                cmd.Parameters.AddWithValue("@FleetName", f.FleetName);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public Fleet GetFleetById(int? fleetID)
        {
            Fleet f = new Fleet();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetFleetById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FleetID", fleetID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    f.FleetID = Convert.ToInt32(dr["FleetID"]);
                    f.Manager = dr["Manager"].ToString();
                    f.FleetName = dr["FleetName"].ToString();
                }
                con.Close();

            }
            return f;
        }
    }
}
