﻿using WebApi.Hal;

namespace PS_project.Models
{
    public class EventModel : Representation
    {
        public int service_id { get; set; }
        public int id { get; set; }
        public string text { get; set; }        
        public long creation_date { get; set; }
        public long event_begin { get; set; }
        public long event_end { get; set; }
    }
}