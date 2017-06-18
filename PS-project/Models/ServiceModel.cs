using WebApi.Hal;
using System.Collections.Generic;

namespace PS_project.Models
{
    public class ServiceModel : Representation
    {
        public int id { get; set; }
        public string provider_email { get; set; }
        public string name { get; set; }
        public int contact_number { get; set; }
        public string contact_name { get; set; }
        public string contact_location { get; set; }
        public int service_type { get; set; }
        public int n_subscribers { get; set; }
        public double avg_rank { get; set; }

        public bool subscribed = false;

        public List<EventModel> service_events { get; set; }
        public List<NoticeModel> service_notices { get; set; }
        public List<RankingModel> service_rankings { get; set; }
    }
}