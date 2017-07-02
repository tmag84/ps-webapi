using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PS_project.Models.NotificationModel
{
    public class PushObjectModel
    {
        public PushObjectModel(string title, string body, int service_id)
        {
            this.title = title;
            this.body = body;            
            this.service_id = service_id;
        }

        public string title { get; set; }
        public string body { get; set; }
        public int service_id { get; set; }
    }
}