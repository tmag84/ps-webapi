using PS_project.Models;
using PS_project.Utils.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_User
    {
        public static bool UserLogin(string email, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    UsersModel u = DB_Gets.GetUser(con, email);
                    if (u == null || !u.password.Equals(password))
                    {
                        throw new InvalidLoginException("Invalid username/password");
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool RegisterUser(UserRegistrationModel registration)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    UsersModel u = DB_Gets.GetUser(con, registration.email);
                    if (u==null)
                    {
                        DB_Creates.CreateUser(con, registration);

                        return true;
                    }


                    if (u!=null)
                    {
                        return true;
                    }
                    throw new DuplicateUserException("User "+ registration.email+" already exists.");
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static UsersModel GetUser(string user_email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    return DB_Gets.GetUser(con,user_email);
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool EditUser(UsersModel user)
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
                    resp.list_service_types = DB_Gets.GetServiceTypes(con);
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
                    DB_Creates.CreateRanking(con, ranking);
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
                    DB_Creates.CreateSubscription(con, email,id);
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
                    DB_Deletes.DeleteSubscription(con, email,id);
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