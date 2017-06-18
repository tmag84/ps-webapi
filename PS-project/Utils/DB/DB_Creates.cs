using PS_project.Models;
using PS_project.Utils.Exceptions;
using System;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_Creates
    {
        public static bool CreateProvider(SqlConnection con, ProviderRegistrationModel registration)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into Provider values(@email,@password);";

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = registration.email;
                cmd.Parameters.Add(param_email);

                SqlParameter param_password = new SqlParameter("@password", System.Data.SqlDbType.VarChar, 20);
                param_password.Value = registration.password;
                cmd.Parameters.Add(param_password);

                SqlDataReader dr = cmd.ExecuteReader();
                return true;
            }
        }

        public static bool CreateUser(SqlConnection con, UserRegistrationModel registration)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into Users values(@email,@password,@name);";

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = registration.email;
                cmd.Parameters.Add(param_email);

                SqlParameter param_password = new SqlParameter("@password", System.Data.SqlDbType.VarChar, 20);
                param_password.Value = registration.password;
                cmd.Parameters.Add(param_password);

                SqlParameter param_name = new SqlParameter("@name", System.Data.SqlDbType.VarChar, 100);
                param_name.Value = registration.name;
                cmd.Parameters.Add(param_name);

                SqlDataReader dr = cmd.ExecuteReader();
                return true;
            }
        }

        public static bool CreateDeviceId(SqlConnection con, UserRegistrationModel registration)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into DeviceRegistration values(@device_id,@email);";

                SqlParameter param_device_id = new SqlParameter("@device_id", System.Data.SqlDbType.VarChar, 100);
                param_device_id.Value = registration.device_id;
                cmd.Parameters.Add(param_device_id);

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = registration.email;
                cmd.Parameters.Add(param_email);                

                SqlDataReader dr = cmd.ExecuteReader();
                return true;
            }
        }

        public static int CreateService(SqlConnection con, ProviderRegistrationModel registration)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into Service(provider_email,name,contact_number,contact_name,contact_location,service_type) values(@provider_email,@name,@contact_number,@contact_name,@contact_location,@service_type); SELECT SCOPE_IDENTITY()";

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

        public static int CreateNotice(SqlConnection con, NoticeModel notice)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into Notice(service_id,text,creation_date) values(@serv_id,@notice_text,GETUTCDATE()); SELECT SCOPE_IDENTITY()";

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = notice.service_id;
                cmd.Parameters.Add(param_serv_id);

                SqlParameter param_text = new SqlParameter("@notice_text", System.Data.SqlDbType.VarChar, 150);
                param_text.Value = notice.text;
                cmd.Parameters.Add(param_text);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int CreateEvent(SqlConnection con, EventModel ev) {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into Event(service_id,text,creation_date,event_date) values(@serv_id,@event_text,GETUTCDATE(),@event_date); SELECT SCOPE_IDENTITY()";

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = ev.service_id;
                cmd.Parameters.Add(param_serv_id);

                SqlParameter param_text = new SqlParameter("@event_text", System.Data.SqlDbType.VarChar, 150);
                param_text.Value = ev.text;
                cmd.Parameters.Add(param_text);

                SqlParameter param_event_date = new SqlParameter("@event_date", System.Data.SqlDbType.DateTime);
                param_event_date.Value = ev.event_date;
                cmd.Parameters.Add(param_event_date);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static bool CreateRanking(SqlConnection con, RankingModel rank)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into Ranking(user_email,service_id,value,text,creation_date) values(@user_email,@serv_id,@value,@text,GETUTCDATE());";

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

                cmd.ExecuteReader();
                return true;
            }
        }

        public static bool CreateSubscription(SqlConnection con, string email, int id)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "insert into Subscriber(user_email,service_id) values(@user_email,@serv_id)";

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