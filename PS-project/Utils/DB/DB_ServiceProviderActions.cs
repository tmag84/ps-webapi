using PS_project.Models;
using PS_project.Models.Exceptions;
using PS_project.Models.NotificationModel;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

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

        public static NoticeModel GetNotice(int service_id, int notice_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    return DB_Gets.GetNotice(con, service_id, notice_id);
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static int CreateNotice(string email, NoticeModel notice)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ServiceModel service = DB_Gets.GetServiceWithProviderEmail(con, email);
                    if (service.id != notice.service_id)
                    {
                        throw new InvalidServicePermissionException("User "+email+" doesn't have permission to handle service "+service.name+" with id "+service.id);
                    }
                    int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var id = DB_Inserts.InsertNotice(con, notice, unixTimestamp);

                    List<DeviceModel> devices = DB_Gets.GetServiceSubscribersRegistredDevices(con, notice.service_id);
                    if (devices==null || devices.Count==0)
                    {
                        return id;
                    }
                    devices.ForEach(d =>
                    {
                        var diff = unixTimestamp - d.last_used;
                        if (diff > Const_Strings.TIME_DIFFERENCE)
                        {
                            DB_Deletes.DeleteDevice(con, d);
                            d.last_used = -1;
                        }
                    });

                    devices.RemoveAll(d => d.last_used == -1);

                    PushObjectModel push = new PushObjectModel("Nova notícia do serviço "+service.name, notice.text, service.id, service.name);
                    FcmHandler.PushNotification(devices, push);                

                    return id;
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
                    if (service.id != notice.service_id)
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

        public static EventModel GetEvent(int service_id, int event_id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    return DB_Gets.GetEvent(con,service_id, event_id);
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static int CreateEvent(string email, EventModel ev)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    ServiceModel service = DB_Gets.GetServiceWithProviderEmail(con, email);
                    if (service.id != ev.service_id)
                    {
                        throw new InvalidServicePermissionException("User " + email + " doesn't have permission to handle service " + service.name + " with id " + service.id);
                    }
                    int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var id = DB_Inserts.InsertEvent(con, ev, unixTimestamp);

                    List<DeviceModel> devices = DB_Gets.GetServiceSubscribersRegistredDevices(con, ev.service_id);
                    if (devices == null || devices.Count == 0)
                    {
                        return id;
                    }
                    devices.ForEach(d =>
                    {
                        var diff = unixTimestamp - d.last_used;
                        if (diff > Const_Strings.TIME_DIFFERENCE)
                        {
                            DB_Deletes.DeleteDevice(con, d);
                            d.last_used = -1;
                        }
                    });

                    devices.RemoveAll(d => d.last_used == -1);
                    PushObjectModel push = new PushObjectModel("Nova evento do serviço " + service.name, "Evento "+ev.text+" para o dia "+ev.event_begin.ToString(), ev.service_id, service.name);
                    FcmHandler.PushNotification(devices, push);

                    return id;
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
                    if (service.id != ev.service_id)
                    {
                        throw new InvalidServicePermissionException("User " + email + " doesn't have permission to handle service " + service.name + " with id " + service.id);
                    }

                    DB_Deletes.DeleteEvent(con, ev);

                    List<DeviceModel> devices = DB_Gets.GetServiceSubscribersRegistredDevices(con, ev.service_id);
                    if (devices == null || devices.Count == 0)
                    {
                        return true;
                    }

                    int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    devices.ForEach(d =>
                    {
                        var diff = unixTimestamp - d.last_used;
                        if (diff > Const_Strings.TIME_DIFFERENCE)
                        {
                            DB_Deletes.DeleteDevice(con, d);
                            d.last_used = -1;
                        }
                    });

                    devices.RemoveAll(d => d.last_used == -1);
                    PushObjectModel push = new PushObjectModel("Evento do serviço " + service.name+" foi removido", "Evento " + ev.text + " para o dia " + ev.event_begin.ToString(), ev.service_id, service.name);
                    FcmHandler.PushNotification(devices, push);

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