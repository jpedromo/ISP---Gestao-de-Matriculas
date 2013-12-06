using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IPessoaRepository : IDisposable
    {
        IQueryable<Pessoa> All { get; }
        IQueryable<Pessoa> AllIncluding(params Expression<Func<Pessoa, object>>[] includeProperties);
        Pessoa Find(int id);
        void InsertOrUpdate(Pessoa pessoa);
        void Delete(int id);
        void Save();
    }
}
