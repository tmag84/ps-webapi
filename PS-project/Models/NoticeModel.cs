using System;
using WebApi.Hal;

namespace PS_project.Models
{
    public class NoticeModel : Representation
    {
        public int service_id { get; set; }
        public int id { get; set; }
        public string text { get; set; }
        public DateTime creation_date { get; set; }
    }
}