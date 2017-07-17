namespace PS_project.Models
{
    public class ProviderRegistrationModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string service_name { get; set; }
        public string service_description { get; set; }
        public int service_contact_number { get; set; }
        public string service_contact_name { get; set; }
        public string service_contact_location { get; set; }
        public int service_type { get; set; }
    }
}