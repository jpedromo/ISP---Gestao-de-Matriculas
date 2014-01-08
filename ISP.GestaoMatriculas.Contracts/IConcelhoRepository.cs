using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISP.GestaoMatriculas.Model;
using System.Linq.Expressions;

namespace ISP.GestaoMatriculas.Contracts
{
    public interface IConcelhoRepository : IDisposable
    {
        IQueryable<Concelho> All { get; }
        IQueryable<Concelho> AllIncluding(params Expression<Func<Concelho, object>>[] includeProperties);
        Concelho Find(int id);
        void InsertOrUpdate(Concelho concelho);
        void Delete(int id);
        void Save();
    }
}
