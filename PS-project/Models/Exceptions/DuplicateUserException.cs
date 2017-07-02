namespace PS_project.Models.Exceptions
{
    class DuplicateUserException : PS_Exception
    {
        private ErrorModel error;
        public DuplicateUserException(string msg) : base()
        {
            this.error = new ErrorModel
            {
                type = "api/prob/duplicate user",
                title = "Attempted to create duplicated item",
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