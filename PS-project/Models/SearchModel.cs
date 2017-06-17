using System.Collections.Generic;

namespace PS_project.Models
{
    public class SearchModel
    {
        public string user_email { get; set; }
        public int service_type { get; set; }
        public List<int> list_types { get; set; }
    }
}