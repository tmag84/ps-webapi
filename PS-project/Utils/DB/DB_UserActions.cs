using PS_project.Models;
using PS_project.Models.Exceptions;
using System.Data.SqlClient;

namespace PS_project.Utils.DB
{
    public class DB_UserActions
    {
        public static bool LogUser(string email, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();
                    UserModel user = DB_Gets.GetUser(con, email);
                    if (user == null || !Hashing.ComparePasswords(password,user))
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

                    UserModel user = DB_Gets.GetUser(con, registration.email);                    
                    if (user==null)
                    {                        
                        string salt = Hashing.GetSalt();
                        string hashed_password = Hashing.GetHashed(registration.password, salt);
                        DB_Inserts.InsertUser(con, registration.email, hashed_password, salt);
                        DB_Inserts.InsertServiceUser(con, registration);
                        return true;
                    }
                    throw new DuplicateUserException("User " + registration.email + " already exists.");
                }
            }
            catch (SqlException e)
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

                    ServiceProviderModel provider = DB_Gets.GetServiceProvider(con, registration.email);
                    if (provider == null)
                    {
                        string salt = Hashing.GetSalt();
                        string hashed_password = Hashing.GetHashed(registration.password, salt);
                        DB_Inserts.InsertUser(con, registration.email, hashed_password, salt);
                        DB_Inserts.InsertServiceProvider(con, registration);
                        int service_id = DB_Inserts.InsertService(con, registration);
                        if (service_id == -1)
                        {
                            throw new InternalDBException("An error occured with the database after a sucesseful insertion");
                        }
                        return service_id;
                    }
                    throw new DuplicateUserException("User " + registration.email + " already exists.");                    
                }
            }
            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }

        public static bool EditUserPassword(string email, string new_password)
        {
            try
            {
                using(SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = DB_Config.GetConnectionString();
                    con.Open();

                    UserModel u = DB_Gets.GetUser(con, email);

                    UserModel user = new UserModel();
                    user.email = email;
                    user.salt = u.salt;
                    user.hashedpassword = Hashing.GetHashed(new_password, u.salt);
                    return DB_Updates.UpdateUserPassword(con, user);
                }
            }

            catch (SqlException e)
            {
                throw new InternalDBException(e.ToString());
            }
        }   
    }
}