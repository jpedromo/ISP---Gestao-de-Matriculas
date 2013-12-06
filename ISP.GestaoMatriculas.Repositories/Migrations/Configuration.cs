namespace ISP.GestaoMatriculas.Repositories.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ISP.GestaoMatriculas.Repositories.DbPopulate;
    using ISP.GestaoMatriculas.Model;
    using System.Collections.Generic;
    using WebMatrix.WebData;
    using System.Data.Entity.Infrastructure;
    using System.Threading;

    internal sealed class Configuration : DbMigrationsConfiguration<ISP.GestaoMatriculas.Model.DomainModels>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ISP.GestaoMatriculas.Model.DomainModels context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //DbExample1.PopulateDB(context);
        }

    }
}
