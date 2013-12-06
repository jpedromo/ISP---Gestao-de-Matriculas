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
    public class CategoriaRepository : ICategoriaRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Categoria> All
        {
            get {
                return context.Categorias;
            }
        }

        public IQueryable<Categoria> AllIncluding(params Expression<Func<Categoria, object>>[] includeProperties)
        {
            IQueryable<Categoria> query = context.Categorias;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Categoria Find(int id)
        {
            return context.Categorias.Find(id);
        }

        public void InsertOrUpdate(Categoria categoria){
            if (categoria.categoriaId == default(int))
            {
                context.Categorias.Add(categoria);
            }
            else
            {
                context.Entry(categoria).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var categoria = context.Categorias.Find(id);
            context.Categorias.Remove(categoria);
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
