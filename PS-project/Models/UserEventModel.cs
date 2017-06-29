using System;

namespace PS_project.Models
{
    public class UserEventModel
    {
        public int service_id { get; set; }
        public int service_type { get; set; }
        public string service_name { get; set; }
        public int id { get; set; }
        public string text { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime event_date { get; set; }
    }
}