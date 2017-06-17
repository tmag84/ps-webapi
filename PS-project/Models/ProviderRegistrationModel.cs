using WebApi.Hal;

namespace PS_project.Models
{
    public class ProviderRegistrationModel : Representation
    {
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public int contact_number { get; set; }
        public string contact_name { get; set; }
        public string contact_location { get; set; }
        public int serv_type { get; set; }
    }
}