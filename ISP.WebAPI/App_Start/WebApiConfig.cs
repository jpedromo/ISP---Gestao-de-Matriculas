using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Xml.Serialization;

namespace ISP.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/Consulta",
                defaults: new { controller = "ConsultaPublica", action = "PublicAction" }
            );
            
            config.Routes.MapHttpRoute(
                name: "ProtectedApi",
                routeTemplate: "protected/{controller}",
                defaults: new { action = "ProtectedAction" }
            );

            config.Routes.MapHttpRoute(
                name: "AuthApi",
                routeTemplate: "auth/{controller}",
                defaults: new { action = "AuthAction" }
            );

            // New code:
            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling =
            //    Newtonsoft.Json.PreserveReferencesHandling.Objects;

            //var xml = config.Formatters.XmlFormatter;
            //xml.UseXmlSerializer = true;

            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/json");
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
