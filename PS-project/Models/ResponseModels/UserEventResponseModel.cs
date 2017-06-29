using WebApi.Hal;
using System.Collections.Generic;

namespace PS_project.Models
{
    public class UserEventResponseModel : Representation
    {
        public int total_events { get; set; }
        public List<UserEventModel> events { get; set; }
    }
}