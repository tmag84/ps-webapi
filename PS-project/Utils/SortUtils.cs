using PS_project.Models;
using System.Linq;
using System.Collections.Generic;


namespace PS_project.Utils
{
    public static class SortUtils
    {
        public static IOrderedEnumerable<ServiceModel> sortServicesBy(this IEnumerable<ServiceModel> list, string sortOrder)
        {
            switch(sortOrder)
            {
                case Const_Strings.SORT_BY_AVERAGE_RANKING:
                    return list.OrderByDescending(s => s.avg_rank);
                case Const_Strings.SORT_BY_NUMBER_SUBSCRIBERS:
                    return list.OrderByDescending(s => s.n_subscribers);
                default: return list.OrderByDescending(s => s.n_subscribers);
            }
        }

        public static IOrderedEnumerable<UserEventModel> sortUserEventsBy(this IEnumerable<UserEventModel> list, string sortOrder)
        {
            switch(sortOrder)
            {
                case Const_Strings.SORT_BY_CREATION_DATE_ASC:
                    return list.OrderBy(ev => ev.event_date);
                case Const_Strings.SORT_BY_CREATION_DATE_DESC:
                    return list.OrderByDescending(ev => ev.event_date);
                default: return list.OrderByDescending(ev => ev.event_date);
            }
        }
    }
}