using PS_project.Utils;
using PS_project.Utils.DB;
using PS_project.Models;
using PS_project.Utils.Exceptions;
using Drum;
using WebApi.Hal;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace PS_project.Controllers
{
    [RoutePrefix(Const_Strings.PROVIDER_ROUTE_PREFIX)]
    public class ProviderController : ApiController
    {
        [HttpPost, Route("register")]
        public HttpResponseMessage Register(ProviderRegistrationModel registration)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();

            try
            {
                DB_UserActions.RegisterProvider(registration);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.Register(registration)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }

        [HttpGet, Route("get-service")]
        [Authorize]
        public HttpResponseMessage GetService(string email)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                ProviderResponseModel ps_hal = DB_ServiceProviderActions.GetServiceWithProviderEmail(email);
                HrefBuilders.BuildServiceHrefs(uriMaker, ps_hal);
                resp = Request.CreateResponse<ProviderResponseModel>(HttpStatusCode.OK, ps_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetService(email)).AbsoluteUri;
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
                HrefBuilders.BuildServiceHrefs(uriMaker, ps_hal);
                resp = Request.CreateResponse<ProviderResponseModel>(HttpStatusCode.OK, ps_hal);
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
               
        [HttpPost, Route("create-notice")]
        [Authorize]
        public HttpResponseMessage CreateNotice(NoticeModel notice)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_ServiceProviderActions.CreateNotice(notice);
                return GetService(notice.id);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.CreateNotice(notice)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }

        [HttpPost, Route("delete-notice")]
        [Authorize]
        public HttpResponseMessage DeleteNotice(NoticeModel notice)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_ServiceProviderActions.DeleteNotice(notice);
                return GetService(notice.service_id);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.DeleteNotice(notice)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }

        [HttpPost, Route("create-event")]
        [Authorize]
        public HttpResponseMessage CreateEvent(EventModel ev)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_ServiceProviderActions.CreateEvent(ev);
                return GetService(ev.service_id);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.CreateEvent(ev)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }

        [HttpPost, Route("delete-event")]
        [Authorize]
        public HttpResponseMessage DeleteEvent(EventModel ev)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_ServiceProviderActions.DeleteEvent(ev);
                return GetService(ev.service_id);

            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.DeleteEvent(ev)).AbsoluteUri;
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

        [HttpPut, Route("edit-service")]
        [Authorize]
        public HttpResponseMessage EditService(ServiceModel service)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_ServiceProviderActions.EditServiceInfo(service);
                return GetService(service.id);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.EditService(service)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }
    }
}
