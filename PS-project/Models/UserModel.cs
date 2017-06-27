using WebApi.Hal;

namespace PS_project.Models
{
    public class UserModel : Representation
    {
        public string email { get; set; }
        public string hashedpassword { get; set; }
        public string salt { get; set; }
    }
}