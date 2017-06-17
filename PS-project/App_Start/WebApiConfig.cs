﻿using Drum;
using PS_project.Utils;
using System.Web.Http;
using WebApi.Hal;
using Newtonsoft.Json;

namespace PS_project
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutesAndUseUriMaker();

            config.Routes.MapHttpRoute(
                "users",
                Const_Strings.USER_ROUTE_PREFIX
                );

            config.Routes.MapHttpRoute(
                "providers",
                Const_Strings.PROVIDER_ROUTE_PREFIX
                );

            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;

            //config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            /*
            var jsonhal = new JsonHalMediaTypeFormatter();
            jsonhal.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonhal.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            jsonhal.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.Add(jsonhal);*/
        }
    }
}
