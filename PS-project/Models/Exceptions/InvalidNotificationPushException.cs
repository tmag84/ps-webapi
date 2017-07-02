using System;
using System.Collections.Generic;
using System.Linq;
namespace PS_project.Models.Exceptions
{
    class InvalidNotificationPushException : PS_Exception
    {
        private ErrorModel error;
        public InvalidNotificationPushException(string msg) : base()
        {
            this.error = new ErrorModel
            {
                type = "api/prob/push notifcation",
                title = "Error attempting to push notification",
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