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
    public class AvisoRepository : IAvisoRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Aviso> All
        {
            get {
                return context.Avisos;
            }
        }

        public IQueryable<Aviso> AllIncluding(params Expression<Func<Aviso, object>>[] includeProperties)
        {
            IQueryable<Aviso> query = context.Avisos;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Aviso Find(int id)
        {
            return context.Avisos.Find(id);
        }

        public void InsertOrUpdate(Aviso aviso)
        {
            if (aviso.avisoId == default(int))
            {
                context.Avisos.Add(aviso);
            }
            else
            {
                context.Entry(aviso).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var aviso = context.Avisos.Find(id);
            context.Avisos.Remove(aviso);
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
