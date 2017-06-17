using PS_project.Models;
using PS_project.Utils.Exceptions;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_Deletes
    {
        public static bool DeleteNotice(SqlConnection con, NoticeModel notice)
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "delete from Notice where service_id=@serv_id and id=@notice_id";

                    SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                    param_serv_id.Value = notice.service_id;
                    cmd.Parameters.Add(param_serv_id);

                    SqlParameter param_notice_id = new SqlParameter("@notice_id", System.Data.SqlDbType.Int);
                    param_notice_id.Value = notice.id;
                    cmd.Parameters.Add(param_notice_id);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool DeleteEvent(SqlConnection con, EventModel ev)
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "delete from Event where id=@event_id";

                    SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                    param_serv_id.Value = ev.service_id;
                    cmd.Parameters.Add(param_serv_id);

                    SqlParameter param_event_id = new SqlParameter("@event_id", System.Data.SqlDbType.Int);
                    param_event_id.Value = ev.id;
                    cmd.Parameters.Add(param_event_id);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool DeleteSubscription(SqlConnection con, SubscriptionModel sub)
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "delete from Subscriber where service_id=@serv_id and user_email=@email";

                    SqlParameter param_serv_id = new SqlParameter("@serv_id", System.Data.SqlDbType.Int);
                    param_serv_id.Value = sub.service_id;
                    cmd.Parameters.Add(param_serv_id);

                    SqlParameter param_email = new SqlParameter("@email", System.Data.SqlDbType.VarChar, 100);
                    param_email.Value = sub.user_email;
                    cmd.Parameters.Add(param_email);                    

                    cmd.ExecuteReader();
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