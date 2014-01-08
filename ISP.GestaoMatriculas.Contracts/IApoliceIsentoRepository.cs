using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IApoliceIsentoRepository : IDisposable
    {
        IQueryable<ApoliceIsento> All { get; }
        IQueryable<ApoliceIsento> AllIncluding(params Expression<Func<ApoliceIsento, object>>[] includeProperties);
        ApoliceIsento Find(int id);
        void InsertOrUpdate(ApoliceIsento apolice);
        void Delete(int id);
        void Save();
    }
}
