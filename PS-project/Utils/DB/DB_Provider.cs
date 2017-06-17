using PS_project.Models;
using PS_project.Utils.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_Provider
    {
        public static bool ProviderLogin(ProviderModel provider)
        {            
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ProviderModel p = DB_Gets.GetProvider(con, provider.email);
                    if (p==null || !p.password.Equals(provider.password))
                    {
                        throw new InvalidLoginException("Invalid username/password");
                    }
                    return true;
                }
            }
            catch(SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }           
        }

        public static int RegisterProvider(ProviderRegistrationModel registration)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ProviderModel provider = DB_Gets.GetProvider(con, registration.email);
                    if (provider != null)
                    {
                        throw new DuplicateUserException("The email " + registration.email + " is already in use");
                    }
                    DB_Creates.CreateProvider(con, registration);
                    int service_id = DB_Creates.CreateService(con, registration);
                    if (service_id == -1)
                    {
                        throw new InternalDBException("An error occured with the database after a sucesseful insertion");
                    }
                    return service_id;
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
                    resp.list_service_types = DB_Gets.GetServiceTypes(con);
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
                    resp.list_service_types = DB_Gets.GetServiceTypes(con);
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
            catch(SqlException e)
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
                    DB_Creates.CreateNotice(con, notice);
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
                    DB_Creates.CreateEvent(con, ev);
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

        public static bool EditProviderPassword(ProviderModel provider)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    DB_Updates.UpdateProviderInfo(con,provider);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

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
    }
}