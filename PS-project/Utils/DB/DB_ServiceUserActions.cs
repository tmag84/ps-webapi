using PS_project.Models;
using PS_project.Models.Exceptions;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

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

        public static ServiceUserModel GetServiceUser(string user_email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    return DB_Gets.GetServiceUser(con, user_email);
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
                    DB_Deletes.DeleteRanking(con, ranking.user_email, ranking.service_id);
                    int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    DB_Inserts.InsertRanking(con, ranking, unixTimestamp);
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

                    List<ServiceModel> list = DB_Gets.GetSubscribedServices(con, email);
                    foreach(var service in list)
                    {
                        if (service.id==id)
                        {
                            return true;
                        }
                    }

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

                    List<ServiceModel> list = DB_Gets.GetSubscribedServices(con, email);
                    foreach (var service in list)
                    {
                        if (service.id == id)
                        {
                            return true;
                        }
                    }

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

                    List<DeviceModel> devices = DB_Gets.GetUserRegisteredDevices(con, email);
                    if (devices==null || !devices.Exists(d=>d.device_id==device_id))
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