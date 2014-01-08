using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using ISP.GestaoMatriculas.Model;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IApoliceHistoricoRepository : IDisposable
    {
        IQueryable<ApoliceHistorico> All { get; }
        IQueryable<ApoliceHistorico> AllIncluding(params Expression<Func<ApoliceHistorico, object>>[] includeProperties);
        ApoliceHistorico Find(int id);
        void InsertOrUpdate(ApoliceHistorico apolice);
        void Delete(int id);
        void Save();
    }
}
