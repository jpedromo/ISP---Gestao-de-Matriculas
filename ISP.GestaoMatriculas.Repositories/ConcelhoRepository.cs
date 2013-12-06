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
    public class ConcelhoRepository : IConcelhoRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Concelho> All
        {
            get {
                return context.Concelhos;
            }
        }

        public IQueryable<Concelho> AllIncluding(params Expression<Func<Concelho, object>>[] includeProperties)
        {
            IQueryable<Concelho> query = context.Concelhos;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Concelho Find(int id)
        {
            return context.Concelhos.Find(id);
        }

        public void InsertOrUpdate(Concelho concelho){
            if (concelho.Id == default(int))
            {
                context.Concelhos.Add(concelho);
            }
            else
            {
                context.Entry(concelho).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var concelho = context.Concelhos.Find(id);
            context.Concelhos.Remove(concelho);
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
