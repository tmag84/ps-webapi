using WebApi.Hal;


namespace PS_project.Models
{
    public class ServiceTypeModel : Representation
    {
        public int id_type { get; set; }
        public string name { get; set; }
    }
}