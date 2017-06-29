using PS_project.Models;
using PS_project.Utils.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_ServiceProviderActions
    {
        public static bool EditServiceInfo(ServiceModel service)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Updates.UpdateServiceInfo(con, service);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static ProviderResponseModel GetServiceWithServiceId(int service_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    ProviderResponseModel resp = new ProviderResponseModel();
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    resp.service = DB_Gets.GetServiceWithServiceId(con, service_id);
                    return resp;
                }
            }
            catch (InternalDBException e)
            {
                throw e;
            }
        }

        public static ProviderResponseModel GetServiceWithProviderEmail(string provider_email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    ProviderResponseModel resp = new ProviderResponseModel();
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    resp.service = DB_Gets.GetServiceWithProviderEmail(con, provider_email);
                    return resp;
                }
            }
            catch (InternalDBException e)
            {
                throw e;
            }
        }

        public static List<ServiceTypeModel> GetServiceTypes()
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    return DB_Gets.GetServiceTypes(con);
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool CreateNotice(NoticeModel notice)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Inserts.InsertNotice(con, notice);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool DeleteNotice(NoticeModel notice)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Deletes.DeleteNotice(con, notice);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool CreateEvent(EventModel ev)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Inserts.InsertEvent(con, ev);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool DeleteEvent(EventModel ev)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Deletes.DeleteEvent(con, ev);
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