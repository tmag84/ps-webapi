using PS_project.Utils;
using PS_project.Utils.DB;
using PS_project.Utils.Exceptions;
using PS_project.Models;
using Drum;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Collections.Generic;

namespace PS_project.Controllers
{
    [RoutePrefix(Const_Strings.USER_ROUTE_PREFIX)]
    public class UserController : ApiController
    {
        [HttpGet, Route("login")]
        public HttpResponseMessage LoginUser(UsersModel user)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_User.UserLogin(user);
                UserResponseModel user_hal = DB_User.GetSubscribedServices(user.email);
                user_hal.user_email = user.email;
                user_hal.Href = uriMaker.UriFor(c => c.GetUserSubscriptions(user.email)).AbsolutePath;
                HrefBuilders.BuildSubscriptionsHrefs(uriMaker, user_hal);
                resp = Request.CreateResponse<UserResponseModel>(HttpStatusCode.OK, user_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.LoginUser(user)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("subscriptions")]
        public HttpResponseMessage GetUserSubscriptions(string user_email)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                UserResponseModel user_hal = DB_User.GetSubscribedServices(user_email);
                user_hal.user_email = user_email;
                user_hal.Href = uriMaker.UriFor(c => c.GetUserSubscriptions(user_email)).AbsolutePath;
                HrefBuilders.BuildSubscriptionsHrefs(uriMaker, user_hal);
                resp = Request.CreateResponse<UserResponseModel>(HttpStatusCode.OK, user_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetUserSubscriptions(user_email)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("search-by-type")]
        public HttpResponseMessage SearchServicesByType(SearchModel search)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                List<int> list_types = new List<int>() { search.service_type };
                UserResponseModel user_info = DB_User.GetSubscribedServices(search.user_email);
                List<ServiceModel> list_services = DB_User.GetServicesByTypes(list_types);
                HrefBuilders.BuildSearchServicesHrefs(uriMaker, list_services, user_info);
                resp = Request.CreateResponse<List<ServiceModel>>(HttpStatusCode.OK, list_services);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.SearchServicesByType(search)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("search-by-preferences")]
        public HttpResponseMessage SearchServicesByPreferences(SearchModel search)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {                
                List<ServiceModel> list_services = DB_User.GetServicesByTypes(search.list_types);
                UserResponseModel user_info = DB_User.GetSubscribedServices(search.user_email);
                HrefBuilders.BuildSearchServicesHrefs(uriMaker, list_services, user_info);
                resp = Request.CreateResponse<List<ServiceModel>>(HttpStatusCode.OK, list_services);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.SearchServicesByPreferences(search)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpPost, Route("register")]
        public HttpResponseMessage RegisterUser(UserRegistrationModel registration)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_User.RegisterUser(registration);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.RegisterUser(registration)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpPost, Route("create-rank")]
        public HttpResponseMessage CreateRanking(RankingModel rank)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_User.CreateRanking(rank);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.CreateRanking(rank)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpPost, Route("add-subscription")]
        public HttpResponseMessage AddSubscription(SubscriptionModel sub)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_User.AddSubscription(sub);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.AddSubscription(sub)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpPost, Route("remove-subscription")]
        public HttpResponseMessage RemoveSubscription(SubscriptionModel sub)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_User.RemoveSubscription(sub);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.RemoveSubscription(sub)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpPut, Route("edit-user")]
        public HttpResponseMessage EditUser(UsersModel user)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_User.EditUser(user);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.EditUser(user)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;

        }
    }
}
