using PS_project.Models;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PS_project.Utils.DB
{
    public class DB_Gets
    {
        public static UserModel GetUser(SqlConnection con, string email)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_USER;

                SqlParameter param_provider_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_provider_email.Value = email;
                cmd.Parameters.Add(param_provider_email);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;

                UserModel provider = new UserModel();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    provider = (UserModel)JsonConvert.DeserializeObject(json, typeof(UserModel));
                }
                return provider;
            }
        }

        public static ServiceProviderModel GetServiceProvider(SqlConnection con, string provider_email)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICE_PROVIDER;

                SqlParameter param_provider_email = new SqlParameter("@provider_email", System.Data.SqlDbType.VarChar, 100);
                param_provider_email.Value = provider_email;
                cmd.Parameters.Add(param_provider_email);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;

                ServiceProviderModel provider = new ServiceProviderModel();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    provider = (ServiceProviderModel)JsonConvert.DeserializeObject(json, typeof(ServiceProviderModel));
                }
                return provider;
            }
        }

        public static ServiceUserModel GetServiceUser(SqlConnection con, string user_email)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICE_USER;

                SqlParameter param_email = new SqlParameter("@user_email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = user_email;
                cmd.Parameters.Add(param_email);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;

                ServiceUserModel u = new ServiceUserModel();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    u = (ServiceUserModel)JsonConvert.DeserializeObject(json, typeof(ServiceUserModel));
                }
                return u;
            }
        }

        private static ServiceModel GetService(SqlConnection con, string query, SqlParameter param)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.Add(param);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;

                ServiceModel service = new ServiceModel();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    service = (ServiceModel)JsonConvert.DeserializeObject(json, typeof(ServiceModel));
                }
                return service;
            }
        }

        private static void FillServiceInformation(SqlConnection con, ServiceModel service)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            service.avg_rank = GetServiceAvgRanking(con, service.id);
            service.n_subscribers = GetTotatServiceSubscribers(con, service.id);

            service.service_events = DB_Gets.GetEvents(con, service.id)
                .OrderBy(ev => ev.event_begin)
                .ToList();

            service.service_notices = DB_Gets.GetNotices(con, service.id)
                .OrderByDescending(notice => notice.creation_date)
                .ToList();

            service.service_rankings = DB_Gets.GetRankings(con, service.id)
                .OrderByDescending(rank=>rank.value)
                .ToList();

            foreach (var rank in service.service_rankings)
            {
                ServiceUserModel user = DB_Gets.GetServiceUser(con, rank.user_email);
                rank.user_name = user.name;
            }
        }

        public static ServiceModel GetServiceWithProviderEmail(SqlConnection con, string provider_email)
        {
            string query = DB_QueryStrings.GET_SERVICE_WITH_EMAIL;
            SqlParameter param = new SqlParameter("@email", System.Data.SqlDbType.VarChar,100);
            param.Value = provider_email;

            ServiceModel service = GetService(con, query, param);
            FillServiceInformation(con, service);
            return service;
        }

        public static ServiceModel GetServiceWithServiceId(SqlConnection con, int service_id)
        {
            string query = DB_QueryStrings.GET_SERVICE_WITH_ID;
            SqlParameter param = new SqlParameter("@service_id", System.Data.SqlDbType.Int);
            param.Value = service_id;

            ServiceModel service = GetService(con, query, param);
            FillServiceInformation(con, service);
            return service;
        }
        
        private static int GetTotatServiceSubscribers(SqlConnection con, int service_id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_TOTAL_SERVICE_SUBSCRIBERS;

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = service_id;
                cmd.Parameters.Add(param_serv_id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return 0;

                dr.Read();
                if (dr.IsDBNull(0))
                {
                    return 0;
                }
                return dr.GetInt32(0);
            }
        }

        private static double GetServiceAvgRanking(SqlConnection con, int service_id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICE_AVERAGE_RANKING;

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = service_id;
                cmd.Parameters.Add(param_serv_id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return 0;

                dr.Read();
                if (dr.IsDBNull(0))
                {
                    return 0;
                }
                return dr.GetInt32(0);
            }
        }

        private static List<EventModel> GetEvents(SqlConnection con, int service_id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                cmd.CommandText = DB_QueryStrings.GET_SERVICE_EVENTS;

                SqlParameter param_service_id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 100);
                param_service_id.Value = service_id;
                cmd.Parameters.Add(param_service_id);

                SqlParameter param_now_date = new SqlParameter("@now_date", System.Data.SqlDbType.Int);
                param_now_date.Value = unixTimestamp;
                cmd.Parameters.Add(param_now_date);

                List<EventModel> list_events = new List<EventModel>();

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return list_events;

                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    list_events.Add((EventModel)JsonConvert.DeserializeObject(json, typeof(EventModel)));
                }
                return list_events;
            }
        }

        private static List<NoticeModel> GetNotices(SqlConnection con, int service_id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICE_NOTICES;

                SqlParameter param_service_id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 100);
                param_service_id.Value = service_id;
                cmd.Parameters.Add(param_service_id);

                SqlDataReader dr = cmd.ExecuteReader();

                List<NoticeModel> list_notices = new List<NoticeModel>();

                if (!dr.HasRows) return list_notices;                
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    list_notices.Add((NoticeModel)JsonConvert.DeserializeObject(json, typeof(NoticeModel)));
                }
                return list_notices;
            }
        }

        private static List<RankingModel> GetRankings(SqlConnection con, int service_id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICE_RANKINGS;

                SqlParameter param_service_id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 100);
                param_service_id.Value = service_id;
                cmd.Parameters.Add(param_service_id);

                SqlDataReader dr = cmd.ExecuteReader();
                List<RankingModel> list_rankings = new List<RankingModel>();

                if (!dr.HasRows) return list_rankings;
                
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    list_rankings.Add((RankingModel)JsonConvert.DeserializeObject(json, typeof(RankingModel)));
                }
                return list_rankings;
            }
        }

        public static List<ServiceTypeModel> GetServiceTypes(SqlConnection con)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICE_TYPES;
                SqlDataReader dr = cmd.ExecuteReader();

                List<ServiceTypeModel> list_service_types = new List<ServiceTypeModel>();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    list_service_types.Add((ServiceTypeModel)JsonConvert.DeserializeObject(json, typeof(ServiceTypeModel)));
                }
                return list_service_types;
            }
        } 

        public static List<ServiceModel> GetSubscribedServices(SqlConnection con, string user_email)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SUBSCRIBED_SERVICES;

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = user_email;
                cmd.Parameters.Add(param_email);

                SqlDataReader dr = cmd.ExecuteReader();

                List<ServiceModel> list_services = new List<ServiceModel>();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    ServiceModel service = (ServiceModel)JsonConvert.DeserializeObject(json, typeof(ServiceModel));
                    FillServiceInformation(con, service);
                    list_services.Add(service);
                }
                return list_services;
            }
        }

        public static List<ServiceModel> GetServicesByType(SqlConnection con, int service_type)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICES_BY_TYPE;

                SqlParameter param_type = new SqlParameter("@type", System.Data.SqlDbType.Int);
                param_type.Value = service_type;
                cmd.Parameters.Add(param_type);

                SqlDataReader dr = cmd.ExecuteReader();

                List<ServiceModel> list_services = new List<ServiceModel>();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    ServiceModel service = (ServiceModel)JsonConvert.DeserializeObject(json, typeof(ServiceModel));
                    FillServiceInformation(con, service);
                    list_services.Add(service);
                }
                return list_services;
            }
        }

        public static List<ServiceModel> GetServicesByTypes(SqlConnection con, List<int> list_types)
        {
            List<ServiceModel> list_services = new List<ServiceModel>();
            foreach (int service_type in list_types)
            {
                list_services.AddRange(GetServicesByType(con,service_type));
            }
            return list_services;            
        }

        public static List<DeviceModel> GetUserRegisteredDevices(SqlConnection con, string user_email)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_USER_REGISTRED_DEVICES;

                SqlParameter param_user_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_user_email.Value = user_email;
                cmd.Parameters.Add(param_user_email);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;

                List<DeviceModel> list_devices = new List<DeviceModel>();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    DeviceModel device = (DeviceModel)JsonConvert.DeserializeObject(json, typeof(DeviceModel));
                    list_devices.Add(device);
                }
                return list_devices;
            }
        }

        public static List<DeviceModel> GetServiceSubscribersRegistredDevices(SqlConnection con, int id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.GET_SERVICE_SUBSCRIBED_DEVICES;

                SqlParameter param_id = new SqlParameter("@id", System.Data.SqlDbType.Int);
                param_id.Value = id;
                cmd.Parameters.Add(param_id);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows) return null;

                List<DeviceModel> list_devices = new List<DeviceModel>();
                while (dr.Read())
                {
                    var data = new Dictionary<string, object>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        data.Add(dr.GetName(i), dr.IsDBNull(i) ? null : dr.GetValue(i));
                    }
                    string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    list_devices.Add((DeviceModel)JsonConvert.DeserializeObject(json, typeof(DeviceModel)));
                }
                return list_devices;
            }
        }
    }
}