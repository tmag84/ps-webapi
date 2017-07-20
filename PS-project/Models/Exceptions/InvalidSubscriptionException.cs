using System;
using System.Collections.Generic;
using System.Linq;
namespace PS_project.Models.Exceptions
{
    class InvalidSubscriptionException : PS_Exception
    {
        private ErrorModel error;
        public InvalidSubscriptionException(string msg) : base()
        {
            this.error = new ErrorModel
            {
                type = "api/prob/subscription action",
                title = "Error attempting to perform subscription action",
                detail = msg,
                status = System.Net.HttpStatusCode.InternalServerError
            };
        }

        public override ErrorModel GetError()
        {
            return this.error;
        }
    }
}