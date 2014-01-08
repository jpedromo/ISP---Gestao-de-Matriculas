using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IEntidadeRepository : IDisposable
    {
        IQueryable<Entidade> All { get; }
        IQueryable<Entidade> AllIncluding(params Expression<Func<Entidade, object>>[] includeProperties);
        Entidade Find(int id);
        void InsertOrUpdate(Entidade entidade);
        void Delete(int id);
        void Save();
    }
}
