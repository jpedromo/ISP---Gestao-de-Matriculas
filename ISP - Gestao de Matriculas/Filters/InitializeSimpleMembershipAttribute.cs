using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using ISP.GestaoMatriculas.Model;
using ISP.GestaoMatriculas.Repositories.DbPopulate;
<<<<<<< HEAD
=======
using System.Linq;
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c

namespace ISP.GestaoMatriculas.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
                // Ensure ASP.NET Simple Membership is initialized only once per app start
                LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                //DbExample1.PopulateDB();
                
                Database.SetInitializer<DomainModels>(null);
                try
                {
                    using (var context = new DomainModels())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    if (!WebSecurity.Initialized)
                    {
<<<<<<< HEAD
                        WebSecurity.InitializeDatabaseConnection("DomainModels", "MAT_USER_PROFILE", "UserId_PK", "UserName", autoCreateTables: true);
                    }
                    DbExample1.PopulateDB(new DomainModels());
=======
                        WebSecurity.InitializeDatabaseConnection("ISPMatriculas", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    }
                    
                    if (!WebMatrix.WebData.WebSecurity.UserExists("admin1"))
                    {
                        string userName = WebMatrix.WebData.WebSecurity.CreateUserAndAccount("admin1", "administrador", new { email = "admin1@app.net", ativo = true, entidadeId = new DomainModels().Entidades.Single(e => e.Nome == "ISP").Id }, false);
                        System.Web.Security.Roles.AddUserToRole("admin1", "Admin");
                    }
                    
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
       
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
