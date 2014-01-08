using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISP.WebAPI.Modules;
<<<<<<< HEAD
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.WebApi;
using System.Reflection;
using WebMatrix.WebData;
=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.WebAPI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

<<<<<<< HEAD
            RegisterIoCContainer();

=======
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

<<<<<<< HEAD
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DomainModels", "MAT_USER_PROFILE", "UserId_PK", "UserName", autoCreateTables: true);
            }

        }

        private void RegisterIoCContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
=======

>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
        }
    }
}