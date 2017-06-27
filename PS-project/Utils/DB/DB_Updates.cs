using PS_project.Models;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_Updates
    {
        public static bool UpdateUserPassword(SqlConnection con, UserModel user)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.UPDATE_USER_PASSWORD;

                SqlParameter param_password = new SqlParameter("@password", System.Data.SqlDbType.Char, 64);
                param_password.Value = user.hashedpassword;
                cmd.Parameters.Add(param_password);

                SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar,100);
                param_email.Value = user.email;
                cmd.Parameters.Add(param_email);

                cmd.ExecuteReader();
                return true;
            }
        }

        public static bool UpdateServiceInfo(SqlConnection con, ServiceModel service)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.UPDATE_SERVICE_INFO;

                SqlParameter param_serv_name = new SqlParameter("@serv_name", System.Data.SqlDbType.VarChar, 150);
                param_serv_name.Value = service.name;
                cmd.Parameters.Add(param_serv_name);

                SqlParameter param_con_num = new SqlParameter("@c_num", System.Data.SqlDbType.Int);
                param_con_num.Value = service.contact_number;
                cmd.Parameters.Add(param_con_num);

                SqlParameter param_con_name = new SqlParameter("@c_name", System.Data.SqlDbType.VarChar, 100);
                param_con_name.Value = service.contact_name;
                cmd.Parameters.Add(param_con_name);

                SqlParameter param_con_loc = new SqlParameter("@c_loc", System.Data.SqlDbType.VarChar, 150);
                param_con_loc.Value = service.contact_location;
                cmd.Parameters.Add(param_con_loc);

                SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                param_serv_id.Value = service.id;
                cmd.Parameters.Add(param_serv_id);

                cmd.ExecuteReader();
                return true;
            }
        }

        public static bool UpdateUserInfo(SqlConnection con, ServiceUserModel user)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = DB_QueryStrings.UPDATE_USER_INFO;

                SqlParameter param_username = new SqlParameter("@user_name", System.Data.SqlDbType.VarChar, 100);
                param_username.Value = user.name;
                cmd.Parameters.Add(param_username);

                SqlParameter param_email = new SqlParameter("@user_email", System.Data.SqlDbType.VarChar, 100);
                param_email.Value = user.email;
                cmd.Parameters.Add(param_email);

                cmd.ExecuteReader();
                return true;
            }
        }
    }
}