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
    public class EntidadeRepository : IEntidadeRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Entidade> All
        {
            get {
                return context.Entidades;
            }
        }

        public IQueryable<Entidade> AllIncluding(params Expression<Func<Entidade, object>>[] includeProperties)
        {
            IQueryable<Entidade> query = context.Entidades;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Entidade Find(int id)
        {
            return context.Entidades.Find(id);
        }

        public void InsertOrUpdate(Entidade entidade){
<<<<<<< HEAD
            if (entidade.entidadeId == default(int))
=======
            if (entidade.Id == default(int))
>>>>>>> 6bef4ea7199f182f1dcc5a1156a157494ff9f29c
            {
                context.Entidades.Add(entidade);
            }
            else
            {
                context.Entry(entidade).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var entidade = context.Entidades.Find(id);
            context.Entidades.Remove(entidade);
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
