using WebApi.Hal;

namespace PS_project.Models
{
    public class SubscriptionModel : Representation
    {
        public string email { get; set; }
        public int id { get; set; }
    }
}