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
    public class FicheiroRepository : IFicheiroRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Ficheiro> All
        {
            get
            {
                return context.Ficheiros;
            }
        }

        public IQueryable<Ficheiro> AllIncluding(params Expression<Func<Ficheiro, object>>[] includeProperties)
        {
            IQueryable<Ficheiro> query = context.Ficheiros;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Ficheiro Find(int id)
        {
            return context.Ficheiros.Find(id);
        }

        public void InsertOrUpdate(Ficheiro ficheiro)
        {
            if (ficheiro.ficheiroId == default(int))
            {
                context.Ficheiros.Add(ficheiro);
            }
            else
            {
                context.Entry(ficheiro).State = EntityState.Modified;
            }
        }

        public Ficheiro Insert(Ficheiro ficheiro)
        {
            ficheiro.ficheiroId = default(int);
            context.Ficheiros.Add(ficheiro);
            return ficheiro;
        }

        public bool Update(Ficheiro ficheiro)
        {
            try
            {
                context.Entry(ficheiro).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Delete(int id)
        {
            var ficheiro = context.Ficheiros.Find(id);
            context.Ficheiros.Remove(ficheiro);
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
