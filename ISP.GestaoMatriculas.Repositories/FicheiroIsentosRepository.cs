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
    public class FicheiroIsentosRepository : IFicheiroIsentosRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<FicheiroIsentos> All
        {
            get
            {
                return context.FicheirosIsentos;
            }
        }

        public IQueryable<FicheiroIsentos> AllIncluding(params Expression<Func<FicheiroIsentos, object>>[] includeProperties)
        {
            IQueryable<FicheiroIsentos> query = context.FicheirosIsentos;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public FicheiroIsentos Find(int id)
        {
            return context.FicheirosIsentos.Find(id);
        }

        public void InsertOrUpdate(FicheiroIsentos ficheiro)
        {
            if (ficheiro.ficheiroIsentosId == default(int))
            {
                context.FicheirosIsentos.Add(ficheiro);
            }
            else
            {
                context.Entry(ficheiro).State = EntityState.Modified;
            }
        }

        public FicheiroIsentos Insert(FicheiroIsentos ficheiro)
        {
            ficheiro.ficheiroIsentosId = default(int);
            context.FicheirosIsentos.Add(ficheiro);
            return ficheiro;
        }

        public bool Update(FicheiroIsentos ficheiro)
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
            var ficheiro = context.FicheirosIsentos.Find(id);
            context.FicheirosIsentos.Remove(ficheiro);
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
