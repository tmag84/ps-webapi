using PS_project.Models;
using System;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_Inserts
    {
        public static bool InsertUser(SqlConnection con, string email, string hashed_password, string salt)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_USER;

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = email;
                cmd.Parameters.Add(param_email);

                SqlParameter param_password = new SqlParameter("@password", System.Data.SqlDbType.Char, 64);
                param_password.Value = hashed_password;
                cmd.Parameters.Add(param_password);

                SqlParameter param_salt = new SqlParameter("@salt", System.Data.SqlDbType.Char, 8);
                param_salt.Value = salt;
                cmd.Parameters.Add(param_salt);

                SqlDataReader dr = cmd.ExecuteReader();
                return true;
            }
        }
                 
        public static bool InsertServiceProvider(SqlConnection con, ProviderRegistrationModel registration)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_SERVICE_PROVIDER;

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = registration.email;
                cmd.Parameters.Add(param_email);
                
                SqlDataReader dr = cmd.ExecuteReader();
                return true;
            }
        }

        public static bool InsertServiceUser(SqlConnection con, UserRegistrationModel registration)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_SERVICE_USER;

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = registration.email;
                cmd.Parameters.Add(param_email);

                SqlParameter param_name = new SqlParameter("@name", System.Data.SqlDbType.VarChar, 100);
                param_name.Value = registration.name;
                cmd.Parameters.Add(param_name);

                SqlDataReader dr = cmd.ExecuteReader();
                return true;
            }
        }

        public static bool InsertDeviceId(SqlConnection con, string email, string device_id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                int unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                cmd.CommandText = DB_QueryStrings.INSERT_DEVICE_REGISTRATION;

                SqlParameter param_device_id = new SqlParameter("@device_id", System.Data.SqlDbType.VarChar, 300);
                param_device_id.Value = device_id;
                cmd.Parameters.Add(param_device_id);

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = email;
                cmd.Parameters.Add(param_email);

                SqlParameter param_date = new SqlParameter("@now_date", System.Data.SqlDbType.Int);
                param_date.Value = unixTimestamp;
                cmd.Parameters.Add(param_date);

                SqlDataReader dr = cmd.ExecuteReader();
                return true;
            }
        }

        public static int InsertService(SqlConnection con, ProviderRegistrationModel registration)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_SERVICE + ";" + DB_QueryStrings.SELECT_SCOPE_IDENTITY;

                SqlParameter param_email = new SqlParameter("@provider_email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = registration.email;
                cmd.Parameters.Add(param_email);

                SqlParameter param_name = new SqlParameter("@name", System.Data.SqlDbType.VarChar, 150);
                param_name.Value = registration.name;
                cmd.Parameters.Add(param_name);

                SqlParameter param_cont_number = new SqlParameter("@contact_number", System.Data.SqlDbType.Int);
                param_cont_number.Value = registration.contact_number == 0 ? 0 : registration.contact_number;
                cmd.Parameters.Add(param_cont_number);

                SqlParameter param_cont_name = new SqlParameter("@contact_name", System.Data.SqlDbType.VarChar, 100);
                param_cont_name.Value = registration.contact_name == null ?  "" : registration.contact_name;
                cmd.Parameters.Add(param_cont_name);

                SqlParameter param_cont_loc = new SqlParameter("@contact_location", System.Data.SqlDbType.VarChar, 150);
                param_cont_loc.Value = registration.contact_location == null ? "" : registration.contact_location;
                cmd.Parameters.Add(param_cont_loc);

                SqlParameter param_serv_type = new SqlParameter("@service_type", System.Data.SqlDbType.Int);
                param_serv_type.Value = registration.serv_type;
                cmd.Parameters.Add(param_serv_type);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int InsertNotice(SqlConnection con, NoticeModel notice, int unixTimestamp)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_NOTICE + ";" + DB_QueryStrings.SELECT_SCOPE_IDENTITY;

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = notice.service_id;
                cmd.Parameters.Add(param_serv_id);

                SqlParameter param_text = new SqlParameter("@notice_text", System.Data.SqlDbType.VarChar, 150);
                param_text.Value = notice.text;
                cmd.Parameters.Add(param_text);

                SqlParameter param_now_date = new SqlParameter("@now_date", System.Data.SqlDbType.Int);
                param_now_date.Value = unixTimestamp;
                cmd.Parameters.Add(param_now_date);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int InsertEvent(SqlConnection con, EventModel ev, int unixTimestamp) {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_EVENT + ";" + DB_QueryStrings.SELECT_SCOPE_IDENTITY;

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = ev.service_id;
                cmd.Parameters.Add(param_serv_id);

                SqlParameter param_text = new SqlParameter("@event_text", System.Data.SqlDbType.VarChar, 150);
                param_text.Value = ev.text;
                cmd.Parameters.Add(param_text);

                SqlParameter param_now_date = new SqlParameter("@now_date", System.Data.SqlDbType.Int);
                param_now_date.Value = unixTimestamp;
                cmd.Parameters.Add(param_now_date);

                SqlParameter param_event_begin = new SqlParameter("@event_begin", System.Data.SqlDbType.Int);
                param_event_begin.Value = ev.event_begin;
                cmd.Parameters.Add(param_event_begin);

                SqlParameter param_event_end = new SqlParameter("@event_end", System.Data.SqlDbType.Int);
                param_event_end.Value = ev.event_end;
                cmd.Parameters.Add(param_event_end);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static bool InsertRanking(SqlConnection con, RankingModel rank, int unixTimestamp)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_RANKING;

                SqlParameter param_user_email = new SqlParameter("@user_email", System.Data.SqlDbType.VarChar, 100);
                param_user_email.Value = rank.user_email;
                cmd.Parameters.Add(param_user_email);

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = rank.service_id;
                cmd.Parameters.Add(param_serv_id);

                SqlParameter param_value = new SqlParameter("@value", System.Data.SqlDbType.Int);
                param_value.Value = rank.value;
                cmd.Parameters.Add(param_value);

                SqlParameter param_text = new SqlParameter("@text", System.Data.SqlDbType.VarChar, 150);
                param_text.Value = rank.text;
                cmd.Parameters.Add(param_text);

                SqlParameter param_now_date = new SqlParameter("@now_date", System.Data.SqlDbType.Int);
                param_now_date.Value = unixTimestamp;
                cmd.Parameters.Add(param_now_date);

                cmd.ExecuteReader();
                return true;
            }
        }

        public static bool InsertSubscription(SqlConnection con, string email, int id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.INSERT_SUBSCRIPTION;

                SqlParameter param_user_email = new SqlParameter("@user_email", System.Data.SqlDbType.VarChar, 100);
                param_user_email.Value = email;
                cmd.Parameters.Add(param_user_email);

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = id;
                cmd.Parameters.Add(param_serv_id);

                cmd.ExecuteReader();
                return true;
            }
        }
    }
}