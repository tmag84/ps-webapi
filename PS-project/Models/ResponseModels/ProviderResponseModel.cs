using WebApi.Hal;
using System.Collections.Generic;

namespace PS_project.Models
{
    public class ProviderResponseModel : Representation
    {
        public ServiceModel service { get; set; }
    }
}