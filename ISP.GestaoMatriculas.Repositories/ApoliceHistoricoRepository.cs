using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Data.Entity;
using System.Web;

using ISP.GestaoMatriculas.Contracts;
using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Repositories
{
<<<<<<< HEAD
    public class ApoliceHistoricoRepository : IApoliceHistoricoRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<ApoliceHistorico> All
        {
            get {
                return context.ApolicesHistorico;
            }
        }

        public IQueryable<ApoliceHistorico> AllIncluding(params Expression<Func<ApoliceHistorico, object>>[] includeProperties)
        {
            IQueryable<ApoliceHistorico> query = context.ApolicesHistorico;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ApoliceHistorico Find(int id)
        {
            return context.ApolicesHistorico.Find(id);
        }

        public void InsertOrUpdate(ApoliceHistorico apolice){
            if (apolice.apoliceId == default(int))
            {
                context.ApolicesHistorico.Add(apolice);
            }
            else
            {
                context.Entry(apolice).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var apolice = context.ApolicesHistorico.Find(id);
            context.ApolicesHistorico.Remove(apolice);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

=======
    public class ApoliceHistoricoRepository : Respository<DomainModels, ApoliceHistorico>, IApoliceHistoricoRepository
    {
        public ApoliceHistoricoRepository():base()
        {
            DbSet = Context.ApolicesHistorico;
        }
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
    }
}
