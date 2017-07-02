namespace PS_project.Models.Exceptions
{
    class InternalDBException : PS_Exception
    {
        private ErrorModel error;
        public InternalDBException(string msg) : base()
        {
            this.error = new ErrorModel
            {
                type = "api/prob/internal-db-error",
                title = "An error has occoured with the database",
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