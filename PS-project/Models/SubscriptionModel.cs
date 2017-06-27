using WebApi.Hal;

namespace PS_project.Models
{
    public class SubscriptionModel : Representation
    {
        public SubscriptionModel(string email, int id)
        {
            this.email = email;
            this.id = id;
        }

        public string email { get; set; }
        public int id { get; set; }
    }
}