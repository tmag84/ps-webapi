using WebApi.Hal;

namespace PS_project.Models
{
    public class UserRegistrationModel : Representation
    {
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string device_id { get; set; }
    }
}