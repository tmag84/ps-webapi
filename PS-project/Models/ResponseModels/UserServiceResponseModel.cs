using WebApi.Hal;

namespace PS_project.Models
{
    public class UserServiceResponseModel : Representation
    {
        public ServiceModel service { get; set; }
    }
}