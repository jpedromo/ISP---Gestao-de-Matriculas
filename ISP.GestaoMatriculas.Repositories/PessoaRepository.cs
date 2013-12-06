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
    public class PessoaRepository : IPessoaRepository
    {
        DomainModels context = new DomainModels();

        public IQueryable<Pessoa> All
        {
            get {
                return context.Segurados;
            }
        }

        public IQueryable<Pessoa> AllIncluding(params Expression<Func<Pessoa, object>>[] includeProperties)
        {
            IQueryable<Pessoa> query = context.Segurados;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Pessoa Find(int id)
        {
            return context.Segurados.Find(id);
        }

        public void InsertOrUpdate(Pessoa pessoa){
            if (pessoa.pessoaId == default(int))
            {
                context.Segurados.Add(pessoa);
            }
            else
            {
                context.Entry(pessoa).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var pessoa = context.Segurados.Find(id);
            context.Segurados.Remove(pessoa);
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
