using PS_project.Models;
using PS_project.Utils.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_ServiceUserActions
    {
        public static bool EditServiceUser(ServiceUserModel user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    return DB_Updates.UpdateUserInfo(con, user);
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static UserResponseModel GetSubscribedServices(string user_email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    UserResponseModel resp = new UserResponseModel();
                    resp.services = DB_Gets.GetSubscribedServices(con, user_email);
                    resp.list_service_types = DB_Gets.GetServiceTypes(con);
                    return resp;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static List<ServiceModel> GetServicesByTypes(List<int> list_types)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    return DB_Gets.GetServicesByTypes(con, list_types);
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool CreateRanking(RankingModel ranking)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Inserts.InsertRanking(con, ranking);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool AddSubscription(string email, int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Inserts.InsertSubscription(con, email, id);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool RemoveSubscription(string email, int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Deletes.DeleteSubscription(con, email, id);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool RegisterDevice(string email, string device_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    List<string> devices = DB_Gets.GetUserRegisteredDevices(con, email);
                    if (!devices.Contains(device_id))
                    {
                        DB_Inserts.InsertDeviceId(con, email, device_id);
                    }

                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }
    }
}