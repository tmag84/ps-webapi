using System;

namespace PS_project.Models
{
    public class UserEventModel
    {
        public int service_id { get; set; }
        public int service_type { get; set; }
        public string service_name { get; set; }
        public string service_location { get; set; }
        public int id { get; set; }
        public string text { get; set; }
        public long creation_date { get; set; }
        public long event_begin { get; set; }
        public long event_end { get; set; }
    }
}