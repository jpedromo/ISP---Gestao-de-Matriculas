using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Configuration;
using ISP.GestaoMatriculas.Repositories.DbPopulate;
using ISP.GestaoMatriculas.Model;


namespace ISP.GestaoMatriculas
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterIoCContainer();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //DbExample1.PopulateDB(new DomainModels());
        }

        private void RegisterIoCContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .InstancePerHttpRequest()
                .PropertiesAutowired();

            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}