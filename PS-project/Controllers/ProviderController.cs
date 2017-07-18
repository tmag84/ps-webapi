using PS_project.Utils;
using PS_project.Utils.DB;
using PS_project.Models;
using PS_project.Models.ResponseModels;
using PS_project.Models.Exceptions;
using Drum;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Security.Claims;

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
        public HttpResponseMessage GetService()
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                string email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                ProviderResponseModel ps_hal = DB_ServiceProviderActions.GetServiceWithProviderEmail(email);
                HrefBuilders.BuildServiceHrefs(uriMaker, ps_hal);
                resp = Request.CreateResponse<ProviderResponseModel>(HttpStatusCode.OK, ps_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetService()).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }

        [HttpGet, Route("get-rankings")]
        [Authorize]
        public HttpResponseMessage GetServiceRankings()
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                string email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                ProviderRankingsResponseModel provider_hal = new ProviderRankingsResponseModel();
                provider_hal.service_rankings = DB_ServiceProviderActions.GetServiceWithProviderEmail(email).service.service_rankings;

                resp = Request.CreateResponse<ProviderRankingsResponseModel> (HttpStatusCode.OK, provider_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetServiceRankings()).AbsoluteUri;
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
                string email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                var _id = DB_ServiceProviderActions.CreateNotice(email, notice);
                resp = Request.CreateResponse<object>(HttpStatusCode.OK,new {id= _id });
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
                string email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                DB_ServiceProviderActions.DeleteNotice(email, notice);
                resp = Request.CreateResponse(HttpStatusCode.OK);
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
                string email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                var _id = DB_ServiceProviderActions.CreateEvent(email, ev);
                resp = Request.CreateResponse<object>(HttpStatusCode.OK,new {id=_id});
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
                string email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                DB_ServiceProviderActions.DeleteEvent(email, ev);
                resp = Request.CreateResponse(HttpStatusCode.OK);
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
        public HttpResponseMessage EditUserPassword(string new_password)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<UserController>();
            try
            {
                string email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                DB_UserActions.EditUserPassword(email, new_password);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.EditUserPassword(new_password)).AbsoluteUri;
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
                service.provider_email = ClaimsHandler.GetUserNameFromClaim(Request.GetRequestContext().Principal as ClaimsPrincipal);

                DB_ServiceProviderActions.EditServiceInfo(service);
                resp = Request.CreateResponse(HttpStatusCode.OK);
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
