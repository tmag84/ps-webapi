using PS_project.Models;
using PS_project.Models.Exceptions;
using PS_project.Models.NotificationModel;
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

        public static bool CreateNotice(string email, NoticeModel notice)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ServiceModel service = DB_Gets.GetServiceWithProviderEmail(con, email);
                    if (service.id != notice.id)
                    {
                        throw new InvalidServicePermissionException("User "+email+" doesn't have permission to handle service "+service.name+" with id "+service.id);
                    }
                    DB_Inserts.InsertNotice(con, notice);

                    List<string> devices = DB_Gets.GetSubscriberRegistredDevices(con, notice.id);
                    PushObjectModel push = new PushObjectModel("Nova notícia do serviço "+service.name, notice.text, notice.id);
                    FcmHandler.PushNotification(devices, push);                

                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool DeleteNotice(string email, NoticeModel notice)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ServiceModel service = DB_Gets.GetServiceWithProviderEmail(con, email);
                    if (service.id != notice.id)
                    {
                        throw new InvalidServicePermissionException("User " + email + " doesn't have permission to handle service " + service.name + " with id " + service.id);
                    }

                    DB_Deletes.DeleteNotice(con, notice);
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool CreateEvent(string email, EventModel ev)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ServiceModel service = DB_Gets.GetServiceWithProviderEmail(con, email);
                    if (service.id != ev.id)
                    {
                        throw new InvalidServicePermissionException("User " + email + " doesn't have permission to handle service " + service.name + " with id " + service.id);
                    }

                    DB_Inserts.InsertEvent(con, ev);

                    List<string> devices = DB_Gets.GetSubscriberRegistredDevices(con, ev.id);
                    PushObjectModel push = new PushObjectModel("Nova evento do serviço " + service.name, "Evento "+ev.text+" para o dia "+ev.event_date.ToString(), ev.id);
                    FcmHandler.PushNotification(devices, push);

                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool DeleteEvent(string email, EventModel ev)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ServiceModel service = DB_Gets.GetServiceWithProviderEmail(con, email);
                    if (service.id != ev.id)
                    {
                        throw new InvalidServicePermissionException("User " + email + " doesn't have permission to handle service " + service.name + " with id " + service.id);
                    }

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