using WebApi.Hal;

namespace PS_project.Models
{
    public class ServiceUserModel : Representation
    {
        public string email { get; set; }
        public string name { get; set; }
    }
}