using WebApi.Hal;

namespace PS_project.Models
{
    public class ProviderModel : Representation
    {
        public string email { get; set; }
        public string password { get; set; }        
    }
}