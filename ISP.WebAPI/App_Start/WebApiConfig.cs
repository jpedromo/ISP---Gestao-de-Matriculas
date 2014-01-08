using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
<<<<<<< HEAD
using System.Xml.Serialization;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
<<<<<<< HEAD
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
=======
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
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }
    }
}
