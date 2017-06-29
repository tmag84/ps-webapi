using PS_project.Utils;
using PS_project.Utils.DB;
using PS_project.Utils.Exceptions;
using PS_project.Models;
using Drum;
using WebApi.Hal;
using System.Linq;
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
        private const int DEFAULT_PAGESIZE = 5;

        [HttpPost, Route("register")]
        public HttpResponseMessage RegisterUser(UserRegistrationModel registration)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_UserActions.RegisterUser(registration);
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

        [HttpGet, Route("subscriptions")]
        [Authorize]
        public HttpResponseMessage GetUserSubscriptions(string email, int page)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                UserResponseModel user_hal = DB_ServiceUserActions.GetSubscribedServices(email);
                user_hal.user_email = email;
                user_hal.total_services = user_hal.services.Count;
                user_hal.Href = uriMaker.UriFor(c => c.GetUserSubscriptions(email,page)).AbsolutePath;

                var begin = (page - 1) * DEFAULT_PAGESIZE;
                var end = page * DEFAULT_PAGESIZE;

                user_hal.services = user_hal.services
                    .Skip((page - 1) * DEFAULT_PAGESIZE)
                    .Take(DEFAULT_PAGESIZE)
                    .ToList();

                if (page > 1)
                {
                    user_hal.Links.Add(new Link("prev", uriMaker.UriFor(c => c.GetUserSubscriptions(user_hal.user_email, page - 1)).AbsoluteUri));
                }                
                
                if (end < user_hal.total_services)
                {
                    user_hal.Links.Add(new Link("next", uriMaker.UriFor(c => c.GetUserSubscriptions(user_hal.user_email, page + 1)).AbsoluteUri));
                }

                HrefBuilders.BuildSubscriptionsHrefs(uriMaker, user_hal);
                resp = Request.CreateResponse<UserResponseModel>(HttpStatusCode.OK, user_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetUserSubscriptions(email,page)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("search-by-type")]
        [Authorize]
        public HttpResponseMessage SearchServicesByType(string email, int type, int page)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                List<int> list_types = new List<int>() { type };
                UserResponseModel user_info = DB_ServiceUserActions.GetSubscribedServices(email);

                UserResponseModel user_hal = new UserResponseModel();
                user_hal.user_email = email;
                user_hal.services = DB_ServiceUserActions.GetServicesByTypes(list_types);
                user_hal.total_services = user_hal.services.Count;

                var begin = (page - 1) * DEFAULT_PAGESIZE;
                var end = page * DEFAULT_PAGESIZE;

                user_hal.services = user_hal.services.
                    Skip((page - 1) * DEFAULT_PAGESIZE)
                    .Take(DEFAULT_PAGESIZE)
                    .ToList();

                if (page > 1)
                {
                    user_hal.Links.Add(new Link("prev", uriMaker.UriFor(c => c.SearchServicesByType(user_hal.user_email, type, page - 1)).AbsoluteUri));
                }

                if (end < user_hal.total_services)
                {
                    user_hal.Links.Add(new Link("next", uriMaker.UriFor(c => c.SearchServicesByType(user_hal.user_email, type, page + 1)).AbsoluteUri));
                }

                HrefBuilders.BuildSearchServicesHrefs(uriMaker, user_hal.services, user_info);
                resp = Request.CreateResponse<UserResponseModel>(HttpStatusCode.OK, user_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.SearchServicesByType(email,type,page)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("search-by-preferences")]
        [Authorize]
        public HttpResponseMessage SearchServicesByPreferences(string email, int page, [FromUri]int[] service_types)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                List<int> list_types = new List<int>(service_types);
                UserResponseModel user_info = DB_ServiceUserActions.GetSubscribedServices(email);

                UserResponseModel user_hal = new UserResponseModel();
                user_hal.services = DB_ServiceUserActions.GetServicesByTypes(list_types);
                user_hal.total_services = user_hal.services.Count;

                var begin = (page - 1) * DEFAULT_PAGESIZE;
                var end = page * DEFAULT_PAGESIZE;

                user_hal.services = user_hal.services
                    .Skip((page - 1) * DEFAULT_PAGESIZE)
                    .Take(DEFAULT_PAGESIZE)
                    .ToList();

                if (page > 1)
                {
                    user_hal.Links.Add(new Link("prev", uriMaker.UriFor(c => c.SearchServicesByPreferences(user_hal.user_email, page - 1, service_types)).AbsoluteUri));
                }

                if (end < user_hal.total_services)
                {
                    user_hal.Links.Add(new Link("next", uriMaker.UriFor(c => c.SearchServicesByPreferences(user_hal.user_email, page + 1, service_types)).AbsoluteUri));
                }

                HrefBuilders.BuildSearchServicesHrefs(uriMaker, user_hal.services, user_info);
                resp = Request.CreateResponse<UserResponseModel>(HttpStatusCode.OK, user_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.SearchServicesByPreferences(email,page,service_types)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("get-user-events")]
        [Authorize]
        public HttpResponseMessage GetUserEvents(string email, int page)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                UserResponseModel user_info = DB_ServiceUserActions.GetSubscribedServices(email);

                UserEventResponseModel user_hal = new UserEventResponseModel();
                user_hal.events = new List<UserEventModel>();

                foreach (var service in user_info.services)
                {
                    foreach(var ev in service.service_events)
                    {
                        UserEventModel model = new UserEventModel();
                        model.creation_date = ev.creation_date;
                        model.event_date = ev.event_date;
                        model.id = ev.id;
                        model.text = ev.text;
                        model.service_id = service.id;
                        model.service_name = service.name;
                        model.service_type = service.service_type;
                        user_hal.events.Add(model);
                    }
                }
                user_hal.total_events = user_hal.events.Count;

                user_hal.Href = uriMaker.UriFor(c => c.GetUserEvents(email, page)).AbsolutePath;

                var begin = (page - 1) * DEFAULT_PAGESIZE;
                var end = page * DEFAULT_PAGESIZE;

                user_hal.events = user_hal.events
                    .Skip((page - 1) * DEFAULT_PAGESIZE)
                    .Take(DEFAULT_PAGESIZE)
                    .ToList();

                if (page > 1)
                {
                    user_hal.Links.Add(new Link("prev", uriMaker.UriFor(c => c.GetUserEvents(email, page - 1)).AbsoluteUri));
                }

                if (end < user_hal.total_events)
                {
                    user_hal.Links.Add(new Link("next", uriMaker.UriFor(c => c.GetUserEvents(email, page + 1)).AbsoluteUri));
                }

                resp = Request.CreateResponse<UserEventResponseModel>(HttpStatusCode.OK, user_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetUserEvents(email, page)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("get-service")]
        [Authorize]
        public HttpResponseMessage GetService(int id)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                ProviderResponseModel ps_hal = DB_ServiceProviderActions.GetServiceWithServiceId(id);
                resp = Request.CreateResponse<ServiceModel>(HttpStatusCode.OK, ps_hal.service);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetService(id)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpPost, Route("create-rank")]
        [Authorize]
        public HttpResponseMessage CreateRanking(RankingModel rank)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_ServiceUserActions.CreateRanking(rank);
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
        [Authorize]
        public HttpResponseMessage AddSubscription(SubscriptionModel sub)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_ServiceUserActions.AddSubscription(sub.email,sub.id);
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
        [Authorize]
        public HttpResponseMessage RemoveSubscription(SubscriptionModel sub)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_ServiceUserActions.RemoveSubscription(sub.email,sub.id);
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

        [HttpPut, Route("edit-password")]
        [Authorize]
        public HttpResponseMessage EditUserPassword(UserModel user)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_UserActions.EditUserPassword(user);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.EditUserPassword(user)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }

        [HttpPut, Route("edit-user")]
        [Authorize]
        public HttpResponseMessage EditUserInformation(ServiceUserModel user)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                DB_ServiceUserActions.EditServiceUser(user);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.EditUserInformation(user)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }
    }
}
