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
    public class ApoliceRepository : IApoliceRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Apolice> All
        {
            get {
                return context.Apolices;
            }
        }

        public IQueryable<Apolice> AllIncluding(params Expression<Func<Apolice, object>>[] includeProperties)
        {
            IQueryable<Apolice> query = context.Apolices;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Apolice Find(int id)
        {
            return context.Apolices.Find(id);
        }

        public void InsertOrUpdate(Apolice apolice){
            if (apolice.Id == default(int))
            {
                context.Apolices.Add(apolice);
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
