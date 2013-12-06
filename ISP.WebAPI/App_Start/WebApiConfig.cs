using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ISP.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ProtectedApi",
                routeTemplate: "protected/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            // New code:
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling =
                Newtonsoft.Json.PreserveReferencesHandling.Objects;

            var xml = config.Formatters.XmlFormatter;
            xml.UseXmlSerializer = true;

            //config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
