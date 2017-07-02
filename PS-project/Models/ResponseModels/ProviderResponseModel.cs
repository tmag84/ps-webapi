using WebApi.Hal;

namespace PS_project.Models
{
    public class ProviderResponseModel : Representation
    {
        public int curr_page { get; set; }
        public ServiceModel service { get; set; }
    }
}