namespace PS_project.Models.Exceptions
{
    class InvalidServicePermissionException : PS_Exception
    {
        private ErrorModel error;
        public InvalidServicePermissionException(string msg) : base()
        {
            this.error = new ErrorModel
            {
                type = "api/prob/invalid service permission",
                title = "No permission to handle service actions",
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