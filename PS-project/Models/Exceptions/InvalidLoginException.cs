namespace PS_project.Models.Exceptions
{
    class InvalidLoginException : PS_Exception
    {
        private ErrorModel error;
        public InvalidLoginException(string msg) : base()
        {
            this.error = new ErrorModel
            {
                type = "api/prob/invalid_login",
                title = "Error in login information",
                detail = msg,
                status = System.Net.HttpStatusCode.Forbidden
            };
        }

        public override ErrorModel GetError()
        {
            return this.error;
        }
    }
}