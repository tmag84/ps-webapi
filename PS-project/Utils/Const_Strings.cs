namespace PS_project.Utils
{
    public class Const_Strings
    {
        public const string USER_ROUTE_PREFIX = "api/user";
        public const string PROVIDER_ROUTE_PREFIX = "api/provider";

        public const string SORT_BY_NUMBER_SUBSCRIBERS = "sub";
        public const string SORT_BY_AVERAGE_RANKING = "rank";
        public const string SORT_BY_CREATION_DATE_ASC = "date";
        public const string SORT_BY_CREATION_DATE_DESC = "date_desc";

        public const int N_DAYS = 3;
        public const int TIME_DIFFERENCE = 60 * 60 * 24 * N_DAYS;
    }
}