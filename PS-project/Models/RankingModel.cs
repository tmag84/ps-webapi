using System;

namespace PS_project.Models
{
    public class RankingModel
    {
        public string user_email { get; set; }
        public int service_id { get; set; }
        public int value { get; set; }
        public string text { get; set; }
        public DateTime creation_date { get; set; }
    }
}