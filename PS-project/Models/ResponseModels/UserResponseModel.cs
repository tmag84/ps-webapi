using WebApi.Hal;
using System.Collections.Generic;

namespace PS_project.Models
{
    public class UserResponseModel : Representation
    {
        public int curr_page { get; set; }
        public string user_email { get; set; }
        public int total_services { get; set; }
        public List<ServiceModel> services { get; set; }
    }
}