using System.Configuration;

namespace PS_project.Utils.DB
{
    public class DB_Config
    {
        //Get the connection string from App config file.
        public static string GetConnectionString()
        {
            string returnValue = null;

            ConnectionStringSettings settings =
            ConfigurationManager.ConnectionStrings["PS_project.Properties.Settings.connString"];

            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
    }
}