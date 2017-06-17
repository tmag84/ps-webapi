using WebApi.Hal;

namespace PS_project.Models
{
    public class RegisterModel : Representation
    {
        public string provider_email { get; set; }
        public string provider_pass { get; set; }
        public string provider_name { get; set; }
        public int service_id { get; set; }        
        public string service_name { get; set; }
    }
}