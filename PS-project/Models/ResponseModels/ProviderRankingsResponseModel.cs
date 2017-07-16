using WebApi.Hal;
using System.Collections.Generic;

namespace PS_project.Models.ResponseModels
{
    public class ProviderRankingsResponseModel : Representation
    {       
        public List<RankingModel> service_rankings { get; set; }
    }
}