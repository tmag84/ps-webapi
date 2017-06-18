using WebApi.Hal;
using System.Collections.Generic;

namespace PS_project.Models
{
    public class UserResponseModel : Representation
    {
        public string user_email { get; set; }
        public List<ServiceModel> services { get; set; }
        public List<ServiceTypeModel> list_service_types { get; set; }
    }
}