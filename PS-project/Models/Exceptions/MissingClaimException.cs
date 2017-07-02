namespace PS_project.Models.Exceptions
{
    class MissingClaimException : PS_Exception
    {
        private ErrorModel error;
        public MissingClaimException(string msg) : base()
        {
            this.error = new ErrorModel
            {
                type = "api/prob/claim missing",
                title = "Essential claim property missing from auth token",
                detail = msg,
                status = System.Net.HttpStatusCode.Unauthorized
            };
        }

        public override ErrorModel GetError()
        {
            return this.error;
        }
    }
}