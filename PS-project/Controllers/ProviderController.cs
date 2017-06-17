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
        [HttpGet, Route("")]
        public HttpResponseMessage GetProviderOptions()
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                ProviderResponseModel ps_hal = new ProviderResponseModel();
                ps_hal.list_service_types = DB_Provider.GetServiceTypes();
                ps_hal.Href = uriMaker.UriFor(c => c.GetProviderOptions()).AbsolutePath;
                ps_hal.Links.Add(new Link("login_provider", uriMaker.UriFor(c => c.Login(null)).AbsolutePath));
                ps_hal.Links.Add(new Link("register_provider", uriMaker.UriFor(c => c.Register(null)).AbsolutePath));
                resp = Request.CreateResponse<ProviderResponseModel>(HttpStatusCode.OK, ps_hal);
            }
            catch(PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.GetProviderOptions()).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }            
            return resp;
        }

        [HttpGet, Route("login")]
        public HttpResponseMessage Login(ProviderModel provider)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_Provider.ProviderLogin(provider);
                ProviderResponseModel ps_hal = DB_Provider.GetServiceWithProviderEmail(provider.email);
                ps_hal.Href = uriMaker.UriFor(c => c.Login(provider)).AbsolutePath;
                HrefBuilders.BuildServiceHrefs(uriMaker, ps_hal);
                resp = Request.CreateResponse<ProviderResponseModel>(HttpStatusCode.OK, ps_hal);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.Login(provider)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }

            return resp;
        }        

        [HttpGet, Route("get-service")]
        public HttpResponseMessage GetService(int id)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                ProviderResponseModel ps_hal = DB_Provider.GetServiceWithServiceId(id);
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

        [HttpPost, Route("register")]
        public HttpResponseMessage Register(ProviderRegistrationModel registration)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();

            try
            {
                DB_Provider.RegisterProvider(registration);
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

        [HttpPost, Route("create-notice")]
        public HttpResponseMessage CreateNotice(NoticeModel notice)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_Provider.CreateNotice(notice);
                return GetService(notice.service_id);
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
        public HttpResponseMessage DeleteNotice(NoticeModel notice)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_Provider.DeleteNotice(notice);
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
        public HttpResponseMessage CreateEvent(EventModel ev)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_Provider.CreateEvent(ev);
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
        public HttpResponseMessage DeleteEvent(EventModel ev)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_Provider.DeleteEvent(ev);
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

        [HttpPut, Route("edit-provider")]
        public HttpResponseMessage EditProvider(ProviderModel provider)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_Provider.EditProviderPassword(provider);
                resp = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (PS_Exception e)
            {
                ErrorModel error = e.GetError();
                error.instance = uriMaker.UriFor(c => c.EditProvider(provider)).AbsoluteUri;
                resp = Request.CreateResponse<ErrorModel>(
                    error.status, error,
                    new JsonMediaTypeFormatter(),
                    new MediaTypeHeaderValue("application/problem+json"));
            }
            return resp;
        }

        [HttpPut, Route("edit-service")]
        public HttpResponseMessage EditService(ServiceModel service)
        {
            HttpResponseMessage resp;
            var uriMaker = Request.TryGetUriMakerFor<ProviderController>();
            try
            {
                DB_Provider.EditServiceInfo(service);
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
