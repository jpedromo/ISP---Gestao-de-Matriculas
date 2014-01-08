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
    public class ApoliceIsentoRepository : IApoliceIsentoRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<ApoliceIsento> All
        {
            get {
                return context.ApolicesIsentos;
            }
        }

        public IQueryable<ApoliceIsento> AllIncluding(params Expression<Func<ApoliceIsento, object>>[] includeProperties)
        {
            IQueryable<ApoliceIsento> query = context.ApolicesIsentos;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ApoliceIsento Find(int id)
        {
            return context.ApolicesIsentos.Find(id);
        }

        public void InsertOrUpdate(ApoliceIsento apolice)
        {
            if (apolice.apoliceIsentoId == default(int))
            {
                context.ApolicesIsentos.Add(apolice);
            }
            else
            {
                context.Entry(apolice).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var apolice = context.Apolices.Find(id);
            context.Apolices.Remove(apolice);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
