namespace PS_project.Models.NotificationModel
{
    public class PushObjectModel
    {
        public PushObjectModel(string title, string body, int service_id, string service_name)
        {
            this.title = title;
            this.body = body;            
            this.service_id = service_id;
            this.service_name = service_name;
        }

        public string title { get; set; }
        public string body { get; set; }
        public int service_id { get; set; }
        public string service_name { get; set; }
    }
}